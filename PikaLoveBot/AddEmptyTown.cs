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
    public partial class AddEmptyTown : Form
    {
        public AddEmptyTown()
        {
            InitializeComponent();
            listBox1.Items.Add("Россия");
            listBox1.Items.Add("Украина");
            listBox1.Items.Add("Белоруссия");
            listBox1.Items.Add("Казахстан");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 2)
                return;
            switch (listBox1.SelectedItem.ToString())
            {
                case "Россия":
                    UserProfile.rus.Add(textBox1.Text);
                    break;
                case "Украина":
                    UserProfile.ukr.Add(textBox1.Text);
                    break;
                case "Казахстан":
                    UserProfile.kz.Add(textBox1.Text);
                    break;
                case "Белоруссия":
                    UserProfile.bel.Add(textBox1.Text);
                    break;
            }
            UserProfile.AddTownToSettings();
            MainFormPikaLoveBot.statistic.towns++;
            this.Close();
        }
    }
}
