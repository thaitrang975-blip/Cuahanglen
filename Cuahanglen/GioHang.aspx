<%@ Page Title="" Language="C#" MasterPageFile="~/SiteLen.Master" AutoEventWireup="true" CodeBehind="GioHang.aspx.cs" Inherits="Cuahanglen.GioHang" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main style="max-width: 1140px; margin: 30px auto 60px; padding: 0 20px;">
        <div class="breadcrumb" style="margin-bottom: 18px;">
            <a href="TrangChu.aspx">Trang chủ</a> / <span>Giỏ hàng & Thanh toán</span>
        </div>

        <!-- THÔNG BÁO ĐẶT HÀNG THÀNH CÔNG -->
        <asp:Panel ID="pnlThanhCong" runat="server" Visible="false"
            style="background:#ecfdf5; border:1px solid #6ee7b7; color:#065f46; padding:24px; border-radius:12px; text-align:center; margin-bottom:24px;">
            <h3 style="margin-bottom:8px;">🎉 Đặt hàng thành công!</h3>
            <p><asp:Label ID="lblThongBaoDon" runat="server"></asp:Label></p>
            <div style="margin-top:16px;">
                <a href="TrangChu.aspx" class="btn-continue" style="display:inline-block;">Tiếp tục mua sắm 🧶</a>
            </div>
        </asp:Panel>

        <!-- KHI GIỎ HÀNG TRỐNG -->
        <asp:Panel ID="pnlGioTrong" runat="server" Visible="false"
            style="background:#fff; border:1px solid #f3e4ec; border-radius:12px; padding:50px 20px; text-align:center;">
            <span class="empty-icon">🧶</span>
            <h3 style="margin-bottom:6px;">Giỏ hàng của bạn đang trống</h3>
            <p style="color:#6b7280; font-size:14px;">Hãy chọn thêm những cuộn len xinh xắn để bắt đầu dự án nhé!</p>
            <a href="TrangChu.aspx" class="btn-continue" style="display:inline-block; margin-top:16px;">Quay lại chọn len</a>
        </asp:Panel>

        <!-- KHI GIỎ HÀNG CÓ SẢN PHẨM -->
        <asp:Panel ID="pnlCoHang" runat="server">
            <div style="display:grid; grid-template-columns: 1.6fr 1fr; gap: 24px; align-items: start;">

                <!-- CỘT TRÁI: DANH SÁCH SẢN PHẨM TRONG GIỎ -->
                <div style="background:#fff; border:1px solid #f3e4ec; border-radius:12px; padding:20px;">
                    <h3 style="color:#be185d; margin-bottom:14px;">🛒 Sản phẩm trong giỏ hàng</h3>

                    <table class="admin-table">
                        <thead>
                            <tr>
                                <th>Sản phẩm</th>
                                <th>Đơn giá</th>
                                <th>Số lượng</th>
                                <th>Thành tiền</th>
                                <th>Xóa</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptGioHang" runat="server" OnItemCommand="rptGioHang_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td style="display:flex; align-items:center; gap:10px;">
                                            <img src='<%# ResolveUrl("~/Images/" + Eval("HinhAnh").ToString().Trim()) %>'
                                                 width="54" height="54" style="object-fit:cover; border-radius:8px;" />
                                            <div>
                                                <b><%# Eval("TenSanPham") %></b><br />
                                                <small style="color:#6b7280;">Phân loại: <%# Eval("Size") %></small>
                                            </div>
                                        </td>
                                        <td><%# String.Format("{0:N0}₫", Eval("DonGia")) %></td>
                                        <td>
                                            <div class="qty-control">
                                                <asp:LinkButton ID="btnGiam" runat="server" CommandName="Giam"
                                                    CommandArgument='<%# Eval("MaSanPham") %>'
                                                    style="padding:2px 9px; font-weight:700;">−</asp:LinkButton>
                                                <span><%# Eval("SoLuong") %></span>
                                                <asp:LinkButton ID="btnTang" runat="server" CommandName="Tang"
                                                    CommandArgument='<%# Eval("MaSanPham") %>'
                                                    style="padding:2px 9px; font-weight:700;">+</asp:LinkButton>
                                            </div>
                                        </td>
                                        <td><b style="color:#be185d;"><%# String.Format("{0:N0}₫", Eval("ThanhTien")) %></b></td>
                                        <td>
                                            <asp:LinkButton ID="btnXoa" runat="server" CommandName="Xoa"
                                                CommandArgument='<%# Eval("MaSanPham") %>'
                                                CssClass="btn-sm btn-del" OnClientClick="return confirm('Xóa sản phẩm này khỏi giỏ?');">
                                                🗑️
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>

                    <div style="margin-top:16px; display:flex; justify-content:space-between; align-items:center;">
                        <a href="TrangChu.aspx" style="color:#be185d; font-weight:600; font-size:14px;">← Tiếp tục chọn thêm len</a>
                        <asp:Button ID="btnXoaHet" runat="server" Text="Xóa sạch giỏ hàng" CssClass="btn-sm btn-del"
                            OnClick="btnXoaHet_Click" CausesValidation="false"
                            OnClientClick="return confirm('Bạn muốn xóa toàn bộ giỏ hàng?');" />
                    </div>
                </div>

                <!-- CỘT PHẢI: TỔNG TIỀN & FORM THÔNG TIN ĐẶT HÀNG -->
                <div style="background:#fff; border:1px solid #f3e4ec; border-radius:12px; padding:22px;">
                    <h3 style="color:#be185d; margin-bottom:14px;">📦 Thông tin giao hàng</h3>

                    <div style="background:#fdf2f8; padding:12px; border-radius:8px; margin-bottom:16px; font-size:13.5px;">
                        <div class="summary-row">
                            <span>Tạm tính:</span>
                            <b><asp:Label ID="lblTamTinh" runat="server" Text="0₫"></asp:Label></b>
                        </div>
                        <div class="summary-row">
                            <span>Phí vận chuyển (Miễn phí từ 250k):</span>
                            <b><asp:Label ID="lblPhiShip" runat="server" Text="25,000₫"></asp:Label></b>
                        </div>
                        <div class="summary-row total-row" style="margin-bottom:0; border-top:1px dashed #f3d5e5; padding-top:8px;">
                            <span>Tổng thanh toán:</span>
                            <asp:Label ID="lblTongCong" runat="server" Text="0₫"></asp:Label>
                        </div>
                    </div>

                    <div class="form-group">
                        <label>Họ tên người nhận *</label>
                        <asp:TextBox ID="txtHoTen" runat="server" placeholder="Nhập họ tên..."></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvTen" runat="server" ControlToValidate="txtHoTen"
                            ErrorMessage="Vui lòng nhập họ tên!" ForeColor="Red" Display="Dynamic" Font-Size="12px" />
                    </div>

                    <div class="form-group">
                        <label>Số điện thoại *</label>
                        <asp:TextBox ID="txtSDT" runat="server" placeholder="09xxxxxxxx"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSDT" runat="server" ControlToValidate="txtSDT"
                            ErrorMessage="Vui lòng nhập SĐT!" ForeColor="Red" Display="Dynamic" Font-Size="12px" />
                    </div>

                    <div class="form-group">
                        <label>Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" placeholder="email@gmail.com"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Địa chỉ nhận hàng *</label>
                        <asp:TextBox ID="txtDiaChi" runat="server" placeholder="Số nhà, đường, phường/xã, tỉnh/thành..."></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDiaChi" runat="server" ControlToValidate="txtDiaChi"
                            ErrorMessage="Vui lòng nhập địa chỉ giao hàng!" ForeColor="Red" Display="Dynamic" Font-Size="12px" />
                    </div>

                    <div class="form-group">
                        <label>Phương thức thanh toán</label>
                        <asp:DropDownList ID="ddlThanhToan" runat="server">
                            <asp:ListItem Value="Thanh toán khi nhận hàng (COD)" Text="💵 Thanh toán khi nhận hàng (COD)"></asp:ListItem>
                            <asp:ListItem Value="Chuyển khoản ngân hàng" Text="🏦 Chuyển khoản ngân hàng"></asp:ListItem>
                            
                        </asp:DropDownList>
                    </div>

                    <asp:Label ID="lblLoi" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>

                    <asp:Button ID="btnDatHang" runat="server" Text="HOÀN TẤT ĐẶT HÀNG ✨"
                        CssClass="btn-checkout" style="margin-top:8px;" OnClick="btnDatHang_Click" />
                </div>

            </div>
        </asp:Panel>
    </main>
</asp:Content>