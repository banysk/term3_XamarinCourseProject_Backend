using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;
using Nito.AsyncEx;
using System.Threading.Tasks;

namespace BackEnd
{
    class Client
    {
        // Конструктор класса. Ему нужно передавать принятого клиента от TcpListener
        public Client(TcpClient TcpClient_)
        {
            try
            {
                // запрос
                string Request = "";
                byte[] Buffer1 = new byte[1024];
                int Count;
                while ((Count = TcpClient_.GetStream().Read(Buffer1, 0, Buffer1.Length)) > 0)
                {
                    Request += Encoding.ASCII.GetString(Buffer1, 0, Count);
                    if (Request.IndexOf("\r\n\r\n") >= 0 || Request.Length > 4096)
                    {
                        break;
                    }
                }
                Request = Request.Substring(Request.LastIndexOf('\n') + 1, Request.Length - Request.LastIndexOf('\n') - 1);
                var ip = ((IPEndPoint)TcpClient_.Client.RemoteEndPoint).Address;
                Console.WriteLine($"\n{DateTime.Now}\nClient connected({ip}):{Request}");

                if (Request.Length == 0)
                {
                    Request = "operation=default";
                }

                if (Request.Length != 0)
                {
                    // парсим операцию
                    Dictionary<string, string> DArgs = GetArgsDict(Request);
                    // выбор операции
                    Console.WriteLine('\t' + DArgs["operation"]);
                    string json;
                    switch (DArgs["operation"])
                    {
                        case "login": // done
                            json = JsonConvert.SerializeObject(Login(DArgs)); // login, password / 0, 1
                            SendResponse(TcpClient_, json);
                            break;
                        case "get_user_data": // done
                            json = JsonConvert.SerializeObject(GetUserData(DArgs)); // login / User
                            SendResponse(TcpClient_, json);
                            break;
                        case "add_visit": // done
                            json = JsonConvert.SerializeObject(AddVisit(DArgs)); // login / 1
                            SendResponse(TcpClient_, json);
                            break;
                        case "get_cards": // done
                            json = JsonConvert.SerializeObject(GetCards(DArgs)); // login / List<Card>
                            SendResponse(TcpClient_, json);
                            break;
                        case "get_bills": // done
                            json = JsonConvert.SerializeObject(GetBills(DArgs)); // login List<Bill>
                            SendResponse(TcpClient_, json);
                            break;
                        case "get_credits": // done
                            json = JsonConvert.SerializeObject(GetCredits(DArgs)); // login List<Credit>
                            SendResponse(TcpClient_, json);
                            break;
                        case "change_login": // done
                            json = JsonConvert.SerializeObject(ChangeLogin(DArgs)); // login, new_login / 0, 1
                            SendResponse(TcpClient_, json);
                            break;
                        case "change_password": // done
                            json = JsonConvert.SerializeObject(ChangePassword(DArgs)); // login, new_password / 0, 1
                            SendResponse(TcpClient_, json);
                            break;
                        case "get_auth_history": // done
                            json = JsonConvert.SerializeObject(GetAuthHistory(DArgs)); // login / List<string>
                            SendResponse(TcpClient_, json);
                            break;
                        case "can_transfer_to": // done
                            json = JsonConvert.SerializeObject(CanTransferTo(DArgs)); // number / 0, 1
                            SendResponse(TcpClient_, json);
                            break;
                        case "get_history": // done
                            json = JsonConvert.SerializeObject(GetHistory(DArgs)); // login / list<Operation>
                            SendResponse(TcpClient_, json);
                            break;
                        case "get_part_history": // done
                            json = JsonConvert.SerializeObject(GetPartHistory(DArgs)); // number / list<Operation>
                            SendResponse(TcpClient_, json);
                            break;
                        case "do_transfer": //done
                            json = JsonConvert.SerializeObject(DoTransfer(DArgs)); // start_number, target_number, amount / 0, 1, 2, 3, 4
                            SendResponse(TcpClient_, json);
                            break;
                        case "block_card": // done
                            json = JsonConvert.SerializeObject(BlockCard(DArgs)); // number / 0, 1
                            SendResponse(TcpClient_, json);
                            break;
                        case "rename_card": // done
                            json = JsonConvert.SerializeObject(RenameCard(DArgs)); // number, name / 0, 1
                            SendResponse(TcpClient_, json);
                            break;
                        case "get_patterns": // done
                            json = JsonConvert.SerializeObject(GetPatterns(DArgs)); // login / List<Pattern>
                            SendResponse(TcpClient_, json);
                            break;
                        case "create_pattern": 
                            json = JsonConvert.SerializeObject(CreatePattern(DArgs)); // login, name, from, to, amount / 0, 1
                            SendResponse(TcpClient_, json);
                            break;
                        case "remove_pattern":
                            json = JsonConvert.SerializeObject(RemovePattern(DArgs)); // login, name / 0, 1
                            SendResponse(TcpClient_, json);
                            break;
                        case "send_message":
                            json = JsonConvert.SerializeObject(SendMessage(DArgs)); // from_, to_, msg / 0, 1
                            SendResponse(TcpClient_, json);
                            break;
                        case "get_messages":
                            json = JsonConvert.SerializeObject(GetMessages(DArgs)); // login / List<Message>
                            SendResponse(TcpClient_, json);
                            break;
                        default:
                            SendResponse(TcpClient_, LeavePls());
                            break;
                    }
                }
                TcpClient_.Close();
            }
            catch(Exception ex)
            {
                WriteException(ex.Message);
            }
        }

