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
    public partial class frmCheckIn : DevExpress.XtraEditors.XtraForm
    {
        private string mMaPhong = "";
        SqlConnection conn;
        public frmCheckIn()
        {
            InitializeComponent();
        }

        public frmCheckIn(string maphong)
        {
            InitializeComponent();
            mMaPhong = maphong;
        }

        private void frmCheckIn_Load(object sender, EventArgs e)
        {

            conn = ConnectionDatabase.getInstance();
            ConnectionDatabase.openConnectionStage();
            layPhongTrong();

            //Lấy dữ liệu MaNV đưa vào cột Mã NV
            string sqlMaNV = "Select manv from nhanvien";
            SqlCommand commandMaNV = new SqlCommand(sqlMaNV, conn);
            SqlDataReader readerMaNV = commandMaNV.ExecuteReader();
            while (readerMaNV.Read())
            {
                edtMaNV.Properties.Items.Add(readerMaNV.GetString(0));
            }
            readerMaNV.Close();
            loadData();

            //Nếu như được gọi từ form tìm phòng trống thì xóa dữ liệu các field hiện tại, tạo dữ liệu mới
            if (mMaPhong != "")
            {
                string mathuephong = "";
                string makh = "";
                //Lấy mã thuê phòng cuối +1 để đưa vô textedit mathuephong
                string sqlMaThuePhong = "Select top 1 mathuephong from thuephong order by mathuephong desc";
                SqlCommand command = new SqlCommand(sqlMaThuePhong, conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    mathuephong = reader.GetString(0);
                }
                reader.Close();

                //Lấy mã KH cuối +1 để đưa vô textedit mathuephong
                string sqlMaKH = "Select top 1 makh from khachhang order by makh desc";
                SqlCommand commandMaKH = new SqlCommand(sqlMaKH, conn);
                SqlDataReader readerKH = commandMaKH.ExecuteReader();
                while (readerKH.Read())
                {
                    makh = readerKH.GetString(0);
                }

                readerKH.Close();
                if (makh.Equals(""))
                {
                    edtMaKH.Text = "KH001";
                }
                else
                {
                    edtMaKH.Text = laychuoitudong(makh);
                }

                if (mathuephong.Equals(""))
                {
                    edtMaThuePhong.Text = "TP001";
                }
                else
                {
                    edtMaThuePhong.Text = laychuoitudong(mathuephong);
                }
                //Lấy ngày giờ hiện tại đưa vào ngày thuê
                edtNgayThue.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                edtTenKH.Text = "";
                edtCMND.Text = "";
                edtSDT.Text = "";
                edtEmail.Text = "";
                edtDiaChi.Text = "";
                edtNgayTra.Text = "";

                edtMaPhong.Text = mMaPhong;
                edtMaNV.Text = "";
                edtNguoi.Text = "";
            }
        }
        string laychuoitudong(string chuoi)
        {
            string cutChar = chuoi.Substring(0, 2);
            string cutNumber = chuoi.Substring(2);
            int number = Int32.Parse(cutNumber);
            if (number >= 0 && number < 9) return cutChar + "00" + (number + 1);
            else if (number >= 9 && number < 99) return cutChar + "0" + (number + 1);
            else return cutChar + (number + 1);
        }


        private void loadData()
        {//Load dữ liệu có thể sài nhiều lần sau khi xóa sửa thêm, đều có thể gọi lại để kiểm tra
            string sql = "SELECT mathuephong AS 'Mã TP', Phong.MaPhong AS 'Mã Phòng', NhanVien.MaNV as 'Mã NV', KhachHang.MaKH as 'Mã KH' ,KhachHang.HoTen AS 'Tên KH'," +
                "Khachhang.GioiTinh as 'Giới Tính',KhachHang.CMND as 'CMND', KhachHang.DIENTHOAI as 'SĐT',KhachHang.DIACHI as 'Địa chỉ', KhachHang.Email as 'Email' , NgayThue as 'Ngày Thuê', NgayTra as 'Ngày Trả'"
                + ",SLNguoi as 'Số Người' from Phong inner join ThuePhong on Phong.MaPhong = ThuePhong.MaPhong "
                + "inner join KhachHang on ThuePhong.MaKH = KhachHang.MaKH inner join NhanVien on ThuePhong.MaNV = NhanVien.MaNV order by mathuephong desc";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            gridThuePhong.DataSource = dataSet.Tables[0];
        }

        private void btnThue_Click(object sender, EventArgs e)
        {
            string maTP = edtMaThuePhong.Text;
            string maPhong = edtMaPhong.EditValue.ToString();
            string maKH = edtMaKH.Text;
            string tenKH = edtTenKH.Text;
            string maNV = edtMaNV.EditValue.ToString();
            string cmnd = edtCMND.Text;
            string diaChi = edtDiaChi.Text;
            string dienThoai = edtSDT.Text;
            string email = edtEmail.Text;
            string gioitinh = radGioiTinh.Properties.Items[radGioiTinh.SelectedIndex].Description;
            string ngaythue = edtNgayThue.Text;
            string ngaytra = edtNgayTra.Text;
            string slNguoi = edtNguoi.Text;
            if (maTP == "" || maPhong == "" || maKH == "" || tenKH == "" || cmnd == "" || diaChi == "" || dienThoai == "" || email == "" || ngaythue == "" || ngaytra == "" || slNguoi == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isNumber(dienThoai) || (!isNumber(cmnd)))
            {
                XtraMessageBox.Show("Số điện thoại hoặc CMND phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlTrungKhoa = "SELECT * from thuephong WHERE MaThuePhong = '" + maTP + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (reader.Read())//Nếu đọc được 
                {
                    XtraMessageBox.Show("Đã tồn tại mã thuê phòng này", "Thông báo", MessageBoxButtons.OK);
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
                        string trangthai = "";
                        string sqlMaPhong = "Select trangthai from phong where maphong='" + maPhong + "'";
                        SqlCommand commandMaPhong = new SqlCommand(sqlMaPhong, conn);
                        SqlDataReader readerMaPhong = commandMaPhong.ExecuteReader();
                        while (readerMaPhong.Read())
                        {
                            trangthai = readerMaPhong.GetString(0);
                        }
                        readerMaPhong.Close();
                        if (trangthai.Equals("1"))
                        {
                            XtraMessageBox.Show("Phòng này đã có người đặt rồi", "Thông báo", MessageBoxButtons.OK);
                        }
                        else
                        {
                            string sqlInsert = "INSERT INTO khachhang VALUES(N'" + maKH + "',N'" + tenKH + "',N'" + gioitinh + "',N'" + diaChi + "',N'" + cmnd + "',N'" + dienThoai + "',N'" + email + "')";
                            SqlCommand commandUpdate = new SqlCommand(sqlInsert, conn);
                            commandUpdate.ExecuteNonQuery();
                            string sqlInsertTP = "Insert into thuephong values('" + maTP + "','" + maPhong + "','" + maKH + "','" + maNV + "','" + slNguoi + "','" + ngaythue + "','" + ngaytra + "','" + 0 + "','" + 0 + "')";
                            SqlCommand commanTP = new SqlCommand(sqlInsertTP, conn);
                            commanTP.ExecuteNonQuery();
                            string sqlPhong = "update phong set trangthai='1' where maphong='" + maPhong + "'";
                            SqlCommand commandUpdatePhong = new SqlCommand(sqlPhong, conn);
                            commandUpdatePhong.ExecuteNonQuery();
                            XtraMessageBox.Show("Thêm thành công ", "Thông báo", MessageBoxButtons.OK);
                            loadData();
                            refreshForm();
                            layPhongTrong();//Combobox maphong
                        }
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string maTP = edtMaThuePhong.Text;
            string maPhong = edtMaPhong.EditValue.ToString();
            string maKH = edtMaKH.Text;
            string tenKH = edtTenKH.Text;
            string maNV = edtMaNV.EditValue.ToString();
            string cmnd = edtCMND.Text;
            string diaChi = edtDiaChi.Text;
            string dienThoai = edtSDT.Text;
            string email = edtEmail.Text;
            string gioitinh = radGioiTinh.Properties.Items[radGioiTinh.SelectedIndex].Description;
            string ngaythue = edtNgayThue.Text;
            string ngaytra = edtNgayTra.Text;
            string slNguoi = edtNguoi.Text;
            if (maTP == "" || maPhong == "" || maKH == "" || tenKH == "" || cmnd == "" || diaChi == "" || dienThoai == "" || email == "" || ngaythue == "" || ngaytra == "" || slNguoi == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isNumber(dienThoai) || (!isNumber(cmnd)))
            {
                XtraMessageBox.Show("Số điện thoại hoặc CMND phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlTrungKhoa = "SELECT * from thuephong WHERE mathuephong = '" + maTP + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (!reader.Read())//Nếu không đọc được 
                {
                    XtraMessageBox.Show("Chưa tồn tại mã thuê phòng này", "Thông báo", MessageBoxButtons.OK);
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    string sqlUpdate = "UPDATE khachhang Set HoTen=N'" + tenKH + "',GioiTinh=N'" + gioitinh + "',DiaChi=N'" + diaChi + "',CMND=N'" + cmnd + "',DienThoai=N'" + dienThoai + "',Email=N'" + email + "' WHERE MaKH=N'" + maKH + "'";
                    SqlCommand commandUpdate = new SqlCommand(sqlUpdate, conn);
                    commandUpdate.ExecuteNonQuery();
                    string sqlUpdate1 = "UPDATE thuephong Set MaPhong=N'" + maPhong + "',MaKH='" + maKH + "',MaNV='" + maNV + "',NgayThue='" + ngaythue + "',NgayTra='" + ngaytra + "',SLNguoi='" + slNguoi + "' WHERE mathuephong=N'" + maTP + "'";
                    SqlCommand commandUpdate1 = new SqlCommand(sqlUpdate1, conn);
                    commandUpdate1.ExecuteNonQuery();
                    XtraMessageBox.Show("Sửa thành công ", "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }

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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maTP = gvThuePhong.GetRowCellValue(gvThuePhong.FocusedRowHandle, "Mã TP").ToString();
            if (maTP != null)
            {
                if (XtraMessageBox.Show("Bạn có chắc muốn xóa bản ghi này không", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string sqlDelete = "DELETE FROM thuephong where MaThuePhong = N'" + maTP + "'";
                    SqlCommand commandDelete = new SqlCommand(sqlDelete, conn);
                    commandDelete.ExecuteNonQuery();
                    XtraMessageBox.Show("Thuê phòng có mã: " + maTP + " đã được xóa", "Thông báo", MessageBoxButtons.OK);
                    //gvKhachHang.DeleteRow(gvKhachHang.FocusedRowHandle);
                    refreshForm();
                    loadData();
                    string sqlPhong = "update phong set trangthai='0' where maphong='" + edtMaPhong.Text + "'";
                    SqlCommand commandUpdatePhong = new SqlCommand(sqlPhong, conn);
                    commandUpdatePhong.ExecuteNonQuery();

                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            string mathuephong = "";
            string makh = "";
            //Lấy mã thuê phòng cuối +1 để đưa vô textedit mathuephong
            string sqlMaThuePhong = "Select top 1 mathuephong from thuephong order by mathuephong desc";
            SqlCommand command = new SqlCommand(sqlMaThuePhong, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                mathuephong = reader.GetString(0);
            }
            reader.Close();

            //Lấy mã KH cuối +1 để đưa vô textedit mathuephong
            string sqlMaKH = "Select top 1 makh from khachhang order by makh desc";
            SqlCommand commandMaKH = new SqlCommand(sqlMaKH, conn);
            SqlDataReader readerKH = commandMaKH.ExecuteReader();
            while (readerKH.Read())
            {
                makh = readerKH.GetString(0);
            }
            readerKH.Close();
            if (makh.Equals(""))
            {
                edtMaKH.Text = "KH001";
            }
            else
            {
                edtMaKH.Text = laychuoitudong(makh);
            }

            if (mathuephong.Equals(""))
            {
                edtMaThuePhong.Text = "TP001";
            }
            else
            {
                edtMaThuePhong.Text = laychuoitudong(mathuephong);
            }

            //Lấy ngày giờ hiện tại đưa vào ngày thuê
            edtNgayThue.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            edtTenKH.Text = "";
            edtCMND.Text = "";
            edtSDT.Text = "";
            edtEmail.Text = "";
            edtDiaChi.Text = "";
            edtNgayTra.Text = "";
            edtMaPhong.Text = "";
            edtMaNV.Text = "";
            edtNguoi.Text = "";
        }

        private void gvThuePhong_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
            {

            }
            else
            {
                edtMaThuePhong.Text = gvThuePhong.GetRowCellValue(e.FocusedRowHandle, "Mã TP").ToString();
                edtMaKH.Text = gvThuePhong.GetRowCellValue(e.FocusedRowHandle, "Mã KH").ToString();
                edtTenKH.Text = gvThuePhong.GetRowCellValue(e.FocusedRowHandle, "Tên KH").ToString();
                edtCMND.Text = gvThuePhong.GetRowCellValue(e.FocusedRowHandle, "CMND").ToString();
                edtSDT.Text = gvThuePhong.GetRowCellValue(e.FocusedRowHandle, "SĐT").ToString();
                edtEmail.Text = gvThuePhong.GetRowCellValue(e.FocusedRowHandle, "Email").ToString();
                edtDiaChi.Text = gvThuePhong.GetRowCellValue(e.FocusedRowHandle, "Địa chỉ").ToString();
                edtNgayThue.Text = gvThuePhong.GetRowCellValue(e.FocusedRowHandle, "Ngày Thuê").ToString();
                edtNgayTra.Text = gvThuePhong.GetRowCellValue(e.FocusedRowHandle, "Ngày Trả").ToString();
                edtNguoi.Text = gvThuePhong.GetRowCellValue(e.FocusedRowHandle, "Số Người").ToString();
                edtMaPhong.EditValue = gvThuePhong.GetRowCellValue(e.FocusedRowHandle, "Mã Phòng").ToString();
                edtMaNV.Text = gvThuePhong.GetRowCellValue(e.FocusedRowHandle, "Mã NV").ToString();
            }
        }

        private void gvThuePhong_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void gvThuePhong_DoubleClick(object sender, EventArgs e)
        {
            string mathuephong = gvThuePhong.GetRowCellValue(gvThuePhong.FocusedRowHandle, "Mã TP").ToString();
            frmService f = new frmService(mathuephong);
            f.MdiParent = frmHome.ActiveForm;
            f.Show();
        }

        private void layPhongTrong()
        {
            //Lấy dữ liệu maphong đưa vào cột Mã phòng
            string sqlMaPhong = "Select maphong from phong where trangthai = '0' ";
            SqlCommand commandMaPhong = new SqlCommand(sqlMaPhong, conn);
            SqlDataReader readerMaPhong = commandMaPhong.ExecuteReader();
            while (readerMaPhong.Read())
            {
                edtMaPhong.Properties.Items.Add(readerMaPhong.GetString(0));
            }
            readerMaPhong.Close();
        }

        public void refreshForm()
        {
            edtNgayThue.Text = "";
            edtTenKH.Text = "";
            edtCMND.Text = "";
            edtSDT.Text = "";
            edtEmail.Text = "";
            edtDiaChi.Text = "";
            edtNgayTra.Text = "";
            edtMaPhong.Text = "";
            edtMaNV.Text = "";
            edtNguoi.Text = "";
            edtMaThuePhong.Text = "";
            edtMaKH.Text = "";
        }
    }
}