using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Buttons {AVATAR = 0, CAMERA, MENU, COUNT};
public enum TutorialPanel {CONTENT = 0, DOWN_ARROWS, TAP_ICON, MILESTONE_ARROW, END_TUT_BUTTON, COUNT};

public class TutorialController : MonoBehaviour {

	[SerializeField] GameObject tutorialTextBox; 
	[SerializeField] GameObject tutPictureBox;
	[SerializeField] GameObject photoAlbum;
	[SerializeField] Button[] menuButtons;
	[SerializeField] Button savePhotoButton;
	[SerializeField] Button leavePicturePanelButton;
	[SerializeField] Button dictionaryButton;
	[SerializeField] Button milestoneBackButton;
	[SerializeField] Button resetTutorialButton;
	[SerializeField] GameObject pictureBoxParent;
	[SerializeField] GameObject milestoneUI;

	string introMessage = "Learn be able to explore your home, school and neighbourhood translate your world into french! To begin, click the button below to take a picture!";
	string labelPicMessage = "Label up to 3 of the things you can recognize in your picture, En Francais! Go ahead, tap the screen and give it a try. When you are finished hit the save button to see your picture.";
	string openAlbumMessage = "Now you can view the image you took with the tags you included, click on the numbered circle to view your photos!";
	string openSavedPictureMessage = "We are in the photo album, you can click on your photo to see it and the tags you marked again, other students will be able to see your photo as well!";
	string wordTowardsAMilestoneMessage = "You can gain points by working towards milestones we have set out for you, click on the button below to see all the milestones available";
	string selectAMilestoneMessage = "Choose any milestome in the list by clicking on the name. Then take pictures and create tags for that topic! Then you can show off you're hard work to the city!";
	string endTutorialMessage = "Now we can take apicture and work towards this milestone!";

	public bool inTutorial;

	// Use this for initialization
	void Start () {
		Debug.Log ("Starting Tut");
		StartMilestoneTutorial ();
		pictureBoxParent.GetComponent<PictureBoxParent> ().DeactivateChildren();
	}

