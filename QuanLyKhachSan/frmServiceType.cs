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
    public partial class frmServiceType : DevExpress.XtraEditors.XtraForm
    {
        public frmServiceType()
        {
            InitializeComponent();
        }
        SqlConnection conn;

        private void frmServiceType_Load(object sender, EventArgs e)
        {
            conn = ConnectionDatabase.getInstance();
            ConnectionDatabase.openConnectionStage();
            loadData();
        }

        public bool isNumber(string pValue)//Kiểm tra chuỗi có phải là số không
        {
            foreach (Char c in pValue)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }

        private void loadData()
        {
            //Load dữ liệu có thể sài nhiều lần sau khi xóa sửa thêm, đều có thể gọi lại để kiểm tra
            string sql = "SELECT MADV as 'Mã dịch vụ',TENDV as 'Tên dịch vụ',GIA as 'Giá' FROM DICHVU";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            gridCtrlServiceType.DataSource = dataSet.Tables[0];
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string maDV = edtIDService.Text;
            string tenDV = edtServiceName.Text;
            string gia = edtPrice.Text;

            if (maDV == "" || tenDV == "" || gia == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isNumber(gia))
            {
                XtraMessageBox.Show("Giá dịch vụ phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlTrungKhoa = "SELECT * from DICHVU WHERE MADV = '" + maDV + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (reader.Read())//Nếu đọc được 
                {
                    XtraMessageBox.Show("Đã tồn tại mã dịch vụ này", "Thông báo", MessageBoxButtons.OK);
                    reader.Close();
                }
                else
                {
                    reader.Close();//Chữ N trước '' thêm được tiếng việt vào sql
                    string sqlInsert = "INSERT INTO DICHVU VALUES(N'" + maDV + "',N'" + tenDV + "',N'" + gia + "')";
                    SqlCommand commandUpdate = new SqlCommand(sqlInsert, conn);
                    commandUpdate.ExecuteNonQuery();
                    XtraMessageBox.Show("Thêm thành công mã khách hàng: " + maDV, "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            string maDV = gvLoaiDV.GetRowCellValue(gvLoaiDV.FocusedRowHandle, "Mã dịch vụ").ToString();
            if (maDV != null)
            {
                if (XtraMessageBox.Show("Bạn có chắc muốn xóa dịch vụ này không", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string sqlDelete = "DELETE FROM DICHVU where MaDV = N'" + maDV + "'";
                    SqlCommand commandDelete = new SqlCommand(sqlDelete, conn);
                    commandDelete.ExecuteNonQuery();
                    XtraMessageBox.Show("Dịch vụ có mã: " + maDV + " đã được xóa", "Thông báo", MessageBoxButtons.OK);
                    //gvKhachHang.DeleteRow(gvKhachHang.FocusedRowHandle);
                    loadData();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string maDV = edtIDService.Text;
            string tenDV = edtServiceName.Text;
            string gia = edtPrice.Text;

            if (maDV == "" || tenDV == "" || gia == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isNumber(gia))
            {
                XtraMessageBox.Show("Giá dịch vụ phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlTrungKhoa = "SELECT * from khachhang WHERE MaKH = '" + maDV + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (!reader.Read())
                {
                    XtraMessageBox.Show("Không tồn tại mã khách hàng này để cập nhật", "Thông báo", MessageBoxButtons.OK);
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    string sqlUpdate = "UPDATE DICHVU Set MADV=N'" + maDV + "',TENDV=N'" + tenDV + "',GIA=N'" + gia + "' WHERE MADV=N'" + maDV + "'";
                    SqlCommand commandUpdate = new SqlCommand(sqlUpdate, conn);
                    commandUpdate.ExecuteNonQuery();
                    XtraMessageBox.Show("Cập nhật thành công mã dịch vụ: " + maDV, "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }
        }

        private void btnReLoad_Click(object sender, EventArgs e)
        {
            edtIDService.ResetText();
            edtServiceName.ResetText();
            edtPrice.ResetText();
            loadData();
        }
        private void gvLoaiDV_FocusedRowChanged_1(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
            {

            }
            else
            {
                edtIDService.Text = gvLoaiDV.GetRowCellValue(e.FocusedRowHandle, "Mã dịch vụ").ToString();
                edtServiceName.Text = gvLoaiDV.GetRowCellValue(e.FocusedRowHandle, "Tên dịch vụ").ToString();
                edtPrice.Text = gvLoaiDV.GetRowCellValue(e.FocusedRowHandle, "Giá").ToString();
            }
        }
        

        private void gvLoaiDV_ShownEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}