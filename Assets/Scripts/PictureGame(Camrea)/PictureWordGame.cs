using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/*
	Change this so that all the words are in ONE list - the first 3 words will be required and the rest are bonus
	There can only be 10 text boxes on screen, but there can be any number of bonus words
*/

public class PictureWordGame : MonoBehaviour {

	public TextAsset bonusWordsTxt;
	private string fileAsOneString;

	public enum GameState {
		INTRO = 0,
		TAKE_PIC,
		LABEL_PIC,
		SHOW_RESULTS,
	}
	public GameState currentGameState;
	public GameState lastGameState;

	[Header("Game Data:")]
	[SerializeField] string[] wordsToFind;
	[SerializeField] InputField[] wordsWritten;

	[Header("In Game UI:")]
	[SerializeField] Button checkWordsButton;
	[SerializeField] GameObject introStateUI;
	[SerializeField] GameObject takePicStateUI;
	[SerializeField] GameObject labelPictureStateUI;
	[SerializeField] GameObject showResultsStateUI;

	[SerializeField] GameObject notifyTakePic;
	[SerializeField] Text[] wordsToFindUI;
	[SerializeField] Text[] wordsToFindReminderUI;
	[SerializeField] Text playerPointsUI;

	int wordCount;
	int maxWords = 10;
	int playerPoints = 0;
	int[] wordsFound;
	List<string> bonusWords = new List<string>();
	bool pictureLoaded;
	bool foundWord;

	void Awake(){
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		InitializeGame ();
	}
		
	void InitializeGame(){
		
		// Fill the bonus words list
		fileAsOneString = bonusWordsTxt.text;
		bonusWords.Clear ();
		bonusWords.AddRange (fileAsOneString.Split ("\n" [0]));

		currentGameState = GameState.INTRO;
		introStateUI.SetActive (true);
		pictureLoaded = false;
		wordsFound = new int[maxWords];

		// Initialize WordsFound to 0
		for (int i = 0; i < wordsFound.Length; i++) { wordsFound [i] = 0; }

		// Set the UI for the required words
		for (int i = 0; i < wordsToFindUI.Length; i++) {
			wordsToFindUI [i].text = (i + 1).ToString() + ". " + wordsToFind [i];
			wordsToFindReminderUI [i].text = wordsToFind [i];
		}
	}

	public int CheckWord(InputField word){

		Debug.Log ("Checking Word");

		foundWord = false;
		int points = 0;

		// First Check to see if it contains numbers
		if (CheckAlpha (word.text)) {
			Debug.Log ("Word is Valid");
			// Check if required Word
			for (int i = 0; i < wordsToFind.Length; i++) {
				if ((word.text.ToLower () == wordsToFind [i].ToLower ()) && (wordsFound [i] != 1)) {
					Debug.Log ("Found a match!");
					points = 200;
					AddWord (i, points, word);
				}	
			}
			// Check for bonus word
			if (foundWord == false) {
				for (int i = 0; i < bonusWords.Count; i++){
					if (word.text.ToLower() == bonusWords [i].ToLower()) {
						Debug.Log("Bonus Word");
						// Find an available space in wordsFound
						for (int j = wordsToFind.Length; j < wordsFound.Length; j++){
							if (wordsFound [j] == 0 && foundWord == false) {
								foundWord = true;
							}
						}
						points = 100;
						AddWord (i, points, word);
					}
				}
			}
		}

		return points;
	}

	void AddWord(int index, int points, InputField word){
		foundWord = true;
		wordsFound [index] = 1;
		playerPoints += points;
		word.GetComponent<InputFieldController> ().PinWord ();
		word.GetComponent<InputFieldController> ().SetIndex (index);
		playerPointsUI.text = playerPoints.ToString ();
	}

	// Returns true if only letters are found
	bool CheckAlpha(string word){
		bool containsNumbers = Regex.IsMatch (word, @"\d");

		return !containsNumbers;
	}
		
