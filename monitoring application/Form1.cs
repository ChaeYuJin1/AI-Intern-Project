using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using System.Net.Sockets;
using System.IO.Ports;
using System.IO;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using Size = System.Drawing.Size;

namespace helmet_manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetComponent();
            HandleClient.SetLogMsg += new HandleClient.SetLogHandler(EVENT_SET_LOG_MSG);
            HandleClient.ChangeImg += new HandleClient.ChangeImgHandler(CHANGE_IMG);

            StartServer();
            timer1.Start();
            

        }

        Label[] lb_state = new Label[4];
        OpenCvSharp.UserInterface.PictureBoxIpl[] pbi = new OpenCvSharp.UserInterface.PictureBoxIpl[4];

        private void SetComponent()
        {
            lb_state[0] = lb_state1;
            lb_state[1] = lb_state2;
            lb_state[2] = lb_state3;
            lb_state[3] = lb_state4;

            pbi[0] = pbi1;
            pbi[1] = pbi2;
            pbi[2] = pbi3;
            pbi[3] = pbi4;
        }

        public void CHANGE_IMG(Image img, int channel)
        {
            Size resize = new Size(640, 360);
            Bitmap resizeImage = new Bitmap(img, resize);
            int idx = channel - 1;

            HandleInvoke(() =>
            {
                lb_state[idx].Visible = false;
            });
            pbi[idx].Image = resizeImage;
        }

        const bool SAFETY = true;
        const bool DANGER = false;
        const int DECISION_NUM = 30;

        bool[] current_state = new bool[4] { SAFETY, SAFETY, SAFETY, SAFETY };
        bool[] prev_state = new bool[4] { SAFETY, SAFETY, SAFETY, SAFETY };
      
        int[] danger_count = new int[4] {0, 0, 0, 0};
        int[] safety_count = new int[4] { 0, 0, 0, 0 };
       

        public void EVENT_SET_LOG_MSG(string msg, int channel)
        {
            int idx = channel - 1;

            if (msg == "safety") safety_count[idx]++;
            else if (msg == "danger") danger_count[idx]++;
            else
            {
                safety_count[idx] = 0;
                danger_count[idx] = 0;

                HandleInvoke(() =>
                {
                    lb_state[idx].Text = "이상 없음";
                    lb_state[idx].ForeColor = Color.White;
                    lb_state[idx].Visible = true;
                    pbi[idx].Image = null;
                });
            }

            if((safety_count[idx] + danger_count[idx]) > DECISION_NUM)
            {
                if (safety_count[idx] > danger_count[idx]) current_state[idx] = SAFETY;
                else current_state[idx] = DANGER;

                safety_count[idx] = 0;
                danger_count[idx] = 0;
            }

            if(prev_state[idx] == SAFETY && current_state[idx] == DANGER)
            {
                HandleInvoke(() =>
                {
                    tbOutput.AppendText("[" + DateTime.Now + "]" + System.Environment.NewLine + "구역" + channel.ToString() + " - 안전모 미착용 감지" + System.Environment.NewLine);
                });

                prev_state[idx] = DANGER;
            }
            else if(prev_state[idx] == DANGER && current_state[idx] == SAFETY)
            {
                HandleInvoke(() =>
                {
                    tbOutput.AppendText("[" + DateTime.Now + "]" + System.Environment.NewLine + "구역" + channel.ToString() + " - 미착용 감지 해제" + System.Environment.NewLine);
                });

                prev_state[idx] = SAFETY;
            }

            comm_sec[idx] = 0;
            comm_state[idx] = CONN;
            
        }

        void HandleInvoke(Action action)
        {
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }
        public void updateUI(string message)
        {
            Func<int> del = delegate ()
            {
                tbOutput.AppendText(message + System.Environment.NewLine);
                return 0;
            };
            Invoke(del);
        }

        const int PORT = 8888;
        TcpListener serverSocket = new TcpListener(PORT);
        Thread opThread;
        bool _isOperatingServer = true;
        private void StartServer()
        {
            opThread = new Thread(Control_TCP);
            opThread.Start();
        }

        private void Control_TCP()
        {
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;
            

            try
            {
                serverSocket.Start();
            }
            catch (Exception e)
            {
                _isOperatingServer = false;
                MessageBox.Show("서버 프로그램이 구동 중입니다.");
                Console.WriteLine("에러: 이미 서버 프로그램이 구동 중입니다.");
                Application.Exit();
            }
            Console.WriteLine(" >> " + "Server Started");

            while (_isOperatingServer)
            {
                try
                {
                    clientSocket = serverSocket.AcceptTcpClient();
                }
                catch (Exception e)
                {
                    _isOperatingServer = false;
                    break;
                }
                counter += 1;
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");

                HandleClient client = new HandleClient();

                //client.Owner = this;
                client.startClient(clientSocket, Convert.ToString(counter));
            }
        }

        public class HandleClient
        {

            public delegate void SetLogHandler(string message, int channel);
            public static event SetLogHandler SetLogMsg;
            public delegate void ChangeImgHandler(Image img, int channel);
            public static event ChangeImgHandler ChangeImg;
            
         
            TcpClient clientSocket;
            string clNo;
            public void startClient(TcpClient inClientSocket, string clineNo)
            {
                this.clientSocket = inClientSocket;
                this.clNo = clineNo;
               
                Thread ctThread = new Thread(Receive);
                ctThread.Start();
            }

            
            public Mat frame = new Mat();
            bool Connected;

            int temp_count = 0;
            private void Receive()
            {
                byte[] buff = new byte[100000];
                byte[] buff2 = new byte[100];

                NetworkStream stream = clientSocket.GetStream();
                StreamReader Reader = new StreamReader(stream);
                StreamWriter Writer = new StreamWriter(stream);
                
                Connected = true;
                
                Array.Clear(buff2, 0, buff2.Length);
                int size = stream.Read(buff2, 0, 100);
                Console.WriteLine(size.ToString());
               
                Writer.Write('O');
                Writer.Flush();

                int err_count = 0;
                int channel = 1;
                while (Connected)
                {
                    if (stream.CanRead)
                    {
                        try
                        {
                            Array.Clear(buff, 0, buff.Length);
                            int tempInt1 = stream.Read(buff, 0, buff.Length);

                            Console.Write("len: ");
                            Console.WriteLine(tempInt1.ToString());

                            if(48 < buff[0] && buff[0] < 53)
                            {
                                string recv_state = ByteToString(buff);
                                recv_state = recv_state.Remove(tempInt1);
                                Console.WriteLine(recv_state);

                                channel = recv_state[0] - 48;
                                string state_msg = recv_state.Remove(0, 1);

                                SetLogMsg(state_msg, channel);

                                if (state_msg == "check")
                                {
                                    Console.WriteLine("conn check");
                                    Writer.Write('O');
                                    Writer.Flush();
                                }
                                else
                                {
                                    Writer.Write('A');
                                    Writer.Flush();
                                }
                            }
                            else
                            {
                                Image x = (Bitmap)((new ImageConverter()).ConvertFrom(buff));
                                ChangeImg(x, channel);

                                Writer.Write('B');
                                Writer.Flush();
                            }

                            err_count = 0;
                        }
                        catch
                        {
                            err_count++;
                            if (err_count > 10) Connected = false;
                        }
                    }
                }
            }

            private string ByteToString(byte[] strByte)
            {
                string str = Encoding.Default.GetString(strByte);
                return str;
            }
        }
      
        int[] comm_sec = new int[4] { 0, 0, 0, 0 };

        const bool CONN = true;
        const bool DISCN = false;

        bool[] comm_state = new bool[4] {CONN, CONN, CONN, CONN};


        const int CONN_CHECK_SEC = 50;
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                comm_sec[i]++;
                if (comm_sec[i] > CONN_CHECK_SEC)
                {
                    comm_state[i] = DISCN;
                    comm_sec[i] = 0;
                    HandleInvoke(() =>
                    {
                        lb_state[i].Text = "통신 불량";
                        lb_state[i].ForeColor = Color.Gray;
                        lb_state[i].Visible = true;
                        pbi[i].Image = null;
                    });
                }
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            tbOutput.Text = "";
        }


        private void btn_save_Click(object sender, EventArgs e)
        {
            sfd.FileName = "[" + DateTime.Now.ToString() + "] Alarm Log.txt";
            sfd.FileName = sfd.FileName.Replace(':', ';');
            sfd.Title = "저장경로 지정하세요";
            sfd.OverwritePrompt = true;
            sfd.InitialDirectory = "d:\\";
            sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.FilterIndex = 2;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(sfd.OpenFile());
                writer.Write(tbOutput.Text);
                writer.Dispose();
                writer.Close();
            }
        }
    }
}
