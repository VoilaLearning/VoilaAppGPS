using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class PictureBoxController : MonoBehaviour {

	TutorialController tutorialController;

	[SerializeField] List<PictureContainer> photos = new List<PictureContainer>();

	[SerializeField] GameObject photoAlbum;
	[SerializeField] string locationName;

	AlbumController albumContainer;
	TextMesh number;

	void Start(){
		/* FOR TUTORIAL */
		/*GameObject tempContainer = GameObject.FindGameObjectWithTag ("Tutorial Controller").transform.GetChild (0).gameObject;
		tutorialController = tempContainer.GetComponent<TutorialController> ();
		tempContainer.SetActive(true);*/

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
		/*if ((tutorialController.InTutorial () && tutorialController.GetCurrentState() == TutorialState.JASONS_PIC) || !tutorialController.InTutorial ()) {
			if(tutorialController.InTutorial()) { tutorialController.AdvanceTutorial(); }
			photoAlbum.SetActive (true);
			this.GetComponentInParent<PictureBoxParent> ().DeactivateChildren ();

			// Load in all the pictures from a certain location - OR - pass the location to the photo album
			albumContainer = photoAlbum.transform.GetComponentInChildren<AlbumController> ();
			albumContainer.LoadAlbum (photos, this.gameObject);
		} */

		photoAlbum.SetActive (true);
		this.GetComponentInParent<PictureBoxParent> ().DeactivateChildren ();

		// Load in all the pictures from a certain location - OR - pass the location to the photo album
		albumContainer = photoAlbum.transform.GetComponentInChildren<AlbumController> ();
		albumContainer.LoadAlbum (photos, this.gameObject);

		photoAlbum.transform.GetComponentInChildren<Text> ().text = locationName;
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

	public void RemoveLastPicture(){
		// Debug.Log ("Removing Last Pic");
		if (this.transform.childCount > 1) { 
			Destroy (this.transform.GetChild (this.transform.childCount - 1).gameObject); 
			photos.RemoveAt (photos.Count - 1);
			SetNumberText ();
		}
	}
}