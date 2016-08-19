using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoadLanguage : MonoBehaviour {

    [Tooltip("The uiComponents must be entered into this lidt in the SAME ORDER that they are beiing read from the file - see text file to avoid confusion.")]
    [SerializeField] List <Text> uiComponents = new List<Text>();

    bool english = true;

    public TextAsset englishText;
    public TextAsset frenchText;

    private string wholeFileAsOneString;
    private List<string> eachLine = new List<string>();


    public void ToggleLanguage() {
        english = !english;

        if (english) { LoadEnglish(); }
        else { LoadFrench(); }
    }

    void LoadEnglish() {
        wholeFileAsOneString = englishText.text;
        FillUI();
    }

    void LoadFrench() {
        wholeFileAsOneString = frenchText.text;
        FillUI();
    }

    void FillUI() {
        eachLine.Clear();
        eachLine.AddRange(wholeFileAsOneString.Split("\n"[0]));

        // Now fill the text fields 
        for (int i = 0; i < eachLine.Count; i++)
        {
            uiComponents[i].text = eachLine[i];
        }
    }
}
