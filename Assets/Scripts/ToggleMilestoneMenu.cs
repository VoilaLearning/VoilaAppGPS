using UnityEngine;
using System.Collections;

public class ToggleMilestoneMenu : MonoBehaviour {

	public void OpenContainer(){
		this.GetComponent<RectTransform> ().localScale = new Vector3(1, 1, 1);
	}

	public void CloseContainer(){
		this.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
	}
}
