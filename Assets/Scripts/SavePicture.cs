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

	public void SaveImage(){
		// Get All of the input fields - and insert their text components and positions into a list 
		InputField[] tempArray = GameObject.FindObjectsOfType<InputField> ();
		List<string> words = new List<string> ();
		List<Vector3> wordPos = new List<Vector3> ();
		for (int i = 0; i < tempArray.Length; i++) {
			if (tempArray [i].text != "") {
				words.Add (tempArray [i].text);
				Vector3 screenPos = /*Camera.main.WorldToScreenPoint*/(tempArray [i].GetComponent<RectTransform> ().localPosition);
				wordPos.Add (screenPos);
			}
		}
			
		if (words.Count == 0) {
			Debug.Log ("Must enter a tag to progress");
		} else {
			CreateSave (tempArray, words, wordPos);
		}
	}

	void CreateSave(InputField[] inputs, List<string> words, List<Vector3> wordPos){
		// Expand Photo Album, then add child
		GameObject newPicture = Instantiate (pictureContainerPrefab, this.transform.position, Quaternion.identity) as GameObject;
		newPicture.GetComponent<PictureContainer> ().FillContainer (this.GetComponent<Image> ().sprite, words, wordPos);
		newPicture.transform.SetParent (photoAlbum.transform, false);
		ExpandPhotoAlbum();
		ResetPicturePanel (inputs);
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
