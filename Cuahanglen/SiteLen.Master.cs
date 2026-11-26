using System;
using System.Web;

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

                string vaiTro = Session["VaiTro"] != null ? Session["VaiTro"].ToString() : "KhachHang";

                if (vaiTro == "Admin" || vaiTro == "NhanVien")
                {
                    lnkAdmin.Visible = true;
                    lblVaiTro.Text = vaiTro;
                    lblVaiTro.CssClass = "role-badge role-admin";
                }
                else
                {
                    lnkAdmin.Visible = false;
                    lblVaiTro.Text = "Khách";
                    lblVaiTro.CssClass = "role-badge role-customer";
                }
            }
            else
            {
                pnlChuaDangNhap.Visible = true;
                pnlDaDangNhap.Visible = false;
                lnkAdmin.Visible = false;
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