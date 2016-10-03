using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class GameStartLogo3 : MonoBehaviour {

	public AudioClip ouistitiSFX;
	AudioSource audio;
    public Animator animSplash;


	// Use this for initialization
	void Start ()
    {
        animSplash = GetComponent<Animator>();
		audio = GetComponent<AudioSource>();

	}

    void StartGame ()
    {
		SceneManager.LoadScene("Intro Page");
    }

    public void StartLogo()
    {
        StartCoroutine(SplashSequencer());
    }

    public IEnumerator SplashSequencer()
    {

        animSplash.Play("FadeIn");
		yield return new WaitForSeconds(0.2f);

		audio.PlayOneShot(ouistitiSFX, 1f);
        yield return new WaitForSeconds(2f);
        StartGame();
    }
}








