using System;
using System.Linq;
using System.Web.UI.WebControls;
using Cuahanglen.Models;

namespace Cuahanglen.Admin
{
    public partial class QuanLyNhaCungCap : System.Web.UI.Page
    {
        private QLCuaHangLenEntities _db = new QLCuaHangLenEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadThongKe();
                LoadDanhSachNCC();
            }
        }

        private void LoadThongKe()
        {
            lblTongNCC.Text = _db.NhaCungCaps.Count().ToString();
            lblTongSP.Text = _db.SanPhams.Count().ToString();
            lblTongDM.Text = _db.DanhMucs.Count().ToString();
        }

        private void LoadDanhSachNCC()
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();

            var ds = _db.NhaCungCaps
                        .OrderBy(n => n.MaNCC)
                        .ToList()
                        .Select(n => new
                        {
                            MaNCC = n.MaNCC.Trim(),
                            n.TenNCC,
                            n.SoDienThoai,
                            n.Email,
                            n.DiaChi
                        }).ToList();

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                ds = ds.Where(x =>
                    x.MaNCC.ToLower().Contains(tuKhoa) ||
                    (x.TenNCC != null && x.TenNCC.ToLower().Contains(tuKhoa)) ||
                    (x.SoDienThoai != null && x.SoDienThoai.Contains(tuKhoa))).ToList();
            }

            gvNhaCungCap.DataSource = ds;
            gvNhaCungCap.DataBind();
        }

        private bool KiemTraDuLieu()
        {
            if (string.IsNullOrWhiteSpace(txtMaNCC.Text) ||
                string.IsNullOrWhiteSpace(txtTenNCC.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                HienThongBao("Vui lòng nhập đầy đủ Mã NCC, Tên nhà cung cấp, SĐT và Địa chỉ!", false);
                return false;
            }
            return true;
        }

        protected void btnThem_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieu()) return;

            string maNCC = txtMaNCC.Text.Trim();
            if (_db.NhaCungCaps.Any(x => x.MaNCC.Trim() == maNCC))
            {
                HienThongBao("Mã nhà cung cấp này đã tồn tại!", false);
                return;
            }

            NhaCungCap nccMoi = new NhaCungCap
            {
                MaNCC = maNCC,
                TenNCC = txtTenNCC.Text.Trim(),
                SoDienThoai = txtSDT.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim()
            };

            _db.NhaCungCaps.Add(nccMoi);
            _db.SaveChanges();

            HienThongBao("Thêm nhà cung cấp thành công!", true);
            XoaTrangForm();
            LoadThongKe();
            LoadDanhSachNCC();
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieu()) return;

            string maNCC = txtMaNCC.Text.Trim();
            var ncc = _db.NhaCungCaps.FirstOrDefault(x => x.MaNCC.Trim() == maNCC);
            if (ncc == null)
            {
                HienThongBao("Hãy bấm nút Sửa ở bảng bên dưới trước khi lưu cập nhật!", false);
                return;
            }

            ncc.TenNCC = txtTenNCC.Text.Trim();
            ncc.SoDienThoai = txtSDT.Text.Trim();
            ncc.Email = txtEmail.Text.Trim();
            ncc.DiaChi = txtDiaChi.Text.Trim();

            _db.SaveChanges();

            HienThongBao("Cập nhật nhà cung cấp thành công!", true);
            XoaTrangForm();
            LoadDanhSachNCC();
        }

        protected void gvNhaCungCap_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string maNCC = e.CommandArgument.ToString().Trim();

            if (e.CommandName == "ChonSua")
            {
                var ncc = _db.NhaCungCaps.FirstOrDefault(x => x.MaNCC.Trim() == maNCC);
                if (ncc != null)
                {
                    txtMaNCC.Text = ncc.MaNCC.Trim();
                    txtMaNCC.ReadOnly = true;
                    txtTenNCC.Text = ncc.TenNCC;
                    txtSDT.Text = ncc.SoDienThoai;
                    txtEmail.Text = ncc.Email;
                    txtDiaChi.Text = ncc.DiaChi;

                    btnThem.Enabled = false;
                    btnThem.Style["opacity"] = "0.5";
                }
            }
            else if (e.CommandName == "XoaNCC")
            {
                var ncc = _db.NhaCungCaps.FirstOrDefault(x => x.MaNCC.Trim() == maNCC);
                if (ncc == null) return;

                // Chặn xóa nếu nhà cung cấp đã có phiếu nhập hàng liên kết
                if (ncc.PhieuNhaps.Any())
                {
                    HienThongBao("Không thể xóa nhà cung cấp này vì đã có phiếu nhập hàng liên quan!", false);
                    return;
                }

                _db.NhaCungCaps.Remove(ncc);
                _db.SaveChanges();

                HienThongBao("Đã xóa nhà cung cấp!", true);
                XoaTrangForm();
                LoadThongKe();
                LoadDanhSachNCC();
            }
        }

        protected void btnTim_Click(object sender, EventArgs e)
        {
            LoadDanhSachNCC();
        }

        protected void btnTatCa_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            LoadDanhSachNCC();
        }

        protected void btnLamMoi_Click(object sender, EventArgs e)
        {
            XoaTrangForm();
            lblThongBao.Text = "";
        }

        private void XoaTrangForm()
        {
            txtMaNCC.Text = "";
            txtMaNCC.ReadOnly = false;
            txtTenNCC.Text = "";
            txtSDT.Text = "";
            txtEmail.Text = "";
            txtDiaChi.Text = "";

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