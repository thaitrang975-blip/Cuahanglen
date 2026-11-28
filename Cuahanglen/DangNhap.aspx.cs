using System;
using System.Linq;
using Cuahanglen.Models;

namespace Cuahanglen
{
    public partial class DangNhap : System.Web.UI.Page
    {
        private QLCuaHangLenEntities _db = new QLCuaHangLenEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Nếu đã đăng nhập đúng Khách hàng rồi thì không cần vào lại trang đăng nhập
                if (Session["TenDangNhap"] != null && Session["VaiTro"] != null && Session["VaiTro"].ToString() == "KhachHang")
                {
                    Response.Redirect("~/TrangChu.aspx");
                    return;
                }

                // Hiện thông báo nếu bị chặn từ nút Thêm giỏ hàng / Giỏ hàng
                if (Request.QueryString["yeucau"] == "khachhang")
                {
                    ddlVaiTro.SelectedValue = "KhachHang";
                    lblThongBao.Text = "Vui lòng đăng nhập bằng tài khoản Khách hàng (hoặc đăng ký mới) để thêm giỏ hàng và đặt mua!";
                }
            }
        }

        protected void btnDangNhap_Click(object sender, EventArgs e)
        {
            string vaiTroChon = ddlVaiTro.SelectedValue; // "KhachHang" hoặc "Admin"
            string user = txtTenDN.Text.Trim();
            string pass = txtMatKhau.Text.Trim();

            // 1. Tìm tài khoản trong CSDL bằng LINQ
            var tk = _db.TaiKhoans.FirstOrDefault(t =>
                t.TenDangNhap.Trim() == user &&
                t.MatKhau.Trim() == pass &&
                t.TrangThai == true);

            if (tk == null)
            {
                lblThongBao.Text = "Sai tên đăng nhập, mật khẩu hoặc tài khoản đã bị khóa!";
                return;
            }

            // 2. Kiểm tra vai trò chọn có khớp với quyền thật trong SQL Server không
            string vaiTroThat = tk.VaiTro.Trim();
            bool hopLe = (vaiTroChon == "KhachHang" && vaiTroThat == "KhachHang")
                      || (vaiTroChon == "Admin" && (vaiTroThat == "Admin" || vaiTroThat == "NhanVien"));

            if (!hopLe)
            {
                lblThongBao.Text = "Tài khoản này không thuộc vai trò bạn vừa chọn!";
                return;
            }

            // 3. Lưu Session để hiển thị và phân quyền trên SiteLen.Master
            Session["TenDangNhap"] = tk.TenDangNhap.Trim();
            Session["VaiTro"] = vaiTroThat;

            if (!string.IsNullOrEmpty(tk.MaNhanVien))
            {
                var nv = _db.NhanViens.FirstOrDefault(x => x.MaNhanVien == tk.MaNhanVien);
                Session["HoTen"] = nv != null ? nv.HoTen : tk.TenDangNhap.Trim();
                Session["MaNV"] = tk.MaNhanVien.Trim();
            }
            else if (!string.IsNullOrEmpty(tk.MaKhachHang))
            {
                var kh = _db.KhachHangs.FirstOrDefault(x => x.MaKhachHang == tk.MaKhachHang);
                Session["HoTen"] = kh != null ? kh.HoTen : tk.TenDangNhap.Trim();
                Session["MaKH"] = tk.MaKhachHang.Trim();
            }

            // Điều hướng tách biệt theo vai trò
            if (vaiTroThat == "Admin" || vaiTroThat == "NhanVien")
            {
                Response.Redirect("~/Admin/QuanLySanPham.aspx");
            }
            else
            {
                Response.Redirect("~/TrangChu.aspx");
            }
        }
    }
}