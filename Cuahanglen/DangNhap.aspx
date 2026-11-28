<%@ Page Title="" Language="C#" MasterPageFile="~/SiteLen.Master" AutoEventWireup="true" CodeBehind="DangNhap.aspx.cs" Inherits="Cuahanglen.DangNhap" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="display:flex; justify-content:center; padding: 50px 16px;">
        <div class="auth-box">
            <div class="auth-header">
                <div class="auth-brand">
                    <span>🧶</span>
                    <h3>Đăng nhập Tiệm Len Sợi</h3>
                </div>
            </div>

            <!-- 1. CHỌN VAI TRÒ TRƯỚC -->
            <div class="form-group">
                <label>Chọn vai trò đăng nhập *</label>
                <asp:DropDownList ID="ddlVaiTro" runat="server">
                    <asp:ListItem Value="KhachHang" Text="🛍️ Khách hàng"></asp:ListItem>
                    <asp:ListItem Value="Admin" Text="⚙️ Quản trị viên"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <!-- 2. NHẬP TÊN ĐĂNG NHẬP -->
            <div class="form-group">
                <label>Tên đăng nhập *</label>
                <asp:TextBox ID="txtTenDN" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvTenDN" runat="server" ControlToValidate="txtTenDN"
                    ErrorMessage="Vui lòng nhập tên đăng nhập!" ForeColor="Red" Display="Dynamic" Font-Size="12px" />
            </div>

            <!-- 3. NHẬP MẬT KHẨU -->
            <div class="form-group">
                <label>Mật khẩu *</label>
                <asp:TextBox ID="txtMatKhau" runat="server" TextMode="Password" placeholder="Nhập mật khẩu..."></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMatKhau" runat="server" ControlToValidate="txtMatKhau"
                    ErrorMessage="Vui lòng nhập mật khẩu!" ForeColor="Red" Display="Dynamic" Font-Size="12px" />
            </div>

            <div style="margin-bottom: 12px;">
                <asp:Label ID="lblThongBao" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
            </div>

            <asp:Button ID="btnDangNhap" runat="server" Text="ĐĂNG NHẬP" CssClass="btn-auth-submit" OnClick="btnDangNhap_Click" />

            <p style="text-align:center; margin-top:16px; font-size:13.5px; color:#6b7280;">
                Chưa có tài khoản khách hàng?
                <a href="DangKy.aspx" style="color:#be185d; font-weight:600;">Đăng ký ngay 🌸</a>
            </p>
        </div>
    </div>
</asp:Content>