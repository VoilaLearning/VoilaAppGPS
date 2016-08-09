using UnityEngine;
using System.Collections;

public class ToggleGameobject : MonoBehaviour {

    public void ToggleObject (GameObject objectToToggle) {

        objectToToggle.SetActive(!objectToToggle.activeSelf);
    }
}
