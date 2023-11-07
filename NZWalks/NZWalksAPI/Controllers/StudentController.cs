using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("GetAllStudents")]
        public IActionResult GetAllStudents()
        {
            List<string> names = new List<string>()
            {
                   
            };
            if (names.Count is not 0)
            {
                return Ok(names);
            }
            else
            {
                return BadRequest();
            }   

        }
    }
}
