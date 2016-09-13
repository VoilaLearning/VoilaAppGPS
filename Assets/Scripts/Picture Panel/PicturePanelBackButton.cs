using UnityEngine;
using System.Collections;

public class PicturePanelBackButton : MonoBehaviour {

	SavePicture savePicture;

	[SerializeField] GameObject picturePanel;
	[SerializeField] GameObject avatarPanel;
	[SerializeField] GameObject stickerPanel;
	[SerializeField] GameObject clickField;
	[SerializeField] GameObject stickerView;
	[SerializeField] GameObject pictureView;

	// Use this for initialization
	void Start () {
		savePicture = gameObject.GetComponent<SavePicture> ();
	}

	public void TogglePanel(){
		Debug.Log ("Toggling Menu");
		if (stickerPanel.activeSelf == true) {
			stickerPanel.SetActive (false);
			clickField.SetActive (true);
		} else if (avatarPanel.activeSelf == true) {
			avatarPanel.SetActive (false);
			clickField.SetActive (true);
		} else {
			picturePanel.SetActive (false);
			savePicture.ResetPicturePanel ();
		}
	}
}
