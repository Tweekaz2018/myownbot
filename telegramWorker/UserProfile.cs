using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace telegramWorker
{
    public class UserProfile
    {
        
        public static List<string> ukr = new List<string>{ "Киев", "Одесса", "Харьков", "Николаев", "Днепропетровск", "Запорожье", "Львов", "Винница" };
        public static List<string> bel = new List<string> { "Гомель", "Минск", "Гомель" };
        public static List<string> rus = new List<string> { "Челябинск", "Калининград", "Алматы", "Екатеринбург", "Краснодар", "Саратов", "Иркутск", "Самара", "Ярославль", "Воронеж", "Ростов", "Петербург", "Тольятти", "Тюмень", "Омск", "Чебоксары", "Оренбург", "Уфа", "Астрахань", "Тула", "Пермь", "Новороссийск", "Киров", "Сочи", "Казань" };

        public string nickname;
        public string photo_link;
        public string post_link;
        public string country;
        public string town;
        public string telegram;
        public string age;
        public string gender;
        public string text;
        public UserProfile(string nick, string photo, string post, string count, string to, string tel, string a, string gen, string tex)
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
/*XElement user = new XElement("user",
                        new XAttribute("nickname", a.nickname),
                        new XAttribute("photo_link", a.photo_link),
                        new XAttribute("post_link", a.post_link),
                        new XAttribute("country", a.country),
                        new XAttribute("town", a.town),
                        new XAttribute("telegram", a.telegram),
                        new XAttribute("age", a.age),
                        new XAttribute("gender", a.gender),
                        new XAttribute("text", a.text)
                        );*/