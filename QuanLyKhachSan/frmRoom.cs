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
    public partial class frmRoom : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection conn;

        public frmRoom()
        {
            InitializeComponent();
        }

        private void frmRoom_Load(object sender, EventArgs e)
        {
            conn = ConnectionDatabase.getInstance();
            ConnectionDatabase.openConnectionStage();
            string sql = "select tenloai from loaiphong";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                cbLoaiPhong.Properties.Items.Add(reader.GetString(0));
            }
            reader.Close();
            loadData();
            lblHinhAnh.Enabled = false;
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
            string sql = "SELECT PHONG.MAPHONG as 'Mã phòng',LOAIPHONG.TENLOAI as 'Tên loại phòng', LOAIPHONG.SOSAO as 'Số sao', LOAIPHONG.GIAPHONG as 'Giá phòng', PHONG.TrangThai as 'Trạng thái', PHONG.MOTA as 'Mô tả'" +
                ",HinhAnh as 'Hình Ảnh' FROM phong inner join loaiphong on phong.maloai=loaiphong.maloai ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            gridRoom.DataSource = dataSet.Tables[0];
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string maphong = edtMaPhong.Text;
            string maloai = cbLoaiPhong.Text;
            string trangthai = edtStatus.Text;
            string mota = edtDescribe.Text;
            string hinhanh = lblHinhAnh.Text;
            if (maphong == "" || maloai == "" || trangthai == "" || mota == "" || hinhanh == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else
            {
                string sqlTrungKhoa = "SELECT * from PHONG WHERE MAPHONG = '" + maphong + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (reader.Read())//Nếu đọc được 
                {
                    XtraMessageBox.Show("Đã tồn tại mã phòng này", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    reader.Close();//Chữ N trước '' thêm được tiếng việt vào sql
                    string sqlInsert = "INSERT INTO PHONG VALUES(N'" + maphong + "',N'" + maloai + "',N'" + trangthai + "',N'" + mota + "',N'" + hinhanh + "')";
                    SqlCommand commandUpdate = new SqlCommand(sqlInsert, conn);
                    commandUpdate.ExecuteNonQuery();
                    XtraMessageBox.Show("Thêm thành công mã phòng: " + maphong, "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string maloai = "";
            string maphong = edtMaPhong.Text;
            string sqlMaLoai = "select maloai from loaiphong where TenLoai like N'" + cbLoaiPhong.Text + "'";
            SqlCommand commandMaLoai = new SqlCommand(sqlMaLoai, conn);
            SqlDataReader readerMaLoai = commandMaLoai.ExecuteReader();
            while (readerMaLoai.Read())
            {
                maloai = readerMaLoai.GetString(0);
            }
            readerMaLoai.Close();
            string trangthai = edtStatus.Text;
            string mota = edtDescribe.Text;
            string hinhanh = lblHinhAnh.Text;
            if (maphong == "" || maloai == "" || trangthai == "" || mota == "")
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlTrungKhoa = "SELECT * from PHONG WHERE MAPHONG = '" + maphong + "'";
                SqlCommand commandTrungKhoa = new SqlCommand(sqlTrungKhoa, conn);
                SqlDataReader reader = commandTrungKhoa.ExecuteReader();
                if (!reader.Read())
                {
                    XtraMessageBox.Show("Không tồn tại mã phòng này để cập nhật", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    reader.Close();
                    string sqlUpdate = "UPDATE PHONG Set MaPhong=N'" + maphong + "',MaLoai=N'" + maloai + "',TrangThai=N'" + trangthai + "',MoTa=N'" + mota + "',HinhAnh=N'" + hinhanh + "' WHERE MaPhong=N'" + maphong + "'";
                    SqlCommand commandUpdate = new SqlCommand(sqlUpdate, conn);
                    commandUpdate.ExecuteNonQuery();
                    XtraMessageBox.Show("Cập nhật thành công mã phòng: " + maphong, "Thông báo", MessageBoxButtons.OK);
                    loadData();
                }
            }
        }



        private void lkedLoaiPhong_TextChanged(object sender, EventArgs e)
        {
            //string str;
            //if (lkedLoaiPhong.Text == "")
            //{
            //    edtPrice.Text = "";
            //    edtStar.Text = "";
            //}
            ////Khi chọn Mã khách hàng thì các thông tin của khách hàng sẽ hiện ra
            //str = "Select SoSao from LOAIPHONG where MALOAI = N'" + lkedLoaiPhong.EditValue + "'";
            //edtStar.Text = ConnectionDatabase.GetFieldValues(str);
            //str = "Select GiaPhong from LOAIPHONG where MALOAI = N'" + lkedLoaiPhong.EditValue + "'";
            //edtPrice.Text = ConnectionDatabase.GetFieldValues(str);
        }


        private void btnReLoad_Click(object sender, EventArgs e)
        {
            edtMaPhong.ResetText();
            cbLoaiPhong.ResetText();
            edtPrice.ResetText();
            edtStar.ResetText();
            edtStatus.ResetText();
            edtDescribe.ResetText();
            lblHinhAnh.ResetText();
            pictureEdit1.Reset();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            string maphong = gvPhong.GetRowCellValue(gvPhong.FocusedRowHandle, "Mã phòng").ToString();
            if (maphong != null)
            {
                if (XtraMessageBox.Show("Bạn có chắc muốn xóa phòng này không", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string sqlDelete = "DELETE FROM PHONG where MaPhong = N'" + maphong + "'";
                    SqlCommand commandDelete = new SqlCommand(sqlDelete, conn);
                    commandDelete.ExecuteNonQuery();
                    XtraMessageBox.Show("Phòng có mã: " + maphong + " đã được xóa", "Thông báo", MessageBoxButtons.OK);
                    //gvKhachHang.DeleteRow(gvKhachHang.FocusedRowHandle);
                    loadData();
                }
            }
        }

        private void gvPhong_FocusedRowChanged_1(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
            {

            }
            else
            {
                edtMaPhong.Text = gvPhong.GetRowCellValue(e.FocusedRowHandle, "Mã phòng").ToString();
                cbLoaiPhong.EditValue = gvPhong.GetRowCellValue(e.FocusedRowHandle, "Tên loại phòng").ToString();
                edtStar.Text = gvPhong.GetRowCellValue(e.FocusedRowHandle, "Số sao").ToString();
                edtPrice.Text = gvPhong.GetRowCellValue(e.FocusedRowHandle, "Giá phòng").ToString();
                edtStatus.Text = gvPhong.GetRowCellValue(e.FocusedRowHandle, "Trạng thái").ToString();
                edtDescribe.Text = gvPhong.GetRowCellValue(e.FocusedRowHandle, "Mô tả").ToString();
                string nameImage = gvPhong.GetRowCellValue(e.FocusedRowHandle, "Hình Ảnh").ToString();
                string currentDebug = Environment.CurrentDirectory + "\\";
                pictureEdit1.Image = Image.FromFile(currentDebug + nameImage);
            }
        }

        private void gvPhong_ShowingEditor_1(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "Bitmap(*.bmp)|*.bmp|JPEG(*.jpg)|*.jpg|GIF(*.gif)|*.gif|All files(*.*)|*.*";
            dlgOpen.FilterIndex = 2;
            dlgOpen.Title = "Chọn ảnh minh hoạ cho phòng";
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                pictureEdit1.Image = Image.FromFile(dlgOpen.FileName);
                lblHinhAnh.Text = dlgOpen.FileName;
            }
        }

        private void cbLoaiPhong_SelectedValueChanged(object sender, EventArgs e)
        {
            string tenloai = cbLoaiPhong.Text;
            string sql = "select giaphong, sosao from loaiphong where TenLoai like N'" + tenloai + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                edtPrice.Text = reader.GetString(0);
                edtStar.Text = reader.GetString(1);
            }
            reader.Close();
        }
    }
}