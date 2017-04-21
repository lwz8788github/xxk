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
    private string _SiteCode;
    public string SiteCode
    {
        get { return _SiteCode; }
        set { _SiteCode = value; }
    }
    [Description("场地名")]
    private string _SiteName;
    public string SiteName
    {
        get { return _SiteName; }
        set { _SiteName = value; }
    }
    [Description("场地类型")]
    private string _SiteType;
    public string SiteType
    {
        get { return _SiteType; }
        set { _SiteType = value; }
    }
    [Description("观测类型")]
    private string _Type;
    public string Type
    {
        get { return _Type; }
        set { _Type = value; }
    }
    [Description("经度")]
    private double _Longtitude;
    public double Longtitude
    {
        get { return _Longtitude; }
        set { _Longtitude = value; }
    }
    [Description("纬度")]
    private double _Latitude;
    public double Latitude
    {
        get { return _Latitude; }
        set { _Latitude = value; }
    }

    [Description("高程")]
    private double _Altitude;
    public double Altitude
    {
        get { return _Altitude; }
        set { _Altitude = value; }
    }

    [Description("所在地")]
    private string _Place;
    public string Place
    {
        get { return _Place; }
        set { _Place = value; }
    }

    [Description("所跨断层名")]
    private string _FaultName;
    public string FaultName
    {
        get { return _FaultName; }
        set { _FaultName = value; }
    }

    [Description("所属断层带")]
    private string _FaultZone;
    public string FaultZone
    {
        get { return _FaultZone; }
        set { _FaultZone = value; }
    }

    [Description("断层性质")]
    private string _FaultProperty;
    public string FaultProperty
    {
        get { return _FaultProperty; }
        set { _FaultProperty = value; }
    }

    [Description("断层走向")]
    private string _FaultStrike;
    public string FaultStrike
    {
        get { return _FaultStrike; }
        set { _FaultStrike = value; }
    }

    [Description("断层倾向")]
    private string _FaultTendency;
    public string FaultTendency
    {
        get { return _FaultTendency; }
        set { _FaultTendency = value; }
    }

    [Description("断层倾角")]
    private string _FaultDip;
    public string FaultDip
    {
        get { return _FaultDip; }
        set { _FaultDip = value; }
    }

    [Description("断层上盘岩性")]
    private string _UpRock;
    public string UpRock
    {
        get { return _UpRock; }
        set { _UpRock = value; }
    }

    [Description("断层下盘岩性")]
    private string _BottomRock;
    public string BottomRock
    {
        get { return _BottomRock; }
        set { _BottomRock = value; }
    }

    [Description("起测试间")]
    private string _StartDate;
    public string StartDate
    {
        get { return _StartDate; }
        set { _StartDate = value; }
    }

    [Description("行政区代码")]
    private string _XzCode;
    public string XzCode
    {
        get { return _XzCode; }
        set { _XzCode = value; }
    }

    [Description("单位代码")]
    private string _UnitCode;
    public string UnitCode
    {
        get { return _UnitCode; }
        set { _UnitCode = value; }
    }

    [Description("场地布设图路径")]
    private string _SiteMapFile;
    public string SiteMapFile
    {
        get { return _SiteMapFile; }
        set { _SiteMapFile = value; }
    }

    [Description("历史迁移")]
    private string _Historysite;
    public string Historysite
    {
        get { return _Historysite; }
        set { _Historysite = value; }
    }

    [Description("运行状况")]
    private string _SiteStatus;
    public string SiteStatus
    {
        get { return _SiteStatus; }
        set { _SiteStatus = value; }
    }
}
