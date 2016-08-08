using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PictureBoxController : MonoBehaviour {

	[SerializeField] GameObject photoAlbum;
	[SerializeField] GameObject albumContainer;
	[SerializeField] TextMesh number;

	public void SetNumberText(){
		number.text = albumContainer.transform.childCount.ToString();
		if (albumContainer.transform.childCount > 0) {
			this.gameObject.SetActive (true);
		}
	}

	public void OnMouseDown(){
		Debug.Log ("Click click!");
		photoAlbum.SetActive (true);
		// Load in all the pictures from a certain location - OR - pass the location to the photo album
	}
}