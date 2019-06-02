using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKhachSan
{
    class Common
    {
        public static string laychuoitudong(string chuoi)
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
