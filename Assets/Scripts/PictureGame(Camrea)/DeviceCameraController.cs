using UnityEngine;
using System.Collections;

public class DeviceCameraController : MonoBehaviour {

	[SerializeField] WebCamTexture mCamera = null;
	[SerializeField] GameObject plane;

	// Use this for initialization
	void Start () {

		Debug.Log ("Script has been started.");
		plane = GameObject.FindGameObjectWithTag ("Player");

		mCamera = new WebCamTexture ();
		plane.GetComponent<Renderer>().material.mainTexture = mCamera;
		mCamera.Play();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
