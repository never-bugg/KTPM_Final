using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KTPM_Final.Controllers;

namespace App
{
    public partial class ChiTietPhieuNhap : Form
    {
        private int _maPhieuNhap;

        public ChiTietPhieuNhap(int maPhieuNhap)
        {
            InitializeComponent();
            _maPhieuNhap = maPhieuNhap;
        }

        private void ChiTietPhieuNhap_Load(object sender, EventArgs e)
        {
            LoadChiTietPhieuNhap();
        }

        private void LoadChiTietPhieuNhap()
        {
            ChiTietNhapSachController ctBus = new ChiTietNhapSachController();
            var chiTietList = ctBus.LayChiTietTheoMaPhieu(_maPhieuNhap); // DataTable hoặc List<ChiTietNhapSachDTO>
            dgvChiTietPhieuNhap.DataSource = chiTietList;
            dgvChiTietPhieuNhap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dgvChiTietPhieuNhap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
