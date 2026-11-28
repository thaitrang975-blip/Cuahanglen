<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="QuanLySanPham.aspx.cs" Inherits="Cuahanglen.Admin.QuanLySanPham" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- THỐNG KÊ NHANH -->
    <div class="stat-grid">
        <div class="stat-card">
            <small>Tổng số sản phẩm</small>
            <h2><asp:Label ID="lblTongSP" runat="server" Text="0"></asp:Label></h2>
        </div>
        <div class="stat-card">
            <small>Tổng số đơn hàng</small>
            <h2><asp:Label ID="lblTongDon" runat="server" Text="0"></asp:Label></h2>
        </div>
        <div class="stat-card">
            <small>Doanh thu đơn hoàn thành</small>
            <h2><asp:Label ID="lblDoanhThu" runat="server" Text="0₫"></asp:Label></h2>
        </div>
    </div>

    <!-- FORM THÊM / SỬA SẢN PHẨM -->
    <div class="card-box">
        <h3>📦 Thêm mới / Cập nhật sản phẩm len</h3>

        <div class="form-grid">
            <div class="field-group">
                <label>Mã sản phẩm *</label>
                <asp:TextBox ID="txtMaSP" runat="server" placeholder="VD: SP29"></asp:TextBox>
            </div>
            <div class="field-group">
                <label>Tên sản phẩm *</label>
                <asp:TextBox ID="txtTenSP" runat="server" placeholder="VD: Len Milk Cotton 125g"></asp:TextBox>
            </div>
            <div class="field-group">
                <label>Danh mục *</label>
                <asp:DropDownList ID="ddlDanhMuc" runat="server"></asp:DropDownList>
            </div>

            <div class="field-group">
                <label>Màu sắc</label>
                <asp:TextBox ID="txtMauSac" runat="server" placeholder="VD: Hồng pastel"></asp:TextBox>
            </div>
            <div class="field-group">
                <label>Giá nhập (VNĐ) *</label>
                <asp:TextBox ID="txtGiaNhap" runat="server" TextMode="Number" placeholder="20000"></asp:TextBox>
            </div>
            <div class="field-group">
                <label>Giá bán (VNĐ) *</label>
                <asp:TextBox ID="txtGiaBan" runat="server" TextMode="Number" placeholder="35000"></asp:TextBox>
            </div>

            <div class="field-group">
                <label>Phân loại / Size kho</label>
                <asp:TextBox ID="txtSize" runat="server" Text="Cuộn 50g"></asp:TextBox>
            </div>
            <div class="field-group">
                <label>Số lượng tồn kho</label>
                <asp:TextBox ID="txtSoLuongTon" runat="server" TextMode="Number" Text="50"></asp:TextBox>
            </div>
            <div class="field-group">
                <label>Chọn ảnh sản phẩm</label>
                <asp:FileUpload ID="fuHinhAnh" runat="server" />
            </div>
        </div>

        <div class="field-group" style="margin-top: 12px;">
            <label>Mô tả ngắn</label>
            <asp:TextBox ID="txtMoTa" runat="server" placeholder="Mô tả chất liệu, công dụng sản phẩm..."></asp:TextBox>
        </div>

        <div style="margin-top: 10px;">
            <asp:Label ID="lblThongBao" runat="server" Font-Bold="true" Font-Size="13.5px"></asp:Label>
        </div>

        <div class="btn-bar">
            <asp:Button ID="btnThem" runat="server" Text="➕ Thêm sản phẩm" CssClass="btn-primary" OnClick="btnThem_Click" />
            <asp:Button ID="btnCapNhat" runat="server" Text="💾 Lưu cập nhật" CssClass="btn-warning" OnClick="btnCapNhat_Click" />
            <asp:Button ID="btnLamMoi" runat="server" Text="🔄 Làm mới form" CssClass="btn-secondary" OnClick="btnLamMoi_Click" />
        </div>
    </div>

    <!-- DANH SÁCH SẢN PHẨM HIỆN CÓ -->
    <div class="card-box">
        <h3>📋 Danh sách sản phẩm trong kho</h3>
        <asp:GridView ID="gvSanPham" runat="server" AutoGenerateColumns="False" CssClass="gv-table"
            DataKeyNames="MaSanPham" OnRowCommand="gvSanPham_RowCommand" GridLines="None">
            <Columns>
                <asp:BoundField DataField="MaSanPham" HeaderText="Mã SP" />
                <asp:TemplateField HeaderText="Ảnh">
                    <ItemTemplate>
                        <img src='<%# ResolveUrl("~/Images/" + Eval("HinhAnh").ToString().Trim()) %>'
                             width="48" height="48" style="object-fit:cover; border-radius:6px;" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TenSanPham" HeaderText="Tên sản phẩm" />
                <asp:BoundField DataField="TenDanhMuc" HeaderText="Danh mục" />
                <asp:BoundField DataField="GiaBan" HeaderText="Giá bán" DataFormatString="{0:N0}₫" />
                <asp:BoundField DataField="SoLuongTon" HeaderText="Tồn kho" />
                <asp:BoundField DataField="DaBan" HeaderText="Đã bán" />
                <asp:TemplateField HeaderText="Thao tác">
                    <ItemTemplate>
                        <div style="display:flex; gap:6px; align-items:center;">
                            <asp:LinkButton ID="btnChonSua" runat="server" CommandName="ChonSua"
                                CommandArgument='<%# Eval("MaSanPham") %>' CssClass="btn-edit">Sửa</asp:LinkButton>
                            <asp:LinkButton ID="btnXoa" runat="server" CommandName="XoaSP"
                                CommandArgument='<%# Eval("MaSanPham") %>' CssClass="btn-danger"
                                OnClientClick="return confirm('Bạn chắc chắn muốn xóa sản phẩm này?');">Xóa</asp:LinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>