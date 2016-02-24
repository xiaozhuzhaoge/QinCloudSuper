using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class TimeManager  {

    public static TimeManager instance;
    static int time = 0;
    static long lastRealTime = 0;
    static public int frameLength = 200;
    static long localTimeOnSet = 0;//tick
    //服务器时间
    static long serverTime = 0;//ms
    //东八区
    static long utcOffset = -480 * 60 * 1000;

    public static readonly int Hou = 3600000;
    public static readonly int Min = 60000;
    public static readonly int Sec = 1000;

    public static long GetUTCMS()
    {
        long localOffset = DateTime.Now.Ticks / 10000 - localTimeOnSet;
        DateTime correctTime = new DateTime(TimeManager.CorrectServerTime(serverTime) * 10000 + localOffset * 10000, DateTimeKind.Utc);
        return correctTime.Ticks / 10000;
    }

    public static DateTime GetUTCTime()
    {
        return new DateTime(TimeManager.GetUTCMS() * 10000, DateTimeKind.Utc);
    }

    public static long GetTick(DateTime time , DateTimeKind kind = DateTimeKind.Local)
    {
        return (kind == DateTimeKind.Local) ?
            time.Ticks : time.ToUniversalTime().Ticks;
    }

    public static string GetStringTime(DateTime time, DateTimeKind kind = DateTimeKind.Utc, bool isNow = true)
    {
        return (kind == DateTimeKind.Local) ?
            DateTime.Now.ToString() : DateTime.UtcNow.ToString();
    }

    public static DateTime GetCurrentDateTime(DateTimeKind kind = DateTimeKind.Utc)
    {
        return (kind == DateTimeKind.Local) ?
            DateTime.Now : DateTime.UtcNow;
    }

    /// <summary>
    /// 获取指定月多少日
    /// </summary>
    /// <param name="UTCTime"></param>
    /// <returns></returns>
    public static int GetMaxDaysInMonth(long UTCTime)
    {
        DateTime time = new DateTime(UTCTime);
        return GetMaxDaysInMonth(time);
    }

    public static int GetMaxDaysInMonth(DateTime time)
    {
       return DateTime.DaysInMonth(time.Year,time.Month);
    }

    /// <summary>
    /// 获取制定年多少日
    /// </summary>
    /// <param name="UTCTime"></param>
    /// <returns></returns>
    public static int GetDaysInYear(long UTCTime)
    {
        DateTime time = new DateTime(UTCTime);
        return GetDaysInYear(time);
    }
   
    public static int GetDaysInYear(DateTime time)
    {
        return DateTime.IsLeapYear(time.Year) ? 365 : 366;
    }


    public static void SetServerTime(long serverT, int utcOff = -100000)
    {
        serverTime = serverT;
        localTimeOnSet = DateTime.Now.Ticks / 10000;
        if (utcOff != -100000)
            utcOffset = utcOff * 60 * 1000;
    }


    /// <summary>
    /// 服务器时间纠错
    /// </summary>
    /// <param name="serverTime">服务器UTC时间</param>
    /// <returns></returns>
    public static long CorrectServerTime(long serverTime)
    {
        DateTime offSetTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime serverTim = new DateTime((serverTime - utcOffset) * 10000, DateTimeKind.Utc);
        DateTime serverUtcTime = new DateTime(offSetTime.Ticks + serverTim.Ticks, DateTimeKind.Utc);
        return serverUtcTime.Ticks / 10000;
    }
    /// <summary>
    /// 获取午夜凌晨时间
    /// </summary>
    /// <param name="now">当前时间戳</param>
    /// <param name="kind">时间类型</param>
    /// <returns></returns>
    public static DateTime GetMidnight(DateTime now, DateTimeKind kind = DateTimeKind.Utc)
    {
        return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, kind);
    }

    public static long GetTodayPastTime(DateTime now)
    {
        DateTime midnight = GetMidnight(now);
        return (now.Ticks - midnight.Ticks) / 10000;
    }
}
