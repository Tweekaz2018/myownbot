using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PikaLoveBot
{
    public partial class deleteTownFromBaseForm : Form
    {
        public deleteTownFromBaseForm()
        {
            InitializeComponent();
            listBox1.Items.Add("Украина");
            listBox1.Items.Add("Россия");
            listBox1.Items.Add("Белоруссия");
            listBox1.Items.Add("Казахстан");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
                UserProfile.DeleteTownFromBase(listBox2.SelectedItem.ToString());
            this.Close();
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem.ToString() == "Украина")
                foreach (string n in UserProfile.ukr)
                    listBox2.Items.Add(n);
            if (listBox1.SelectedItem.ToString() == "Белоруссия")
                foreach (string n in UserProfile.bel)
                    listBox2.Items.Add(n);
            if (listBox1.SelectedItem.ToString() == "Казахстан")
                foreach (string n in UserProfile.kz)
                    listBox2.Items.Add(n);
            if (listBox1.SelectedItem.ToString() == "Россия")
                foreach (string n in UserProfile.rus)
                    listBox2.Items.Add(n);
        }
    }
}
