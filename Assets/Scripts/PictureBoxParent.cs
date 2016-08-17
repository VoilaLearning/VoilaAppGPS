using UnityEngine;
using System.Collections;

public class PictureBoxParent : MonoBehaviour {

	public void ActivateChildren () {

		for (int i = 0; i < this.transform.childCount; i++) {

			this.transform.GetChild (i).gameObject.SetActive (true);
		}
	}

	public void DeactivateChildren () {

		for (int i = 0; i < this.transform.childCount; i++) {

			this.transform.GetChild (i).gameObject.SetActive (false);
		}
	}
}
