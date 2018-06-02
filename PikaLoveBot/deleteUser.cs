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
    public partial class deleteUser : Form
    {
        public deleteUser()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var i = from x in Telegram._base
                    where x.nickname == textBox1.Text
                    select x;
            button1.Visible = false;
            label2.ForeColor = Color.Red;
            if (i.Count() != 0)
            {
                label2.Text = "Valid";
                button1.Visible = true;
                label2.ForeColor = Color.Green;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var i = from x in Telegram._base
                    where x.nickname == textBox1.Text
                    select x;
            UserProfile u = i.First();
            Telegram._base.Remove(u);
            UserProfile.writeBase(Telegram._base);
            this.Close();
        }
    }
}
