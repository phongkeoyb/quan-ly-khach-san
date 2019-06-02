using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace QuanLyKhachSan
{
    public partial class frmHome : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frmHome()
        {
            InitializeComponent();
        }

        string quyenTK = "";
        public frmHome(string quyenTK1)
        {
            quyenTK = quyenTK1;
            InitializeComponent();
        }

        private Form KiemTraTonTai(Type fType)
        {
            foreach (Form f in MdiChildren)
            {
                if (f.GetType() == fType)
                {
                    return f;
                }
            }
            return null;
        }
        
        
        private void frmHome_Load(object sender, EventArgs e)
        {
            if (!quyenTK.Equals("admin"))
            {
                btnNhanVien.Enabled = false;
                btnTaiKhoan.Enabled = false;
            }
        }
        
        
        private void btnDangXuat_ItemClick(object sender, ItemClickEventArgs e)
        {
            Hide();
            frmLogin frm = new frmLogin();
            frm.Show();
        }

        private void btnTimKiem_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = KiemTraTonTai(typeof(frmSearch));
            if (frm != null)
            {
                frm.Activate();
            }
            else
            {

                frmSearch f = new frmSearch();
                IsMdiContainer = true;
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnThongKe_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = KiemTraTonTai(typeof(frmStatistic));
            if (frm != null)
            {
                frm.Activate();
            }
            else
            {
                frmStatistic f = new frmStatistic();
                IsMdiContainer = true;
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnTraPhong_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmCheckOut f = new frmCheckOut();
            IsMdiContainer = true;
            f.MdiParent = this;
            f.Show();
        }

        private void btnThuePhong_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmCheckIn f = new frmCheckIn();
            IsMdiContainer = true;
            f.MdiParent = this;
            f.Show();
        }

        private void btnTaiKhoan_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = KiemTraTonTai(typeof(frmAccounts));
            if (frm != null)
            {
                frm.Activate();
            }
            else
            {

                frmAccounts f = new frmAccounts();
                IsMdiContainer = true;
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnNhanVien_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = KiemTraTonTai(typeof(frmEmployees));
            if (frm != null)
            {
                frm.Activate();
            }
            else
            {

                frmEmployees f = new frmEmployees();
                IsMdiContainer = true;
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnLoaiPhong_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = KiemTraTonTai(typeof(frmRoomType));
            if (frm != null)
            {
                frm.Activate();
            }
            else
            {

                frmRoomType f = new frmRoomType();
                IsMdiContainer = true;
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnDichVuKS_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = KiemTraTonTai(typeof(frmServiceType));
            if (frm != null)
            {
                frm.Activate();
            }
            else
            {

                frmServiceType f = new frmServiceType();
                IsMdiContainer = true;
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnDatPhong_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = KiemTraTonTai(typeof(frmBook));
            if (frm != null)
            {
                frm.Activate();
            }
            else
            {

                frmBook f = new frmBook();
                IsMdiContainer = true;
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnSuDungDichVu_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = KiemTraTonTai(typeof(frmService));
            if (frm != null)
            {
                frm.Activate();
            }
            else
            {

                frmService f = new frmService();
                IsMdiContainer = true;
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnKhachHang_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = KiemTraTonTai(typeof(frmCustomer));
            if (frm != null)
            {
                frm.Activate();
            }
            else
            {
                frmCustomer f = new frmCustomer();
                IsMdiContainer = true;
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnThongTinPhong_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = KiemTraTonTai(typeof(frmRoom));
            if (frm != null)
            {
                frm.Activate();
            }
            else
            {
                frmRoom f = new frmRoom();
                IsMdiContainer = true;
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPhong_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = KiemTraTonTai(typeof(frmListRoom));
            if (frm != null)
            {
                frm.Activate();
            }
            else
            {
                frmListRoom f = new frmListRoom();
                IsMdiContainer = true;
                f.MdiParent = this;
                f.Show();
            }
        }
    }
}