using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickForTextBox : MonoBehaviour, IPointerClickHandler {

	[SerializeField] InputField textInput;

	int maxWords = 3;
	bool foundSticker = false;

	public void OnPointerClick(PointerEventData eventData){

		// Reset the check for stickers boolean
		foundSticker = false;

		// Check for sticker
		CheckForSticker();

		// Find all other text box
		if (!foundSticker) {
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
				newInput.ActivateInputField ();
			}
		}
	}

	void CheckPosition(InputField newWord){
		// Check Right
		if (newWord.GetComponent<RectTransform> ().localPosition.x + (newWord.GetComponent<RectTransform> ().sizeDelta.x / 2f) > (this.GetComponentInParent<CanvasScaler>().referenceResolution.x / 2)) {
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
	}

	void CheckForSticker(){
		GameObject[] stickers = GameObject.FindGameObjectsWithTag ("Sticker");

		if (stickers != null) {
			Debug.Log ("FoundSomeStickers.");
			foreach (GameObject sticker in stickers) {
				if (sticker.GetComponent<StickerController> ().CheckMouseOver () == true) {
					Debug.Log ("Found Sticker");
					foundSticker = true;
					sticker.GetComponent<StickerController> ().SetMouseOver (false);
				}
			}
		}
	}
}
