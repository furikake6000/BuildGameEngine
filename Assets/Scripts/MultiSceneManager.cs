using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using System.Collections;

public class MultiSceneManager : MonoBehaviour {
    const string MasterSceneName = "Master";    //マスターシーン（このマネジャの存在するシーン）の名前

    static MultiSceneManager selfRef;  //Staticメソッドからの自己参照（自己が唯一である原則に基づく）
    static List<string> loadedScenes = new List<string>();   //現在呼ばれているシーン

    [SerializeField]
    List<string> startingScenes;    //ゲーム起動時に呼ぶシーン（0番から順に）

	// Use this for initialization
	void Start () {
        selfRef = this;
        loadedScenes.Add(MasterSceneName);  //自分は既に読まれている

        //初期読み込みシーンを全てロード
        foreach(var s in startingScenes)
        {
            AddScene(s);
        }
	}
	
	public static void AddScene(string sceneName)
    {
        //シーン重複検知
        bool isAlreadyLoaded = loadedScenes.Contains(sceneName);
        Assert.IsTrue(!isAlreadyLoaded, "Scene " + sceneName + " has already loaded! You have to unload before reload it!");
        if (isAlreadyLoaded) return;

        loadedScenes.Add(sceneName);
        selfRef.StartCoroutine("AddSceneASync", sceneName);
    }
    private IEnumerator AddSceneASync(string sceneName)
    {
        //読み込んだら完了待たずにコルーチン終了
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return null;
    }

    public static void RemoveScene(string sceneName)
    {
        //シーンがもとより存在しない場合を検知
        bool isNotExist = !loadedScenes.Contains(sceneName);
        Assert.IsTrue(!isNotExist, "Scene " + sceneName + " has not loaded! You have to load before unload it!");
        if (isNotExist) return;

        loadedScenes.Remove(sceneName);
        selfRef.StartCoroutine("RemoveSceneASync", sceneName);
    }
    private IEnumerator RemoveSceneASync(string sceneName)
    {
        //シーン破棄（完了待たずにコルーチン終了）
        SceneManager.UnloadSceneAsync(sceneName);
        yield return null;
    }
}
