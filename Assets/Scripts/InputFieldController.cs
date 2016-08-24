using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputFieldController : MonoBehaviour {

	// PictureWordGame pictureWordGame;
	TutorialController tutorialController;

	[SerializeField] GameObject destroyButton;
	[SerializeField] GameObject editButton;
	[SerializeField] GameObject pointValueUI;

	bool closed;
	bool pointsShown = false;
	bool pinned = false;
	int index = 0;
	int points = 500;

	void Start(){
		tutorialController = GameObject.FindGameObjectWithTag ("Tutorial Controller").GetComponentInChildren<TutorialController> ();
		// pictureWordGame = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PictureWordGame>();
	}

	public void DestroyField(){
		Destroy (this.gameObject);
	}

	public void FinishInput(){
		// Make it no longer interractable
		this.GetComponent<InputField> ().interactable = false;
		// Shrink Box Down 0.5, 0.5, 0.5
		this.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		// Deactivate Destroy Button
		destroyButton.SetActive (false);
		// Activate the re-open Button
		editButton.SetActive (true);
		closed = true;

		// wordPointValue = pictureWordGame.CheckWord (this.GetComponent<InputField> ());
		if (!pointsShown && (this.GetComponent<InputField>().text != null || this.GetComponent<InputField>().text != "")) StartCoroutine (ShowPoints ());

		if (tutorialController.GetCurrentState() == TutorialState.TAG_PHOTO) {
			tutorialController.AdvanceTutorial ();
		}
	}

	public void OpenInputField(){
			InputField[] tempArray = GameObject.FindObjectsOfType<InputField> ();
			for (int i = 0; i < tempArray.Length; i++) {
			if (tempArray [i].GetComponent<InputFieldController> ().IsClosed () == false && tempArray [i].GetComponent<InputFieldController> ().IsPinned () == false) {
					tempArray [i].GetComponent<InputFieldController> ().FinishInput ();
					pinned = true;
				}
			}

			// Make it interractable
			this.GetComponent<InputField> ().interactable = true;
			// Increase size to original
			this.transform.localScale = new Vector3 (1, 1, 1);
			// activate Destroy Button
			destroyButton.SetActive (true);
			// deactivate the re-open Button
			editButton.SetActive (false);
			closed = false;
	}

	public bool IsClosed(){
		return closed;
	}

	public void SetIndex(int newIndex){
		index = newIndex;
	}

	public bool IsPinned(){
		return pinned;
	}

	public void PinWord(){
		// Deactivate Destroy Button
		destroyButton.SetActive(false);
		// Activate the re-open Button
		editButton.SetActive(false);
		pinned = true;
	}

	public string GetInput(){
		return this.GetComponent<InputField> ().text;
	}

	IEnumerator ShowPoints(){

		// Debug.Log ("Showing Points");
		pointsShown = true;

		pointValueUI.GetComponentInChildren<Text> ().text = "500 points!";
		pointValueUI.SetActive (true);

		yield return new WaitForSeconds(1);

		pointValueUI.SetActive (false);
	}
}
