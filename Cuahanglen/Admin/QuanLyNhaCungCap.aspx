<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="QuanLyNhaCungCap.aspx.cs" Inherits="Cuahanglen.Admin.QuanLyNhaCungCap" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- THỐNG KÊ NHANH -->
    <div class="stat-grid">
        <div class="stat-card">
            <small>Tổng số nhà cung cấp</small>
            <h2><asp:Label ID="lblTongNCC" runat="server" Text="0"></asp:Label></h2>
        </div>
        <div class="stat-card">
            <small>Tổng sản phẩm đang kinh doanh</small>
            <h2><asp:Label ID="lblTongSP" runat="server" Text="0"></asp:Label></h2>
        </div>
        <div class="stat-card">
            <small>Tổng danh mục sản phẩm</small>
            <h2><asp:Label ID="lblTongDM" runat="server" Text="0"></asp:Label></h2>
        </div>
    </div>

    <!-- FORM THÊM / CẬP NHẬT NHÀ CUNG CẤP -->
    <div class="card-box">
        <h3>🏭 Thêm mới / Cập nhật nhà cung cấp</h3>

        <div class="form-grid">
            <div class="field-group">
                <label>Mã nhà cung cấp *</label>
                <asp:TextBox ID="txtMaNCC" runat="server" placeholder="VD: NCC01"></asp:TextBox>
            </div>
            <div class="field-group">
                <label>Tên nhà cung cấp *</label>
                <asp:TextBox ID="txtTenNCC" runat="server" placeholder="VD: Xưởng Len Sợi Miền Nam"></asp:TextBox>
            </div>
            <div class="field-group">
                <label>Số điện thoại *</label>
                <asp:TextBox ID="txtSDT" runat="server" placeholder="09xxxxxxxx"></asp:TextBox>
            </div>
            <div class="field-group">
                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server" placeholder="ncc@gmail.com"></asp:TextBox>
            </div>
            <div class="field-group" style="grid-column: span 2;">
                <label>Địa chỉ *</label>
                <asp:TextBox ID="txtDiaChi" runat="server" placeholder="Số nhà, đường, quận/huyện, tỉnh/thành..."></asp:TextBox>
            </div>
        </div>

        <div style="margin-top: 12px;">
            <asp:Label ID="lblThongBao" runat="server" Font-Bold="true" Font-Size="13.5px"></asp:Label>
        </div>

        <div class="btn-bar">
            <asp:Button ID="btnThem" runat="server" Text="➕ Thêm nhà cung cấp" CssClass="btn-primary" OnClick="btnThem_Click" />
            <asp:Button ID="btnCapNhat" runat="server" Text="💾 Lưu cập nhật" CssClass="btn-warning" OnClick="btnCapNhat_Click" />
            <asp:Button ID="btnLamMoi" runat="server" Text="🔄 Làm mới form" CssClass="btn-secondary" OnClick="btnLamMoi_Click" />
        </div>
    </div>

    <!-- DANH SÁCH & TÌM KIẾM NHÀ CUNG CẤP -->
    <div class="card-box">
        <div style="display:flex; justify-content:space-between; align-items:center; flex-wrap:wrap; gap:12px; margin-bottom:16px;">
            <h3 style="margin-bottom:0;">📋 Danh sách nhà cung cấp</h3>
            <div style="display:flex; gap:8px;">
                <asp:TextBox ID="txtTimKiem" runat="server" placeholder="Tìm theo mã, tên, SĐT..."
                    style="padding:8px 12px; border:1px solid #e5e7eb; border-radius:8px; font-size:13.5px; width:240px;"></asp:TextBox>
                <asp:Button ID="btnTim" runat="server" Text="Tìm" CssClass="btn-primary" OnClick="btnTim_Click" />
                <asp:Button ID="btnTatCa" runat="server" Text="Tất cả" CssClass="btn-secondary" OnClick="btnTatCa_Click" />
            </div>
        </div>

        <asp:GridView ID="gvNhaCungCap" runat="server" AutoGenerateColumns="False" CssClass="gv-table"
            DataKeyNames="MaNCC" OnRowCommand="gvNhaCungCap_RowCommand" GridLines="None"
            EmptyDataText="Chưa có nhà cung cấp nào.">
            <Columns>
                <asp:BoundField DataField="MaNCC" HeaderText="Mã NCC" />
                <asp:BoundField DataField="TenNCC" HeaderText="Tên nhà cung cấp" />
                <asp:BoundField DataField="SoDienThoai" HeaderText="Số điện thoại" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="DiaChi" HeaderText="Địa chỉ" />
                <asp:TemplateField HeaderText="Thao tác">
                    <ItemTemplate>
                        <div style="display:flex; gap:6px; align-items:center;">
                            <asp:LinkButton ID="btnSua" runat="server" CommandName="ChonSua"
                                CommandArgument='<%# Eval("MaNCC") %>' CssClass="btn-edit">Sửa</asp:LinkButton>
                            <asp:LinkButton ID="btnXoa" runat="server" CommandName="XoaNCC"
                                CommandArgument='<%# Eval("MaNCC") %>' CssClass="btn-danger"
                                OnClientClick="return confirm('Bạn chắc chắn muốn xóa nhà cung cấp này?');">Xóa</asp:LinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>