using System;

namespace Cuahanglen.Admin
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra quyền truy cập cho toàn bộ trang nằm trong thư mục Admin
            string vaiTro = Session["VaiTro"] != null ? Session["VaiTro"].ToString().Trim() : "";
            if (Session["TenDangNhap"] == null || (vaiTro != "Admin" && vaiTro != "NhanVien"))
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblAdminName.Text = Session["HoTen"] != null
                    ? Session["HoTen"].ToString()
                    : Session["TenDangNhap"].ToString();
                lblRole.Text = vaiTro;
            }
        }

        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/DangNhap.aspx");
        }
    }
}