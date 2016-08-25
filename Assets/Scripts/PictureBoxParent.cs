using UnityEngine;
using System.Collections;

public class PictureBoxParent : MonoBehaviour {

	TutorialController tutorialController;

	void Start(){
		// tutorialController = GameObject.FindGameObjectWithTag ("Tutorial Controller").GetComponentInChildren<TutorialController> ();
	}

	public void ActivateChildren () {

		// Debug.Log ("Activate Children");
		this.gameObject.SetActive (true);

		/*for (int i = 0; i < this.transform.childCount; i++) {

			this.transform.GetChild (i).gameObject.SetActive (true);
		}*/
	}

	public void DeactivateChildren () {

		// Debug.Log ("Deactivate Children");
		this.gameObject.SetActive (false);

		/*for (int i = 0; i < this.transform.childCount; i++) {

			this.transform.GetChild (i).gameObject.SetActive (false);
		}*/
	}
}
