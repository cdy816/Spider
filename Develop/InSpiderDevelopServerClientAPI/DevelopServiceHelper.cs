//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/3/28 22:49:54.
//  Version 1.0
//  种道洋
//==============================================================

using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Data.Common;
using Cdy.Spider;
using InSpiderDevelopServer;
using System.Reflection.PortableExecutable;

namespace InSpiderDevelopServerClientAPI
{
    /// <summary>
    /// 
    /// </summary>
    public class DevelopServiceHelper
    {

        #region ... Variables  ...
        
        public static DevelopServiceHelper Helper = new DevelopServiceHelper();

        private InSpiderDevelopServer.DevelopServer.DevelopServerClient mCurrentClient;

        private string mLoginId = string.Empty;

        private object mLockObj = new object();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public bool UseTls
        {
            get;
            set;
        } = true;

        /// <summary>
        /// 
        /// </summary>
        public Action OfflineCallBack { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        private InSpiderDevelopServer.DevelopServer.DevelopServerClient GetServicClient(string ip)
        {
            try
            {
               

                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                
                var httpClient = new HttpClient(httpClientHandler);

                Grpc.Net.Client.GrpcChannel grpcChannel;
                if (UseTls)
                {
                    grpcChannel = Grpc.Net.Client.GrpcChannel.ForAddress(@"https://" + ip + ":25001", new GrpcChannelOptions { HttpClient = httpClient });
                }
                else
                {

                    //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                    grpcChannel = Grpc.Net.Client.GrpcChannel.ForAddress(@"http://" + ip + ":25001", new GrpcChannelOptions { HttpClient = httpClient });
                    //grpcChannel = Grpc.Net.Client.GrpcChannel.ForAddress(@"http://" + ip + ":5001");
                }
                return new InSpiderDevelopServer.DevelopServer.DevelopServerClient(grpcChannel);
            }
            catch(Exception ex)
            {
                LoggerService.Service.Erro("DevelopService", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public bool Save(string database)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    return mCurrentClient.Save(new  InSpiderDevelopServer.MachineCommonRequest { Token = mLoginId,Machine=database }).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,Dictionary<string,string>> LoadMachines(string solution)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    Dictionary<string, Dictionary<string, string>> dtmp = new Dictionary<string, Dictionary<string, string>>();
                    var res = mCurrentClient.MachineList(new InSpiderDevelopServer.MachineListRequest { Token = mLoginId,Solution=solution });
                    foreach ( var item in res.Machines)
                    {
                        Dictionary<string, string> ddd= new Dictionary<string, string>();
                        ddd.Add("Api", item.Api);
                        ddd.Add("Device", item.Device);
                        ddd.Add("Channel", item.Channel);
                        ddd.Add("Driver", item.Driver);
                        ddd.Add("Link", item.Link);
                        dtmp.Add(item.Name, ddd);
                    }
                    return dtmp;
                }
            }
            catch
            {
                Logout();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    return mCurrentClient.IsAdmin(new InSpiderDevelopServer.CommonRequest { Token = mLoginId }).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Tuple<bool, bool, List<string>>> GetUsers()
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    Dictionary<string, Tuple<bool, bool, List<string>>> dd = new Dictionary<string, Tuple<bool, bool, List<string>>>();
                    var re = mCurrentClient.GetUsers(new CommonRequest() { Token = mLoginId }).Users;
                    foreach (var vv in re)
                    {

                        dd.Add(vv.UserName, new Tuple<bool, bool, List<string>>(vv.IsAdmin, vv.NewDatabase, vv.Machine.ToList()));
                    }
                    return dd;
                }
            }
            catch
            {
                Logout();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool AddUser(string name, string password, bool isadmin, bool cannewdatabase)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    return mCurrentClient.NewUser(new NewUserRequest() { Token = mLoginId, UserName = name, Password = password, IsAdmin = isadmin, NewDatabasePermission = cannewdatabase }).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldname"></param>
        /// <param name="newname"></param>
        /// <returns></returns>
        public bool ReNameUser(string oldname, string newname)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    return mCurrentClient.ReNameUser(new ReNameUserRequest() { Token = mLoginId, OldName = oldname, NewName = newname }).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RemoveUser(string name)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    return mCurrentClient.RemoveUser(new RemoveUserRequest() { Token = mLoginId, UserName = name }).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public bool UpdateUser(string name, bool isadmin, bool cannewdatabase, List<string> databases)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    var req = new UpdateUserRequest() { Token = mLoginId, UserName = name, IsAdmin = isadmin, NewDatabasePermission = cannewdatabase };
                    req.Database.AddRange(databases);
                    return mCurrentClient.UpdateUser(req).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false;
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool UpdateUserPassword(string name, string password)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    return mCurrentClient.UpdateUserPassword(new UpdatePasswordRequest() { Token = mLoginId, UserName = name, Password = password }).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false;
        }

        /// <summary>
        /// 设置服务器用户密码
        /// </summary>
        /// <param name="name">用户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public bool ModifyUserPassword(string name, string password, string newpassword)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    return mCurrentClient.ModifyPassword(new ModifyPasswordRequest() { Token = mLoginId, UserName = name, Password = password, Newpassword = newpassword }).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool StartMachine(string solution, string name)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    return mCurrentClient.Start(new InSpiderDevelopServer.MachineCommonRequest { Token = mLoginId, Machine=name, Solution = solution }).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool StopMachine(string solution, string name)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    return mCurrentClient.Stop(new InSpiderDevelopServer.MachineCommonRequest { Token = mLoginId, Machine = name , Solution = solution }).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsMachineRunning(string solution, string name)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    return mCurrentClient.IsMachineRunning(new InSpiderDevelopServer.MachineCommonRequest { Token = mLoginId, Machine = name, Solution = solution }).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldname"></param>
        /// <param name="newname"></param>
        /// <returns></returns>
        public bool ReNameMachine(string solution, string oldname,string newname)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    return mCurrentClient.MachineReName(new InSpiderDevelopServer.MachineReNameRequest { Token = mLoginId,OldName=oldname,NewName=newname, Solution = solution }).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machine"></param>
        /// <returns></returns>
        public bool RemoveMachine(string solution, string machine)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    return mCurrentClient.MachineDelete(new InSpiderDevelopServer.MachineDeleteRequest { Token = mLoginId,Name=machine, Solution = solution }).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machine"></param>
        /// <returns></returns>
        public string NewMachine(string solution,string machine)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    return mCurrentClient.MachineNew(new InSpiderDevelopServer.MachineNewRequest { Token = mLoginId, Name = machine, Solution = solution }).Name;
                }
            }
            catch
            {
                Logout();
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        public bool UpdateMachine(string solution, string name ,Dictionary<string, string> mc)
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    var mr = new MachineUpdateRequest() { Machine = new MachineItem() };
                    mr.Machine.Name= name;
                    mr.Token = mLoginId;
                    mr.Machine.Api = mc["Api"];
                    mr.Machine.Channel = mc["Channel"];
                    mr.Machine.Device = mc["Device"];
                    mr.Machine.Driver = mc["Driver"];
                    mr.Machine.Link = mc["Link"];
                    mr.Machine.Solution = solution;
                    return mCurrentClient.MachineUpdate(mr).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Cancel()
        {
            try
            {
                if (mCurrentClient != null && !string.IsNullOrEmpty(mLoginId))
                {
                    return mCurrentClient.Cancel(new InSpiderDevelopServer.MachineCommonRequest { Token = mLoginId }).Result;
                }
            }
            catch
            {
                Logout();
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public string Login(string ip, string user, string pass)
        {
            mCurrentClient = GetServicClient(ip);
            if (mCurrentClient != null)
            {
                try
                {
                    var lres = mCurrentClient.Login(new LoginRequest() { UserName = user==null?"":user, Password = pass==null?"":pass });
                    if (lres != null)
                    {
                        mLoginId = lres.Token;
                        return lres.Token;
                    }
                }
                catch (Exception ex)
                {
                    LoggerService.Service.Erro("DevelopService", ex.Message);
                }
            }
            return string.Empty;
        }



        /// <summary>
        /// 
        /// </summary>
        public void Logout()
        {
            lock(mLockObj)
            {
                if(!string.IsNullOrEmpty(mLoginId))
                {
                    mLoginId = "";
                    OfflineCallBack?.Invoke();
                }
            }
            
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
