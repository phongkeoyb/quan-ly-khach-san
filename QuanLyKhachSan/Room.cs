using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKhachSan
{
    class Room
    {
        private string maphong;
        private string maloai;
        private string trangthai;
        private string mota;
        private string hinhanh;
        private string tenloai;
        private string giaphong;
        private string sosao;

        public Room() { }
        public Room(string _maphong, string _maloai, string _trangthai, string _mota, string _hinhanh, string _giaphong, string _tenloai, string _sosao)
        {
            this.Maphong = _maphong;
            this.Maloai = _maloai;
            this.Trangthai = _trangthai;
            this.Mota = _mota;
            this.Hinhanh = _hinhanh;
            this.Giaphong = _giaphong;
            this.Tenloai = _tenloai;
            this.Sosao = _sosao;
        }

        public Room(DataRow row)
        {
            this.Maphong = row["MaPhong"].ToString();
            this.Maloai = row["MaLoai"].ToString();
            this.Trangthai = row["TrangThai"].ToString();
            this.Mota = row["MoTa"].ToString();
            this.Hinhanh = row["HinhAnh"].ToString();
            //this.Giaphong = row["GiaPhong"].ToString();
            //this.Tenloai = row["TenLoai"].ToString();
            //this.Sosao = row["SoSao"].ToString();
        }
        public string Maphong { get => maphong; set => maphong = value; }
        public string Maloai { get => maloai; set => maloai = value; }
        public string Trangthai { get => trangthai; set => trangthai = value; }
        public string Mota { get => mota; set => mota = value; }
        public string Hinhanh { get => hinhanh; set => hinhanh = value; }
        public string Tenloai { get => tenloai; set => tenloai = value; }
        public string Giaphong { get => giaphong; set => giaphong = value; }
        public string Sosao { get => sosao; set => sosao = value; }
    }
}
