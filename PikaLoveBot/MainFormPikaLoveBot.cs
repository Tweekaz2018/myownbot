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
        public static string api_key_telegram = "";
        public static stats statistic;
        public MainFormPikaLoveBot()
        {
            InitializeComponent();
            UserProfile.TownsAdder();
            Telegram.AllTownsList = Telegram.AllTownsList.Concat(UserProfile.bel).Concat(UserProfile.kz).Concat(UserProfile.rus).Concat(UserProfile.ukr).ToList();
            
            KeySelect k = new KeySelect();
            k.ShowDialog();
            Telegram.BotStart();
            this.Text = Telegram.Bot.GetMeAsync().Result.Username;
            UserProfile.cleaner();
            statistic = new stats(acs.GetUsers().Count, Telegram._base.Count, acs.GetUsers().Sum(x => Convert.ToInt32(x.countOfRequests)), Telegram.AllTownsList.Count);

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
            UserProfile.writeBase(Telegram._base);
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
            statistic = new stats(acs.GetUsers().Count, Telegram._base.Count, acs.GetUsers().Sum(x => Convert.ToInt32(x.countOfRequests)), Telegram.AllTownsList.Count);

            labelTotalInBase.Text = "Всего в базе анкет: " + statistic.acconts;
            labelUsersReq.Text = "Количество пользователей бота: " + statistic.users;
            labelRequests.Text = "Общее количество запросов: " + statistic.req;
            labelTowns.Text = "Количество городов: " + statistic.towns;
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

        private void добавитьПустойГородToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEmptyTown f = new AddEmptyTown();
            f.ShowDialog();
        }

        private void выгрузитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAll();
        }

        public static void SaveAll()
        {
            try
            {
                UserProfile.cleaner();
                UserProfile.writeBase(Telegram._base);
                Telegram.Log("Сохранено " + Telegram._base.Count + " юзеров");
            }
            catch
            {
                Telegram.Log("Не удалось сохранить базу");
            }
            try
            {
                XDocument xdoc = new XDocument();
                xdoc.Add(new XElement("base"));
                acs._acs = acs._acs.Distinct().ToList();
                foreach (acs a in acs._acs)
                {
                    xdoc.Root.Add(new XElement("user", new XAttribute("name", a.name), new XAttribute("counter", a.countOfRequests), new XAttribute("isAlive", a.hasAnket), new XAttribute("id", a.id)));
                }
                xdoc.Save("stats.xml");
            }
            catch
            {
                Telegram.Log("Не удалось выгрузить статистику");
            }
            try
            {
                UserProfile.AddTownToSettings();
            }
            catch
            {
                Telegram.Log("Не удалось сохранить города");
            }
            Telegram.Log("Выгрузка закончена");
        }

    }
}
