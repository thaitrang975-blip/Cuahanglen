using System;
using System.Linq;
using Cuahanglen.Models;

namespace Cuahanglen
{
    public partial class DangKy : System.Web.UI.Page
    {
        private QLCuaHangLenEntities _db = new QLCuaHangLenEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnDangKy_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string tenDN = txtTenDN.Text.Trim();

            // 1. Kiểm tra trùng tên đăng nhập bằng LINQ
            bool daTonTai = _db.TaiKhoans.Any(t => t.TenDangNhap.Trim() == tenDN);
            if (daTonTai)
            {
                lblThongBao.Text = "Tên đăng nhập này đã có người sử dụng, vui lòng chọn tên khác!";
                return;
            }

            // 2. Tạo mã khách hàng tự động dài đúng 10 ký tự (khớp CHAR(10))
            string maKH = "KH" + DateTime.Now.ToString("ddHHmmss");

            // 3. Lưu vào bảng KhachHang trước
            KhachHang khMoi = new KhachHang
            {
                MaKhachHang = maKH,
                HoTen = txtHoTen.Text.Trim(),
                SoDienThoai = txtSDT.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim()
            };
            _db.KhachHangs.Add(khMoi);

            // 4. Lưu vào bảng TaiKhoan với vai trò KhachHang
            TaiKhoan tkMoi = new TaiKhoan
            {
                TenDangNhap = tenDN,
                MatKhau = txtMatKhau.Text.Trim(),
                MaKhachHang = maKH,
                MaNhanVien = null,
                VaiTro = "KhachHang",
                TrangThai = true
            };
            _db.TaiKhoans.Add(tkMoi);

            _db.SaveChanges();

            // 5. Tự động đăng nhập luôn cho khách sau khi tạo tài khoản
            Session["TenDangNhap"] = tkMoi.TenDangNhap;
            Session["HoTen"] = khMoi.HoTen;
            Session["VaiTro"] = "KhachHang";
            Session["MaKH"] = maKH;

            Response.Redirect("~/TrangChu.aspx");
        }
    }
}