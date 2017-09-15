using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransManager : MonoBehaviour {

	public void ExitGame()
    {
        //終了措置
        Application.Quit();
    }

    public void SceneTrans(string sceneName)
    {
        //シーン遷移
        SceneManager.LoadScene(sceneName);
    }
}
