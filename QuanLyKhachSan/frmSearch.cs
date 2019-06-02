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
    public partial class frmSearch : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection conn;
        public frmSearch()
        {
            InitializeComponent();
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            conn = ConnectionDatabase.getInstance();
            ConnectionDatabase.openConnectionStage();
            string sqlMaPhong = "select maphong from phong";
            SqlCommand command = new SqlCommand(sqlMaPhong, conn);
            SqlDataReader reader = command.ExecuteReader();
            edtMaPhong.Properties.Items.Add("");
            while (reader.Read())
            {
                edtMaPhong.Properties.Items.Add(reader.GetString(0));
            }
            reader.Close();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            gvThongTin.Columns.Clear();
            if (edtTenKH.Text.Equals("") && edtMaPhong.Text.Equals("") && edtTuNgay.Text.Equals("") && edtDenNgay.Text.Equals(""))
            {
                XtraMessageBox.Show("Nhập dữ liệu để tìm kiếm", "Thông báo", MessageBoxButtons.OK);
            }
            else if (!edtTenKH.Text.Equals("") && edtMaPhong.Text.Equals("") && edtTuNgay.Text.Equals("") && edtDenNgay.Text.Equals(""))
            {
                string sqlKH = "select khachhang.hoten as 'Tên KH', maphong as 'Phòng',ngaythue as 'Ngày thuê', ngaytra as 'Ngày trả' " +
                    "from thuephong inner join khachhang on thuephong.makh = khachhang.makh where hoten = N'" + edtTenKH.Text + "'";
                SqlDataAdapter adapter1 = new SqlDataAdapter(sqlKH, conn);
                DataSet dataSet = new DataSet();
                adapter1.Fill(dataSet);
                gridThongTin.DataSource = dataSet.Tables[0];
            }
            else if (edtTenKH.Text.Equals("") && !edtMaPhong.Text.Equals("") && edtTuNgay.Text.Equals("") && edtDenNgay.Text.Equals(""))
            {
                string sqlPhong = "select khachhang.hoten as 'Tên KH', maphong as 'Phòng',ngaythue as 'Ngày thuê', ngaytra as 'Ngày trả' " +
                    "from thuephong inner join khachhang on thuephong.makh = khachhang.makh where maphong = '" + edtMaPhong.Text + "'";
                SqlDataAdapter adapter2 = new SqlDataAdapter(sqlPhong, conn);
                DataSet dataSet1 = new DataSet();
                adapter2.Fill(dataSet1);
                gridThongTin.DataSource = dataSet1.Tables[0];
            }
            else if (!edtTenKH.Text.Equals("") && !edtMaPhong.Text.Equals("") && edtTuNgay.Text.Equals("") && edtDenNgay.Text.Equals(""))
            {
                string sqlPhong = "select khachhang.hoten as 'Tên KH', maphong as 'Phòng',ngaythue as 'Ngày thuê', ngaytra as 'Ngày trả' " +
                    "from thuephong inner join khachhang on thuephong.makh = khachhang.makh where maphong = '" + edtMaPhong.Text + "' and hoten =N'" + edtTenKH.Text + "'";
                SqlDataAdapter adapter2 = new SqlDataAdapter(sqlPhong, conn);
                DataSet dataSet1 = new DataSet();
                adapter2.Fill(dataSet1);
                gridThongTin.DataSource = dataSet1.Tables[0];
            }
            else if (!edtTenKH.Text.Equals("") && edtMaPhong.Text.Equals("") && !edtTuNgay.Text.Equals("") && edtDenNgay.Text.Equals(""))
            {
                XtraMessageBox.Show("Vui lòng chọn ngày đến", "Thông báo");
            }
            else if (!edtTenKH.Text.Equals("") && edtMaPhong.Text.Equals("") && !edtTuNgay.Text.Equals("") && !edtDenNgay.Text.Equals(""))
            {
                DateTime tungay = Convert.ToDateTime(edtTuNgay.Text);
                DateTime denngay = Convert.ToDateTime(edtDenNgay.Text);
                string sqlKH = "select khachhang.hoten as 'Tên KH', maphong as 'Phòng',ngaythue as 'Ngày thuê', ngaytra as 'Ngày trả' " +
                                    "from thuephong inner join khachhang on thuephong.makh = khachhang.makh where hoten = N'" + edtTenKH.Text + "' and ngaythue >= '" + tungay + "' and ngaytra <= '" + denngay + "'";
                SqlDataAdapter adapter1 = new SqlDataAdapter(sqlKH, conn);
                DataSet dataSet = new DataSet();
                adapter1.Fill(dataSet);
                gridThongTin.DataSource = dataSet.Tables[0];
            }
            else if (edtTenKH.Text.Equals("") && !edtMaPhong.Text.Equals("") && !edtTuNgay.Text.Equals("") && edtDenNgay.Text.Equals(""))
            {
                XtraMessageBox.Show("Vui lòng chọn ngày đến", "Thông báo");
            }
            else if (edtTenKH.Text.Equals("") && !edtMaPhong.Text.Equals("") && !edtTuNgay.Text.Equals("") && !edtDenNgay.Text.Equals(""))
            {
                DateTime tungay = Convert.ToDateTime(edtTuNgay.Text);
                DateTime denngay = Convert.ToDateTime(edtDenNgay.Text);
                string sqlPhong = "select khachhang.hoten as 'Tên KH', maphong as 'Phòng',ngaythue as 'Ngày thuê', ngaytra as 'Ngày trả' " +
                   "from thuephong inner join khachhang on thuephong.makh = khachhang.makh where maphong = '" + edtMaPhong.Text + "' and ngaythue >= '" + tungay + "' and ngaytra <= '" + denngay + "'";
                SqlDataAdapter adapter2 = new SqlDataAdapter(sqlPhong, conn);
                DataSet dataSet1 = new DataSet();
                adapter2.Fill(dataSet1);
                gridThongTin.DataSource = dataSet1.Tables[0];
            }
            else if (!edtTenKH.Text.Equals("") && !edtMaPhong.Text.Equals("") && !edtTuNgay.Text.Equals("") && edtDenNgay.Text.Equals(""))
            {
                XtraMessageBox.Show("Vui lòng chọn ngày đến", "Thông báo");
            }
            else if (!edtTenKH.Text.Equals("") && !edtMaPhong.Text.Equals("") && !edtTuNgay.Text.Equals("") && !edtDenNgay.Text.Equals(""))
            {
                DateTime tungay = Convert.ToDateTime(edtTuNgay.Text);
                DateTime denngay = Convert.ToDateTime(edtDenNgay.Text);
                string sqlPhong = "select khachhang.hoten as 'Tên KH', maphong as 'Phòng',ngaythue as 'Ngày thuê', ngaytra as 'Ngày trả' " +
                                    "from thuephong inner join khachhang on thuephong.makh = khachhang.makh where maphong = '" + edtMaPhong.Text + "' and hoten =N'" + edtTenKH.Text + "' and ngaythue >= '" + tungay + "' and ngaytra <= '" + denngay + "'";
                SqlDataAdapter adapter2 = new SqlDataAdapter(sqlPhong, conn);
                DataSet dataSet1 = new DataSet();
                adapter2.Fill(dataSet1);
                gridThongTin.DataSource = dataSet1.Tables[0];
            }
            else if (edtTenKH.Text.Equals("") && edtMaPhong.Text.Equals("") && !edtTuNgay.Text.Equals("") && !edtDenNgay.Text.Equals(""))
            {
                DateTime tungay = Convert.ToDateTime(edtTuNgay.Text);
                DateTime denngay = Convert.ToDateTime(edtDenNgay.Text);
                string sqlPhong = "select khachhang.hoten as 'Tên KH', maphong as 'Phòng',ngaythue as 'Ngày thuê', ngaytra as 'Ngày trả' " +
                   "from thuephong inner join khachhang on thuephong.makh = khachhang.makh where ngaythue >= '" + tungay + "' and ngaytra <= '" + denngay + "'";
                SqlDataAdapter adapter2 = new SqlDataAdapter(sqlPhong, conn);
                DataSet dataSet1 = new DataSet();
                adapter2.Fill(dataSet1);
                gridThongTin.DataSource = dataSet1.Tables[0];
            }
            else
            {
                XtraMessageBox.Show("Vui lòng nhập tên hoặc chọn 1 phòng", "Thông báo");
            }
        }
    }
}