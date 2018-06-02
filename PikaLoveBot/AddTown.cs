using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PikaLoveBot
{
    public partial class AddTown : Form
    {
        public AddTown()
        {
            InitializeComponent();
            comboBox1.Items.Add("Украина");
            comboBox1.Items.Add("Россия");
            comboBox1.Items.Add("Белоруссия");
            comboBox1.Items.Add("Казахстан");
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            Add();
        }
        private async void Add()
        {
            List<UserProfile> p = new List<UserProfile>();
            if (comboBox1.SelectedItem.ToString() == "Украина")
            {
                UserProfile.ukr.Add(textBox1.Text);
                p = await UserProfile.AddTown(textBox1.Text, "Украина");
            }
            if (comboBox1.SelectedItem.ToString() == "Казахстан")
            {
                UserProfile.kz.Add(textBox1.Text);
                p = await UserProfile.AddTown(textBox1.Text, "Казахстан");
            }
            if (comboBox1.SelectedItem.ToString() == "Россия")
            {
                UserProfile.rus.Add(textBox1.Text);
                p = await UserProfile.AddTown(textBox1.Text, "Россия");
            }

            if (comboBox1.SelectedItem.ToString() == "Белоруссия")
            {
                UserProfile.bel.Add(textBox1.Text);
                p = await UserProfile.AddTown(textBox1.Text, "Белоруссия");
            }
            Telegram._base = Telegram._base.Concat(p).ToList();
            this.Close();
            UserProfile.writeBase(Telegram._base);
            UserProfile.AddTownToSettings();
        }
    }
}
