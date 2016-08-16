﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class GPSReverseGeocoding : MonoBehaviour {
    	
    const float EARTH_RADIUS = 6371000; // meters
    const float METERS_TO_UNITY_UNITS = 0.0133979f;

    GeocodingResult[] addresses;
    PlacesResult[] places;
    GameObject[] pictureBoxArray;
    GoogleMapTextureLoader mapTexture;

    float lastLatitude;
    float lastLongitude;

    [SerializeField] GameObject pictureBoxPrefab;
    [SerializeField] GameObject photoAlbum;


    void Awake () {

        pictureBoxArray = new GameObject[0];
        mapTexture = this.GetComponent<GoogleMapTextureLoader>();
    }

    public void RequestInfo () {

        StopCoroutine(GooglePlacesRequest());
        StartCoroutine(GooglePlacesRequest());
    }

    IEnumerator ReverseGeocodingRequest () {

		string url = "https://maps.googleapis.com/maps/api/geocode/json?";

        // Get position
        Vector2 position = mapTexture.GetPosition();
        url += "latlng=" + position.x.ToString() + "," + position.y.ToString();

		// Optional Parameters
        url += "&region=ca";
		url += "&result_type=point_of_interest|park|neighborhood|natural_feature";
        url += "&language=fr";

		// API Key
		url += "&key=AIzaSyBSc8OXNrLzOXmhTpJ4XIPCUFia6KxPvqc"; // Google Maps Geocoding Key

        // Send request
		WWW www = new WWW(System.Uri.EscapeUriString(url));
        yield return www;

		// Parse result
        addresses = JsonHelper.FromJson<GeocodingResult>(www.text);

        RemoveOldPictureBoxes();
        //RemoveCloseLocations();
        SetGeocodingMarkers();
	}

    IEnumerator GooglePlacesRequest () {

        // Check if position has changed
        Vector2 position = mapTexture.GetPosition();

        if (position.x != lastLatitude && position.y != lastLongitude) {

            string url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?";

            // Get position
            url += "location=" + position.x.ToString() + "," + position.y.ToString();

            // Optional Parameters
            url += "&radius=500";
            url += "&language=fr";
            url += "&type=airport|amusement_park|aquarium|art_gallery|bakery|book_store|bowling_alley";
            url += "|cafe|campground|casino|cemetery|church|city_hall|courthouse|department_store|embassy|fire_station";
            url += "|gym|hindu_temple|hospital|library|mosque|movie_theater|museum|park|police|post_office";
            url += "|rv_park|school|shopping_mall|stadium|subway_station|synagogue|train_station|university|zoo";

            // API Key
            url += "&key=AIzaSyDtAGznI_CkixZSR2j6yHrfdrr46vSpHIM";

            // Send request
            WWW www = new WWW(System.Uri.EscapeUriString(url));
            yield return www;

            // Parsing
            Debug.Log(www.text);
            places = JsonHelper.FromJson<PlacesResult>(www.text);

            for (int i = 0; i < places.Length; i++) {

                Debug.Log(i + ": " + places[i].name);
            }

            // Update positions
            lastLatitude = position.x;
            lastLongitude = position.y;

            RemoveOldPictureBoxes();
            SetPictureBoxes();
        }
    }

    void RemoveOldPictureBoxes () {

        if(pictureBoxArray.Length > 0) {

            for(int i = 0; i < pictureBoxArray.Length; i++) {

                Destroy(pictureBoxArray[i].gameObject);
            }

            pictureBoxArray = new GameObject[0];
        }
    }

    void RemoveCloseLocations () {

        float minAllowableDistance = 50f; // meters
        float maxAllowableDistance = 1000f; // meters
        List<GeocodingResult> addressesList = addresses.ToList();

        for(int i = 0; i < addresses.Length; i++) {

            for(int j = addresses.Length - 1; j > i; j--) {

                Location location1 = addresses[i].geometry.location;
                Location location2 = addresses[j].geometry.location;
                float distance = HaversineDistance(location1.lat, location1.lng, location2.lat, location2.lng);

                if(distance < minAllowableDistance || distance > maxAllowableDistance) {

                    addressesList.RemoveAt(j);
                    addresses = addressesList.ToArray();
                }
            }
        }
    }

    float HaversineDistance (float latitude1, float longitude1, float latitude2, float longitude2) {

        float deltaLatitude = (latitude2 - latitude1) * Mathf.Deg2Rad;
        float deltaLongitude = (longitude2 - longitude1) * Mathf.Deg2Rad;

        float distance = 2 * EARTH_RADIUS * Mathf.Asin(Mathf.Sqrt(Mathf.Sin(deltaLatitude * 0.5f) * Mathf.Sin(deltaLatitude * 0.5f)
            + Mathf.Cos(latitude1 * Mathf.Deg2Rad) * Mathf.Cos(latitude2 * Mathf.Deg2Rad) * Mathf.Sin(deltaLongitude * 0.5f) * Mathf.Sin(deltaLongitude * 0.5f)));

        return distance;
    }

    void SetGeocodingMarkers () {

        // Set markers on the Google map texture
        Location[] locations = new Location[addresses.Length];
        for(int i = 0; i < addresses.Length; i++) { locations[i] = addresses[i].geometry.location; }
        mapTexture.SetMarkers(locations);

        List<GameObject> markerList = new List<GameObject>();

        // Set gameobject markers
        for(int i = 0; i < addresses.Length; i++) {

            Vector2 position = mapTexture.GetPosition();
            float lat1 = Mathf.Deg2Rad * position.x;
            float lon1 = Mathf.Deg2Rad * position.y;

            float lat2 = Mathf.Deg2Rad * addresses[i].geometry.location.lat;
            float lon2 = Mathf.Deg2Rad * addresses[i].geometry.location.lng;

            float deltaLon = lon2 - lon1;

            float x = Mathf.Cos(lat1) * Mathf.Sin(lat2) - Mathf.Sin(lat1) * Mathf.Cos(lat2) * Mathf.Cos(deltaLon);
            float y = Mathf.Sin(deltaLon) * Mathf.Cos(lat2);

            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            float distance = HaversineDistance(lat1 * Mathf.Rad2Deg, lon1 * Mathf.Rad2Deg, lat2 * Mathf.Rad2Deg, lon2 * Mathf.Rad2Deg);

            float objX = distance * Mathf.Sin(angle * Mathf.Deg2Rad) * METERS_TO_UNITY_UNITS;
            float objY = distance * Mathf.Cos(angle * Mathf.Deg2Rad) * METERS_TO_UNITY_UNITS + this.transform.position.y;
            float objZ = -1;

            GameObject newPictureBox = Instantiate(pictureBoxPrefab, new Vector3(objX, objY, objZ), Quaternion.identity) as GameObject;
            //Debug.Log(addresses[i].address_components[0].short_name + ", angle: " + angle + ", distance: " + distance);

            PictureBoxController pictureBoxController = newPictureBox.GetComponent<PictureBoxController>();
            pictureBoxController.SetPhotoAlbum(photoAlbum);
            pictureBoxController.SetLocationName(addresses[i].address_components[0].short_name);

            markerList.Add(newPictureBox);
        }

        pictureBoxArray = markerList.ToArray();
    }

    void SetPictureBoxes () {

        // Set markers on the Google map texture
        Location[] locations = new Location[places.Length];
        for(int i = 0; i < places.Length; i++) { locations[i] = places[i].geometry.location; }
        mapTexture.SetMarkers(locations);

        List<GameObject> markerList = new List<GameObject>();

        // Set gameobject markers
        for(int i = 0; i < places.Length; i++) {

            Vector2 position = mapTexture.GetPosition();
            float lat1 = Mathf.Deg2Rad * position.x;
            float lon1 = Mathf.Deg2Rad * position.y;

            float lat2 = Mathf.Deg2Rad * places[i].geometry.location.lat;
            float lon2 = Mathf.Deg2Rad * places[i].geometry.location.lng;

            float deltaLon = lon2 - lon1;

            float x = Mathf.Cos(lat1) * Mathf.Sin(lat2) - Mathf.Sin(lat1) * Mathf.Cos(lat2) * Mathf.Cos(deltaLon);
            float y = Mathf.Sin(deltaLon) * Mathf.Cos(lat2);

            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            float distance = HaversineDistance(lat1 * Mathf.Rad2Deg, lon1 * Mathf.Rad2Deg, lat2 * Mathf.Rad2Deg, lon2 * Mathf.Rad2Deg);

            float objX = distance * Mathf.Sin(angle * Mathf.Deg2Rad) * METERS_TO_UNITY_UNITS;
            float objY = 1;
            float objZ = distance * Mathf.Cos(angle * Mathf.Deg2Rad) * METERS_TO_UNITY_UNITS + this.transform.position.y;

            GameObject newPictureBox = Instantiate(pictureBoxPrefab, new Vector3(objX, objY, objZ), Quaternion.Euler(90, 0, 0)) as GameObject;
            //Debug.Log(addresses[i].address_components[0].short_name + ", angle: " + angle + ", distance: " + distance);

            PictureBoxController pictureBoxController = newPictureBox.GetComponent<PictureBoxController>();
            pictureBoxController.SetPhotoAlbum(photoAlbum);
            pictureBoxController.SetLocationName(places[i].name);

            markerList.Add(newPictureBox);
        }

        pictureBoxArray = markerList.ToArray();
    }
}

[System.Serializable]
public class GeocodingResult {
            
    public AddressComponent[] address_components;
    public string formatted_address;
    public Geometry geometry;
    public string place_id;
    public string[] types;
}

[System.Serializable]
public class AddressComponent {

    public string long_name;
    public string short_name;
    public string[] types;
}

[System.Serializable]
public class Geometry {

    public Location location;
    public string location_type;
}

[System.Serializable]
public class Location {

    public float lat;
    public float lng;

    public override string ToString() {
        
        return lat + ", " + lng;
    }
}

[System.Serializable]
public class PlacesResult {

    public Geometry geometry;
    public string icon;
    public string id;
    public string name;
    public bool opening_hours;
    public Photo[] photos;
    public string place_id;
    public string scope;
    public string alt_ids;
    public string reference;
    public string[] types;
    public string vicinity;
}

[System.Serializable]
public class Photo {

    public int height;
    public string[] html_attributions;
    public string photo_reference;
    public int width;
}