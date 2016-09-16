﻿using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class MapPinchToZoom : MonoBehaviour {
    
	[Header("Parameters")]
	[SerializeField, Range(0.01f, 0.1f)] float zoomSpeed = 0.1f;

	[Header("References")]
    [SerializeField] Text outputText;
    [SerializeField] GoogleMapTextureLoader googleMap;


    void Update() {
		
        // If there are two touches on the device...
        if (Input.touchCount == 2) {
            
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			if(outputText) outputText.text = "Diff: " + deltaMagnitudeDiff;

            if(Mathf.Abs(deltaMagnitudeDiff) > 15) {

				int changeInZoom = (int)(deltaMagnitudeDiff * zoomSpeed);

				googleMap.ChangeZoom(changeInZoom);
            }
        }
        else if((Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.W)) ||
            (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.Q))) {

			googleMap.ChangeZoom(-zoomSpeed);
            //Debug.Log("Zooming in");
        }
        else if((Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E)) ||
            (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Q))) {

			googleMap.ChangeZoom(zoomSpeed);
            //Debug.Log("Zooming out");
        }
    }
}