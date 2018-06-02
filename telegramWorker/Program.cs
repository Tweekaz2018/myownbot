using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace telegramWorker
{
    public class Program
    {
        static void Main(string[] args)
        {
            Telegram.BotStart();
            foreach(string s in UserProfile.ukr)
            UserProfile.countryTowns.Add("Украина", s);
            foreach(string s in UserProfile.rus)
            UserProfile.countryTowns.Add("Россия", s);
            foreach(string s in UserProfile.bel)
            UserProfile.countryTowns.Add("Белоруссия", s);
        }
    }
    public class Telegram
    {
        public static List<UserProfile> _base = new List<UserProfile>();
        private static readonly TelegramBotClient Bot = new TelegramBotClient("327959615:AAG0sVBX8Gz-cE-WbMOtXHoCGbgoRp9ndzU");
        public static void BotStart()
        {
            System.IO.File.Delete("photo.jpg");

            XDocument xdoc = XDocument.Load("base-last.xml");
            foreach (XElement x in xdoc.Element("base").Elements())
            {
                _base.Add(new UserProfile(x.Attribute("nickname").Value,
                    x.Attribute("photo_link").Value,
                    x.Attribute("post_link").Value,
                    x.Attribute("country").Value,
                    x.Attribute("town").Value,
                    x.Attribute("telegram").Value,
                    x.Attribute("age").Value,
                    x.Attribute("gender").Value,
                    x.Attribute("text").Value));
                Console.WriteLine(x.Attribute("photo_link").Value);
            }
            User me = Bot.GetMeAsync().Result;
            Console.Title = me.Username;

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnReceiveError += BotOnReceiveError;
            Bot.DeleteWebhookAsync();
            Bot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Start listening for @{me.Username}");            
            Console.ReadLine();
            Bot.StopReceiving();
        }
        
        private static async void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            switch (e.CallbackQuery.Data.Split('&').Length)//Длинна запроса
            {
                case 1:
                    if (e.CallbackQuery.Data == "Украина")
                        await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Выберите город!)", ParseMode.Default, false, false, 0, parser("Украина", 0));
                    if (e.CallbackQuery.Data == "Россия")
                        await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Выберите город!)", ParseMode.Default, false, false, 0, parser("Россия", 0));
                    if (e.CallbackQuery.Data == "Белоруссия") 
                        await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Выберите город!)", ParseMode.Default, false, false, 0, parser("Белоруссия", 0));
                    break;
                case 2:
                    if (e.CallbackQuery.Data.Split('&').Last().Length > 2)
                    {
                        InlineKeyboardMarkup k = new InlineKeyboardMarkup(
                        new InlineKeyboardButton[]
                        {
                            InlineKeyboardButton.WithCallbackData("Мужской", e.CallbackQuery.Data + "&male"),
                            InlineKeyboardButton.WithCallbackData("Женский", e.CallbackQuery.Data + "&female")
                        });
                        await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Выберите пол!)", ParseMode.Default, false, false, 0, k);
                    }
                    else
                        await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Выберите город!)", ParseMode.Default, false, false, 0, parser(e.CallbackQuery.Data.Split('&').First(), Convert.ToInt32(e.CallbackQuery.Data.Split('&').Last())));

                    break;
                case 3:
                    string[]parsedQuery = e.CallbackQuery.Data.Split('&');
                        var users = _base.FindAll(x => x.country == parsedQuery[0] && x.town == parsedQuery[1] && x.gender == parsedQuery[2]).ToList();
                        Random r = new Random();
                        List<UserProfile> f = new List<UserProfile>();
                        foreach (UserProfile u in _base)
                        {
                            if (u.country == parsedQuery[0] && u.town == parsedQuery[1] && u.gender == parsedQuery[2])
                                f.Add(u);
                        }
                        try
                        {
                            int ran = r.Next(0, f.Count - 1);
                            if (f[ran].photo_link != "")
                                SendAnket(f[ran], e.CallbackQuery.From.Id, e.CallbackQuery.Data);
                            else
                            {
                                string gender = "парень";
                                if (f[ran].gender == "female")
                                    gender = "девушка";
                                WebClient wc = new WebClient();
                                string message = "Ник: " + f[ran].nickname + "\r\n" + "Телеграм: " + f[ran].telegram + "\r\nВозможно: " + f[ran].age.Split('.').First() + " лет, " + gender;
                                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, message, ParseMode.Html, true, false, 0, NextKey(e.CallbackQuery.Data, f[ran]));
                            }
                        }
                        catch
                        {
                            await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Никого не найдено");
                        }
                    break;

            }
        }
        private static async void SendAnket(UserProfile userProfile, int id, string query)
        {//string nick, string photo, string post, string count, string to, string tel, string a, string gen, string tex)
            string gender = "парень";
            if (userProfile.gender == "female")
                gender = "девушка";
            WebClient wc = new WebClient();                        
            string message = "Ник: " + userProfile.nickname + "\r\n" + "Телеграм: " + userProfile.telegram + "\r\nВозможно: " + userProfile.age.Split('.').First() + ", " + gender;
            Random r = new Random();
            int num = r.Next(0, 1000000);
            wc.DownloadFile(userProfile.photo_link, num + "photo.jpg");
            Stream s = new StreamReader(num + "photo.jpg").BaseStream;
            await Bot.SendPhotoAsync(id, new InputOnlineFile(s), message, ParseMode.Html, false, 0, NextKey(query, userProfile));
            s.Close();
            System.IO.File.Delete(num + "photo.jpg");
        }
            private static async void BotOnMessageReceived(object sender, MessageEventArgs e)
            {
                if (e.Message.Text == "/start" || e.Message.Text == "Начать заново")
                {
                var keyboard = new ReplyKeyboardMarkup
                {
                    Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new KeyboardButton("Начать заново")
                                                },
                                            },
                    ResizeKeyboard = true
                };
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Добро пожаловать! ", ParseMode.Default, false, false, 0, keyboard);

                    InlineKeyboardMarkup k = new InlineKeyboardMarkup(
                        new InlineKeyboardButton[]
                        {
                            InlineKeyboardButton.WithCallbackData("Украина"),
                            InlineKeyboardButton.WithCallbackData("Россия"),
                            InlineKeyboardButton.WithCallbackData("Белоруссия")
                        });
                    await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Выбрать страну", ParseMode.Default, false, false, 0, k);
                
            }
        }
        private static InlineKeyboardMarkup NextKey(string query, UserProfile up)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new [] // first row
                        {
                            InlineKeyboardButton.WithUrl("Перейти к посту", up.post_link),
                            InlineKeyboardButton.WithCallbackData("Дальше", query)
                        }
                    });
            return inlineKeyboard;
        }
        private static InlineKeyboardMarkup parser(string query, int offset)
        {
            List<List<InlineKeyboardButton>> s = new List<List<InlineKeyboardButton>>();

            List<string> towns = new List<string>();

            if (query == "Украина")
                towns = UserProfile.ukr.ToList();
            if (query == "Россия")
                towns = UserProfile.rus.ToList();
            if (query == "Белоруссия")
                towns = UserProfile.bel.ToList();
            int d = 0;
            if (offset > 0)
                d = 24 * offset;

            if (towns.Count > 24)
            {
                for (int c = 0; c < 5; c++)
                {
                    s.Add(new List<InlineKeyboardButton>());
                    for (int g = 0; g < 5; g++)
                    {
                        InlineKeyboardButton button = InlineKeyboardButton.WithCallbackData(towns[d], query + "&" + towns[d]);
                        s[c].Add(button);
                        d++;
                    }
                }
            InlineKeyboardButton nextButton = InlineKeyboardButton.WithCallbackData("Далее", query + "&" + (offset + 1));
            s[4].Add(nextButton);
            }
            if (towns.Count < 24 && towns.Count > 5)
            {
                int i = Convert.ToInt32(towns.Count / 5);
                int k = towns.Count - (i * 5);
                int counter = towns.Count;
                for (int c = 0; c < i + 1; c++)
                {
                    s.Add(new List<InlineKeyboardButton>());
                    if (counter < 5)
                    {
                        for (int g = 0; g < k; g++)
                        {
                            InlineKeyboardButton button = InlineKeyboardButton.WithCallbackData(towns[d], query + "&" + towns[d]);
                            s[c].Add(button);
                            d++;
                        }
                    }
                    else
                    {
                        for (int g = 0; g < 5; g++)
                        {
                            InlineKeyboardButton button = InlineKeyboardButton.WithCallbackData(towns[d], query + "&" + towns[d]);
                            s[c].Add(button);
                            d++;
                            counter--;
                        }
                    }
                }
            }
            else 
            {
                int h = towns.Count - d;

                s.Add(new List<InlineKeyboardButton>());
                for (int m = 0; m < h; m++)
                {
                    InlineKeyboardButton button = InlineKeyboardButton.WithCallbackData(towns[d], query + "&" + towns[d]);
                    s[s.Count - 1].Add(button);
                    d++;
                }
            }
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(s);

            return keyboard;
        }
        private static InlineKeyboardMarkup parser(string query)//Автозаполнение клавиатуры
        {
            int i;
            int j;
            List<string> towns = new List<string>();
            if (query == "Украина")
                towns = UserProfile.ukr.ToList();
            if (query == "Россия")
                towns = UserProfile.rus.ToList();
            if (query == "Белоруссия")
                towns = UserProfile.bel.ToList();
            i = Convert.ToInt32((towns.Count / 5).ToString().Split('.').First());
            j = towns.Count - (i * 5);
            int d = towns.Count - 1;
            List<List<InlineKeyboardButton>> s = new List<List<InlineKeyboardButton>>();
            int h = towns.Count - (i*5);


            if (i > 0)
            {
                for (int c = 0; c < i; c++)
                {
                    s.Add(new List<InlineKeyboardButton>());
                    for (int g = 0; g < 5; g++)
                    {
                        InlineKeyboardButton button = InlineKeyboardButton.WithCallbackData(towns[d], query + "&" + towns[d]);
                        s[c].Add(button);
                        d--;
                    }
                    if(h != 0)
                    {
                        s.Add(new List<InlineKeyboardButton>());
                        for (int m = 0; m < h; m++) {
                            InlineKeyboardButton button = InlineKeyboardButton.WithCallbackData(towns[d], query + "&" + towns[d]);
                            s[s.Count - 1].Add(button);
                            d--;
                        }
                    }
                }
            }
            if(i == 0)
            {
                s.Add(new List<InlineKeyboardButton>());
                for (int m = 0; m < h; m++)
                {
                    InlineKeyboardButton button = InlineKeyboardButton.WithCallbackData(towns[d], query + "&" + towns[d]);
                    s[s.Count - 1].Add(button);
                    d--;
                }
            }
            
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(s);
            return keyboard; 
        }

    }
}

