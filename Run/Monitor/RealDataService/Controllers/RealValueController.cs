using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealDataService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealValueController : ControllerBase
    {
        [HttpGet]
        public List<RealValueItem> Get([FromBody] RealValueRequest request)
        {

            return null;
        }

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
