using System;
using UnityEngine;

/* Describe：控制软件使用期限
/* Author：SmalBox
 * Blog：https://smalbox.top
 * GitHub：https://GitHub.com/SmalBox
 * Usage：将脚本挂在到项目中第一个启动的脚本上即可。
 * 脚本会默认将软件使用期限设置为从当前日期向后推一个月，
 * 可在Ispector面板中调整具体到期时间。
 */

/// <summary>
/// 软件使用时间限制脚本
/// </summary>
public class TimeControl : MonoBehaviour
{
    // 软件使用时间控制
    public int deadlineYear = DateTime.Now.Year;
    public int deadlineMonth = DateTime.Now.Month + 1;
    public int deadlineDay = DateTime.Now.Day;
    public int deadlineHour = 0;
    public int deadlineMinute = 0;
    public int deadlineSecond = 0;
    private void Awake()
    {
        // 控制试用时间
        UsageTimeControl(
            DateTime.Now,
            new DateTime(
                deadlineYear,
                deadlineMonth,
                deadlineDay,
                deadlineHour,
                deadlineMinute,
                deadlineSecond));
    }

    // 使用时间控制
    private bool UsageTimeControl(DateTime startTime, DateTime endTime)
    {
        if (ConvertDateTimeToLong(endTime) - ConvertDateTimeToLong(startTime) < 0)
        {
            Debug.Log("时间到，自动退出程序");
            Application.Quit();
            return false;
        }else
        {
            Debug.Log("进入试用程序");
            Debug.Log("还剩试用时间：" +
                (ConvertDateTimeToLong(endTime) - ConvertDateTimeToLong(startTime)) / 3600 +
                "小时。"
                );
            return true;
        }
    }
    // 时间转换
    private System.DateTime ConvertLongToDateTime( long timeStamp )
    {
        System.DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime( new System.DateTime( 1970, 1, 1 ) );
        long lTime = long.Parse( timeStamp + "0000000" );
        System.TimeSpan toNow = new System.TimeSpan( lTime );
        return dtStart.Add( toNow );
    }
 
    private long ConvertDateTimeToLong( System.DateTime time )
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime( new System.DateTime( 1970, 1, 1 ) );
        return (long)( time - startTime ).TotalSeconds;
    }
}
