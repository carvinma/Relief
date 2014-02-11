using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections;
namespace ReliefAnalysis
{
    public class DBRelief
    {
        public string connectString;
        public OleDbConnection conn = new OleDbConnection(); 
        public DBRelief(string dbFile)
        {
            connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbFile + ";Persist Security Info=False;";
            conn = new OleDbConnection(connectString);
        }
        public DBRelief()
        {
            string dbFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "template.accdb";
            connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbFile + ";Persist Security Info=False;";
            conn = new OleDbConnection(connectString);
        }

        public DataSet getDataStructure()
        {
            DataSet ds = new DataSet();
            DataSet dsEqList = new DataSet();          
            DataSet dsStream = new DataSet();
            //DataSet dsvEqList = new DataSet();
            //DataSet dsvStream = new DataSet();
            DataSet dsEqType = new DataSet();

            OleDbCommand cmd = new OleDbCommand("select * from eqlist where 1>1", conn);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(dsEqList);

            cmd = new OleDbCommand("select * from stream where 1>1", conn);
            da = new OleDbDataAdapter(cmd);
            da.Fill(dsStream);

           

            cmd = new OleDbCommand("select * from eqtype ", conn);
            da = new OleDbDataAdapter(cmd);
            da.Fill(dsEqType);


            DataTable dt1 = dsEqList.Tables[0].Copy();
            dt1.TableName = "eqlist";
            DataTable dt2 = dsStream.Tables[0].Copy();
            dt2.TableName = "stream";
           

            DataTable dt5 = dsEqType.Tables[0].Copy();
            dt5.TableName = "eqtype";

            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);
          
            ds.Tables.Add(dt5);
            return ds;

        }


