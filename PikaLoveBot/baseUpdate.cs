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

namespace PikaLoveBot
{
    class baseUpdate
    {
        static List<account> accounts = new List<account>();
        static Stopwatch q = new Stopwatch();

        public static void creatBase()
        {
            accounts = accounts.Concat(t(UserProfile.bel, "Белоруссия")).ToList();

            accounts = accounts.Concat(t(UserProfile.kz, "Казахстан")).ToList();
            accounts = accounts.Concat(t(UserProfile.ukr, "Украина")).ToList();
            accounts = accounts.Concat(t(UserProfile.rus, "Россия")).ToList();
        }
        public static List<account> t(List<string> towns, string country)
        {
            List<account> ac = new List<account>();

            WebClient wc = new WebClient();
            wc.Encoding = Encoding.Default;
            foreach (string town in towns)
            {
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
                        if (nick != "pikabu" || nick != "pikabu" || nick != "" || page != "https://pikabu.ru/story/dofeniie__i_sny_dvodi_prhitnloolndenv_oiy__pr_2931188")
                        {
                            account ak = null;
                            if (img != "")
                            {
                                Thread.Sleep(21000);
                                ak = new account(town, country, nick, teleg, text, img, page);
                            }
                            else
                            {

                                ak = new account(town, country, nick, teleg, text, page);
                            }
                            ac.Add(ak);
                            //            Console.WriteLine("Ended\r\n\r\nTime: " + s.Elapsed.Hours + ":" + s.Elapsed.Minutes + ":" + s.Elapsed.Seconds);

                        }

                    }
                    catch
                    {

                    }
                }
            }
            return ac;
        }
    }
    public class account
    {
        static List<account> wholeBase = new List<account>();
        public string town = "";
        public string country = "";
        public string nickname = "";
        public string telegram = "";
        public string text = "";
        public string photo_link = "";
        public string post_link = "";
        public string age = "";
        public string gender = "";
        public account(string city, string c, string n, string telg, string txt, string photo, string url)
        {
            town = city;
            country = c;
            nickname = n;
            telegram = telg;
            text = txt;
            photo_link = photo;
            post_link = url;
            getAgeGender(photo_link);
        }
        public account(string city, string c, string n, string telg, string txt, string url)
        {
            town = city;
            country = c;
            nickname = n;
            telegram = telg;
            text = txt;
            photo_link = "";
            post_link = url;
        }
        async void getAgeGender(string img)
        {
            string subKey = "885d08300f7f4202a4f62edd151f3810";
            string uriBase = "https://eastus2.api.cognitive.microsoft.com/face/v1.0/detect";
            WebClient wc = new WebClient();
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
                    File.Delete("photo.jpg");
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
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            byte[] b = binaryReader.ReadBytes((int)fileStream.Length);
            fileStream.Close();
            return b;
        }
        public static List<account> getBase()
        {
            return null;
        }
        public static void writeBase(List<account> list)
        {
            XDocument xdoc = new XDocument();
            xdoc.Add(new XElement("base"));
            foreach (account a in list)
            {
                if (a.nickname.StartsWith("@")) a.nickname = a.nickname.Substring(1);
                if (a.age != "")
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
                if (a.age == "")
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
            File.Copy("base-last.xml", "base-" + DateTime.Now.Day + "-" + DateTime.Now.Year + "-" + r.Next(999, 9999) + "xml");
            xdoc.Save("base-last.xml");
        }        
    }
}
