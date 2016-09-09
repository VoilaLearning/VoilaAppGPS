using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[DisallowMultipleComponent]
public class GoogleMapTextureLoader : MonoBehaviour {

    [Header("Parameters")]
	public float zoom = 16;
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
        ChangeZoom(0);
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

        StartCoroutine(AutoRefresh());
    }

    IEnumerator AutoRefresh () {

        if (SystemInfo.deviceType == DeviceType.Handheld) {

//            float newLatitude = Input.location.lastData.latitude;
//            float newLongitude = Input.location.lastData.longitude;
//            if(locationText) { locationText.text = newLatitude + ", " + newLongitude; }
//
//            if (newLatitude != currentLatitude && newLongitude != currentLongitude) {
//            
//                currentLatitude = newLatitude;
//                currentLongitude = newLongitude;
//
//                //googlePlaces.RequestInfo();
//                StopCoroutine(_Refresh());
//                StartCoroutine(_Refresh());
//            }

            currentLatitude = 43.6205f; // Centreville Theme Park
            currentLongitude = -79.3744f;
            StopCoroutine(_Refresh());
            StartCoroutine(_Refresh());
        }
        else if(SystemInfo.deviceType == DeviceType.Desktop) {

            currentLatitude = 43.6517f; // Work
            currentLongitude = -79.36607f;
//            currentLatitude = 43.6205f; // Centreville Theme Park
//            currentLongitude = -79.3744f;
//            currentLatitude = 43.6289f;   // Toronto Islands
//            currentLongitude = -79.3944f;
			if (locationText) { locationText.text = currentLatitude + ", " + currentLongitude; }

            //googlePlaces.RequestInfo();
            StopCoroutine(_Refresh());
            StartCoroutine(_Refresh());
        }

        yield return new WaitForSeconds(refreshRate);

        StartCoroutine(AutoRefresh());
    }
	
	IEnumerator _Refresh () {
        
        var url = "http://maps.googleapis.com/maps/api/staticmap";
        var qs = "";

		qs += "center=" + WWW.UnEscapeURL (string.Format ("{0},{1}", currentLatitude, currentLongitude));
        qs += "&zoom=" + ((int)zoom).ToString ();
		qs += "&size=" + WWW.UnEscapeURL (string.Format ("{0}x{0}", size));
		qs += "&scale=" + (doubleResolution ? "2" : "1");
        qs += "&maptype=roadmap";
        		
        // Stylize map: https://developers.google.com/maps/documentation/static-maps/styling#examples
//        qs += "&style=feature:landscape|color:0xff0000|saturation:-50";         // Landscape color
//        qs += "&style=feature:road|element:geometry.fill|color:0xffffff";       // Road's fill color
//        qs += "&style=feature:road|element:geometry.stroke|color:0xff0000";     // Road's outline color
//        qs += "&style=feature:road|element:labels.text.fill|color:0xffffff";    // Road's text fill color
//        qs += "&style=feature:road|element:labels.text.stroke|color:0x000000";  // Road's text outline color
//        qs += "&style=feature:poi|color:0x00ff00";                              // Point of interest color
//        qs += "&style=feature:poi|element:labels.text.fill|color:0xffffff";     // Point of interest text fill color
//        qs += "&style=feature:poi|element:labels.text.stroke|color:0x000000";   // Point of interest text outline color
//        qs += "&style=feature:water|color:0x0000ff";                            // Water color
//        qs += "&style=feature:transit|visibility:off";                          // Hiding all transit related stuff

        // API Key
        qs += "&key=AIzaSyDO-k0OB4_xCCMlaWpGls9xnZ1cFwerHd8";

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

    // Called from GooglePlaces.cs
    public Vector2 GetPosition () {

        return new Vector2(currentLatitude, currentLongitude);
    }        

    public void ChangeZoom (float change) {

        // Move plane
        const float minDistance = 0;
        const float maxDistance = 7;
        float newZ = this.transform.position.z + change;
        newZ = Mathf.Clamp(newZ, minDistance, maxDistance);
        this.transform.position = new Vector3(0, 1, newZ);
        //Debug.Log("newZ: " + newZ);

        // Change zoom level
        const float minZoom = 10;
        const float maxZoom = 20;
        float distanceRatio = 1 - (this.transform.position.z - minDistance) / (maxDistance - minDistance);
        //zoom = (maxZoom - minZoom) * distanceRatio + minZoom;
        //Debug.Log("zoom: " + zoom);


        _Refresh();
    }
}