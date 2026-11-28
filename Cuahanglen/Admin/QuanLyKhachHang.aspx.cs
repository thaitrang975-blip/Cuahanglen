using System;
using System.Linq;
using System.Web.UI.WebControls;
using Cuahanglen.Models;

namespace Cuahanglen.Admin
{
    public partial class QuanLyKhachHang : System.Web.UI.Page
    {
        private QLCuaHangLenEntities _db = new QLCuaHangLenEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadThongKe();
                LoadDanhSachKhachHang();
            }
        }

        private void LoadThongKe()
        {
            lblTongKH.Text = _db.KhachHangs.Count().ToString();
            lblDangHoatDong.Text = _db.TaiKhoans.Count(t => t.VaiTro.Trim() == "KhachHang" && t.TrangThai == true).ToString();
            lblBiKhoa.Text = _db.TaiKhoans.Count(t => t.VaiTro.Trim() == "KhachHang" && t.TrangThai == false).ToString();
        }

        private void LoadDanhSachKhachHang()
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();
            string locTT = ddlLocTrangThai.SelectedValue;

            var ds = (from kh in _db.KhachHangs
                      join tk in _db.TaiKhoans on kh.MaKhachHang equals tk.MaKhachHang into tkGroup
                      from t in tkGroup.DefaultIfEmpty()
                      orderby kh.MaKhachHang descending
                      select new
                      {
                          MaKhachHang = kh.MaKhachHang.Trim(),
                          kh.HoTen,
                          kh.SoDienThoai,
                          kh.Email,
                          TenDangNhap = t != null ? t.TenDangNhap.Trim() : "Chưa có TK",
                          TrangThai = t != null ? (bool?)t.TrangThai : null,
                          SoDonHang = _db.HoaDons.Count(hd => hd.MaKhachHang == kh.MaKhachHang),
                          TongChiTieu = _db.HoaDons
                                           .Where(hd => hd.MaKhachHang == kh.MaKhachHang && hd.TrangThai == "Đã hoàn thành")
                                           .Sum(hd => (decimal?)hd.TongTien) ?? 0
                      }).ToList();

            // Lọc theo trạng thái
            if (locTT == "1")
            {
                ds = ds.Where(x => x.TrangThai == true).ToList();
            }
            else if (locTT == "0")
            {
                ds = ds.Where(x => x.TrangThai == false).ToList();
            }
            else if (locTT == "null")
            {
                ds = ds.Where(x => x.TrangThai == null).ToList();
            }
            // Lọc theo từ khóa
            if (!string.IsNullOrEmpty(tuKhoa))
            {
                ds = ds.Where(x =>
                    (x.HoTen != null && x.HoTen.ToLower().Contains(tuKhoa)) ||
                    (x.SoDienThoai != null && x.SoDienThoai.Contains(tuKhoa)) ||
                    (x.Email != null && x.Email.ToLower().Contains(tuKhoa)) ||
                    x.TenDangNhap.ToLower().Contains(tuKhoa)).ToList();
            }

            gvKhachHang.DataSource = ds;
            gvKhachHang.DataBind();
        }

        protected void btnTim_Click(object sender, EventArgs e)
        {
            lblThongBao.Text = "";
            LoadDanhSachKhachHang();
        }

        protected void btnTatCa_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            ddlLocTrangThai.SelectedIndex = 0;
            lblThongBao.Text = "";
            LoadDanhSachKhachHang();
        }

        protected void gvKhachHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DoiTrangThai")
            {
                string tenDN = e.CommandArgument.ToString().Trim();
                var tk = _db.TaiKhoans.FirstOrDefault(t => t.TenDangNhap.Trim() == tenDN);
                if (tk != null)
                {
                    bool trangThaiMoi = !(tk.TrangThai == true);
                    tk.TrangThai = trangThaiMoi;
                    _db.SaveChanges();

                    lblThongBao.Text = trangThaiMoi
                        ? "✔ Đã mở khóa tài khoản \"" + tenDN + "\"!"
                        : "🔒 Đã khóa tài khoản \"" + tenDN + "\" (khách sẽ không thể đăng nhập)!";
                    lblThongBao.ForeColor = trangThaiMoi ? System.Drawing.Color.Green : System.Drawing.Color.DarkRed;

                    LoadThongKe();
                    LoadDanhSachKhachHang();
                }
            }
        }

        public string LayChuTrangThai(object trangThai)
        {
            if (trangThai == null || trangThai == DBNull.Value) return "Khách lẻ";
            return Convert.ToBoolean(trangThai) ? "Hoạt động" : "Đã khóa";
        }

        public string LayMauTrangThai(object trangThai)
        {
            string baseStyle = "padding:4px 10px; border-radius:14px; font-size:12px; font-weight:600; display:inline-block; white-space:nowrap;";
            if (trangThai == null || trangThai == DBNull.Value)
                return baseStyle + "background:#f3f4f6; color:#4b5563;";

            return Convert.ToBoolean(trangThai)
                ? baseStyle + "background:#d1fae5; color:#065f46;"
                : baseStyle + "background:#fee2e2; color:#991b1b;";
        }
    }
}