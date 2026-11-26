<%@ Page Title="" Language="C#" MasterPageFile="~/SiteLen.Master" AutoEventWireup="true" CodeBehind="TrangChu.aspx.cs" Inherits="Cuahanglen.TrangChu" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- HERO BANNER -->
    <section class="hero-section">
        <div class="hero-container">
            <div class="hero-banner">
                <div class="breadcrumb">
                    <a href="TrangChu.aspx">Trang chủ</a> / <span>Len Sợi & Dụng Cụ Đan Móc</span>
                </div>
                <div class="banner-card">
                    <img src="Images/nentiemlen.jpg" alt="Banner Tiệm Len Sợi" />
                    <div class="banner-overlay">
                        <span class="tag">BỘ SƯU TẬP MỚI</span>
                        <h1>Thế Giới Len Sợi Handmade Dễ Thương</h1>
                        <p>Nguyên liệu & Phụ kiện đan móc cao cấp nhập khẩu chính hãng</p>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- NỘI DUNG CHÍNH: BỘ LỌC + DANH SÁCH SẢN PHẨM -->
    <main class="container" id="productSection">
        <!-- SIDEBAR LỌC NHANH -->
        <aside class="sidebar">
            <div class="filter-card">
                <h3 class="filter-title">DANH MỤC SẢN PHẨM</h3>
                <div class="filter-group">
                    <ul class="styled-list" style="list-style:none; padding-left:0;">
                        <li style="margin-bottom:8px;"><a href="TrangChu.aspx">🌸 Tất cả sản phẩm</a></li>
                        <li style="margin-bottom:8px;"><a href="TrangChu.aspx?dm=DM01">🧶 Len Sợi Các Loại</a></li>
                        <li style="margin-bottom:8px;"><a href="TrangChu.aspx?dm=DM02">🪝 Kim Đan & Kim Móc</a></li>
                        <li style="margin-bottom:8px;"><a href="TrangChu.aspx?dm=DM03">🧸 Phụ Kiện Thú Bông</a></li>
                        <li style="margin-bottom:8px;"><a href="TrangChu.aspx?dm=DM04">💐 Sản Phẩm Thành Phẩm</a></li>
                    </ul>
                </div>
                <a href="TrangChu.aspx" class="btn-reset-filter" style="display:block; text-align:center;">Xem tất cả</a>
            </div>
        </aside>

        <!-- LƯỚI SẢN PHẨM ĐỔ TỪ SQL SERVER -->
        <section class="main-content">
            <div class="content-header">
                <div>
                    <h2><asp:Label ID="lblTieuDe" runat="server" Text="Len Sợi & Sản Phẩm Đan Móc"></asp:Label></h2>
                    <span class="product-count">
                        <asp:Label ID="lblSoLuongSP" runat="server" Text="Hiển thị 0 sản phẩm"></asp:Label>
                    </span>
                </div>
                <div class="sort-wrapper">
                    <label>Sắp xếp:</label>
                    <asp:DropDownList ID="ddlSapXep" runat="server" CssClass="sort-dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlSapXep_SelectedIndexChanged">
                        <asp:ListItem Value="default" Text="Mới nhất / Nổi bật"></asp:ListItem>
                        <asp:ListItem Value="asc" Text="Giá: Thấp đến Cao"></asp:ListItem>
                        <asp:ListItem Value="desc" Text="Giá: Cao đến Thấp"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <!-- REPEATER HIỂN THỊ SẢN PHẨM -->
            <div class="product-grid">
                <asp:Repeater ID="rptSanPham" runat="server">
                    <ItemTemplate>
                        <div class="product-card">
                            <div class="product-thumb">
                                <img src='<%# ResolveUrl("~/Images/" + Eval("HinhAnh")) %>' alt='<%# Eval("TenSanPham") %>' />
                                <asp:LinkButton ID="btnThemGio" runat="server" CssClass="quick-add-btn"
                                    CommandArgument='<%# Eval("MaSanPham") %>' OnClick="btnThemGio_Click">
                                    Thêm giỏ hàng 🛒
                                </asp:LinkButton>
                            </div>
                            <div class="product-info">
                                <h4 class="product-title"><%# Eval("TenSanPham") %></h4>
                                <div class="product-price">
                                    <span class="price-new"><%# String.Format("{0:N0}₫", Eval("GiaBan")) %></span>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </section>
    </main>

</asp:Content>