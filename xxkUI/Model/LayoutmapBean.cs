using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Common.Data;

[TableName("t_layoutmap")]
[Description("测线布设图")]

public class LayoutmapBean
{
    private string _layoutmapcode;
        [Description("布设图编码")]
    public string layoutmapcode
    {
        get { return _layoutmapcode; }
        set { _layoutmapcode = value; }
    }

    private string _sitecode;
    [Description("场地编码")]
    public string sitecode
    {
        get { return _sitecode; }
        set { _sitecode = value; }
    }

    private string _layoutmapname;
    [Description("布设图名称")]
    public string layoutmapname
    {
        get { return _layoutmapname; }
        set { _layoutmapname = value; }
    }

    private byte[] _layoutmap;
    [Description("布设图")]
    public byte[] layoutmap
    {
        get { return _layoutmap; }
        set { _layoutmap = value; }
    }

    private string _Bindinglines;
    [Description("绑定的测线")]
    public string Bindinglines
    {
        get { return _Bindinglines; }
        set { _Bindinglines = value; }
    }


}

