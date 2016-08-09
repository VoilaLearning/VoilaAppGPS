using UnityEngine;
using UnityEngine.UI;

public class GPSAutoUpdate : GPSLocationServices {
    
    [SerializeField] Text latitudeText;
    [SerializeField] Text longitudeText;
    [SerializeField] Text distanceText;

    float distance = 0;	
    float lastLatitude;
    float lastLongitude;

	void Awake () {
	
        #if UNITY_IOS
        if(Input.location.isEnabledByUser) {

            Input.location.Start();
        }
        #endif
	}

	void Update () {
	
        #if UNITY_IOS
        if(Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running) {

            if(lastLatitude == 0 || lastLongitude == 0) {

                lastLatitude = Input.location.lastData.latitude;
                lastLongitude = Input.location.lastData.longitude;
            }

            UpdateDistance();
            UpdateText();
        }
        #endif
	}

    void UpdateDistance () {

        float deltaDistance = HaversineDistance(ref lastLatitude, ref lastLongitude);

        if(deltaDistance > 0) {

            distance += deltaDistance;
        }
    }

    void UpdateText () {

        latitudeText.text = "Latitude: " + Input.location.lastData.latitude.ToString("N5");
        longitudeText.text = "Longitude: " + Input.location.lastData.longitude.ToString("N5");
        distanceText.text = "Distance: " + distance.ToString("N5");
    }
}
