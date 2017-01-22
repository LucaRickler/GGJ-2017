using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditSceneManager : MonoBehaviour {

    public string startGameScene;

    public void startGame()
    {
        SceneManager.LoadScene(startGameScene);
    }

}
