using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyKhachSan
{
    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection conn;
        public frmLogin()
        {
            InitializeComponent();
            conn = ConnectionDatabase.getInstance();
            ConnectionDatabase.openConnectionStage();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = edtUsername.Text;
            string password = edtPassword.Text;
            checkLogin(user, password);
        }

        private void checkLogin(string user, string password)
        {

            string query = "SELECT * from taikhoan where TenTK='" + user + "' and MatKhau='" + password + "'";
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            DataTable table = new DataTable();
            adapter.Fill(table);
            if (table.Rows.Count == 1)
            {
                frmHome frm = new frmHome(table.Rows[0][3].ToString());
                Hide();
                frm.Show();
            }
            else
            {
                XtraMessageBox.Show("Tài khoản hoặc mật khẩu không đúng", "Thông báo");
            }
        }

        private void edtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                string user = edtUsername.Text;
                string password = edtPassword.Text;
                checkLogin(user, password);
            }
        }
    }
}
