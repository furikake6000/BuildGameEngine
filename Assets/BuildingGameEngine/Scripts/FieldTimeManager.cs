using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FieldTimeManager : MonoBehaviour {
    
    [SerializeField]
    private float fieldTimeUpdateFreq;  //フィールド時間を更新する頻度(秒)
    [SerializeField]
    private VirtualClock startTime;  //初期時間
    [SerializeField]
    private VirtualClockSpan fieldTimeUpdateAddSpan;  //フィールド時間を更新するたびに加算される時間

    [SerializeField]
    private Image backImage;
    [SerializeField]
    private Image backImageFade;    //背景とフェード用重ねイメージ
    [SerializeField]
    private int morningHour = 6, dayHour = 8, eveningHour = 18, nightHour = 20;    //朝昼夕夜が開始する時間
    [SerializeField]
    private Sprite morningBack, dayBack, eveningBack, nightBack;    //それぞれの背景

    [SerializeField]
    private bool fieldTimeEnabled;  //フィールド時間が動いているか止まっているか
    
    [SerializeField]
    private Text dateLabel;    //日付表示窓
    [SerializeField]
    private Text timeLabel;    //時間表示窓

    private static VirtualClock fieldTime; //フィールド上の時間データ
    public static VirtualClock FieldTime
    {
        get
        {
            return fieldTime;
        }

        set
        {
            fieldTime = value;
        }
    }

    private void Awake()
    {
        //時間は2000年1月1日にリセット
        FieldTime = startTime;
        RefreshClockView();
        RefreshBackImage();
    }

    // Use this for initialization
    void Start () {
        //時間アップデートコルーチン開始
        StartCoroutine(FieldTimeUpdate());
    }

    #region 独自時間コルーチン
    IEnumerator FieldTimeUpdate()
    {
        //ループ
        while (true)
        {

            //Enabledの判定
            while (!fieldTimeEnabled)
            {
                //fieldTimeEnabledがfalseなら永遠にここを回り続ける
                yield return null;
            }
            
            //時間の加算
            FieldTime.Add(fieldTimeUpdateAddSpan);
            RefreshBackImage();

            yield return new WaitForSeconds(fieldTimeUpdateFreq);
        }
    }
    #endregion

    public void RefreshClockView()
    {
        //表示変更
        if(dateLabel != null)dateLabel.text = FieldTime.month + "月" + FieldTime.day + "日";
        if(timeLabel != null)timeLabel.text = FieldTime.hour + ":" + FieldTime.minute.ToString("D2");
    }

    public void RefreshBackImage()
    {
        if(FieldTime.hour < morningHour || FieldTime.hour >= nightHour)
        {
            //よる
            backImage.sprite = nightBack;
            if(FieldTime.hour == morningHour - 1)
            {
                backImageFade.sprite = morningBack;
                backImageFade.color = new Color(1f, 1f, 1f, (float)FieldTime.minute / 60f);
            }
            else
            {
                backImageFade.color = new Color(1f, 1f, 1f, 0f);
            }
        }
        else if(FieldTime.hour < dayHour)
        {
            //あさ
            backImage.sprite = morningBack;
            if (FieldTime.hour == dayHour - 1)
            {
                backImageFade.sprite = dayBack;
                backImageFade.color = new Color(1f, 1f, 1f, (float)FieldTime.minute / 60f);
            }
            else
            {
                backImageFade.color = new Color(1f, 1f, 1f, 0f);
            }
        }
        else if(FieldTime.hour < eveningHour)
        {
            //ひる
            backImage.sprite = dayBack;
            if (FieldTime.hour == eveningHour - 1)
            {
                backImageFade.sprite = eveningBack;
                backImageFade.color = new Color(1f, 1f, 1f, (float)FieldTime.minute / 60f);
            }
            else
            {
                backImageFade.color = new Color(1f, 1f, 1f, 0f);
            }
        }
        else
        {
            //ゆうがた
            backImage.sprite = eveningBack;
            if (FieldTime.hour == nightHour - 1)
            {
                backImageFade.sprite = nightBack;
                backImageFade.color = new Color(1f, 1f, 1f, (float)FieldTime.minute / 60f);
            }
            else
            {
                backImageFade.color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }

    public void ToggleClockEnabled()
    {
        fieldTimeEnabled = !fieldTimeEnabled;
    }
}
