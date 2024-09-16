using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace homework42client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SendRequest("время");
            SendRequest("дата");
            SendRequest("ip");
            SendRequest("порт");
            SendRequest("хост");
            SendRequest("Stop");

            void SendRequest(string message)
            {
                const int serverPort = 11000;
                const string serverIp = "127.0.0.1";

                TcpClient client = null;
                NetworkStream stream = null;

                try
                {
                    client = new TcpClient(serverIp, serverPort);
                    stream = client.GetStream();

                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine($"Sent: {message}");

                    byte[] responseData = new byte[1024];
                    int bytes = stream.Read(responseData, 0, responseData.Length);
                    string response = Encoding.UTF8.GetString(responseData, 0, bytes);
                    Console.WriteLine($"Received: {response}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e.Message}");
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                    if (client != null)
                        client.Close();
                }
            }
        }
    }
}
