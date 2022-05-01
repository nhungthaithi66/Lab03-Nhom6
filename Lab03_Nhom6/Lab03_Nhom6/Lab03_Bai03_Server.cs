﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace Lab03_Nhom6
{
    public partial class Lab03_Bai03_Server : Form
    {
        public Lab03_Bai03_Server()
        {
            InitializeComponent();
        }

        void StartUnsafeThread()
        {
            int bytesReceived = 0;
            byte[] recv = new byte[1024];
            Socket clientSocket;
            try
            {
                Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipepServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
                listenerSocket.Bind(ipepServer);
                MessageBox.Show("Listening!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listenerSocket.Listen(-1);
                clientSocket = listenerSocket.Accept();
                lsvMessage.Items.Add(new ListViewItem("New client connected"));
                while (clientSocket.Connected)
                {
                    string text = "";
                    do
                    {
                        bytesReceived = clientSocket.Receive(recv);
                        text += Encoding.UTF8.GetString(recv);
                    }
                    while (text[text.Length - 1] != '\0');
                    lsvMessage.Items.Add(new ListViewItem(text));
                }
                listenerSocket.Close();
            }
            catch
            {
                MessageBox.Show("Listening!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Thread serverThread = new Thread(new ThreadStart(StartUnsafeThread));
            serverThread.Start();
            btnListen.Enabled = false;
        }
    }
}
