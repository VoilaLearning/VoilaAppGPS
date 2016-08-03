using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickForTextBox : MonoBehaviour, IPointerClickHandler {

	[SerializeField] InputField textInput;

	public void OnPointerClick(PointerEventData eventData){
		Debug.Log ("Click click!");
		// Find all other text box
		InputField[] otherFields = GameObject.FindObjectsOfType<InputField> ();
		// close them 
		if (otherFields.Length > 0) {
			for (int i = 0; i < otherFields.Length; i++) {
				if (otherFields [i].GetComponent<InputFieldController> () != null ) {
					if (otherFields [i].GetComponent<InputFieldController> ().IsPinned () == false) {
						otherFields [i].GetComponent<InputFieldController> ().FinishInput ();
					}
				}
			}
		}

		InputField newInput = Instantiate (textInput, Vector3.zero, Quaternion.identity) as InputField;
		newInput.transform.SetParent (this.transform.parent);
		newInput.GetComponent<RectTransform> ().position = Input.mousePosition;
		newInput.transform.localScale = Vector3.one;
	}
}
