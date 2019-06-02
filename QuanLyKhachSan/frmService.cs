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
    public partial class frmService : DevExpress.XtraEditors.XtraForm
    {
        private string mMathuephong = "";
        SqlConnection conn;
        public frmService()
        {
            InitializeComponent();
        }

        public frmService(string mathuephong)
        {
            mMathuephong = mathuephong;
            InitializeComponent();
        }

        private void frmService_Load(object sender, EventArgs e)
        {
            string masddv = "";
            conn = ConnectionDatabase.getInstance();
            ConnectionDatabase.openConnectionStage();

            //Đưa data vào dich vụ
            string sql = "SELECT MADV, TENDV, GIA FROM DICHVU";
            ConnectionDatabase.FillCombo(sql, lkedMaDV, "MADV", "TENDV");

            //Lấy mathuephong can su dung dv
            string sqlMaTP = "select mathuephong from thuephong inner join phong on phong.maphong = thuephong.maphong " +
                "where trangthai='1'";
            SqlCommand commandMaTP = new SqlCommand(sqlMaTP, conn);
            SqlDataReader readerMaTP = commandMaTP.ExecuteReader();
            while (readerMaTP.Read())
            {
                edtMaTP.Properties.Items.Add(readerMaTP.GetString(0));
            }
            readerMaTP.Close();

            loadData();
            if (!mMathuephong.Equals(""))
            {
                string sqlMaSDDV = "Select top 1 maSDDV from sudungdv order by masddv desc";
                SqlCommand command = new SqlCommand(sqlMaSDDV, conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    masddv = reader.GetString(0);
                }
                reader.Close();
                if (masddv.Equals(""))
                {
                    edtMaSD.Text = "SD001";
                }
                else
                {
                    edtMaSD.Text = laychuoitudong(masddv);
                }
                edtMaTP.Text = mMathuephong;
                edtSoLuong.Text = "";
                edtMaDV.Text = "";
                dtpNgaySD.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
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

        private void loadData()
        {
            //Load dữ liệu có thể sài nhiều lần sau khi xóa sửa thêm, đều có thể gọi lại để kiểm tra
            string sql = "SELECT SUDUNGDV.MASDDV as 'Mã SDDV', DICHVU.MADV as 'Mã DV',ThuePhong.MaThuePhong as 'Mã TP', DICHVU.TENDV as 'Tên dịch vụ'," +
                " DICHVU.GIA as 'Giá', SUDUNGDV.SOLUONG as 'Số lượng', SUDUNGDV.NGAYSD as 'Ngày sử dụng'  " +
                "FROM dichvu inner join sudungdv on dichvu.madv=sudungdv.madv inner join thuephong on thuephong.mathuephong = sudungdv.mathuephong ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            gridService.DataSource = dataSet.Tables[0];
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string masd = edtMaSD.Text;
            string madv = lkedMaDV.Text;
            string soluong = edtSoLuong.Text;
            string ngay = dtpNgaySD.Text;
            string mathuephong = edtMaTP.Text;
            if (masd == "" || madv == "" || soluong == "" || ngay == "" || mathuephong == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else
            {
                string sqlTrungKhoa = "SELECT * from SUDUNGDV WHERE MaSDDV = '" + masd + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (reader.Read())//Nếu đọc được 
                {
                    reader.Close();
                    XtraMessageBox.Show("Đã tồn tại mã sddv này", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    reader.Close();//Chữ N trước '' thêm được tiếng việt vào sql
                    string sqlInsert = "INSERT INTO SUDUNGDV VALUES('" + masd + "','" + madv + "','" + mathuephong + "','" + soluong + "','" + ngay + "')";
                    SqlCommand commandUpdate = new SqlCommand(sqlInsert, conn);
                    commandUpdate.ExecuteNonQuery();
                    XtraMessageBox.Show("Thêm thành công mã sử dụng dv: " + masd, "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string masd = edtMaSD.Text;
            string madv = lkedMaDV.Text;
            string soluong = edtSoLuong.Text;
            string ngay = dtpNgaySD.Text;
            string mathuephong = edtMaTP.Text;
            if (masd == "" || madv == "" || soluong == "" || ngay == "" || mathuephong == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else
            {
                string sqlTrungKhoa = "SELECT * from SUDUNGDV WHERE MaSDDV = '" + masd + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (!reader.Read())//Nếu không đọc được
                {
                    reader.Close();
                    XtraMessageBox.Show("Bản ghi bạn muốn sửa không tồn tại", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    reader.Close();
                    string sqlUpdate = "UPDATE SUDUNGDV Set MASDDV=N'" + masd + "',MaDV=N'" + madv + "',MaThuePhong=N'" + mathuephong + "',SoLuong=N'" + soluong + "',NgaySD=N'" + ngay + "' WHERE MaSDDV=N'" + masd + "'";
                    SqlCommand commandUpdate = new SqlCommand(sqlUpdate, conn);
                    commandUpdate.ExecuteNonQuery();
                    XtraMessageBox.Show("Cập nhật thành công mã sử dụng: " + masd, "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }
        }

        private void lkedMaDV_TextChanged(object sender, EventArgs e)
        {
            string str;
            if (lkedMaDV.Text == "")
            {
                edtTenDV.Text = "";
                edtGia.Text = "";
            }
            //Khi chọn Mã khách hàng thì các thông tin của khách hàng sẽ hiện ra
            str = "Select TENDV from DICHVU where MADV = N'" + lkedMaDV.EditValue + "'";
            edtTenDV.Text = ConnectionDatabase.GetFieldValues(str);
            str = "Select Gia from DICHVU where MADV = N'" + lkedMaDV.EditValue + "'";
            edtGia.Text = ConnectionDatabase.GetFieldValues(str);
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            string masddv = "";
            string sqlMaSDDV = "Select top 1 maSDDV from sudungdv order by masddv desc";
            SqlCommand command = new SqlCommand(sqlMaSDDV, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                masddv = reader.GetString(0);
            }
            reader.Close();
            if (masddv.Equals(""))
            {
                edtMaSD.Text = "SD001";
            }
            else
            {
                edtMaSD.Text = laychuoitudong(masddv);
            }
            edtSoLuong.Text = "";
            edtMaDV.Text = "";
            dtpNgaySD.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            string masd = gvDichVu.GetRowCellValue(gvDichVu.FocusedRowHandle, "Mã SDDV").ToString();
            if (masd != null)
            {
                if (XtraMessageBox.Show("Bạn có chắc muốn xóa phòng này không", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string sqlDelete = "DELETE FROM SUDUNGDV where MaSDDV = N'" + masd + "'";
                    SqlCommand commandDelete = new SqlCommand(sqlDelete, conn);
                    commandDelete.ExecuteNonQuery();
                    XtraMessageBox.Show("Sử dụng DV có mã: " + masd + " đã được xóa", "Thông báo", MessageBoxButtons.OK);
                    //gvKhachHang.DeleteRow(gvKhachHang.FocusedRowHandle);
                    loadData();
                }
            }
        }

        private void gvDichVu_ShowingEditor_1(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void gvDichVu_FocusedRowChanged_1(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //string maloai;
            //Sự kiện này lắng nghe dòng đang focuss để lấy dữ liệu lên mấy ô textbox
            if (e.FocusedRowHandle < 0)
            {

            }
            else
            {
                edtMaSD.Text = gvDichVu.GetRowCellValue(e.FocusedRowHandle, "Mã SDDV").ToString();
                lkedMaDV.EditValue = gvDichVu.GetRowCellValue(e.FocusedRowHandle, "Mã DV").ToString();
                edtTenDV.Text = gvDichVu.GetRowCellValue(e.FocusedRowHandle, "Tên dịch vụ").ToString();
                edtGia.Text = gvDichVu.GetRowCellValue(e.FocusedRowHandle, "Giá").ToString();
                edtSoLuong.Text = gvDichVu.GetRowCellValue(e.FocusedRowHandle, "Số lượng").ToString();
                dtpNgaySD.Text = gvDichVu.GetRowCellValue(e.FocusedRowHandle, "Ngày sử dụng").ToString();
                edtMaTP.Text = gvDichVu.GetRowCellValue(e.FocusedRowHandle, "Mã TP").ToString();

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
        
    }
}