	void ResetButtonsAndArrows(){
		for (int i = 0; i < (int)Buttons.COUNT; i++) {
			menuButtons [i].interactable = true;
			menuButtons[i].gameObject.GetComponent<Image> ().color = Color.white;
			menuButtons[i].GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
			this.transform.GetChild((int)TutorialPanel.DOWN_ARROWS).transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	void ToggleButtonOff(Button button){
		button.interactable = false;
		button.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f);
		button.GetComponent<Image> ().color = Color.grey;
	}

	void ToggleButtonOn(Button button){
		button.interactable = true;
		button.GetComponent<RectTransform> ().localScale = new Vector3 (1.1f, 1.1f, 1.1f);
		button.GetComponent<Image> ().color = Color.white;
	}

	// called in Start()
	public void StartTutorial(){
		inTutorial = true;
		this.gameObject.SetActive (true);
		tutorialTextBox.gameObject.SetActive (true);
		StopAllCoroutines ();

		// Reset the buttons and arrows before beginning a new step in the Tutorial
		ResetButtonsAndArrows();
		// Update tutorial text
		UpdateText ("Bienvenue!", introMessage);
		// Ensure the other buttons cannot be pressed
		ToggleButtonOff(menuButtons[(int)Buttons.AVATAR]);
		ToggleButtonOff(menuButtons[(int)Buttons.MENU]);
		// Make sure that the camera button is more "Obvious"
		ToggleButtonOn(menuButtons[(int)Buttons.CAMERA]);
		// Turn on the center arrow
		this.transform.GetChild((int)TutorialPanel.DOWN_ARROWS).transform.GetChild((int)Buttons.CAMERA).gameObject.SetActive(true);
		resetTutorialButton.gameObject.SetActive (false);
	}

	// Called in EtecertraPictureGameController.cs
	public void CloseCamera(){
		if (inTutorial) {
			tutorialTextBox.SetActive (true);
			// Turn off the tutorial box so the player can see the full screen 
			StopAllCoroutines ();
			StartCoroutine(CloseInstructions());

			ResetButtonsAndArrows ();
			// Turn on the "Tap" UI
			this.transform.GetChild ((int)TutorialPanel.TAP_ICON).gameObject.SetActive (true);


			// Set the Text Boxes
			UpdateText ("Label Your Picture!", labelPicMessage);

			// Turn on the Middle Arrow
			this.transform.GetChild((int)TutorialPanel.DOWN_ARROWS).transform.GetChild((int)Buttons.CAMERA).gameObject.SetActive(true);

			// Disable the back Button and Enlarge the Save Button in the Picture Panel
			savePhotoButton.GetComponent<RectTransform>().localScale = new Vector3(1.1f, 1.1f, 1.1f);
			leavePicturePanelButton.interactable = false;
			leavePicturePanelButton.GetComponent<Image> ().color = Color.grey;
		}
	}

	// Called in SavePicture.cs in CreateTutorialSave()
	public void TempPhotoBox(){
		if (inTutorial) {
			StopAllCoroutines ();

			// Reset the buttons in the Picture Panel
			savePhotoButton.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
			leavePicturePanelButton.interactable = true;
			leavePicturePanelButton.GetComponent<Image> ().color = Color.white;

			// Turn off the Middle Arrow
			this.transform.GetChild((int)TutorialPanel.DOWN_ARROWS).transform.GetChild((int)Buttons.CAMERA).gameObject.SetActive(false);
			// Turn off milestone UI??
			this.transform.GetChild ((int)TutorialPanel.MILESTONE_ARROW).gameObject.SetActive (false);

			// When the player has saved their image - turn off all other buttons and create a temp photo box - we will delete this after the tutorial
			tutPictureBox.SetActive(true);
			ResetButtonsAndArrows ();
			ToggleButtonOff (menuButtons[(int)Buttons.AVATAR]);
			ToggleButtonOff (menuButtons[(int)Buttons.CAMERA]);
			ToggleButtonOff (menuButtons[(int)Buttons.MENU]);
			// Turn off the click icon on the Tutorial panel 
			this.transform.GetChild((int)TutorialPanel.TAP_ICON).gameObject.SetActive(false);

			// Toggle on the text box and tell player to open the album
			tutorialTextBox.SetActive (true);

			UpdateText ("Open Your Album!", openAlbumMessage);
		}
	}

	// Called in PictureBoxController.cs in OnMouseDown()
	public void OpenPhotoBox(){
		if (inTutorial) {
			tutorialTextBox.SetActive (true);
			// Turn off the tutorial box so the player can see the full screen 
			StopAllCoroutines ();
			StartCoroutine(CloseInstructions());
			// Set the header and body text of the message
			UpdateText("Look at your Photos!", openSavedPictureMessage);
		}
	}

	// Called from the Back Button in the Photo Album Scroll View
	public void StartMilestoneTutorial(){

		inTutorial = true;
		this.gameObject.SetActive (true);
		tutorialTextBox.gameObject.SetActive (true);
		StopAllCoroutines ();

		StopAllCoroutines ();
		tutorialTextBox.SetActive (true);

		// Remove the Image from the photo album
		// photoAlbum.GetComponent<AlbumController>().EmptyAlbum();

		// Delete the Temp Photo Box
		// tutPictureBox.SetActive(false);

		ResetButtonsAndArrows ();

		// Turn on the arrow above the milestone container
		this.transform.GetChild ((int)TutorialPanel.DOWN_ARROWS).transform.GetChild ((int)Buttons.MENU).gameObject.SetActive (true);

		ToggleButtonOff (menuButtons [(int)Buttons.AVATAR]);
		ToggleButtonOff (menuButtons [(int)Buttons.CAMERA]);

		// Ensure they can only Select the Milestone Button
		menuButtons [(int)Buttons.MENU].transform.GetChild (0).transform.GetChild (0).GetComponent<Button> ().interactable = false;
		menuButtons [(int)Buttons.MENU].transform.GetChild (0).transform.GetChild (0).GetComponent<Image> ().color = Color.grey;
		menuButtons [(int)Buttons.MENU].transform.GetChild (0).transform.GetChild (1).GetComponent<Button> ().interactable = false;
		menuButtons [(int)Buttons.MENU].transform.GetChild (0).transform.GetChild (1).GetComponent<Image> ().color = Color.grey;

		// Set the header and the message
		UpdateText("Work Towards a Milestone!", wordTowardsAMilestoneMessage);
	}

	// Called from the Menu Options Button
	public void OpenMenuButtons(){
		// Turn off the arrow in order to advance
		if (inTutorial) {
			this.transform.GetChild ((int)TutorialPanel.DOWN_ARROWS).transform.GetChild ((int)Buttons.MENU).gameObject.SetActive (false);
		}
	}

	// Called from the milestone button
	public void OpenMilestoneContainer(){
		if (inTutorial) {
			tutorialTextBox.SetActive (true);
			// Turn off the tutorial box so the player can see the full screen 
			StopAllCoroutines ();
			StartCoroutine(CloseInstructions());

			// Set the Header and the Body
			UpdateText ("Select a Milestone!", selectAMilestoneMessage);

			// Toggle on the first arrow
			this.transform.GetChild((int)TutorialPanel.DOWN_ARROWS).transform.GetChild((int)Buttons.AVATAR).gameObject.SetActive(true);

			// Toggle off the dictionary Button
			dictionaryButton.interactable = false;
			dictionaryButton.GetComponent<Image> ().color = Color.grey;
			milestoneBackButton.GetComponent<RectTransform> ().localScale = new Vector3 (1.1f, 1.1f, 1.1f);
		}
	}

	// Called from the back Button in the milestone menu
	public void EndMilestoneTutorial(){
		if (inTutorial) {
			StopAllCoroutines ();
			tutorialTextBox.SetActive (true);

			// Turn off the main menu Buttons
			ToggleButtonOff (menuButtons [(int)Buttons.AVATAR]);
			ToggleButtonOff (menuButtons [(int)Buttons.MENU]);

			// Turn on the camera
			ToggleButtonOn (menuButtons [(int)Buttons.CAMERA]);

			// Reset the Milestone Menu Buttons
			dictionaryButton.interactable = true;
			dictionaryButton.GetComponent<Image> ().color = Color.white;
			milestoneBackButton.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f);

			// Toggle on the cam arrow
			this.transform.GetChild((int)TutorialPanel.DOWN_ARROWS).transform.GetChild((int)Buttons.CAMERA).gameObject.SetActive(true);
			this.transform.GetChild((int)TutorialPanel.DOWN_ARROWS).transform.GetChild((int)Buttons.AVATAR).gameObject.SetActive(false);

			// Turn on the arrow pointing at the Milestone
			this.transform.GetChild ((int)TutorialPanel.MILESTONE_ARROW).gameObject.SetActive (true);

			// Reactivate the menu Buttons
			menuButtons [(int)Buttons.MENU].transform.GetChild (0).transform.GetChild (0).GetComponent<Button> ().interactable = true;
			menuButtons [(int)Buttons.MENU].transform.GetChild (0).transform.GetChild (0).GetComponent<Image> ().color = Color.white;
			menuButtons [(int)Buttons.MENU].transform.GetChild (0).transform.GetChild (1).GetComponent<Button> ().interactable = true;
			menuButtons [(int)Buttons.MENU].transform.GetChild (0).transform.GetChild (1).GetComponent<Image> ().color = Color.white;

			// Turn on the Button that will end the tutorial
			// this.transform.GetChild ((int)TutorialPanel.END_TUT_BUTTON).gameObject.SetActive (true);

			//Ensure that the milestone UI gets turned on
			milestoneUI.SetActive(true);

			// Update the header and body text
			UpdateText ("We're Done!", endTutorialMessage);
		}

	}

