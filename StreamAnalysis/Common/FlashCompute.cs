using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Data;
using P2Wrap91;
using System.Runtime.InteropServices;

namespace ReliefAnalysis
{
    public  class FlashCompute
    {
       
        public  string compute(string fileContent, int iFirst, string firstValue,int iSecond,string secondValue,DataRow dr, string vapor,string liquid,string dir)
        {
            CP2ServerClass cp2Srv = new CP2ServerClass();
            cp2Srv.Initialize();

            string streamData = getStreamData(iFirst,firstValue,iSecond,secondValue,dr);
            string flashData = getFlashData(iFirst, firstValue, iSecond, secondValue, dr,vapor,liquid);
            StringBuilder sb = new StringBuilder();
            sb.Append(fileContent).Append(streamData).Append(flashData);
            string onlyFileName=dir + @"\" + Guid.NewGuid().ToString().Substring(0, 5);
            string inpFile = onlyFileName + ".inp";
            File.WriteAllText(inpFile, sb.ToString());
            int resultImport = cp2Srv.Import(inpFile);
            string przFile = onlyFileName + ".prz";
            CP2File cp2File = (CP2File)cp2Srv.OpenDatabase(przFile);
            int runResult = cp2Srv.RunCalcs(przFile);
            runResult = runResult + cp2Srv.GenerateReport(przFile);
            Marshal.FinalReleaseComObject(cp2Srv);
            GC.ReRegisterForFinalize(cp2Srv);

            return przFile;
            
        }
        private string getStreamData(int iFirst, string firstValue, int iSecond, string secondValue, DataRow dr)
        {
            StringBuilder data1 = new StringBuilder();
            string streamName = dr["streamname"].ToString();
            data1.Append("\tPROP STRM=").Append(streamName.ToUpper()).Append(",&\n");
            if (iFirst == 1)
            {
                data1.Append("\t PRES(MPAG)=").Append(firstValue).Append(",&\n");
            }
            else
            {
                data1.Append("\t TEMP(C)=").Append(firstValue).Append(",&\n");
            }
            if (iSecond == 1)
            {
                data1.Append("\t PRES(MPAG)=").Append(secondValue).Append(",&\n");
            }
            else if (iSecond == 2)
            {
                data1.Append("\t TEMP(C)=").Append(secondValue).Append(",&\n");
            }
            if (dr.Table.Columns.Contains("ProdType"))
            {
                if (dr["ProdType"].ToString() == "1" || dr["ProdType"].ToString() == "3")
                {
                    data1.Append("\t PHASE=V,&\n");
                }
                else
                {
                    data1.Append("\t PHASE=L,&\n");
                }
            }
            else
            {
                if (iSecond == 3)
                {
                    data1.Append("\t PHASE=V,&\n");
                }
                else
                {
                    data1.Append("\t PHASE=L,&\n");
                }
            }


            string rate = dr["TotalMolarRate"].ToString();
            if (rate == "")
                rate = "1";
            data1.Append("\t RATE(KGM/S)=").Append(rate).Append(",&\n");
            string com = dr["TotalComposition"].ToString();
            string Componentid = dr["Componentid"].ToString();
            string CompIn = dr["CompIn"].ToString();
            Dictionary<string, string> compdict = new Dictionary<string, string>();
            data1.Append("\t COMP=&\n");
            string[] coms = com.Split(',');
            string[] Componentids = Componentid.Split(',');
            string[] CompIns = CompIn.Split(',');
            StringBuilder sbCom = new StringBuilder();
            for (int i = 0; i < coms.Length; i++)
            {
                compdict.Add(Componentids[i], coms[i]);

            }
            foreach (string s in CompIns)
            {
                sbCom.Append("/&\n").Append(compdict[s]);
            }
            data1.Append("\t").Append(sbCom.Remove(0, 2)).Append("\n");
            return data1.ToString();
        }
        private string getFlashData(int iFirst, string firstValue, int iSecond, string secondValue, DataRow dr,string vapor,string liquid)
        {
            StringBuilder data2 = new StringBuilder("UNIT OPERATIONS\n");
            string streamName = dr["streamname"].ToString();
            Guid guid = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();
            string FlashName = "F_" + guid.ToString().Substring(0, 5).ToUpper();
            
            data2.Append("\tFLASH UID=").Append(FlashName).Append("\n");
            data2.Append("\t FEED ").Append(streamName.ToUpper()).Append("\n");
            data2.Append("\t PRODUCT V=").Append(vapor).Append(",&\n");
            data2.Append("\t L=").Append(liquid).Append("\n");

            if (iSecond == 1)
            {
                data2.Append("\t ISO PRES(MPAG)=").Append(secondValue).Append(",&\n");
                data2.Append("\t TEMP(C)=").Append(firstValue).Append("\n");
            }
            else if (iSecond == 2)
            {
                data2.Append("\t ISO TEMP(C)=").Append(secondValue).Append(",&\n");
                data2.Append("\t PRES(MPAG)=").Append(firstValue).Append("");
            }
            else if (iSecond == 3)
            {
                data2.Append("\t Dew ");
                if (iFirst == 1)
                {
                    data2.Append("PRES(MPAG)=").Append(firstValue).Append("\n");
                }
                else
                {
                    data2.Append("TEMP(C)=").Append(firstValue).Append("\n");
                }
            }
            else if (iSecond == 4)
            {
                data2.Append("\t Bubble ");
                if (iFirst == 1)
                {
                    data2.Append("PRES(MPAG)=").Append(firstValue).Append("\n");
                }
                else
                {
                    data2.Append("TEMP(C)=").Append(firstValue).Append("\n");
                }
            }
            else
            {
                data2.Append("\t ADIABATIC ");
                if (iFirst == 1)
                {
                    data2.Append("PRES(MPAG)=").Append(firstValue).Append(",&\n");

                }
                else
                {
                    data2.Append("TEMP(C)=").Append(firstValue).Append(",&\n");
                }
                data2.Append("\t Duty=").Append(secondValue).Append(",&\n");
            }

            data2.Append("\t DEFINE ERAT AS 1\n").Append("END");

            return data2.ToString();
        }
       
    }



}
