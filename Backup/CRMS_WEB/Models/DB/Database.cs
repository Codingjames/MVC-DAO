using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;

namespace CRMS_WEB.Models.DB
{
    public class Database
    {
        public SqlConnection conn;
        public Database()
        {
            string connection = "Defaultconnect";
            conn = new SqlConnection(@WebConfigurationManager.ConnectionStrings[connection].ToString());
            try
            {
                if (!conn.State.ToString().Equals("Open"))
                {
                    conn.Open();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                SqlConnection.ClearPool(conn);
            }
        }
        public bool Close()
        {
            try
            {
                conn.Close();
                conn.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private int ToInt(object obj)
        {
            try
            {
                return Int32.Parse(obj.ToString());
            }
            catch
            {

                return 0;
            }
        }

        // Query function field
        public int add(string sql)
        {
            int lastID = 0;
            if (conn.State.ToString().Equals("Open"))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                lastID = this.ToInt(cmd.ExecuteScalar());
            }

            return lastID;
        }
        public int update(string sql)
        {
            
            try
            {
                int i = 0;
                if (conn.State.ToString().Equals("Open"))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    i = this.ToInt(cmd.ExecuteNonQuery());

                }
                return i;
            }catch(Exception e){
                throw e;
            }
            
        }
        public int remove(string sql)
        {
            return update(sql);
        }


        // query single 
        public Dictionary<String, Object> querySingle(string sql)
        {
            Dictionary<String, Object> map = new Dictionary<String, Object>();
            try
            {
                if (conn.State.ToString().Equals("Open"))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    var table = reader.GetSchemaTable();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                map.Add(reader.GetName(i), reader.GetValue(i));
                            }
                            return map;
                        }// while
                    } // if
                }// if

            }
            catch (SqlException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

            return map;
        }

        public List<Dictionary<String, Object>> queryList(string sql)
        {
            List<Dictionary<String, Object>> list = new List<Dictionary<string, object>>();
            try
            {
                if (conn.State.ToString().Equals("Open"))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            Dictionary<string, object> map = new Dictionary<string, object>();
                            for (int i = 0; i < rd.FieldCount; i++)
                            {
                                map.Add(rd.GetName(i), rd.GetValue(i));
                            }
                            list.Add(map);
                        }
                        return list;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return list;
        }
        public HashSet<Dictionary<String, Object>> querySet(string sql)
        {
            HashSet<Dictionary<String, Object>> list = new HashSet<Dictionary<string, object>>();
            try
            {
                if (conn.State.ToString().Equals("Open"))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            Dictionary<string, object> map = new Dictionary<string, object>();
                            for (int i = 0; i < rd.FieldCount; i++)
                            {
                                map.Add(rd.GetName(i), rd.GetValue(i));
                            }
                            list.Add(map);
                        }
                        return list;
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }

            return list;
        }

    }
}