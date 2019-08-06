using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        // GET api/values/5
        [HttpGet("{vreme}")]
        public ActionResult<string> Get(DateTime dateTime)
        {

            List<ElementP> result = dBS.GetData(dateTime);
            string json = null; // convert result to json 

            return Ok(json);
           
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            ElementP ElP = new ElementP(value); //transform json to ElementP
            dBS.AddData(ElP);
        }

      
    }
}
