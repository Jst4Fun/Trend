using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trend
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            Trend.Update();
            InitializeComponent();
            dataGridView1.DataSource = Trend.Trends;
            dataGridView1.Columns[0].Name = "Пара";
            dataGridView1.Columns[0].MinimumWidth = 40;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].Name = "% Long";
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // string link = "http://www.contoso.com/default.html";
            //string link = "https://query.yahooapis.com/v1/public/yql?q=SELECT%20*%20FROM%20data.html.cssselect%20WHERE%20url%3D'http%3A%2F%2Ffxtrade.oanda.co.uk%2Flang%2Fru%2Fanalysis%2Fopen-position-ratios'%20AND%20css%3D'.position-ratio-list'&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";

            //float response = Trend.Long(@"XAG/USD"); //GetResponse(link);
            //MessageBox.Show(@"XAG/USD - " + response.ToString());
            Trend.PrintAll();
        }
    }
}
