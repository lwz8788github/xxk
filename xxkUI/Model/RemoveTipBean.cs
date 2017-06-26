/***********************************************************/
//---模    块：RemoveTipBean
//---功能描述：消突跳或台阶预处理返回信息属性
//---编码时间：2017-05-24
//---编码人员：张超
//---单    位：一测中心
/***********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xxkUI.Model
{
    public class RemoveTipBean
    {

        private string _LeftAve;
        private string _RightAve;
        private string _BothAve;
        private string _TargetNum;

        private int _Totalleft;
        private int _Totalright;

        private double _LeftSetp;
        private double _RightSetp;

        private double _Offset;

        private DateTime _StartSel;
        private DateTime _EndSel;

        private int _TipRowIndex;
        private string _Tip;


        /// <summary>
        /// 左侧采样平均值
        /// </summary>
        public string LeftAve
        {
            get
            {
                return _LeftAve;
            }

            set
            {
                _LeftAve = value;
            }
        }

        /// <summary>
        /// 右侧采样平均值
        /// </summary>
        public string RightAve
        {
            get
            {
                return _RightAve;
            }

            set
            {
                _RightAve = value;
            }
        }

        /// <summary>
        /// 两侧采样平均值
        /// </summary>
        public string BothAve
        {
            get
            {
                return _BothAve;
            }

            set
            {
                _BothAve = value;
            }
        }

        /// <summary>
        /// 左侧总数据数
        /// </summary>
        public int Totaleft
        {
            get
            {
                return _Totalleft;
            }

            set
            {
                _Totalleft = value;
            }
        }

        /// <summary>
        /// 右侧总数据数
        /// </summary>
        public int Totalright
        {
            get
            {
                return _Totalright;
            }

            set
            {
                _Totalright = value;
            }
        }

        /// <summary>
        /// 移动台阶左偏量
        /// </summary>
        public double LeftSetp
        {
            get
            {
                return _LeftSetp;
            }

            set
            {
                _LeftSetp = value;
            }
        }

        /// <summary>
        /// /移动台阶右偏量
        /// </summary>
        public double RightSetp
        {
            get
            {
                return _RightSetp;
            }

            set
            {
                _RightSetp = value;
            }
        }

        /// <summary>
        /// //提示语句(备注信息）
        /// </summary>
        public string Tip
        {
            get
            {
                return _Tip;
            }

            set
            {
                _Tip = value;
            }
        }

        public DateTime StartSel
        {
            get
            {
                return _StartSel;
            }

            set
            {
                _StartSel = value;
            }
        }

        public DateTime EndSel
        {
            get
            {
                return _EndSel;
            }

            set
            {
                _EndSel = value;
            }
        }

        /// <summary>
        /// 偏移值
        /// </summary>
        public double Offset
        {
            get
            {
                return _Offset;
            }

            set
            {
                _Offset = value;
            }
        }
        /// <summary>
        /// 待处理目标数
        /// </summary>
        public string TargetNum
        {
            get
            {
                return _TargetNum;
            }

            set
            {
                _TargetNum = value;
            }
        }
        /// <summary>
        /// 备注存放的位置
        /// </summary>
        public int TipRowIndex
        {
            get
            {
                return _TipRowIndex;
            }

            set
            {
                _TipRowIndex = value;
            }
        }
    }
}
