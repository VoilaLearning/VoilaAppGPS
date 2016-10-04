using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameStartLogo2 : MonoBehaviour {

	public GameStartLogo3 logo;
	public Animator animVidCabin;
	public GameObject bear;
	public GameObject text;

	// Use this for initialization
	void Start ()
	{
		logo = logo.GetComponent<GameStartLogo3>();
		animVidCabin = GetComponent<Animator>();
		bear.SetActive (false);
		text.SetActive (false);

	}

	public void StartLogo()
	{
		StartCoroutine(SplashSequencer());
	}

	public IEnumerator SplashSequencer()
	{
		bear.SetActive (true);
		text.SetActive (true);

		animVidCabin.Play("FadeIn");
		yield return new WaitForSeconds(3.7f);
		animVidCabin.Play("FadeOut");
		yield return new WaitForSeconds(0.2f);

		logo.StartLogo();

	}

}



