using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[DisallowMultipleComponent]
public class GPSReverseGeocoding : MonoBehaviour {

    [SerializeField] Text outputText;

	
	void OnEnable () {
	
        StartCoroutine(GetAddresses());
	}
	
    IEnumerator GetAddresses () {
        Debug.Log("Getting address.");
        string url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=43.65184,-79.36607&result_type=point_of_interest";
        url += "&key=AIzaSyBKTgEzJxSOP5ukHmDUSdqIIXLKBwpdiaE";
        //string url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=43.65184,-79.36607";
        WWW www = new WWW(url);
        yield return www;

        outputText.text = www.text;
    }
}
