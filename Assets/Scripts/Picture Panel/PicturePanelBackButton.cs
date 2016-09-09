using UnityEngine;
using System.Collections;

public class PicturePanelBackButton : MonoBehaviour {

	SavePicture savePicture;

	[SerializeField] GameObject picturePanel;
	[SerializeField] GameObject avatarPanel;
	[SerializeField] GameObject stickerPanel;

	// Use this for initialization
	void Start () {
		savePicture = gameObject.GetComponent<SavePicture> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TogglePanel(){
		Debug.Log ("Toggling Menu");
		if (stickerPanel.activeSelf == true) {
			stickerPanel.SetActive (false);
		} else if (avatarPanel.activeSelf == true) {
			avatarPanel.SetActive (false);
		} else {
			picturePanel.SetActive (false);
			savePicture.ResetPicturePanel ();
		}
	}
}
