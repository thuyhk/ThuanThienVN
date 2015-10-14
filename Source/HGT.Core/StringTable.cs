using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Core
{
    public static partial class StringTable
    {
        public static string Home
        {
            get { return "Trang chủ"; }
        }

        public static string AddNew
        {
            get { return "Thêm mới"; }
        }

        public static string Save
        {
            get { return "Lưu"; }
        }

        public static string Back
        {
            get { return "Quay lại"; }
        }

        public static string Cancel
        {
            get { return "Hủy"; }
        }

        public static string Reset
        {
            get { return "Làm lại"; }
        }

        public static string Update
        {
            get { return "Cập nhật"; }
        }

        public static string DataSaveSuccess
        {
            get { return "Lưu thành công!"; }
        }

        public static string DataSaveUnsuccess
        {
            get { return "Lưu không thành công!"; }
        }

        public static string DataDeletedSuccess
        {
            get { return "Xóa dữ liệu thành công!"; }
        }

        public static string DataDeletedUnsuccess
        {
            get { return "Xóa dữ liệu không thành công!"; }
        }

        public static string CaptchaInCorrect
        {
            get { return "Mã xác thực không đúng. Vui lòng thử lại!"; }
        }

        public static string NotFoundItem {
            get { return "Không tìm thấy dữ liệu!"; }
        }

        public static string Require
        {
            get { return "Vui lòng nhập dữ liệu!"; }
        }
    }
}
