using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class PRZPicker
    {
        string[] arrStreamAttributes = { "Pressure", "Temperature", "VaporFraction", "VaporZFmKVal", "TotalComposition", "TotalMolarEnthalpy", "TotalMolarRate", "InertWeightEnthalpy", "InertWeightRate" };
        string[] arrBulkPropAttributes = { "BulkMwOfPhase", "BulkDensityAct", "BulkViscosity", "BulkCPCVRatio", "BulkCP", "BulkThermalCond", "BulkSurfTension" };

        string[] arrColumnAttributes = { "NumberOfTrays", "HeaterNames", "HeaterDuties", "HeaterNumber", "HeaterPANumberfo", "HeaterRegOrPAFlag", "HeaterTrayLoc", "HeaterTrayNumber" };
        string[] arrColumnInAttributes = { "ProdType", "FeedTrays", "ProdTrays", "FeedData", "ProductData" };
        string[] arrEqListAttributes = { "FeedData", "ProductData", "PressureDrop", "Duty" };

        string[] customEqListAttributes = { "ID", "StreamName", "SourceFile", "EqType", "EqName" };
        string[] customStreamAttributes = { "ID", "StreamName", "SourceFile" };


        ArrayList arrFeedData = new ArrayList();
        ArrayList arrFeedTrays = new ArrayList();
        ArrayList arrProductData = new ArrayList();
        ArrayList arrProdTrays = new ArrayList();
        ArrayList arrProdTypes = new ArrayList();


        string przFile;
        CP2File cp2File;
        CP2Object objCompCalc;
        CP2ServerClass cp2Srv;
        string fileName = string.Empty;
        string ComponentIds = string.Empty;
        string CompIns = string.Empty;
        public PRZPicker(string File)
        {
            przFile = File;
            cp2Srv =new CP2ServerClass();
            cp2Srv.Initialize();
            fileName = System.IO.Path.GetFileName(przFile); 
            cp2File = (CP2File)cp2Srv.OpenDatabase(przFile);
            objCompCalc = (CP2Object)cp2File.ActivateObject("CompCalc", "CompCalc");
            object ComponentId = objCompCalc.GetAttribute("ComponentId");
            if (ComponentId != null && ComponentId is Array)
            {
                ComponentIds = UnitConverter.convertData(ComponentId);
            }
            else
            {
                if (ComponentId == null)
                    ComponentIds = string.Empty;
                else
                    ComponentIds = ComponentId.ToString();
            }
            object CompIn = cp2File.GetObjectNames("CompIn");
            if (CompIn != null && ComponentId is Array)
            {
                CompIns = UnitConverter.convertData(CompIn);
            }
            else
            {
                if (CompIn == null)
                    CompIns = string.Empty;
                else
                    CompIns = CompIn.ToString();
            }
        }
        //获得设备和物流线的个数和名字信息
        public int getAllEqAndStreamCount(DataTable dtEqType,ref ArrayList eqList, ref ArrayList streamList)
        {
            int count = 0;           
            foreach (DataRow dr in dtEqType.Rows)
            {
                string otype = dr["eqtypename"].ToString();
                int ct = cp2File.GetObjectCount(otype);
                if (ct > 0)
                {
                    object objectnames = cp2File.GetObjectNames(otype);
                    if (objectnames is Array)
                    {
                        string[] oNames = (string[])objectnames;
                        foreach (string name in oNames)
                        {
                            EqInfo eq = new EqInfo();
                            eq.eqName = name;
                            eq.eqType = otype;
                            if (otype == "Column")
                            {
                                eq.isColumn = true;
                            }
                            eqList.Add(eq);
                        }
                    }
                    else
                    {
                        EqInfo eq = new EqInfo();
                        eq.eqName = objectnames.ToString();
                        eq.eqType = otype;
                        if (otype == "Column")
                        {
                            eq.isColumn = true;
                        }
                        eqList.Add(eq);
                    }
                    count = count + ct;
                }
            }
            int streamCount = cp2File.GetObjectCount("Stream");
            count = streamCount + count;
            object streamnames = cp2File.GetObjectNames("Stream");
            if (streamnames is Array)
            {
                string[] oNames = (string[])streamnames;
                foreach (string name in oNames)
                {
                    streamList.Add(name);
                }
            }
            else
            {
                streamList.Add(streamnames.ToString());
            }
            
            return count;
        }

        public void getEqInfo(string otype, string name, ref DataTable dtEqList)
        {
            DataRow r = dtEqList.NewRow();
            CP2Object eq = (CP2Object)cp2File.ActivateObject(otype, name);
           
            r["eqtype"] = otype;
            r["eqname"] = name;
            r["sourcefile"] = fileName;          
            if (otype == "Column" || otype=="SideColumn")
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
                string[] feeds = r["feeddata"].ToString().Split(',');
                string[] feedtrays = r["feedtrays"].ToString().Split(',');
                string[] prods = r["ProductData"].ToString().Split(',');
                string[] prodtypes = r["prodtype"].ToString().Split(',');
                string[] prodtrays = r["prodtrays"].ToString().Split(',');
                for(int i=0;i<feeds.Length;i++)
                {
                    arrFeedData.Add(feeds[i]);
                    arrFeedTrays.Add(feedtrays[i]);
                }
                for (int i = 0; i < prods.Length; i++)
                {
                    arrProductData.Add(prods[i]);
                    arrProdTrays.Add(prodtrays[i]);
                    arrProdTypes.Add(prodtypes[i]);
                }


            }
            dtEqList.Rows.Add(r);
            //Close();
        }


        public void getSteamInfo(string name, ref DataTable dtStream)
        {           
            DataRow r = dtStream.NewRow();
            bool bCalulate = cp2File.CalculateStreamProps(name);
            r["streamname"] = name;
            r["sourcefile"] = fileName;
            r["prodtype"] = "";
            r["tray"] = "";
            int idx = arrFeedData.IndexOf(name);
            if (idx >= 0)
            {
                r["tray"] = arrFeedTrays[idx];
            }
            idx = arrProductData.IndexOf(name);
            if (idx >= 0)
            {
                r["tray"] = arrProdTrays[idx];
                r["prodtype"] = arrProdTypes[idx];
            }



            r["CompIn"] = CompIns;
            r["ComponentId"] = ComponentIds;
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

        public void Close()
        {
            Marshal.FinalReleaseComObject(cp2Srv);
            GC.ReRegisterForFinalize(cp2Srv);
        }
    }
    public class EqInfo
    {
        public string eqName;
        public string eqType;
        public bool isColumn;
    }
}
