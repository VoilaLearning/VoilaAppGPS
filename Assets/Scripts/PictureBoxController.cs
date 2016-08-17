using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
	Holds all of the pictures and their associated data for specific stops
	Feeds data to the photo album when needed
*/

public class PictureBoxController : MonoBehaviour {

	TutorialController tutorialController;

	public List<PictureContainer> photos = new List<PictureContainer>();

	GameObject photoAlbum;
	AlbumController albumContainer;
	TextMesh number;
	string locationName;

	void Start(){
		photoAlbum = GameObject.FindGameObjectWithTag ("Photo Album");
		tutorialController = GameObject.FindGameObjectWithTag ("Tutorial Controller").GetComponent<TutorialController> ();
		number = GetComponentInChildren<TextMesh> ();
		SetNumberText ();
	}

	public void SetNumberText(){
		number = transform.GetChild(0).GetComponent<TextMesh>();
		number.text = albumContainer.transform.childCount.ToString();
		if (albumContainer.transform.childCount > 0) {
			this.gameObject.SetActive (true);
		}
	}

	public void OnMouseDown(){
		// Debug.Log ("Click click!");
		photoAlbum.gameObject.SetActive (true);

		if (tutorialController.InTutorial ()) {
			tutorialController.OpenPhotoBox ();
			photoAlbum.transform.GetChild(1).GetComponent<Text>().text = "Tutorial Album";
		}

		// Load in all the pictures from a certain location - OR - pass the location to the photo album
		albumContainer = photoAlbum.transform.GetComponentInChildren<AlbumController>();
		albumContainer.LoadAlbum(photos);

		photoAlbum.transform.GetComponentInChildren<Text> ().text = locationName;
	}

	public void SetPhotoAlbum(GameObject newPhotoAlbum){
		photoAlbum = newPhotoAlbum;
	}

	public void SetLocationName(string name){
		locationName = name;
	}

	public void AddPicture(PictureContainer newPicture){
		photos.Add (newPicture);
		SetNumberText ();
	}
}