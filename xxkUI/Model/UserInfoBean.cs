using System;
using System.Collections.Generic;
using System.Text;


public class UserInfoBean
    {
    /// <summary>
    /// 用户名

    /// </summary>
    private string _UserName;
    public string UserName
    {
        get { return _UserName; }
        set { _UserName = value; }
    }

    /// <summary>
    /// 密码
    /// </summary>
    private string _Password;
    public string Password
    {
        get { return _Password; }
        set { _Password = value; }
    }

    /// <summary>
    /// 用户单位
    /// </summary>
    private string _UserUnit;
    public string UserUnit
    {
        get { return _UserUnit; }
        set { _UserUnit = value; }
    }

    /// <summary>
    /// 用户权限
    /// </summary>
    private string _UserAthrty;
    public string UserAthrty
    {
        get { return _UserAthrty; }
        set { _UserAthrty = value; }
    }
}

