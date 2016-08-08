using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/*

	this is all temp - these will need to be saved to a server/database, not locally

*/

public class SavePicture : MonoBehaviour {

	[SerializeField] GameObject photoAlbum;
	[SerializeField] GameObject pictureContainerPrefab;
	[SerializeField] GameObject clickField;

	PictureContainer pictureContainer; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SaveImage(){
		// Get All of the input fields - and insert their text components and positions into a list 
		InputField[] tempArray = GameObject.FindObjectsOfType<InputField> ();
		List<string> words = new List<string> ();
		List<Vector3> wordPos = new List<Vector3> ();
		foreach (InputField field in tempArray) {
			words.Add (field.text);
			// translate to screen space
			Vector3 screenPos = /*Camera.main.WorldToScreenPoint*/(field.GetComponent<RectTransform>().localPosition);
			wordPos.Add (screenPos);
		}

		// Expand Photo Album, then add child
		GameObject newPicture = Instantiate (pictureContainerPrefab, this.transform.position, Quaternion.identity) as GameObject;
		newPicture.GetComponent<PictureContainer> ().FillContainer (this.GetComponent<Image> ().sprite, words, wordPos);
		newPicture.transform.SetParent (photoAlbum.transform, false);
		ExpandPhotoAlbum();
		ResetPicturePanel (tempArray);
	}

	void ExpandPhotoAlbum(){
		int numberOfChildren = photoAlbum.transform.childCount;
		float panelWidth = photoAlbum.GetComponent<RectTransform>().sizeDelta.x;
		float panelHeight = numberOfChildren * 170;
		photoAlbum.GetComponent<RectTransform>().sizeDelta = new Vector2(panelWidth, panelHeight);

	}

	void ResetPicturePanel (InputField[] inputs){
		foreach (InputField input in inputs) {
			Destroy (input.gameObject);
		}

		this.GetComponent<Image> ().sprite = null;
		this.gameObject.SetActive (false);
		clickField.SetActive (false);
	}
}
