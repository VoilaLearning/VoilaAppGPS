using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PictureContainer : MonoBehaviour {

	public List<string> words = new List<string>();
	public List<Vector3> wordCoordinates = new List<Vector3>();
	public Sprite picture;

	public void FillContainer(Sprite newImage, List<string> newWords, List<Vector3>newWordCoords){
		picture = newImage;
		// Fill the words array
		foreach (string word in newWords) {
			words.Add (word);
		}
		// Fill the Coordinates array
		foreach(Vector3 pos in newWordCoords){
			wordCoordinates.Add (pos);
		}
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
}
