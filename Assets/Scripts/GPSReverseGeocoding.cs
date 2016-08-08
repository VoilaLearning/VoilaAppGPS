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
        
		Debug.Log("Getting address.");
		//string url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=43.65184,-79.36607";
		string url = "https://maps.googleapis.com/maps/api/geocode/json?";

		// Latitude and longitude
		if (SystemInfo.deviceType == DeviceType.Handheld) {
			
			url += "latlng=";
			url += Input.location.lastData.latitude.ToString () + ",";
			url += Input.location.lastData.longitude.ToString ();
		}
		else {

			//url += "latlng=43.65184,-79.36607"; // Work
			url += "latlng=43.7334,-79.43484"; // Home
		}

		// Optional Parameters
		url += "&result_type=point_of_interest|park|neighborhood|natural_feature";
		//url += "&location_type=ROOFTOP";

		// API Key
		//url += "&key=AIzaSyBKTgEzJxSOP5ukHmDUSdqIIXLKBwpdiaE"; // Google Maps iOS Key
		url += "&key=AIzaSyBSc8OXNrLzOXmhTpJ4XIPCUFia6KxPvqc"; // Google Maps Geocoding Key
        
		// WWWForm
		WWWForm form = new WWWForm();
		form.AddField ("address_components", "long_name");


		WWW www = new WWW(System.Uri.EscapeUriString(url), form);
        yield return www;
        outputText.text = www.text;

		// Parsing
		AddressCollection addresses = JsonUtility.FromJson<AddressCollection>(www.text);
		Debug.Log ("length: " + addresses.collection.Length);
	}
}

// http://forum.unity3d.com/threads/how-parse-json-data-in-unity-in-c.383804/
// http://forum.unity3d.com/threads/how-to-load-an-array-with-jsonutility.375735/
// http://forum.unity3d.com/threads/jsonutility-unexpected-node-type.409106/
// http://stackoverflow.com/questions/16937879/grabbing-country-from-google-geocode-jquery/16942609#16942609
// http://stackoverflow.com/questions/16747494/i-want-to-parse-google-map-api-for-reverse-geocode-in-android

[System.Serializable]
public class AddressComponent {

	[System.Serializable]
	public struct AddressEntry {
		public string long_name;
		public string short_name;
		public string[] types;
	}

	public AddressEntry[] addresses;
}

[System.Serializable]
public class AddressCollection {

	public AddressComponent[] collection;
}