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

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
           
            
            if(args.Length>0)
            {
                mRunner = new SpiderRuntime.Runer() { Name = args[1] };
                mRunner.Name = args[0];
                mRunner.Init();
                mRunner.Start();
            }

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
                        if (mRunner!=null && mRunner.IsStarted)
                        {
                            mRunner.Stop();
                        }
                        mIsClosed = true;

                        break;
                    case "stop":
                        if (mRunner != null && mRunner.IsStarted)
                        {
                            mRunner.Stop();
                        }
                        break;
                    case "start":
                        if (cmd.Length > 0)
                        {
                            if (mRunner == null)
                                mRunner = new SpiderRuntime.Runer() { Name = cmd[1] };
                            if (!mRunner.IsStarted)
                            {
                                mRunner.Init();
                                mRunner.Start();
                            }
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

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
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
            if (mRunner!=null && mRunner.IsStarted)
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
            if (mRunner != null && mRunner.IsStarted)
            {
                mRunner.Stop();
            }
            mIsClosed = true;
            e.Cancel = true;
        }
    }
}
