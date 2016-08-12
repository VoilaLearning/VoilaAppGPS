using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MilestoneClass : MonoBehaviour {

	[SerializeField]string title;
	[SerializeField] Image fillBar;

	public void Start(){
		fillBar.fillAmount = 0;
		this.GetComponentInChildren<Text> ().text = title;
	}

	public void SetFillPercentage(float percentValue){
		// Debug.Log ("Setting Fill Perc: " + percentValue);
		fillBar.fillAmount += percentValue;
	}

	public string GetTitle(){
		return title;
	}

	public float GetFillPercentage(){
		return fillBar.fillAmount;
	}
}
