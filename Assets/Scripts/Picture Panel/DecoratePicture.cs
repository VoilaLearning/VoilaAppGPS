using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DecoratePicture : MonoBehaviour {

	SavePicture savePicture;
	GameObject picturePanel;

	[SerializeField] GameObject stickerPrefab;
	[SerializeField] GameObject avatarPrefab;

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
	}

	public void CreateAvatarPrefab(Image thisSprite){
		
		GameObject newAvatar = Instantiate (avatarPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		newAvatar.transform.parent = picturePanel.transform;
		newAvatar.transform.GetChild (0).GetComponent<Image>().sprite = thisSprite.sprite;
		newAvatar.transform.localScale = Vector3.one;
		newAvatar.transform.localPosition = Vector3.zero;
		savePicture.AddSticker (newAvatar);
	}
}
