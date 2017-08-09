using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xxkUI.MyCls
{
    public class EqkArraCls
    {
        public double x1,  y1;
        public string text;
        public EqkArraCls(double _x1, double _y1, string _text)
        {
            this.x1 = _x1;
            this.y1 = _y1;
            text = _text;
        }
        public void move(double xStep, double yStep)
        {
            x1 += xStep;
            y1 += yStep;
        }

    }
}
