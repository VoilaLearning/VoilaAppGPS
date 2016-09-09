using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Buttons { BACK = 0, CAMERA, MILESTONES, COUNT };
public enum TutorialPanel { CONTENT = 0, DOWN_ARROWS, TAP_ICON, MILESTONE_ARROW, END_TUT_BUTTON, PANEL_COUNT };

public enum TutorialState { INTRO = 0, TAKE_PHOTO, TAG_PHOTO, SAVE_PHOTO, SHOW_RESULTS, JASONS_PIC, MILESTONES, COMPLETE, COUNT };

[DisallowMultipleComponent]
public class TutorialController : MonoBehaviour {

	[SerializeField] GameObject tutorialTextBox; 
	[SerializeField] GameObject tutPictureBox;
	[SerializeField] GameObject photoAlbum;
	[SerializeField] GameObject picturePanel;
	[SerializeField] GameObject pictureBoxParent;
	[SerializeField] GameObject milestoneUI;
	[SerializeField] GameObject[] tutorialPanels;

	[SerializeField] Button[] menuButtons;
	[SerializeField] Button resetTutorialButton;
	[SerializeField] Button[] picturePanelButtons;

	bool inTutorial;
	TutorialState currentState = TutorialState.INTRO;

	// Use this for initialization
	void Start () {
		// Debug.Log ("Starting Tut");
		// StartTutorial ();
		// TagPhoto(TutorialState.TAKE_PHOTO);
		// inTutorial = true;
		pictureBoxParent.GetComponent<PictureBoxParent> ().DeactivateChildren();
	}

	void Update(){
		if (Input.anyKey && (currentState == TutorialState.INTRO || currentState == TutorialState.SHOW_RESULTS)) {
			AdvanceTutorial ();
		}
	}

	void ToggleButtonOff(Button button){
		button.gameObject.SetActive (true);
		button.interactable = false;
		button.GetComponent<Image> ().color = Color.grey;
	}

	void ToggleButtonOn(Button button){
		button.gameObject.SetActive (true);
		button.interactable = true;
		button.GetComponent<Image> ().color = Color.white;
	}

	void ToggleAllButtonsOn(Button[] buttons){
		foreach (Button button in buttons) {
			button.gameObject.SetActive (true);
		}
	}

	void ToggleAllButtonsOff(Button[] buttons){
		foreach (Button button in buttons) {
			button.gameObject.SetActive (false);
		}
	}

	public void AdvanceTutorial(){
		// Debug.Log ("Advancing");
		if (inTutorial) {
			switch (currentState) {
			case TutorialState.INTRO:
				TakePhoto ();
				break;
			case TutorialState.TAKE_PHOTO:
				TagPhoto (currentState);
				break;
			case TutorialState.TAG_PHOTO:
				SavePhoto ();
				break;
			case TutorialState.SAVE_PHOTO:
				ShowResults ();
				break;
			case TutorialState.SHOW_RESULTS:
				JasonsPhoto ();
				break;
			case TutorialState.JASONS_PIC:
				Milestones ();
				break;
			case TutorialState.MILESTONES:
				EndTutorial ();
				break;
			}
		}
	}

	public void RevertTutorial(){
		if (inTutorial) {
			switch (currentState) {
			case TutorialState.INTRO:
			// End Tutorial???
				break;
			case TutorialState.TAKE_PHOTO:
				StartTutorial ();
				break;
			case TutorialState.TAG_PHOTO:
				TakePhoto ();
				break;
			case TutorialState.SAVE_PHOTO:
				TakePhoto ();
				break;
			case TutorialState.SHOW_RESULTS:
			// TagPhoto();
				break;
			case TutorialState.JASONS_PIC:
				TagPhoto (currentState);
				break;
			case TutorialState.MILESTONES:
				JasonsPhoto ();
				break;
			}
		}
	}

	void TogglePanel(int index){
		// Debug.Log ("toggling panel: " + index);
		for (int i = 0; i < tutorialPanels.Length; i++) {
			if (i == index) {
				// Debug.Log ("Found Panel");
				tutorialPanels [i].SetActive (true);
			} else {
				tutorialPanels [i].SetActive (false);
			}
		}
	}

	// called in Start()
	public void StartTutorial(){
		inTutorial = true;
		this.gameObject.SetActive (true);
		resetTutorialButton.gameObject.SetActive (false);
		currentState = TutorialState.INTRO;
		TogglePanel ((int)currentState);

		// Reset the Milestone and make sure it is the proper Milestone fot the tutorial
		milestoneUI.GetComponent<MilestoneController>().SetCurrentMilestone(5);
		milestoneUI.GetComponent<MilestoneController>().ResetTutorialFill();
		milestoneUI.SetActive (true);

		// Turn off all buttons
		ToggleAllButtonsOff(menuButtons);
	}

