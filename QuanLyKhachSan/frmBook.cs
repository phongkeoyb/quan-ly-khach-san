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
    public partial class frmBook : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection conn;
        public frmBook()
        {
            InitializeComponent();
        }

        private void frmBook_Load(object sender, EventArgs e)
        {
            conn = ConnectionDatabase.getInstance();
            ConnectionDatabase.openConnectionStage();
            string sql = "select maphong from phong";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                cbMaPhong.Properties.Items.Add(reader.GetString(0));
            }
            reader.Close();
            loadData();
        }
        private void loadData()
        {//Load dữ liệu có thể sài nhiều lần sau khi xóa sửa thêm, đều có thể gọi lại để kiểm tra
            string sql = "SELECT MaDatPhong AS 'Mã ĐP', Phong.MaPhong AS 'Mã Phòng', KhachHang.MaKH as 'Mã KH' ,HoTen AS 'Tên KH'," +
                "GioiTinh as 'Giới Tính',CMND as 'CMND', DIENTHOAI as 'SĐT',DIACHI as 'Địa chỉ', Email as 'Email' , NgayThue as 'Ngày Thuê', NgayTra as 'Ngày Trả'"
                + ",SLPhong as 'Số Phòng',SLNguoi as 'Số Người' from Phong inner join DatPhong on Phong.MaPhong = DatPhong.MaPhong "
                + "inner join KhachHang on DatPhong.MaKH = KhachHang.MaKH";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            gridDatPhong.DataSource = dataSet.Tables[0];
        }

        private void gvDatPhong_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
            {

            }
            else
            {
                edtMaDP.Text = gvDatPhong.GetRowCellValue(e.FocusedRowHandle, "Mã ĐP").ToString();
                edtMaKH.Text = gvDatPhong.GetRowCellValue(e.FocusedRowHandle, "Mã KH").ToString();
                edtTenKH.Text = gvDatPhong.GetRowCellValue(e.FocusedRowHandle, "Tên KH").ToString();
                edtCMND.Text = gvDatPhong.GetRowCellValue(e.FocusedRowHandle, "CMND").ToString();
                edtSDT.Text = gvDatPhong.GetRowCellValue(e.FocusedRowHandle, "SĐT").ToString();
                edtEmail.Text = gvDatPhong.GetRowCellValue(e.FocusedRowHandle, "Email").ToString();
                edtDiaChi.Text = gvDatPhong.GetRowCellValue(e.FocusedRowHandle, "Địa chỉ").ToString();
                edtNgayThue.Text = gvDatPhong.GetRowCellValue(e.FocusedRowHandle, "Ngày Thuê").ToString();
                edtNgayTra.Text = gvDatPhong.GetRowCellValue(e.FocusedRowHandle, "Ngày Trả").ToString();
                edtSLNguoi.Text = gvDatPhong.GetRowCellValue(e.FocusedRowHandle, "Số Người").ToString();
                edtSLPhong.Text = gvDatPhong.GetRowCellValue(e.FocusedRowHandle, "Số Phòng").ToString();
                if (gvDatPhong.GetRowCellValue(e.FocusedRowHandle, "Giới Tính").ToString().Equals("Nam"))
                {
                    radGioiTinh.Properties.Items[0].Value = true;
                }
                cbMaPhong.EditValue = gvDatPhong.GetRowCellValue(e.FocusedRowHandle, "Mã Phòng").ToString();
            }
        }

        private void gvDatPhong_ShowingEditor(object sender, CancelEventArgs e)
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

        private void btnThemPhong_Click(object sender, EventArgs e)
        {

            string maDP = edtMaDP.Text;
            string maPhong = cbMaPhong.EditValue.ToString();
            string maKH = edtMaKH.Text;
            string tenKH = edtTenKH.Text;
            string cmnd = edtCMND.Text;
            string diaChi = edtDiaChi.Text;
            string dienThoai = edtSDT.Text;
            string email = edtEmail.Text;
            string gioitinh = radGioiTinh.Properties.Items[radGioiTinh.SelectedIndex].Description;
            string ngaythue = edtNgayThue.Text;
            string ngaytra = edtNgayTra.Text;
            string slNguoi = edtSLNguoi.Text;
            string slPhong = edtSLPhong.Text;
            if (maDP == "" || maPhong == "" || maKH == "" || tenKH == "" || cmnd == "" || diaChi == "" || dienThoai == "" || email == "" || ngaythue == "" || ngaytra == "" || slNguoi == "" || slPhong == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isNumber(dienThoai) || (!isNumber(cmnd)))
            {
                XtraMessageBox.Show("Số điện thoại hoặc CMND phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlTrungKhoa = "SELECT * from datphong WHERE MaDatPhong = '" + maDP + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (reader.Read())//Nếu đọc được 
                {
                    XtraMessageBox.Show("Đã tồn tại mã đặt phòng này", "Thông báo", MessageBoxButtons.OK);
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    string sqlTrungKhoaKH = "Select * from khachhang where MaKH = '" + maKH + "'";
                    SqlCommand commandTrungKhoa1 = new SqlCommand(sqlTrungKhoaKH, conn);
                    SqlDataReader reader1 = commandTrungKhoa1.ExecuteReader();
                    if (reader1.Read())//Nếu đọc được 
                    {
                        XtraMessageBox.Show("Đã tồn tại mã khách hàng này", "Thông báo", MessageBoxButtons.OK);
                        reader1.Close();
                    }
                    else
                    {
                        reader1.Close();
                        string sqlInsert = "INSERT INTO khachhang VALUES(N'" + maKH + "',N'" + tenKH + "',N'" + gioitinh + "',N'" + diaChi + "',N'" + cmnd + "',N'" + dienThoai + "',N'" + email + "')";
                        SqlCommand commandUpdate = new SqlCommand(sqlInsert, conn);
                        commandUpdate.ExecuteNonQuery();
                        string sqlInsertDP = "INSErt into datphong values('" + maDP + "','" + maPhong + "','" + maKH + "','" + ngaythue + "','" + ngaytra + "','" + slPhong + "','" + slNguoi + "')";
                        SqlCommand commanDP = new SqlCommand(sqlInsertDP, conn);
                        commanDP.ExecuteNonQuery();
                        XtraMessageBox.Show("Thêm thành công ", "Thông báo", MessageBoxButtons.OK);
                        loadData();
                    }

                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string maDP = edtMaDP.Text;
            string maPhong = cbMaPhong.EditValue.ToString();
            string maKH = edtMaKH.Text;
            string tenKH = edtTenKH.Text;
            string cmnd = edtCMND.Text;
            string diaChi = edtDiaChi.Text;
            string dienThoai = edtSDT.Text;
            string email = edtEmail.Text;
            string gioitinh = radGioiTinh.Properties.Items[radGioiTinh.SelectedIndex].Description;
            string ngaythue = edtNgayThue.Text;
            string ngaytra = edtNgayTra.Text;
            string slNguoi = edtSLNguoi.Text;
            string slPhong = edtSLPhong.Text;
            if (maDP == "" || maPhong == "" || maKH == "" || tenKH == "" || cmnd == "" || diaChi == "" || dienThoai == "" || email == "" || ngaythue == "" || ngaytra == "" || slNguoi == "" || slPhong == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isNumber(dienThoai) || (!isNumber(cmnd)))
            {
                XtraMessageBox.Show("Số điện thoại hoặc CMND phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlUpdate = "UPDATE khachhang Set HoTen=N'" + tenKH + "',GioiTinh=N'" + gioitinh + "',DiaChi=N'" + diaChi + "',CMND=N'" + cmnd + "',DienThoai=N'" + dienThoai + "',Email=N'" + email + "' WHERE MaKH=N'" + maKH + "'";
                SqlCommand commandUpdate = new SqlCommand(sqlUpdate, conn);
                commandUpdate.ExecuteNonQuery();
                string sqlUpdate1 = "UPDATE datphong Set MaPhong=N'" + maPhong + "',MaKH='" + maKH + "',NgayThue='" + ngaythue + "',NgayTra='" + ngaytra + "',SLPhong='" + slPhong + "',SLNguoi='" + slNguoi + "' WHERE MaDatPhong=N'" + maDP + "'";
                SqlCommand commandUpdate1 = new SqlCommand(sqlUpdate1, conn);
                commandUpdate1.ExecuteNonQuery();
                XtraMessageBox.Show("Sửa thành công ", "Thông báo", MessageBoxButtons.OK);
                loadData();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maDP = gvDatPhong.GetRowCellValue(gvDatPhong.FocusedRowHandle, "Mã ĐP").ToString();
            if (maDP != null)
            {
                if (XtraMessageBox.Show("Bạn có chắc muốn xóa bản ghi này không", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string sqlDelete = "DELETE FROM datphong where MaDatPhong = N'" + maDP + "'";
                    SqlCommand commandDelete = new SqlCommand(sqlDelete, conn);
                    commandDelete.ExecuteNonQuery();
                    XtraMessageBox.Show("Đặt phòng có mã: " + maDP + " đã được xóa", "Thông báo", MessageBoxButtons.OK);
                    //gvKhachHang.DeleteRow(gvKhachHang.FocusedRowHandle);
                    loadData();
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            edtMaDP.Text = "";
            edtMaKH.Text = "";
            edtTenKH.Text = "";
            edtCMND.Text = "";
            edtSDT.Text = "";
            edtEmail.Text = "";
            edtDiaChi.Text = "";
            edtNgayThue.Text = "";
            edtNgayTra.Text = "";
            edtSLNguoi.Text = "";
            edtSLPhong.Text = "";
            loadData();
        }

        private void gvDatPhong_ShownEditor(object sender, EventArgs e)
        {

        }
    }
}