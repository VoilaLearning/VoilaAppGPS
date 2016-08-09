using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[DisallowMultipleComponent]
public class GPSReverseGeocoding : MonoBehaviour {

    [SerializeField] Text outputText;

	
	void OnEnable () {
		
        StartCoroutine(GetAddresses());
	}
	
    IEnumerator GetAddresses () {
        
		//Debug.Log("Reverse geocoding brah.");
		string url = "https://maps.googleapis.com/maps/api/geocode/json?";

		// Latitude and longitude
		if (SystemInfo.deviceType == DeviceType.Handheld) {
			
			url += "latlng=";
			url += Input.location.lastData.latitude.ToString () + ",";
			url += Input.location.lastData.longitude.ToString ();
		}
		else {

			url += "latlng=43.65184,-79.36607"; // Work
			//url += "latlng=43.7334,-79.43484"; // Home
		}

		// Optional Parameters
        url += "&region=ca";
		url += "&result_type=point_of_interest|park|neighborhood|natural_feature";
		//url += "&location_type=ROOFTOP";

		// API Key
		//url += "&key=AIzaSyBKTgEzJxSOP5ukHmDUSdqIIXLKBwpdiaE"; // Google Maps iOS Key
		url += "&key=AIzaSyBSc8OXNrLzOXmhTpJ4XIPCUFia6KxPvqc"; // Google Maps Geocoding Key

		WWW www = new WWW(System.Uri.EscapeUriString(url));
        yield return www;
        outputText.text = www.text;

		// Parsing
		//AddressCollection addresses = JsonUtility.FromJson<AddressCollection>(www.text);
        GeocodingResult[] addresses = JsonHelper.FromJson<GeocodingResult>(www.text);

        for(int i = 0; i < addresses.Length; i++) {

            //Debug.Log(i + ": " + addresses[i].address_components[0].long_name);
            Debug.Log(i + ": " + addresses[i].address_components[0].long_name + " - " + addresses[i].geometry.location.ToString());
            //Debug.Log(i + ": " + addresses[i].formatted_address);
        }
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