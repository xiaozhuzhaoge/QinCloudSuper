using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;


public class LunarInfo {

    public DateTime m_date;
    public int m_LunarYear, m_LunarMonth, m_LunarDay;
    public bool IsLeapMoth = false;
    public string m_LunarYearSexagenary = null, m_LunarYearAnimal = null;
    public string m_LunarYearText = null, m_LunarMonthText = null, m_LunarDayText = null;
    public string m_SolarWeekText = null, m_SolarConstellation = null, m_SolarBirthStone = null;


    public static ChineseLunisolarCalendar calendar = new ChineseLunisolarCalendar();
    public const string ChineseNumber = "〇一二三四五六七八九";
    public const string CelestialStem = "甲乙丙丁戊己庚辛壬癸";
    public const string TerrestrialBranch = "子丑寅卯辰巳午未申酉戌亥";
    public const string Animals = "鼠牛虎兔龙蛇马羊猴鸡狗猪";
    public static readonly string[] ChineseWeekName
        = new string[] { "星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
    public static readonly string[] ChineseDayName = new string[] {  
        "初一","初二","初三","初四","初五","初六","初七","初八","初九","初十",  
        "十一","十二","十三","十四","十五","十六","十七","十八","十九","二十",  
        "廿一","廿二","廿三","廿四","廿五","廿六","廿七","廿八","廿九","三十"};
    public static readonly string[] ChineseMonthName
        = new string[] { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二" };
    public static readonly string[] Constellations
        = new string[] { "白羊座", "金牛座", "双子座", "巨蟹座", "狮子座", "处女座", "天秤座", "天蝎座", "射手座", "摩羯座", "水瓶座", "双鱼座" };
    public static readonly string[] BirthStones
        = new string[] { "钻石", "蓝宝石", "玛瑙", "珍珠", "红宝石", "红条纹玛瑙", "蓝宝石", "猫眼石", "黄宝石", "土耳其玉", "紫水晶", "月长石，血石" };


    public LunarInfo(DateTime date)
    {
        m_date = date;
        InitData();
    }

    private void InitData()
    {
        IsLeapMoth = false;
        m_LunarYearSexagenary = null;
        m_LunarYearAnimal = null;
        m_LunarYearText = null;
        m_LunarMonthText = null;
        m_LunarDayText = null;
        m_SolarWeekText = null;
        m_SolarConstellation = null;
        m_SolarBirthStone = null;

        m_LunarYear = calendar.GetYear(m_date);
        Debug.Log(m_LunarYear);
        m_LunarMonth = calendar.GetMonth(m_date);
        Debug.Log(calendar.GetMonth(m_date));
        int leapMonth = calendar.GetLeapMonth(m_LunarYear);

        if (leapMonth == m_LunarMonth)
        {
            IsLeapMoth = true;
            m_LunarMonth -= 1;
        }
        else if (leapMonth > 0 && leapMonth < m_LunarMonth)
        {
            m_LunarMonth -= 1;
        }

        m_LunarDay = calendar.GetDayOfMonth(m_date);
        Debug.Log(calendar.GetDayOfMonth(m_date));
    }

   
}
