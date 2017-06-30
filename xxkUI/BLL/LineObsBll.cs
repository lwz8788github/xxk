using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.Dal;
using System.IO;
using xxkUI.Tool;

namespace xxkUI.Bll
{
    public class LineObsBll
    {
        public static LineObsBll Instance
        {
            get { return SingletonProvider<LineObsBll>.Instance; }
        }

        public int Add(LineObsBean model)
        {
           return LineObsDal.Instance.Insert(model);
        }

        public int Update(LineObsBean model)
        {
            return LineObsDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return LineObsDal.Instance.Delete(keyid);
        }

        public LineObsBean Get(int id)
        {
            return LineObsDal.Instance.Get(id);
        }

        public DataTable GetDataTable(string sql)
        {
            return LineObsDal.Instance.GetDataTable(sql);
        }
        public DataTable GetDataTable(string linecode, string path)
        {
            DataTable dt = null;

            string filename = path + "\\" + linecode + ".xls";
            if (File.Exists(filename))
            {
                NpoiCreator npcreator = new NpoiCreator();
                dt = npcreator.ExcelToDataTable_LineObs(filename, true);
            }

            return dt;
        }

        public IEnumerable<LineObsBean> GetAll()
        {
            return LineObsDal.Instance.GetAll();
        }


        internal List<string> GetNameByID(string p1, string p2, string lineCode)
        {
            throw new NotImplementedException();
        }
    }
}