        // логин
        private string Login(Dictionary<string, string> DArgs) // OK
        {
            Console.WriteLine('\t' + DArgs["login"]); // login
            Console.WriteLine('\t' + DArgs["password"]); // password
            var task = Task.Run(async () => await Program.database.Login(DArgs["login"], DArgs["password"]));
            return task.Result;
        }

        // получаем данные пользователя
        private User GetUserData(Dictionary<string, string> DArgs) // OK
        {
            Console.WriteLine('\t' + DArgs["login"]); // логин
            var task = Task.Run(async () => await Program.database.GetUserData(DArgs["login"]));
            return task.Result;
        }

        // добавляем посещение
        private string AddVisit(Dictionary<string, string> DArgs) // OK
        {
            Console.WriteLine('\t' + DArgs["login"]); // логин
            var task = Task.Run(async () => await Program.database.AddVisit(DArgs["login"]));
            return task.Result;
        }

        // получаем карты
        private List<Card> GetCards(Dictionary<string, string> DArgs) // OK
        {
            Console.WriteLine('\t' + DArgs["login"]); // логин
            var task = Task.Run(async () => await Program.database.GetCards(DArgs["login"]));
            return task.Result;
        }

        // получаем счета
        private List<Bill> GetBills(Dictionary<string, string> DArgs) // OK
        {
            Console.WriteLine('\t' + DArgs["login"]); // id
            var task = Task.Run(async () => await Program.database.GetBills(DArgs["login"]));
            return task.Result;
        }

        // получаем кредиты
        private List<Credit> GetCredits(Dictionary<string, string> DArgs) // OK
        {
            Console.WriteLine('\t' + DArgs["login"]); // id
            var task = Task.Run(async () => await Program.database.GetCredits(DArgs["login"]));
            return task.Result;
        }

        // меняем логин 
        private string ChangeLogin(Dictionary<string, string> DArgs) // OK
        {
            Console.WriteLine('\t' + DArgs["login"]); // логин
            Console.WriteLine('\t' + DArgs["new_login"]); // новый логин
            var task = Task.Run(async () => await Program.database.ChangeLogin(DArgs["login"], DArgs["new_login"]));
            return task.Result;
        }

        // меняем пароль
        private string ChangePassword(Dictionary<string, string> DArgs) // OK
        {
            Console.WriteLine('\t' + DArgs["login"]); // логин
            Console.WriteLine('\t' + DArgs["new_password"]); // новый пароль
            var task = Task.Run(async () => await Program.database.ChangePassword(DArgs["login"], DArgs["new_password"]));
            return task.Result;
        }

        // получаем историю входов
        private List<string> GetAuthHistory(Dictionary<string, string> DArgs) // OK
        {
            Console.WriteLine('\t' + DArgs["login"]); // логин
            var task = Task.Run(async () => await Program.database.GetAuthHistory(DArgs["login"]));
            return task.Result;
        }

        // можем перевести?
        private string CanTransferTo(Dictionary<string, string> DArgs) // OK
        {
            Console.WriteLine('\t' + DArgs["number"]); // номер
            var task = Task.Run(async () => await Program.database.CanTransferTo(DArgs["number"]));
            return task.Result;
        }

        // получаем всю историю операций
        private List<Operation> GetHistory(Dictionary<string, string> DArgs)
        {
            Console.WriteLine('\t' + DArgs["login"]); // login
            var task = Task.Run(async () => await Program.database.GetHistory(DArgs["login"]));
            return task.Result;
        }

        // получаем историю операций по номеру
        private List<Operation> GetPartHistory(Dictionary<string, string> DArgs)
        {
            Console.WriteLine('\t' + DArgs["number"]); // number
            var task = Task.Run(async () => await Program.database.GetPartHistory(DArgs["number"]));
            return task.Result;
        }

        // выполняем операцию
        private string DoTransfer(Dictionary<string, string> DArgs)
        {
            Console.WriteLine('\t' + DArgs["start_number"]); // откуда
            Console.WriteLine('\t' + DArgs["target_number"]); // куда
            DArgs["amount"] = DArgs["amount"].Replace("%2C", ".");
            Console.WriteLine('\t' + DArgs["amount"]); // количество
            var task = Task.Run(async () => await Program.database.DoTransfer(DArgs["start_number"], DArgs["target_number"], DArgs["amount"]));
            return task.Result;
        }

        // блокируем карту
        private string BlockCard(Dictionary<string, string> DArgs)
        {
            Console.WriteLine('\t' + DArgs["number"]); // id
            var task = Task.Run(async () => await Program.database.BlockCard(DArgs["number"]));
            return task.Result;
        }

