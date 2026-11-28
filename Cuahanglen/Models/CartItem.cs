using System;

namespace Cuahanglen.Models
{
    [Serializable]
    public class CartItem
    {
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string HinhAnh { get; set; }
        public string Size { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien
        {
            get { return DonGia * SoLuong; }
        }
    }
}