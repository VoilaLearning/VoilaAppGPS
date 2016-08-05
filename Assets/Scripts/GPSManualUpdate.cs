using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GPSManualUpdate : GPSLocationServices {

    const float EARTH_RADIUS = 6371000; // meters

    [SerializeField] Text lastLatitudeText;
    [SerializeField] Text lastLongitudeText;
    [SerializeField] Text newLatitudeText;
    [SerializeField] Text newLongitudeText;
    [SerializeField] Text haversineText;

    float lastLatitude;
    float lastLongitude;

    void Awake () {

        StopCoroutine(InitializeLocationServices());
        StartCoroutine(InitializeLocationServices());
    }

    IEnumerator InitializeLocationServices () {

        #if UNITY_IOS
        if(Input.location.isEnabledByUser) {

            Input.location.Start();

            while(Input.location.status == LocationServiceStatus.Initializing) {

                yield return null;
            }

            lastLatitude = Input.location.lastData.latitude;
            lastLongitude = Input.location.lastData.longitude;

            lastLatitudeText.text = "Last Latitude: " + lastLatitude;
            lastLongitudeText.text = "Last Longitude: " + lastLongitude;
        }
        #endif

        yield return null;
    }

    public void RecalculatePosition () {

        #if UNITY_IOS
        if(Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running) {

            lastLatitudeText.text = "Last Latitude: " + lastLatitude;
            lastLongitudeText.text = "Last Longitude: " + lastLongitude;

            newLatitudeText.text = "New Latitude: " + Input.location.lastData.latitude;
            newLongitudeText.text = "New Longitude: " + Input.location.lastData.longitude;

            haversineText.text = "Haversine: " + HaversineDistance(ref lastLatitude, ref lastLongitude);
        }
        #endif
    }
}
