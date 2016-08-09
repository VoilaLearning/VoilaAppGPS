using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoogleMapTextureLoader : MonoBehaviour {

    [Header("Parameters")]
	public int zoom = 16;
	public int size = 512;
    public float refreshRate = 3;
	public bool doubleResolution = true;
	
    [Header("References")]
    public Text coordinatesText;
    public GameObject loadingScreen;

    private float currentLatitude;
    private float currentLongitude;


	void Start() {

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

        Refresh();
    }

	void Refresh() {

		StartCoroutine(_Refresh());
	}
	
	IEnumerator _Refresh () {
        
        if (SystemInfo.deviceType == DeviceType.Handheld) {
            
            currentLatitude = Input.location.lastData.latitude;
            currentLongitude = Input.location.lastData.longitude;
        }
        else {

            currentLatitude = 43.65184f;
            currentLongitude = -79.36607f;
        }

        var url = "http://maps.googleapis.com/maps/api/staticmap";
        var qs = "";

		qs += "center=" + WWW.UnEscapeURL (string.Format ("{0},{1}", currentLatitude, currentLongitude));
		qs += "&zoom=" + zoom.ToString ();
		qs += "&size=" + WWW.UnEscapeURL (string.Format ("{0}x{0}", size));
		qs += "&scale=" + (doubleResolution ? "2" : "1");
		qs += "&maptype=roadmap";

		var usingSensor = false;

        #if UNITY_IPHONE
		usingSensor = Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running;
        #endif
		
        qs += "&sensor=" + (usingSensor ? "true" : "false");
				
        var req = new WWW(url + "?" + qs);
        yield return req;

        this.GetComponent<Renderer>().material.mainTexture = req.texture;
        if(coordinatesText){ coordinatesText.text = currentLatitude + ", " + currentLongitude; }
	}

    public void ToggleAutoRefresh(bool startRefreshing) {

        if(startRefreshing) {
            
            StopCoroutine(AutoRefresh());
            StartCoroutine(AutoRefresh());
        }
        else {
            
            StopAllCoroutines();
        }
    }

    IEnumerator AutoRefresh () {

        if(loadingScreen) { loadingScreen.SetActive(true); }

        float newLatitude = Input.location.lastData.latitude;
        float newLongitude = Input.location.lastData.longitude;

        if (newLatitude != currentLatitude && newLongitude != currentLongitude) {
            
            StopCoroutine(_Refresh());
            StartCoroutine(_Refresh());
        }

        yield return new WaitForSeconds(refreshRate);

        StartCoroutine(AutoRefresh());
    }
}