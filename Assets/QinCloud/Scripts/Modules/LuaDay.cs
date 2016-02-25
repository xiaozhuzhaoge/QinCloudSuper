using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// LunDay 的摘要说明。
/// 用法说明
/// 直接调用即可,比较简单
/// TODO ： 网上复制的算法，有空看看
/// </summary>
public class LuaDay
{
    public LuaDay()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    //天干
    private static readonly string[] TianGan = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

    //地支
    private static readonly string[] DiZhi = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

    //十二生肖
    private static readonly string[] ShengXiao = { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

    //农历日期
    private static readonly string[] DayName =   {"*","初一","初二","初三","初四","初五",
             "初六","初七","初八","初九","初十",
             "十一","十二","十三","十四","十五",
             "十六","十七","十八","十九","二十",
             "廿一","廿二","廿三","廿四","廿五",      
             "廿六","廿七","廿八","廿九","三十"};

    //农历月份
    private static readonly string[] MonthName = { "*", "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "腊" };

    //公历月计数天
    private static readonly int[] MonthAdd = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
    //农历数据
    private static readonly int[] LunarData = {2635,333387,1701,1748,267701,694,2391,133423,1175,396438
            ,3402,3749,331177,1453,694,201326,2350,465197,3221,3402
            ,400202,2901,1386,267611,605,2349,137515,2709,464533,1738
            ,2901,330421,1242,2651,199255,1323,529706,3733,1706,398762
            ,2741,1206,267438,2647,1318,204070,3477,461653,1386,2413
            ,330077,1197,2637,268877,3365,531109,2900,2922,398042,2395
            ,1179,267415,2635,661067,1701,1748,398772,2742,2391,330031
            ,1175,1611,200010,3749,527717,1452,2742,332397,2350,3222
            ,268949,3402,3493,133973,1386,464219,605,2349,334123,2709
            ,2890,267946,2773,592565,1210,2651,395863,1323,2707,265877};


    public enum DayType
    {
        Year,
        Month,
        Day,
        Hour,
        TianGan,
        DiZhi,
    }

    /// <summary>
    /// 获取对应日期的农历
    /// </summary>
    /// <param name="dtDay">公历日期</param>
    /// <returns></returns>
    public Dictionary<DayType,string> GetLunarCalendar(DateTime dtDay)
    {
        string sYear = dtDay.Year.ToString();
        string sMonth = dtDay.Month.ToString();
        string sDay = dtDay.Day.ToString();
        int year;
        int month;
        int day;
        try
        {
            year = int.Parse(sYear);
            month = int.Parse(sMonth);
            day = int.Parse(sDay);
        }
        catch
        {
            year = DateTime.Now.Year;
            month = DateTime.Now.Month;
            day = DateTime.Now.Day;
        }

        int nTheDate;
        int nIsEnd;
        int k, m, n, nBit, i;
        string calendar = string.Empty;
        //计算到初始时间1921年2月8日的天数：1921-2-8(正月初一)
        nTheDate = (year - 1921) * 365 + (year - 1921) / 4 + day + MonthAdd[month - 1] - 38;
        if ((year % 4 == 0) && (month > 2))
            nTheDate += 1;
        //计算天干，地支，月，日
        nIsEnd = 0;
        m = 0;
        k = 0;
        n = 0;
        while (nIsEnd != 1)
        {
            if (LunarData[m] < 4095)
                k = 11;
            else
                k = 12;
            n = k;
            while (n >= 0)
            {
                //获取LunarData[m]的第n个二进制位的值
                nBit = LunarData[m];
                for (i = 1; i < n + 1; i++)
                    nBit = nBit / 2;
                nBit = nBit % 2;
                if (nTheDate <= (29 + nBit))
                {
                    nIsEnd = 1;
                    break;
                }
                nTheDate = nTheDate - 29 - nBit;
                n = n - 1;
            }
            if (nIsEnd == 1)
                break;
            m = m + 1;
        }
        year = 1921 + m;
        month = k - n + 1;
        day = nTheDate;
        //return year+"-"+month+"-"+day;

        #region 格式化日期显示为三月廿四
        if (k == 12)
        {
            if (month == LunarData[m] / 65536 + 1)
                month = 1 - month;
            else if (month > LunarData[m] / 65536 + 1)
                month = month - 1;
        }

        Dictionary<DayType, string> values = new Dictionary<DayType, string>();
        //生肖
        values.Add(DayType.Year, ShengXiao[(year - 4) % 60 % 12].ToString() + "年");
        //天干
        values.Add(DayType.TianGan , TianGan[(year - 4) % 60 % 10].ToString());
        //地支
        values.Add(DayType.DiZhi, DiZhi[(year - 4) % 60 % 12].ToString());

        //农历月
        if (month < 1)
            values.Add(DayType.Month, "闰" + MonthName[-1 * month].ToString() + "月");
        else
            values.Add(DayType.Month,  MonthName[month].ToString() + "月");

        //农历日
        values.Add(DayType.Day, DayName[day].ToString() + "日");

        return values;

        #endregion
    }



    private static readonly string[] Constellations
        = new string[] { "白羊座", "金牛座", "双子座", "巨蟹座", "狮子座", "处女座", "天秤座", "天蝎座", "射手座", "摩羯座", "水瓶座", "双鱼座" };
    private static readonly string[] BirthStones
        = new string[] { "钻石", "蓝宝石", "玛瑙", "珍珠", "红宝石", "红条纹玛瑙", "蓝宝石", "猫眼石", "黄宝石", "土耳其玉", "紫水晶", "月长石，血石" };

    /// <summary>  
    /// 根据指定阳历日期计算星座＆诞生石  
    /// </summary>  
    /// <param name="date">指定阳历日期</param>  
    /// <param name="constellation">星座</param>  
    /// <param name="birthstone">诞生石</param>  
    public static void CalcConstellation(DateTime date, out string constellation, out string birthstone)
    {
        int i = Convert.ToInt32(date.ToString("MMdd"));
        int j;
        if (i >= 321 && i <= 419)
            j = 0;
        else if (i >= 420 && i <= 520)
            j = 1;
        else if (i >= 521 && i <= 621)
            j = 2;
        else if (i >= 622 && i <= 722)
            j = 3;
        else if (i >= 723 && i <= 822)
            j = 4;
        else if (i >= 823 && i <= 922)
            j = 5;
        else if (i >= 923 && i <= 1023)
            j = 6;
        else if (i >= 1024 && i <= 1121)
            j = 7;
        else if (i >= 1122 && i <= 1221)
            j = 8;
        else if (i >= 1222 || i <= 119)
            j = 9;
        else if (i >= 120 && i <= 218)
            j = 10;
        else if (i >= 219 && i <= 320)
            j = 11;
        else
        {
            constellation = "未知星座";
            birthstone = "未知诞生石";
            return;
        }
        constellation = Constellations[j];
        birthstone = BirthStones[j];
        #region 星座划分
        //白羊座：   3月21日------4月19日     诞生石：   钻石     
        //金牛座：   4月20日------5月20日   诞生石：   蓝宝石     
        //双子座：   5月21日------6月21日     诞生石：   玛瑙     
        //巨蟹座：   6月22日------7月22日   诞生石：   珍珠     
        //狮子座：   7月23日------8月22日   诞生石：   红宝石     
        //处女座：   8月23日------9月22日   诞生石：   红条纹玛瑙     
        //天秤座：   9月23日------10月23日     诞生石：   蓝宝石     
        //天蝎座：   10月24日-----11月21日     诞生石：   猫眼石     
        //射手座：   11月22日-----12月21日   诞生石：   黄宝石     
        //摩羯座：   12月22日-----1月19日   诞生石：   土耳其玉     
        //水瓶座：   1月20日-----2月18日   诞生石：   紫水晶     
        //双鱼座：   2月19日------3月20日   诞生石：   月长石，血石    
        #endregion
    }
}
