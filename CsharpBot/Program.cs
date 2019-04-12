using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using SimpleJSON;
using System.Collections.Specialized;

namespace CsharpBot
{
    class Program
    {
        public static string Token = @"YOUR TOKEN HERE";
        public static int LastUpdateID = 0;

        static void Main(string[] args)
        {
            while (true)
            {
                GetUpdates();
                Thread.Sleep(1000);
            }
        }

        static void GetUpdates()
        {
            using (var webClient = new WebClient())
            {
                Console.WriteLine("Запрос обновление {0}", LastUpdateID + 1);


                string response = webClient.DownloadString("https://api.telegram.org/bot" + Token + "/getUpdates" + "?offset=" + (LastUpdateID + 1));

                var N = JSON.Parse(response);

                foreach (JSONNode r in N["result"].AsArray)
                {
                    string output;
                    LastUpdateID = r["update_id"].AsInt;
                    string request = r["message"]["text"];
                        // full path of python interpreter  
                        string python = @"python.exe";
                        // python app to call  
                        string myPythonApp = "C:\\Users\\Fln1k\\source\\repos\\CsharpBot\\CsharpBot\\Connector.py";
                        // Create new process start info 
                        ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);
                        // make sure we can read the output from stdout 
                        myProcessStartInfo.UseShellExecute = false;
                        myProcessStartInfo.RedirectStandardOutput = true;
                    // 1st argument is pointer to itself
                    bool AddData = false;
                    try
                    {
                        if (Convert.ToInt32(request[0].ToString() + request[1].ToString() + request[2].ToString()) == 987)
                        {
                            AddData = true;
                            int pointer = 3;
                            string request_to_add_info = "";
                            while(pointer<request.Length)
                            {
                                request_to_add_info += request[pointer];
                                pointer += 1;
                            }
                            Console.WriteLine("Run Procedure to add: " + request_to_add_info);
                            myProcessStartInfo.Arguments = myPythonApp + " " + "AddData" + " " + request;
                        }
                    }
                    catch
                    {
                        output = "Извините, я вас не понял";
                    }
                    if (!AddData)
                        {
                            Console.WriteLine("Пришёл запрос: {0}", request);
                            myProcessStartInfo.Arguments = myPythonApp + " " + "Request" + " " + request;
                        }
                Process myProcess = new Process();
                // assign start information to the process 
                myProcess.StartInfo = myProcessStartInfo;
                myProcessStartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding(1251);
                // start process 
                myProcess.Start();
                output = myProcess.StandardOutput.ReadToEnd();
                myProcess.WaitForExit();
                Console.InputEncoding = System.Text.Encoding.GetEncoding(1251);
                Console.OutputEncoding = System.Text.Encoding.GetEncoding(1251);
                SendMessage(output, r["message"]["chat"]["id"].AsInt);
                }
            }
        }

        static void SendMessage(string message, int chatid)
        {
            using (var webClient = new WebClient())
            {
                var pars = new NameValueCollection();

                pars.Add("text", message);
                pars.Add("chat_id", chatid.ToString());


                webClient.UploadValues("https://api.telegram.org/bot" + Token + "/sendMessage", pars);

            }
        }
    }
}
