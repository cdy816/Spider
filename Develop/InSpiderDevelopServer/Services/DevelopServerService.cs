using Cdy.Spider;
using Grpc.Core;
using InSpiderDevelop;
using InSpiderDevelopServer;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InSpiderDevelopServer.Services
{
    public class DevelopServerService :InSpiderDevelopServer.DevelopServer.DevelopServerBase
    {
        private readonly ILogger<DevelopServerService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            LoginResponse re = new LoginResponse() { Token = SecurityManager.Manager.Login(request.UserName, request.Password) };
            return Task.FromResult(re);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> IsAdmin(CommonRequest request, ServerCallContext context)
        {
            if (IsAdmin(request.Token))
            {
                return Task.FromResult(new BoolResponse() { Result = true });
            }
            else
            {
                return Task.FromResult(new BoolResponse() { Result = false });
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> Logout(CommonRequest request, ServerCallContext context)
        {
            SecurityManager.Manager.Logout(request.Token);
            return Task.FromResult(new BoolResponse() { Result = true });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> CanNewMachine(CommonRequest request, ServerCallContext context)
        {
            if (HasNewDatabasePermission(request.Token))
            {
                return Task.FromResult(new BoolResponse() { Result = true });
            }
            else
            {
                return Task.FromResult(new BoolResponse() { Result = false });
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> Save(MachineCommonRequest request, ServerCallContext context)
        {
            if(CheckLoginId(request.Token,request.Machine))
            {
                DevelopManager.Manager.Save(request.Solution,request.Machine);
                return Task.FromResult(new BoolResponse() { Result = true });
            }
            else
            {
              return  Task.FromResult(new BoolResponse() { Result = false });
            }
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> Cancel(MachineCommonRequest request, ServerCallContext context)
        {
            if (CheckLoginId(request.Token, request.Machine))
            {
                DevelopManager.Manager.ReLoadMachine(request.Solution, request.Machine);
                return Task.FromResult(new BoolResponse() { Result = true });
            }
            else
            {
                return Task.FromResult(new BoolResponse() { Result = false });
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool CheckLoginId(string id, string database = "")
        {
            return SecurityManager.Manager.CheckKeyAvaiable(id) && (SecurityManager.Manager.CheckDatabase(id, database) || SecurityManager.Manager.IsAdmin(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsAdmin(string id)
        {
            return SecurityManager.Manager.CheckKeyAvaiable(id) && SecurityManager.Manager.IsAdmin(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool HasNewDatabasePermission(string id)
        {
            return SecurityManager.Manager.CheckKeyAvaiable(id) && (SecurityManager.Manager.HasNewDatabasePermission(id) || SecurityManager.Manager.IsAdmin(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool HasDeleteDatabasePerssion(string id)
        {
            return SecurityManager.Manager.CheckKeyAvaiable(id) && (SecurityManager.Manager.HasDeleteDatabasePermission(id) || SecurityManager.Manager.IsAdmin(id));
        }

        public DevelopServerService(ILogger<DevelopServerService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> IsMachineRunning(MachineCommonRequest request, ServerCallContext context)
        {
            if (!SecurityManager.Manager.CheckKeyAvaiable(request.Token))
            {
                return Task.FromResult(new BoolResponse() { Result = false });
            }
            return Task.FromResult(new BoolResponse() { Result = ServiceLocator.Locator.Resolve<IMachineManager>().IsRunning(request.Solution, request.Machine) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> Stop(MachineCommonRequest request, ServerCallContext context)
        {
            if (!SecurityManager.Manager.CheckKeyAvaiable(request.Token))
            {
                return Task.FromResult(new BoolResponse() { Result = false });
            }
            return Task.FromResult(new BoolResponse() { Result = ServiceLocator.Locator.Resolve<IMachineManager>().Stop(request.Solution, request.Machine) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> Start(MachineCommonRequest request, ServerCallContext context)
        {
            if (!SecurityManager.Manager.CheckKeyAvaiable(request.Token))
            {
                return Task.FromResult(new BoolResponse() { Result = false });
            }
            return Task.FromResult(new BoolResponse() { Result = ServiceLocator.Locator.Resolve<IMachineManager>().Start(request.Solution,request.Machine) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<MachineListResponse> MachineList(MachineListRequest request, ServerCallContext context)
        {
            List<MachineDocument> re = new List<MachineDocument>();
            foreach(var vv in DevelopManager.Manager.ListMachines(request.Solution))
            {
                if(CheckLoginId(request.Token,vv.Name))
                {
                    re.Add(vv);
                }
            }
            MachineListResponse rm = new MachineListResponse();
            foreach(var vv in re)
            {
                rm.Machines.Add(new MachineItem() { Name= vv.Name,Api= vv.Api!=null?vv.Api.SaveToString():"",Device= vv.Device!=null?vv.Device.SaveToString():"",Driver=vv.Driver!=null? vv.Driver.SaveToString():"",Channel=vv.Channel!=null? vv.Channel.SaveToString():"",Link=vv.Link!=null?vv.Link.SaveToString():"" });
            }
            return Task.FromResult(rm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> MachineUpdate(MachineUpdateRequest request, ServerCallContext context)
        {
            if(CheckLoginId(request.Token, request.Machine.Name))
            {
                var re = DevelopManager.Manager.UpdateWithString(request.Machine.Solution,request.Machine.Name, request.Machine.Api, request.Machine.Channel, request.Machine.Device, request.Machine.Driver, request.Machine.Link);
                return Task.FromResult(new BoolResponse() { Result = re });
            }
            return base.MachineUpdate(request, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> MachineDelete(MachineDeleteRequest request, ServerCallContext context)
        {
            if(HasDeleteDatabasePerssion(request.Token)&&CheckLoginId(request.Token,request.Name))
            {
                bool re = DevelopManager.Manager.Remove(request.Solution,request.Name);
                return Task.FromResult(new BoolResponse() { Result = re });
            }
            else
            {
                return Task.FromResult(new BoolResponse() { Result = false });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<MachineNewResponse> MachineNew(MachineNewRequest request, ServerCallContext context)
        {
            if (HasNewDatabasePermission(request.Token))
            {
                var re = DevelopManager.Manager.NewMachine(request.Solution, request.Name);
                return Task.FromResult(new MachineNewResponse() { Result = true,Name=re.Name });
            }
            else
            {
                return Task.FromResult(new MachineNewResponse() { Result = false });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> MachineReName(MachineReNameRequest request, ServerCallContext context)
        {
            if (CheckLoginId(request.Token,request.Token) && CheckLoginId(request.Token, request.OldName))
            {
                bool re = DevelopManager.Manager.ReName(request.Solution,request.OldName,request.NewName);
                return Task.FromResult(new BoolResponse() { Result = re });
            }
            else
            {
                return Task.FromResult(new BoolResponse() { Result = false });
            }
        }


        #region System user

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> NewUser(NewUserRequest request, ServerCallContext context)
        {
            if (!IsAdmin(request.Token))
            {
                return Task.FromResult(new BoolResponse() { Result = false });
            }
            var user = new User() { Name = request.UserName, Password = request.Password };
            SecurityManager.Manager.Securitys.User.AddUser(user);
            SecurityManager.Manager.Save();
            return Task.FromResult(new BoolResponse() { Result = true });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> ReNameUser(ReNameUserRequest request, ServerCallContext context)
        {
            if (!IsAdmin(request.Token))
            {
                return Task.FromResult(new BoolResponse() { Result = false });
            }
            bool re = SecurityManager.Manager.Securitys.User.RenameUser(request.OldName, request.NewName);
            SecurityManager.Manager.RenameLoginUser(request.OldName, request.NewName);
            SecurityManager.Manager.Save();
            return Task.FromResult(new BoolResponse() { Result = re });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> ModifyPassword(ModifyPasswordRequest request, ServerCallContext context)
        {
            var userName = SecurityManager.Manager.GetUserName(request.Token);
            if (SecurityManager.Manager.CheckKeyAvaiable(request.Token))
            {
                if (!(userName == request.UserName && SecurityManager.Manager.CheckPasswordIsCorrect(userName, request.Password)))
                {
                    return Task.FromResult(new BoolResponse() { Result = false });
                }

                var user = SecurityManager.Manager.Securitys.User.GetUser(request.UserName);
                if (user != null)
                {
                    user.Password = request.Newpassword;
                    SecurityManager.Manager.Save();
                    return Task.FromResult(new BoolResponse() { Result = true });
                }
            }
            return Task.FromResult(new BoolResponse() { Result = false });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            if (!IsAdmin(request.Token))
            {
                return Task.FromResult(new BoolResponse() { Result = false });
            }
            var user = SecurityManager.Manager.Securitys.User.GetUser(request.UserName);
            if (user != null)
            {
                user.IsAdmin = request.IsAdmin;
                user.NewMachines = request.NewDatabasePermission;
                user.Machines = request.Database.ToList();
                SecurityManager.Manager.Save();
            }

            return Task.FromResult(new BoolResponse() { Result = true });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> UpdateUserPassword(UpdatePasswordRequest request, ServerCallContext context)
        {
            if (!IsAdmin(request.Token))
            {
                return Task.FromResult(new BoolResponse() { Result = false });
            }
            var user = SecurityManager.Manager.Securitys.User.GetUser(request.UserName);
            if (user != null)
            {
                user.Password = request.Password;
                SecurityManager.Manager.Save();
            }

            return Task.FromResult(new BoolResponse() { Result = true });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<GetUsersReplay> GetUsers(CommonRequest request, ServerCallContext context)
        {
            if (!IsAdmin(request.Token))
            {
                return Task.FromResult(new GetUsersReplay() { Result = false });
            }

            GetUsersReplay re = new GetUsersReplay() { Result = true };
            foreach (var vv in SecurityManager.Manager.Securitys.User.Users)
            {
                var user = new UserMessage() { UserName = vv.Value.Name, IsAdmin = vv.Value.IsAdmin, NewDatabase = vv.Value.NewMachines };
                if (vv.Value.Machines != null)
                    user.Machine.AddRange(vv.Value.Machines);
                re.Users.Add(user);
            }
            return Task.FromResult(re);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<BoolResponse> RemoveUser(RemoveUserRequest request, ServerCallContext context)
        {
            if (!IsAdmin(request.Token))
            {
                return Task.FromResult(new BoolResponse() { Result = false });
            }
            var user = SecurityManager.Manager.Securitys.User.GetUser(request.UserName);
            if (user != null)
            {
                SecurityManager.Manager.Securitys.User.RemoveUser(request.UserName);
                SecurityManager.Manager.Save();
            }
            return Task.FromResult(new BoolResponse() { Result = true });
        }



        #endregion

    }
}