        // переименовываем карту
        private string RenameCard(Dictionary<string, string> DArgs)
        {
            Console.WriteLine('\t' + DArgs["number"]); // id
            Console.WriteLine('\t' + DArgs["name"]); // id
            var task = Task.Run(async () => await Program.database.RenameCard(DArgs["number"], DArgs["name"]));
            return task.Result;
        }

        // получаем шаблоны
        private List<Pattern> GetPatterns(Dictionary<string, string> DArgs)
        {
            Console.WriteLine('\t' + DArgs["login"]); // id
            var task = Task.Run(async () => await Program.database.GetPatterns(DArgs["login"]));
            return task.Result;
        }

        // создаём шаблон
        private string CreatePattern(Dictionary<string, string> DArgs)
        {
            Console.WriteLine('\t' + DArgs["login"]); // id
            Console.WriteLine('\t' + DArgs["name"]); // название
            Console.WriteLine('\t' + DArgs["from"]); // откуда
            Console.WriteLine('\t' + DArgs["to"]); // откуда
            DArgs["amount"] = DArgs["amount"].Replace(",", ".");
            Console.WriteLine('\t' + DArgs["amount"]); // сколько
            var task = Task.Run(async () => await Program.database.CreatePattern(DArgs["login"], DArgs["name"], DArgs["from"], DArgs["to"], DArgs["amount"]));
            return task.Result;
        }

        // удаляем шаблон
        private string RemovePattern(Dictionary<string, string> DArgs)
        {
            Console.WriteLine('\t' + DArgs["login"]); // login
            Console.WriteLine('\t' + DArgs["name"]); // name
            var task = Task.Run(async () => await Program.database.RemovePattern(DArgs["login"], DArgs["name"]));
            return task.Result;
        }

        // отправляем сообщение

        private string SendMessage(Dictionary<string, string> DArgs)
        {
            Console.WriteLine('\t' + DArgs["from_"]); // from_
            Console.WriteLine('\t' + DArgs["to_"]); // to_
            Console.WriteLine('\t' + DArgs["msg"]); // to_
            var task = Task.Run(async () => await Program.database.SendMessage(DArgs["from_"], DArgs["to_"], DArgs["msg"]));
            return task.Result;
        }

        private List<Message> GetMessages(Dictionary<string, string> DArgs)
        {
            Console.WriteLine('\t' + DArgs["login"]); // from_
            var task = Task.Run(async () => await Program.database.GetMessages(DArgs["login"]));
            return task.Result;
        }

        // выйди, разбойник
        private string LeavePls()
        {
            var html = "<html>" +
                "<head></head>" +
                "<style>" +
                "html {height: 100%;width: 100%}" +
                "body {background: rgb(131,58,180);background: linear-gradient(90deg, rgba(131,58,180,1) 0%, rgba(253,29,29,0.6194852941176471) 50%, rgba(252,176,69,1) 100%);}" +
                "h1 {font-size: 700%;color: white;}" +
                "div {margin: 5%;text-align: center;}" +
                "</style>" +
                "<body>" +
                "<div>" +
                "<h1>Выйдите отсюда, выйди отсюда, разбойник! Плохо слышишь меня? Я спрашиваю, это Вы? Выйди, пожалуйста</h1>" +
                "</div>" + 
                "</body>" +
                "</html>";
            return html;
        }
        //
        //
        //
        // парс UTF-8 string
        private string ParseString(string str)
        {
            str = str.Replace("%25", "%");
            var words = str.Split("+");
            str = "";
            foreach (var word in words)
            {
                if (word.IndexOf('%') == -1)
                {
                    str += word + " ";
                }
                else
                {
                    byte[] bytes = new byte[word.Length / 3];
                    for (int i = 0; i < word.Length / 3; i++)
                    {
                        string buf = word[3 * i + 1].ToString() + word[3 * i + 2].ToString();
                        bytes[i] = Convert.ToByte(Convert.ToInt32(buf, 16));
                    }
                    str += Encoding.UTF8.GetString(bytes) + " ";
                }
            }
            return str[0..^1];
        }
        // ответ сервера
        async private void SendResponse(TcpClient tcpClient, string Response)
        {
            string Str = "HTTP/1.1 200 OK\nContent-type: text/html;charset=utf-8\nContent-Length:" + (Encoding.UTF8.GetBytes(Response)).Length + "\n\n" + Response;
            byte[] Buffer = Encoding.UTF8.GetBytes(Str);
            await tcpClient.GetStream().WriteAsync(Buffer, 0, Buffer.Length);
            tcpClient.Close();
        }

        // получаем аргументы в словарь
        private Dictionary<string, string> GetArgsDict(string Request)
        {
            Dictionary<string, string> Res = new Dictionary<string, string>();
            var lines = Request.Split('&');
            foreach (var line in lines)
            {
                var buf = line.Split('=');
                if (buf[0] != "operation")
                {
                    Res[buf[0]] = ParseString(buf[1]);
                }
                else
                {
                    Res[buf[0]] = buf[1];
                }
            }
            return Res;
        }

        // вывод ошибок
        private void WriteException(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
