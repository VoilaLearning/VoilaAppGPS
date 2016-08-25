using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MilestoneController : MonoBehaviour {

	[SerializeField] GameObject milestoneContainer; // Content GameObject in the milestone menu
	[SerializeField] GameObject particles;
	[SerializeField] Text titleUI;
	[SerializeField] Image fillBarUI;
    [SerializeField] Text challengeUI;

	MilestoneClass currentMilestone;
	GameObject[] milestones;

	// Use this for initialization
	void Start () {
        milestones = new GameObject[milestoneContainer.transform.childCount];
		for (int i = 0; i < milestoneContainer.transform.childCount; i++) {
			milestones [i] = milestoneContainer.transform.GetChild (i).gameObject;
		}

		// For the tutorial
		FillMilestone (milestones [5].GetComponent<MilestoneClass>());
	}

	void FillMilestone(MilestoneClass selectedMilestone){
		// Set this as the new chosen Milestone!
		this.gameObject.SetActive(true);
		currentMilestone = selectedMilestone;
		titleUI.text = currentMilestone.GetTitle ();
		fillBarUI.fillAmount = currentMilestone.GetFillPercentage ();
        challengeUI.text = currentMilestone.GetChallenge();
	}

    public void UpdateMilestone() {

        if (currentMilestone) {
            titleUI.text = currentMilestone.GetTitle();
            fillBarUI.fillAmount = currentMilestone.GetFillPercentage();
            challengeUI.text = currentMilestone.GetChallenge();
        }
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
				// Set the Fill
				milestones[i].GetComponent<MilestoneClass>().SetFillPercentage(increaseValue);
			}
		}
	}

	public void ResetTutorialFill(){
		StopAllCoroutines ();
		fillBarUI.fillAmount = 0;
		currentMilestone.Empty ();

		// Reset the particles position
		float newX = (1080 * fillBarUI.fillAmount) - 540;
		particles.transform.localPosition = new Vector3 (newX, 0, 0);
	}

	public void TutorialFill(){
		particles.SetActive (true);
		currentMilestone.Fill ();
		StartCoroutine (TutorialFillUI());
	}

	public void QuickFill(){
		fillBarUI.fillAmount = 1;
		particles.SetActive(false);
	}

	IEnumerator TutorialFillUI(){

		fillBarUI.fillAmount += 0.01f;
		float newX = (1080 * fillBarUI.fillAmount) - 540;
		particles.transform.localPosition = new Vector3 (newX, 0, 0);

		yield return new WaitForSeconds (0.02f);

		if (fillBarUI.fillAmount < 1) {
			StartCoroutine (TutorialFillUI ());
		} else {
			// Advance the tutorial
			particles.SetActive(false);
			// Debug.Log("Advance the Tutorial");
		}
	}

	public MilestoneClass GetCurrentMilestone(){
		return currentMilestone;
	}

	public void SetCurrentMilestone(int index){
		currentMilestone = milestones [index].GetComponent<MilestoneClass> ();
		FillMilestone (currentMilestone);
	}
}
