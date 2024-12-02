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
    public partial class CreateAccount : Form
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void CreateAccount_Load(object sender, EventArgs e)
        {

        }
        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(RegisterAccount()) this.Close();
        }
        bool RegisterAccount()
        {
            PCS.CustomerData data = new PCS.CustomerData();
            Random random = new Random();
            data.CustomerID = (ulong)random.Next(int.MaxValue);
            data.Username = textBox1.Text;
            data.Password = textBox2.Text;
            data.FirstName = textBox3.Text;
            data.LastName = textBox4.Text;
            data.Email = textBox5.Text;
            data.PhoneNumber = textBox6.Text;
            data.Address = textBox7.Text + "|" + textBox8.Text + "|" + textBox9.Text + "|" + textBox10.Text + "|" + textBox11.Text;

            if (textBox12.Text != String.Empty && textBox13.Text != string.Empty && textBox14.Text != string.Empty && textBox15.Text != string.Empty && textBox16.Text != string.Empty)
            {
                data.CreditCard = new PCS.CustomerData.CardInfo();
                data.CreditCard.CardHolderName = textBox12.Text;
                data.CreditCard.ExpirationDate = textBox13.Text;
                data.CreditCard.CardNum = textBox14.Text;
                data.CreditCard.CSV = textBox15.Text;
                data.CreditCard.BillingAddress = textBox16.Text;
            }
            
            return PCS.Singleton.UpdateAccountInfo(data.CustomerID, data);
                
        }
    }
}
