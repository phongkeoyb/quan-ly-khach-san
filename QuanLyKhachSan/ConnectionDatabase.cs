using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKhachSan
{
    class ConnectionDatabase
    {
        private static SqlConnection conn;
        public ConnectionDatabase()
        {

        }
        public static SqlConnection getInstance()
        {
            if (conn == null)
            {
                conn = new SqlConnection("Data Source=DESKTOP-45P13V1\\SQLEXPRESS01;Initial Catalog=QuanLyKhachSan;Integrated Security=True");
            }
            return conn;
        }
        public static void openConnectionStage()
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
        }
        public static void closeConnectionStage()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }
        public static void FillCombo(string sql, LookUpEdit cbo, string ma, string ten)
        {
            SqlDataAdapter Mydata = new SqlDataAdapter(sql, conn);
            DataTable table = new DataTable();
            Mydata.Fill(table);
            cbo.Properties.DataSource = table;
            cbo.Properties.ValueMember = ma; //Trường giá trị
            cbo.Properties.DisplayMember = ma; //Trường hiển thị
        }

        //lay du lieu tu mot cau lenh sql
        public static string GetFieldValues(string sql)
        {
            string ma = "";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
                ma = reader.GetValue(0).ToString();
            reader.Close();
            return ma;
        }
    }
}
