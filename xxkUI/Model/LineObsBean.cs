using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Common.Data;

[TableName("t_obsrvtntb")]
[Description("测线观测信息")]

public class LineObsBean
{
    private string _obslinecode;
   
     [Description("测线编码")]
    public string obslinecode
    {
        get { return _obslinecode; }
        set { _obslinecode = value; }
    }
    private DateTime _obvdate;
    [Description("观测日期")]
    public DateTime obvdate
    {
        get { return _obvdate; }
        set { _obvdate = value; }
    }
    private double _obvvalue;
    [Description("数据")]
    public double obvvalue
    {
        get { return _obvvalue; }
        set { _obvvalue = value; }
    }
    private double _pressure;
    [Description("气压")]
    public double pressure
    {
        get { return _pressure; }
        set { _pressure = value; }
    }
    private double _precipitation;
    [Description("降水量")]
    public double precipitation
    {
        get { return _precipitation; }
        set { _precipitation = value; }
    }
    private double _hangingco2temperature;
    [Description("上盘CO2温度")]

    public double hangingco2temperature
    {
        get { return _hangingco2temperature; }
        set { _hangingco2temperature = value; }
    }
    private double _hangingco2concentration;
    [Description("上盘CO2浓度")]
    public double hangingco2concentration
    {
        get { return _hangingco2concentration; }
        set { _hangingco2concentration = value; }
    }
    private double _footco2temperature;
    [Description("下盘CO2温度")]

    public double footco2temperature
    {
        get { return _footco2temperature; }
        set { _footco2temperature = value; }
    }
    private double _footco2concentration;
    [Description("下盘CO2浓度")]
    public double footco2concentration
    {
        get { return _footco2concentration; }
        set { _footco2concentration = value; }
    }
    private string _note;
    [Description("备注")]
    public string note
    {
        get { return _note; }
        set { _note = value; }
    }


}

