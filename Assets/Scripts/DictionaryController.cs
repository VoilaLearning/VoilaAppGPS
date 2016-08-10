using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DictionaryController : MonoBehaviour {

	[SerializeField] Scrollbar dictionaryScroller;

	public GameObject[] letterContainers;

	// Use this for initialization
	void Start () {
		letterContainers = new GameObject[6];
		for (int i = 0; i < this.transform.childCount; i++) {
			letterContainers [i] = this.transform.GetChild (i).gameObject;
		}
	}

	public void GoToLetter(int index){

		float totalPercentage = 0;

		// Get the size of the container
		float dictionaryHeight = this.GetComponent<RectTransform>().sizeDelta.y;

		// loop through all of the letters before the letter you are looking for
		for(int i = 0; i < index; i++){
			// Get the percetage that letter takes in the container
			float tempPercent = letterContainers[i].GetComponent<RectTransform>().sizeDelta.y / dictionaryHeight;
			// add it to the total
			totalPercentage += tempPercent;
		}

		// Go to that spot in the scroll rect
		dictionaryScroller.value = 1f - totalPercentage;

	}
}
