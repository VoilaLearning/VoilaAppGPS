using UnityEngine;
using System.Collections;

public class AutoReactivateSelf : MonoBehaviour {

    [SerializeField] float reactivationTime = 0.5f;

    void OnEnable () {

        StopCoroutine(DelayReactivation());
        StartCoroutine(DelayReactivation());
    }

    IEnumerator DelayReactivation () {

        yield return new WaitForSeconds(reactivationTime);

        this.gameObject.SetActive(true);
    }
}
