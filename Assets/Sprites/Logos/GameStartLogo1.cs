using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameStartLogo1 : MonoBehaviour {

	public GameStartLogo2 logo;
	public Animator animVoila;
	private bool stopLogo;


	//PLAY VOILA LOGO

	void Start ()
	{
		stopLogo = true;
		logo = logo.GetComponent<GameStartLogo2>();
		animVoila = GetComponent<Animator>();
		animVoila.Play("VoilaFull");

		if (stopLogo)
		{
			StartCoroutine(LogoSequencer());
		}
	}



	public IEnumerator LogoSequencer()
	{
		print("Start");
		yield return new WaitForSeconds(3f);
		animVoila.Play("FadeOut");
		yield return new WaitForSeconds(0.2f);

		logo.StartLogo();
		stopLogo = false;
	}
}









