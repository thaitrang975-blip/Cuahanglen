<%@ Page Title="" Language="C#" MasterPageFile="~/SiteLen.Master" AutoEventWireup="true" CodeBehind="DangKy.aspx.cs" Inherits="Cuahanglen.DangKy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="display:flex; justify-content:center; padding: 40px 16px;">
        <div class="auth-box" style="max-width: 500px;">
            <div class="auth-header">
                <div class="auth-brand">
                    <span>🌸</span>
                    <h3>Đăng ký tài khoản khách hàng</h3>
                </div>
            </div>

            <div class="form-group">
                <label>Họ và tên *</label>
                <asp:TextBox ID="txtHoTen" runat="server" placeholder="VD: Nguyễn Minh Anh"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvHoTen" runat="server" ControlToValidate="txtHoTen"
                    ErrorMessage="Họ tên không được để trống!" ForeColor="Red" Display="Dynamic" Font-Size="12px" />
            </div>

            <div class="form-row">
                <div class="form-group">
                    <label>Số điện thoại *</label>
                    <asp:TextBox ID="txtSDT" runat="server" placeholder="09xxxxxxxx"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSDT" runat="server" ControlToValidate="txtSDT"
                        ErrorMessage="Vui lòng nhập SĐT!" ForeColor="Red" Display="Dynamic" Font-Size="12px" />
                    <asp:RegularExpressionValidator ID="revSDT" runat="server" ControlToValidate="txtSDT"
                        ValidationExpression="^0\d{9}$" ErrorMessage="SĐT phải gồm 10 chữ số bắt đầu bằng 0!"
                        ForeColor="Red" Display="Dynamic" Font-Size="12px" />
                </div>

                <div class="form-group">
                    <label>Email *</label>
                    <asp:TextBox ID="txtEmail" runat="server" placeholder="email@gmail.com"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Vui lòng nhập Email!" ForeColor="Red" Display="Dynamic" Font-Size="12px" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                        ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ErrorMessage="Email không đúng định dạng!" ForeColor="Red" Display="Dynamic" Font-Size="12px" />
                </div>
            </div>

            <div class="form-group">
                <label>Địa chỉ giao hàng *</label>
                <asp:TextBox ID="txtDiaChi" runat="server" placeholder="Số nhà, đường, tỉnh/thành..."></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDiaChi" runat="server" ControlToValidate="txtDiaChi"
                    ErrorMessage="Vui lòng nhập địa chỉ!" ForeColor="Red" Display="Dynamic" Font-Size="12px" />
            </div>

            <div class="form-group">
                <label>Tên đăng nhập *</label>
                <asp:TextBox ID="txtTenDN" runat="server" placeholder="Không dấu, viết liền..."></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvTenDN" runat="server" ControlToValidate="txtTenDN"
                    ErrorMessage="Vui lòng nhập tên đăng nhập!" ForeColor="Red" Display="Dynamic" Font-Size="12px" />
            </div>

            <div class="form-row">
                <div class="form-group">
                    <label>Mật khẩu *</label>
                    <asp:TextBox ID="txtMatKhau" runat="server" TextMode="Password" placeholder="Nhập mật khẩu"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMK" runat="server" ControlToValidate="txtMatKhau"
                        ErrorMessage="Vui lòng nhập mật khẩu!" ForeColor="Red" Display="Dynamic" Font-Size="12px" />
                </div>

                <div class="form-group">
                    <label>Xác nhận mật khẩu *</label>
                    <asp:TextBox ID="txtXacNhanMK" runat="server" TextMode="Password" placeholder="Nhập lại mật khẩu"></asp:TextBox>
                    <asp:CompareValidator ID="cvMatKhau" runat="server" ControlToValidate="txtXacNhanMK"
                        ControlToCompare="txtMatKhau" ErrorMessage="Mật khẩu xác nhận không khớp!"
                        ForeColor="Red" Display="Dynamic" Font-Size="12px" />
                </div>
            </div>

            <div style="margin-bottom: 12px;">
                <asp:Label ID="lblThongBao" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
            </div>

            <asp:Button ID="btnDangKy" runat="server" Text="TẠO TÀI KHOẢN MỚI 🌸" CssClass="btn-auth-submit" OnClick="btnDangKy_Click" />

            <p style="text-align:center; margin-top:16px; font-size:13.5px; color:#6b7280;">
                Đã có tài khoản?
                <a href="DangNhap.aspx" style="color:#be185d; font-weight:600;">Đăng nhập tại đây</a>
            </p>
        </div>
    </div>
</asp:Content>