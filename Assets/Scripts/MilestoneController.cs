using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MilestoneController : MonoBehaviour {

	[SerializeField] GameObject milestoneContainer; // Content GameObject in the milestone menu
	[SerializeField] Text titleUI;
	[SerializeField] Image fillBarUI;
    [SerializeField] Text challengeUI;

	MilestoneClass currentMilestone;
	GameObject[] milestones;

	// Use this for initialization
	void Start () {
		// Declare and fill the milestone array
		this.gameObject.SetActive(false);
		milestones = new GameObject[14];
		for (int i = 0; i < milestoneContainer.transform.childCount; i++) {
			milestones [i] = milestoneContainer.transform.GetChild (i).gameObject;
		}

		// FillMilestone (milestones [0].GetComponent<MilestoneClass>());
	}

	void FillMilestone(MilestoneClass selectedMilestone){
		// Set this as the new chosen Milestone!
		this.gameObject.SetActive(true);
		currentMilestone = selectedMilestone;
		titleUI.text = currentMilestone.GetTitle ();
		fillBarUI.fillAmount = currentMilestone.GetFillPercentage ();
        challengeUI.text = currentMilestone.GetChallenge();
	}

	public void SelectMilestone(MilestoneClass selectedMilestone) {
		if (currentMilestone == null || selectedMilestone.GetTitle () != currentMilestone.GetTitle ()) {
			FillMilestone (selectedMilestone);
		}
		else {
			// Deselect the milestone
			currentMilestone = null;
			this.gameObject.SetActive(false);
		} 
	}

	public void IncreaseMilestonePercentage( float increaseValue ){
	
		// Update the milestone UI in the main menu
		if(fillBarUI.fillAmount < 1){
			fillBarUI.fillAmount += increaseValue;
		}

		// Update the actual milestone container in the milestone menu
		for (int i = 0; i < milestones.Length; i++) {
			// Find the current milestone
			if(currentMilestone != null && currentMilestone.GetTitle() == milestones[i].GetComponent<MilestoneClass>().GetTitle()){
				// Debug.Log ("Found a match! " + currentMilestone.GetTitle());
				// Set the Fill
				milestones[i].GetComponent<MilestoneClass>().SetFillPercentage(increaseValue);
				// Debug.Log ("Fill Bar Value: " + milestones[i].GetComponent<MilestoneClass>().GetFillPercentage());
			}
		}
	}

	public MilestoneClass GetCurrentMilestone(){
		return currentMilestone;
	}
}
