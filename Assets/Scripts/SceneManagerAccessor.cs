using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerAccessor : MonoBehaviour {

    //EventTriggerとかからシーン追加・破棄するためだけのコンポーネント

    public void AddScene(string sceneName)
    {
        MultiSceneManager.AddScene(sceneName);
    }
    public void RemoveScene(string sceneName)
    {
        MultiSceneManager.RemoveScene(sceneName);
    }
    public void QuitGame()
    {
        MultiSceneManager.QuitGame();
    }
}
