using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xxkUI.MyCls
{
   public class MyDragPoint : DragPoint
    {
      
       protected override void MouseEvent(Steema.TeeChart.MouseEventKinds kind, System.Windows.Forms.MouseEventArgs e, ref System.Windows.Forms.Cursor c)
       {
           base.MouseEvent(kind, e, ref c);

         
           if (kind == MouseEventKinds.Down)
           {
               
           }
           else if (kind == MouseEventKinds.Move)
           {

           }
           else if (kind == MouseEventKinds.Up)
           {
 
           }
       }
    }
}
