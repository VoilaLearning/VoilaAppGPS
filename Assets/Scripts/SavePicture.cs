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
	[SerializeField] GameObject milestoneGoalUI;

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

		string milestoneTitle = "";
		if (milestoneGoalUI.GetComponent<MilestoneController> ().GetCurrentMilestone () != null) {
			milestoneTitle = milestoneGoalUI.GetComponent<MilestoneController> ().GetCurrentMilestone ().GetTitle ();
		}

		// Debug.Log ("milestone: " + milestoneTitle);

		newPicture.GetComponent<PictureContainer> ().FillContainer (this.GetComponent<Image> ().sprite, words, wordPos, milestoneTitle);
		newPicture.transform.SetParent (photoAlbum.transform, false);
		IncreaseMilestoneGoal (words);
		ExpandPhotoAlbum();
		ResetPicturePanel (inputs);
	}

	void IncreaseMilestoneGoal(List<string> words){
		float percentageIncrease = 0.01f * words.Count;
		milestoneGoalUI.GetComponent<MilestoneController> ().IncreaseMilestonePercentage (percentageIncrease);
	}

	void ExpandPhotoAlbum(){
		int numberOfChildren = photoAlbum.transform.childCount;
		float panelWidth = photoAlbum.GetComponent<RectTransform>().sizeDelta.x;
		float panelHeight = numberOfChildren * 250;
		photoAlbum.GetComponent<RectTransform>().sizeDelta = new Vector2(panelWidth, panelHeight);
	}

	void ResetPicturePanel (InputField[] inputs){
		foreach (InputField input in inputs) {
			// Debug.Log ("Destroying Inputs");
			Destroy (input.gameObject);
		}

		this.GetComponent<Image> ().sprite = null;
		this.gameObject.SetActive (false);
		clickField.SetActive (false);
	}

	// Leave the photo edit without savine - delete data
	public void GoBack(){
	
		InputField[] tempArray = GameObject.FindObjectsOfType<InputField> ();
		foreach (InputField input in tempArray) {
			Destroy (input);
		}

		this.GetComponent<Image> ().sprite = null;
		clickField.SetActive (false);
		this.gameObject.SetActive (false);
	}
}
