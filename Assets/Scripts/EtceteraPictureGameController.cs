using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Prime31;

namespace Prime31
{
	public class EtceteraPictureGameController : MonoBehaviour {

		#if UNITY_IOS

		TutorialController tutorialController;

		[SerializeField] SavePicture savePicture;
		[SerializeField] Image imagePlane;
		[SerializeField] Image picture;
		[SerializeField] GameObject clickField;
		[SerializeField] GameObject[] mainMenuButtons;
        [SerializeField] PictureBoxParent pictureBoxParent;
		string imagePath;

		void Start(){
			// tutorialController = GameObject.FindGameObjectWithTag ("Tutorial Controller").GetComponentInChildren<TutorialController> ();
			EtceteraManager.imagePickerChoseImageEvent += imagePickerChoseImage;
		}

		void OnDisable(){
			EtceteraManager.imagePickerChoseImageEvent += imagePickerChoseImage;
		}

		public void OpenCamera(){
			imagePath = null;
			EtceteraBinding.promptForPhoto (0.25f, PhotoPromptType.Camera);
			StartCoroutine (LoadPic ());
		}

		IEnumerator LoadPic(){
			// Debug.Log ("Loading Texture");

			while (imagePath == null) {
				yield return null; 
			}

			StartCoroutine (EtceteraManager.textureFromFileAtPath ("file://" + imagePath, textureLoaded, textureLoadFailed));
		}

		public void SavePhotoToAlbum(){

			if( imagePath == null ) {
				Debug.Log("Take a picture before saving.");
				return;
			}

			EtceteraBinding.saveImageToPhotoAlbum( imagePath );
		}

		public void TakeScreenShot(){
			StartCoroutine( EtceteraBinding.takeScreenShot( "someScreenshot.png", imagePath =>
				{
					Debug.Log( "Screenshot taken and saved to: " + imagePath );
				}) );
		}

		void imagePickerChoseImage( string imagePath ) {
			this.imagePath = imagePath;
		}

		// Texture loading delegates
		public void textureLoaded( Texture2D texture ) {
			// Debug.Log ("Texture Loaded");
			Rect rect = new Rect (0,0,texture.width,texture.height);
			Vector2 pivot = new Vector2(0.5f,0.5f);
			Sprite newPic = Sprite.Create (texture, rect, pivot);
			if(tutorialController.GetCurrentState() == TutorialState.TAKE_PHOTO) tutorialController.AdvanceTutorial ();

			imagePlane.gameObject.SetActive (true);
			picture.sprite = newPic;
			clickField.SetActive (true);
            pictureBoxParent.DeactivateChildren();

			// Turn off the main menu buttons
			foreach(GameObject button in mainMenuButtons){ button.SetActive(false); }
		}
			
		public void textureLoadFailed( string error )
		{
			var buttons = new string[] { "OK" };
			EtceteraBinding.showAlertWithTitleMessageAndButtons( "Error Loading Texture.  Did you choose a photo first?", error, buttons );
			Debug.Log( "textureLoadFailed: " + error );
		}


		#endif
	}
}