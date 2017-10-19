using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using FrikLib.Unity;

public class NovelManager : MonoBehaviour, IPointerDownHandler {

    [SerializeField]
    Text textBox;
    [SerializeField]
    Image faceImage;
    [SerializeField]
    float oneCharDispTime = 0.03f;  //一文字が表示されるのにかかる秒数
    [SerializeField]
    float textUpdateMarginTime = 0f; //連続ページ送りにかかるマージンタイム

    [SerializeField]
    Sprite[] faces; //表情差分

    private static Queue<int> faceID = new Queue<int>();
    private static Queue<string> message = new Queue<string>();

    private string displayingText;  //現在表示しているテキスト
    private float timeSinceDisplayStart;    //現在の文字列表示を開始してからの時間

    // Use this for initialization
    void Start ()
    {
        //最初のページ送り
        TextUpdate();
    }
	
	// Update is called once per frame
	void Update () {
        //経過時間プラス処理
        timeSinceDisplayStart += Time.deltaTime;

        //時間に応じて文字列を表示(経過時間/1文字表示時間 文字表示)
        textBox.text = displayingText.Substring(0, Math.Min((int)(timeSinceDisplayStart / oneCharDispTime), displayingText.Length));
    }

    public static void PutMessage(string inMessage, int inFaceID)
    {
        //とりあえずメッセージと顔グラ情報をキューに追加
        message.Enqueue(inMessage);
        faceID.Enqueue(inFaceID);

        //シーンがロードされていなかった場合追加読み込み
        if (!MultiSceneManager.HasScene("Novel"))
        {
            MultiSceneManager.AddScene("Novel");
        }
    }

    /// <summary>
    /// ページ送り
    /// </summary>
    /// <returns></returns>
    bool TextUpdate()
    {
        if(message.Count >= 1)
        {
            //テキストの更新
            displayingText = message.Dequeue();
            //顔グラの変更処理
            int newFaceID = faceID.Dequeue();
            if (newFaceID < 0 || newFaceID > faces.Length - 1)
            {
                newFaceID = 0;
            }
            faceImage.sprite = faces[newFaceID];

            //経過時間を0にリセット
            timeSinceDisplayStart = 0f;

            //まだある
            return false;
        }
        else
        {
            //もうない
            return true;
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //時間で分岐
        if (timeSinceDisplayStart < oneCharDispTime * displayingText.Length)
        {
            //まだ完全に表示していない場合

            //最後まで表示する
            timeSinceDisplayStart = oneCharDispTime * displayingText.Length;
        }
        else if (timeSinceDisplayStart > oneCharDispTime * displayingText.Length + textUpdateMarginTime)
        {
            //完全に表示しきって、連続タップ猶予時間も過ぎていた場合

            //ページ送り
            if (TextUpdate())
            {
                //メッセージ最後まで表示しきったら終了
                Tutorial1.FinishTutorial();
                MultiSceneManager.RemoveScene("Novel");
            }
        }
    }
}
