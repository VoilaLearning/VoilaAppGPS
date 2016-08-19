using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MilestoneClass : MonoBehaviour {
    	
    [SerializeField] Image fillBar;
    string title;
    string challenge;

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

    public void SetTitle (string newTitle) {

        title = newTitle;
    }

    public void SetChallenge(string newChallenge) {

        challenge = newChallenge;
    }
}
