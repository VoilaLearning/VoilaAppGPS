using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputFieldController : MonoBehaviour {

	[SerializeField] GameObject destroyButton;
	[SerializeField] GameObject editButton;
	[SerializeField] GameObject pointValueUI;

	bool closed;
	bool pinned = false;
	int index = 0;
	int wordPointValue;

	void Start(){

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

		StartCoroutine (ShowPoints ());
	}

	public void OpenInputField(){
			InputField[] tempArray = GameObject.FindObjectsOfType<InputField> ();
			for (int i = 0; i < tempArray.Length; i++) {
			if (tempArray [i].GetComponent<InputFieldController> ().IsClosed () == false && tempArray [i].GetComponent<InputFieldController> ().IsPinned () == false) {
					tempArray [i].GetComponent<InputFieldController> ().FinishInput ();
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

	IEnumerator ShowPoints(){

		pointValueUI.GetComponentInChildren<Text> ().text = wordPointValue.ToString () + " points!";
		pointValueUI.SetActive (true);

		yield return new WaitForSeconds(1);

		pointValueUI.SetActive (false);
	}
}
