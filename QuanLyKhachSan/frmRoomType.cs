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
    public partial class frmRoomType : DevExpress.XtraEditors.XtraForm
    {
        private SqlConnection conn;
        public frmRoomType()
        {
            InitializeComponent();
        }

        private void frmRoomType_Load(object sender, EventArgs e)
        {
            conn = ConnectionDatabase.getInstance();
            ConnectionDatabase.openConnectionStage();
            loadData();
        }

        private void loadData()
        {
            string sql = "SELECT MALOAI AS 'Mã Loại', TenLoai AS 'Tên loại phòng', GiaPhong AS 'Giá tiền'," +
                " SOSAO as 'Số sao' FROM LOAIPHONG";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            gridCtrlRoomType.DataSource = dataSet.Tables[0];
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string ma = edtIDType.Text;
            string ten = edtTypeName.Text;
            string sao = edtStar.Text;
            string gia = edtPrice.Text;

            if (ma == "" || ten == "" || sao == "" || gia == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isNumber(gia) || !isNumber(sao))
            {
                XtraMessageBox.Show("Giá dịch vụ phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlTrungKhoa = "SELECT * from LOAIPHONG WHERE MALOAI = '" + ma + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (reader.Read())//Nếu đọc được 
                {
                    XtraMessageBox.Show("Đã tồn tại mã loại phòng này", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    reader.Close();//Chữ N trước '' thêm được tiếng việt vào sql
                    string sqlInsert = "INSERT INTO LOAIPHONG VALUES(N'" + ma + "',N'" + ten + "',N'" + gia + "',N'" + sao + "')";
                    SqlCommand commandUpdate = new SqlCommand(sqlInsert, conn);
                    commandUpdate.ExecuteNonQuery();
                    XtraMessageBox.Show("Thêm thành công mã loại phòng: " + ma, "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }
        }

        private void btnReLoad_Click(object sender, EventArgs e)
        {
            edtIDType.ResetText();
            edtTypeName.ResetText();
            edtPrice.ResetText();
            edtStar.ResetText();
            loadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string ma = edtIDType.Text;
            string ten = edtTypeName.Text;
            string sao = edtStar.Text;
            string gia = edtPrice.Text;

            if (ma == "" || ten == "" || sao == "" || gia == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isNumber(gia) || !isNumber(sao))
            {
                XtraMessageBox.Show("Giá dịch vụ phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlTrungKhoa = "SELECT * from LOAIPHONG WHERE MALOAI = '" + ma + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (!reader.Read())
                {
                    XtraMessageBox.Show("Không tồn tại mã khách hàng này để cập nhật", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    reader.Close();
                    string sqlUpdate = "UPDATE LOAIPHONG Set MALOAI=N'" + ma + "',TENLOAI=N'" + ten + "',GIAPHONG=N'" + gia + "',SOSAO=N'" + sao + "' WHERE MALOAI=N'" + ma + "'";
                    SqlCommand commandUpdate = new SqlCommand(sqlUpdate, conn);
                    commandUpdate.ExecuteNonQuery();
                    XtraMessageBox.Show("Cập nhật thành công mã loại phòng: " + ma, "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            string maLoai = gvLoaiPhong.GetRowCellValue(gvLoaiPhong.FocusedRowHandle, "Mã Loại").ToString();
            if (maLoai != null)
            {
                if (XtraMessageBox.Show("Bạn có chắc muốn xóa loại phòng này không", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string sqlDelete = "DELETE FROM LOAIPHONG where MALOAI = N'" + maLoai + "'";
                    SqlCommand commandDelete = new SqlCommand(sqlDelete, conn);
                    commandDelete.ExecuteNonQuery();
                    XtraMessageBox.Show("Loại phòng có mã: " + maLoai + " đã được xóa", "Thông báo", MessageBoxButtons.OK);
                    //gvKhachHang.DeleteRow(gvKhachHang.FocusedRowHandle);
                    loadData();
                }
            }
        }
        private void gvLoaiPhong_FocusedRowChanged_1(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
            {

            }
            else
            {
                edtIDType.Text = gvLoaiPhong.GetRowCellValue(e.FocusedRowHandle, "Mã Loại").ToString();
                edtTypeName.Text = gvLoaiPhong.GetRowCellValue(e.FocusedRowHandle, "Tên loại phòng").ToString();
                edtPrice.Text = gvLoaiPhong.GetRowCellValue(e.FocusedRowHandle, "Giá tiền").ToString();
                edtStar.Text = gvLoaiPhong.GetRowCellValue(e.FocusedRowHandle, "Số sao").ToString();

            }
        }

        private void gvLoaiPhong_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}