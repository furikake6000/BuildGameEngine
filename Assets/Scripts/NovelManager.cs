using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NovelManager : MonoBehaviour {

    [SerializeField]
    Text messageWindow;
    [SerializeField]
    Image faceImage;
    [SerializeField]
    GameObject novelParent;  //UIセットの親オブジェクト（表示・非表示に使用）
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

    private bool enable;
    public bool Enable
    {
        get
        {
            return enable;
        }

        set
        {
            enable = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        novelParent.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (!Enable)
        {
            if(message.Count >= 1)
            {
                //メッセージ開始
                novelParent.SetActive(true);
                TextUpdate();
                Enable = true;
            }
        }

		if(Enable)
        {
            //経過時間プラス処理
            timeSinceDisplayStart += Time.deltaTime;

            //時間に応じて文字列を表示(経過時間/1文字表示時間 文字表示)
            messageWindow.text = displayingText.Substring(0, Math.Min((int)(timeSinceDisplayStart / oneCharDispTime), displayingText.Length));

            //タップされた時の処理
            if ((Input.touchCount >= 1 && Input.GetTouch(Input.touchCount - 1).phase == TouchPhase.Began)
                || Input.GetKeyDown(KeyCode.Return))
            {
                //時間で分岐
                if (timeSinceDisplayStart < oneCharDispTime * displayingText.Length)
                {
                    //まだ完全に表示していない場合

                    //最後まで表示する
                    timeSinceDisplayStart = oneCharDispTime * displayingText.Length;
                }
                else if(timeSinceDisplayStart > oneCharDispTime * displayingText.Length + textUpdateMarginTime)
                {
                    //完全に表示しきって、連続タップ猶予時間も過ぎていた場合
                    
                    //ページ送り
                    if (TextUpdate())
                    {
                        //メッセージ最後まで表示しきったら終了
                        novelParent.SetActive(false);
                        Enable = false;
                        Tutorial1.FinishTutorial();
                    }
                }
                
            }
        }
	}

    public static void PutMessage(string inMessage, int inFaceID)
    {
        message.Enqueue(inMessage);
        faceID.Enqueue(inFaceID);
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
}
