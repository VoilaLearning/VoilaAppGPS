using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[DisallowMultipleComponent]
public class GoogleMapTextureLoader : MonoBehaviour {

    [Header("Parameters")]
	public int zoom = 16;
	public int size = 512;
    public float refreshRate = 3;
	public bool doubleResolution = true;
    public GameObject loadingScreen;
    public Text locationText;

    private float currentLatitude;
    private float currentLongitude;
    Location[] locations;
    GooglePlaces googlePlaces;


	void Start() {
        
        locations = new Location[0];
        googlePlaces = this.GetComponent<GooglePlaces>();
        StopCoroutine(InitializeLocationServices());
        StartCoroutine(InitializeLocationServices());
	}

    IEnumerator InitializeLocationServices () {
        
        #if UNITY_IOS
        if(Input.location.isEnabledByUser) {

            Input.location.Start();
        }
        #endif

        while(Input.location.status == LocationServiceStatus.Initializing) {

            yield return null;
        }

        if(SystemInfo.deviceType == DeviceType.Desktop) {

            //currentLatitude = 43.6517f; // Work
            //currentLongitude = -79.36607f;
            currentLatitude = 43.6205f; // Centreville Theme Park
            currentLongitude = -79.3744f;
            //currentLatitude = 43.6289f;   // Toronto Islands
            //currentLongitude = -79.3944f;
        }

        StartCoroutine(AutoRefresh());
    }

    IEnumerator AutoRefresh () {

        if (SystemInfo.deviceType == DeviceType.Handheld) {

			//currentLatitude = 43.6205f; // Centreville Theme Park
			//currentLongitude = -79.3744f;
            float newLatitude = Input.location.lastData.latitude;
            float newLongitude = Input.location.lastData.longitude;
            if(locationText) { locationText.text = newLatitude + ", " + newLongitude; }

            if (newLatitude != currentLatitude && newLongitude != currentLongitude) {
            
                googlePlaces.RequestInfo();
                StopCoroutine(_Refresh());
                StartCoroutine(_Refresh());
            }
        }
        else if(SystemInfo.deviceType == DeviceType.Desktop) {

			if (locationText) {
				locationText.text = currentLatitude + ", " + currentLongitude;
			}
            googlePlaces.RequestInfo();
            StopCoroutine(_Refresh());
            StartCoroutine(_Refresh());
        }

        yield return new WaitForSeconds(refreshRate);

        //StartCoroutine(AutoRefresh());
    }
	
	IEnumerator _Refresh () {
        
//        if (SystemInfo.deviceType == DeviceType.Handheld) {
//            
//            currentLatitude = Input.location.lastData.latitude;
//            currentLongitude = Input.location.lastData.longitude;
//        }

        var url = "http://maps.googleapis.com/maps/api/staticmap";
        var qs = "";

		qs += "center=" + WWW.UnEscapeURL (string.Format ("{0},{1}", currentLatitude, currentLongitude));
		qs += "&zoom=" + zoom.ToString ();
		qs += "&size=" + WWW.UnEscapeURL (string.Format ("{0}x{0}", size));
		qs += "&scale=" + (doubleResolution ? "2" : "1");
        qs += "&maptype=roadmap";
        qs += "&key=AIzaSyDO-k0OB4_xCCMlaWpGls9xnZ1cFwerHd8";
		
        // Place markers
//        if(locations.Length > 0) {
//
//            qs += "&markers=" + string.Format ("color:blue");
//
//            foreach (var i in locations) {
//                
//                qs += "|" + WWW.UnEscapeURL (string.Format ("{0},{1}", i.lat, i.lng));
//            }
//        }

        // Send request to Google
        var req = new WWW(url + "?" + qs);
        yield return req;

        // Handle response from Google
        if (req.error != null) {
        
            Debug.Log("error: " + req.error);
            if(locationText) { locationText.text = req.error; }
        }
        else {

            Destroy(this.GetComponent<Renderer>().material.mainTexture);
            this.GetComponent<Renderer>().material.mainTexture = req.texture;
        }

		if(loadingScreen) { loadingScreen.SetActive(true); }
	}

    // Called from GPSReversegooglePlaces.cs
    public void SetMarkers (Location[] newLocations) {

        locations = newLocations;

        //StopCoroutine(_Refresh());
        //StartCoroutine(_Refresh());
    }

    public Vector2 GetPosition () {

        return new Vector2(currentLatitude, currentLongitude);
    }

    public void MovePosition (float deltaLat, float deltaLon) {

        currentLatitude += deltaLat;
        currentLongitude += deltaLon;

        googlePlaces.RequestInfo();
        //StopCoroutine(_Refresh());
        //StartCoroutine(_Refresh());
    }
}