using UnityEngine;
using System.Collections;

public class StickerController : MonoBehaviour {

	Vector3 savedPosition;
	bool placed = false;

	bool mouseOver = false;

	void OnMouseOver(){
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Mouse Over Sticker");
			this.transform.GetComponentInParent<DecoratePicture> ().EditSticker (this.gameObject);
			mouseOver = true;
		}
	}

	public void OnDrag(){

		if (!placed) {
			float newX = Input.mousePosition.x - (Screen.width * 0.5f);
			float newY = Input.mousePosition.y - (Screen.height * 0.5f);

			transform.localPosition = new Vector3 (newX, newY, this.transform.position.z);
		}
	}

	public void SavePosition(){
		placed = true;
		savedPosition = this.transform.localPosition;
	}

	public Vector3 GetSavedPosition(){
		return savedPosition;
	}

	public void SetPlaced(bool state){
		placed = state;
	}

	public bool CheckMouseOver(){
		return mouseOver;
	}

	public void SetMouseOver(bool state){
		mouseOver = state;
	}
}
