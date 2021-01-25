using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

namespace BackEnd
{
    class Server
    {
        // переменные
        public static TcpListener Listener; // Объект, принимающий TCP-клиентов
        // конструктор
        public Server(int Port)
        {
            Listener = new TcpListener(IPAddress.Any, Port); // Создаем "слушателя" для указанного порта
            Listener.Start(); // Запускаем его
            Console.WriteLine("OK");
            while (true)
            {
                // Принимаем новых клиентов и передаем их на обработку новому экземпляру класса Client
                //ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), Listener.AcceptTcpClient());
                new Client(Listener.AcceptTcpClient());
            }
        }

        // ???
        static void ClientThread(Object StateInfo)
        {
            //// Просто создаем новый экземпляр класса Client и передаем ему приведенный к классу TcpClient объект StateInfo
            //new Client((TcpClient)StateInfo);
            TcpClient Client = Listener.AcceptTcpClient();
            // Создаем поток
            Thread Thread = new Thread(new ParameterizedThreadStart(ClientThread));
            // И запускаем этот поток, передавая ему принятого клиента
            Thread.Start(Client);
        }

        // деструктор
        ~Server()
        {
            // Если "слушатель" был создан
            if (Listener != null)
            {
                // Остановим его
                Listener.Stop();
            }
        }
    }
}
