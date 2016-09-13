﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DecoratePicture : MonoBehaviour {

	SavePicture savePicture;
	GameObject picturePanel;

	[SerializeField] GameObject stickerPrefab;
	[SerializeField] GameObject avatarPrefab;
	[SerializeField] GameObject stickerView;
	[SerializeField] GameObject pictureView;
	[SerializeField] GameObject clickField;

	GameObject currentSticker;
	bool createdAvatar = false;

	// Use this for initialization
	void Start () {
		savePicture = this.GetComponent<SavePicture> ();
		picturePanel = this.transform.GetChild (0).gameObject;
	}

	public void CreateStickerPrefab(Image thisSprite){

		GameObject newSticker = Instantiate (stickerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		newSticker.transform.parent = picturePanel.transform;
		newSticker.transform.GetChild (0).GetComponent<Image>().sprite = thisSprite.sprite;
		newSticker.transform.localScale = Vector3.one;
		newSticker.transform.localPosition = Vector3.zero;
		savePicture.AddSticker (newSticker);

		currentSticker = newSticker;
	}

	public void CreateAvatarPrefab(Image thisSprite){

		if (!createdAvatar) {
			GameObject newAvatar = Instantiate (avatarPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			newAvatar.transform.parent = picturePanel.transform;
			newAvatar.transform.GetChild (0).GetComponent<Image> ().sprite = thisSprite.sprite;
			newAvatar.transform.localScale = Vector3.one;
			newAvatar.transform.localPosition = Vector3.zero;
			savePicture.AddSticker (newAvatar);
		}

		createdAvatar = true;
	}

	public void DeleteSticker(){
		savePicture.RemoveSticker (currentSticker);
		Destroy (currentSticker.gameObject);
		LeaveStickerView ();
	}

	public void SaveSticker(){
		currentSticker.transform.GetChild (1).gameObject.SetActive (false);
		currentSticker.GetComponent<Button> ().enabled = true;
		LeaveStickerView ();
	}

	public void EditSticker(GameObject chosenSticker){
		currentSticker = chosenSticker;
		currentSticker.GetComponent<StickerController> ().SetPlaced (false);
		clickField.SetActive (false);
		stickerView.SetActive (true);
		pictureView.SetActive (false);
	}

	void LeaveStickerView(){
		stickerView.SetActive (false);
		pictureView.SetActive (true);
		clickField.SetActive (true);
	}
}
