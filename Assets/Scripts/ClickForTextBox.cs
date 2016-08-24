using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickForTextBox : MonoBehaviour, IPointerClickHandler {

	TutorialController tutorialController;

	// [SerializeField] PictureWordGame pictureWordGame;
	[SerializeField] InputField textInput;

	int maxWords = 3;
	int maxPoints = 1500;
	int points = 0;

	void Start(){

		tutorialController = GameObject.FindGameObjectWithTag ("Tutorial Controller").GetComponentInChildren<TutorialController> ();
	}

	public void OnPointerClick(PointerEventData eventData){

		// Debug.Log ("Click click!");
		// Find all other text box
		InputField[] otherFields = GameObject.FindObjectsOfType<InputField> ();
		// close them 
		for (int i = 0; i < otherFields.Length; i++) {
			if (otherFields [i].GetComponent<InputFieldController> ().IsPinned () == false) {
				if (otherFields [i].GetComponent<InputField> ().text == null || otherFields [i].GetComponent<InputField> ().text == "") {
					Destroy (otherFields [i].gameObject);
				} else {
					otherFields [i].GetComponent<InputFieldController> ().FinishInput ();	
				}
			}
		}

		if (otherFields.Length < maxWords) {
			InputField newInput = Instantiate (textInput, Vector3.zero, Quaternion.identity) as InputField;
			newInput.transform.SetParent (this.transform.parent);
			newInput.GetComponent<RectTransform> ().position = Input.mousePosition;
			newInput.transform.localScale = Vector3.one;
			CheckPosition (newInput);
		}

		/*if (tutorialController.InTutorial ()) {
			Debug.Log ("Advancing Tut");
			tutorialController.AdvanceTutorial ();
		}*/
	}

	void CheckPosition(InputField newWord){

		// Debug.Log ("Position: " + newWord.GetComponent<RectTransform> ().localPosition.x);
		// Debug.Log ("Half Size: " + newWord.GetComponent<RectTransform> ().sizeDelta.x / 2f);
		// Debug.Log("Screen Width: " + Screen.width);
		// Debug.Log("Screen Width: " + (this.GetComponentInParent<CanvasScaler>().referenceResolution.x / 2));

		// Check Right
		if (newWord.GetComponent<RectTransform> ().localPosition.x + (newWord.GetComponent<RectTransform> ().sizeDelta.x / 2f) > (this.GetComponentInParent<CanvasScaler>().referenceResolution.x / 2)) {
			// float newX = newWord.GetComponent<RectTransform> ().localPosition.x + (newWord.GetComponent<RectTransform> ().sizeDelta.x / 2f) - (this.GetComponentInParent<CanvasScaler>().referenceResolution.x / 2);
			float newX = (this.GetComponentInParent<CanvasScaler>().referenceResolution.x / 2) - (newWord.GetComponent<RectTransform> ().sizeDelta.x / 1.5f);
			newWord.transform.localPosition = new Vector3 (newX, newWord.GetComponent<RectTransform> ().localPosition.y, newWord.GetComponent<RectTransform> ().localPosition.z);
			// Debug.Log ("Right Move: " + newX);
		} 
		// Check Left
		else if (newWord.GetComponent<RectTransform> ().localPosition.x - (newWord.GetComponent<RectTransform> ().sizeDelta.x / 2f) < (this.GetComponentInParent<CanvasScaler>().referenceResolution.x / -2)) {
			
			float newX = newWord.GetComponent<RectTransform> ().localPosition.x + (newWord.GetComponent<RectTransform> ().sizeDelta.x / 2f);
			newWord.transform.localPosition = new Vector3 (newX, newWord.GetComponent<RectTransform> ().localPosition.y, newWord.GetComponent<RectTransform> ().localPosition.z);
			// Debug.Log ("Left Move: " + newX);
		} 

		// Check Top
		/*if (newWord.GetComponent<RectTransform> ().localPosition.y + (newWord.GetComponent<RectTransform> ().sizeDelta.y) > Screen.height){
			Debug.Log ("At the Top");
			float newY = newWord.GetComponent<RectTransform> ().localPosition.y - (newWord.GetComponent<RectTransform> ().sizeDelta.y);
			newWord.transform.localPosition = new Vector3 (newWord.GetComponent<RectTransform> ().localPosition.x, newY,  newWord.GetComponent<RectTransform> ().localPosition.z);
		}*/ 
	}
}