	// Called from the end tutorial button on the tutorial panel
	public void EndTutorial(){
		if (inTutorial) {
			// Turn on the arrow pointing at the Milestone
			this.transform.GetChild ((int)TutorialPanel.MILESTONE_ARROW).gameObject.SetActive (false);
			// Turn on the Button that will end the tutorial
			this.transform.GetChild ((int)TutorialPanel.END_TUT_BUTTON).gameObject.SetActive (false);
			// Turn on the restart tutorial Button
			resetTutorialButton.gameObject.SetActive (true);

			// Turn off the picture Box
			tutPictureBox.SetActive(false);

			this.gameObject.SetActive (false);
			ResetButtonsAndArrows ();
			inTutorial = false;
			pictureBoxParent.GetComponent<PictureBoxParent> ().ActivateChildren();
		}
	}

	void UpdateText(string headerMessage, string bodyMessage){
		this.transform.GetChild ((int)TutorialPanel.CONTENT).GetChild (0).GetComponent<Text> ().text = headerMessage;
		this.transform.GetChild((int)TutorialPanel.CONTENT).GetChild(1).GetComponent<Text>().text = bodyMessage;
	}

	public bool InTutorial(){
		return inTutorial;
	}

	public void ToggleOffTapImage(){
		this.transform.GetChild ((int)TutorialPanel.TAP_ICON).gameObject.SetActive (false);
	}

	IEnumerator CloseInstructions(){

		Debug.Log ("Closing Text Box.");

		yield return new WaitForSeconds (5);

		tutorialTextBox.SetActive (false);
	}
}
