using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace PikaLoveBot
{
    public class Telegram
    {
        public static List<UserProfile> _base = new List<UserProfile>();
        public static TelegramBotClient Bot;
        public static List<string> AllTownsList = new List<string>();
        public static void BotStart()
        {
            Bot = new TelegramBotClient(MainFormPikaLoveBot.api_key_telegram);
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
            }
            _base.RemoveAll(x => x.photo_link == "");
            User me = Bot.GetMeAsync().Result;

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnReceiveError += BotOnReceiveError;
            Bot.DeleteWebhookAsync();
            Telegram.Log("Bot Stared " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
            Bot.StartReceiving(Array.Empty<UpdateType>());
        }
        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            Telegram.Log(e.ApiRequestException.Message);
        }
        public static void ToPoolCallbackQueryReceived(Object ev)
        {
            CallbackQueryEventArgs e = (CallbackQueryEventArgs)ev;
            switch (e.CallbackQuery.Data.Split('&').Length)//Длинна запроса
            {
                case 1:
                    if (e.CallbackQuery.Data == "Украина")
                        SendMessageToUser(e.CallbackQuery.From.Id, "Выберите город!)", parser("Украина", 0));
                    if (e.CallbackQuery.Data == "Россия")
                        SendMessageToUser(e.CallbackQuery.From.Id, "Выберите город!)", parser("Россия", 0));
                    if (e.CallbackQuery.Data == "Белоруссия")
                        SendMessageToUser(e.CallbackQuery.From.Id, "Выберите город!)", parser("Белоруссия", 0));
                    if (e.CallbackQuery.Data == "Казахстан")
                        SendMessageToUser(e.CallbackQuery.From.Id, "Выберите город!)", parser("Казахстан", 0));
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
                        SendMessageToUser(e.CallbackQuery.From.Id, "Выберите пол!)", k);
                    }
                    else
                        SendMessageToUser(e.CallbackQuery.From.Id, "Выберите город!)", parser(e.CallbackQuery.Data.Split('&').First(), Convert.ToInt32(e.CallbackQuery.Data.Split('&').Last())));

                    break;
                case 3:
                    string[] parsedQuery = e.CallbackQuery.Data.Split('&');
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
                            SendMessageToUser(e.CallbackQuery.From.Id, message, NextKey(e.CallbackQuery.Data, f[ran]));
                        }
                    }
                    catch
                    {
                        SendMessageToUser(e.CallbackQuery.From.Id, "Никого не найдено");
                    }
                    break;

            }
        }
        private static void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ToPoolCallbackQueryReceived), e);            
        }
        private static async void createPage(Message message)
        {
            try
            {
                if (Telegram._base.Exists(x => x.telegram == message.From.Id.ToString()) != true)
                {
                    UserProfile n = new UserProfile();
                    var file = await Bot.GetFileAsync(message.Photo.Last()?.FileId);
                    n.nickname = "Никнейм не установлен";
                    if (message.From.Username != null)
                        n.nickname = message.From.Username;
                    n.telegram = message.From.Id.ToString();
                    n.photo_link = file.FilePath;
                    n.getAgeGenderFromTelegramImage(message);
                    Thread.Sleep(1000);
                    if(n.age == null)
                    {
                        Thread.Sleep(20050);
                        await Bot.SendChatActionAsync(message.From.Id, ChatAction.Typing);
                        n.getAgeGenderFromTelegramImage(message);
                    }
                    Telegram._base.Add(n);
                    string m = "Фото добавлено\r\nТеперь введите Ваш город, если он есть в базе\r\n\r\nЕсли его нет, то напишите нам в наш чат\r\nhttps://t.me/joinchat/Ci4beRGr_bq_7OMVm2EnZw";
                    SendMessageToUser(message.From.Id, m);
                }
            }
            catch//(Exception ex)
            {
                Telegram.SendMessageToUser(170793849, "Ошибка сохранения\r\nЮзер "+ message.Chat.FirstName  + " " + message.Chat.LastName + " не был добавлен" );
                Telegram.SendMessageToUser(message.From.Id, "Что-то пошло не так\r\nПовторите попытку, пожалуйста");
            }
        }
        private static void AddPageInform(Message message)
        {
            try
            { 
            UserProfile u = Telegram._base.Find(x => x.telegram == message.From.Id.ToString());
            UserProfile n = u;
            if (u == null)
            {
                SendMessageToUser(message.From.Id, "Для начала Вам нужно отправить Ваше фото\r\nВведите команду /add, чтобы узнать по поводу добавления анкеты");
                return;
            }
            if (message.Text.EndsWith(" "))
                message.Text = message.Text.Substring(0, message.Text.Length - 2);
            n.town = message.Text;
            if (UserProfile.ukr.Exists(x => x == n.town))
                n.country = "Украина";
            if (UserProfile.rus.Exists(x => x == n.town))
                n.country = "Россия";
            if (UserProfile.bel.Exists(x => x == n.town))
                n.country = "Белоруссия";
                if (UserProfile.kz.Exists(x => x == n.town))
                    n.country = "Казахстан";
                if (n.country == "" || n.country == null)
            {
                SendMessageToUser(message.From.Id, "Город ещё не добавлен в базу");
                return;
            }

            n.post_link = "";
                n.text = "";
            _base.Remove(u);
            _base.Add(n);
                UserProfile.AddUserToBaseXml(_base);
            SendMessageToUser(message.Chat.Id, "Город успешно установлен ;)");
            Telegram.Log("New user " + n.nickname + " added\r\nId - " + n.telegram);
                MainFormPikaLoveBot.statistic.users++;
        }
   
            catch
            {
                Telegram.SendMessageToUser(170793849, "Ошибка сохранения\r\nЮзер "+ message.Chat.FirstName  + " " + message.Chat.LastName + " не был добавлен" );
                Telegram.SendMessageToUser(message.From.Id, "Что-то пошло не так\r\nПовторите попытку, пожалуйста");
            }

        }
        private static async void SendAnket(UserProfile userProfile, int id, string query)
        {
            try
            {
                string gender = "парень";
                if (userProfile.gender == "female")
                    gender = "девушка";
                WebClient wc = new WebClient();
                string message = "Ник: @" + userProfile.nickname + "\r\n"+ "\r\nВозможно: " + userProfile.age.Split('.').First() + ", " + gender;
                Random r = new Random();
                int num = r.Next(0, 1000000);
                Stream s = null;
                ////////////////////////////////////////////////////////////////////////
                if (userProfile.photo_link.StartsWith("http"))
                {
                    wc.DownloadFile(userProfile.photo_link, num + "photo.jpg");
                    s = new StreamReader(num + "photo.jpg").BaseStream;
                }
                else
                {
                    string filename = userProfile.photo_link;

                    using (var saveImageStream = System.IO.File.Open(filename, FileMode.Create))
                    {
                        await Bot.DownloadFileAsync(userProfile.photo_link, saveImageStream);
                    }
                    s = new StreamReader(filename).BaseStream;
                }
                await Bot.SendPhotoAsync(id, new InputOnlineFile(s), message, ParseMode.Html, false, 0, NextKey(query, userProfile));
                s.Close();
                System.IO.File.Delete(num + "photo.jpg");
            }
            catch
            {
                Telegram.Log("Не удалось отправить анкету " + userProfile.telegram);
            }
        }
        public static void ToPoolMessageReceived(object objecxt)
        {
            MessageEventArgs e = (MessageEventArgs)objecxt;
            acs.AddUserToStats(e.Message.From.Username, e.Message.From.Id);
            switch (e.Message.Type)
            {
                case MessageType.Photo:
                    createPage(e.Message);
                    break;
                case MessageType.Text:
                    switch (e.Message.Text)
                    {
                        case "/start":
                        case "Начать заново":
                            var keyboard = new ReplyKeyboardMarkup
                            {
                                Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new KeyboardButton("Начать заново"),
                                                    new KeyboardButton("Краткая информация")
                                                },
                                            },
                                ResizeKeyboard = true
                            };
                            SendMessageToUser(e.Message.Chat.Id, "Добро пожаловать!", keyboard);
                            InlineKeyboardMarkup k = new InlineKeyboardMarkup(
                                new InlineKeyboardButton[]
                                {
                            InlineKeyboardButton.WithCallbackData("Украина"),
                            InlineKeyboardButton.WithCallbackData("Россия"),
                            InlineKeyboardButton.WithCallbackData("Белоруссия"),
                            InlineKeyboardButton.WithCallbackData("Казахстан")
                                });
                            SendMessageToUser(e.Message.Chat.Id, "Выбрать страну", k);
                            break;
                        case "/add":
                            SendMessageToUser(e.Message.From.Id, "Чтобы добавить профиль, перешлите мне своё фото :3, а после того, как бот подтвердит получение - введите город, к которому привязать аккаунт :3");
                            break;
                        case "/get":
                            if (Telegram._base.Exists(x => x.nickname == e.Message.From.Username && x.country != "" && x.country != null))
                            {
                                UserProfile prof = Telegram._base.Find(x => x.nickname == e.Message.From.Username);
                                SendAnket(prof, e.Message.From.Id, prof.country + "&" + prof.town + "&" + prof.gender);
                                return;
                            }
                            else
                                SendMessageToUser(e.Message.From.Id, "Вас ещё нет у Нас в базе ;|\r\nВведите /Add , чтобы узнать как добавить себя :3");
                            break;
                        case "/chat":
                            SendMessageToUser(e.Message.Chat.Id, "Наш чат: \r\n https://t.me/joinchat/Ci4beRGr_bq_7OMVm2EnZw");
                            break;
                        case "Краткая информация":
                            SendMessageToUser(e.Message.Chat.Id, "Чтобы добавить свою анкету - отошлите боту своё фото >> дождитесь, пока бот его примет(может занять около 30 секунд >> привяжите фото к городу" +
                                                    "\r\n Основные команды:" +
                                                    "\r\n/add - даёт подсказку, как добавить свою анкету" +
                                                    "\r\n/chat - даёт ссылку на чат бота, где Вы можете пообщаться или же внести предложения по работе бота" +
                                                    "\r\n/get - проверить, есть ли Вы в базе" +
                                                    "\r\n/start - Перезапуск бота");
                            break;
                        default:
                            if (AllTownsList.Exists(x => x == e.Message.Text))
                            {
                                AddPageInform(e.Message);
                                break;
                            }
                            else
                                SendMessageToUser(e.Message.From.Id, "Попробуйте, пожалуйста, ещё раз. \r\nЧто-то пошло не так ;|");
                            break;
                    }
                    break;
                default:
                    SendMessageToUser(e.Message.From.Id, "Попробуйте, пожалуйста, ещё раз. \r\nЧто-то пошло не так ;|");
                    break;
            }
        }
        private static void BotOnMessageReceived(object sender, MessageEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ToPoolMessageReceived), e);
        }
        private static InlineKeyboardMarkup NextKey(string query, UserProfile up)
        {
            InlineKeyboardMarkup inlineKeyboard = null;
            if (up.post_link != "")
                inlineKeyboard = new InlineKeyboardMarkup(new[]
                        {
                        new [] // first row
                        {
                            InlineKeyboardButton.WithCallbackData("Дальше", query)
                        },
                        new [] // second row
                        {
                            InlineKeyboardButton.WithUrl("Перейти к посту", up.post_link)
                        }
                    });
            else
                inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new [] // first row
                        {
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
            if (query == "Казахстан")
                towns = UserProfile.kz.ToList();
            if (offset == 0)
            {
                if (towns.Count < 17)
                {
                    for (int i = 0; i < towns.Count; i = i + 2)
                    {
                        List<InlineKeyboardButton> d = new List<InlineKeyboardButton>();
                        for (int g = 0; g < 2; g++)
                        {
                            InlineKeyboardButton button = new InlineKeyboardButton();
                            try
                            {
                                button = InlineKeyboardButton.WithCallbackData(towns[i + g], query + "&" + towns[i + g]);
                            }
                            catch
                            {

                            }
                            if (button.Text == null)
                                break;
                            if (button != null)
                                d.Add(button);
                        }
                        s.Add(d);
                    }
                }
                if (towns.Count > 16)
                {
                    List<InlineKeyboardButton> f = new List<InlineKeyboardButton>();
                    for (int i = 0; i < 16; i = i + 2)
                    {
                        List<InlineKeyboardButton> d = new List<InlineKeyboardButton>();
                        try
                        {
                            for (int g = 0; g < 2; g++)
                            {
                                InlineKeyboardButton button = InlineKeyboardButton.WithCallbackData(towns[i + g], query + "&" + towns[i + g]);
                                if (button.Text == null)
                                    break;
                                d.Add(button);
                            }
                        }
                        catch
                        {

                        }

                        s.Add(d);
                    }
                    if (s.Count > 7)
                    {
                        InlineKeyboardButton NextKey = InlineKeyboardButton.WithCallbackData("Далее", query + "&" + (offset + 1));
                        f.Add(NextKey);
                    }
                    s.Add(f);
                }
            }
            if (offset > 0)
            {
                int townsl = towns.Count - (offset * 8);
                if (townsl < 17)
                {
                    for (int i = 0; i < towns.Count; i++)
                    {
                        InlineKeyboardButton button = InlineKeyboardButton.WithCallbackData(towns[i], query + "&" + towns[i]);
                        List<InlineKeyboardButton> d = new List<InlineKeyboardButton>();
                        d.Add(button);
                        s.Add(d);
                    }
                    goto end;
                }
                if (towns.Count > 16)
                {
                    int incr = offset * 16;
                    List<InlineKeyboardButton> f = new List<InlineKeyboardButton>();
                    for (int i = incr; i < incr + 16; i = i + 2)
                    {
                        List<InlineKeyboardButton> d = new List<InlineKeyboardButton>();
                        try
                        {
                            for (int g = 0; g < 2; g++)
                            {
                                InlineKeyboardButton button = InlineKeyboardButton.WithCallbackData(towns[i + g], query + "&" + towns[i + g]);
                                d.Add(button);
                            }
                        }
                        catch
                        {

                        }

                        s.Add(d);
                    }
                    if (s[7].Count == 2)
                    {
                        InlineKeyboardButton NextKey = InlineKeyboardButton.WithCallbackData("Далее", query + "&" + (offset + 1));
                        f.Add(NextKey);
                    }
                    InlineKeyboardButton BackKey = InlineKeyboardButton.WithCallbackData("Назад", query + "&" + (offset - 1));
                    f.Add(BackKey);
                    s.Add(f);
                }
            }
            end:
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(s);
            return keyboard;
        }
        public static void Log(string text)
        {
            try
            {
                StreamReader sr = new StreamReader("log.txt");
                string data = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "      " + text + "\r\n" + sr.ReadToEnd();
                sr.Close();
                StreamWriter sw = new StreamWriter("log.txt");
                sw.Write(data);
                sw.Close();
            }
            catch
            {

            }
        }
        public static async void SendMessageToUser(long id, string text, InlineKeyboardMarkup keyboard)
        {
            try
            {
                await Bot.SendTextMessageAsync(id, text, ParseMode.Html, true, false, 0, keyboard);
            }
            catch(Exception ex)
            {
                StreamWriter sw = new StreamWriter("Errors.txt", true, Encoding.UTF8);
                sw.WriteLine("[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "]\r\n      " + ex.ToString() + "\r\n\r\n\r\n");
                sw.Close();
            }
        }
        public static async void SendMessageToUser(long id, string text, ReplyKeyboardMarkup keyboard)
        {
            try
            {
                await Bot.SendTextMessageAsync(id, text, ParseMode.Html, true, false, 0, keyboard);
            }
            catch
            {
                StreamWriter sw = new StreamWriter("Errors.txt", true, Encoding.UTF8);
                sw.WriteLine("[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "]\r\n      " + text + "\r\n\r\n\r\n");
                sw.Close();
            }
        }
        public static async void SendMessageToUser(long id, string text)
        {
            try
            {
                    await Bot.SendTextMessageAsync(id, text, ParseMode.Html);
            }
            catch
            {
                StreamWriter sw = new StreamWriter("Errors.txt", true, Encoding.UTF8);
                sw.WriteLine("[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "]\r\n      " + text + "\r\n\r\n\r\n");
                sw.Close();
            }
        }
        
    }
}
