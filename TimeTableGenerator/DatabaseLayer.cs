﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Runtime.Remoting.Messaging;
using System.Drawing;
using System.Runtime.Remoting.Contexts;

namespace TimeTableGenerator
{
    public class DatabaseLayer
    {
        public static SqlConnection conn;

        public static SqlConnection ConOpen()
        {
            if(conn == null)
            {
                conn = new SqlConnection(@"Data Source=MAX-1203\SQLEXPRESS;Initial Catalog=AutoTimeTableDb;Integrated Security=true;TrustServerCertificate=true;");
            }
            if (conn.State != ConnectionState.Open) 
            {
                conn.Open();
            }
            return conn;
        }

        public static bool Insert(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, ConOpen());
                int roweffected = cmd.ExecuteNonQuery();
                return roweffected > 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool Update(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, ConOpen());
                int roweffected = cmd.ExecuteNonQuery();
                return roweffected > 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool Delete(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, ConOpen());
                int roweffected = cmd.ExecuteNonQuery();
                return roweffected > 0;
            }
            catch
            {
                return false;
            }
        }

        public static DataTable Retrieve(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(query, ConOpen());
                da.Fill(dt);
                if(dt != null)
                {
                    if(dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }  
}
