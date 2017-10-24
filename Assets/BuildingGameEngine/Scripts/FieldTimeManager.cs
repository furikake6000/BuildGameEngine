using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using FrikLib;

public class FieldTimeManager : MonoBehaviour {
    
    [SerializeField]
    private float fieldTimeUpdateFreq;  //フィールド時間を更新する頻度(秒)
    [SerializeField]
    private VirtualClock startTime;  //初期時間
    [SerializeField]
    private VirtualClockSpan fieldTimeUpdateAddSpan;  //フィールド時間を更新するたびに加算される時間

    public static bool TimeEnabled;  //フィールド時間が動いているか止まっているか
    
    [SerializeField]
    private Text dateLabel;    //日付表示窓
    [SerializeField]
    private Text timeLabel;    //時間表示窓

    //現在使用せず
    //[SerializeField]
    //private Image backImage;
    //[SerializeField]
    //private Image backImageFade;    //背景とフェード用重ねイメージ
    //[SerializeField]
    //private int morningHour = 6, dayHour = 8, eveningHour = 18, nightHour = 20;    //朝昼夕夜が開始する時間
    //[SerializeField]
    //private Sprite morningBack, dayBack, eveningBack, nightBack;    //それぞれの背景
    //[SerializeField]
    //private BackImageChangeData[] backImageList;
    //private VirtualClock lastTimeBackChanged;

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
    private static VirtualClock fieldTimeNow,fieldTimePast; //1フレーム前のフィールド上の時間データ
    public static VirtualClock FieldTimePast
    {
        get
        {
            return fieldTimePast;
        }
    }
    public static VirtualClock FieldTimeNow
    {
        get
        {
            return fieldTimeNow;
        }
    }
    private static int deltaMinute, deltaSecond;    //何分、何秒経過したか
    public static int DeltaMinute
    {
        get
        {
            return deltaMinute;
        }
    }
    public static int DeltaSecond
    {
        get
        {
            return deltaSecond;
        }
    }

    private void Awake()
    {
        //時間は2000年1月1日にリセット
        FieldTime = startTime;
        RefreshClockView();
        
        fieldTimePast = startTime.Clone();
        fieldTimeNow = startTime.Clone();
    }

    // Use this for initialization
    void Start()
    {
        //時間アップデートコルーチン開始
        StartCoroutine(FieldTimeUpdate());
    }

    private void Update()
    {
        //fieldTimeNow,Pastの更新
        fieldTimePast.Set(fieldTimeNow);
        fieldTimeNow.Set(fieldTime);

        //deltaMinute, deltaSecondの更新
        deltaMinute = FieldTimeNow.minute - FieldTimePast.minute;
        if (deltaMinute < 0) deltaMinute += 60;
        deltaSecond = deltaMinute * 60 + (FieldTimeNow.second - FieldTimePast.second);

    }

    #region 独自時間コルーチン
    IEnumerator FieldTimeUpdate()
    {
        //ループ
        while (true)
        {

            //Enabledの判定
            while (!TimeEnabled)
            {
                //fieldTimeEnabledがfalseなら永遠にここを回り続ける
                yield return null;
            }

            //時間の加算
            FieldTime.Add(fieldTimeUpdateAddSpan);
            //RefreshBackImage();

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

    //現在使用せず
    //public void RefreshBackImage()
    //{
    //    if (FieldTime.hour < morningHour || FieldTime.hour >= nightHour)
    //    {
    //        //よる
    //        backImage.sprite = nightBack;
    //        if (FieldTime.hour == morningHour - 1)
    //        {
    //            backImageFade.sprite = morningBack;
    //            backImageFade.color = new Color(1f, 1f, 1f, (float)FieldTime.minute / 60f);
    //        }
    //        else
    //        {
    //            backImageFade.color = new Color(1f, 1f, 1f, 0f);
    //        }
    //    }
    //    else if (FieldTime.hour < dayHour)
    //    {
    //        //あさ
    //        backImage.sprite = morningBack;
    //        if (FieldTime.hour == dayHour - 1)
    //        {
    //            backImageFade.sprite = dayBack;
    //            backImageFade.color = new Color(1f, 1f, 1f, (float)FieldTime.minute / 60f);
    //        }
    //        else
    //        {
    //            backImageFade.color = new Color(1f, 1f, 1f, 0f);
    //        }
    //    }
    //    else if (FieldTime.hour < eveningHour)
    //    {
    //        //ひる
    //        backImage.sprite = dayBack;
    //        if (FieldTime.hour == eveningHour - 1)
    //        {
    //            backImageFade.sprite = eveningBack;
    //            backImageFade.color = new Color(1f, 1f, 1f, (float)FieldTime.minute / 60f);
    //        }
    //        else
    //        {
    //            backImageFade.color = new Color(1f, 1f, 1f, 0f);
    //        }
    //    }
    //    else
    //    {
    //        //ゆうがた
    //        backImage.sprite = eveningBack;
    //        if (FieldTime.hour == nightHour - 1)
    //        {
    //            backImageFade.sprite = nightBack;
    //            backImageFade.color = new Color(1f, 1f, 1f, (float)FieldTime.minute / 60f);
    //        }
    //        else
    //        {
    //            backImageFade.color = new Color(1f, 1f, 1f, 0f);
    //        }
    //    }
    //}

    public void ToggleClockEnabled()
    {
        TimeEnabled = !TimeEnabled;
    }

    public static void ToggleClockEnabledStatic()
    {
        TimeEnabled = !TimeEnabled;
    }
}

[Serializable]
class BackImageChangeData
{
    public Sprite sprite;
    public int changeHour;
}