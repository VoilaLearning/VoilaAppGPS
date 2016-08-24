using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class IntroPageController : MonoBehaviour {


    public void SetPlayerName (InputField inputField) {

        PlayerData.SetPlayerName(inputField.text);
    }
}
