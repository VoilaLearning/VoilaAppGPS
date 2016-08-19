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
	[SerializeField] Image picture;

	PictureContainer pictureContainer; 

	void Start(){
		tutorialController = GameObject.FindGameObjectWithTag ("Tutorial Controller").GetComponentInChildren<TutorialController>();
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
		Debug.Log ("Creating Tutorial Save.");

		// Increase the Milestone that the player was working on at the time
		string milestoneTitle = "";
		if (milestoneGoalUI.GetComponent<MilestoneController> ().GetCurrentMilestone () != null) {
			milestoneTitle = milestoneGoalUI.GetComponent<MilestoneController> ().GetCurrentMilestone ().GetTitle ();
		}
		IncreaseMilestoneGoal (words);

		// Create the pic as per ususal
		GameObject newPicture = Instantiate (pictureContainerPrefab, this.transform.position, Quaternion.identity) as GameObject;
		newPicture.GetComponent<PictureContainer> ().FillContainer (picture.sprite, words, wordPos, "");
		// Turn on the temp photo box
		tutorialController.TempPhotoBox ();
		tutorialPhotoBox.GetComponent<PictureBoxController> ().AddPicture (newPicture.GetComponent<PictureContainer>());

		ResetPicturePanel (inputs);
	}

	void CreateSave(InputField[] inputs, List<string> words, List<Vector3> wordPos){
		Debug.Log ("Create Regular Save");

		// Increase the Milestone that the player was working on at the time
		string milestoneTitle = "";
		if (milestoneGoalUI.GetComponent<MilestoneController> ().GetCurrentMilestone () != null) {
			milestoneTitle = milestoneGoalUI.GetComponent<MilestoneController> ().GetCurrentMilestone ().GetTitle ();
		}
		IncreaseMilestoneGoal (words);

		// Create an instance of the saved picture/tags
		GameObject newPicture = Instantiate (pictureContainerPrefab, this.transform.position, Quaternion.identity) as GameObject;
		// Fill the data in the container
		newPicture.GetComponent<PictureContainer> ().FillContainer (picture.sprite, words, wordPos, milestoneTitle);

		// Send that picture to the closest photo box
		GameObject[] pictureBoxArray = GameObject.FindGameObjectsWithTag("Picture Box");
		float[] distancesArray = new float[pictureBoxArray.Length];
		for (int i = 0; i < pictureBoxArray.Length; i++) {

			distancesArray [i] = Vector3.Distance (new Vector3 (0, 1, 0), pictureBoxArray [i].transform.localPosition);
			//Debug.Log ("distances: " + distancesArray [i]);
		}

		float minValue = Mathf.Min (distancesArray);
		int minValueIndex = System.Array.IndexOf (distancesArray, minValue);
		pictureBoxArray [minValueIndex].GetComponent<PictureBoxController> ().AddPicture (newPicture.GetComponent<PictureContainer> ());
		// pictureBox.GetComponent<PictureBoxController> ().AddPicture (newPicture.GetComponent<PictureContainer>());

		ResetPicturePanel (inputs);
	}

	void IncreaseMilestoneGoal(List<string> words){
		float percentageIncrease = 0.01f * words.Count;
		milestoneGoalUI.GetComponent<MilestoneController> ().IncreaseMilestonePercentage (percentageIncrease);
	}

	void ResetPicturePanel (InputField[] inputs){
		foreach (InputField input in inputs) {
			// Debug.Log ("Destroying Inputs");
			Destroy (input.gameObject);
		}

		picture.sprite = null;
		this.gameObject.SetActive (false);
		clickField.SetActive (false);
	}

	// Leave the photo edit without saving - delete data
	public void GoBack(){
	
		InputField[] tempArray = GameObject.FindObjectsOfType<InputField> ();
		foreach (InputField input in tempArray) {
			Destroy (input);
		}

		picture.sprite = null;
		clickField.SetActive (false);
		this.gameObject.SetActive (false);
	}
}
