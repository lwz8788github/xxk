using System;
using System.Collections.Generic;
using System.Text;


public class UnitInfoBean
    {
    /// <summary>
    /// 单位名称
    /// </summary>
    private string _UnitName;
    public string UnitName
    {
        get { return _UnitName; }
        set { _UnitName = value; }
    }

    /// <summary>
    /// 单位代码
    /// </summary>
    private string _UnitCode;
    public string UnitCode
    {
        get { return _UnitCode; }
        set { _UnitCode = value; }
    }
}

