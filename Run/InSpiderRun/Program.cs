using Cdy.Spider;
using System;
using System.Text;
using System.Threading;

namespace InSpiderRun
{
    class Program
    {
        static bool mIsClosed = false;
        static SpiderRuntime.Runer mRunner;
        static void Main(string[] args)
        {
            LogoHelper.Print();
            Console.WriteLine(Res.Get("WelcomeMsg"));

            Console.CancelKeyPress += Console_CancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;


            mRunner = new SpiderRuntime.Runer();
            mRunner.Init();
            mRunner.Start();

            Console.WriteLine(Res.Get("HelpMsg"));

            while (!mIsClosed)
            {
                Console.Write(">");
                while (!Console.KeyAvailable)
                {
                    if (mIsClosed)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
                if (mIsClosed)
                {
                    break;
                }

                string smd = Console.ReadLine();
                string[] cmd = smd.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string scmd = cmd[0].ToLower();
                switch (scmd)
                {
                    case "exit":
                        if (mRunner.IsStarted)
                        {
                            mRunner.Stop();
                        }
                        mIsClosed = true;
                        break;
                    case "stop":
                        if (mRunner.IsStarted)
                        {
                            mRunner.Stop();
                        }
                        break;
                    case "start":
                        if (!mRunner.IsStarted)
                        {
                            mRunner.Start();
                        }
                        break;
                    case "h":
                        Console.WriteLine(GetHelpString());
                        break;
                    case "**":
                        LogoHelper.PrintAuthor();
                        break;
                }
            }
           
        }

        private static string GetHelpString()
        {
            StringBuilder re = new StringBuilder();
            re.AppendLine();
            re.AppendLine("exit   stop spider and exit application");
            re.AppendLine("start   start spider ");
            re.AppendLine("stop    stop spider ");
            re.AppendLine("h       print help message ");
            return re.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (mRunner.IsStarted)
            {
                mRunner.Stop();
            }
            mIsClosed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (mRunner.IsStarted)
            {
                mRunner.Stop();
            }
            mIsClosed = true;
            e.Cancel = true;
        }
    }
}
