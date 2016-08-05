using UnityEngine;
using System.Collections;

public class AutoDeactivateSelf : MonoBehaviour {

    [SerializeField] float deactivationTime = 0.5f;

    void OnEnable () {

        StopCoroutine(DelayDeactivation());
        StartCoroutine(DelayDeactivation());
    }

    IEnumerator DelayDeactivation () {

        yield return new WaitForSeconds(deactivationTime);

        this.gameObject.SetActive(false);
    }
}
