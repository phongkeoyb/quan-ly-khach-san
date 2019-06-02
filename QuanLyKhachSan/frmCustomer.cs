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
    public partial class frmCustomer : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection conn;
        public frmCustomer()
        {
            InitializeComponent();
        }

        private void frmCustomer_Load(object sender, EventArgs e)
        {
            conn = ConnectionDatabase.getInstance();
            ConnectionDatabase.openConnectionStage();
            loadData();

        }
        private void loadData()
        {//Load dữ liệu có thể sài nhiều lần sau khi xóa sửa thêm, đều có thể gọi lại để kiểm tra
            string sql = "SELECT MAKH AS 'Mã KH', HOTEN AS 'Họ tên', GIOITINH AS 'Giới tính'," +
                " DIACHI as 'Địa chỉ', CMND, DIENTHOAI as 'SĐT', Email FROM KHACHHANG";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            gridCustomer.DataSource = dataSet.Tables[0];
        }

        private void gvKhachHang_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {//Sự kiện này lắng nghe dòng đang focuss để lấy dữ liệu lên mấy ô textbox
            if (e.FocusedRowHandle < 0)
            {

            }
            else
            {
                edtMaKH.Text = gvKhachHang.GetRowCellValue(e.FocusedRowHandle, "Mã KH").ToString();
                edtTenKH.Text = gvKhachHang.GetRowCellValue(e.FocusedRowHandle, "Họ tên").ToString();
                edtCMND.Text = gvKhachHang.GetRowCellValue(e.FocusedRowHandle, "CMND").ToString();
                edtSDT.Text = gvKhachHang.GetRowCellValue(e.FocusedRowHandle, "SĐT").ToString();
                edtEmail.Text = gvKhachHang.GetRowCellValue(e.FocusedRowHandle, "Email").ToString();
                edtDiaChi.Text = gvKhachHang.GetRowCellValue(e.FocusedRowHandle, "Địa chỉ").ToString();

            }
        }

        private void gvKhachHang_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)//Không cho chỉnh sửa tại dòng đang chọn
        {
            e.Cancel = true;
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

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maKH = edtMaKH.Text;
            string hoTen = edtTenKH.Text;
            string diaChi = edtDiaChi.Text;
            string cmnd = edtCMND.Text;
            string dienThoai = edtSDT.Text;
            string email = edtEmail.Text;
            string gioitinh = radGioiTinh.Properties.Items[radGioiTinh.SelectedIndex].Description;
            if (maKH == "" || hoTen == "" || diaChi == "" || cmnd == "" || dienThoai == "" || email == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isNumber(dienThoai) || (!isNumber(cmnd)))
            {
                XtraMessageBox.Show("Số điện thoại hoặc CMND phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlTrungKhoa = "SELECT * from khachhang WHERE MaKH = '" + maKH + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (reader.Read())//Nếu đọc được 
                {
                    XtraMessageBox.Show("Đã tồn tại mã khách hàng này", "Thông báo", MessageBoxButtons.OK);
                    reader.Close();
                }
                else
                {
                    reader.Close();//Chữ N trước '' thêm được tiếng việt vào sql
                    string sqlInsert = "INSERT INTO khachhang VALUES(N'" + maKH + "',N'" + hoTen + "',N'" + gioitinh + "',N'" + diaChi + "',N'" + cmnd + "',N'" + dienThoai + "',N'" + email + "')";
                    SqlCommand commandUpdate = new SqlCommand(sqlInsert, conn);
                    commandUpdate.ExecuteNonQuery();
                    XtraMessageBox.Show("Thêm thành công mã khách hàng: " + maKH, "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string maKH = edtMaKH.Text;
            string hoTen = edtTenKH.Text;
            string diaChi = edtDiaChi.Text;
            string cmnd = edtCMND.Text;
            string dienThoai = edtSDT.Text;
            string email = edtEmail.Text;
            string gioitinh = radGioiTinh.Properties.Items[radGioiTinh.SelectedIndex].Description;
            if (maKH == "" || hoTen == "" || diaChi == "" || cmnd == "" || dienThoai == "" || email == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isNumber(dienThoai) || (!isNumber(cmnd)))
            {
                XtraMessageBox.Show("Số điện thoại hoặc CMND phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlTrungKhoa = "SELECT * from khachhang WHERE MaKH = '" + maKH + "'";
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
                    string sqlUpdate = "UPDATE khachhang Set HoTen=N'" + hoTen + "',GioiTinh=N'" + gioitinh + "',DiaChi=N'" + diaChi + "',CMND=N'" + cmnd + "',DienThoai=N'" + dienThoai + "',Email=N'" + email + "' WHERE MaKH=N'" + maKH + "'";
                    SqlCommand commandUpdate = new SqlCommand(sqlUpdate, conn);
                    commandUpdate.ExecuteNonQuery();
                    XtraMessageBox.Show("Cập nhật thành công mã khách hàng: " + maKH, "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maKH = gvKhachHang.GetRowCellValue(gvKhachHang.FocusedRowHandle, "Mã KH").ToString();
            if (maKH != null)
            {
                if (XtraMessageBox.Show("Bạn có chắc muốn xóa khách hàng này không", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string sqlDelete = "DELETE FROM khachhang where MaKH = N'" + maKH + "'";
                    SqlCommand commandDelete = new SqlCommand(sqlDelete, conn);
                    commandDelete.ExecuteNonQuery();
                    XtraMessageBox.Show("Khách hàng có mã: " + maKH + " đã được xóa", "Thông báo", MessageBoxButtons.OK);
                    //gvKhachHang.DeleteRow(gvKhachHang.FocusedRowHandle);
                    loadData();
                }
            }


        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            edtMaKH.Text = "";
            edtTenKH.Text = "";
            edtEmail.Text = "";
            edtSDT.Text = "";
            edtCMND.Text = "";
            edtDiaChi.Text = "";
            loadData();
        }
    }
}