	public void FinishGame(){
		int requiredWordsFound = 0;
		// Check to see if all of the required words are found
		for (int i = 0; i < wordsToFind.Length; i++){
			if (wordsFound [i] == 1) {
				requiredWordsFound++;
			}
		}

		SetGameState(3);

		if (requiredWordsFound == wordsToFind.Length) {
			// Tally up points and end Game
			showResultsStateUI.transform.FindChild ("Win").gameObject.SetActive (true);
			showResultsStateUI.transform.FindChild ("Try Again").gameObject.SetActive (false);
			Debug.Log("You Win!");
		} else {
			// Tell them how many words they still need to find
			Debug.Log("You need " + (wordsToFind.Length - requiredWordsFound).ToString() + " more words" );
			showResultsStateUI.transform.FindChild ("Win").gameObject.SetActive (false);
			showResultsStateUI.transform.FindChild ("Try Again").gameObject.SetActive (true);
		}

	}

	/*public void CheckWords(){
		wordsWritten = GameObject.FindObjectsOfType<InputField> ();
		wordCount = 0;

		for (int i = 0; i < wordsWritten.Length; i++) {
			CheckWord(i);
		}

		Debug.Log ("Word Count: " + wordCount);
		// showResultsStateUI.SetActive (true);
		SetGameState(3);

		if (wordCount == wordsToFind.Length) {
			showResultsStateUI.transform.FindChild ("Win").gameObject.SetActive (true);
			showResultsStateUI.transform.FindChild ("Try Again").gameObject.SetActive (false);
		} else {
			showResultsStateUI.transform.FindChild ("Win").gameObject.SetActive (false);
			showResultsStateUI.transform.FindChild ("Try Again").gameObject.SetActive (true);
		}
	}

	void CheckWord(int index){
		foreach (InputField input in wordsWritten) {
			if (input.text.ToLower() == wordsToFind [index].ToLower()) {
				Debug.Log ("Found Word!");
				wordCount++;
				break;
			}
		}
	}*/

	void ToggleState(GameState state, bool toggle){
		switch (state) {
		case GameState.INTRO:
			introStateUI.SetActive (toggle);
			break;
		case GameState.TAKE_PIC:
			// Find all the input fields and dleete the - if there are any
			InputField[] placedWords = GameObject.FindObjectsOfType<InputField> ();
			if (placedWords != null) {
				for (int i = 0; i < placedWords.Length; i++) {
					placedWords [i].GetComponent<InputFieldController> ().DestroyField ();
				}
			}
			takePicStateUI.SetActive (toggle);
			break;
		case GameState.LABEL_PIC:
			labelPictureStateUI.SetActive (toggle);
			break;
		case GameState.SHOW_RESULTS:
			showResultsStateUI.SetActive (toggle);
			break;
		default:
			break;
		}
	}

	public void SetGameState(int state){
		if ((currentGameState == GameState.TAKE_PIC && pictureLoaded) || currentGameState != GameState.TAKE_PIC) {
			lastGameState = currentGameState;
			currentGameState = (GameState)state;
			ToggleState (currentGameState, true);
			ToggleState (lastGameState, false);
		} else {
			notifyTakePic.SetActive (true);
			Debug.Log("Please Take and Load a Picture");
		}

		Debug.Log ("Current State: " + currentGameState + " Last State: " + lastGameState);
	}

	public void ShowButton(){
		if (!checkWordsButton.gameObject.activeSelf) {
			checkWordsButton.gameObject.SetActive (true);
		}
	}

	public void SetPictureLoaded(bool state){
		pictureLoaded = state;
	}

	public GameState GetGameState(){
		return currentGameState;
	}

	public bool CanLayInput(){
		InputField[] otherFields = GameObject.FindObjectsOfType<InputField> ();
		if (otherFields.Length == maxWords) {
			Debug.Log ("Too Many Fields");
			return false;
		} else {
			return true;
		}
	}
}
