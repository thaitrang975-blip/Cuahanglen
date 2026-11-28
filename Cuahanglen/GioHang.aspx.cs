using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Cuahanglen.Models;

namespace Cuahanglen
{
    public partial class GioHang : System.Web.UI.Page
    {
        private QLCuaHangLenEntities _db = new QLCuaHangLenEntities();
        private const decimal FREE_SHIP_LIMIT = 250000;
        private const decimal SHIPPING_FEE = 25000;

        private List<CartItem> LayGioHang()
        {
            var gio = Session["GioHang"] as List<CartItem>;
            if (gio == null)
            {
                gio = new List<CartItem>();
                Session["GioHang"] = gio;
            }
            return gio;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // BẮT BUỘC đăng nhập vai trò Khách hàng mới được vào Giỏ hàng
            if (Session["TenDangNhap"] == null || Session["VaiTro"] == null || Session["VaiTro"].ToString() != "KhachHang")
            {
                Response.Redirect("~/DangNhap.aspx?yeucau=khachhang");
                return;
            }

            if (!IsPostBack)
            {
                string maAdd = Request.QueryString["add"];
                if (!string.IsNullOrEmpty(maAdd))
                {
                    ThemSanPhamVaoGio(maAdd.Trim());
                    Response.Redirect("~/GioHang.aspx");
                    return;
                }

                DienThongTinKhachHang();
                HienThiGioHang();
            }
            if (!IsPostBack)
            {
                // 1. Nhận mã sản phẩm từ nút "Thêm giỏ hàng" ở TrangChu.aspx (?add=SP01)
                string maAdd = Request.QueryString["add"];
                if (!string.IsNullOrEmpty(maAdd))
                {
                    ThemSanPhamVaoGio(maAdd.Trim());
                    // Chuyển lại URL sạch để bấm F5 không bị cộng dồn thêm sản phẩm
                    Response.Redirect("~/GioHang.aspx");
                    return;
                }

                // 2. Tự động điền thông tin nếu khách đã đăng nhập
                DienThongTinKhachHang();

                // 3. Hiển thị giỏ hàng
                HienThiGioHang();
            }
        }

        private void ThemSanPhamVaoGio(string maSP)
        {
            var sp = _db.SanPhams.FirstOrDefault(x => x.MaSanPham.Trim() == maSP);
            if (sp == null) return;

            var gio = LayGioHang();
            var item = gio.FirstOrDefault(x => x.MaSanPham.Trim() == maSP);

            if (item != null)
            {
                item.SoLuong += 1;
            }
            else
            {
                // Lấy Size mặc định từ bảng ChiTietKho (Lưu ý: nếu EF sinh tên ChiTietKhos thì đổi lại cho khớp)
                var kho = _db.ChiTietKhoes.FirstOrDefault(k => k.MaSanPham.Trim() == maSP);

                gio.Add(new CartItem
                {
                    MaSanPham = sp.MaSanPham.Trim(),
                    TenSanPham = sp.TenSanPham,
                    HinhAnh = sp.HinhAnh,
                    Size = kho != null ? kho.Size : "Tiêu chuẩn",
                    DonGia = sp.GiaBan,
                    SoLuong = 1
                });
            }

            Session["GioHang"] = gio;
        }

        private void DienThongTinKhachHang()
        {
            if (Session["MaKH"] != null)
            {
                string maKH = Session["MaKH"].ToString().Trim();
                var kh = _db.KhachHangs.FirstOrDefault(x => x.MaKhachHang.Trim() == maKH);
                if (kh != null)
                {
                    txtHoTen.Text = kh.HoTen;
                    txtSDT.Text = kh.SoDienThoai;
                    txtEmail.Text = kh.Email;
                    txtDiaChi.Text = kh.DiaChi;
                }
            }
        }

        private void HienThiGioHang()
        {
            var gio = LayGioHang();

            if (gio.Count == 0)
            {
                pnlGioTrong.Visible = true;
                pnlCoHang.Visible = false;
                return;
            }

            pnlGioTrong.Visible = false;
            pnlCoHang.Visible = true;

            rptGioHang.DataSource = gio;
            rptGioHang.DataBind();

            decimal tamTinh = gio.Sum(x => x.ThanhTien);
            decimal phiShip = tamTinh >= FREE_SHIP_LIMIT ? 0 : SHIPPING_FEE;
            decimal tongCong = tamTinh + phiShip;

            lblTamTinh.Text = string.Format("{0:N0}₫", tamTinh);
            lblPhiShip.Text = phiShip == 0 ? "Miễn phí" : string.Format("{0:N0}₫", phiShip);
            lblTongCong.Text = string.Format("{0:N0}₫", tongCong);
        }

