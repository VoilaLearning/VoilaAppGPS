using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MilestoneTranslator : MonoBehaviour {

    [Header("Text Assets")]
    [SerializeField] TextAsset englishTextFile;
    [SerializeField] TextAsset frenchTextFile;

    [Header("References")]
    [SerializeField] Transform milestoneContainer;

    bool english = false;


	void Start () {
	
        LoadLanguage();
	}

    public void ToggleLanguage () {

        english = !english;
    }

    void LoadLanguage () {

        List<string> eachLine = new List<string>();
        TextAsset textAsset = (english) ? englishTextFile : frenchTextFile;

        eachLine.AddRange(textAsset.text.Split("\n"[0]));

        for(int i = 0; i < milestoneContainer.childCount; i++) {

            MilestoneClass milestone = milestoneContainer.GetChild(i).GetComponent<MilestoneClass>();
            milestone.SetTitle(eachLine[i]);

            int challengeIndex = i + (int)(eachLine.Count * 0.5f);
            milestone.SetChallenge(eachLine[challengeIndex]);
        }
    }
}
