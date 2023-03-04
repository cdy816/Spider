using System.Threading;
using System;
using Cdy.Spider;
using Cdy.Spider.Common;
using System.Text;
using InSpiderDevelopServer;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO.Pipes;

namespace InSpiderStudioServer
{
    public class Program: InSpiderDevelopServer.IMachineManager
    {
        static bool mIsExited = false;

        static object mLockObj = new object();

        static void Main(string[] args)
        {
            Console.Title = "InSpiderStudioServer";

            LogoHelper.Print();

            //注册日志
            ServiceLocator.Locator.Registor<ILog>(new ConsoleLogger());

            Program pg = new Program();
            //Config.Instance.Load();

            //if (!DBDevelopService.DbManager.Instance.IsLoaded)
            //    DBDevelopService.DbManager.Instance.PartLoad();

            //int port = Config.Instance.GrpcPort;
            //int webPort = Config.Instance.WebApiPort;

            ServiceLocator.Locator.Registor(typeof(InSpiderDevelopServer.IMachineManager), pg);

            bool isNeedMinMode = false;
            if (args.Length > 0)
            {
                for(int i=0;i<args.Length;i++)
                {
                    var agr = args[i];
                    if (agr.StartsWith("Sec="))
                    {
                        SecurityManager.SecurityDataFile= agr.Substring(4);
                    }
                    else if(agr.StartsWith("WorkPath="))
                    {
                        Service.Instanse.WorkPath = agr.Substring("WorkPath=".Length);
                    }
                    else if(agr=="/m")
                    {
                        isNeedMinMode = true;
                    }
                }
            }

            WindowConsolHelper.DisbleQuickEditMode();

            if (isNeedMinMode)
            {
                WindowConsolHelper.MinWindow("InSpiderStudioServer");
            }

            if (!Console.IsInputRedirected)
            {
                Console.CancelKeyPress += Console_CancelKeyPress;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            }
            else
            {
                SecurityManager.IsSubSystemMode = true;
            }

            Service.Instanse.Start(25001);

            Thread.Sleep(100);
            if (!Console.IsInputRedirected)
                OutByLine("", Res.Get("HelpMsg"));

            while (!mIsExited)
            {
                OutInLine("", "");
                var vv = Console.In.ReadLine();

                if (vv != null)
                {
                    string[] cmd = vv.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (cmd.Length == 0) continue;

                    string cmsg = cmd[0].ToLower();

                    if (cmsg == "exit")
                    {
                        if (!Console.IsInputRedirected)
                        {
                            OutByLine("", Res.Get("AppExitHlp"));
                            cmd = Console.ReadLine().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (cmd.Length == 0) continue;
                            if (cmd[0].ToLower() == "y")
                                break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (cmsg == "h")
                    {
                        OutByLine("", GetHelpString());
                    }
                    else if (cmsg == "**")
                    {
                        LogoHelper.PrintAuthor();
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string GetHelpString()
        {
            string str = "{0,-10} {1,-16} {2}";
            StringBuilder re = new StringBuilder();
            re.AppendLine();
            re.AppendLine(string.Format(str, "list", "", "// " + Res.Get("ListDeviceHlp")));
            re.AppendLine(string.Format(str, "exit", "", "// " + Res.Get("Exit")));
            re.AppendLine(string.Format(str, "h", "", "// " + Res.Get("HMsg")));
            return re.ToString();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LoggerService.Service.Erro("InSpiderStudioServer", e.ExceptionObject.ToString());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            mIsExited = true;
            e.Cancel = true;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Res.Get("AnyKeyToExit"));
            Console.ResetColor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public  bool Start(string solution, string name)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    var info = new ProcessStartInfo() { FileName = "InSpiderRun.exe" };
                    info.UseShellExecute = true;
                    info.Arguments = "start " + name+" "+solution;
                    info.WorkingDirectory = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);
                    Process.Start(info).WaitForExit(1000);
                }
                else
                {
                    var info = new ProcessStartInfo() { FileName = "dotnet" };
                    info.UseShellExecute = true;
                    info.CreateNoWindow = false;
                    info.Arguments = "./InSpiderRun.dll start " + name + " " + solution;
                    info.WorkingDirectory = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);
                    Process.Start(info).WaitForExit(1000);
                }


                Console.WriteLine(string.Format(Res.Get("StartMachineSuccessfull"), name));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public  bool Stop(string solution, string name)
        {
            using (var client = new NamedPipeClientStream(".", "InSpider_" +solution+"_" + name, PipeDirection.InOut))
            {
                try
                {
                    client.Connect(2000);
                    client.WriteByte(0);
                    client.FlushAsync();

                    if (OperatingSystem.IsWindows())
                    {
                        client.WaitForPipeDrain();
                    }

                    if (client.IsConnected)
                    {
                        var res = client.ReadByte();
                        int count = 0;
                        while (res == -1)
                        {
                            res = client.ReadByte();
                            count++;
                            if (count > 20) break;
                            Thread.Sleep(100);
                        }
                        if (res == 1)
                        {
                            Console.WriteLine(string.Format(Res.Get("StopMachineSuccessfull"), name));
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(Res.Get("StopMachineFailed"), name) + ex.Message);
                }
                return false;
            }
        }


        public bool IsRunning(string solution, string name)
        {
            lock (mLockObj)
            {
                if (IsMachineRunning(solution,name, out bool isdbrun))
                {
                    return true;
                }
                else if (!isdbrun) return false;
               
                using (var client = new NamedPipeClientStream(".", "InSpider_" + solution + "_" +name, PipeDirection.InOut))
                {
                    try
                    {
                        client.Connect(500);
                        client.Close();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="isdbrun"></param>
        /// <returns></returns>
        public static bool IsMachineRunning(string solution,string database, out bool isdbrun)
        {
            var pps = System.Diagnostics.Process.GetProcessesByName("InSpiderRun");
            if (pps != null && pps.Length > 0)
            {
                foreach (var p in pps)
                {
                    
                    if (string.Compare(p.MainWindowTitle, "InSpiderRun-" + database, true) == 0)
                    {
                        isdbrun = true;
                        return true;
                    }
                }
            }

            var mRunInfoFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location), solution + "_" + database);
            if (!System.IO.File.Exists(mRunInfoFile))
            {
                isdbrun = false;
                return false;
            }
            else
            {
                isdbrun = true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prechar"></param>
        /// <param name="msg"></param>
        private static void OutByLine(string prechar, string msg)
        {
            Console.WriteLine(prechar + ">" + msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prechar"></param>
        /// <param name="msg"></param>
        private static void OutInLine(string prechar, string msg)
        {
            Console.Write(prechar + ">" + msg);
        }


    }
}