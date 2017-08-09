using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Common.Data;

[TableName("t_datacommit")]
[Description("数据提交表")]

public class DataCommitBean
{
    private string _username;
    [Description("用户名")]
    public string username
    {
        get { return _username; }
        set { _username = value; }
    }

    private string _unitcode;
    [Description("单位编码")]
    public string unitcode
    {
        get { return _unitcode; }
        set { _unitcode = value; }
    }

    private string _sitecode;
    [Description("场地编码")]
    public string sitecode
    {
        get { return _sitecode; }
        set { _sitecode = value; }
    }

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

    private DateTime _cmitdate;
    [Description("提交日期")]
    public DateTime cmitdate
    {
        get { return _cmitdate; }
        set { _cmitdate = value; }
    }

    private string _note;
    [Description("备注")]
    public string note
    {
        get { return _note; }
        set { _note = value; }
    }


}

