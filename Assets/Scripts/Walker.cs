using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class Walker : MonoBehaviour {

    [Header("Parameters")]
    [SerializeField] float latitudeIncrement = 1e-4f;
    [SerializeField] float longitudeIncrement = 1e-4f;

    [Header("References")]
    [SerializeField] GoogleMapTextureLoader mapTexture;


	void Update () {
	
        ProcessInput();
	}

    void ProcessInput () {

        // Up-down / Latitude
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {

            mapTexture.MovePosition(latitudeIncrement, 0);
        }
        else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {

            mapTexture.MovePosition(-latitudeIncrement, 0);
        }

        // Left-right / Longitude
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {

            mapTexture.MovePosition(0, -longitudeIncrement);
        }
        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {

            mapTexture.MovePosition(0, longitudeIncrement);
        }
    }
}