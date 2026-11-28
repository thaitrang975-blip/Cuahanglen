using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using Cuahanglen.Models;

namespace Cuahanglen
{
    public partial class SiteLen : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CapNhatTrangThaiDangNhap();
            }
            // Cập nhật số lượng hiển thị trên badge Giỏ hàng
            var gio = Session["GioHang"] as List<CartItem>;
            lblSoLuongGio.Text = gio != null ? gio.Sum(x => x.SoLuong).ToString() : "0";
        }

        private void CapNhatTrangThaiDangNhap()
        {
            if (Session["TenDangNhap"] != null)
            {
                pnlChuaDangNhap.Visible = false;
                pnlDaDangNhap.Visible = true;
                lblTenNguoiDung.Text = Session["HoTen"] != null
                    ? Session["HoTen"].ToString()
                    : Session["TenDangNhap"].ToString();

                string vaiTro = Session["VaiTro"] != null ? Session["VaiTro"].ToString().Trim() : "KhachHang";

                if (vaiTro == "Admin" || vaiTro == "NhanVien")
                {
                    lnkAdmin.Visible = true;
                    hlDonHangCuaToi.Visible = false; // Ẩn với Admin / Nhân viên
                    lblVaiTro.Text = vaiTro;
                    lblVaiTro.CssClass = "role-badge role-admin";
                }
                else
                {
                    lnkAdmin.Visible = false;
                    hlDonHangCuaToi.Visible = true;  // Hiện với Khách hàng
                    lblVaiTro.Text = "Khách";
                    lblVaiTro.CssClass = "role-badge role-customer";
                }
            }
            else
            {
                pnlChuaDangNhap.Visible = true;
                pnlDaDangNhap.Visible = false;
                lnkAdmin.Visible = false;
                hlDonHangCuaToi.Visible = false;     // Ẩn khi chưa đăng nhập
            }
        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();
            Response.Redirect("~/TrangChu.aspx?tuKhoa=" + Server.UrlEncode(tuKhoa));
        }

        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/TrangChu.aspx");
        }
    }
}