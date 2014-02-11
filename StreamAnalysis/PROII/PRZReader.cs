using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;
using SharpCompress;
using SharpCompress.Reader;
using SharpCompress.Common;

using P2Wrap91;

namespace ReliefAnalysis
{
    public class PRZReader
    {
        string[] arrStreamAttributes = { "Pressure", "Temperature", "VaporFraction", "VaporZFmKVal", "TotalComposition", "TotalMolarEnthalpy", "TotalMolarRate", "InertWeightEnthalpy", "InertWeightRate" };
        string[] arrBulkPropAttributes = { "BulkMwOfPhase", "BulkDensityAct", "BulkViscosity", "BulkCPCVRatio", "BulkCP", "BulkThermalCond", "BulkSurfTension" };
       
        string[] arrColumnAttributes = { "NumberOfTrays",  "HeaterNames", "HeaterDuties", "HeaterNumber", "HeaterPANumberfo", "HeaterRegOrPAFlag", "HeaterTrayLoc", "HeaterTrayNumber" };
        string[] arrColumnInAttributes = { "ProdType", "FeedTrays","ProdTrays", "FeedData", "ProductData"};
        string[] arrEqListAttributes = { "FeedData", "ProductData", "PressureDrop", "Duty" };

        string[] customEqListAttributes = { "ID", "StreamName", "SourceFile", "EqType", "EqName" };
        string[] customStreamAttributes = { "ID", "StreamName", "SourceFile" };

        
        


        string przFile;
        CP2File cp2File;
        CP2Object objCompCalc;
        CP2ServerClass cp2Srv;
        string fileName = string.Empty;
        public PRZReader(string File)
        {
            przFile = File;
            cp2Srv =new CP2ServerClass();
            cp2Srv.Initialize();
            fileName = System.IO.Path.GetFileName(przFile);
            //cp2File = (CP2File)cp2Srv.OpenDatabase(przFile);
            //////cp2Srv.RunCalcs(przFile);
            //objCompCalc = (CP2Object)cp2File.ActivateObject("CompCalc", "CompCalc");
        }
        
        //private void CloseReader()
        //{
            //Marshal.FinalReleaseComObject(objCompCalc);
            //GC.ReRegisterForFinalize(objCompCalc);
            //Marshal.FinalReleaseComObject(cp2File);
            //GC.ReRegisterForFinalize(cp2File);
            
            
       // }

        //复制塔的首层物流
        public DataTable  copyTray1Stream2(string columnName)
        {
            
            
            DBRelief dbR = new DBRelief();
            DataTable dt = dbR.getStructure("stream");

            string streamName = "TEMP" + Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
            CP2Object tempStream = (CP2Object)cp2File.CreateObject("Stream", streamName);

            bool b = cp2File.CopyTrayToStream(columnName, 1, (p2Phase)2, 0, (p2TrayFlow)1, streamName);

            string bb = b.ToString();
            DataRow dr = dt.NewRow();
            bool bCalulate = cp2File.CalculateStreamProps(streamName);

            CP2Object compCalc = (CP2Object)cp2File.ActivateObject("CompCalc", "CompCalc");
            object ComponentId = compCalc.GetAttribute("ComponentId");
            if (ComponentId != null && ComponentId is Array)
            {
                dr["ComponentId"] = UnitConverter.convertData(ComponentId);
            }
            else
            {
                dr["ComponentId"] = ComponentId;
            }
            object CompIn = cp2File.GetObjectNames("CompIn");
            if (CompIn != null && ComponentId is Array)
            {
                dr["CompIn"] = UnitConverter.convertData(CompIn);
            }
            else
            {
                dr["CompIn"] = CompIn;
            }
            dr["streamname"] = streamName;
            dr["sourcefile"] = przFile;
            dr["tray"] = 1;
            dr["prodtype"] = 3;
            CP2Object curStream = (CP2Object)cp2File.ActivateObject("Stream", streamName);
            foreach (string s in arrStreamAttributes)
            {
                object v = curStream.GetAttribute(s);
                if (v != null && v is Array)
                {
                    dr[s] = UnitConverter.convertData(v);
                }
                else
                {
                    dr[s] = v;
                }
            }
            if (bCalulate)
            {
                CP2Object bulkDrop = (CP2Object)cp2File.ActivateObject("SrBulkProp", streamName);
                foreach (string s in arrBulkPropAttributes)
                {
                    object v = bulkDrop.GetAttribute(s);
                    if (v != null && v is Array)
                    {
                        dr[s] = UnitConverter.convertData(v);
                    }
                    else
                    {
                        dr[s] = v;
                    }
                }
            }

            cp2File.DeleteObject("Stream", streamName);
            dt.Rows.Add(dr);
            //CloseReader();
            //Marshal.FinalReleaseComObject(cp2Srv1);
            //GC.ReRegisterForFinalize(cp2Srv1);
            //Marshal.FinalReleaseComObject(tempStream);
            //GC.ReRegisterForFinalize(tempStream);
           // Marshal.FinalReleaseComObject(curStream);
           // GC.ReRegisterForFinalize(curStream);
            
            return dt;
        }

