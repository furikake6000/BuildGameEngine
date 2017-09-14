using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {

    [SerializeField]
    float messageSpeed = 5f;

    [SerializeField]
    float createXPos, deleteXPos; //メッセージが消えるX座標
    [SerializeField]
    float messageMargin;    //メッセージ間の最小距離

    [SerializeField]
    List<Text> messageTexts;

    private Text recentRefreshedMessagebox;

    public enum MessagePriority
    {
        High,
        Middle,
        Low
    }

    //それぞれの優先度に対しキューを用意する
    private static Dictionary<MessagePriority, Queue<string>> messageList = new Dictionary<MessagePriority, Queue<string>>();

	// Use this for initialization
	void Start () {
		foreach(MessagePriority priority in Enum.GetValues(typeof(MessagePriority)))
        {
            messageList.Add(priority, new Queue<string>());
        }
    }
	
	// Update is called once per frame
	void Update () {
        //ボックス左移動
        foreach(var messageText in messageTexts)
        {
            messageText.transform.localPosition += Vector3.left * messageSpeed;
        }

        //新規メッセージを流すかの判断
        //最近流したメッセージが一定距離以上離れているかnullの時
        if(recentRefreshedMessagebox == null ||
            recentRefreshedMessagebox.transform.localPosition.x
            + recentRefreshedMessagebox.preferredWidth
            + messageMargin < createXPos)
        {
            //どこかに既に表示の終わっているボックスがあるか
            foreach (var messageText in messageTexts)
            {
                if (messageText.transform.localPosition.x + messageText.preferredWidth < deleteXPos)
                {
                    //まだ表示してないメッセージがキューに残っているか
                    if(messageList[MessagePriority.High].Count != 0)
                    {
                        messageText.text = messageList[MessagePriority.High].Dequeue();
                        messageText.color = Color.red;
                        messageText.transform.localPosition = new Vector3(createXPos, messageText.transform.localPosition.y, messageText.transform.localPosition.z);

                        recentRefreshedMessagebox = messageText;
                    }
                    else if (messageList[MessagePriority.Middle].Count != 0)
                    {
                        messageText.text = messageList[MessagePriority.Middle].Dequeue();
                        messageText.color = Color.yellow;
                        messageText.transform.localPosition = new Vector3(createXPos, messageText.transform.localPosition.y, messageText.transform.localPosition.z);

                        recentRefreshedMessagebox = messageText;
                    }
                    else if (messageList[MessagePriority.Low].Count != 0)
                    {
                        messageText.text = messageList[MessagePriority.Low].Dequeue();
                        messageText.color = Color.cyan;
                        messageText.transform.localPosition = new Vector3(createXPos, messageText.transform.localPosition.y, messageText.transform.localPosition.z);

                        recentRefreshedMessagebox = messageText;
                    }

                    break;
                }
            }
        }
        
    }

    public static void PutMessage(string message, MessagePriority priority)
    {
        messageList[priority].Enqueue(message);
    }
}