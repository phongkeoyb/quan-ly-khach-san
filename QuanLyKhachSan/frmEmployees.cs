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
    public partial class frmEmployees : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection conn;
        public frmEmployees()
        {
            InitializeComponent();
        }

        private void frmEmployees_Load(object sender, EventArgs e)
        {
            conn = ConnectionDatabase.getInstance();
            ConnectionDatabase.openConnectionStage();
            string sql = "select matk from taikhoan";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                edtMaTK.Properties.Items.Add(reader.GetString(0));
            }
            reader.Close();

            loadData();
        }
        private void loadData()
        {//Load dữ liệu có thể sài nhiều lần sau khi xóa sửa thêm, đều có thể gọi lại để kiểm tra
            string sql = "SELECT MaNV AS 'Mã NV', hoten AS 'Họ Tên',  matk as 'Mã TK' ,CMND AS 'CMND',GioiTinh as 'Giới Tính',DIENTHOAI as 'SĐT',DIACHI as 'Địa chỉ', quocgia as 'Quốc gia', Email as 'Email'  from Nhanvien";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            gridNV.DataSource = dataSet.Tables[0];
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

        private void subGridNV_FocusedRowChanged_1(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
            {

            }
            else
            {
                edtMaNV.Text = subGridNV.GetRowCellValue(e.FocusedRowHandle, "Mã NV").ToString();
                edtHoTen.Text = subGridNV.GetRowCellValue(e.FocusedRowHandle, "Họ Tên").ToString();
                edtMaTK.Text = subGridNV.GetRowCellValue(e.FocusedRowHandle, "Mã TK").ToString();
                edtCMND.Text = subGridNV.GetRowCellValue(e.FocusedRowHandle, "CMND").ToString();
                edtDiaChi.Text = subGridNV.GetRowCellValue(e.FocusedRowHandle, "Địa chỉ").ToString();
                radGioiTinh.Text = subGridNV.GetRowCellValue(e.FocusedRowHandle, "Giới Tính").ToString();
                edtQuocGia.Text = subGridNV.GetRowCellValue(e.FocusedRowHandle, "Quốc gia").ToString();
                edtDienThoai.Text = subGridNV.GetRowCellValue(e.FocusedRowHandle, "SĐT").ToString();
                edtEmail.Text = subGridNV.GetRowCellValue(e.FocusedRowHandle, "Email").ToString();
                if (subGridNV.GetRowCellValue(e.FocusedRowHandle, "Giới Tính").ToString().Equals("Nam"))
                {
                    radGioiTinh.Properties.Items[0].Value = true;
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maNV = edtMaNV.Text;
            string hoTen = edtHoTen.Text;
            string maTK = edtMaTK.Text;
            string cmnd = edtCMND.Text;
            string diaChi = edtDiaChi.Text;
            string quocGia = edtQuocGia.Text;
            string dienThoai = edtDienThoai.Text;
            string email = edtEmail.Text;
            string gioitinh = radGioiTinh.Properties.Items[radGioiTinh.SelectedIndex].Description;
            if (maNV == "" || hoTen == "" || maTK == "" || quocGia == "" || diaChi == "" || cmnd == "" || dienThoai == "" || email == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isNumber(dienThoai) || (!isNumber(cmnd)))
            {

                XtraMessageBox.Show("Số điện thoại hoặc CMND phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlTrungKhoa = "SELECT * from nhanvien WHERE MaNV = '" + maNV + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (reader.Read())//Nếu đọc được 
                {
                    reader.Close();
                    XtraMessageBox.Show("Đã tồn tại mã nhân viên này", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    reader.Close();//Chữ N trước '' thêm được tiếng việt vào sql
                    string sqlInsert = "INSERT INTO nhanvien VALUES(N'" + maNV + "',N'" + hoTen + "',N'" + maTK + "',N'" + cmnd + "',N'" + diaChi + "',N'" + gioitinh + "',N'" + quocGia + "',N'" + dienThoai + "',N'" + email + "')";
                    SqlCommand commandUpdate = new SqlCommand(sqlInsert, conn);
                    commandUpdate.ExecuteNonQuery();
                    XtraMessageBox.Show("Thêm thành công mã nhân viên: " + maNV, "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string maNV = edtMaNV.Text;
            string hoTen = edtHoTen.Text;
            string maTK = edtMaTK.Text;
            string cmnd = edtCMND.Text;
            string diaChi = edtDiaChi.Text;
            string quocGia = edtQuocGia.Text;
            string dienThoai = edtDienThoai.Text;
            string email = edtEmail.Text;
            string gioitinh = radGioiTinh.Properties.Items[radGioiTinh.SelectedIndex].Description;
            if (maNV == "" || hoTen == "" || maTK == "" || quocGia == "" || diaChi == "" || cmnd == "" || dienThoai == "" || email == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isNumber(dienThoai) || (!isNumber(cmnd)))
            {
                XtraMessageBox.Show("Số điện thoại hoặc CMND phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlTrungKhoa = "SELECT * from nhanvien WHERE MaNV = '" + maNV + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (!reader.Read())
                {
                    XtraMessageBox.Show("Không tồn tại mã nhân viên này để cập nhật", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    reader.Close();
                    string sqlUpdate = "UPDATE nhanvien Set HoTen=N'" + hoTen + "',MaTK=N'" + maTK + "',GioiTinh=N'" + gioitinh + "',DiaChi=N'" + diaChi + "',CMND=N'" + cmnd + "',QuocGia=N'" + quocGia + "',DienThoai=N'" + dienThoai + "',Email=N'" + email + "'WHERE MaNV=N'" + maNV + "'";
                    SqlCommand commandUpdate = new SqlCommand(sqlUpdate, conn);
                    commandUpdate.ExecuteNonQuery();
                    XtraMessageBox.Show("Cập nhật thành công mã nhân viên: " + maNV, "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maNV = subGridNV.GetRowCellValue(subGridNV.FocusedRowHandle, "Mã NV").ToString();
            if (maNV != null)
            {
                if (XtraMessageBox.Show("Bạn có chắc muốn xóa nhân viên này không", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string sqlDelete = "DELETE FROM nhanvien where MaNV = N'" + maNV + "'";
                    SqlCommand commandDelete = new SqlCommand(sqlDelete, conn);
                    commandDelete.ExecuteNonQuery();
                    XtraMessageBox.Show("Nhân viên có mã: " + maNV + " đã được xóa", "Thông báo", MessageBoxButtons.OK);
                    //gvKhachHang.DeleteRow(gvKhachHang.FocusedRowHandle);
                    loadData();
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            edtMaNV.Text = "";
            edtHoTen.Text = "";
            edtMaTK.Text = "";
            edtCMND.Text = "";
            edtDiaChi.Text = "";
            edtQuocGia.Text = "";
            edtDienThoai.Text = "";
            edtEmail.Text = "";
            loadData();
        }

        private void subGridNV_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = true; //không cho chinh sửa trong gridview
        }
    }
}