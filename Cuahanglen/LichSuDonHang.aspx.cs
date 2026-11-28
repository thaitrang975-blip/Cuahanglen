using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Cuahanglen.Models;

namespace Cuahanglen
{
    public partial class LichSuDonHang : System.Web.UI.Page
    {
        private QLCuaHangLenEntities _db = new QLCuaHangLenEntities();
        private const string ANH_MAC_DINH = "lenmilkcotton50g.jpg";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Chưa đăng nhập thì chuyển về trang Đăng nhập
            if (Session["TenDangNhap"] == null)
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadDonHangCuaToi();
            }
        }

        // Lấy MaKhachHang của người đang đăng nhập (ưu tiên Session, nếu thiếu tự tra từ TaiKhoan)
        private string LayMaKhachHangHienTai()
        {
            if (Session["MaKH"] != null && !string.IsNullOrWhiteSpace(Session["MaKH"].ToString()))
            {
                return Session["MaKH"].ToString().Trim();
            }

            if (Session["TenDangNhap"] != null)
            {
                string tenDN = Session["TenDangNhap"].ToString().Trim();
                var tk = _db.TaiKhoans.FirstOrDefault(t => t.TenDangNhap.Trim() == tenDN);
                if (tk != null && !string.IsNullOrEmpty(tk.MaKhachHang))
                {
                    Session["MaKH"] = tk.MaKhachHang.Trim();
                    return tk.MaKhachHang.Trim();
                }
            }

            return "";
        }

        private void LoadDonHangCuaToi()
        {
            string maKH = LayMaKhachHangHienTai();
            if (string.IsNullOrEmpty(maKH))
            {
                gvDonHangCuaToi.DataSource = null;
                gvDonHangCuaToi.DataBind();
                return;
            }

            string trangThaiLoc = ddlTrangThai.SelectedValue;

            var query = _db.HoaDons
                           .Where(h => h.MaKhachHang.Trim() == maKH);

            if (!string.IsNullOrEmpty(trangThaiLoc))
            {
                query = query.Where(h => h.TrangThai == trangThaiLoc);
            }

            var ds = query.OrderByDescending(h => h.NgayLap)
                          .ToList()
                          .Select(h => new
                          {
                              MaHoaDon = h.MaHoaDon.Trim(),
                              h.NgayLap,
                              h.TongTien,
                              h.PhuongThucThanhToan,
                              h.TrangThai
                          }).ToList();

            gvDonHangCuaToi.DataSource = ds;
            gvDonHangCuaToi.DataBind();
        }

        protected void ddlTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlChiTiet.Visible = false;
            lblThongBao.Text = "";
            LoadDonHangCuaToi();
        }

        protected void gvDonHangCuaToi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string maHD = e.CommandArgument.ToString().Trim();
            string maKH = LayMaKhachHangHienTai();

            // Kiểm tra đơn hàng thuộc đúng khách đang đăng nhập
            var hd = _db.HoaDons.FirstOrDefault(h =>
                h.MaHoaDon.Trim() == maHD &&
                h.MaKhachHang.Trim() == maKH);

            if (hd == null) return;

            if (e.CommandName == "XemCT")
            {
                lblMaHD.Text = hd.MaHoaDon.Trim();
                lblNgayLap.Text = string.Format("{0:dd/MM/yyyy HH:mm}", hd.NgayLap);
                lblThanhToan.Text = hd.PhuongThucThanhToan;
                lblTrangThaiCT.Text = hd.TrangThai;
                lblTongTienCT.Text = string.Format("{0:N0}₫", hd.TongTien);

                var chiTiet = (from ct in _db.ChiTietHoaDons
                               join sp in _db.SanPhams on ct.MaSanPham equals sp.MaSanPham
                               where ct.MaHoaDon.Trim() == maHD
                               select new
                               {
                                   sp.HinhAnh,
                                   sp.TenSanPham,
                                   ct.Size,
                                   ct.SoLuong,
                                   ct.DonGia,
                                   ThanhTien = ct.SoLuong * ct.DonGia
                               }).ToList();

                gvChiTietDon.DataSource = chiTiet;
                gvChiTietDon.DataBind();

                pnlChiTiet.Visible = true;
                lblThongBao.Text = "";
            }
            else if (e.CommandName == "HuyDon")
            {
                // Chỉ cho hủy khi đơn vẫn còn ở trạng thái "Đang xác nhận"
                if (hd.TrangThai != "Đang xác nhận")
                {
                    lblThongBao.Text = "Đơn hàng này đã được xử lý, không thể hủy!";
                    lblThongBao.ForeColor = System.Drawing.Color.Red;
                    LoadDonHangCuaToi();
                    return;
                }

                // Hoàn lại số lượng tồn kho cho từng sản phẩm trong đơn
                var dsCT = _db.ChiTietHoaDons.Where(c => c.MaHoaDon.Trim() == maHD).ToList();
                foreach (var ct in dsCT)
                {
                    var kho = _db.ChiTietKhoes.FirstOrDefault(k => k.MaSanPham == ct.MaSanPham);
                    if (kho != null) kho.SoLuongTon += ct.SoLuong;
                }

                hd.TrangThai = "Đã hủy";
                _db.SaveChanges();

                lblThongBao.Text = "✔ Đã hủy đơn hàng #" + maHD + " thành công!";
                lblThongBao.ForeColor = System.Drawing.Color.Green;
                pnlChiTiet.Visible = false;
                LoadDonHangCuaToi();
            }
        }

        protected void btnDongChiTiet_Click(object sender, EventArgs e)
        {
            pnlChiTiet.Visible = false;
        }

        public string LayDuongDanAnh(object hinhAnh)
        {
            string tenFile = hinhAnh != null ? hinhAnh.ToString().Trim() : "";
            if (!string.IsNullOrEmpty(tenFile))
            {
                string duongDanVatLy = Server.MapPath("~/Images/" + tenFile);
                if (File.Exists(duongDanVatLy))
                {
                    return ResolveUrl("~/Images/" + tenFile);
                }
            }
            return ResolveUrl("~/Images/" + ANH_MAC_DINH);
        }

        public string LayMauTrangThai(string trangThai)
        {
            string baseStyle = "padding:4px 10px; border-radius:14px; font-size:12px; font-weight:600; display:inline-block; white-space:nowrap;";
            switch (trangThai)
            {
                case "Đang xác nhận":
                    return baseStyle + "background:#fef3c7; color:#92400e;";
                case "Đang giao hàng":
                    return baseStyle + "background:#dbeafe; color:#1e40af;";
                case "Đã hoàn thành":
                    return baseStyle + "background:#d1fae5; color:#065f46;";
                case "Đã hủy":
                    return baseStyle + "background:#fee2e2; color:#991b1b;";
                default:
                    return baseStyle + "background:#f3f4f6; color:#374151;";
            }
        }
    }
}