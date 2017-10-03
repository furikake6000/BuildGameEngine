using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrikLib
{
    [Serializable]
    public class VirtualClockSpan : System.Object
    {

        public int year, month, day, hour, minute, second;

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ（年月日のみ）
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        public VirtualClockSpan(int year, int month, int day)
            : this(year, month, day, 0, 0, 0)
        {

        }

        /// <summary>
        /// コンストラクタ（時分のみ）
        /// 年月日部分は0年0ヶ月0日に設定されます。
        /// </summary>
        /// <param name="hour">時</param>
        /// <param name="minute">分</param>
        public VirtualClockSpan(int hour, int minute)
            : this(0, 0, 0, hour, minute, 0)
        {

        }

        /// <summary>
        /// コンストラクタ（時間パラメータなし）
        /// すべてが0（Spanなし）に設定されます。
        /// </summary>
        public VirtualClockSpan()
            : this(0, 0, 0, 0, 0, 0)
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
        public VirtualClockSpan(int year, int month, int day, int hour, int minute, int second)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }

        #endregion

    }
}