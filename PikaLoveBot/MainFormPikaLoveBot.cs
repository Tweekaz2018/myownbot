using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PikaLoveBot
{
    public partial class MainFormPikaLoveBot : Form
    {
        public MainFormPikaLoveBot()
        {
            InitializeComponent();
            UserProfile.TownsAdder();
            Telegram.BotStart();
            update();
        }
        public void up()
        {
            labelTowns.Text = (UserProfile.ukr.Count + UserProfile.rus.Count + UserProfile.bel.Count).ToString();
            labelTotalInBase.Text = Telegram._base.Count.ToString();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Telegram.Bot.StopReceiving();
            XDocument xdoc = new XDocument();
            xdoc.Add(new XElement("base"));
            foreach (acs a in acs._acs)
            {
                xdoc.Root.Add(new XElement("user", new XAttribute("name", a.name), new XAttribute("counter", a.countOfRequests), new XAttribute("isAlive", a.hasAnket), new XAttribute("id", a.id)));
            }
            xdoc.Save("stats.xml");
        }

        private void добавитьГородToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTown t = new AddTown();
            t.Show();
        }

        private void полностьюОбновитьБазуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(baseUpdate.creatBase));
            t.Start();
            MessageBox.Show("Started", "Message");
        }

        private void удалитьЮзераToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteUser delete = new deleteUser();
            delete.ShowDialog();
        }

        private void удалитьГородToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteTownFromBaseForm d = new deleteTownFromBaseForm();
            d.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = new StreamReader("log.txt").ReadToEnd();
            update();
        }
        private void update()
        {
            labelTotalInBase.Text = "Всего в базе анкет: " + Telegram._base.Count.ToString();
            labelUsersReq.Text = "Количество пользователей бота: " + acs.GetUsers().Count.ToString();
            labelRequests.Text = "Общее количество запросов: " + acs.GetUsers().Sum(x => Convert.ToInt32(x.countOfRequests)).ToString();
            labelTowns.Text = "Количество городов" + (UserProfile.bel.Count + UserProfile.ukr.Count + UserProfile.rus.Count + UserProfile.kz.Count).ToString();
        }
        private void статистикаПоГородамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter("TownStats.txt", false);
            foreach (string t in UserProfile.bel)
            {
                string data = "В городе " + t + " " + Telegram._base.FindAll(x => x.town == t && x.gender == "female").Count + " девушек и " + Telegram._base.FindAll(x => x.town == t && x.gender == "male").Count + " парней";
                sw.WriteLine(data);
            }
            foreach (string t in UserProfile.kz)
            {
                string data = "В городе " + t + " " + Telegram._base.FindAll(x => x.town == t && x.gender == "female").Count + " девушек и " + Telegram._base.FindAll(x => x.town == t && x.gender == "male").Count + " парней";
                sw.WriteLine(data);
            }
            foreach (string t in UserProfile.ukr)
            {
                string data = "В городе " + t + " " + Telegram._base.FindAll(x => x.town == t && x.gender == "female").Count + " девушек и " + Telegram._base.FindAll(x => x.town == t && x.gender == "male").Count + " парней";
                sw.WriteLine(data);
            }
            foreach (string t in UserProfile.rus)
            {
                string data = "В городе " + t + " " + Telegram._base.FindAll(x => x.town == t && x.gender == "female").Count + " девушек и " + Telegram._base.FindAll(x => x.town == t && x.gender == "male").Count + " парней";
                sw.WriteLine(data);
            }
            sw.Close();
            MessageBox.Show("Отчет статистики по городам подготовлен", "Закончено");
        }

        private void поНикуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditViaNickname ed = new EditViaNickname();
            ed.ShowDialog();
        }
    }
}
