using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace FS1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Starter();
        }
        IPAddress PC;
        int pcPort;
        IPAddress Mob;
        int mobPort;
        private Socket udpSock;
        private byte[] buffer;
        public void Starter()
        {
            //Setup the socket and message buffer
            udpSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSock.Bind(new IPEndPoint(IPAddress.Any, 12345));
            buffer = new byte[1024];

            //Start listening for a new message.
            EndPoint newClientEP = new IPEndPoint(IPAddress.Any, 0);
            udpSock.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref newClientEP, DoReceiveFrom, udpSock);
        }
        public void sendBackHolePunching()
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

           // IPAddress serverAddr = IPAddress.Parse(ip);

            IPEndPoint endPoint = new IPEndPoint(PC, pcPort);
            IPEndPoint endPoint1 = new IPEndPoint(Mob, mobPort);
          
            byte[] send_buffer = Encoding.ASCII.GetBytes(Mob + ":" + mobPort);
            byte[] send_buffer1 = Encoding.ASCII.GetBytes(PC + ":" + pcPort);
            sock.SendTo(send_buffer, endPoint);
            sock.SendTo(send_buffer1, endPoint1);
        }
        private void DoReceiveFrom(IAsyncResult iar)
        {
            try
            {
                //Get the received message.
                Socket recvSock = (Socket)iar.AsyncState;
                EndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);
                int msgLen = recvSock.EndReceiveFrom(iar, ref clientEP);
                byte[] localMsg = new byte[msgLen];
                Array.Copy(buffer, localMsg, msgLen);

                //Start listening for a new message.
                EndPoint newClientEP = new IPEndPoint(IPAddress.Any, 0);
                udpSock.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref newClientEP, DoReceiveFrom, udpSock);

                //Handle the received message
                
                int port = ((IPEndPoint)clientEP).Port;
                IPAddress myip = IPAddress.Parse(((IPEndPoint)clientEP).Address.ToString());
               
                MessageBox.Show("Received Connection: \nIP: " + ((IPEndPoint)clientEP).Address + "\n Port: " + ((IPEndPoint)clientEP).Port + "\n\n");
                //Storing Target Ip and port
                String recMsg = System.Text.Encoding.UTF8.GetString(localMsg);
                if (recMsg == "PC")
                {
                    PC = myip;
                    pcPort = port;

                }
                else
                    if (recMsg == "MOB")
                    {
                        Mob = myip;
                        mobPort = port;
                    }

                if (PC != null && Mob != null)
                {
                    sendBackHolePunching();
                }



                //END
                //Do other, more interesting, things with the received message.
                
                
            }
            catch (ObjectDisposedException)
            {
                //expected termination exception on a closed socket.
                // ...I'm open to suggestions on a better way of doing this.
            }
        }

        
    }
}
