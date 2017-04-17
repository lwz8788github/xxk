using System;
using System.Collections.Generic;
using System.Text;


public class LineBean
{

    private string _SITECODE;
    /// <summary>
    /// 场地编码
    /// </summary>
    public string SITECODE
    {
        get { return _SITECODE; }
        set { _SITECODE = value; }
    }

    private string _OBSLINECODE;
    /// <summary>
    /// 测线编码
    /// </summary>
    public string OBSLINECODE
    {
        get { return _OBSLINECODE; }
        set { _OBSLINECODE = value; }
    }
    private string _OBSLINENAME;
    /// <summary>
    /// 测线名称
    /// </summary>
    public string OBSLINENAME
    {
        get { return _OBSLINENAME; }
        set { _OBSLINENAME = value; }
    }
   
    private string _BASEOBSTYPE;
    /// <summary>
    /// 基础测项
    /// </summary>
    public string BASEOBSTYPE
    {
        get { return _BASEOBSTYPE; }
        set { _BASEOBSTYPE = value; }
    }
    private string _AIDSOBSTYPE;
    /// <summary>
    /// 辅助测项
    /// </summary>
    public string AIDSOBSTYPE
    {
        get { return _AIDSOBSTYPE; }
        set { _AIDSOBSTYPE = value; }
    }
    private string _OBSCYCLE;
    /// <summary>
    /// 观测周期
    /// </summary>
    public string OBSCYCLE
    {
        get { return _OBSCYCLE; }
        set { _OBSCYCLE = value; }
    }
    private string _UP_BOT;
    /// <summary>
    /// 上盘-下盘
    /// </summary>
    public string UP_BOT
    {
        get { return _UP_BOT; }
        set { _UP_BOT = value; }
    }
    private string _BUILDDATE;
     /// <summary>
    /// 建立时间
    /// </summary>
    public string BUILDDATE
    {
        get { return _BUILDDATE; }
        set { _BUILDDATE = value; }
    }
    private string _SATRTDATE;
    /// <summary>
    /// 开测时间
    /// </summary>
    public string SATRTDATE
    {
        get { return _SATRTDATE; }
        set { _SATRTDATE = value; }
    }
    private string _LENGTH;
    /// <summary>
    /// 测线长度
    /// </summary>
    public string LENGTH
    {
        get { return _LENGTH; }
        set { _LENGTH = value; }
    }
    private string _STATIONCOUNT;
    /// <summary>
    /// 测站数
    /// </summary>
    public string STATIONCOUNT
    {
        get { return _STATIONCOUNT; }
        set { _STATIONCOUNT = value; }
    }
    private string _FAULTZONE;
    /// <summary>
    /// 所属断层
    /// </summary>
    public string FAULTZONE
    {
        get { return _FAULTZONE; }
        set { _FAULTZONE = value; }
    }
    private string _FAULTSTRIKE;
    /// <summary>
    /// 断层走向
    /// </summary>
    public string FAULTSTRIKE
    {
        get { return _FAULTSTRIKE; }
        set { _FAULTSTRIKE = value; }
    }
    private string _FAULTTENDENCY;
    /// <summary>
    /// 断层倾向
    /// </summary>
    public string FAULTTENDENCY
    {
        get { return _FAULTTENDENCY; }
        set { _FAULTTENDENCY = value; }
    }
    private string _FAULTDIP;
    /// <summary>
    /// 断层倾角
    /// </summary>
    public string FAULTDIP
    {
        get { return _FAULTDIP; }
        set { _FAULTDIP = value; }
    }
    private string _LINE_FAULT_ANGLE;
    /// <summary>
    /// 夹角
    /// </summary>
    public string LINE_FAULT_ANGLE
    {
        get { return _LINE_FAULT_ANGLE; }
        set { _LINE_FAULT_ANGLE = value; }
    }
    private string _PTROCK;
    /// <summary>
    /// 测点岩性
    /// </summary>
    public string PTROCK
    {
        get { return _PTROCK; }
        set { _PTROCK = value; }
    }
    private string _INSTRREPLACEDISCRIP;
    /// <summary>
    /// 仪器更换情况
    /// </summary>
    public string INSTRREPLACEDISCRIP
    {
        get { return _INSTRREPLACEDISCRIP; }
        set { _INSTRREPLACEDISCRIP = value; }
    }

    private string _STARTPOINTCODE;
    /// <summary>
    /// 起点
    /// </summary>
    public string STARTPOINTCODE
    {
        get { return _STARTPOINTCODE; }
        set { _STARTPOINTCODE = value; }
    }

    private string _ENDPOINTCODE;
    /// <summary>
    /// 终点
    /// </summary>
    public string ENDPOINTCODE
    {
        get { return _ENDPOINTCODE; }
        set { _ENDPOINTCODE = value; }
    }
}