	void TakePhoto(){
		currentState = TutorialState.TAKE_PHOTO;
		TogglePanel ((int)currentState);
		ToggleAllButtonsOn (menuButtons);
		// Ensure that the player can only select the camera or the back button
		ToggleButtonOff (menuButtons[(int)Buttons.MILESTONES]);

		// Toggle off the picture panel and tap icon in case the player is digressing in the tutorial
		if(picturePanel.activeSelf) { picturePanel.SetActive(false); }

		// Remove any Input boxes left behind
		picturePanel.GetComponent<SavePicture>().ResetPicturePanel();
	}

	void TagPhoto(TutorialState lastState){
		currentState = TutorialState.TAG_PHOTO;
		TogglePanel ((int)currentState);
		milestoneUI.SetActive (false);

		// Turn off both buttons in picture panel
		ToggleAllButtonsOff(picturePanelButtons);

		// Turn on the picture panel and the input fields if you are digressing through the tutorial
		if (!picturePanel.activeSelf) {
			picturePanel.SetActive (true);
		}
		picturePanel.GetComponent<SavePicture> ().ToggleOnInputs ();

		// Reset the tutorial milestone and delete the last photo taken
		milestoneUI.GetComponent<MilestoneController> ().ResetTutorialFill();

		if (lastState == TutorialState.JASONS_PIC) picturePanel.GetComponent<SavePicture> ().DeleteLastTutorialPhoto ();
	}

	void SavePhoto(){
		currentState = TutorialState.SAVE_PHOTO;
		TogglePanel ((int)currentState);

		//Turn on the save pic button
		ToggleButtonOn(picturePanelButtons[1]);
	}

	public void ShowResults(){
		// Debug.Log ("Showing Results");
		currentState = TutorialState.SHOW_RESULTS;
		TogglePanel ((int)currentState);
		milestoneUI.SetActive (true);
		milestoneUI.GetComponent<MilestoneController> ().TutorialFill ();

		// Turn back on the buttons in the picture panel
		ToggleAllButtonsOn(picturePanelButtons);
		ToggleAllButtonsOff (menuButtons);
	}

	public void JasonsPhoto(){
		// Debug.Log ("Jasons Pic");
		currentState = TutorialState.JASONS_PIC;
		TogglePanel ((int)currentState);
		milestoneUI.GetComponent<MilestoneController> ().QuickFill ();
		milestoneUI.SetActive (false);

		// Turn on the picture boxes
		pictureBoxParent.GetComponent<PictureBoxParent>().ActivateChildren();

		// Dont allow the player to use them though
		ToggleButtonOff (menuButtons[(int)Buttons.MILESTONES]);
		ToggleButtonOff (menuButtons [(int)Buttons.CAMERA]);
	}

	public void Milestones(){
		currentState = TutorialState.MILESTONES;
		// Turn off all panels
		foreach(GameObject panel in tutorialPanels){
			panel.SetActive (false);
		}
		ToggleButtonOn (menuButtons[(int)Buttons.MILESTONES]);
		ToggleButtonOff (menuButtons [(int)Buttons.CAMERA]);
	}

	public void ShowMilestoneMessage(){
		if (inTutorial) {
			TogglePanel ((int)currentState);
		}
	}

	public void EndTutorial(){
		Debug.Log ("End Tutorial");
		currentState = TutorialState.COMPLETE;
		ToggleAllButtonsOn (menuButtons);

		ToggleButtonOn (menuButtons [(int)Buttons.CAMERA]);
		inTutorial = false;
		this.gameObject.SetActive (false);
		resetTutorialButton.gameObject.SetActive (true);
		// remove left over data from the picture panel
		picturePanel.GetComponent<SavePicture>().ResetPicturePanel();
	}
		
	public bool InTutorial(){
		return inTutorial;
	}

	public void ToggleOffTapImage(){
		this.transform.GetChild ((int)TutorialPanel.TAP_ICON).gameObject.SetActive (false);
	}

	public TutorialState GetCurrentState(){
		return currentState;
	}
		
	IEnumerator CloseInstructions(){

		Debug.Log ("Closing Text Box.");

		yield return new WaitForSeconds (5);

		tutorialTextBox.SetActive (false);
	}
}
