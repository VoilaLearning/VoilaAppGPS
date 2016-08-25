using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadPicture : MonoBehaviour {

	[SerializeField] Text[] words;
	[SerializeField] Image picture;
	[SerializeField] Text tagCountAndID;
	[SerializeField] Button likeButton;

	PictureContainer pictureContainer = null;

	public void LoadSelectedPicture(GameObject newPictureContainer){

		// enable the like button
		likeButton.interactable = true;

		// Empty the words array
		for (int i = 0; i < words.Length; i++){
			words [i].transform.parent.gameObject.SetActive (false);
			words [i].text = "";
			words [i].transform.parent.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		}

		this.gameObject.SetActive (true);

		pictureContainer = newPictureContainer.GetComponent<PictureContainer>();

		// Set the Image
		picture.sprite = pictureContainer.GetImage();

		// Set and position the words
		string[] newWords = pictureContainer.GetWords () .ToArray();
		Vector3[] newPos = pictureContainer.GetCoords ().ToArray();
				
		for (int i = 0; i < newWords.Length; i++) {
			words [i].transform.parent.gameObject.SetActive (true);
			words [i].text = newWords [i];
			words [i].transform.parent.GetComponent<RectTransform> ().localPosition = newPos [i] * 0.75f;
		}

		if (pictureContainer.GetTagID() == null || pictureContainer.GetTagID() == "") {
			tagCountAndID.text = newWords.Length.ToString () + " Tags";
		} else {
			tagCountAndID.text = newWords.Length.ToString () + " Tags " + "- " + pictureContainer.GetTagID();
		}
	}

	public void LikePic(){
		pictureContainer.AddLike();
	}
}
