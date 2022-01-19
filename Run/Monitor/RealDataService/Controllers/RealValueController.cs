using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cdy.Spider.RealDataService
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealValueController : ControllerBase
    {
        /// <summary>
        /// 获取变量的实时值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public RealValueResult Get([FromBody] RealValueRequest request)
        {
            RealValueResult rr = new RealValueResult();
            if (UserConfigDocument.ConfigInstance.CheckUser(request.UserName,request.Password))
            {
                var service = Cdy.Spider.ServiceLocator.Locator.Resolve<Cdy.Spider.IRealDataService>();
                if (service != null)
                {
                    var vals = service.GetDeviceALLTagValues(request.DeviceName);
                    if (vals != null)
                    {
                        List<RealValueItem> re = new List<RealValueItem>();
                        foreach (var vv in vals)
                        {
                            re.Add(new RealValueItem() { TagName = vv.Key, Value = vv.Value.Item1, Quality = vv.Value.Item2 });
                        }
                    }
                }
                else
                {
                    rr.Result = false;
                    rr.ErroMessage = "service not ready";
                }
            }
            else
            {
                rr.Result = false;
                rr.ErroMessage = "login failed";
            }
            return rr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetTagValues")]
        public RealValueResult GetTagValues([FromBody] RealValueRequestByTagName request)
        {
            RealValueResult rr = new RealValueResult();
            if (UserConfigDocument.ConfigInstance.CheckUser(request.UserName, request.Password))
            {
                var service = Cdy.Spider.ServiceLocator.Locator.Resolve<Cdy.Spider.IRealDataService>();
                if (service != null)
                {
                    var vals = service.GetTagValues(request.DeviceName,request.Tags);
                    if (vals != null)
                    {
                        List<RealValueItem> re = new List<RealValueItem>();
                        foreach (var vv in vals)
                        {
                            re.Add(new RealValueItem() { TagName = vv.Key, Value = vv.Value.Item1, Quality = vv.Value.Item2 });
                        }
                    }
                }
                else
                {
                    rr.Result = false;
                    rr.ErroMessage = "service not ready";
                }
            }
            else
            {
                rr.Result = false;
                rr.ErroMessage = "login failed";
            }
            return rr;
        }

    }

    public class RealValueResult
    {
        public bool Result { get; set; }

        public string ErroMessage { get; set; }


        public List<RealValueItem> Value { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class RealValueRequest
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string  UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RealValueRequestByTagName
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Tags { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class RealValueItem
    {
        /// <summary>
        /// 变量名称
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 质量戳
        /// </summary>
        public byte Quality { get; set; }
    }
}
