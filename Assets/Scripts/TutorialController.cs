using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Buttons {AVATAR = 0, CAMERA, MENU, COUNT};
public enum TutorialPanel {HEADER = 1, BODY, DOWN_ARROWS, TAP_ICON, MILESTONE_ARROW, END_TUT_BUTTON, COUNT};

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

	string introMessage = "With this app you will learn be able to explore your home, school and neighbourhood translate your world into french! To begin, click the button below! we will take a picture and then label it, en francias!";
	string labelPicMessage = "Now you have the chance to label up to 3 of the things you can recognize in your picture, En Francais! Go ahead, tap the screen and give it a try. When you are finished labeling your picture, hit the next button, then we can go see your shared picture.";
	string openAlbumMessage = "Now you can view the image you took with the tags you included, click on the numbered circle to view your photos!";
	string openSavedPictureMessage = "Now that we are in the photo album, you can click on your photo to see it and the tags you marked again, other student will be able to see your photo as well!";
	string wordTowardsAMilestoneMessage = "When taking pictures, you can gain points by working towards milestones we have set out for you, click on the button below to see all the milestones available";
	string selectAMilestoneMessage = "Choose any milestome in the list by clicking on the name. Then take pictures and create tags for that topic! Then you can show off you're hard work to the city, just like we did before!";
	string endTutorialMessage = "Now you can see the Milestone above in you game so you can remember what you are working towards! That is all for the tutoria, go on and get playing!";

	public bool inTutorial;

	// Use this for initialization
	void Start () {
		StartTutorial ();
	}

	void ResetButtonsAndArrows(){
		for (int i = 0; i < (int)Buttons.COUNT; i++) {
			menuButtons [i].interactable = true;
			menuButtons[i].gameObject.GetComponent<Image> ().color = Color.white;
			menuButtons[i].GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
			tutorialTextBox.transform.GetChild((int)TutorialPanel.DOWN_ARROWS).transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	void ToggleButtonOff(Button button){
		button.interactable = false;
		button.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f);
		button.GetComponent<Image> ().color = Color.grey;
	}

	void ToggleButtonOn(Button button){
		button.interactable = true;
		button.GetComponent<RectTransform> ().localScale = new Vector3 (1.5f, 1.5f, 1.5f);
		button.GetComponent<Image> ().color = Color.white;
	}

	// called in Start()
	public void StartTutorial(){
		inTutorial = true;
		tutorialTextBox.SetActive (true);
		// Reset the buttons and arrows before beginning a new step in the Tutorial
		ResetButtonsAndArrows();
		// Set the header Text
		tutorialTextBox.transform.GetChild((int)TutorialPanel.HEADER).GetComponentInChildren<Text>().text = "Bienvenue!";
		// Set the body Text
		tutorialTextBox.transform.GetChild((int)TutorialPanel.BODY).GetComponentInChildren<Text>().text = introMessage;
		// Ensure the other buttons cannot be pressed
		ToggleButtonOff(menuButtons[(int)Buttons.AVATAR]);
		ToggleButtonOff(menuButtons[(int)Buttons.MENU]);
		// Make sure that the camera button is more "Obvious"
		ToggleButtonOn(menuButtons[(int)Buttons.CAMERA]);
		// Turn on the center arrow
		tutorialTextBox.transform.GetChild((int)TutorialPanel.DOWN_ARROWS).transform.GetChild((int)Buttons.CAMERA).gameObject.SetActive(true);
		resetTutorialButton.gameObject.SetActive (false);
	}

	// Called in EtecertraPictureGameController.cs
	public void CloseCamera(){
		if (inTutorial) {
			tutorialTextBox.SetActive (true);
			ResetButtonsAndArrows ();
			// Turn on the "Tap" UI
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.TAP_ICON).gameObject.SetActive (true);
			// Set the Text Boxes
			tutorialTextBox.transform.GetChild((int)TutorialPanel.HEADER).GetComponentInChildren<Text>().text = "Label Your Picture!";
			tutorialTextBox.transform.GetChild((int)TutorialPanel.BODY).GetComponentInChildren<Text>().text = labelPicMessage;

			// Disable the back Button and Enlarge the Save Button in the Picture Panel
			savePhotoButton.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
			leavePicturePanelButton.interactable = false;
			leavePicturePanelButton.GetComponent<Image> ().color = Color.grey;
		}
	}

	// Called in SavePicture.cs in CreateTutorialSave()
	public void TempPhotoBox(){
		if (inTutorial) {
			// Reset the buttons in the Picture Panel
			savePhotoButton.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
			leavePicturePanelButton.interactable = true;
			leavePicturePanelButton.GetComponent<Image> ().color = Color.white;

			// When the player has saved their image - turn off all other buttons and create a temp photo box - we will delete this after the tutorial
			tutPictureBox.SetActive(true);
			ResetButtonsAndArrows ();
			ToggleButtonOff (menuButtons[(int)Buttons.AVATAR]);
			ToggleButtonOff (menuButtons[(int)Buttons.CAMERA]);
			ToggleButtonOff (menuButtons[(int)Buttons.MENU]);
			// Turn off the click icon on the Tutorial panel 
			tutorialTextBox.transform.GetChild((int)TutorialPanel.TAP_ICON).gameObject.SetActive(false);

			// Toggle on the text box and tell player to open the album
			tutorialTextBox.SetActive (true);
			tutorialTextBox.transform.GetChild((int)TutorialPanel.HEADER).GetComponentInChildren<Text>().text = "Open Your Album!";
			tutorialTextBox.transform.GetChild((int)TutorialPanel.BODY).GetComponentInChildren<Text>().text = openAlbumMessage;
		}
	}

	// Called in PictureBoxController.cs in OnMouseDown()
	public void OpenPhotoBox(){

		if (inTutorial) {
			// Set the header and body text of the message
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.HEADER).GetComponentInChildren<Text> ().text = "Look at your Photos!";
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.BODY).GetComponentInChildren<Text> ().text = openSavedPictureMessage;
		}
	}

	// Called from the Back Button in the Photo Album Scroll View
	public void StartMilestoneTutorial(){
		if (inTutorial) {
			// Remove the Image from the photo album
			photoAlbum.GetComponent<AlbumController>().EmptyAlbum();
			// Delete the Temp Photo Box
			tutPictureBox.SetActive(false);
			ResetButtonsAndArrows ();

			// Turn on the arrow above the milestone container
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.DOWN_ARROWS).transform.GetChild ((int)Buttons.MENU).gameObject.SetActive (true);

			ToggleButtonOff (menuButtons [(int)Buttons.AVATAR]);
			ToggleButtonOff (menuButtons [(int)Buttons.CAMERA]);

			// Ensure they can only Select the Dictionary Button
			menuButtons [(int)Buttons.MENU].transform.GetChild (0).transform.GetChild (0).GetComponent<Button> ().interactable = false;
			menuButtons [(int)Buttons.MENU].transform.GetChild (0).transform.GetChild (0).GetComponent<Image> ().color = Color.grey;
			menuButtons [(int)Buttons.MENU].transform.GetChild (0).transform.GetChild (1).GetComponent<Button> ().interactable = false;
			menuButtons [(int)Buttons.MENU].transform.GetChild (0).transform.GetChild (1).GetComponent<Image> ().color = Color.grey;

			// Set the header and the message
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.HEADER).GetComponentInChildren<Text> ().text = "Work Towards a Milestone!";
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.BODY).GetComponentInChildren<Text> ().text = wordTowardsAMilestoneMessage;
		}
	}

	// Called from the Menu Options Button
	public void OpenMenuButtons(){
		// Turn off the arrow in order to advance
		if (inTutorial) {
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.DOWN_ARROWS).transform.GetChild ((int)Buttons.MENU).gameObject.SetActive (false);
		}
	}

	// Called from the milestone button
	public void OpenMilestoneContainer(){
		if (inTutorial) {
			// Set the Header and the Body
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.HEADER).GetComponentInChildren<Text> ().text = "Select a Milestone!";
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.BODY).GetComponentInChildren<Text> ().text = selectAMilestoneMessage;

			// Toggle off the dictionary Button
			dictionaryButton.interactable = false;
			dictionaryButton.GetComponent<Image> ().color = Color.grey;
			milestoneBackButton.GetComponent<RectTransform> ().localScale = new Vector3 (1.5f, 1.5f, 1.5f);
		}
	}

	// Called from the back Button in the milestone menu
	public void EndMilestoneTutorial(){
		if (inTutorial) {
			// Turn off the main menu Buttons
			ToggleButtonOff (menuButtons [(int)Buttons.AVATAR]);
			ToggleButtonOff (menuButtons [(int)Buttons.CAMERA]);
			ToggleButtonOff (menuButtons [(int)Buttons.MENU]);

			// Reset the Milestone Menu Buttons
			dictionaryButton.interactable = true;
			dictionaryButton.GetComponent<Image> ().color = Color.white;
			milestoneBackButton.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1.5f);

			// Turn on the arrow pointing at the Milestone
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.MILESTONE_ARROW).gameObject.SetActive (true);
			// Turn on the Button that will end the tutorial
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.END_TUT_BUTTON).gameObject.SetActive (true);
			// Update the header and body text
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.HEADER).GetComponentInChildren<Text> ().text = "We're Done!";
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.BODY).GetComponentInChildren<Text> ().text = endTutorialMessage;
		}

	}

	// Called from the end tutorial button on the tutorial panel
	public void EndTutorial(){
		if (inTutorial) {
			// Turn on the arrow pointing at the Milestone
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.MILESTONE_ARROW).gameObject.SetActive (false);
			// Turn on the Button that will end the tutorial
			tutorialTextBox.transform.GetChild ((int)TutorialPanel.END_TUT_BUTTON).gameObject.SetActive (false);
			// Turn on the restart tutorial Button
			resetTutorialButton.gameObject.SetActive (true);

			tutorialTextBox.SetActive (false);
			ResetButtonsAndArrows ();
			inTutorial = false;
		}
	}

	public bool InTutorial(){
		return inTutorial;
	}
}
