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
    public partial class EditViaNickname : Form
    {
        public EditViaNickname()
        {
            InitializeComponent();
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            if(textBoxNicmName.Text != "")
            {
                UserProfile userToEdit = Telegram._base.Find(x => x.nickname == textBoxNicmName.Text);
                textBoxNicmName.Text = userToEdit.nickname;
                textBoxGender.Text = userToEdit.gender;
                textBoxCountry.Text = userToEdit.country;
                textBoxAge.Text = userToEdit.age;
                textBoxPicture.Text = userToEdit.photo_link;
                textBoxPostLink.Text = userToEdit.post_link;
                textBoxTelegram.Text = userToEdit.telegram;
                textBoxText.Text = userToEdit.text;
                textBoxTown.Text = userToEdit.town;
            }
        }
        private void Save()
        {
            UserProfile edited = new UserProfile();
            edited.age = textBoxAge.Text;
            edited.country = textBoxCountry.Text;
            edited.gender = textBoxGender.Text;
            edited.nickname = textBoxNicmName.Text;
            edited.photo_link = textBoxPicture.Text;
            edited.post_link = textBoxPostLink.Text;
            edited.telegram = textBoxTelegram.Text;
            edited.text = textBoxText.Text;
            edited.town = textBoxTown.Text;
            lock (Telegram._base)
            {
                Telegram._base.RemoveAll(x => x.nickname == edited.nickname);
            }
            Telegram._base.Add(edited);
            UserProfile.writeBase(Telegram._base);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}
