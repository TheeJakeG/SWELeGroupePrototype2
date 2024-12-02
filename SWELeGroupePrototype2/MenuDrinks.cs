using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWELeGroupePrototype2
{
    public partial class MenuDrinks : Form
    {
        public MenuDrinks()
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

        void AddMenuItem(PCS.Product item)
        {
            PCS.Singleton.C_OrderInProgress.Items.Add(item);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddMenuItem(PCS.Singleton.RetrieveProduct(ulong.Parse("0301")));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddMenuItem(PCS.Singleton.RetrieveProduct(ulong.Parse("0302")));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddMenuItem(PCS.Singleton.RetrieveProduct(ulong.Parse("0303")));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddMenuItem(PCS.Singleton.RetrieveProduct(ulong.Parse("0304")));
        }
    }
}
