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

	// General 
	[SerializeField] GameObject photoAlbum;
	[SerializeField] GameObject pictureContainerPrefab;
	[SerializeField] GameObject clickField;
	[SerializeField] GameObject milestoneGoalUI;
	[SerializeField] Image picture;
    [SerializeField] PictureBoxParent pictureBoxParent;

	// Lists to hold the Avatar and stickers
	GameObject avatar;
	public List<GameObject> stickers = new List<GameObject> ();

	InputField[] inputs;
	PictureContainer pictureContainer; 
	GameObject tutorialPictureBox;
	string milestoneTitle = "";

	void Start(){
		// tutorialController = GameObject.FindGameObjectWithTag ("Tutorial Controller").GetComponentInChildren<TutorialController>();
	}

	public void SaveImage(){
		// Get All of the input fields - and insert their text components and positions into a list 
		inputs = GameObject.FindObjectsOfType<InputField> ();
		List<string> words = new List<string> ();
		List<Vector3> wordPos = new List<Vector3> ();
		for (int i = 0; i < inputs.Length; i++) {
			if (inputs [i].text != "") {
				words.Add (inputs [i].text);
				Vector3 screenPos = (inputs [i].GetComponent<RectTransform> ().localPosition);
				wordPos.Add (screenPos);
			}
		}

		// Ensure the player has atgged a word
		if (words.Count == 0) {
			Debug.Log ("Must enter a tag to progress");
		} else {
			CreateSave (inputs, words, wordPos);
			/*if (!tutorialController.InTutorial()) {
				CreateSave (tempArray, words, wordPos);
			} else {
				CreateTutorialSave (tempArray, words, wordPos);
			}*/
		}
	}

	void CreateSave(InputField[] inputs, List<string> words, List<Vector3> wordPos){
		
		// Increase the Milestone that the player was working on at the time
		if (milestoneGoalUI.GetComponent<MilestoneController> ().GetCurrentMilestone () != null) {
            IncreaseMilestoneGoal (words);
		}

		// Create an instance of the saved picture/tags
		GameObject newPicture = Instantiate (pictureContainerPrefab, this.transform.position, Quaternion.identity) as GameObject;
		// Fill the data in the container
		newPicture.GetComponent<PictureContainer> ().FillContainer (picture.sprite, words, wordPos, milestoneTitle, PlayerData.GetPlayerName(), stickers, avatar);

		SendToAlbum (newPicture);

		// Save all the positions of the stickers to be used later
		foreach(GameObject sticker in stickers){
			sticker.GetComponent<StickerController> ().SavePosition ();
		}

		// If we are in the tutorial - advance a step
		if (tutorialController.GetCurrentState() == TutorialState.SAVE_PHOTO) {
			tutorialController.AdvanceTutorial ();
			// tutorialPictureBox = pictureBoxArray [minValueIndex].gameObject;
		}

		ResetPicturePanel ();
	}

	void SendToAlbum(GameObject newPicture){
		// Send that picture to the closest photo boxAddSticker
		pictureBoxParent.GetComponent<PictureBoxParent> ().ActivateChildren();
		GameObject[] pictureBoxArray = GameObject.FindGameObjectsWithTag("Picture Box");
		float[] distancesArray = new float[pictureBoxArray.Length];
		for (int i = 0; i < pictureBoxArray.Length; i++) {
			distancesArray [i] = Vector3.Distance (new Vector3 (0, 1, 0), pictureBoxArray [i].transform.localPosition);
		}

		float minValue = Mathf.Min (distancesArray);
		int minValueIndex = System.Array.IndexOf (distancesArray, minValue);
		Debug.Log (minValueIndex);
		pictureBoxArray [minValueIndex].GetComponent<PictureBoxController> ().AddPicture (newPicture.GetComponent<PictureContainer> ());
		newPicture.transform.parent = pictureBoxArray [minValueIndex].transform;

	}

	void IncreaseMilestoneGoal(List<string> words){
		milestoneTitle = milestoneGoalUI.GetComponent<MilestoneController> ().GetCurrentMilestone ().GetTitle ();
		float percentageIncrease = 0.01f * words.Count;
		milestoneGoalUI.GetComponent<MilestoneController> ().IncreaseMilestonePercentage (percentageIncrease);
	}

	public void ResetPicturePanel (){
		Debug.Log ("Resetting Picture Panel");

		if (inputs != null) {
			foreach (InputField input in inputs) {
				if (input != null) {
					// if you are out of the tutorial destroy the inputs
					if (!tutorialController.InTutorial ()) {
						Destroy (input.gameObject);
					}
					// Just toggle them off if you are in the tutorial
					else {
						input.gameObject.SetActive (false);
					}
				}
			}
		}

		inputs = new InputField[0]; 

		// Clear the avatar and sticker lists\
		Destroy(avatar.gameObject);
		avatar = null;
		foreach(GameObject sticker in stickers){ Destroy(sticker.gameObject); }
		stickers.Clear ();

		if (!tutorialController.InTutorial ()) picture.sprite = null;
		this.gameObject.SetActive (false);
		clickField.SetActive (false);
	}

	public void ToggleOnInputs (){
		// Debug.Log ("toggling on inputs");
		if (inputs != null) {
			foreach (InputField input in inputs) {
				input.gameObject.SetActive (true);
			}
		}

		this.gameObject.SetActive (true);
		clickField.SetActive (true);
	}

	public void DeleteLastTutorialPhoto(){
		if (tutorialPictureBox) { 
			// Debug.Log ("Removing last Image");
			tutorialPictureBox.GetComponent<PictureBoxController> ().RemoveLastPicture (); 
		}
	}

	public void TogglePictureBoxParent(){
		/*if (!tutorialController.InTutorial ()) {
			pictureBoxParent.ActivateChildren ();
		}*/

		pictureBoxParent.ActivateChildren ();
	}

	// Leave the photo edit without saving - delete data
	/* public void GoBack(){
		// Debug.Log ("Going Back");
		InputField[] tempArray = GameObject.FindObjectsOfType<InputField> ();
		// Debug.Log ("Array Length: " + tempArray.Length);

		foreach (InputField input in tempArray) {
			Destroy (input.gameObject);
		}

		ResetPicturePanel ();

		picture.sprite = null;
		clickField.SetActive (false);
		this.gameObject.SetActive (false);
	} */ 

	public void AddSticker(GameObject sticker){
		stickers.Add (sticker);
	}

	public void RemoveSticker(GameObject sticker){
		stickers.Remove (sticker);
	}

	public void AddAvatar(GameObject newAvatar){
		avatar = newAvatar;
	}
}
