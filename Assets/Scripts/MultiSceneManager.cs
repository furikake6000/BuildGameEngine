using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using System.Collections;

public class MultiSceneManager : MonoBehaviour {
    const string MasterSceneName = "Master";    //マスターシーン（このマネジャの存在するシーン）の名前

    static MultiSceneManager selfRef;  //Staticメソッドからの自己参照（自己が唯一である原則に基づく）
    static List<string> loadedSubScenes = new List<string>();   //現在呼ばれているシーン
    
    [SerializeField]
    string startScene;    //ゲーム起動時に呼ぶシーン
    [SerializeField]
    List<string> startSubScenes;    //ゲーム起動時に呼ぶサブシーン（0番から順に）

    private void Awake()
    {
        DontDestroyOnLoad(this);    //シーンを駆ける少女
    }

    // Use this for initialization
    void Start () {
        selfRef = this;
        loadedSubScenes.Add(MasterSceneName);  //自分は既に読まれている

        //初期読み込みサブシーンを全てロード
        foreach(var s in startSubScenes)
        {
            AddSubScene(s);
        }

        //最初のシーン遷移
        TransScene(startScene);
	}
	
	public static void AddSubScene(string sceneName)
    {
        //シーン重複検知
        bool isAlreadyLoaded = loadedSubScenes.Contains(sceneName);
        Assert.IsTrue(!isAlreadyLoaded, "Subscene " + sceneName + " has already loaded! You have to unload before reload it!");
        if (isAlreadyLoaded) return;

        loadedSubScenes.Add(sceneName);
        selfRef.StartCoroutine("AddSubSceneASync", sceneName);
    }
    private IEnumerator AddSubSceneASync(string sceneName)
    {
        //読み込んだら完了待たずにコルーチン終了
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return null;
    }

    public static void RemoveSubScene(string sceneName)
    {
        //シーンがもとより存在しない場合を検知
        bool isNotExist = !loadedSubScenes.Contains(sceneName);
        Assert.IsTrue(!isNotExist, "Scene " + sceneName + " has not loaded! You have to load before unload it!");
        if (isNotExist) return;

        loadedSubScenes.Remove(sceneName);
        selfRef.StartCoroutine("RemoveSubSceneASync", sceneName);
    }
    private IEnumerator RemoveSubSceneASync(string sceneName)
    {
        //シーン破棄（完了待たずにコルーチン終了）
        SceneManager.UnloadSceneAsync(sceneName);
        yield return null;
    }

    public static void TransScene(string sceneName)
    {
        loadedSubScenes.Clear();
        selfRef.StartCoroutine("TransSceneASync", sceneName);
    }
    private IEnumerator TransSceneASync(string sceneName)
    {
        //シーン破棄（完了待たずにコルーチン終了）
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        yield return null;
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static bool HasSubScene(string sceneName)
    {
        return loadedSubScenes.Contains(sceneName);
    }
}
