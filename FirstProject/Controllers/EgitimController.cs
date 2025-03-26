using FirstProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EgitimController : ControllerBase
    {
        [HttpGet("TestMethod")]
        public ResponseModel TestMethod()
        {
            return new ResponseModel()
            {
                StatusCode = 200,
                Description = "Başarılı",
            };
        }
    }
}