using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour {

    public string startGameScene;
    public string creditScene;
    //public float backgroundDelay;
    //public Image background;
    //public float titleDelay;
    //public Image title;
    //public float startButtonDelay;
    //public Button startButton;
    //public float creditsButtonDelay;
    //public Button creditsButton;
    //public float enableAllButtonScriptsDelay;

	public void startGame()
    {
        SceneManager.LoadScene(startGameScene);
    }

    public void startCredits()
    {
        SceneManager.LoadScene(creditScene);
    }

    //IEnumerator loadBackground ()
    //{
    //    yield return new WaitForSecondsRealtime(backgroundDelay);
    //    if (background != null)
    //        background.gameObject.SetActive(true);
    //}

    //IEnumerator loadTitle()
    //{
    //    yield return new WaitForSecondsRealtime(backgroundDelay);
    //    if (title != null)
    //        title.gameObject.SetActive(true);
    //}

    //IEnumerator loadStartButton()
    //{
    //    yield return new WaitForSecondsRealtime(backgroundDelay);
    //    if (title != null)
    //        title.gameObject.SetActive(true);
    //}
}
