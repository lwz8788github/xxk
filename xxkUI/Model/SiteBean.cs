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

    [Description("起测试间")]
    private string _StartDate;
    public string StartDate
    {
        get { return _StartDate; }
        set { _StartDate = value; }
    }

    [Description("单位代码")]
    private string _UnitCode;
    public string UnitCode
    {
        get { return _UnitCode; }
        set { _UnitCode = value; }
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

    [Description("标石类型")]
    private string _MarkStoneType;
    public string MarkStoneType
    {
        get { return _MarkStoneType; }
        set { _MarkStoneType = value; }
    }

    [Description("建设单位")]
    private string _BuildUnit;
    public string BuildUnit
    {
        get { return _BuildUnit; }
        set { _BuildUnit = value; }
    }

    [Description("监测单位")]
    private string _ObsUnit;
    public string ObsUnit
    {
        get { return _ObsUnit; }
        set { _ObsUnit = value; }
    }

    [Description("场地概况")]
    private string _SiteSituation;
    public string SiteSituation
    {
        get { return _SiteSituation; }
        set { _SiteSituation = value; }
    }

    [Description("地质概况")]
    private string _GeoSituation;
    public string GeoSituation
    {
        get { return _GeoSituation; }
        set { _GeoSituation = value; }
    }

    [Description("资料变更")]
    private string _Datachg;
    public string Datachg
    {
        get { return _Datachg; }
        set { _Datachg = value; }
    }

    [Description("备注")]
    private string _Note;
    public string Note
    {
        get { return _Note; }
        set { _Note = value; }
    }

    [Description("卫星图")]
    private string _RemoteMap;
    public string RemoteMap
    {
        get { return _RemoteMap; }
        set { _RemoteMap = value; }
    }

    [Description("布设图")]
    private string _LayoutMap;
    public string LayoutMap
    {
        get { return _LayoutMap; }
        set { _LayoutMap = value; }
    }

    [Description("坐标集")]
    private string _Locations;
    public string Locations
    {
        get { return _Locations; }
        set { _Locations = value; }
    }

    [Description("其他情况")]
    private string _OtherSituation;
    public string OtherSituation
    {
        get { return _OtherSituation; }
        set { _OtherSituation = value; }
    }
}
