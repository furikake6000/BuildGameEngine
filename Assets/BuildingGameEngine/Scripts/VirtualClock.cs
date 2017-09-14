using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VirtualClock : System.Object {

    public int year, month, day, hour, minute, second;

    public readonly bool useRealCalender;

    //一ヶ月何日か
    private readonly int[] daysOfMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

    #region コンストラクタ

    /// <summary>
    /// コンストラクタ（年月日のみ）
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <param name="day">日</param>
    /// <param name="useRealCalender">リアル暦を使うか否か</param>
    public VirtualClock(int year, int month, int day, bool useRealCalender)
        : this(year, month, day, 0, 0, 0, useRealCalender)
    {

    }

    /// <summary>
    /// コンストラクタ（時分のみ）
    /// 日付は2000年1月1日に設定されます。
    /// </summary>
    /// <param name="hour">時</param>
    /// <param name="minute">分</param>
    /// <param name="useRealCalender">リアル暦を使うか否か</param>
    public VirtualClock(int hour, int minute, bool useRealCalender)
        : this(2000, 1, 1, hour, minute, 0, useRealCalender)
    {

    }

    /// <summary>
    /// コンストラクタ（時間パラメータなし）
    /// 2000年1月1日0時0分に設定されます。
    /// </summary>
    /// <param name="useRealCalender">リアル暦を使うか否か</param>
    public VirtualClock(bool useRealCalender)
        : this(2000, 1, 1, 0, 0, 0, useRealCalender)
    {

    }

    /// <summary>
    /// デフォルトコンストラクタ
    /// 明示的は使用は非推奨です（VirtualClock(bool)を使ってください）
    /// </summary>
    public VirtualClock()
        : this(2000, 1, 1, 0, 0, 0, false)
    {

    }

    /// <summary>
    /// コンストラクタ（全てのデータ）
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <param name="day">日</param>
    /// <param name="hour">時</param>
    /// <param name="minute">分</param>
    /// <param name="second">秒</param>
    /// <param name="useRealCalender">リアル暦を使うか否か</param>
    public VirtualClock(int year, int month, int day, int hour, int minute, int second, bool useRealCalender)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
        this.second = second;

        this.useRealCalender = useRealCalender;
    }

    #endregion

    #region 時刻進める系関数

    /// <summary>
    /// 指定秒数を追加する
    /// </summary>
    /// <param name="addedSecond"></param>
    /// <returns></returns>
    public void AddSecond( int addedSecond )
    {
        //秒加算
        second += addedSecond;

        //分加算
        AddMinute(second / 60);
        second = second % 60;
    }

    public void AddMinute(int addedMinute)
    {
        //分加算
        minute += addedMinute;

        //時加算
        AddHour(minute / 60);
        minute = minute % 60;
    }

    public void AddHour(int addedHour)
    {
        //時加算
        hour += addedHour;

        //日加算
        AddDay(hour / 24);
        hour = hour % 24;
    }

    public void AddDay(int addedDay)
    {
        //日加算
        day += addedDay;

        //月加算
        if (useRealCalender)
        {
            //リアル暦による計算（西向く侍・うるう年アリ）
            while (DaysOfMonth() >= day)
            {
                day -= DaysOfMonth();

                AddMonth(1);
            }
        }
        else
        {
            //バーチャル暦による計算（すべての月が30日）
            AddMonth(day / 30);
            day = day % 30;
        }
    }

    public void AddMonth(int addedMonth)
    {
        //月計算
        month += addedMonth;

        //年計算
        AddYear(addedMonth / 12);
        month = month % 12;
    }

    public void AddYear(int addedYear)
    {
        //年計算
        year += addedYear;
    }

    public void Add(VirtualClockSpan addedSpan)
    {
        //秒から順に足していく（逆だとうるう年の処理などが多少変わる）
        AddSecond(addedSpan.second);
        AddMinute(addedSpan.minute);
        AddHour(addedSpan.hour);
        AddDay(addedSpan.day);
        AddMonth(addedSpan.month);
        AddYear(addedSpan.year);
    }

    #endregion

    #region 演算子
    public static bool operator ==(VirtualClock v1, VirtualClock v2)
    {
        //nullの確認
        if (object.ReferenceEquals(v1, v2))
        {
            return true;
        }
        if (((object)v1 == null) || ((object)v2 == null))
        {
            return false;
        }

        return (v1.year == v2.year) && (v1.month == v2.month) && (v1.day == v2.day)
            && (v1.hour == v2.hour) && (v1.minute == v2.minute) && (v1.second == v2.second);
    }

    public static bool operator !=(VirtualClock v1, VirtualClock v2)
    {
        return !(v1 == v2);
    }

    public static bool operator >(VirtualClock v1, VirtualClock v2)
    {
        //nullの確認
        if (object.ReferenceEquals(v1, v2))
        {
            return false;
        }
        if (((object)v1 == null) || ((object)v2 == null))
        {
            return false;
        }

        //あたまのわるい比較関数
        if (v1.year > v2.year)
        {
            return true;
        }
        else if(v1.year == v2.year)
        {
            if (v1.month > v2.month)
            {
                return true;
            }
            else if (v1.month == v2.month)
            {
                if(v1.day > v2.day)
                {
                    return true;
                }
                else if(v1.day == v2.day)
                {
                    if (v1.hour > v2.hour)
                    {
                        return true;
                    }
                    else if(v1.hour == v2.hour)
                    {
                        if(v1.minute > v2.minute)
                        {
                            return true;
                        }
                        else if(v1.minute == v2.minute)
                        {
                            return v1.second > v2.second;
                        }
                    }
                }
            }
        }

        return false;
            
    }

    public static bool operator <(VirtualClock v1, VirtualClock v2)
    {
        //nullの確認
        if (object.ReferenceEquals(v1, v2))
        {
            return false;
        }
        if (((object)v1 == null) || ((object)v2 == null))
        {
            return false;
        }

        //あたまのわるい比較関数
        if (v1.year < v2.year)
        {
            return true;
        }
        else if (v1.year == v2.year)
        {
            if (v1.month < v2.month)
            {
                return true;
            }
            else if (v1.month == v2.month)
            {
                if (v1.day < v2.day)
                {
                    return true;
                }
                else if (v1.day == v2.day)
                {
                    if (v1.hour < v2.hour)
                    {
                        return true;
                    }
                    else if (v1.hour == v2.hour)
                    {
                        if (v1.minute < v2.minute)
                        {
                            return true;
                        }
                        else if (v1.minute == v2.minute)
                        {
                            return v1.second < v2.second;
                        }
                    }
                }
            }
        }

        return false;
    }

    public static bool operator >=(VirtualClock v1, VirtualClock v2)
    {
        return (v1 > v2) || (v1 == v2);
    }

    public static bool operator <=(VirtualClock v1, VirtualClock v2)
    {
        return (v1 < v2) || (v1 == v2);
    }
    #endregion

    private int DaysOfMonth()
    {
        //エラー処理
        if(month < 1 || month > 12)
        {
            //ここに来てしまったのならエラー
            return -1;
        }

        if(month == 2 && (year % 4 == 0 && year % 100 != 0 || year % 2000 == 0))
        {
            //うるう年の2月
            return 29;
        }
        else
        {
            return daysOfMonth[month - 1];
        }
    }
}