using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 * This script will save and delete the photos and tags the player has inputted
 * It also uses the data to communicate milestrone progress back to the selected goal milestone
*/

public class SavePicture : MonoBehaviour {

	// Tutorial Stuff
	TutorialController tutorialController;
	[SerializeField] GameObject tutorialPhotoBox;

	// General 
	[SerializeField] GameObject photoAlbum;
	[SerializeField] GameObject pictureContainerPrefab;
	[SerializeField] GameObject clickField;
	[SerializeField] GameObject milestoneGoalUI;
	[SerializeField] GameObject photoBox;

	PictureContainer pictureContainer; 

	void Start(){
		tutorialController = GameObject.FindGameObjectWithTag ("Tutorial Controller").GetComponent<TutorialController>();
	}

	public void SaveImage(){
		// Get All of the input fields - and insert their text components and positions into a list 
		InputField[] tempArray = GameObject.FindObjectsOfType<InputField> ();
		List<string> words = new List<string> ();
		List<Vector3> wordPos = new List<Vector3> ();
		for (int i = 0; i < tempArray.Length; i++) {
			if (tempArray [i].text != "") {
				words.Add (tempArray [i].text);
				Vector3 screenPos = (tempArray [i].GetComponent<RectTransform> ().localPosition);
				wordPos.Add (screenPos);
			}
		}

		// Ensure the player has atgged a word
		if (words.Count == 0) {
			Debug.Log ("Must enter a tag to progress");
		} else {
			if (!tutorialController.InTutorial()) {
				CreateSave (tempArray, words, wordPos);
			} else {
				CreateTutorialSave (tempArray, words, wordPos);
			}
		}
	}

	void CreateTutorialSave(InputField[] inputs, List<string> words, List<Vector3> wordPos){
		// Create the pic as per ususal
		GameObject newPicture = Instantiate (pictureContainerPrefab, this.transform.position, Quaternion.identity) as GameObject;
		newPicture.GetComponent<PictureContainer> ().FillContainer (this.GetComponent<Image> ().sprite, words, wordPos, "");
		newPicture.transform.SetParent (photoAlbum.transform, false);
		// Turn on the temp photo box
		tutorialController.TempPhotoBox ();
		// tutorialPhotoBox.GetComponent<PictureBoxController>().ExpandPhotoAlbum ();
		tutorialPhotoBox.GetComponent<PictureBoxController> ().SetNumberText ();

		ResetPicturePanel (inputs);
	}

	void CreateSave(InputField[] inputs, List<string> words, List<Vector3> wordPos){

		// Increase the Milestone that the player was working on at the time
		string milestoneTitle = "";
		if (milestoneGoalUI.GetComponent<MilestoneController> ().GetCurrentMilestone () != null) {
			milestoneTitle = milestoneGoalUI.GetComponent<MilestoneController> ().GetCurrentMilestone ().GetTitle ();
		}
		IncreaseMilestoneGoal (words);

		// Create an instance of the saved picture/tags
		GameObject newPicture = Instantiate (pictureContainerPrefab, this.transform.position, Quaternion.identity) as GameObject;
		// Fill the data in the container
		newPicture.GetComponent<PictureContainer> ().FillContainer (this.GetComponent<Image> ().sprite, words, wordPos, milestoneTitle);
		// Send that picture to the closest photo box
		GameObject pictureBox = GameObject.FindGameObjectWithTag("Picture Box");
		pictureBox.GetComponent<PictureBoxController> ().AddPicture (newPicture.GetComponent<PictureContainer>());

		ResetPicturePanel (inputs);
	}

	void IncreaseMilestoneGoal(List<string> words){
		float percentageIncrease = 0.01f * words.Count;
		milestoneGoalUI.GetComponent<MilestoneController> ().IncreaseMilestonePercentage (percentageIncrease);
	}

	/*void ExpandPhotoAlbum(){
		int numberOfChildren = photoAlbum.transform.childCount;
		float panelWidth = photoAlbum.GetComponent<RectTransform>().sizeDelta.x;
		float panelHeight = numberOfChildren * 250;
		photoAlbum.GetComponent<RectTransform>().sizeDelta = new Vector2(panelWidth, panelHeight);
	}*/

	void ResetPicturePanel (InputField[] inputs){
		foreach (InputField input in inputs) {
			// Debug.Log ("Destroying Inputs");
			Destroy (input.gameObject);
		}

		this.GetComponent<Image> ().sprite = null;
		this.gameObject.SetActive (false);
		clickField.SetActive (false);
	}

	// Leave the photo edit without saving - delete data
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