        #region 获取ProII设备类型
        public DataSet getEqTypeList()
        {
            try
            {
                DataSet ds = new DataSet();
                string sql = "select * from eqtype order by id";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 获取ProII设备数据
        public DataSet getEqSourceFileList()
        {
            try
            {
                DataSet ds = new DataSet();
                string sql = "select distinct sourcefile from eqlist";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }        
        public DataSet getEqlistBySourceFile(string fileName)
        {
            try
            {
                DataSet ds = new DataSet();
                OleDbConnection conn = new OleDbConnection(connectString);
                string sql = "select * from eqlist where sourcefile='"+fileName+"'";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet getEqlistBySourceFile()
        {
            try
            {
                DataSet ds = new DataSet();
                OleDbConnection conn = new OleDbConnection(connectString);
                string sql = "select * from eqlist ";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public DataSet getEqData(string eqname)
        {
            try
            {
                DataSet ds = new DataSet();
                OleDbConnection conn = new OleDbConnection(connectString);
                string sql = "select * from eqlist where eqname='" + eqname + "'";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet getEqList(string eqType,string sourceFile)
        {
            try
            {
                DataSet ds = new DataSet();
                OleDbConnection conn = new OleDbConnection(connectString);
                string sql = "select * from eqlist where eqtype='" + eqType + "' and sourcefile='"+sourceFile+"'";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        
        
        
        
        
        
        #endregion


        #region 获取本系统设备数据
        public DataSet getVEqList()
        {
            try
            {
                DataSet ds = new DataSet();
                string sql = "select * from veqlist";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet getVEqList(string eqType)
        {
            try
            {
                DataSet ds = new DataSet();
                OleDbConnection conn = new OleDbConnection(connectString);
                string sql = "select * from eqlist where eqtype='" + eqType + "'";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet getVEqData(string eqname)
        {
            try
            {
                DataSet ds = new DataSet();
                OleDbConnection conn = new OleDbConnection(connectString);
                string sql = "select * from veqlist where eqname='" + eqname + "'";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 获取ProII Stream数据
        public DataSet getStreamByName(string streamName)
        {
            try
            {
                DataSet ds = new DataSet();
                string sql = "select * from stream  where streamName='" + streamName + "'";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet getStreamSourceFileList()
        {
            try
            {
                DataSet ds = new DataSet();
                OleDbConnection conn = new OleDbConnection(connectString);
                string sql = "select distinct sourcefile from stream";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet getStreamList(string sourceFile)
        {
            try
            {
                DataSet ds = new DataSet();
                OleDbConnection conn = new OleDbConnection(connectString);
                string sql = "select * from stream where sourcefile='"+sourceFile+"'";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet getStreamListByEq(string eqName,string FeedOrProduct)
        {
            try
            {
                Dictionary<string,string> dictFeeds=new Dictionary<string,string>();
                Dictionary<string, string> dicProducts = new Dictionary<string, string>();
                getMaincolumnRealFeedProduct(eqName, ref dictFeeds, ref dicProducts);
                StringBuilder realstreams = new StringBuilder();
                if (FeedOrProduct == "ProductData")
                {
                    foreach (string s in dicProducts.Keys)
                    {
                        realstreams.Append(",'").Append(s).Append("'");
                    }

                }
                else
                {
                    foreach (string s in dictFeeds.Keys)
                    {
                        realstreams.Append(",'").Append(s).Append("'") ;
                    }
                }
                DataSet ds = new DataSet();
                DataSet ds2 = new DataSet();
                OleDbConnection conn = new OleDbConnection(connectString);
                string sql = "select " + FeedOrProduct + " from eqlist  where eqname='" + eqName + "'";
                
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                 
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    StringBuilder sb = new StringBuilder();
                   
                    string[] streams=dr[FeedOrProduct].ToString().Split(',');
                    for(int i=0;i<streams.Length;i++) 
                    {
                        string s=streams[i].Trim();
                        if (s != string.Empty)
                        {
                            sb.Append(",'").Append(s).Append("'");                            
                        }
                    }
                    sb.Remove(0, 1);
                    sql = "select * from stream  where streamname in (" +sb.ToString()  + ")";
                    if (streams.ToString() != string.Empty)
                    {
                        sql = "select * from stream  where   streamname in ("+realstreams.ToString().Substring(1)+")";
                    }
                    cmd = new OleDbCommand(sql, conn);
                    da = new OleDbDataAdapter(cmd);
                    da.Fill(ds2); 
                    

                    if (ds2 != null && ds2.Tables.Count > 0)
                    {
                        DataTable dt = ds2.Tables[0];
                        DataColumn dcflowrate = new DataColumn("flowrate");
                        DataColumn dcSpH = new DataColumn("specificenthalpy");
                        DataColumn dcH = new DataColumn("enthalpy");

                        dt.Columns.Add(dcflowrate);
                        dt.Columns.Add(dcSpH);
                        dt.Columns.Add(dcH);
                        


                        for (int i = 0; i < dt.Rows.Count;i++ )
                        {                           
                            DataRow dr2 = dt.Rows[i];
                            string temperature = dr2["Temperature"].ToString();
                            if (temperature != "")
                            {
                                dr2["Temperature"] = UnitConverter.unitConv(temperature, "K", "C", "{0:0.0000}");
                            }
                            string pressure = dr2["Pressure"].ToString();
                            if (pressure != "")
                            {
                                dr2["pressure"] = UnitConverter.unitConv(pressure, "KPA", "MPAG", "{0:0.0000}");
                            }


                            double TotalMolarRate = 0;
                            if (dr2["TotalMolarRate"].ToString() != "")
                                TotalMolarRate = double.Parse(dr2["TotalMolarRate"].ToString());
                            string bulkmwofphase = dr2["BulkMwOfPhase"].ToString();
                            if (bulkmwofphase != "")
                            {
                                double wf = TotalMolarRate * double.Parse(bulkmwofphase);
                                dr2["FlowRate"] = string.Format("{0:0.0000}", wf * 3600);
                            }

                            //enthalpy=TotalMolarEnthalpy*TotalMolarRate+InertWeightEnthalpy*InertWeightRate;
                            double TotalMolarEnthalpy = 0;
                            if (dr2["TotalMolarEnthalpy"].ToString() != "")
                            {
                                TotalMolarEnthalpy = double.Parse(dr2["TotalMolarEnthalpy"].ToString());
                            }


                            double InertWeightEnthalpy = 0;
                            string strInertWeightEnthalpy = dr2["InertWeightEnthalpy"].ToString();
                            if (strInertWeightEnthalpy != "")
                            {
                                InertWeightEnthalpy = double.Parse(strInertWeightEnthalpy);
                            }
                            double InertWeightRate = 0;
                            string strInertWeightRate = dr2["InertWeightRate"].ToString();
                            if (strInertWeightRate != "")
                            {
                                InertWeightRate = double.Parse(strInertWeightRate);
                            }
                            double Enthalpy = TotalMolarEnthalpy * TotalMolarRate + InertWeightEnthalpy * InertWeightRate;
                            dr2["Enthalpy"] = string.Format("{0:0.0000}", Enthalpy * 3600 / 1000000);

                            //TotalMassRate=IF(TotalMolarRate>0,TotalMolarRate*BulkMwOfPhase,RMISS)
                            double TotalMassRate = 0;
                            if (TotalMolarRate > 0 && bulkmwofphase != "")
                            {
                                TotalMassRate = TotalMolarRate * double.Parse(bulkmwofphase);
                            }

                            //SpEnthalpy=IF(TotalMolarRate+InertWeightRate>0,Enthalpy/(TotalMassRate+InertWeightRate),RMISS)
                            double SpEnthalpy = 0;
                            if (TotalMolarRate + InertWeightRate > 0)
                            {
                                SpEnthalpy = Enthalpy / (TotalMassRate + InertWeightRate);
                            }
                            dr2["specificenthalpy"] = string.Format("{0:0.0000}", SpEnthalpy);

                            
                            

                        }
                    }


                }
                
                return ds2;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet getHeaterListByEq(string eqName)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds2 = new DataSet();
                
                DataTable dtHeatIn = getStructure("frmcase_reboiler").Clone(); 
                DataTable dtHeatOut = getStructure("frmcase_condenser").Clone();  
                ds2.Tables.Add(dtHeatIn);
                ds2.Tables.Add(dtHeatOut);
                OleDbConnection conn = new OleDbConnection(connectString);
                string sql = "select HeaterNames,HeaterDuties  from eqlist  where eqname='" + eqName + "'";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    string heaterNames = dr["HeaterNames"].ToString();
                    string heaterDuties = dr["HeaterDuties"].ToString();
                    string[] arrHeaterNames = heaterNames.Split(',');
                    string[] arrHeaterDuties = heaterDuties.Split(',');

                    for (int i = 0; i < arrHeaterNames.Length; i++)
                    {

                        if (double.Parse(arrHeaterDuties[i]) >= 0)
                        {
                            DataRow r = dtHeatIn.NewRow();
                            r["heatername"] = arrHeaterNames[i];
                            r["heaterduty"] = arrHeaterDuties[i];
                            r["dutylost"] = false;
                            r["dutycalcfactor"] = 1;
                            dtHeatIn.Rows.Add(r);

                        }
                        else
                        {
                            DataRow r = dtHeatOut.NewRow();
                            r["heatername"] = arrHeaterNames[i];
                            r["heaterduty"] = arrHeaterDuties[i];
                            r["dutylost"] = false;
                            r["dutycalcfactor"] = 1;
                            dtHeatOut.Rows.Add(r);

                        }
                    }

                }
                return ds2;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        

        #endregion

       

        public void addRecordByDataTable(DataTable dt)
        {
            try
            {
                conn.Open();
                foreach (DataRow dr in dt.Rows)
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sb2 = new StringBuilder();
                    StringBuilder sb3 = new StringBuilder();

                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToUpper() != "ID")
                        {
                            sb.Append(",").Append(dc.ColumnName);
                            sb2.Append(",'").Append(dr[dc.ColumnName].ToString()).Append("'");
                            sb3.Append(",").Append(dc.ColumnName).Append("='").Append(dr[dc.ColumnName].ToString()).Append("'");
                        }
                    }
                    string fileds = sb.Remove(0, 1).ToString();
                    string values = sb2.Remove(0, 1).ToString();
                    string sql = "insert into " +dt.TableName+"("+ fileds + ")values(" + values + ")";
                    if (dt.TableName == "frmtower")
                    {
                        string visiofile = dr["visiofile"].ToString();
                        string towername = dr["towername"].ToString();
                        string strWhere = "towername='" + towername + "' and visiofile='" + visiofile + "'";
                        if (isExist(dt.TableName, strWhere))
                        {
                            sql = "update frmtower set " + sb3.Remove(0, 1).ToString() + " where " + strWhere;
                        }
                    }
                    OleDbCommand cmd = new OleDbCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                
            }
        }

        public void saveDataByTable(DataTable dt,string vsdFile)
        {
            conn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandText = "delete from " + dt.TableName + " where visiofile='" + vsdFile + "'";
                cmd.ExecuteNonQuery();
                foreach (DataRow dr in dt.Rows)
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sb2 = new StringBuilder();
                    
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToUpper() == "ID")
                        {
                            
                        }
                        else
                        {
                            sb.Append(",").Append(dc.ColumnName);
                            if (dc.DataType == typeof(bool))
                            {
                                sb2.Append(",").Append(dr[dc.ColumnName].ToString()).Append("");
                            }                            
                            else
                            {
                                string value = dr[dc.ColumnName].ToString();
                                if (dc.ColumnName.ToLower().Contains("_color"))
                                {
                                    if (value == "")
                                        value = "green";
                                    sb2.Append(",'").Append(value).Append("'");
                                }
                                else
                                {
                                    sb2.Append(",'").Append(value).Append("'");
                                }
                            }
                           
                        }
                    }
                    string fileds = sb.Remove(0, 1).ToString();
                    string values = sb2.Remove(0, 1).ToString();
                    string sql = "insert into " + dt.TableName + "(" + fileds + ")values(" + values + ")";

                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public DataTable getDataByVsdFile(string tableName, string vsdFile)
        {
            try
            {
                DataSet ds = new DataSet();
                string sql = "select * from " + tableName + "  where visiofile='"+vsdFile+"'";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                ds.Tables[0].TableName = tableName;
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getDataByVsdFile(string tableName, string vsdFile,string strWhere)
        {
            try
            {
                DataSet ds = new DataSet();
                string sql = "select * from " + tableName + "  where visiofile='" + vsdFile + "'";
                if (strWhere != "")
                {
                    sql = "select * from " + tableName + "  where visiofile='" + vsdFile + "'  and " + strWhere;
                }
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                ds.Tables[0].TableName = tableName;
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void saveDataByRow(DataRow dr, int op)
        {
            conn.Open();
            try
            {
                DataTable dt = dr.Table;
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;

                StringBuilder sb = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                StringBuilder sb3 = new StringBuilder();
                StringBuilder sb4 = new StringBuilder();
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.ToUpper() == "ID")
                    {
                        sb4.Append("ID=").Append(dr["ID"].ToString());

                    }
                    else
                    {
                        sb.Append(",[").Append(dc.ColumnName).Append("]");
                        if (dc.DataType == typeof(bool))
                        {
                            sb2.Append(",").Append(dr[dc.ColumnName].ToString()).Append("");
                            sb3.Append(",[").Append(dc.ColumnName).Append("]=").Append(dr[dc.ColumnName].ToString()).Append("");
                        }
                        else if (dc.DataType == typeof(int))
                        {
                            if (string.IsNullOrEmpty(dr[dc.ColumnName].ToString()))
                            {
                                sb2.Append(",null");
                                sb3.Append(",[").Append(dc.ColumnName).Append("]=0");
                            }
                            else
                            {
                                sb2.Append(",").Append(dr[dc.ColumnName].ToString()).Append("");
                                sb3.Append(",[").Append(dc.ColumnName).Append("]=").Append(dr[dc.ColumnName].ToString()).Append("");
                            }
                        }
                        else
                        {
                            string value = dr[dc.ColumnName].ToString();                           
                            if (dc.ColumnName.ToLower().Contains("color"))
                            {
                                if (value == "")
                                    value = "green";
                               sb2.Append(",'").Append(value).Append("'");
                               sb3.Append(",[").Append(dc.ColumnName).Append("]='").Append(value).Append("'");
                            }
                            else
                            {
                                sb2.Append(",'").Append(value).Append("'");
                                sb3.Append(",[").Append(dc.ColumnName).Append("]='").Append(dr[dc.ColumnName].ToString()).Append("'");
                            }
                            
                        }
                        
                    }
                }
                string fileds = sb.Remove(0, 1).ToString();
                string values = sb2.Remove(0, 1).ToString();
                string sql = "insert into " + dt.TableName + "(" + fileds + ")values(" + values + ")";

                if (op == 1)
                    sql = "update " + dt.TableName + " set " + sb3.Remove(0, 1).ToString() + " where " + sb4.ToString();
                if (op == 2)
                    sql = "delete " + dt.TableName + "  where " + sb4.ToString();

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();


                conn.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }


        public DataTable  getStructure(string tableName)
        {
            try
            {
                DataSet ds = new DataSet();
                string sql = "select * from "+tableName+"  where 1=0";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                ds.Tables[0].TableName = tableName;
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public void importDataByDataTable(DataSet ds)
        {
            try
            {
                conn.Open();
                foreach (DataTable dt in ds.Tables)
                {
                
                    foreach (DataRow dr in dt.Rows)
                    {
                        StringBuilder sb = new StringBuilder();
                        StringBuilder sb2 = new StringBuilder();
                        StringBuilder sb3 = new StringBuilder();

                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dc.ColumnName.ToUpper() != "ID")
                            {
                                sb.Append(",[").Append(dc.ColumnName).Append("]");
                                sb2.Append(",'").Append(dr[dc.ColumnName].ToString()).Append("'");
                                sb3.Append(",").Append(dc.ColumnName).Append("='").Append(dr[dc.ColumnName].ToString()).Append("'");
                            }
                        }
                        string fileds = sb.Remove(0, 1).ToString();
                        string values = sb2.Remove(0, 1).ToString();
                        string sql = "insert into " + dt.TableName + "(" + fileds + ")values(" + values + ")";


                        OleDbCommand cmd = new OleDbCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
            finally
            {
                conn.Close();
            }
        }

        public bool isExist(string tableName,string strWhere)
        {
            try
            {
                DataSet ds = new DataSet();               
                string sql = "select 1 from " + tableName + "  where "+strWhere;                
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string[] computeH(DataTable dtstream,string streamName)
        {
           string[] h=new string[7];

            double H = 0;
            foreach(DataRow dr in dtstream.Rows)
            {
                string name=dr["streamname"].ToString();
                if (name == streamName)
                {
                    double TotalMolarRate = 0;
                    double TotalMolarEnthalpy = 0;
                    double InertWeightEnthalpy = 0;
                    double InertWeightRate = 0;
                    object oo = dr["InertWeightEnthalpy"];

                    string strInertWeightEnthalpy = dr["InertWeightEnthalpy"].ToString();

                    if (strInertWeightEnthalpy != "")
                        InertWeightEnthalpy = double.Parse(strInertWeightEnthalpy);
                    string strInertWeightRate = dr["InertWeightRate"].ToString();
                    if (strInertWeightRate != "")
                        InertWeightRate = double.Parse(strInertWeightRate);
                    string strTotalMolarEnthalpy = dr["TotalMolarEnthalpy"].ToString();
                    if (strTotalMolarEnthalpy != "")
                        TotalMolarEnthalpy = double.Parse(strTotalMolarEnthalpy);
                    //enthalpy=TotalMolarEnthalpy*TotalMolarRate+InertWeightEnthalpy*InertWeightRate;
                    string strTotalMolarRate = dr["TotalMolarRate"].ToString();
                    if (strTotalMolarRate != "")
                        TotalMolarRate = double.Parse(strTotalMolarRate);
                    double Enthalpy = TotalMolarEnthalpy * TotalMolarRate + InertWeightEnthalpy * InertWeightRate;

                    H = Enthalpy * 3600/1000000;

                    double TotalMassRate = 0;
                    double BulkMwOfPhase = 0;

                    string strBulkMwOfPhase = dr["BulkMwOfPhase"].ToString();
                    if (TotalMolarRate > 0 && strBulkMwOfPhase != "")
                    {
                        BulkMwOfPhase = double.Parse(strBulkMwOfPhase);
                        TotalMassRate = TotalMolarRate * BulkMwOfPhase;
                    }

                    //SpEnthalpy=IF(TotalMolarRate+InertWeightRate>0,Enthalpy/(TotalMassRate+InertWeightRate),RMISS)
                    double SpEnthalpy = 0;
                    if (TotalMolarRate + InertWeightRate > 0)
                    {
                        SpEnthalpy = Enthalpy / (TotalMassRate + InertWeightRate);
                    }

                    double flowrate = 0;
                    string bulkmwofphase = dr["BulkMwOfPhase"].ToString();
                    if (bulkmwofphase != "")
                    {
                        double wf = TotalMolarRate * double.Parse(bulkmwofphase);
                        flowrate = wf * 3600;
                    }

                    

                    h[0]= string.Format("{0:0.0000}", H); 
                    h[1] = string.Format("{0:0.0000}", SpEnthalpy);
                    h[2] = string.Format("{0:0.0000}", flowrate);

                    string temperature = dr["Temperature"].ToString();
                    if (temperature != "")
                    {
                        h[3] = UnitConverter.unitConv(temperature, "K", "C", "{0:0.0000}");
                    }

                    string pressure = dr["Pressure"].ToString();
                    if (pressure != "")
                    {
                        h[4] = UnitConverter.unitConv(pressure, "KPA", "MPAG", "{0:0.0000}");
                    }
                    h[5] = string.Format("{0:0.0000}", strTotalMolarRate);
                    h[6]= string.Format("{0:0.0000}", strBulkMwOfPhase);
                }
            }
            return h;
        }

        private string getSideColumnTray(string[] sideColumnFeeds, Dictionary<string, string> dicProducts)
        {
            string result = string.Empty;
            foreach (string feed in sideColumnFeeds)
            {
                if (dicProducts.Keys.Contains(feed))
                {
                    result = dicProducts[feed];
                    break;
                }
            }
            return result;
        }

       

        public void getMainColumnFeedProduct(string ColumnName, ref Dictionary<string, string> dicFeeds, ref Dictionary<string, string> dicProducts)
        {
            DataSet ds = new DataSet();

            OleDbConnection conn = new OleDbConnection(connectString);
            string sql = "select eqname, feeddata,productdata,feedtrays,prodtrays  from eqlist  where eqname='" + ColumnName + "'";

            OleDbCommand cmd = new OleDbCommand(sql, conn);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string feeddata = dr["feeddata"].ToString();
                    string productdata = dr["productdata"].ToString();
                    string feedtrays = dr["feedtrays"].ToString();
                    string prodtrays = dr["prodtrays"].ToString();
                    string[] arrFeeds = feeddata.Split(',');
                    string[] arrProducts = productdata.Split(',');
                    string[] arrFeedtrays = feedtrays.Split(',');
                    string[] arrProdtrays = prodtrays.Split(',');
                    for (int i = 0; i < arrFeeds.Length; i++)
                    {
                        dicFeeds.Add(arrFeeds[i], arrFeedtrays[i]);
                    }
                    for (int i = 0; i < arrProducts.Length; i++)
                    {
                        dicProducts.Add(arrProducts[i], arrProdtrays[i]);
                    }
                }
            }
        }
        public bool getAllSideColumnFeedProductData(ref Dictionary<string, string[]> dictFeed, ref Dictionary<string, string[]> dictProdcut)
        {
            bool b = false;
            DataSet ds = new DataSet();            
            OleDbConnection conn = new OleDbConnection(connectString);
            string sql = "select eqname, feeddata,productdata  from eqlist  where eqtype='SideColumn'";

            OleDbCommand cmd = new OleDbCommand(sql, conn);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    string key = dr["eqname"].ToString();
                    string feeddata = dr["feeddata"].ToString();
                    string productdata = dr["productdata"].ToString();
                    string[] feeds = feeddata.Split(',');
                    string[] products = productdata.Split(',');
                    dictFeed.Add(key, feeds);
                    dictProdcut.Add(key, products);
                }
                b = true;
            }
            return b;
        }

        public void getMaincolumnRealFeedProduct(string ColumnName, ref Dictionary<string, string> dicFeeds, ref Dictionary<string, string> dicProducts)
        {
            Dictionary<string, string> tempFeeds = new Dictionary<string, string>();
            Dictionary<string, string> tempProducts = new Dictionary<string, string>();
            Dictionary<string,string[]> sideColumnFeeds=new Dictionary<string,string[]>();
            Dictionary<string,string[]> sideColumnProducts=new Dictionary<string,string[]>();
            getMainColumnFeedProduct(ColumnName,ref tempFeeds, ref tempProducts);
            getAllSideColumnFeedProductData(ref sideColumnFeeds, ref sideColumnProducts);
            foreach (KeyValuePair<string, string> feed in tempFeeds)
            {
                bool isInternal =false;
                foreach (KeyValuePair<string, string[]> p in sideColumnProducts )
                {
                    if (p.Value.Contains(feed.Key))
                    {
                        isInternal=true;
                        break;
                    }
                }
                if (!isInternal)
                {
                    dicFeeds.Add(feed.Key, feed.Value);
                }
            }
            foreach (KeyValuePair<string, string> product in tempProducts)
            {
                bool isInternal = false;
                foreach (KeyValuePair<string, string[]> p in sideColumnFeeds)
                {
                    if (p.Value.Contains(product.Key))
                    {
                        isInternal = true;
                        break;
                    }
                }
                if (!isInternal)
                {
                    dicProducts.Add(product.Key,product.Value);
                }
            }

            Dictionary<string, string> sideColumnTray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string[]> p in sideColumnFeeds)
            {
                string tray = getSideColumnTray(p.Value, tempProducts);
                sideColumnTray.Add(p.Key, tray);
                foreach (string feed in p.Value)
                {
                    if (feed != string.Empty)
                    {
                        bool isInternal = false;
                        if (tempProducts.Keys.Contains(feed))
                        {
                            isInternal = true;
                        }
                        if (!isInternal)
                        {
                            if (dicFeeds.Keys.Contains(feed) == false)
                            {
                                dicFeeds.Add(feed, tray);
                            }
                        }
                    }
                }

            }

            foreach (KeyValuePair<string, string[]> p in sideColumnProducts)
            {
                string tray =sideColumnTray[p.Key];
                foreach (string product in p.Value)
                {
                    if (product != string.Empty)
                    {
                        bool isInternal = false;
                        if (tempFeeds.Keys.Contains(product))
                        {
                            isInternal = true;
                        }
                        if (!isInternal)
                        {
                            if (dicProducts.Keys.Contains(product) == false)
                            {
                                dicProducts.Add(product, tray);
                            }
                        }
                    }
                }
                
            }
            


               
        }


        public void  getAndConvertStreamInfo(string streamName,ref DataRow dr)
        {           
            DataSet ds = getStreamByName(streamName);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow drStream = ds.Tables[0].Rows[0];
                dr["streamname"] = drStream["streamname"];
                //显示温度
                string temperature = drStream["Temperature"].ToString();
                if (temperature != "")
                {
                    dr["Temperature"] = UnitConverter.unitConv(temperature, "K", "C", "{0:0.0000}");
                }

                //显示并转换压力
                string pressure = drStream["Pressure"].ToString();
                if (pressure != "")
                {
                    dr["Pressure"] = UnitConverter.unitConv(pressure, "KPA", "MPAG", "{0:0.0000}");
                }

                //
                string vabfrac = drStream["VaporFraction"].ToString();
                if (vabfrac != "")
                {
                   dr["VaporFraction"] =  string.Format("{0:0.0000}", double.Parse(vabfrac));
                }

                double TotalMolarRate = 0;
                if (drStream["TotalMolarRate"].ToString() != "")
                    TotalMolarRate = double.Parse(drStream["TotalMolarRate"].ToString());
                string bulkmwofphase = drStream["BulkMwOfPhase"].ToString();
                if (bulkmwofphase != "")
                {
                    double wf = TotalMolarRate * double.Parse(bulkmwofphase);
                    dr["WeightFlow"] = string.Format("{0:0.0000}", wf * 3600);
                }

                //enthalpy=TotalMolarEnthalpy*TotalMolarRate+InertWeightEnthalpy*InertWeightRate;
                double TotalMolarEnthalpy = 0;
                string strTotalMolarEnthalpy = drStream["TotalMolarEnthalpy"].ToString();
                if (strTotalMolarEnthalpy != "")
                {
                    TotalMolarEnthalpy = double.Parse(strTotalMolarEnthalpy);
                }


                double InertWeightEnthalpy = 0;
                string strInertWeightEnthalpy = drStream["InertWeightEnthalpy"].ToString();
                if (strInertWeightEnthalpy != "")
                {
                    InertWeightEnthalpy = double.Parse(strInertWeightEnthalpy);
                }

                double InertWeightRate = 0;
                string strInertWeightRate = drStream["InertWeightRate"].ToString();
                if (strInertWeightRate != "")
                {
                    InertWeightRate = double.Parse(strInertWeightRate);
                }


                double Enthalpy = TotalMolarEnthalpy * TotalMolarRate + InertWeightEnthalpy * InertWeightRate;
                dr["Enthalpy"] = string.Format("{0:0.0000}", Enthalpy * 3600 / 1000000);

                //TotalMassRate=IF(TotalMolarRate>0,TotalMolarRate*BulkMwOfPhase,RMISS)
                double TotalMassRate = 0;
                if (TotalMolarRate > 0 && bulkmwofphase != "")
                {
                    TotalMassRate = TotalMolarRate * double.Parse(bulkmwofphase);
                }

                //SpEnthalpy=IF(TotalMolarRate+InertWeightRate>0,Enthalpy/(TotalMassRate+InertWeightRate),RMISS)
                double SpEnthalpy = 0;
                if (TotalMolarRate + InertWeightRate > 0)
                {
                    SpEnthalpy = Enthalpy / (TotalMassRate + InertWeightRate);
                }
                dr["SpEnthalpy"] = string.Format("{0:0.0000}", SpEnthalpy);


                string[] columns = { "BulkMwOfPhase", "BulkDensityAct", "BulkViscosity", "BulkCPCVRatio", "VaporZFmKVal", "BulkCP", "BulkThermalCond", "BulkSurfTension", "TotalComposition", "TotalMolarEnthalpy", "TotalMolarRate", "InertWeightEnthalpy", "InertWeightRate",  "CompIn", "ComponentId", "ProdType"};
                foreach (string c in columns)
                {
                    dr[c]=drStream[c].ToString();
                }
            }
            
        }


        public void saveDataByTable(DataTable dt,int op)
        {
            conn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                if (op == -1)
                {
                    cmd.CommandText = "delete from " + dt.TableName;
                    cmd.ExecuteNonQuery();
                }
                foreach (DataRow dr in dt.Rows)
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sb2 = new StringBuilder();
                    StringBuilder sb3 = new StringBuilder();
                    StringBuilder sb4 = new StringBuilder();

                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToUpper() == "ID")
                        {
                            sb4.Append("ID=").Append(dr["ID"].ToString());

                        }
                        else
                        {
                            sb.Append(",[").Append(dc.ColumnName).Append("]");
                            if (dc.DataType == typeof(bool))
                            {
                                sb2.Append(",").Append(dr[dc.ColumnName].ToString()).Append("");
                                sb3.Append(",[").Append(dc.ColumnName).Append("]=").Append(dr[dc.ColumnName].ToString()).Append("");
                            }
                            else if (dc.DataType == typeof(int))
                            {
                                if (string.IsNullOrEmpty(dr[dc.ColumnName].ToString()))
                                {
                                    sb2.Append(",null");
                                    sb3.Append(",[").Append(dc.ColumnName).Append("]=0");
                                }
                                else
                                {
                                    sb2.Append(",").Append(dr[dc.ColumnName].ToString()).Append("");
                                    sb3.Append(",[").Append(dc.ColumnName).Append("]=").Append(dr[dc.ColumnName].ToString()).Append("");
                                }
                            }
                            else
                            {
                                string value = dr[dc.ColumnName].ToString();
                                if (dc.ColumnName.ToLower().Contains("color"))
                                {
                                    if (value == "")
                                        value = "green";
                                    sb2.Append(",'").Append(value).Append("'");
                                    sb3.Append(",[").Append(dc.ColumnName).Append("]='").Append(value).Append("'");
                                }
                                else
                                {
                                    sb2.Append(",'").Append(value).Append("'");
                                    sb3.Append(",[").Append(dc.ColumnName).Append("]='").Append(dr[dc.ColumnName].ToString()).Append("'");
                                }

                            }

                        }
                    }
                    string fileds = sb.Remove(0, 1).ToString();
                    string values = sb2.Remove(0, 1).ToString();
                    string sql = "insert into " + dt.TableName + "(" + fileds + ")values(" + values + ")";

                    if (op == 1)
                        sql = "update " + dt.TableName + " set " + sb3.Remove(0, 1).ToString() + " where " + sb4.ToString();
                    if (op == 2)
                        sql = "delete " + dt.TableName + "  where " + sb4.ToString();

                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public DataTable getDataByTable(string tableName, string strWhere)
        {
            try
            {
                DataSet ds = new DataSet();
                string sql = "select * from " + tableName ;
                if (strWhere != "")
                {
                    sql = "select * from " + tableName +" where " + strWhere;
                }
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                ds.Tables[0].TableName = tableName;
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getDataBySQL(string sql)
        {
            try
            {
                DataSet ds = new DataSet();
                
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public void saveDataBySQL(string sql)
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

    }
    
}
