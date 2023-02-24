using Cdy.Spider;
using System;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            if (!Console.IsInputRedirected)
            {
                Console.CancelKeyPress += Console_CancelKeyPress;
                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;


            Console.WriteLine(Res.Get("HelpMsg"));

            if (args.Length>1)
            {
                mRunner = new SpiderRuntime.Runer() { Name = args[1] };
                mRunner.Name = args[1];
                mRunner.Init();
                mRunner.Start();
                Console.Title = "InSpiderRun-" + args[1];
                Task.Run(() => {
                    StartMonitor(args.Length > 1 ? args[1] : "local");
                });
            }

            while (!mIsClosed)
            {
                Console.Write(">");
                if (!Console.IsInputRedirected)
                {
                    while (!Console.KeyAvailable)
                    {
                        if (mIsClosed)
                        {
                            break;
                        }
                        Thread.Sleep(100);
                    }
                }

                if (mIsClosed)
                {
                    break;
                }

                string smd = Console.In.ReadLine();
                string[] cmd = smd.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (cmd.Length == 0) continue;
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
                        string prj;
                        if (cmd.Length > 1)
                        {
                            prj = cmd[1];
                        }
                        else
                        {
                            prj = ListAvaiableProject();
                        }

                        if (!string.IsNullOrEmpty(prj))
                        {
                            if (SpiderRuntime.Runer.CheckNameExit(prj))
                            {
                                if (mRunner == null)
                                    mRunner = new SpiderRuntime.Runer() { Name = prj };
                                if (!mRunner.IsStarted)
                                {
                                    mRunner.Init();
                                    mRunner.Start();
                                }
                                Console.Title = "InSpiderRun-" + prj;

                                StartMonitor(prj);
                            }
                            else
                            {
                                Console.WriteLine(prj + " is not exist!");
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

        private static string ListAvaiableProject()
        {
            var path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location), "Data");
            if(System.IO.Directory.Exists(path))
            {
                var dd = new System.IO.DirectoryInfo(path).GetDirectories();
                if(dd!=null && dd.Length>0)
                {
                    return dd[0].Name;
                }
            }
            return string.Empty;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
        }

        private static string GetHelpString()
        {
            StringBuilder re = new StringBuilder();
            re.AppendLine();
            re.AppendLine("exit                 // stop spider and exit application");
            re.AppendLine("start  project       // start spider project");
            re.AppendLine("stop                 // stop spider ");
            re.AppendLine("h                    // print help message ");
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

        private static void StartMonitor(string name)
        {
            try
            {
                while (!mIsClosed)
                {
                    try
                    {
                        using (var server = new NamedPipeServerStream("InSpider_"+name, PipeDirection.InOut))
                        {
                            server.WaitForConnection();
                            while (!mIsClosed)
                            {
                                try
                                {
                                    if (!server.IsConnected) break;
                                    var cmd = server.ReadByte();
                                    if (cmd == 0)
                                    {
                                        if (mRunner != null && mRunner.IsStarted)
                                        {
                                            mRunner.Stop();
                                        }
                                        mIsClosed = true;
                                        server.WriteByte(1);
                                        server.FlushAsync();
                                        break;
                                        //退出系统
                                    }
                                    else
                                    {

                                    }
                                }
                                catch (Exception)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine(ex.Message);
                    }

                }

            }
            catch (Exception ex)
            {
                LoggerService.Service.Info("Programe", ex.Message);
                //Console.WriteLine(ex.Message);
            }
        }
    }
}
