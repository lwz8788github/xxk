using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Common.Data;

[TableName("t_siteinfodb")]
[Description("场地信息")]
public class SiteBean
{
    [Description("场地信息")]
    /// <summary>
    /// 场地编码
    /// </summary>
    private string _SiteCode;
    public string SiteCode
    {
        get { return _SiteCode; }
        set { _SiteCode = value; }
    }
    [Description("场地名")]
    /// <summary>
    /// 场地名
    /// </summary>
    private string _SiteName;
    public string SiteName
    {
        get { return _SiteName; }
        set { _SiteName = value; }
    }
    [Description("场地类型")]
    /// <summary>
    /// 场地类型
    /// </summary>
    private string _SiteType;
    public string SiteType
    {
        get { return _SiteType; }
        set { _SiteType = value; }
    }
    [Description("观测类型")]
    /// <summary>
    /// 观测类型
    /// </summary>
    private string _Type;
    public string Type
    {
        get { return _Type; }
        set { _Type = value; }
    }
    [Description("经度")]
    /// <summary>
    /// 经度
    /// </summary>
    private double _Longtitude;
    public double Longtitude
    {
        get { return _Longtitude; }
        set { _Longtitude = value; }
    }
    [Description("纬度")]
    /// <summary>
    /// 纬度
    /// </summary>
    private double _Latitude;
    public double Latitude
    {
        get { return _Latitude; }
        set { _Latitude = value; }
    }
    [Description("高程")]
    /// <summary>
    /// 高程
    /// </summary>
    private double _Altitude;
    public double Altitude
    {
        get { return _Altitude; }
        set { _Altitude = value; }
    }
    [Description("所在地")]
    /// <summary>
    /// 所在地
    /// </summary>
    private string _Place;
    public string Place
    {
        get { return _Place; }
        set { _Place = value; }
    }
    [Description("所跨断层名")]
    /// <summary>
    /// 所跨断层名
    /// </summary>
    private string _FaultName;
    public string FaultName
    {
        get { return _FaultName; }
        set { _FaultName = value; }
    }
    [Description("所属断层带")]
    /// <summary>
    /// 所属断层带
    /// </summary>
    private string _FaultZone;
    public string FaultZone
    {
        get { return _FaultZone; }
        set { _FaultZone = value; }
    }
    [Description("断层性质")]
    /// <summary>
    /// 断层性质
    /// </summary>
    private string _FaultProperty;
    public string FaultProperty
    {
        get { return _FaultProperty; }
        set { _FaultProperty = value; }
    }
    [Description("断层走向")]
    /// <summary>
    /// 断层走向
    /// </summary>
    private string _FaultStrike;
    public string FaultStrike
    {
        get { return _FaultStrike; }
        set { _FaultStrike = value; }
    }
    [Description("断层倾向")]
    /// <summary>
    /// 断层倾向
    /// </summary>
    private string _FaultTendency;
    public string FaultTendency
    {
        get { return _FaultTendency; }
        set { _FaultTendency = value; }
    }
    [Description("断层倾角")]
    /// <summary>
    /// 断层倾角
    /// </summary>
    private string _FaultDip;
    public string FaultDip
    {
        get { return _FaultDip; }
        set { _FaultDip = value; }
    }
    [Description("断层上盘岩性")]
    /// <summary>
    /// 断层上盘岩性
    /// </summary>
    private string _UpRock;
    public string UpRock
    {
        get { return _UpRock; }
        set { _UpRock = value; }
    }
    [Description("断层下盘岩性")]
    /// <summary>
    /// 断层下盘岩性
    /// </summary>
    private string _BottomRock;
    public string BottomRock
    {
        get { return _BottomRock; }
        set { _BottomRock = value; }
    }
    [Description("起测试间")]
    /// <summary>
    /// 起测试间
    /// </summary>
    private string _StartDate;
    public string StartDate
    {
        get { return _StartDate; }
        set { _StartDate = value; }
    }
    [Description("行政区代码")]
    /// <summary>
    /// 行政区代码
    /// </summary>
    private string _XzCode;
    public string XzCode
    {
        get { return _XzCode; }
        set { _XzCode = value; }
    }

    [Description("单位代码")]
    /// <summary>
    /// 单位代码
    /// </summary>
    private string _UnitCode;
    public string UnitCode
    {
        get { return _UnitCode; }
        set { _UnitCode = value; }
    }

    [Description("场地布设图路径")]
    /// <summary>
    /// 场地布设图路径
    /// </summary>
    private string _SiteMapFile;
    public string SiteMapFile
    {
        get { return _SiteMapFile; }
        set { _SiteMapFile = value; }
    }

    [Description("历史迁移")]
    /// <summary>
    /// 历史迁移
    /// </summary>
    private string _Historysite;
    public string Historysite
    {
        get { return _Historysite; }
        set { _Historysite = value; }
    }
    [Description("运行状况")]
    /// <summary>
    /// 运行状况
    /// </summary>
    private string _SiteStatus;
    public string SiteStatus
    {
        get { return _SiteStatus; }
        set { _SiteStatus = value; }
    }
}
