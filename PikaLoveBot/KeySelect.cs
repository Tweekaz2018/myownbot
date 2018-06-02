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
    public partial class KeySelect : Form
    {
        public KeySelect()
        {
            InitializeComponent();
            comboBox1.Items.Add("Emottion finder bot");//327959615:AAG0sVBX8Gz-cE-WbMOtXHoCGbgoRp9ndzU
            comboBox1.Items.Add("PikaLoveBot");//596652430:AAFjpy2bjLfS2wW9i1qICXDNGgIf6HI-81o
        }

        private void KeySelect_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length < 5)
            {
                if (comboBox1.SelectedItem.ToString() == "Emottion finder bot")
                    MainFormPikaLoveBot.api_key_telegram = "327959615:AAG0sVBX8Gz-cE-WbMOtXHoCGbgoRp9ndzU";
                if (comboBox1.SelectedItem.ToString() == "PikaLoveBot")
                    MainFormPikaLoveBot.api_key_telegram = "596652430:AAFjpy2bjLfS2wW9i1qICXDNGgIf6HI-81o";
                this.Close();
            }
        }
    }
}
