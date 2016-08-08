using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadPicture : MonoBehaviour {

	[SerializeField] Text[] words;
	[SerializeField] Image picture;

	public void LoadSelectedPicture(GameObject newPictureContainer){

		Debug.Log ("Loading Pic");

		this.gameObject.SetActive (true);

		PictureContainer pictureContainer = newPictureContainer.GetComponent<PictureContainer>();

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
	}
}
