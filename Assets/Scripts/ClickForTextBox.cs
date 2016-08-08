using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickForTextBox : MonoBehaviour, IPointerClickHandler {

	// [SerializeField] PictureWordGame pictureWordGame;
	[SerializeField] InputField textInput;

	int maxWords = 3;

	public void OnPointerClick(PointerEventData eventData){

		Debug.Log ("Click click!");
		// Find all other text box
		InputField[] otherFields = GameObject.FindObjectsOfType<InputField> ();
		// close them 
		for (int i = 0; i < otherFields.Length; i++) {
			if (otherFields [i].GetComponent<InputFieldController> ().IsPinned () == false) {
				otherFields [i].GetComponent<InputFieldController> ().FinishInput ();	
			}
		}

		if (otherFields.Length < 3) {
			InputField newInput = Instantiate (textInput, Vector3.zero, Quaternion.identity) as InputField;
			newInput.transform.SetParent (this.transform.parent);
			newInput.GetComponent<RectTransform> ().position = Input.mousePosition;
			newInput.transform.localScale = Vector3.one;
			CheckPosition (newInput);
		}
	}

	void CheckPosition(InputField newWord){


		Debug.Log ("Position: " + newWord.GetComponent<RectTransform> ().localPosition.x);
		Debug.Log ("Half Size: " + newWord.GetComponent<RectTransform> ().sizeDelta.x / 2f);
		Debug.Log ("Position and half width: " + (newWord.GetComponent<RectTransform> ().localPosition.x + (newWord.GetComponent<RectTransform> ().sizeDelta.x / 2f)));
		Debug.Log ("Screen width: " + Screen.width * 0.75f);
		Debug.Log ("Canvas Resolution:" + this.GetComponentInParent<CanvasScaler> ().referenceResolution.x);

		// Check Right
		if (newWord.GetComponent<RectTransform> ().localPosition.x + (newWord.GetComponent<RectTransform> ().sizeDelta.x) > Screen.width) {
			float newX = newWord.GetComponent<RectTransform> ().localPosition.x + (newWord.GetComponent<RectTransform> ().sizeDelta.x / 2f) - Screen.width;
			newWord.transform.localPosition = new Vector3 (newX, newWord.GetComponent<RectTransform> ().localPosition.y, newWord.GetComponent<RectTransform> ().localPosition.z);
		} 
		// Check Left
		else if (newWord.GetComponent<RectTransform> ().localPosition.x - (newWord.GetComponent<RectTransform> ().sizeDelta.x / 2f) < 0) {
			float newX = newWord.GetComponent<RectTransform> ().localPosition.x + (newWord.GetComponent<RectTransform> ().sizeDelta.x / 2f);
			newWord.transform.localPosition = new Vector3 (newX, newWord.GetComponent<RectTransform> ().localPosition.y, newWord.GetComponent<RectTransform> ().localPosition.z);
		} 

		// Check Top
		/*if (newWord.GetComponent<RectTransform> ().localPosition.y + (newWord.GetComponent<RectTransform> ().sizeDelta.y) > Screen.height){
			Debug.Log ("At the Top");
			float newY = newWord.GetComponent<RectTransform> ().localPosition.y - (newWord.GetComponent<RectTransform> ().sizeDelta.y);
			newWord.transform.localPosition = new Vector3 (newWord.GetComponent<RectTransform> ().localPosition.x, newY,  newWord.GetComponent<RectTransform> ().localPosition.z);
		}*/ 
	}
}
