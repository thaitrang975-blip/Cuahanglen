<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="QuanLyDonHang.aspx.cs" Inherits="Cuahanglen.Admin.QuanLyDonHang" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- THỐNG KÊ NHANH ĐƠN HÀNG -->
    <div class="stat-grid">
        <div class="stat-card">
            <small>Đơn chờ xác nhận</small>
            <h2><asp:Label ID="lblChoDuyet" runat="server" Text="0"></asp:Label></h2>
        </div>
        <div class="stat-card">
            <small>Đơn đang giao</small>
            <h2><asp:Label ID="lblDangGiao" runat="server" Text="0"></asp:Label></h2>
        </div>
        <div class="stat-card">
            <small>Doanh thu đơn hoàn thành</small>
            <h2><asp:Label ID="lblDoanhThu" runat="server" Text="0₫"></asp:Label></h2>
        </div>
    </div>

    <!-- THANH TÌM KIẾM & LỌC TRẠNG THÁI -->
    <div class="card-box">
        <h3>🔍 Tìm kiếm & Lọc đơn hàng</h3>
        <div class="form-grid" style="align-items: end;">
            <div class="field-group">
                <label>Mã đơn hoặc Tên / SĐT khách</label>
                <asp:TextBox ID="txtTimKiem" runat="server" placeholder="VD: HD..., Minh Anh, 09..."></asp:TextBox>
            </div>
            <div class="field-group">
                <label>Lọc theo trạng thái</label>
                <asp:DropDownList ID="ddlLocTrangThai" runat="server">
                    <asp:ListItem Value="" Text="-- Tất cả trạng thái --"></asp:ListItem>
                    <asp:ListItem Value="Đang xác nhận" Text="🕒 Đang xác nhận"></asp:ListItem>
                    <asp:ListItem Value="Đang giao hàng" Text="🚚 Đang giao hàng"></asp:ListItem>
                    <asp:ListItem Value="Đã hoàn thành" Text="✅ Đã hoàn thành"></asp:ListItem>
                    <asp:ListItem Value="Đã hủy" Text="❌ Đã hủy"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div style="display:flex; gap:8px;">
                <asp:Button ID="btnLoc" runat="server" Text="Lọc đơn hàng" CssClass="btn-primary" OnClick="btnLoc_Click" />
                <asp:Button ID="btnHienTatCa" runat="server" Text="Tất cả" CssClass="btn-secondary" OnClick="btnHienTatCa_Click" />
            </div>
        </div>
    </div>

    <!-- KHUNG XEM CHI TIẾT & DUYỆT ĐƠN (CHỈ HIỆN KHI BẤM "CHI TIẾT / DUYỆT") -->
    <asp:Panel ID="pnlChiTiet" runat="server" Visible="false" CssClass="card-box" style="border: 2px solid #fbcfe8;">
        <div style="display:flex; justify-content:space-between; align-items:center; margin-bottom:14px;">
            <h3 style="margin-bottom:0;">🧾 Chi tiết đơn hàng #<asp:Label ID="lblMaHD" runat="server"></asp:Label></h3>
            <asp:Button ID="btnDongChiTiet" runat="server" Text="✖ Đóng" CssClass="btn-secondary"
                style="padding:6px 12px; font-size:12.5px;" OnClick="btnDongChiTiet_Click" />
        </div>

        <div style="background:#fdf2f8; padding:14px; border-radius:8px; margin-bottom:16px; font-size:13.5px; line-height:1.8;">
            <b>Khách hàng:</b> <asp:Label ID="lblTenKH" runat="server"></asp:Label> &nbsp;|&nbsp;
            <b>SĐT:</b> <asp:Label ID="lblSDT" runat="server"></asp:Label><br />
            <b>Địa chỉ giao:</b> <asp:Label ID="lblDiaChi" runat="server"></asp:Label><br />
            <b>Ngày đặt:</b> <asp:Label ID="lblNgayLap" runat="server"></asp:Label> &nbsp;|&nbsp;
            <b>Thanh toán:</b> <asp:Label ID="lblThanhToan" runat="server"></asp:Label> &nbsp;|&nbsp;
            <b>Tổng tiền:</b> <span style="color:#be185d; font-weight:700;"><asp:Label ID="lblTongTienCT" runat="server"></asp:Label></span>
        </div>

        <!-- CẬP NHẬT TRẠNG THÁI ĐƠN -->
        <div style="display:flex; align-items:center; gap:12px; margin-bottom:16px; flex-wrap:wrap;">
            <label style="font-size:13.5px; font-weight:600;">Cập nhật trạng thái:</label>
            <asp:DropDownList ID="ddlTrangThaiMoi" runat="server"
                style="padding:8px 12px; border:1px solid #d1d5db; border-radius:8px; font-size:13.5px;">
                <asp:ListItem Value="Đang xác nhận" Text="🕒 Đang xác nhận"></asp:ListItem>
                <asp:ListItem Value="Đang giao hàng" Text="🚚 Đang giao hàng"></asp:ListItem>
                <asp:ListItem Value="Đã hoàn thành" Text="✅ Đã hoàn thành"></asp:ListItem>
                <asp:ListItem Value="Đã hủy" Text="❌ Đã hủy"></asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnCapNhatTrangThai" runat="server" Text="💾 Lưu trạng thái"
                CssClass="btn-primary" OnClick="btnCapNhatTrangThai_Click" />
            <asp:Label ID="lblThongBao" runat="server" Font-Bold="true" Font-Size="13.5px"></asp:Label>
        </div>

        <!-- DANH SÁCH SẢN PHẨM TRONG ĐƠN -->
        <asp:GridView ID="gvChiTietDon" runat="server" AutoGenerateColumns="False" CssClass="gv-table" GridLines="None">
            <Columns>
                <asp:TemplateField HeaderText="Ảnh">
                    <ItemTemplate>
                        <img src='<%# ResolveUrl("~/Images/" + Eval("HinhAnh").ToString().Trim()) %>'
                             width="44" height="44" style="object-fit:cover; border-radius:6px;" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TenSanPham" HeaderText="Tên sản phẩm" />
                <asp:BoundField DataField="Size" HeaderText="Phân loại" />
                <asp:BoundField DataField="SoLuong" HeaderText="Số lượng" />
                <asp:BoundField DataField="DonGia" HeaderText="Đơn giá" DataFormatString="{0:N0}₫" />
                <asp:BoundField DataField="ThanhTien" HeaderText="Thành tiền" DataFormatString="{0:N0}₫" />
            </Columns>
        </asp:GridView>
    </asp:Panel>

    <!-- BẢNG DANH SÁCH ĐƠN HÀNG -->
    <div class="card-box">
        <h3>📋 Danh sách đơn hàng</h3>
        <asp:GridView ID="gvDonHang" runat="server" AutoGenerateColumns="False" CssClass="gv-table"
            DataKeyNames="MaHoaDon" OnRowCommand="gvDonHang_RowCommand" GridLines="None"
            EmptyDataText="Chưa có đơn hàng nào phù hợp.">
            <Columns>
                <asp:BoundField DataField="MaHoaDon" HeaderText="Mã đơn" />
                <asp:BoundField DataField="TenKhachHang" HeaderText="Khách hàng" />
                <asp:BoundField DataField="SoDienThoai" HeaderText="Số điện thoại" />
                <asp:BoundField DataField="NgayLap" HeaderText="Ngày đặt" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                <asp:BoundField DataField="TongTien" HeaderText="Tổng tiền" DataFormatString="{0:N0}₫" />
                <asp:BoundField DataField="PhuongThucThanhToan" HeaderText="Thanh toán" />
                <asp:TemplateField HeaderText="Trạng thái">
                    <ItemTemplate>
                        <span style='<%# LayMauTrangThai(Eval("TrangThai").ToString()) %>'>
                            <%# Eval("TrangThai") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thao tác">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnXemCT" runat="server" CommandName="XemCT"
                            CommandArgument='<%# Eval("MaHoaDon") %>' CssClass="btn-edit">
                            Chi tiết / Duyệt
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>