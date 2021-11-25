using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniDolarMarket
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            Model m = new Model("Mini Market");
            m.setup(int.Parse(numExchangeRate.Value.ToString()), int.Parse(numInflationRate.Value.ToString()), chkFuzzy.Checked);
            m.Graph(this.chart1);
            m.Grid(dataGridView1);
            m.run(int.Parse(numTicks.Value.ToString()));
            m.Graph(this.chart2);
            m.Grid(dataGridView2);
            
            txtResults.Text= m.reportStatistics();

        }
    }
}
