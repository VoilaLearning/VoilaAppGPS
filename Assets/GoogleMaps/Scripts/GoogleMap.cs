using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoogleMap : MonoBehaviour
{
	public enum MapType
	{
		RoadMap,
		Satellite,
		Terrain,
		Hybrid
	}

	public bool loadOnStart = true;
	public GoogleMapLocation centerLocation;
	public int zoom = 16;
	public MapType mapType;
	public int size = 512;
	public bool doubleResolution = true;
	public GoogleMapMarker[] markers;
	public GoogleMapPath[] paths;
	
    public bool autoRefresh = false;
    public Text coordinatesText;
    public GameObject loadingScreen;

	void Start() {
        
		if(loadOnStart) Refresh();	
	}

	public void Refresh() {

		StartCoroutine(_Refresh());
	}
	
	IEnumerator _Refresh ()
	{
		var url = "http://maps.googleapis.com/maps/api/staticmap";
		var qs = "";
		    
        loadingScreen.SetActive(true);

        if (centerLocation.address != "") {
			
            qs += "center=" + WWW.UnEscapeURL(centerLocation.address);
        }
		else {
            
            #if UNITY_IOS
            centerLocation.latitude = Input.location.lastData.latitude;
            centerLocation.longitude = Input.location.lastData.longitude;
            #endif

            if (centerLocation.latitude == 0 && centerLocation.longitude == 0) {
                centerLocation.latitude = 43.65184f;
                centerLocation.longitude = -79.36607f;
            }

			qs += "center=" + WWW.UnEscapeURL (string.Format ("{0},{1}", centerLocation.latitude, centerLocation.longitude));
		}
	
		qs += "&zoom=" + zoom.ToString ();
		qs += "&size=" + WWW.UnEscapeURL (string.Format ("{0}x{0}", size));
		qs += "&scale=" + (doubleResolution ? "2" : "1");
		qs += "&maptype=" + mapType.ToString ().ToLower ();

		var usingSensor = false;

        #if UNITY_IPHONE
		usingSensor = Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running;
        #endif
		
        qs += "&sensor=" + (usingSensor ? "true" : "false");
		
		foreach (var i in markers) {
			qs += "&markers=" + string.Format ("size:{0}|color:{1}|label:{2}", i.size.ToString ().ToLower (), i.color, i.label);
			foreach (var loc in i.locations) {
				if (loc.address != "")
					qs += "|" + WWW.UnEscapeURL (loc.address);
				else
					qs += "|" + WWW.UnEscapeURL (string.Format ("{0},{1}", loc.latitude, loc.longitude));
			}
		}
		
		foreach (var i in paths) {
			qs += "&path=" + string.Format ("weight:{0}|color:{1}", i.weight, i.color);
			if(i.fill) qs += "|fillcolor:" + i.fillColor;
			foreach (var loc in i.locations) {
				if (loc.address != "")
					qs += "|" + WWW.UnEscapeURL (loc.address);
				else
					qs += "|" + WWW.UnEscapeURL (string.Format ("{0},{1}", loc.latitude, loc.longitude));
			}
		}
		
		
        var req = new WWW(url + "?" + qs);
        yield return req;

        this.GetComponent<Renderer>().material.mainTexture = req.texture;
        coordinatesText.text = centerLocation.latitude + ", " + centerLocation.longitude;
	}

    public void ToggleAutoRefresh(bool startRefreshing) {

        if(startRefreshing) {
            
            StopCoroutine(AutoRefresh());
            StartCoroutine(AutoRefresh());
        }
        else {
            
            StopCoroutine(AutoRefresh());
        }
    }

    IEnumerator AutoRefresh () {
        
        float currentLatitude = Input.location.lastData.latitude;
        float currentLongitude = Input.location.lastData.longitude;

        if (currentLatitude != centerLocation.latitude && currentLongitude != centerLocation.longitude) {

            StopCoroutine(_Refresh());
            StartCoroutine(_Refresh());
        }

        yield return new WaitForSeconds(3);

        StartCoroutine(AutoRefresh());
    }
}

public enum GoogleMapColor
{
	black,
	brown,
	green,
	purple,
	yellow,
	blue,
	gray,
	orange,
	red,
	white
}

[System.Serializable]
public class GoogleMapLocation
{
	public string address;
	public float latitude;
	public float longitude;
}

[System.Serializable]
public class GoogleMapMarker
{
	public enum GoogleMapMarkerSize
	{
		Tiny,
		Small,
		Mid
	}
	public GoogleMapMarkerSize size;
	public GoogleMapColor color;
	public string label;
	public GoogleMapLocation[] locations;
	
}

[System.Serializable]
public class GoogleMapPath
{
	public int weight = 5;
	public GoogleMapColor color;
	public bool fill = false;
	public GoogleMapColor fillColor;
	public GoogleMapLocation[] locations;	
}