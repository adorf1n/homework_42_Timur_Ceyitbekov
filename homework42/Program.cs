using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace homework42
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RunServer();

            const int serverPort = 11000;
            const string stopMessage = "Stop";

            void RunServer()
            {
                try
                {
                    ServerWorker();
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            void ServerWorker()
            {
                IPAddress ipAddress = IPAddress.Loopback;
                IPEndPoint endPoint = new IPEndPoint(ipAddress, serverPort);

                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    listener.Bind(endPoint);
                    listener.Listen(10);

                    string request = string.Empty;

                    while (true)
                    {
                        Console.WriteLine($"Wait connection for {listener.LocalEndPoint}");

                        Socket handler = listener.Accept();
                        try
                        {
                            Console.WriteLine("Client connected to server");

                            byte[] bytes = new byte[1024];
                            int bytesCount = handler.Receive(bytes);
                            request = Encoding.UTF8.GetString(bytes, 0, bytesCount);
                            Console.WriteLine($"Receive request: {request}");

                            string reply = HandleRequest(request, listener);

                            byte[] sendingBytes = Encoding.UTF8.GetBytes(reply);
                            handler.Send(sendingBytes);
                        }
                        finally
                        {
                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();
                        }

                        if (request.Contains(stopMessage))
                            break;
                    }
                }
                finally
                {
                    listener.Close();
                }

                Console.WriteLine("Stop server");
            }

            string HandleRequest(string request, Socket listener)
            {
                request = request.ToLower().Trim();

                if (request.Contains("время"))
                {
                    return $"Текущее время: {DateTime.Now:HH:mm}";
                }
                else if (request.Contains("дата"))
                {
                    return $"Текущая дата: {DateTime.Now:yyyy-MM-dd}";
                }
                else if (request.Contains("ip"))
                {
                    return $"IP сервера: {((IPEndPoint)listener.LocalEndPoint).Address}";
                }
                else if (request.Contains("порт"))
                {
                    return $"Порт сервера: {((IPEndPoint)listener.LocalEndPoint).Port}";
                }
                else if (request.Contains("хост"))
                {
                    return $"Имя хоста сервера: {Dns.GetHostName()}";
                }
                else
                {
                    return "Неизвестная команда. Попробуйте: время, дата, ip, порт, хост.";
                }
            }

        }

    }
}
