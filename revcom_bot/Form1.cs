using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Collections.Specialized;

namespace revcom_bot
{
    public partial class Form1 : Form
    {
        BackgroundWorker bw;
        bool ShowRBut = true;

        public Form1()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //

            this.bw = new BackgroundWorker();
            this.bw.DoWork += bw_DoWork;
        }

        public string RunPy(string Request)
        {
            // full path of python interpreter  
            string python = @"python.exe";
            // python app to call  
            string myPythonApp = "C:\\Users\\Fln1k\\Desktop\\revcom_bot\\revcom_bot\\Connector.py";
            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);
            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;
            myProcessStartInfo.Arguments = myPythonApp + " " + Request;
            Process myProcess = new Process();
            // assign start information to the process 
            myProcess.StartInfo = myProcessStartInfo;
            myProcessStartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding(1251);
            // start process 
            myProcess.Start();
            string output = myProcess.StandardOutput.ReadToEnd();
            myProcess.WaitForExit();
            return output;
        }

        private bool ContainsSpecialChars(string value)
        {
            var list = new[] { "й", "ц", "у", "к", "е", "н", "г", "ш", "щ", "з", "х", "ъ", "ф", "ы", "в", "а", "п", "р", "о", "л", "д", "ж", "э", "я", "ч", "с", "м", "и", "т", "ь", "б", "ю" };
            return list.Any(value.Contains);
        }

        async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var key = e.Argument as String; // получаем ключ из аргументов
            try
            {
                var Bot = new Telegram.Bot.TelegramBotClient(key); // инициализируем API
                await Bot.SetWebhookAsync("");
                // Callback'и от кнопок
                Bot.OnCallbackQuery += async (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
                {
                    var message = ev.CallbackQuery.Message;
                    if(ev.CallbackQuery.Data == "callback1") {
                        await Bot.AnswerCallbackQueryAsync(ev.CallbackQuery.Id, "You hav choosen " + ev.CallbackQuery.Data, true);
                    } else 
                    if (ev.CallbackQuery.Data == "callback2")
                    {
                        await Bot.SendTextMessageAsync(message.Chat.Id, "тест", replyToMessageId: message.MessageId);
                        await Bot.AnswerCallbackQueryAsync(ev.CallbackQuery.Id);
                    }
                };

                Bot.OnUpdate += async (object su, Telegram.Bot.Args.UpdateEventArgs evu) =>
                {
                    if (evu.Update.CallbackQuery != null || evu.Update.InlineQuery != null) return;
                    var update = evu.Update;
                    var message = update.Message;
                    if (message == null) return;
                    if (message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                    {
                        if (message.Text == "/start")
                        {
                            // в ответ на команду /saysomething выводим сообщение
                            await Bot.SendTextMessageAsync(message.Chat.Id, "Добро пожаловать в ресторан «Interside»! Здесь можно забронировать столик, посмотреть меню и оформить заказ навынос.");
                            ShowRBut = true;
                        }
                        RBUT:
                        // reply buttons
                        if (ShowRBut)
                        {
                            var keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                            {
                                Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new Telegram.Bot.Types.KeyboardButton("Сделать заказ"),
                                                    new Telegram.Bot.Types.KeyboardButton("Бронировать столик")
                                                },
                                                new[] // row 2
                                                {
                                                    new Telegram.Bot.Types.KeyboardButton("Оставить отзыв"),
                                                    new Telegram.Bot.Types.KeyboardButton("О Ресторане")
                                                },
                                                  
                                            },
                                ResizeKeyboard = true
                            };
                            await Bot.SendTextMessageAsync(message.Chat.Id, "/n", false, false, 0, keyboard, Telegram.Bot.Types.Enums.ParseMode.Default);
                        }
                        // обработка reply кнопок
                        if (!ShowRBut && (message.Text.ToLower() == "0" || message.Text.ToLower() == "1" || message.Text.ToLower() == "2" || message.Text.ToLower() == "3" || message.Text.ToLower() == "4" || message.Text.ToLower() == "5" || message.Text.ToLower() == "6" || message.Text.ToLower() == "7" || message.Text.ToLower() == "8" || message.Text.ToLower() == "9" || message.Text.ToLower() == "10"))                        {
                            await Bot.SendTextMessageAsync(message.Chat.Id, "Спасибо, ваша оценка очень ценна для нас");
                            ShowRBut = true;
                            goto RBUT;
                        }
                        if (message.Text.ToLower() == "cделать заказ")
                        {
                            await Bot.SendTextMessageAsync(message.Chat.Id, "rBut1_back");
                        }
                        else
                        if (message.Text.ToLower() == "бронировать столик")
                        {
                            await Bot.SendTextMessageAsync(message.Chat.Id, "rBut1_back");
                        }
                        else
                        if (message.Text.ToLower() == "оставить отзыв")
                        {
                            ShowRBut = false;
                            var keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                            {
                                Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new Telegram.Bot.Types.KeyboardButton("0"),
                                                    new Telegram.Bot.Types.KeyboardButton("1"),
                                                    new Telegram.Bot.Types.KeyboardButton("2"),
                                                    new Telegram.Bot.Types.KeyboardButton("3"),
                                                    new Telegram.Bot.Types.KeyboardButton("4"),
                                                    new Telegram.Bot.Types.KeyboardButton("5")


                                                },
                                                new[] // row 2
                                                {
                                                    new Telegram.Bot.Types.KeyboardButton("6"),
                                                    new Telegram.Bot.Types.KeyboardButton("7"),
                                                    new Telegram.Bot.Types.KeyboardButton("8"),
                                                    new Telegram.Bot.Types.KeyboardButton("9"),
                                                    new Telegram.Bot.Types.KeyboardButton("10"),
                                                },

                                            },
                                ResizeKeyboard = true
                            };

                            await Bot.SendTextMessageAsync(message.Chat.Id, "Пожалуйста, поставьте нам оценку от 0 👎до 10 👍.", false, false, 0, keyboard, Telegram.Bot.Types.Enums.ParseMode.Default);
                        }
                        
                        else
                        {
                            if (message.Text.ToLower() == "о ресторане")
                            {
                                await Bot.SendTextMessageAsync(message.Chat.Id, @"Ресторан «Interside».
Одна ошибка и ты уже ошибся - Сократ, 399 г. до н. э.
📍г.Минск, ул.Платонова, 20Бк1
📞 375445284838
🌐 Intersie.by
📅 Понедельник - пятница:
🍳 Завтраки: 08.00 - 11.30 
🍝 Обеды: 12.00 - 16.30
🍨 Основное меню: 18:00 - 23:00
🍹 Бар и десерты: 08.00 - 23.00
📅 Суббота - воскресенье: 
      11:00 - 23:00" );
                            }
                            else
                            {
                                if (ContainsSpecialChars(message.Text))
                                {
                                    await Bot.SendTextMessageAsync(message.Chat.Id, RunPy(message.Text));
                                }
                            }
                        }
                        
                    }
                };

                // запускаем прием обновлений
                Bot.StartReceiving();
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Console.WriteLine(ex.Message); // если ключ не подошел - пишем об этом в консоль отладки
            }

        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            this.bw.RunWorkerAsync("849678181:AAHMNz9KuZf9vngnALHKxr_oX6h6AOusa2w"); // передаем эту переменную в виде аргумента методу bw_DoWork
            BtnRun.Text = "Server is running...";
        }
    }
}
