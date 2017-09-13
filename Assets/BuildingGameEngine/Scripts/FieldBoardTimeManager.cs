using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldBoardTimeManager : MonoBehaviour {
    
    [SerializeField]
    private float fieldTimeUpdateFreq;  //フィールド時間を更新する頻度(秒)
    [SerializeField]
    private VirtualClock startTime;  //初期時間
    [SerializeField]
    private bool yearEventActivate, monthEventActivate, dailyEventActivate,
        hourEventActivate, minuteEventActivate, secondEventActivate;
    [SerializeField]
    private VirtualClockSpan fieldTimeUpdateAddSpan;  //フィールド時間を更新するたびに加算される時間
    [SerializeField]
    private bool fieldTimeEnabled;  //フィールド時間が動いているか止まっているか
    
    [SerializeField]
    private Text dateLabel;    //日付表示窓
    [SerializeField]
    private Text timeLabel;    //時間表示窓

    private VirtualClock fieldTime; //フィールド上の時間データ

    private void Awake()
    {
        //時間は2000年1月1日にリセット
        fieldTime = startTime;
        RefreshClockView();
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
            fieldTime.Add(fieldTimeUpdateAddSpan);
            RefreshClockView();

            //毎年行われる処理

            //毎月行われる処理

            //毎日行われる処理

            //毎時行われる処理

            //毎分行われる処理

            //毎秒行われる処理

            yield return new WaitForSeconds(fieldTimeUpdateFreq);
        }
    }
    #endregion

    private void RefreshClockView()
    {
        //表示変更
        dateLabel.text = fieldTime.month + "月" + fieldTime.day + "日";
        timeLabel.text = fieldTime.hour + ":" + fieldTime.minute.ToString("D2");
    }

    public void ToggleClockEnabled()
    {
        fieldTimeEnabled = !fieldTimeEnabled;
    }
}
