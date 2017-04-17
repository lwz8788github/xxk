using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Common.Data;

[TableName("t_sysuser")]
[Description("用户信息")]
public class UserInfoBean
{
    [Description("用户名")]
    private string _UserName;
    public string UserName
    {
        get { return _UserName; }
        set { _UserName = value; }
    }

    [Description("密码")]
       private string _Password;
    public string Password
    {
        get { return _Password; }
        set { _Password = value; }
    }

    [Description("用户单位")]
    private string _UserUnit;
    public string UserUnit
    {
        get { return _UserUnit; }
        set { _UserUnit = value; }
    }

    [Description("用户权限")]
    private string _UserAthrty;
    public string UserAthrty
    {
        get { return _UserAthrty; }
        set { _UserAthrty = value; }
    }
}

