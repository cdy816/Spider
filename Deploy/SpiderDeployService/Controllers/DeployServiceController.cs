using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpiderDeployService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeployServiceController : ControllerBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login([FromBody] string user, string password)
        {
            string token = LoginInner(user, password);
            return Ok(token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private string LoginInner(string user, string password)
        {
            return string.Empty;
        }
       
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="files"></param>
        /// <param name="solution"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Upload([FromBody] List<IFormFile> files,  string solution, string token)
        {
            if(string.IsNullOrEmpty(token)||!CheckPermission(token))
            {
                return new ForbidResult();
            }
            string spath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location),"data");
            if(string.IsNullOrEmpty(solution) )
            {
                spath = System.IO.Path.Combine(spath, solution);
            }
            foreach(var vv in files)
            {
                if (vv.Length <= 0) continue;
                var file = System.IO.Path.Combine(spath, vv.FileName);
                await using var stream = new FileStream(file, FileMode.Create);
                await vv.CopyToAsync(stream);
            }
            return Ok(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool CheckPermission(string token)
        {
            return true;
        }

        [HttpPost("Start")]
        public IActionResult Start([FromBody] string token)
        {
            if (string.IsNullOrEmpty(token) || !CheckPermission(token))
            {
                return new ForbidResult();
            }
            return Ok(true);
        }
    }
}
