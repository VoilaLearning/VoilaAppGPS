using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/*
	Holds the image and tags and positions associated with said image
	These exist in the photobox, but are opened, viewed and interacted with in the album
*/

public class PictureContainer : MonoBehaviour {
	
	[SerializeField] Image iconImage;
	[SerializeField] List<string> words = new List<string>();
	[SerializeField] List<Vector3> wordCoordinates = new List<Vector3>();
	[SerializeField] Sprite picture;
	[SerializeField] string userName;
	string tagID;

    public void FillContainer(Sprite newImage, List<string> newWords, List<Vector3>newWordCoords, string newTagID, string newUsername){
		picture = newImage;
		iconImage.sprite = picture;
		tagID = newTagID;
		// userName = "Student";
		userName = newUsername;

		// Fill the words array
		foreach (string word in newWords) {
			words.Add (word);
		}
		// Fill the Coordinates array
		foreach(Vector3 pos in newWordCoords){
			wordCoordinates.Add (pos);
		}

		string message = "Photo By " + userName.ToUpper() + "\n" + newWords.Count.ToString() + " tags";
		this.GetComponentInChildren<Text> ().text = message;
	}

	public List<string> GetWords(){
		return words;
	}

	public List<Vector3> GetCoords(){
		return wordCoordinates;
	}

	public Sprite GetImage(){
		return picture;
	}

	public void OpenSharedPic(){

		GameObject picturePanel = GameObject.FindGameObjectWithTag("Picture Panel");
		picturePanel.transform.GetChild(0).gameObject.SetActive (true);
		picturePanel.transform.GetChild(0).GetComponent<LoadPicture> ().LoadSelectedPicture (this.gameObject);
	}

	public string GetTagID(){
		return tagID;
	}

    public string GetName () {

        return userName;
    }
}
