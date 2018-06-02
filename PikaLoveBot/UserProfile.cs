using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot.Types;

namespace PikaLoveBot
{
    public class UserProfile
    {
        public static List<string> ukr = new List<string> { "Киев", "Одесса", "Харьков", "Николаев", "Днепропетровск", "Запорожье", "Львов", "Винница" };
        public static List<string> bel = new List<string> { "Гомель", "Минск", "Гомель" };
        public static List<string> rus = new List<string> { "Челябинск", "Калининград", "Алматы", "Екатеринбург", "Краснодар", "Саратов", "Иркутск", "Самара", "Ярославль", "Воронеж", "Ростов", "Петербург", "Тольятти", "Тюмень", "Омск", "Чебоксары", "Оренбург", "Уфа", "Астрахань", "Тула", "Пермь", "Новороссийск", "Киров", "Сочи", "Казань" };
        public static List<string> kz = new List<string>();

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
        public UserProfile()
        {

        }
        public async static Task<List<UserProfile>> AddTown(string town, string country)
        {
            List<UserProfile> ac = new List<UserProfile>();
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.Default;            
                List<string> pages = new List<string>();
                for (int i = 1; ; i++)
                {
                    string html = "";
                    try
                    {
                        html = wc.DownloadString("https://pikabu.ru/community/liga_znakomstv/search?q=" + town + "&page=" + i + "&n=4");
                        string ispattern = "<section class=\"stories-feed__message stories-feed__message_show\">.*</section>";
                        Regex x = new Regex(ispattern, RegexOptions.IgnoreCase);
                        //if (x.IsMatch(html)) break;
                        string pattern = "https://pikabu.ru/story/.*\" target=\"_blank\"";
                        Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                        MatchCollection mc = r.Matches(html);
                        if (mc.Count < 5) break;
                        if (mc.Count != 0 && mc.Count != 1)
                        {
                            foreach (Match m in mc)
                            {
                                foreach (string l in m.Value.Split('"'))
                                {
                                    if (l.StartsWith("http")) pages.Add(l);
                                }
                            }
                        }
                    }
                    catch { }
                }
                pages = pages.Distinct().ToList();
                foreach (string page in pages)
                {
                try
                {
                    Stopwatch s = new Stopwatch();
                    s.Start();
                    string a = wc.DownloadString(page);
                    string text = "";
                    string pattern = "<div class=\"story-block story-block_type_text\">\\s*(<p>.*)";
                    Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                    MatchCollection mc = r.Matches(a);
                    foreach (Match ma in mc)
                    {
                        text += ma.Groups[1].Value;
                    }
                    pattern = "<a href=\"(.*.jpg)\" target";
                    r = new Regex(pattern, RegexOptions.IgnoreCase);
                    Match m = r.Match(a);
                    string img = m.Groups[1].Value;
                    string teleg = "";
                    foreach (string teg in text.Split(' '))
                    {
                        if (teg.StartsWith("@"))
                        {
                            teleg = teg;
                            teleg = teleg.Replace("</p>", "");
                            break;
                        }
                    }
                    pattern = "@.*\">(.*)</a";
                    r = new Regex(pattern, RegexOptions.IgnoreCase);
                    m = r.Match(a);
                    string nick = m.Groups[1].Value;
                    if (nick == "ads" || nick == "NonGradaUser" || nick == "В контакте" || nick == "pikabu" || nick == "" || nick == "LifeIsFreedom" || nick == "ramzesssvu" || nick == "oblakoed" || nick == ""|| nick == "@pikabu")
                        continue;
                    string[] arr = await getAgeGender(img);
                    UserProfile ak = null;
                    if (img != "")
                    {
                        Thread.Sleep(21000);
                        ak = new UserProfile(nick, img, "", country, town, teleg, arr[0], arr[1], text);
                        ac.Add(ak);
                    }

                }
                catch
                {

                }
                }
            System.Windows.Forms.MessageBox.Show(ac.Count + " анкет добавлено в " + country + "/" + town);
                
            return ac;
        }
        public static void writeBase(List<UserProfile> list)
        {
            XDocument xdoc = new XDocument();
            xdoc.Add(new XElement("base"));
            foreach (UserProfile a in list)
            {
                if (a.nickname.StartsWith("@")) a.nickname = a.nickname.Substring(1);
                if (a.age == "" || a.age == null)
                {
                    a.age = "";
                    a.gender = "";
                    XElement user = new XElement("user",
                        new XAttribute("nickname", a.nickname),
                        new XAttribute("photo_link", a.photo_link),
                        new XAttribute("post_link", a.post_link),
                        new XAttribute("country", a.country),
                        new XAttribute("town", a.town),
                        new XAttribute("telegram", a.telegram),
                        new XAttribute("age", a.age),
                        new XAttribute("gender", a.gender),
                        new XAttribute("text", a.text)
                        );
                    xdoc.Root.Add(user);
                    continue;
                }
                if (a.age != ""|| a.age != null)
                {
                    XElement user = new XElement("user",
                        new XAttribute("nickname", a.nickname),
                        new XAttribute("photo_link", a.photo_link),
                        new XAttribute("post_link", a.post_link),
                        new XAttribute("country", a.country),
                        new XAttribute("town", a.town),
                        new XAttribute("telegram", a.telegram),
                        new XAttribute("age", a.age),
                        new XAttribute("gender", a.gender),
                        new XAttribute("text", a.text)
                        );
                    xdoc.Root.Add(user);
                }
            }
            Random r = new Random();
            System.IO.File.Copy("base-last.xml", "base-" + DateTime.Now.Day + "-" + DateTime.Now.Year + "-" + r.Next(999, 9999) + ".xml");
            xdoc.Save("base-last.xml");
        }
        public async void getAgeGenderFromTelegramImage(Message message)
        {
            string subKey = "885d08300f7f4202a4f62edd151f3810";
            string uriBase = "https://eastus2.api.cognitive.microsoft.com/face/v1.0/detect";
            WebClient wc = new WebClient();
            var file = await Telegram.Bot.GetFileAsync(message.Photo.LastOrDefault()?.FileId);
            var filename = file.FileId + "." + file.FilePath.Split('.').Last();
                using (var saveImageStream = System.IO.File.Open("photo.jpg", FileMode.Create))
                {
                    await Telegram.Bot.DownloadFileAsync(file.FilePath, saveImageStream);
                }            
                try
                {
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subKey);
                    string queryString = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender";
                    string uri = uriBase + "?" + queryString;

                    HttpResponseMessage response;
                    string JSONString;

                    byte[] b = GetImageAsByteArray("photo.jpg");
                    System.IO.File.Delete("photo.jpg");
                    using (var content = new ByteArrayContent(b))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response = await client.PostAsync(uri, content);
                        JSONString = await response.Content.ReadAsStringAsync();
                        JSONString = JSONString.Replace("[", "");
                        JSONString = JSONString.Replace("]", "");
                        if (JSONString.Length > 20)
                        {
                            JObject _json = new JObject();
                            _json = JObject.Parse(JSONString);
                            age = (string)_json["faceAttributes"]["age"];
                            gender = (string)_json["faceAttributes"]["gender"];
                        }
                    }
                }
                catch (Exception ex)
                {
                }            
        }
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            byte[] b = binaryReader.ReadBytes((int)fileStream.Length);
            fileStream.Close();
            return b;
        }
        static async Task<string[]> getAgeGender(string img)
        {
            string subKey = "885d08300f7f4202a4f62edd151f3810";
            string uriBase = "https://eastus2.api.cognitive.microsoft.com/face/v1.0/detect";
            WebClient wc = new WebClient();
            string[] array = new string[2];
            if (img != "")
            {
                try
                {
                    wc.DownloadFile(img, "photo.jpg");
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subKey);
                    string queryString = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender";
                    string uri = uriBase + "?" + queryString;

                    HttpResponseMessage response;
                    string JSONString;

                    byte[] b = GetImageAsByteArray("photo.jpg");
                    System.IO.File.Delete("photo.jpg");
                    using (var content = new ByteArrayContent(b))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response = await client.PostAsync(uri, content);
                        JSONString = await response.Content.ReadAsStringAsync();
                        JSONString = JSONString.Replace("[", "");
                        JSONString = JSONString.Replace("]", "");
                        if (JSONString.Length > 20)
                        {
                            JObject _json = new JObject();
                            _json = JObject.Parse(JSONString);
                            array[0] = (string)_json["faceAttributes"]["age"];
                            array[1] = (string)_json["faceAttributes"]["gender"];
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return array;
        }
        public static void TownsAdder()
        {
            XDocument xdoc = XDocument.Load("settings.xml");
            UserProfile.bel = xdoc.Root.Elements().Where(x => x.Attribute("name").Value == "Белоруссия").First().Attribute("towns").Value.Split(',').ToList();
            UserProfile.ukr = xdoc.Root.Elements().Where(x => x.Attribute("name").Value == "Украина").First().Attribute("towns").Value.Split(',').ToList();
            UserProfile.rus = xdoc.Root.Elements().Where(x => x.Attribute("name").Value == "Россия").First().Attribute("towns").Value.Split(',').ToList();
            UserProfile.kz = xdoc.Root.Elements().Where(x => x.Attribute("name").Value == "Казахстан").First().Attribute("towns").Value.Split(',').ToList();
        }

        public static void AddTownToSettings()
        {
            XDocument xdoc = new XDocument();
            xdoc.Add(new XElement("base"));
            string towns = "";
            foreach (string ukr in UserProfile.ukr)
            {
                towns += ukr + ",";
            }
            towns = towns.Substring(0, towns.Length - 1);
            XElement x = new XElement("country", new XAttribute("name", "Украина"), new XAttribute("towns", towns));
            xdoc.Root.Add(x);
            towns = null;
            foreach (string kz in UserProfile.kz)
            {
                towns += kz + ",";
            }
            towns = towns.Substring(0, towns.Length - 1);
            x = new XElement("country", new XAttribute("name", "Казахстан"), new XAttribute("towns", towns));
            xdoc.Root.Add(x);
            towns = null;
            foreach (string rus in UserProfile.rus)
            {
                towns += rus + ",";
            }
            towns = towns.Substring(0, towns.Length - 1);
            x = null;
            x = new XElement("country", new XAttribute("name", "Россия"), new XAttribute("towns", towns));
            xdoc.Root.Add(x);
            towns = null;
            foreach (string bel in UserProfile.bel)
            {
                towns += bel + ",";
            }
            towns = towns.Substring(0, towns.Length - 1);
            x = null;
            x = new XElement("country", new XAttribute("name", "Белоруссия"), new XAttribute("towns", towns));
            xdoc.Root.Add(x);
            StreamWriter sw = new StreamWriter("settings.xml", false, Encoding.UTF8);
            sw.Write(xdoc);
            sw.Close();
        }
        public static void DeleteTownFromBase(string town)
        {
            Telegram._base.RemoveAll(x => x.town == town);
            
            if (UserProfile.ukr.Contains(town)) UserProfile.ukr.Remove(town);
            if (UserProfile.rus.Contains(town)) UserProfile.rus.Remove(town);
            if (UserProfile.bel.Contains(town)) UserProfile.bel.Remove(town);
            if (UserProfile.kz.Contains(town)) UserProfile.kz.Remove(town);
            AddTownToSettings();
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