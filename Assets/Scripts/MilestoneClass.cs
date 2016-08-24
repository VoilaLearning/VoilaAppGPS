using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MilestoneClass : MonoBehaviour {
    	
    [SerializeField] Image fillBar;
    string title;
    string challenge;

	bool filled;

	public void Start(){
		filled = false;
		fillBar.fillAmount = 0;
	}

    void OnEnable () {

        this.GetComponentInChildren<Text>().text = title;
    }

	public void SetFillPercentage(float percentValue){
		// Debug.Log ("Setting Fill Perc: " + percentValue);
		fillBar.fillAmount += percentValue;
	}

	public void Fill(){
		fillBar.fillAmount = 1;
		filled = true;
	}

	public void Empty(){
		fillBar.fillAmount = 0;
		filled = false;
	}

	public string GetTitle(){
		return title;
	}

	public float GetFillPercentage(){
		return fillBar.fillAmount;
	}

    public string GetChallenge () {

        return challenge;
    }

    public void SetTitle (string newTitle) {

        title = newTitle;
        this.GetComponentInChildren<Text>().text = title;
    }

    public void SetChallenge(string newChallenge) {

        challenge = newChallenge;
    }
}
