using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataManager.Models;
using DataManager.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public ValuesController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [HttpPost("[action]")]
        public void PostByte([FromBody] byte[] value)
        {
            //_databaseService.Create(new Record()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Node = value,
            //    UserId = 1234
            //});
        }

        [HttpPost("[action]")]
        public void PostJson([FromBody]Node jsonResult)
        {
            Request.ToString();
            //Node item = JsonConvert.DeserializeObject<Node>(jsonResult.ToString());
            _databaseService.Create(new Record()
            {
             //   Id = new Random().Next().ToString(),
                Node = jsonResult,
                UserId = 1234
            });
        }
        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
