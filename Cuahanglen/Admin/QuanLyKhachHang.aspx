<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="QuanLyKhachHang.aspx.cs" Inherits="Cuahanglen.Admin.QuanLyKhachHang" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- THỐNG KÊ NHANH KHÁCH HÀNG -->
    <div class="stat-grid">
        <div class="stat-card">
            <small>Tổng số khách hàng</small>
            <h2><asp:Label ID="lblTongKH" runat="server" Text="0"></asp:Label></h2>
        </div>
        <div class="stat-card">
            <small>Tài khoản đang hoạt động</small>
            <h2><asp:Label ID="lblDangHoatDong" runat="server" Text="0"></asp:Label></h2>
        </div>
        <div class="stat-card">
            <small>Tài khoản đang bị khóa</small>
            <h2><asp:Label ID="lblBiKhoa" runat="server" Text="0"></asp:Label></h2>
        </div>
    </div>

    <!-- THANH TÌM KIẾM & LỌC -->
    <div class="card-box">
        <h3>🔍 Tìm kiếm & Lọc khách hàng</h3>
        <div class="form-grid" style="align-items: end;">
            <div class="field-group">
                <label>Tên khách hàng, SĐT, Email hoặc Tài khoản</label>
                <asp:TextBox ID="txtTimKiem" runat="server" placeholder="VD: Minh Anh, 0912..."></asp:TextBox>
            </div>
            <div class="field-group">
                <label>Trạng thái tài khoản</label>
                <asp:DropDownList ID="ddlLocTrangThai" runat="server">
                    <asp:ListItem Value="" Text="-- Tất cả --"></asp:ListItem>
                    <asp:ListItem Value="1" Text="✅ Đang hoạt động"></asp:ListItem>
                    <asp:ListItem Value="0" Text="🔒 Đã khóa"></asp:ListItem>
                    <asp:ListItem Value="null" Text="👤 Khách lẻ (Chưa có TK)"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div style="display:flex; gap:8px;">
                <asp:Button ID="btnTim" runat="server" Text="Tìm kiếm" CssClass="btn-primary" OnClick="btnTim_Click" />
                <asp:Button ID="btnTatCa" runat="server" Text="Tất cả" CssClass="btn-secondary" OnClick="btnTatCa_Click" />
            </div>
        </div>
        <div style="margin-top:10px;">
            <asp:Label ID="lblThongBao" runat="server" Font-Bold="true" Font-Size="13.5px"></asp:Label>
        </div>
    </div>

    <!-- BẢNG DANH SÁCH KHÁCH HÀNG -->
    <div class="card-box">
        <h3>👤 Danh sách khách hàng</h3>
        <asp:GridView ID="gvKhachHang" runat="server" AutoGenerateColumns="False" CssClass="gv-table"
            DataKeyNames="MaKhachHang" OnRowCommand="gvKhachHang_RowCommand" GridLines="None"
            EmptyDataText="Không tìm thấy khách hàng nào.">
            <Columns>
                <asp:BoundField DataField="MaKhachHang" HeaderText="Mã KH" />
                <asp:BoundField DataField="HoTen" HeaderText="Họ và tên" />
                <asp:BoundField DataField="SoDienThoai" HeaderText="Số điện thoại" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="TenDangNhap" HeaderText="Tài khoản" />
                <asp:BoundField DataField="SoDonHang" HeaderText="Số đơn" />
                <asp:BoundField DataField="TongChiTieu" HeaderText="Tổng chi tiêu" DataFormatString="{0:N0}₫" />
                <asp:TemplateField HeaderText="Trạng thái">
                    <ItemTemplate>
                        <span style='<%# LayMauTrangThai(Eval("TrangThai")) %>'>
                            <%# LayChuTrangThai(Eval("TrangThai")) %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thao tác">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnKhoaMo" runat="server"
                            CommandName="DoiTrangThai"
                            CommandArgument='<%# Eval("TenDangNhap") %>'
                            Visible='<%# Eval("TenDangNhap").ToString() != "Chưa có TK" %>'
                            CssClass='<%# (Eval("TrangThai") != null && Convert.ToBoolean(Eval("TrangThai"))) ? "btn-danger" : "btn-edit" %>'
                            OnClientClick="return confirm('Bạn có chắc muốn thay đổi trạng thái tài khoản khách hàng này?');">
                            <%# (Eval("TrangThai") != null && Convert.ToBoolean(Eval("TrangThai"))) ? "Khóa TK" : "Mở khóa" %>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>