using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class PictureBoxController : MonoBehaviour {

	TutorialController tutorialController;

	public List<PictureContainer> photos = new List<PictureContainer>();

	[SerializeField] GameObject photoAlbum;
	AlbumController albumContainer;
	TextMesh number;
	string locationName;

	void Start(){
		// photoAlbum = GameObject.FindGameObjectWithTag ("Photo Album");
		tutorialController = GameObject.FindGameObjectWithTag ("Tutorial Controller").GetComponent<TutorialController> ();
		number = GetComponentInChildren<TextMesh> ();
		SetNumberText ();
	}

	public void SetNumberText(){
		number = GetComponentInChildren<TextMesh> ();
		number.text = photos.Count.ToString();
		if (photos.Count > 0) {
			this.gameObject.SetActive (true);
		}
	}

	public void OnMouseDown(){
		//Debug.Log ("Click click!");
		photoAlbum.SetActive (true);
		if (!tutorialController.InTutorial()) this.GetComponentInParent<PictureBoxParent> ().DeactivateChildren ();

		// Load in all the pictures from a certain location - OR - pass the location to the photo album
		albumContainer = photoAlbum.transform.GetComponentInChildren<AlbumController> ();
		albumContainer.LoadAlbum (photos, this.gameObject);

		if (tutorialController.InTutorial ()) {
			tutorialController.OpenPhotoBox ();
			photoAlbum.transform.GetChild (1).GetComponent<Text> ().text = "Tutorial Album";
		} else {
			// Load in all the pictures from a certain location - OR - pass the location to the photo album
			photoAlbum.transform.GetComponentInChildren<Text> ().text = locationName;
		}
	}

    public void SetPhotoAlbum (GameObject newPhotoAlbum) {

        photoAlbum = newPhotoAlbum;
		// Debug.Log ("Setting Photo Album");
    }

    public void SetLocationName (string name) {

        locationName = name;
    }

	public void AddPicture(PictureContainer newPicture){
		photos.Add (newPicture);
		SetNumberText ();
	}

}