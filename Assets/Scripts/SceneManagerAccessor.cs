using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerAccessor : MonoBehaviour {

    //EventTriggerとかからシーン追加・破棄するためだけのコンポーネント

    public void AddSubScene(string sceneName)
    {
        MultiSceneManager.AddSubScene(sceneName);
    }
    public void RemoveSubScene(string sceneName)
    {
        MultiSceneManager.RemoveSubScene(sceneName);
    }
    public void TransScene(string sceneName)
    {
        MultiSceneManager.TransScene(sceneName);
    }
    public void QuitGame()
    {
        MultiSceneManager.QuitGame();
    }
}
