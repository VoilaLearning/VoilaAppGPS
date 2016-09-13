using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadPicture : MonoBehaviour {

	[SerializeField] GameObject stickerPrefab;
	[SerializeField] GameObject avatarPrefab;

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
				
		// Go through the  list of stickers
		for (int i = 0; i < pictureContainer.GetComponent<PictureContainer>().GetStickers().Count; i++){
			// Instantiate new sticker
			GameObject newSticker = Instantiate(stickerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			// Set the image
			newSticker.transform.GetChild(0).GetComponent<Image>().sprite = pictureContainer.GetComponent<PictureContainer>().GetStickers()[i].transform.GetChild(0).GetComponent<Image>().sprite;
			// turn off the outline
			newSticker.transform.GetChild(1).gameObject.SetActive(false);
			// place the sticker
			newSticker.transform.position = pictureContainer.GetComponent<PictureContainer>().GetStickers()[i].GetComponent<StickerController>().GetSavedPosition();
		}

		// Create the avatar

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
