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
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan
{
    public partial class frmListRoom : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection conn;
        public frmListRoom()
        {
            InitializeComponent();
        }

        void LoadRoom()
        {
            List<Room> LoadRoom()
            {
                List<Room> listRoom = new List<Room>();

                conn = ConnectionDatabase.getInstance();
                ConnectionDatabase.openConnectionStage();

                string sql1 = "SELECT * from Phong ";
                SqlDataAdapter adapter = new SqlDataAdapter(sql1, conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow item in dataTable.Rows)
                {
                    Room Room = new Room(item);
                    listRoom.Add(Room);

                }
                ConnectionDatabase.closeConnectionStage();
                return listRoom;


            }

            List<Room> dsRoom = LoadRoom();
            foreach (Room item in dsRoom)
            {
                string TT;
                Button btn = new Button() { Width = 130, Height = 130 };
                PanelRoom.Controls.Add(btn);
                if (item.Trangthai == "0")
                {
                    TT = "Trống";
                    btn.BackColor = Color.Tomato;
                    btn.Click += Button_Click1;
                    btn.ImageAlign = ContentAlignment.MiddleCenter;
                    btn.Image = Image.FromFile(@"C:\Phong\QuanLyKhachSan\QuanLyKhachSan\Resources\home.png");

                }
                else
                {
                    TT = "Đã đặt";
                    btn.BackColor = Color.Pink;
                    btn.Click += Button_Click2;
                    btn.Image = Image.FromFile(@"C:\Phong\QuanLyKhachSan\QuanLyKhachSan\Resources\home1.png");
                }
                btn.Text = item.Maphong + Environment.NewLine + TT;
            }
        }
        private void Button_Click1(object sender, EventArgs e)
        {
            contextMenuStrip2.Show(this, this.PointToClient(MousePosition));
        }

        private void Button_Click2(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(this, this.PointToClient(MousePosition));
        }

        private void loadRoom_Load(object sender, EventArgs e)
        {
            LoadRoom();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmRoom frmRoom = new frmRoom();
            frmRoom.ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            frmCheckIn frmCheckIn = new frmCheckIn();
            frmCheckIn.ShowDialog();
        }

        private void đặtPhòngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBook frmBook = new frmBook();
            frmBook.ShowDialog();
        }

        private void dịchVụToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmService frmService = new frmService();
            frmService.ShowDialog();
        }

        private void thôngTinPhòngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRoom frmRoom = new frmRoom();
            frmRoom.ShowDialog();
        }

        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCheckOut frmCheckOut = new frmCheckOut();
            frmCheckOut.ShowDialog();
        }
    }
}