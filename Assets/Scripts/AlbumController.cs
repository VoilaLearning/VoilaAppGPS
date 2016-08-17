using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlbumController : MonoBehaviour {

	[SerializeField] GameObject pictureContainerPrefab;

	GameObject pictureBox;

	public void LoadAlbum(List<PictureContainer> boxPictures, GameObject newPictureBox){

		pictureBox = newPictureBox;

		foreach (PictureContainer picture in boxPictures) {
			GameObject newContainer = Instantiate (pictureContainerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			newContainer.GetComponent<PictureContainer> ().FillContainer (picture.GetImage (), picture.GetWords (), picture.GetCoords (), picture.GetTagID ());
			newContainer.transform.SetParent (this.transform, false);
			ExpandPhotoAlbum ();
		}
	}

	public void EmptyAlbum(){
		for (int i = 0; i < transform.childCount; i++) {
			Destroy (transform.GetChild (i).gameObject);
			ExpandPhotoAlbum ();
		}

		pictureBox.GetComponent<PictureBoxController> ().SetNumberText ();
	}


	public void ExpandPhotoAlbum(){
		int numberOfChildren = this.transform.childCount;
		float panelWidth = this.GetComponent<RectTransform>().sizeDelta.x;
		float panelHeight = numberOfChildren * 250;
		this.GetComponent<RectTransform>().sizeDelta = new Vector2(panelWidth, panelHeight);
	}
}
