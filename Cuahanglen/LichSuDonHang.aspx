<%@ Page Title="" Language="C#" MasterPageFile="~/SiteLen.Master" AutoEventWireup="true" CodeBehind="LichSuDonHang.aspx.cs" Inherits="Cuahanglen.LichSuDonHang" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .order-wrap {
            max-width: 1100px;
            margin: 28px auto;
            padding: 0 16px;
        }
        .order-box {
            background: #fff;
            border-radius: 12px;
            padding: 22px;
            box-shadow: 0 2px 12px rgba(0,0,0,0.06);
            margin-bottom: 22px;
            border: 1px solid #fce7f3;
        }
        .order-box h2, .order-box h3 {
            color: #9d174d;
            margin: 0 0 16px 0;
        }
        .filter-bar {
            display: flex;
            gap: 10px;
            align-items: center;
            flex-wrap: wrap;
            margin-bottom: 16px;
        }
        .filter-bar select {
            padding: 8px 12px;
            border: 1px solid #d1d5db;
            border-radius: 8px;
            font-size: 14px;
        }
        .btn-pink {
            background: #be185d;
            color: #fff;
            border: none;
            padding: 8px 16px;
            border-radius: 8px;
            cursor: pointer;
            font-size: 13.5px;
            font-weight: 600;
            text-decoration: none;
            display: inline-block;
        }
        .btn-pink:hover { background: #9d174d; }
        .btn-gray {
            background: #6b7280;
            color: #fff;
            border: none;
            padding: 7px 14px;
            border-radius: 8px;
            cursor: pointer;
            font-size: 13px;
            text-decoration: none;
            display: inline-block;
        }
        .btn-view {
            background: #2563eb;
            color: #fff;
            padding: 6px 12px;
            border-radius: 6px;
            font-size: 12.5px;
            text-decoration: none;
            display: inline-block;
            white-space: nowrap;
        }
        .btn-cancel {
            background: #dc2626;
            color: #fff;
            padding: 6px 12px;
            border-radius: 6px;
            font-size: 12.5px;
            text-decoration: none;
            display: inline-block;
            white-space: nowrap;
        }
        .order-table {
            width: 100%;
            border-collapse: collapse;
            font-size: 14px;
        }
        .order-table th {
            background: #fdf2f8;
            color: #9d174d;
            text-align: left;
            padding: 12px 10px;
            border-bottom: 2px solid #fbcfe8;
        }
        .order-table td {
            padding: 12px 10px;
            border-bottom: 1px solid #f3f4f6;
            vertical-align: middle;
        }
        .order-info-box {
            background: #fdf2f8;
            padding: 14px 16px;
            border-radius: 8px;
            margin-bottom: 16px;
            font-size: 14px;
            line-height: 1.8;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="order-wrap">

        <!-- KHUNG CHI TIẾT ĐƠN HÀNG (HIỆN KHI BẤM XEM CHI TIẾT) -->
        <asp:Panel ID="pnlChiTiet" runat="server" Visible="false" CssClass="order-box" style="border: 2px solid #fbcfe8;">
            <div style="display:flex; justify-content:space-between; align-items:center; margin-bottom:14px;">
                <h3 style="margin:0;">🧾 Chi tiết đơn hàng #<asp:Label ID="lblMaHD" runat="server"></asp:Label></h3>
                <asp:Button ID="btnDongChiTiet" runat="server" Text="✖ Đóng" CssClass="btn-gray" OnClick="btnDongChiTiet_Click" />
            </div>

            <div class="order-info-box">
                <b>Ngày đặt:</b> <asp:Label ID="lblNgayLap" runat="server"></asp:Label> &nbsp;|&nbsp;
                <b>Thanh toán:</b> <asp:Label ID="lblThanhToan" runat="server"></asp:Label> &nbsp;|&nbsp;
                <b>Trạng thái:</b> <asp:Label ID="lblTrangThaiCT" runat="server" Font-Bold="true"></asp:Label><br />
                <b>Tổng tiền:</b> <span style="color:#be185d; font-weight:700;"><asp:Label ID="lblTongTienCT" runat="server"></asp:Label></span>
            </div>

            <asp:GridView ID="gvChiTietDon" runat="server" AutoGenerateColumns="False" CssClass="order-table" GridLines="None">
                <Columns>
                    <asp:TemplateField HeaderText="Ảnh">
                        <ItemTemplate>
                            <img src='<%# LayDuongDanAnh(Eval("HinhAnh")) %>'
                                 alt='<%# Eval("TenSanPham") %>'
                                 width="48" height="48" style="object-fit:cover; border-radius:6px;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TenSanPham" HeaderText="Sản phẩm" />
                    <asp:BoundField DataField="Size" HeaderText="Phân loại" />
                    <asp:BoundField DataField="SoLuong" HeaderText="Số lượng" />
                    <asp:BoundField DataField="DonGia" HeaderText="Đơn giá" DataFormatString="{0:N0}₫" />
                    <asp:BoundField DataField="ThanhTien" HeaderText="Thành tiền" DataFormatString="{0:N0}₫" />
                </Columns>
            </asp:GridView>
        </asp:Panel>

        <!-- DANH SÁCH ĐƠN HÀNG CỦA KHÁCH -->
        <div class="order-box">
            <h2>📦 Lịch sử đơn hàng của tôi</h2>

            <div class="filter-bar">
                <label style="font-weight:600; font-size:14px;">Lọc trạng thái:</label>
                <asp:DropDownList ID="ddlTrangThai" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTrangThai_SelectedIndexChanged">
                    <asp:ListItem Value="" Text="-- Tất cả đơn hàng --"></asp:ListItem>
                    <asp:ListItem Value="Đang xác nhận" Text="🕒 Đang xác nhận"></asp:ListItem>
                    <asp:ListItem Value="Đang giao hàng" Text="🚚 Đang giao hàng"></asp:ListItem>
                    <asp:ListItem Value="Đã hoàn thành" Text="✅ Đã hoàn thành"></asp:ListItem>
                    <asp:ListItem Value="Đã hủy" Text="❌ Đã hủy"></asp:ListItem>
                </asp:DropDownList>

                <a href='<%= ResolveUrl("~/TrangChu.aspx") %>' class="btn-pink" style="margin-left:auto;">🛍️ Tiếp tục mua sắm</a>
            </div>

            <div style="margin-bottom:12px;">
                <asp:Label ID="lblThongBao" runat="server" Font-Bold="true" Font-Size="14px"></asp:Label>
            </div>

            <asp:GridView ID="gvDonHangCuaToi" runat="server" AutoGenerateColumns="False" CssClass="order-table"
                DataKeyNames="MaHoaDon" OnRowCommand="gvDonHangCuaToi_RowCommand" GridLines="None"
                EmptyDataText="Bạn chưa có đơn hàng nào.">
                <Columns>
                    <asp:BoundField DataField="MaHoaDon" HeaderText="Mã đơn" />
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
                            <div style="display:flex; gap:6px; align-items:center;">
                                <asp:LinkButton ID="btnXem" runat="server" CommandName="XemCT"
                                    CommandArgument='<%# Eval("MaHoaDon") %>' CssClass="btn-view">
                                    Xem chi tiết
                                </asp:LinkButton>

                                <asp:LinkButton ID="btnHuy" runat="server" CommandName="HuyDon"
                                    CommandArgument='<%# Eval("MaHoaDon") %>' CssClass="btn-cancel"
                                    Visible='<%# Eval("TrangThai").ToString() == "Đang xác nhận" %>'
                                    OnClientClick="return confirm('Bạn chắc chắn muốn hủy đơn hàng này?');">
                                    Hủy đơn
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

    </div>
</asp:Content>