        //复制塔的首层物流
        public DataTable copyTray1Stream(string columnName)
        {          
            cp2File = (CP2File)cp2Srv.OpenDatabase(przFile);            
            DBRelief dbR = new DBRelief();
            DataTable dt = dbR.getStructure("stream");

            string streamName = "TEMP" + Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
            CP2Object tempStream = (CP2Object)cp2File.CreateObject("Stream", streamName);

            bool b = cp2File.CopyTrayToStream(columnName, 1, (p2Phase)2, 0, (p2TrayFlow)1, streamName);

            string bb = b.ToString();
            DataRow dr = dt.NewRow();
            bool bCalulate = cp2File.CalculateStreamProps(streamName);
            objCompCalc = (CP2Object)cp2File.ActivateObject("CompCalc", "CompCalc");
            object ComponentId = objCompCalc.GetAttribute("ComponentId");
            if (ComponentId != null && ComponentId is Array)
            {
                dr["ComponentId"] = UnitConverter.convertData(ComponentId);
            }
            else
            {
                dr["ComponentId"] = ComponentId;
            }
            object CompIn = cp2File.GetObjectNames("CompIn");
            if (CompIn != null && ComponentId is Array)
            {
                dr["CompIn"] = UnitConverter.convertData(CompIn);
            }
            else
            {
                dr["CompIn"] = CompIn;
            }
            dr["streamname"] = streamName;
            dr["sourcefile"] = fileName;
            dr["tray"] = 1;
            dr["prodtype"] = 3;
            CP2Object curStream = (CP2Object)cp2File.ActivateObject("Stream", streamName);
            foreach (string s in arrStreamAttributes)
            {
                object v = curStream.GetAttribute(s);
                if (v != null && v is Array)
                {
                    dr[s] = UnitConverter.convertData(v);
                }
                else
                {
                    dr[s] = v;
                }
            }
            if (bCalulate)
            {
                CP2Object bulkDrop = (CP2Object)cp2File.ActivateObject("SrBulkProp", streamName);
                foreach (string s in arrBulkPropAttributes)
                {
                    object v = bulkDrop.GetAttribute(s);
                    if (v != null && v is Array)
                    {
                        dr[s] = UnitConverter.convertData(v);
                    }
                    else
                    {
                        dr[s] = v;
                    }
                }
            }

            //cp2File.DeleteObject("Stream", streamName);
            dt.Rows.Add(dr);
           
            return dt;
        }

      
        public DataSet getDataFromFile()
        {
            try
            {
                DBRelief dbRelief = new DBRelief();
                List<string> streamList = new List<string>();
                DataSet dsStructure = dbRelief.getDataStructure();
                DataTable dtEqlist = dsStructure.Tables["eqlist"].Clone();
                DataTable dtStream = dsStructure.Tables["stream"].Clone();
                DataTable dtEqType = dsStructure.Tables["eqtype"];
                cp2File = (CP2File)cp2Srv.OpenDatabase(przFile);
                objCompCalc = (CP2Object)cp2File.ActivateObject("CompCalc", "CompCalc");
                foreach (DataRow dr in dtEqType.Rows)
                {
                    string otype = dr["eqtypename"].ToString();
                    object objectnames = cp2File.GetObjectNames(otype);
                    if (objectnames.ToString() != "")
                    {
                        if (objectnames is Array)
                        {
                            string[] oNames = (string[])objectnames;
                            foreach (string name in oNames)
                            {                                
                                getEqDataFromFile(otype, name,ref streamList, ref dtEqlist,ref dtStream);                                
                            }
                        }
                        else
                        {
                            string name = (string)objectnames;
                            getEqDataFromFile(otype, name,ref streamList, ref dtEqlist, ref dtStream);     
                        }
                    }
                }
                Marshal.FinalReleaseComObject(cp2Srv);
                GC.ReRegisterForFinalize(cp2Srv);
          
                DataSet ds = new DataSet();
                ds.Tables.Add(dtStream);
                ds.Tables.Add(dtEqlist);                   
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            finally
            {
                //CloseReader();
            }

        }



        private void getEqDataFromFile(string otype, string name, ref List<string> streamList, ref DataTable dtEqlist, ref DataTable dtStream)
        {
            DataRow r = dtEqlist.NewRow();
            CP2Object eq = (CP2Object)cp2File.ActivateObject(otype, name);
            object feeddata = eq.GetAttribute("FeedData");
            object productdata = eq.GetAttribute("ProductData");
            string strfeeddata = UnitConverter.convertData(feeddata);
            string strproductdata = UnitConverter.convertData(productdata);
            string strprodtype = string.Empty;
            r["eqtype"] = otype;
            r["eqname"] = name;
            r["sourcefile"] = fileName;
            r["FeedData"] = strfeeddata;
            r["ProductData"] = strproductdata;
            if (otype == "Column")
            {
                foreach (string s in arrColumnAttributes)
                {
                    object v = eq.GetAttribute(s);
                    string strV = UnitConverter.convertData(v);
                    r[s] = strV;
                }
                P2Wrap91.CP2Object obj = (P2Wrap91.CP2Object)cp2File.ActivateObject("ColumnIn", name);
                foreach (string s in arrColumnInAttributes)
                {
                    object v = obj.GetAttribute(s);
                    string strV = UnitConverter.convertData(v);
                    r[s] = strV;
                }


                Marshal.FinalReleaseComObject(obj);
                GC.ReRegisterForFinalize(obj);

            }
            dtEqlist.Rows.Add(r);
            Marshal.FinalReleaseComObject(eq);
            GC.ReRegisterForFinalize(eq);


            string[] feeds = r["feeddata"].ToString().Split(',');
            string[] feedtrays = r["feedtrays"].ToString().Split(',');
            for (int i = 0; i < feeds.Length; i++)
            {
                string s = feeds[i];
                if (!streamList.Contains(s))
                {
                    streamList.Add(s);
                    if (otype == "Column")
                        getStreamDataFromFile(s, "", feedtrays[i], ref dtStream);
                    else
                        getStreamDataFromFile(s, "", "", ref dtStream);

                }
            }
            string[] prods = r["ProductData"].ToString().Split(',');
            string[] prodtypes = r["prodtype"].ToString().Split(',');
            string[] prodtrays = r["prodtrays"].ToString().Split(',');
            for (int i = 0; i < prods.Length; i++)
            {
                string s = prods[i];
                if (!streamList.Contains(s))
                {
                    streamList.Add(s);
                    if (otype == "Column")
                        getStreamDataFromFile(s, prodtypes[i], prodtrays[i], ref dtStream);
                    else
                        getStreamDataFromFile(s, "", "", ref dtStream);

                }
            }
        }
        private void getStreamDataFromFile(string name, string prodtype, string tray,  ref DataTable dtStream)
        {
            
            DataRow r = dtStream.NewRow();
            bool bCalulate = cp2File.CalculateStreamProps(name);
            r["streamname"] = name;
            r["sourcefile"] = fileName;
            r["prodtype"] = prodtype;
            r["tray"] = tray;
            object ComponentId = objCompCalc.GetAttribute("ComponentId");
            if (ComponentId != null && ComponentId  is Array)
            {
                r["ComponentId"] = UnitConverter.convertData(ComponentId);
            }
            else
            {
                r["ComponentId"] = ComponentId;
            }
            object CompIn = cp2File.GetObjectNames("CompIn");
            if (CompIn != null && ComponentId is Array)
            {
                r["CompIn"] = UnitConverter.convertData(CompIn);
            }
            else
            {
                r["CompIn"] = CompIn;
            }

            CP2Object objStream = (CP2Object)cp2File.ActivateObject("Stream", name);

            foreach (string s in arrStreamAttributes)
            {
                object v = objStream.GetAttribute(s);
                if (v != null && v is Array)
                {
                    r[s] = UnitConverter.convertData(v);
                }
                else
                {
                    r[s] = v;
                }
            }
            Marshal.FinalReleaseComObject(objStream);
            GC.ReRegisterForFinalize(objStream);
            if (bCalulate)
            {
                CP2Object objBulkDrop = (CP2Object)cp2File.ActivateObject("SrBulkProp", name);
                foreach (string s in arrBulkPropAttributes)
                {
                    object v = objBulkDrop.GetAttribute(s);
                    if (v != null && v is Array)
                    {
                        r[s] = UnitConverter.convertData(v);
                    }
                    else
                    {
                        r[s] = v;
                    }
                }
                Marshal.FinalReleaseComObject(objBulkDrop);
                GC.ReRegisterForFinalize(objBulkDrop);

            }

            dtStream.Rows.Add(r);
        }



        
       
    
  
    }
    public static class InpReader
    {
        public static string getUsableContent(string przFile,string streamName,string rootDir)
        {
            StringBuilder sb = new StringBuilder();
            using (Stream stream = File.OpenRead(przFile))
            {
                var reader = ReaderFactory.Open(stream);
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        string dir = Guid.NewGuid().ToString();
                        string tempdir = rootDir+@"\temp\" + dir + @"\";
                        reader.WriteEntryToDirectory(tempdir, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                        string inpFile = reader.Entry.FilePath;
                        string frontInpFile = inpFile.Substring(0, inpFile.Length - 4);
                        if (inpFile.Substring(inpFile.Length - 4, 4) == ".inp")
                        {
                            string sourceFile = tempdir + inpFile;
                            string[] lines = System.IO.File.ReadAllLines(sourceFile);
                            List<int> list = new List<int>();
                            int i = 0;
                            while (i < lines.Length)
                            {
                                string s = lines[i];
                                if (s.Trim().IndexOf("NAME") == 0 || s.Trim().IndexOf("UNIT") == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    int idx=s.IndexOf("REFSTREAM");
                                    if (idx==-1)
                                    {
                                       sb.Append(s).Append("\n");
                                    }
                                    else
                                    {
                                        string subS = s.Substring(idx);
                                        int spitIdx = subS.IndexOf(",");
                                        if (spitIdx > -1)
                                        {
                                            string old = subS.Substring(0, spitIdx);
                                            s=s.Replace(old, "REFSTREAM=" + streamName);
                                        }
                                        else
                                        {
                                           s= s.Replace(subS, "REFSTREAM=" + streamName);
                                        }
                                        sb.Append(s).Append("\n");
                                    }
                                    i++;
                                }

                            }
                        }
                    }
                }
            }



