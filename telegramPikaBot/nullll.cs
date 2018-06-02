using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace telegramPikaBot
{
    public partial class nullll : Form
    {
        public static List<User> users = new List<User>();
        static int i = 0;
        public nullll()
        {
            InitializeComponent();
            XDocument xdoc = XDocument.Load("base-last.xml");
            foreach (XElement x in xdoc.Element("base").Elements())
                users.Add(new User(x.Attribute("nickname").Value,
                   x.Attribute("photo_link").Value,
                   x.Attribute("post_link").Value,
                   x.Attribute("country").Value,
                   x.Attribute("town").Value,
                   x.Attribute("telegram").Value,
                   x.Attribute("age").Value,
                   x.Attribute("gender").Value,
                   x.Attribute("text").Value));
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            List<User> nahui = new List<User>();
            foreach(User r in users)
            {
                if (r.nickname == "ads" || r.nickname == "NonGradaUser" || r.nickname == "В контакте" || r.nickname == "pikabu" || r.nickname == "" || r.nickname == "LifeIsFreedom" || r.nickname == "ramzesssvu" || r.nickname == "oblakoed")
                    nahui.Add(r);
            }
            foreach (User r in nahui)
                users.RemoveAll(x => x.nickname == r.nickname);
            MessageBox.Show(users.Count.ToString());
            var telegram = from us in users
                           where us.telegram != ""
                           select us;
            label2.Text = "Telegram:\r\n" + telegram.Count();
            label1.Text = "Total:\r\n" + users.Count();
            var photos = from us in users
                           where us.photo_link != ""
                           select us;
            label3.Text = "With photo: \r\n" + photos.Count();
            XDocument xdoc1 = new XDocument();
            xdoc1.Add(new XElement("base"));
            users = users.Distinct().ToList();
            foreach (User u in users)
            {
                XElement x = new XElement("user",
                    new XAttribute("nickname", u.nickname),
                    new XAttribute("photo_link", u.photo_link),
                    new XAttribute("post_link", u.post_link),
                    new XAttribute("country", u.country),
                    new XAttribute("town", u.town),
                    new XAttribute("telegram", u.telegram),
                    new XAttribute("age", u.age),
                    new XAttribute("gender", u.gender),
                    new XAttribute("text", u.text)
                    );
                xdoc1.Element("base").Add(x);
            }
            xdoc1.Save("withoutnull.xml");
            Next(users[0]);
        }
        private void Next(User user)
        {
            if (user.photo_link != "")
                pictureBox1.LoadAsync(user.photo_link);
            else
                pictureBox1.Image = null;
            label4.Text = "Nick: " + user.nickname;
            label6.Text = "Age: " + user.age + "/" + user.gender;
            label5.Text = "Telegram: " + user.telegram;
            label7.Text = "Town:" + user.town;
            label8.Text = "Number " + i;
            textBox1.Text = user.text;
            label9.Text = "false";
            if (user.photo_link != "")
                label9.Text = "true";
            i++;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (i < users.Count)
                Next(users[i]);
            else
            {
                MessageBox.Show("Ended and saved\r\nTotal count: " + users.Count);
                XDocument xdoc = new XDocument();
                xdoc.Add(new XElement("base"));
                foreach (User u in users)
                {
                    XElement x = new XElement("user",
                        new XAttribute("nickname", u.nickname),
                        new XAttribute("photo_link", u.photo_link),
                        new XAttribute("post_link", u.post_link),
                        new XAttribute("country", u.country),
                        new XAttribute("town", u.town),
                        new XAttribute("telegram", u.telegram),
                        new XAttribute("age", u.age),
                        new XAttribute("gender", u.gender),
                        new XAttribute("text", u.text)
                        );
                    xdoc.Element("base").Add(x);
                }
                xdoc.Save("new_base.xml");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            users.RemoveAt(i - 1);
            Next(users[i]);
        }
    }
    public class User
    {
        public string nickname;
        public string photo_link;
        public string post_link;
        public string country;
        public string town;
        public string telegram;
        public string age;
        public string gender;
        public string text;
        public User(string nick, string photo, string post, string count, string to, string tel, string a, string gen, string tex)
        {
            nickname = nick;
            photo_link = photo;
            post_link = post;
            country = count;
            town = to;
            telegram = tel;
            age = a;
            gender = gen;
            text = tex;
        }
    }
}
