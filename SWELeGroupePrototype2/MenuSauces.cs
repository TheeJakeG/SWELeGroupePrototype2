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
    public partial class MenuSauces : Form
    {
        public MenuSauces()
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

        private void MenuSauces_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PCS.Singleton.C_OrderInProgress.Items.Add(PCS.Singleton.RetrieveProduct(ulong.Parse("0501")));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PCS.Singleton.C_OrderInProgress.Items.Add(PCS.Singleton.RetrieveProduct(ulong.Parse("0502")));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PCS.Singleton.C_OrderInProgress.Items.Add(PCS.Singleton.RetrieveProduct(ulong.Parse("0503")));
        }

    }
}
