using Cuahanglen.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace Cuahanglen.Admin
{
    public partial class QuanLySanPham : System.Web.UI.Page
    {
        private QLCuaHangLenEntities _db = new QLCuaHangLenEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDanhMuc();
                LoadThongKe();
                LoadGridSanPham();
            }
        }

        private void LoadDanhMuc()
        {
            var dsDM = _db.DanhMucs
                  .ToList()
                  .Select(d => new { MaDanhMuc = d.MaDanhMuc.Trim(), d.TenDanhMuc })
                  .ToList();

            ddlDanhMuc.DataSource = dsDM;
            ddlDanhMuc.DataTextField = "TenDanhMuc";
            ddlDanhMuc.DataValueField = "MaDanhMuc";
            ddlDanhMuc.DataBind();
        }

        private void LoadThongKe()
        {
            lblTongSP.Text = _db.SanPhams.Count().ToString();
            lblTongDon.Text = _db.HoaDons.Count().ToString();
            decimal doanhThu = _db.HoaDons
                                  .Where(h => h.TrangThai == "Đã hoàn thành")
                                  .Sum(h => (decimal?)h.TongTien) ?? 0;
            lblDoanhThu.Text = string.Format("{0:N0}₫", doanhThu);
        }

        private void LoadGridSanPham()
        {
            var ds = (from sp in _db.SanPhams
                      join dm in _db.DanhMucs on sp.MaDanhMuc equals dm.MaDanhMuc
                      join kho in _db.ChiTietKhoes on sp.MaSanPham equals kho.MaSanPham into khoGroup
                      from k in khoGroup.DefaultIfEmpty()
                      orderby sp.MaSanPham descending
                      select new
                      {
                          MaSanPham = sp.MaSanPham.Trim(),
                          sp.TenSanPham,
                          dm.TenDanhMuc,
                          sp.GiaBan,
                          sp.HinhAnh,
                          SoLuongTon = k != null ? k.SoLuongTon : 0,
                          DaBan = (from ct in _db.ChiTietHoaDons
                                   join hd in _db.HoaDons on ct.MaHoaDon equals hd.MaHoaDon
                                   where ct.MaSanPham == sp.MaSanPham && hd.TrangThai == "Đã hoàn thành"
                                   select (int?)ct.SoLuong).Sum() ?? 0,
                          DoanhThuSP = (from ct in _db.ChiTietHoaDons
                                        join hd in _db.HoaDons on ct.MaHoaDon equals hd.MaHoaDon
                                        where ct.MaSanPham == sp.MaSanPham && hd.TrangThai == "Đã hoàn thành"
                                        select (decimal?)(ct.SoLuong * ct.DonGia)).Sum() ?? 0
                      }).ToList();

            gvSanPham.DataSource = ds;
            gvSanPham.DataBind();
        }

        protected void btnThem_Click(object sender, EventArgs e)
        {
            string maSP = txtMaSP.Text.Trim();
            if (string.IsNullOrEmpty(maSP) || string.IsNullOrEmpty(txtTenSP.Text.Trim()))
            {
                HienThongBao("Vui lòng nhập đầy đủ Mã sản phẩm và Tên sản phẩm!", false);
                return;
            }

            if (_db.SanPhams.Any(x => x.MaSanPham.Trim() == maSP))
            {
                HienThongBao("Mã sản phẩm này đã tồn tại!", false);
                return;
            }

            string tenAnh = "lenmilkcotton50g.jpg";
            if (fuHinhAnh.HasFile)
            {
                tenAnh = Path.GetFileName(fuHinhAnh.FileName);
                fuHinhAnh.SaveAs(Server.MapPath("~/Images/" + tenAnh));
            }

            SanPham spMoi = new SanPham
            {
                MaSanPham = maSP,
                TenSanPham = txtTenSP.Text.Trim(),
                MaDanhMuc = ddlDanhMuc.SelectedValue,
                MauSac = txtMauSac.Text.Trim(),
                GiaNhap = decimal.TryParse(txtGiaNhap.Text, out decimal gn) ? gn : 0,
                GiaBan = decimal.TryParse(txtGiaBan.Text, out decimal gb) ? gb : 0,
                HinhAnh = tenAnh,
                MoTa = txtMoTa.Text.Trim()
            };
            _db.SanPhams.Add(spMoi);

            ChiTietKho khoMoi = new ChiTietKho
            {
                MaChiTiet = "CTK" + DateTime.Now.ToString("HHmmss"),
                MaSanPham = maSP,
                Size = string.IsNullOrEmpty(txtSize.Text.Trim()) ? "Tiêu chuẩn" : txtSize.Text.Trim(),
                SoLuongTon = int.TryParse(txtSoLuongTon.Text, out int sl) ? sl : 0
            };
            _db.ChiTietKhoes.Add(khoMoi);

            _db.SaveChanges();
            HienThongBao("Thêm sản phẩm thành công!", true);
            XoaTrangForm();
            LoadThongKe();
            LoadGridSanPham();
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            string maSP = txtMaSP.Text.Trim();
            var sp = _db.SanPhams.FirstOrDefault(x => x.MaSanPham.Trim() == maSP);
            if (sp == null)
            {
                HienThongBao("Không tìm thấy mã sản phẩm cần sửa (hãy bấm nút Sửa ở bảng bên dưới trước)!", false);
                return;
            }

            sp.TenSanPham = txtTenSP.Text.Trim();
            sp.MaDanhMuc = ddlDanhMuc.SelectedValue;
            sp.MauSac = txtMauSac.Text.Trim();
            sp.GiaNhap = decimal.TryParse(txtGiaNhap.Text, out decimal gn) ? gn : sp.GiaNhap;
            sp.GiaBan = decimal.TryParse(txtGiaBan.Text, out decimal gb) ? gb : sp.GiaBan;
            sp.MoTa = txtMoTa.Text.Trim();

            if (fuHinhAnh.HasFile)
            {
                string tenAnh = Path.GetFileName(fuHinhAnh.FileName);
                fuHinhAnh.SaveAs(Server.MapPath("~/Images/" + tenAnh));
                sp.HinhAnh = tenAnh;
            }

            var kho = _db.ChiTietKhoes.FirstOrDefault(k => k.MaSanPham.Trim() == maSP);
            if (kho != null)
            {
                kho.Size = txtSize.Text.Trim();
                kho.SoLuongTon = int.TryParse(txtSoLuongTon.Text, out int sl) ? sl : kho.SoLuongTon;
            }

            _db.SaveChanges();
            HienThongBao("Cập nhật sản phẩm thành công!", true);
            XoaTrangForm();
            LoadGridSanPham();
        }

        protected void gvSanPham_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string maSP = e.CommandArgument.ToString().Trim();

            if (e.CommandName == "ChonSua")
            {
                var sp = _db.SanPhams.FirstOrDefault(x => x.MaSanPham.Trim() == maSP);
                if (sp != null)
                {
                    txtMaSP.Text = sp.MaSanPham.Trim();
                    txtMaSP.ReadOnly = true;
                    txtTenSP.Text = sp.TenSanPham;
                    ddlDanhMuc.SelectedValue = sp.MaDanhMuc.Trim();
                    btnThem.Enabled = false;
                    btnThem.Style["opacity"] = "0.5";
                    txtMauSac.Text = sp.MauSac;
                    txtGiaNhap.Text = string.Format("{0:0}", sp.GiaNhap);
                    txtGiaBan.Text = string.Format("{0:0}", sp.GiaBan);
                    txtMoTa.Text = sp.MoTa;

                    var kho = _db.ChiTietKhoes.FirstOrDefault(k => k.MaSanPham.Trim() == maSP);
                    if (kho != null)
                    {
                        txtSize.Text = kho.Size;
                        txtSoLuongTon.Text = kho.SoLuongTon.ToString();
                    }
                }
            }
            else if (e.CommandName == "XoaSP")
            {
                // Kiểm tra nếu sản phẩm đã nằm trong hóa đơn khách đặt thì không cho xóa để tránh lỗi khóa ngoại
                if (_db.ChiTietHoaDons.Any(ct => ct.MaSanPham.Trim() == maSP))
                {
                    HienThongBao("Không thể xóa sản phẩm này vì đã có trong lịch sử đơn hàng!", false);
                    return;
                }

                var khoList = _db.ChiTietKhoes.Where(k => k.MaSanPham.Trim() == maSP).ToList();
                _db.ChiTietKhoes.RemoveRange(khoList);

                var sp = _db.SanPhams.FirstOrDefault(x => x.MaSanPham.Trim() == maSP);
                if (sp != null) _db.SanPhams.Remove(sp);

                _db.SaveChanges();
                HienThongBao("Đã xóa sản phẩm!", true);
                LoadThongKe();
                LoadGridSanPham();
            }
        }

        protected void btnLamMoi_Click(object sender, EventArgs e)
        {
            XoaTrangForm();
            lblThongBao.Text = "";
        }

        private void XoaTrangForm()
        {
            txtMaSP.Text = "";
            txtMaSP.ReadOnly = false;
            txtTenSP.Text = "";
            txtMauSac.Text = "";
            txtGiaNhap.Text = "";
            txtGiaBan.Text = "";
            txtSize.Text = "Cuộn 50g";
            txtSoLuongTon.Text = "50";
            txtMoTa.Text = "";
            btnThem.Enabled = true;
            btnThem.Style["opacity"] = "1";
        }

        private void HienThongBao(string msg, bool thanhCong)
        {
            lblThongBao.Text = msg;
            lblThongBao.ForeColor = thanhCong ? System.Drawing.Color.Green : System.Drawing.Color.Red;
        }
    }
}