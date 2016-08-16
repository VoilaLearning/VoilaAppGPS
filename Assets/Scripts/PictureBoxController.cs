using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class PictureBoxController : MonoBehaviour {

	[SerializeField] GameObject albumContainer;
    GameObject photoAlbum;
	TextMesh number;
    string locationName;


    void Awake () {

        number = this.GetComponentInChildren<TextMesh>();
    }

	public void SetNumberText(){
		number.text = albumContainer.transform.childCount.ToString();
		if (albumContainer.transform.childCount > 0) {
			this.gameObject.SetActive (true);
		}
	}

	public void OnMouseDown(){
		//Debug.Log ("Click click!");
		photoAlbum.SetActive (true);
		// Load in all the pictures from a certain location - OR - pass the location to the photo album
        photoAlbum.transform.GetComponentInChildren<Text>().text = locationName;
	}

    public void SetPhotoAlbum (GameObject newPhotoAlbum) {

        photoAlbum = newPhotoAlbum;
    }

    public void SetLocationName (string name) {

        locationName = name;
    }
}