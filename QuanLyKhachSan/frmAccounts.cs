using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;

namespace QuanLyKhachSan
{
    public partial class frmAccounts : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection conn;
        public frmAccounts()
        {
            InitializeComponent();
        }

        private void frmAccounts_Load(object sender, EventArgs e)
        {
            conn = ConnectionDatabase.getInstance();
            ConnectionDatabase.openConnectionStage();
            string sql = "select distinct loaiTK from taikhoan";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                edtLoaiTK.Properties.Items.Add(reader.GetString(0));
            }
            reader.Close();

            loadData();
        }
        private void loadData()
        {//Load dữ liệu có thể sài nhiều lần sau khi xóa sửa thêm, đều có thể gọi lại để kiểm tra
            string sql = "SELECT MaTK AS 'Mã TK', TenTK AS 'Tên TK',  MatKhau as 'Mật khẩu' ,LoaiTK as 'Loại TK' from Taikhoan";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            gridTK.DataSource = dataSet.Tables[0];
        }

        private void subGridTK_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
            {

            }
            else
            {
                edtMaTK.Text = subGridTK.GetRowCellValue(e.FocusedRowHandle, "Mã TK").ToString();
                edtTenTK.Text = subGridTK.GetRowCellValue(e.FocusedRowHandle, "Tên TK").ToString();
                edtMatKhau.Text = subGridTK.GetRowCellValue(e.FocusedRowHandle, "Mật khẩu").ToString();
                edtLoaiTK.Text = subGridTK.GetRowCellValue(e.FocusedRowHandle, "Loại TK").ToString();
            }
        }

        // private void subGridTK_ShowingEditor(object sender, CancelEventArgs e)
        //{
        //   e.Cancel = true; //không cho chinh sửa trong gridview
        // }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maTK = edtMaTK.Text;
            string tenTK = edtTenTK.Text;
            string matKhau = edtMatKhau.Text;
            string loaiTK = edtLoaiTK.Text;

            if (maTK == "" || tenTK == "" || matKhau == "" || loaiTK == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlTrungKhoa = "SELECT * from taikhoan WHERE maTK = '" + maTK + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (reader.Read())//Nếu đọc được 
                {
                    reader.Close();
                    XtraMessageBox.Show("Đã tồn tại mã tài khoản này", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    reader.Close();//Chữ N trước '' thêm được tiếng việt vào sql
                    string sqlInsert = "INSERT INTO taikhoan VALUES(N'" + maTK + "',N'" + tenTK + "',N'" + matKhau + "',N'" + loaiTK + "')";
                    SqlCommand commandUpdate = new SqlCommand(sqlInsert, conn);
                    commandUpdate.ExecuteNonQuery();
                    XtraMessageBox.Show("Thêm thành công mã tài khoản: " + maTK, "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string maTK = edtMaTK.Text;
            string tenTK = edtTenTK.Text;
            string matKhau = edtMatKhau.Text;
            string loaiTK = edtLoaiTK.Text;

            if (maTK == "" || tenTK == "" || matKhau == "" || loaiTK == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlTrungKhoa = "SELECT * from taikhoan WHERE maTK = '" + maTK + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (!reader.Read())//Nếu đọc được 
                {
                    reader.Close();
                    XtraMessageBox.Show("Không tồn tại mã tài khoản này để cập nhật", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    reader.Close();//Chữ N trước '' thêm được tiếng việt vào sql
                    string sqlUpdate = "UPDATE taikhoan Set TenTK=N'" + tenTK + "',MatKhau=N'" + matKhau + "',LoaiTK=N'" + loaiTK + "'WHERE MaTK=N'" + maTK + "'";
                    SqlCommand commandUpdate = new SqlCommand(sqlUpdate, conn);
                    commandUpdate.ExecuteNonQuery();
                    XtraMessageBox.Show("Cập nhật thành công mã tài khoản: " + maTK, "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maTK = subGridTK.GetRowCellValue(subGridTK.FocusedRowHandle, "Mã TK").ToString();
            if (maTK != null)
            {
                if (XtraMessageBox.Show("Bạn có chắc muốn xóa tài khoản này không", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string sqlDelete = "DELETE FROM taikhoan where MaTK = N'" + maTK + "'";
                    SqlCommand commandDelete = new SqlCommand(sqlDelete, conn);
                    commandDelete.ExecuteNonQuery();
                    XtraMessageBox.Show("Tài khoản có mã: " + maTK + " đã được xóa", "Thông báo", MessageBoxButtons.OK);
                    //gvKhachHang.DeleteRow(gvKhachHang.FocusedRowHandle);
                    loadData();
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            edtLoaiTK.Text = "";
            edtMaTK.Text = "";
            edtMatKhau.Text = "";
            edtTenTK.Text = "";

        }

        private void subGridTK_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = true; //không cho chinh sửa trong gridview
        }
    }
}