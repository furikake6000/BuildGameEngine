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
    Sprite[] faces; //表情差分

    private static Queue<int> faceID = new Queue<int>();
    private static Queue<string> message = new Queue<string>();

    private float timeSinceDisplayStart;    //現在の文字列表示を開始した時間

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
            if((Input.touchCount >= 1 && Input.GetTouch(Input.touchCount - 1).phase == TouchPhase.Ended)
                || Input.GetKeyDown(KeyCode.Return))
            {
                //ページ送り
                if (!TextUpdate())
                {
                    //メッセージ終了
                    novelParent.SetActive(false);
                    Enable = false;
                    Tutorial1.FinishTutorial();
                }
            }
        }
	}

    public static void PutMessage(string inMessage, int inFaceID)
    {
        message.Enqueue(inMessage);
        faceID.Enqueue(inFaceID);
    }

    bool TextUpdate()
    {
        if(message.Count >= 1)
        {
            messageWindow.text = message.Dequeue();
            int newFaceID = faceID.Dequeue();
            if (newFaceID < 0 || newFaceID > faces.Length - 1)
            {
                newFaceID = 0;
            }
            faceImage.sprite = faces[newFaceID];

            //まだある
            return true;
        }
        else
        {
            //もうない
            return false;
        }
        
    }
}
