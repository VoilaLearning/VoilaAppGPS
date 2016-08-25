using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlbumController : MonoBehaviour {

	[SerializeField] GameObject pictureContainerPrefab;

	List<PictureContainer> containers;
	GameObject pictureBox;

	public void LoadAlbum(List<PictureContainer> boxPictures, GameObject newPictureBox){

		containers = boxPictures;
		pictureBox = newPictureBox;

		foreach (PictureContainer picture in boxPictures) {
			/*GameObject newContainer = Instantiate (pictureContainerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            newContainer.GetComponent<PictureContainer> ().FillContainer (picture.GetImage (), picture.GetWords (), picture.GetCoords (), picture.GetTagID (), picture.GetName());
			newContainer.transform.SetParent (this.transform, false);*/

			picture.transform.SetParent (this.transform, false);
			picture.GetComponent<RectTransform> ().localScale = Vector3.one;
			picture.UpdateText ();
			ExpandPhotoAlbum ();
		}
	}

	public void EmptyAlbum(){
		
		for (int i = transform.childCount - 1; i >= 0; i--) {
			/*Destroy (transform.GetChild (i).gameObject);*/
			containers [i].transform.SetParent (pictureBox.transform, false);
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
