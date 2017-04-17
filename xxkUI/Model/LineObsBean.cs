using System;
using System.Collections.Generic;
using System.Text;


public class LineObsBean
{
    private string _obslinecode;
    /// <summary>
    /// 测线编码
    /// </summary>
    public string obslinecode
    {
        get { return _obslinecode; }
        set { _obslinecode = value; }
    }
    private string _obvdate;
    /// <summary>
    /// 观测日期
    /// </summary>
    public string obvdate
    {
        get { return _obvdate; }
        set { _obvdate = value; }
    }
    private double _obvvalue;
    /// <summary>
    /// 观测值
    /// </summary>
    public double obvvalue
    {
        get { return _obvvalue; }
        set { _obvvalue = value; }
    }
    private double _pressure;
    /// <summary>
    /// 气压
    /// </summary>
    public double pressure
    {
        get { return _pressure; }
        set { _pressure = value; }
    }
    private double _precipitation;
    /// <summary>
    /// 降水量
    /// </summary>
    public double precipitation
    {
        get { return _precipitation; }
        set { _precipitation = value; }
    }
    private double _hangingco2temperature;
    /// <summary>
    /// 上盘CO2温度
    /// </summary>
    public double hangingco2temperature
    {
        get { return _hangingco2temperature; }
        set { _hangingco2temperature = value; }
    }
    private double _hangingco2concentration;
    /// <summary>
    /// 上盘CO2浓度
    /// </summary>
    public double hangingco2concentration
    {
        get { return _hangingco2concentration; }
        set { _hangingco2concentration = value; }
    }
    private double _footco2temperature;
    /// <summary>
    /// 下盘CO2温度
    /// </summary>
    public double footco2temperature
    {
        get { return _footco2temperature; }
        set { _footco2temperature = value; }
    }
    private double _footco2concentration;
    /// <summary>
    /// 下盘CO2浓度
    /// </summary>
     public double footco2concentration
    {
        get { return _footco2concentration; }
        set { _footco2concentration = value; }
    }
     private string _note;
    /// <summary>
    /// 备注
    /// </summary>
     public string note
     {
         get { return _note; }
         set { _note = value; }
     }


}

