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
    static class Order
    {
        public static List<string> Ord = new List<string>();
        public static List<double> Price = new List<double>();
    }
    public partial class Form1 : Form
    {
        BackgroundWorker bw;
        bool ShowRBut = true;
        string previous = "";

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
            string myPythonApp = "C:\\Users\\Fln1k\\source\\repos\\CsharpBot\\revcom_bot\\Connector.py";
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
                await Bot.SetWebhookAsync(""); // comment this line to remove http error and run bot. then uncomment and run again
                // Callback'и от кнопок
                Bot.OnCallbackQuery += async (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
                {
                    var message = ev.CallbackQuery.Message;
                    if(ev.CallbackQuery.Data == "Рисовая каша") {
                        Order.Ord.Add("Рисовая каша с кокосовым молоком - 220 г. - 3.50 р.");
                        Order.Price.Add(3.5);
                    } else 
                    if (ev.CallbackQuery.Data == "Клаб сэндвич")
                    {
                        Order.Ord.Add("Клаб сэндвич с цыпленком  - 300 г. - 9.00 р.");
                        Order.Price.Add(9.0);
                    } else
                    if (ev.CallbackQuery.Data == "Салат")
                    {
                        Order.Ord.Add("Салат с печеной тыквой и ореховым соусом - 150 г. - 5.50 р.");
                        Order.Price.Add(5.5);
                    } else
                    if (ev.CallbackQuery.Data == "Спагетти болоньезе")
                    {
                        Order.Ord.Add("Спагетти болоньезе с сыром Грана Падано  - 325 г. - 7.00 р.");
                        Order.Price.Add(7.0);
                    } else
                    if (ev.CallbackQuery.Data == "Суп")
                    {
                        Order.Ord.Add("Суп Минестроне - 270 г. - 4.00 р.");
                        Order.Price.Add(4.0);
                    }   
                    else
                    if (ev.CallbackQuery.Data == "Цветная капуста")
                    {
                        Order.Ord.Add("Цветная капуста с копченым соусом Чеддер  - 262 г. - 10.00 р.");
                        Order.Price.Add(10.0);
                    }
                    else
                    if (ev.CallbackQuery.Data == "Говядина")
                    {
                        Order.Ord.Add("Говядина с вялеными грибами, соус демиглас - 330 г. - 24.00 р.");
                        Order.Price.Add(24.0);
                    }
                    else
                    if (ev.CallbackQuery.Data == "Ризотто")
                    {
                        Order.Ord.Add("Ризотто с морепродуктами, соус биск  - 340 г. - 24.00 р.");
                        Order.Price.Add(24.0);
                    }
                    else
                    if (ev.CallbackQuery.Data == "Ginger Ale")
                    {
                        Order.Ord.Add("Ginger Ale - 250 г. - 3.50 р.");
                        Order.Price.Add(3.5);
                    }
                    else
                    if (ev.CallbackQuery.Data == "Сок виноградный")
                    {
                        Order.Ord.Add("Сок виноградный  - 200 г. - 2.00 р.");
                        Order.Price.Add(2.0);
                    }
                    else
                    if (ev.CallbackQuery.Data == "Сок апельсиновый")
                    {
                        Order.Ord.Add("Сок виноградный  - 200 г. - 2.00 р.");
                        Order.Price.Add(2.0);
                    }
                    else
                    if (ev.CallbackQuery.Data == "Pepsi")
                    {
                        Order.Ord.Add("Pepsi - 250 г. - 3.50 р.");
                        Order.Price.Add(3.5);
                    }
                    ShowRBut = false;
                    var keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                    {
                        Keyboard = new[] {
                                            new[] // row 1
                                            {
                                                new Telegram.Bot.Types.KeyboardButton("Отправить заказ"),
                                            },
                                            new[] // row 2
                                            {
                                                new Telegram.Bot.Types.KeyboardButton("Отмена"),
                                                new Telegram.Bot.Types.KeyboardButton("Корзина ["+Order.Ord.Count.ToString()+"]"),
                                            },
                                        },
                        ResizeKeyboard = true
                    };
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Added", false, false, 0, keyboard, Telegram.Bot.Types.Enums.ParseMode.Default);
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
                            previous = "/start";
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
                            await Bot.SendTextMessageAsync(message.Chat.Id, "Wait please", false, false, 0, keyboard, Telegram.Bot.Types.Enums.ParseMode.Default);
                        }
                        // обработка reply кнопок
                        if (previous.ToLower() == "оставить отзыв")                        
                        {
                            previous = "";
                            await Bot.SendTextMessageAsync(message.Chat.Id, "Спасибо, ваша оценка очень ценна для нас");
                            ShowRBut = true;
                            goto RBUT;
                        }
                        if (message.Text.ToLower() == "сделать заказ")
                        {
                            previous = "сделать заказ";
                            var keyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(
                                                    new Telegram.Bot.Types.InlineKeyboardButton[][]
                                                    {
                                                            // First row
                                                            new [] {
                                                                new Telegram.Bot.Types.InlineKeyboardButton("Рисовая каша с кокосовым молоком - 220 г. - 3.50 р.","Рисовая каша"),
                                                            },
                                                            new[]
                                                            {
                                                                 new Telegram.Bot.Types.InlineKeyboardButton("Клаб сэндвич с цыпленком  - 300 г. - 9.00 р.","Клаб сэндвич"),
                                                            },
                                                            new[]
                                                            {
                                                                 new Telegram.Bot.Types.InlineKeyboardButton("Салат с печеной тыквой и ореховым соусом - 150 г. - 5.50 р.","Салат"),
                                                            },
                                                            new[]
                                                            {
                                                                 new Telegram.Bot.Types.InlineKeyboardButton("Спагетти болоньезе с сыром Грана Падано -325 г.-7.00 р.","Спагетти болоньезе"),
                                                            },
                                                            new[]
                                                            {
                                                                 new Telegram.Bot.Types.InlineKeyboardButton("Суп Минестроне - 270 г. - 4.00 р.","Суп"),
                                                            },
                                                            new[]
                                                            {
                                                                 new Telegram.Bot.Types.InlineKeyboardButton("Цветная капуста с копченым соусом Чеддер-262 г.-10.00 р.","Цветная капуста"),
                                                            },
                                                            new[]
                                                            {
                                                                 new Telegram.Bot.Types.InlineKeyboardButton("Говядина с вялеными грибами, соус демиглас-330 г.-24.00 р.","Говядина"),
                                                            },
                                                            new[]
                                                            {
                                                                 new Telegram.Bot.Types.InlineKeyboardButton("Ризотто с морепродуктами, соус биск  - 340 г. - 24.00 р.","Ризотто"),
                                                            },
                                                            new[]
                                                            {
                                                                 new Telegram.Bot.Types.InlineKeyboardButton("Ginger Ale  - 250 г. - 3.50 р.","Ginger Ale"),
                                                            },
                                                            new[]
                                                            {
                                                                 new Telegram.Bot.Types.InlineKeyboardButton("Сок виноградный  - 200 г. - 2.00 р.","Сок виноградный"),
                                                            },
                                                            new[]
                                                            {
                                                                 new Telegram.Bot.Types.InlineKeyboardButton("Сок апельсиновый  - 200 г. - 2.00 р.","Сок апельсиновый"),
                                                            },
                                                            new[]
                                                            {
                                                                 new Telegram.Bot.Types.InlineKeyboardButton("Pepsi  - 250 г. - 3.50 р.","Pepsi"),
                                                            },
                                                    }
                                                );

                            await Bot.SendTextMessageAsync(message.Chat.Id,"Меню находится выше", false, false, 0, keyboard, Telegram.Bot.Types.Enums.ParseMode.Default);
                        }
                        if(previous == "сделать заказ")
                        {
                            previous = "Меню";
                            ShowRBut = false;
                            var keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                            {
                                Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new Telegram.Bot.Types.KeyboardButton("Отмена"),
                                                    new Telegram.Bot.Types.KeyboardButton("Корзина ["+Order.Ord.Count.ToString()+"]"),
                                                },
                                            },
                                ResizeKeyboard = true
                            };
                            await Bot.SendTextMessageAsync(message.Chat.Id, "меню находится выше", false, false, 0, keyboard, Telegram.Bot.Types.Enums.ParseMode.Default);
                        }
                        else
                        if (message.Text.ToLower() == "отправить заказ" && previous == "Меню")
                        {
                            previous = "";
                            await Bot.SendTextMessageAsync(message.Chat.Id, "Ваш заказ отправлен и в данный момент обрабатывается");
                            ShowRBut = true;
                            goto RBUT;
                        }
                        else
                        if (message.Text.ToLower() == "отмена" && previous == "Меню")
                        {
                            previous = "";
                            Order.Ord.Clear();
                            Order.Price.Clear();
                            ShowRBut = true;
                            goto RBUT;
                        }
                        else
                        if (message.Text.ToLower() == "корзина [" + Order.Ord.Count.ToString() + "]" && previous == "Меню")
                        {
                            string order = "";
                            double price = 0;
                            for (int point = 0;point<Order.Ord.Count;++point)
                            {
                                order += Order.Ord[point];
                                order += "\n";
                                price += Order.Price[point];
                            }
                            order += ("Общая стоимсоть: " + price);
                            await Bot.SendTextMessageAsync(message.Chat.Id, order);

                        }
                        else
                        if (message.Text.ToLower() == "бронировать столик")
                        {
                            previous = "бронировать столик";
                            string date = DateTime.Now.ToString("dd MMMM yyyy | HH:mm:ss");
                            await Bot.SendTextMessageAsync(message.Chat.Id, date);
                        }
                        else
                        if (message.Text.ToLower() == "оставить отзыв")
                        {
                            previous = "оставить отзыв";
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
Одна ошибка и ты ошибся - Сократ, 399 г. до н. э.
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
                                previous = "";
                                if (ContainsSpecialChars(message.Text) && message.Text.ToLower()!= "отмена" && message.Text.ToLower() != "отправить заказ")
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
