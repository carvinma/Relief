using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReliefAnalysis
{
    public class UnitConverter
    {
        public static string unitConv(string param,string sourcetype,string targetype,string format)
        {
            //string.Format("{0:000.000}", 12.2);
            string result=param;
            if (sourcetype.ToUpper() == "K" && targetype == "C")
            {
                double temp = double.Parse(param) - 273.15;
                result = string.Format(format, temp);
            }
            if (sourcetype.ToUpper() == "KPA" && targetype == "MPAG")
            {
                double temp = double.Parse(param)/1000-0.10135;
                result = string.Format(format, temp);
            }
            if (sourcetype.ToUpper() == "KG/SEC" && targetype == " KG/HR")
            {
                double temp = double.Parse(param)/3600;
                result = string.Format(format, temp);
            }
            if (sourcetype.ToUpper() == "M3/SEC" && targetype == "M3/HR")
            {
                double temp = double.Parse(param)/3600;
                result = string.Format(format, temp);
            }
            
            if (sourcetype.ToUpper() == "PAS" && targetype == "CP")
            {
                //1P=0.1PaS=100CP=100mPaS
                double temp = double.Parse(param)*1000;
                result = string.Format(format, temp);
            }
            return result;
        }

        public static string convertData(object obj)
        {
            string rs = string.Empty;
            if (obj is Array)
            {
                object[] objdata = (System.Object[])obj;
                foreach (object s in objdata)
                {
                    if (s.ToString() != string.Empty)
                    {
                        rs = rs + "," + s;
                    }
                }
                rs = rs.Substring(1);
            }
            else if (obj == null)
            {
                rs = "";
            }
            else
                rs = obj.ToString();

            return rs;
        }
    }
}
