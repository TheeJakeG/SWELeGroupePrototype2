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
    public partial class Checkout : Form
    {
        public Checkout()
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
       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Checkout_Load(object sender, EventArgs e)
        {
            PCS.CustomerData cData = PCS.Singleton.RetrieveCustomerData(PCS.Singleton.AccountID);
            string[] address = cData.Address.Split('|');
            textBox1.Text = address[0];
            textBox2.Text = address[1];
            textBox3.Text = address[2];
            textBox4.Text = address[3];
            textBox5.Text = address[4];
            textBox6.Text = cData.CreditCard.CardNum;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        void Save()
        {
            PCS.CustomerData cData = PCS.Singleton.RetrieveCustomerData(PCS.Singleton.AccountID);
            string[] address = cData.Address.Split('|');
            address[0] = textBox1.Text + "|";
            address[1] = textBox2.Text + "|";
            address[2] = textBox3.Text + "|";
            address[3] = textBox4.Text + "|";
            address[4] = textBox5.Text;
            cData.Address = "";
            foreach (string s in address) { cData.Address += s; }

            cData.CreditCard.CardNum = textBox6.Text;
            PCS.Singleton.UpdateAccountInfo(PCS.Singleton.AccountID, cData);    
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Save();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Save();
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Save();
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Save();
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Save();
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!PCS.Singleton.StoreOrderData(PCS.Singleton.C_OrderInProgress)) { return; }

            if (comboBox1.Text == "Delivery")
            {
                PCS.DeliveryOrder delivery = new PCS.DeliveryOrder();



                PCS.Singleton.C_ActiveDelivery = delivery;
            }

            PCS.Singleton.C_OrderInProgress = new PCS.OrderData();

            Home home = new Home();
            home.Show();
            this.Close();   
        }
    }
}
