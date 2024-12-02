using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWELeGroupePrototype2
{
    public partial class OrderHistory : Form
    {
        public OrderHistory()
        {
            InitializeComponent();
        }
        void LoadForm(Form f)
        {
            f.Show();
            this.Close();
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(new Home());
        }

        private void orderHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(new OrderHistory());
        }

        private void orderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(new OrderSummary());
        }

        private void pizzaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(new MenuPizza());
        }

        private void breadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(new MenuBread());
        }

        private void wingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(new MenuWings());
        }

        private void saucesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(new MenuSauces());
        }

        private void dessertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(new MenuDessert());
        }

        private void drinksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(new MenuDrinks());
        }

        /// <summary>
        /// for each order, if the order belonged to the active customer then show the order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderHistory_Load(object sender, EventArgs e)
        {
            List<PCS.OrderData> orders = new List<PCS.OrderData>();

            List<string> lines = new List<string>();
            try
            {
                StreamReader sr = new StreamReader(PCS.Singleton.fp_Orders);
                while (!sr.EndOfStream) { lines.Add(sr.ReadLine()); }
                sr.Close();
            }
            catch (Exception ex) { }

            foreach (string line in lines)
            {
                if(line == String.Empty) continue;
                if(PCS.Singleton.AccountID == ulong.Parse(line.Split(',')[2])) 
                    orders.Add(PCS.Singleton.RetrieveOrder(ulong.Parse(line.Split(',')[0])));
            }

            BindingSource Source = new BindingSource();
            Source.DataSource = orders;

            dataGridView1.DataSource = Source;
            dataGridView1.AutoResizeColumns();
        }
    }
}
