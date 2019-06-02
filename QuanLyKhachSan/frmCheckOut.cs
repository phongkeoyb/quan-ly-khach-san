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
    public partial class frmCheckOut : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection conn;
        public frmCheckOut()
        {
            InitializeComponent();
        }

        private void frmCheckOut_Load(object sender, EventArgs e)
        {
            conn = ConnectionDatabase.getInstance();
            ConnectionDatabase.openConnectionStage();
            getPhongThue();
        }

        private void cbPhongThue_SelectedValueChanged(object sender, EventArgs e)
        {
            string maphongthue = cbPhongThue.Text;
            string sql = "SELECT top 1 mathuephong, Phong.MaPhong, NhanVien.MaNV, KhachHang.MaKH,KhachHang.HoTen," +
               "Khachhang.GioiTinh,KhachHang.CMND as 'CMND', KhachHang.DIENTHOAI,KhachHang.DIACHI, KhachHang.Email, NgayThue, NgayTra"
               + ",SLNguoi from Phong inner join ThuePhong on Phong.MaPhong = ThuePhong.MaPhong "
               + "inner join KhachHang on ThuePhong.MaKH = KhachHang.MaKH inner join NhanVien on ThuePhong.MaNV = NhanVien.MaNV where thuephong.maphong = '" + maphongthue + "' order by mathuephong desc";

            SqlCommand commandGetData = new SqlCommand(sql, conn);
            SqlDataReader readerGetData = commandGetData.ExecuteReader();
            while (readerGetData.Read())
            {
                edtMaThuePhong.Text = readerGetData.GetString(0);
                edtMaPhong.Text = readerGetData.GetString(1); ;
                edtMaNV.Text = readerGetData.GetString(2);
                edtMaKH.Text = readerGetData.GetString(3);
                edtTenKH.Text = readerGetData.GetString(4);
                edtCMND.Text = readerGetData.GetString(6);
                edtSDT.Text = readerGetData.GetString(7);
                edtEmail.Text = readerGetData.GetString(9);
                edtDiaChi.Text = readerGetData.GetString(8);
                edtNgayThue.Text = readerGetData.GetDateTime(10).ToString();
                edtNgayTra.Text = readerGetData.GetDateTime(11).ToString();
                edtSoNguoi.Text = readerGetData.GetString(12);
            }
            readerGetData.Close();
            edtNgayTra.Text = DateTime.Now.ToString();

            string sqlDichVu = "select masddv as 'Mã SD',sudungdv.madv as 'Mã DV',ngaysd as 'Ngày', dichvu.tendv as 'Tên DV'," +
                "gia as 'Giá', soluong as 'Số lượng',soluong*gia as 'Thành tiền' from dichvu inner join sudungdv on " +
                "dichvu.madv = sudungdv.madv where mathuephong = '" + edtMaThuePhong.Text + "'";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlDichVu, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            gridDichVu.DataSource = dataSet.Tables[0];
        }
        private void btnXuatHoaDon_Click(object sender, EventArgs e)
        {
            string maTP = edtMaThuePhong.Text;
            string tenkh = edtTenKH.Text;
            string maKH = edtMaKH.Text;
            string gioitinh = radGioiTinh.Properties.Items[radGioiTinh.SelectedIndex].Description;
            string cmnd = edtCMND.Text;
            string sdt = edtSDT.Text;
            string diachi = edtDiaChi.Text;
            string email = edtEmail.Text;
            string phong = edtMaPhong.Text;
            string ngaythue = edtNgayThue.Text;
            string ngaytra = edtNgayTra.Text;
            string slNguoi = edtSoNguoi.Text;
            string manv = edtMaNV.Text;
            if (cbPhongThue.Text.Equals(""))
            {
                XtraMessageBox.Show("Bạn chưa chọn một phòng để thanh toán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (maTP == "" || phong == "" || manv == "" || maKH == "" || tenkh == "" || cmnd == "" || diachi == "" || sdt == "" || email == "" || ngaythue == "" || ngaytra == "" || slNguoi == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isNumber(sdt) || !isNumber(cmnd) )
            {
                XtraMessageBox.Show("Số điện thoại,CMND, Số lượng người phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                List<UserService> dichvuList = new List<UserService>();
                string ngaylap = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                string sohd = edtMaThuePhong.Text.Substring(2);
                string tennv = "";
                DateTime date1 = Convert.ToDateTime(ngaythue);
                DateTime date2 = Convert.ToDateTime(ngaytra);
                int sophut = (int)(date2 - date1).TotalMinutes;
                decimal gio = ((decimal)sophut) / ((decimal)60);
                int sogio = (int)Math.Ceiling(gio);
                string giaphong1 = "";

                //Lấy tên nhân viên
                string sqlTenNV = "Select hoten from nhanvien where manv = '" + manv + "'";
                SqlCommand commandTenNV = new SqlCommand(sqlTenNV, conn);
                SqlDataReader readerTenNV = commandTenNV.ExecuteReader();
                while (readerTenNV.Read())
                {
                    tennv = readerTenNV.GetString(0);
                }
                readerTenNV.Close();

                //Lấy giá phòng cần tính
                string sqlGiaPhong = "select giaphong from phong inner join loaiphong on phong.maloai = loaiphong.maloai where maphong='" + phong + "'";
                SqlCommand commanGiaPhong = new SqlCommand(sqlGiaPhong, conn);
                SqlDataReader readerGiaPhong = commanGiaPhong.ExecuteReader();
                while (readerGiaPhong.Read())
                {
                    giaphong1 = readerGiaPhong.GetString(0);
                }
                readerGiaPhong.Close();
                int giaphong = Convert.ToInt32(giaphong1);
                int tienphong = tinhTienPhong(sophut, giaphong);
                int tiendv = 0;

                //Kiem tra phong co su dung dich vu khong, co thi lay ra dua vao list để in report
                string sqlIsService = "select *from sudungdv where mathuephong ='" + edtMaThuePhong.Text + "'";
                SqlCommand commandIsService = new SqlCommand(sqlIsService, conn);
                SqlDataReader readerIsService = commandIsService.ExecuteReader();
                if (readerIsService.Read())
                {
                    readerIsService.Close();

                    //Lấy tất cả dịch vụ của phòng để in ra report
                    string sqlDichVu = "select ngaysd as 'Ngày', dichvu.tendv as 'Tên DV'," +
                       "gia as 'Giá', soluong as 'Số lượng',soluong*gia as 'Thành tiền' from dichvu inner join sudungdv on " +
                       "dichvu.madv = sudungdv.madv where mathuephong = '" + edtMaThuePhong.Text + "'";
                    SqlCommand command = new SqlCommand(sqlDichVu, conn);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        dichvuList.Add(new UserService()
                        {
                            ngay = reader.GetDateTime(0).ToString("MM/dd/yyyy HH:mm"),
                            tendv = reader.GetString(1),
                            gia = reader.GetInt32(2),
                            soluong = reader.GetInt32(3),
                            thanhtien = reader.GetInt32(4)
                        });
                    }
                    reader.Close();

                    //Lấy tổng tiền dv
                    string sqlTongDV = "select sum(soluong*gia) from sudungdv inner join dichvu on sudungdv.MaDV=dichvu.MaDV where mathuephong='" + edtMaThuePhong.Text + "'";
                    SqlCommand sqlCommand = new SqlCommand(sqlTongDV, conn);
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        tiendv = dataReader.GetInt32(0);
                    }
                    dataReader.Close();
                }
                readerIsService.Close();

                string tongtien = String.Format("{0:0,0}", tienphong + tiendv);
                //using (frmPrint frm = new frmPrint())
                //{
                //    frm.PrintHoaDon(tenkh, cmnd, sdt, diachi, phong, ngaythue, ngaytra, sogio.ToString(), sohd, dichvuList, ngaylap, tennv, tienphong.ToString(), tongtien);
                //    frm.ShowDialog();
                //}
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            string maTP = edtMaThuePhong.Text;
            string tenKH = edtTenKH.Text;
            string maKH = edtMaKH.Text;
            string gioitinh = radGioiTinh.Properties.Items[radGioiTinh.SelectedIndex].Description;
            string cmnd = edtCMND.Text;
            string dienThoai = edtSDT.Text;
            string diaChi = edtDiaChi.Text;
            string email = edtEmail.Text;
            string maPhong = edtMaPhong.Text;
            string ngaythue = edtNgayThue.Text;
            string ngaytra = edtNgayTra.Text;
            string slNguoi = edtSoNguoi.Text;
            string maNV = edtMaNV.Text;
            string ngaylaphd = DateTime.Now.ToString();
            if (cbPhongThue.Text.Equals(""))
            {
                XtraMessageBox.Show("Bạn chưa chọn một phòng để thanh toán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (maTP == "" || maPhong == "" || maKH == "" || tenKH == "" || cmnd == "" || diaChi == "" || dienThoai == "" || email == "" || ngaythue == "" || ngaytra == "" || slNguoi == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isNumber(dienThoai) || (!isNumber(cmnd)))
            {
                XtraMessageBox.Show("Số điện thoại,CMND, Số lượng người phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    if (XtraMessageBox.Show("Bạn có chắc chắn thanh toán phòng này", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //cập nhật lại thông tin khách hàng
                        string sqlUpdate = "UPDATE khachhang Set HoTen=N'" + tenKH + "',GioiTinh=N'" + gioitinh + "',DiaChi=N'" + diaChi + "',CMND=N'" + cmnd + "',DienThoai=N'" + dienThoai + "',Email=N'" + email + "' WHERE MaKH=N'" + maKH + "'";
                        SqlCommand commandUpdate = new SqlCommand(sqlUpdate, conn);
                        commandUpdate.ExecuteNonQuery();

                        //Cập nhật thông tin phòng thuê//
                        DateTime date1 = Convert.ToDateTime(ngaythue);
                        DateTime date2 = Convert.ToDateTime(ngaytra);
                        int sophut = (int)(date2 - date1).TotalMinutes;
                        decimal gio = ((decimal)sophut) / ((decimal)60);
                        int sogio = (int)Math.Ceiling(gio);
                        string giaphong1 = "";

                        //Lấy giá phòng cần tính
                        string sqlGiaPhong = "select giaphong from phong inner join loaiphong on phong.maloai = loaiphong.maloai where maphong='" + edtMaPhong.Text + "'";
                        SqlCommand commanGiaPhong = new SqlCommand(sqlGiaPhong, conn);
                        SqlDataReader readerGiaPhong = commanGiaPhong.ExecuteReader();
                        while (readerGiaPhong.Read())
                        {
                            giaphong1 = readerGiaPhong.GetString(0);
                        }
                        readerGiaPhong.Close();
                        int giaphong = Convert.ToInt32(giaphong1);
                        int tienphong = tinhTienPhong(sophut, giaphong);

                        int tiendv = 0;
                        //Nếu có sử dụng dv thì lấy tổng tiền dv
                        string sqlIsService = "select * from sudungdv where mathuephong ='" + edtMaThuePhong.Text + "'";
                        SqlCommand commandIsService = new SqlCommand(sqlIsService, conn);
                        SqlDataReader readerIsService = commandIsService.ExecuteReader();
                        if (readerIsService.Read())
                        {
                            readerIsService.Close();
                            //Lấy tổng tiền dv
                            string sqlTongDV = "select sum(soluong*gia) from sudungdv inner join dichvu on sudungdv.MaDV=dichvu.MaDV where mathuephong='" + edtMaThuePhong.Text + "'";
                            SqlCommand sqlCommand = new SqlCommand(sqlTongDV, conn);
                            SqlDataReader dataReader = sqlCommand.ExecuteReader();
                            while (dataReader.Read())
                            {
                                tiendv = dataReader.GetInt32(0);
                            }
                            dataReader.Close();
                        }
                        readerIsService.Close();
                        string sqlUpdate1 = "UPDATE thuephong Set MaPhong=N'" + maPhong + "',MaKH='" + maKH + "',MaNV='" + maNV + "',NgayThue='" + ngaythue + "',NgayTra='" + ngaytra + "',SLNguoi='" + slNguoi + "',tienphong=" + tienphong + ",tiendv=" + tiendv + " WHERE mathuephong=N'" + maTP + "'";
                        SqlCommand commandUpdate1 = new SqlCommand(sqlUpdate1, conn);
                        commandUpdate1.ExecuteNonQuery();
                        int thanhtien = tienphong + tiendv;
                        //Cập nhật lại trạng thái phòng
                        string sqlTTPhong = "update phong set trangthai='0' where maphong='" + maPhong + "'";
                        SqlCommand commandTTPhong = new SqlCommand(sqlTTPhong, conn);
                        commandTTPhong.ExecuteNonQuery();
                        refreshData();

                        string sqlInsertHD = "INSERT INTO hoadon VALUES('" + maTP + "',N'" + ngaylaphd + "', '" + thanhtien + "' )";
                        SqlCommand commandHoaDon = new SqlCommand(sqlInsertHD, conn);
                        commandHoaDon.ExecuteNonQuery();

                        XtraMessageBox.Show("Hoàn tất thanh toán", "Thông báo", MessageBoxButtons.OK);
                    }
                }
            }
        }

        int tinhTienPhong(int sophut, int giaphong)
        {
            decimal gio = ((decimal)sophut) / ((decimal)60);
            int sogio = Convert.ToInt32(Math.Ceiling(gio).ToString());

            decimal ngay = ((decimal)sophut) / ((decimal)1440);
            int songay = (int)Math.Floor(ngay);

            if (songay < 1)
            {
                if (sogio <= 2)
                {
                    return giaphong * 30 / 100;
                }
                else if (sogio <= 4)
                {
                    return giaphong / 2;
                }
                else if (sogio <= 12)
                {
                    return giaphong * 70 / 100;
                }
                else return giaphong;
            }
            else
            {
                int tienNgay = songay * giaphong;
                int sogioDu = sogio - songay * 24;
                int tienThemGio = 0;
                if (sogioDu <= 2)
                {
                    tienThemGio = giaphong * 30 / 100;
                }
                else if (sogioDu <= 4)
                {
                    tienThemGio = giaphong / 2;
                }
                else if (sogioDu <= 12)
                {
                    tienThemGio = giaphong * 70 / 100;
                }
                else tienThemGio = giaphong;
                return tienNgay + tienThemGio;

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

        void refreshData()
        {
            cbPhongThue.Text = "";
            edtMaThuePhong.Text = "";
            edtMaPhong.Text = "";
            edtMaKH.Text = "";
            edtMaNV.Text = "";
            edtCMND.Text = "";
            edtDiaChi.Text = "";
            edtEmail.Text = "";
            edtNgayThue.Text = "";
            edtNgayTra.Text = "";
            edtSoNguoi.Text = "";
            edtSDT.Text = "";
            edtTenKH.Text = "";
            getPhongThue();
        }

        void getPhongThue()
        {
            string sqlPhongThue = "Select maphong from phong where trangthai='1'";
            SqlCommand commandPhongThue = new SqlCommand(sqlPhongThue, conn);
            SqlDataReader readerPhongThue = commandPhongThue.ExecuteReader();
            while (readerPhongThue.Read())
            {
                cbPhongThue.Properties.Items.Add(readerPhongThue.GetString(0));
            }
            readerPhongThue.Close();
        }
    }
}