            return sb.ToString();
        }
    }

    public class PRZWriter
    {
        string[] arrStreamAttributes = { "Pressure", "Temperature", "VaporFraction", "VaporZFmKVal", "TotalComposition", "TotalMolarEnthalpy", "TotalMolarRate", "InertWeightEnthalpy", "InertWeightRate", "Tray" };
        string[] arrBulkPropAttributes = { "BulkMwOfPhase", "BulkDensityAct", "BulkViscosity", "BulkCPCVRatio", "BulkCP", "BulkThermalCond", "BulkSurfTension" };

        public DataTable copyStream(string przFile, string columnName)
        {
            P2Wrap91.CP2ServerClass cp2Srv = new CP2ServerClass();
            cp2Srv.Initialize();
            P2Wrap91.CP2File cp2File = (CP2File)cp2Srv.OpenDatabase(przFile);
            DBRelief dbR = new DBRelief();
            DataTable dt = dbR.getStructure("stream");

            string streamName = "temp" + Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
            CP2Object tempStream = (CP2Object)cp2File.CreateObject("Stream", streamName);

            bool b = cp2File.CopyTrayToStream(columnName, 1, (p2Phase)2, 0, (p2TrayFlow)1, streamName);

            string bb = b.ToString();
            DataRow dr = dt.NewRow();
            bool bCalulate = cp2File.CalculateStreamProps(streamName);

            CP2Object compCalc = (CP2Object)cp2File.ActivateObject("CompCalc", "CompCalc");
            object ComponentId = compCalc.GetAttribute("ComponentId");
            if (ComponentId != null && ComponentId is Array)
            {
                dr["ComponentId"] = convertdata(ComponentId);
            }
            else
            {
                dr["ComponentId"] = ComponentId;
            }
            object CompIn = cp2File.GetObjectNames("CompIn");
            if (CompIn != null && ComponentId is Array)
            {
                dr["CompIn"] = convertdata(CompIn);
            }
            else
            {
                dr["CompIn"] = CompIn;
            }
            dr["streamname"] = streamName;
            dr["sourcefile"] = przFile;
            dr["tray"] = 1;
            dr["prodtype"] = 2;
            CP2Object curStream = (CP2Object)cp2File.ActivateObject("Stream", streamName);
            foreach (string s in arrStreamAttributes)
            {
                object v = curStream.GetAttribute(s);
                if (v != null && v is Array)
                {
                    dr[s] = convertdata(v);
                }
                else
                {
                    dr[s] = v;
                }
            }
            if (bCalulate)
            {
                CP2Object bulkDrop = (CP2Object)cp2File.ActivateObject("SrBulkProp", streamName);
                foreach (string s in arrBulkPropAttributes)
                {
                    object v = bulkDrop.GetAttribute(s);
                    if (v != null && v is Array)
                    {
                        dr[s] = convertdata(v);
                    }
                    else
                    {
                        dr[s] = v;
                    }
                }
            }

            cp2File.DeleteObject("Stream", streamName);
            dt.Rows.Add(dr);
            Marshal.FinalReleaseComObject(cp2Srv);
            GC.ReRegisterForFinalize(cp2Srv);

            return dt;
        }

        private string convertdata(object obj)
        {
            string rs = string.Empty;
            if (obj is Array)
            {
                object[] arr = (object[])obj;
                foreach (object s in arr)
                {
                    if (s.ToString() != string.Empty)
                    {
                        rs = rs + "," + s;
                    }
                }
                rs = rs.Substring(1);
            }
            else
                rs = obj.ToString();

            return rs;
        }

    }

    
}
