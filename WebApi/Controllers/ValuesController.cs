using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Elements;
using WebApi.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IDatabaseService dBS;

        public ValuesController(IDatabaseService ser)
        {
            dBS = ser;
        }

        [HttpGet]
        [Route("message")]
        public string getSomeString()
        {
            Console.WriteLine("BASIC GET \n\n\n");
            return "some string ";
        }

        // GET api/values/5
        [HttpGet("{vreme}", Name = "Get")]
        public ActionResult<string> Get(string vreme)
        {
            Console.WriteLine($"\n\n\nGET BY TIME REQUEST: {vreme.ToString()}\n\n\n");
            List<ElementP> result = dBS.GetData(vreme);
            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] ElementP value)
        {
            Console.WriteLine($"\n\n\nPOST REQUEST WITH VALUE : {value}\n\n\n");
            dBS.AddData(value);
        }
    }
}
