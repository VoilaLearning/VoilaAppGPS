using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class MainMenuNameUI : MonoBehaviour {

    [SerializeField] Text nameText;


    void Awake () {

        if(nameText) { nameText.text = PlayerData.GetPlayerName(); }
    }
}
