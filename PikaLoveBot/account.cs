using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PikaLoveBot
{
    public class acs
    {
        public string name = "";
        public string countOfRequests = "";
        public bool hasAnket = false;
        public long id = 0;
        public static List<acs> _acs = new List<acs>();
        public static void AddUserToStats(string name, long i)
        {
            List<acs> a = _acs;
            if (a.Exists(x => x.name == name))
            {
                acs addCount = a.Find(x => x.name == name);
                a.Remove(addCount);
                addCount.countOfRequests = (Convert.ToInt32(addCount.countOfRequests) + 1).ToString();
                addCount.id = i;
                if (Telegram._base.Exists(x => x.nickname == name))
                    addCount.hasAnket = true;
                a.Add(addCount);
            }
            else
            {
                acs _new = new acs();
                _new.name = name;
                _new.id = i;
                _new.countOfRequests = "0";
                if (Telegram._base.Exists(x => x.nickname == name))
                    _new.hasAnket = true;
                a.Add(_new);
            }
            _acs = a;
            Save();
        }
        public static void Save()
        {                XDocument xdoc = new XDocument();

            try
            {
                xdoc.Add(new XElement("base"));
                foreach (acs a in acs._acs)
                {
                    if (a.name == null)
                        a.name = "Имя не определено";
                    xdoc.Root.Add(new XElement("user", new XAttribute("name", a.name), new XAttribute("counter", a.countOfRequests), new XAttribute("isAlive", a.hasAnket), new XAttribute("id", a.id)));
                }
                xdoc.Save("stats.xml");
            }
            catch
            {
                foreach (acs a in acs._acs)
                {
                    xdoc.Root.Add(new XElement("user", new XAttribute("name", a.name), new XAttribute("counter", a.countOfRequests), new XAttribute("isAlive", a.hasAnket), new XAttribute("id", a.id)));
                }
                Telegram.SendMessageToUser(170793849, "Ошибка сохранения\r\n" + xdoc.ToString());
            }
        }
        public static List<acs> GetUsers()
        {
            List<acs> us = new List<acs>();
            if (_acs.Count != 0)
                us = _acs;
            else
                us = GetFromFile();
            return us;
        }
        public static List<acs> GetFromFile()
        {
            XDocument xdoc = XDocument.Load("stats.xml");
            List<acs> us = new List<acs>();
            foreach (XElement x in xdoc.Root.Elements())
            {
                acs u = new acs();
                u.name = x.Attribute("name").Value;
                u.id = Convert.ToInt64(x.Attribute("id").Value);
                u.countOfRequests = x.Attribute("counter").Value;
                if (x.Attribute("isAlive").Value == "true")
                    u.hasAnket = true;
                us.Add(u);
            }
            _acs = us;
            return us;
        }
    }
}
