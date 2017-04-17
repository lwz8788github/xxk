using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Common.Data;

[TableName("t_eqkcatalog")]
[Description("地震目录")]
public class EqkBean
{
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
    [Description("发生时间")]
    private string _EakDate;
    public string EakDate
    {
        get { return _EakDate; }
        set { _EakDate = value; }
    }
    [Description("震级")]
    private double _Magntd;
    public double Magntd
    {
        get { return _Magntd; }
        set { _Magntd = value; }
    }
    [Description("地点")]
    private string _Place;
    public string Place
    {
        get { return _Place; }
        set { _Place = value; }
    }
    [Description("深度")]
    private double _Depth;
    public double Depth
    {
        get { return _Depth; }
        set { _Depth = value; }
    }

}

