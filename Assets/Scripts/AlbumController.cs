using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlbumController : MonoBehaviour {

	public void LoadAlbum(List<PictureContainer> boxPictures){

		foreach (PictureContainer picture in boxPictures) {
			picture.transform.SetParent (this.transform, false);
			ExpandPhotoAlbum ();
		}
	}

	public void EmptyAlbum(){
		for (int i = 0; i < transform.childCount; i++) {
			Destroy (transform.GetChild (i));
			ExpandPhotoAlbum ();
		}
	}


	public void ExpandPhotoAlbum(){
		int numberOfChildren = this.transform.childCount;
		float panelWidth = this.GetComponent<RectTransform>().sizeDelta.x;
		float panelHeight = numberOfChildren * 250;
		this.GetComponent<RectTransform>().sizeDelta = new Vector2(panelWidth, panelHeight);
	}
}