        protected void rptGioHang_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string maSP = e.CommandArgument.ToString().Trim();
            var gio = LayGioHang();
            var item = gio.FirstOrDefault(x => x.MaSanPham.Trim() == maSP);

            if (item != null)
            {
                if (e.CommandName == "Tang")
                {
                    item.SoLuong += 1;
                }
                else if (e.CommandName == "Giam")
                {
                    item.SoLuong -= 1;
                    if (item.SoLuong <= 0) gio.Remove(item);
                }
                else if (e.CommandName == "Xoa")
                {
                    gio.Remove(item);
                }
            }

            Session["GioHang"] = gio;
            Response.Redirect("~/GioHang.aspx");
        }

        protected void btnXoaHet_Click(object sender, EventArgs e)
        {
            Session["GioHang"] = null;
            Response.Redirect("~/GioHang.aspx");
        }

        protected void btnDatHang_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var gio = LayGioHang();
            if (gio.Count == 0) return;

            try
            {
                // 1. Xác định Mã Khách Hàng 
                if (Session["MaKH"] == null)
                {
                    Response.Redirect("~/DangNhap.aspx?yeucau=khachhang");
                    return;
                }

                string maKH = Session["MaKH"].ToString().Trim();
                var kh = _db.KhachHangs.FirstOrDefault(x => x.MaKhachHang.Trim() == maKH);
                if (kh != null)
                {
                    // Cập nhật lại thông tin người nhận nếu khách thay đổi khi đặt hàng
                    kh.HoTen = txtHoTen.Text.Trim();
                    kh.SoDienThoai = txtSDT.Text.Trim();
                    kh.Email = txtEmail.Text.Trim();
                    kh.DiaChi = txtDiaChi.Text.Trim();
                }
                // 2. Tính tổng tiền và tạo bản ghi HoaDon (Mã dài đúng 10 ký tự khớp CHAR(10))
                decimal tamTinh = gio.Sum(x => x.ThanhTien);
                decimal phiShip = tamTinh >= FREE_SHIP_LIMIT ? 0 : SHIPPING_FEE;
                decimal tongCong = tamTinh + phiShip;

                string maHD = "HD" + DateTime.Now.ToString("ddHHmmss");

                HoaDon hd = new HoaDon
                {
                    MaHoaDon = maHD,
                    MaKhachHang = maKH,
                    MaNhanVien = null, // Khách tự đặt online trên web
                    NgayLap = DateTime.Now,
                    TongTien = tongCong,
                    PhuongThucThanhToan = ddlThanhToan.SelectedValue,
                    TrangThai = "Đang xác nhận"
                };
                _db.HoaDons.Add(hd);

                // 3. Lưu từng dòng vào ChiTietHoaDon & trừ tồn kho trong ChiTietKho
                int stt = 1;
                foreach (var item in gio)
                {
                    string maCTHD = "CT" + DateTime.Now.ToString("HHmmss") + stt.ToString("D2");
                    ChiTietHoaDon ct = new ChiTietHoaDon
                    {
                        MaChiTietHD = maCTHD,
                        MaHoaDon = maHD,
                        MaSanPham = item.MaSanPham,
                        Size = item.Size,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia
                    };
                    _db.ChiTietHoaDons.Add(ct);

                    // Trừ số lượng tồn kho tương ứng
                    var kho = _db.ChiTietKhoes.FirstOrDefault(k => k.MaSanPham.Trim() == item.MaSanPham);
                    if (kho != null && kho.SoLuongTon >= item.SoLuong)
                    {
                        kho.SoLuongTon -= item.SoLuong;
                    }

                    stt++;
                }

                // 4. Lưu toàn bộ xuống SQL Server
                _db.SaveChanges();

                // 5. Xóa giỏ hàng và hiện thông báo thành công
                Session["GioHang"] = null;
                pnlCoHang.Visible = false;
                pnlGioTrong.Visible = false;
                pnlThanhCong.Visible = true;
                lblThongBaoDon.Text = string.Format(
                    "Mã đơn hàng của bạn là <b>#{0}</b> — Tổng thanh toán: <b>{1:N0}₫</b>. Tiệm Len Sợi sẽ sớm liên hệ xác nhận!",
                    maHD, tongCong);
            }
            catch (Exception ex)
            {
                lblLoi.Text = "Có lỗi khi lưu đơn hàng: " + ex.Message;
            }
        }
    }
}