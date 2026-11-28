using System;
using System.Linq;
using System.Web.UI.WebControls;
using Cuahanglen.Models; // Thư mục chứa file .edmx của bạn

namespace Cuahanglen
{
    public partial class TrangChu : System.Web.UI.Page
    {
        // Lưu ý: Đổi tên "QLCuaHangLenEntities" đúng với tên class DbContext trong Models của bạn
        private QLCuaHangLenEntities _db = new QLCuaHangLenEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSanPham();
            }
        }

        private void LoadSanPham()
        {
            var query = _db.SanPhams.AsQueryable();

            // 1. Lọc theo danh mục trên URL (VD: ?dm=DM01)
            string maDM = Request.QueryString["dm"];
            if (!string.IsNullOrEmpty(maDM))
            {
                query = query.Where(sp => sp.MaDanhMuc.Trim() == maDM.Trim());
                var dm = _db.DanhMucs.FirstOrDefault(d => d.MaDanhMuc.Trim() == maDM.Trim());
                if (dm != null) lblTieuDe.Text = dm.TenDanhMuc;
            }

            // 2. Lọc theo từ khóa tìm kiếm trên Header (VD: ?tuKhoa=Milk)
            string tuKhoa = Request.QueryString["tuKhoa"];
            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query = query.Where(sp => sp.TenSanPham.Contains(tuKhoa));
                lblTieuDe.Text = "Kết quả tìm kiếm: \"" + tuKhoa + "\"";
            }
            // Lọc theo khoảng giá trên URL (VD: ?gia=duoi50)
            string khoangGia = Request.QueryString["gia"];
            if (khoangGia == "duoi50")
            {
                query = query.Where(sp => sp.GiaBan < 50000);
                lblTieuDe.Text = "Sản phẩm dưới 50,000₫";
            }
            else if (khoangGia == "50den200")
            {
                query = query.Where(sp => sp.GiaBan >= 50000 && sp.GiaBan <= 200000);
                lblTieuDe.Text = "Sản phẩm từ 50,000₫ đến 200,000₫";
            }
            else if (khoangGia == "tren200")
            {
                query = query.Where(sp => sp.GiaBan > 200000);
                lblTieuDe.Text = "Sản phẩm trên 200,000₫";
            }
            // 3. Sắp xếp theo giá
            if (ddlSapXep.SelectedValue == "asc")
                query = query.OrderBy(sp => sp.GiaBan);
            else if (ddlSapXep.SelectedValue == "desc")
                query = query.OrderByDescending(sp => sp.GiaBan);

            var danhSach = query.ToList();
            rptSanPham.DataSource = danhSach;
            rptSanPham.DataBind();

            lblSoLuongSP.Text = "Hiển thị " + danhSach.Count + " sản phẩm";
        }

        protected void ddlSapXep_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSanPham();
        }

        protected void btnThemGio_Click(object sender, EventArgs e)
        {
            // Kiểm tra: Chưa đăng nhập HOẶC không phải vai trò Khách hàng -> đẩy sang trang Đăng nhập
            if (Session["TenDangNhap"] == null || Session["VaiTro"] == null || Session["VaiTro"].ToString() != "KhachHang")
            {
                Response.Redirect("~/DangNhap.aspx?yeucau=khachhang");
                return;
            }

            LinkButton btn = (LinkButton)sender;
            string maSP = btn.CommandArgument.Trim();
            Response.Redirect("~/GioHang.aspx?add=" + maSP);
        }
    }
}