using System;
using System.Linq;
using System.Web.UI.WebControls;
using Cuahanglen.Models;

namespace Cuahanglen.Admin
{
    public partial class QuanLyDonHang : System.Web.UI.Page
    {
        private QLCuaHangLenEntities _db = new QLCuaHangLenEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadThongKe();
                LoadDanhSachDonHang();
            }
        }

        private void LoadThongKe()
        {
            lblChoDuyet.Text = _db.HoaDons.Count(h => h.TrangThai == "Đang xác nhận").ToString();
            lblDangGiao.Text = _db.HoaDons.Count(h => h.TrangThai == "Đang giao hàng").ToString();

            decimal doanhThu = _db.HoaDons
                                  .Where(h => h.TrangThai == "Đã hoàn thành")
                                  .Sum(h => (decimal?)h.TongTien) ?? 0;
            lblDoanhThu.Text = string.Format("{0:N0}₫", doanhThu);
        }

        private void LoadDanhSachDonHang()
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();
            string trangThai = ddlLocTrangThai.SelectedValue;

            var query = from hd in _db.HoaDons
                        join kh in _db.KhachHangs on hd.MaKhachHang equals kh.MaKhachHang into khGroup
                        from k in khGroup.DefaultIfEmpty()
                        orderby hd.NgayLap descending
                        select new
                        {
                            MaHoaDon = hd.MaHoaDon.Trim(),
                            TenKhachHang = k != null ? k.HoTen : "Khách lẻ",
                            SoDienThoai = k != null ? k.SoDienThoai : "",
                            hd.NgayLap,
                            hd.TongTien,
                            hd.PhuongThucThanhToan,
                            hd.TrangThai
                        };

            if (!string.IsNullOrEmpty(trangThai))
            {
                query = query.Where(x => x.TrangThai == trangThai);
            }

            var danhSach = query.ToList();

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                danhSach = danhSach.Where(x =>
                    x.MaHoaDon.ToLower().Contains(tuKhoa) ||
                    x.TenKhachHang.ToLower().Contains(tuKhoa) ||
                    x.SoDienThoai.Contains(tuKhoa)).ToList();
            }

            gvDonHang.DataSource = danhSach;
            gvDonHang.DataBind();
        }

        protected void btnLoc_Click(object sender, EventArgs e)
        {
            LoadDanhSachDonHang();
        }

        protected void btnHienTatCa_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            ddlLocTrangThai.SelectedIndex = 0;
            LoadDanhSachDonHang();
        }

        protected void gvDonHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "XemCT")
            {
                string maHD = e.CommandArgument.ToString().Trim();
                HienChiTietDonHang(maHD);
            }
        }

        private void HienChiTietDonHang(string maHD)
        {
            var hd = _db.HoaDons.FirstOrDefault(h => h.MaHoaDon.Trim() == maHD);
            if (hd == null) return;

            var kh = _db.KhachHangs.FirstOrDefault(k => k.MaKhachHang == hd.MaKhachHang);

            lblMaHD.Text = hd.MaHoaDon.Trim();
            lblTenKH.Text = kh != null ? kh.HoTen : "Khách lẻ";
            lblSDT.Text = kh != null ? kh.SoDienThoai : "";
            lblDiaChi.Text = kh != null ? kh.DiaChi : "";
            lblNgayLap.Text = hd.NgayLap.HasValue ? hd.NgayLap.Value.ToString("dd/MM/yyyy HH:mm") : "";
            lblThanhToan.Text = hd.PhuongThucThanhToan;
            lblTongTienCT.Text = string.Format("{0:N0}₫", hd.TongTien);

            if (ddlTrangThaiMoi.Items.FindByValue(hd.TrangThai) != null)
            {
                ddlTrangThaiMoi.SelectedValue = hd.TrangThai;
            }

            // Lấy danh sách sản phẩm trong đơn bằng LINQ Join
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

            lblThongBao.Text = "";
            pnlChiTiet.Visible = true;
        }

        protected void btnCapNhatTrangThai_Click(object sender, EventArgs e)
        {
            string maHD = lblMaHD.Text.Trim();
            var hd = _db.HoaDons.FirstOrDefault(h => h.MaHoaDon.Trim() == maHD);
            if (hd == null) return;

            string trangThaiCu = hd.TrangThai;
            string trangThaiMoi = ddlTrangThaiMoi.SelectedValue;

            // Nếu chuyển sang "Đã hủy" từ trạng thái khác -> hoàn lại số lượng tồn kho
            if (trangThaiMoi == "Đã hủy" && trangThaiCu != "Đã hủy")
            {
                var dsCT = _db.ChiTietHoaDons.Where(c => c.MaHoaDon.Trim() == maHD).ToList();
                foreach (var ct in dsCT)
                {
                    var kho = _db.ChiTietKhoes.FirstOrDefault(k => k.MaSanPham == ct.MaSanPham);
                    if (kho != null) kho.SoLuongTon += ct.SoLuong;
                }
            }

            hd.TrangThai = trangThaiMoi;

            // Ghi nhận nhân viên/admin duyệt đơn
            if (Session["MaNV"] != null)
            {
                hd.MaNhanVien = Session["MaNV"].ToString().Trim();
            }

            _db.SaveChanges();

            lblThongBao.Text = "✔ Đã cập nhật trạng thái đơn #" + maHD + " thành \"" + trangThaiMoi + "\"!";
            lblThongBao.ForeColor = System.Drawing.Color.Green;

            LoadThongKe();
            LoadDanhSachDonHang();
        }

        protected void btnDongChiTiet_Click(object sender, EventArgs e)
        {
            pnlChiTiet.Visible = false;
        }

        // Hàm tô màu nhãn trạng thái cho đẹp mắt
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