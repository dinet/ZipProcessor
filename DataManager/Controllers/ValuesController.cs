using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataManager.Helpers;
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
        private EncryptionHelper _encryptionHelper;

        public ValuesController(DatabaseService databaseService, EncryptionHelper encryptionHelper)
        {
            _databaseService = databaseService;
            _encryptionHelper = encryptionHelper;
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
        public void PostJson()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                string plainText = reader.ReadToEnd();
                byte[] array = Convert.FromBase64String(plainText);
                string json = _encryptionHelper.Decrypt(array);
                Node item = JsonConvert.DeserializeObject<Node>(json);
                _databaseService.Create(new Record()
                {
                    //   Id = new Random().Next().ToString(),
                    Node = item,
                    UserId = 1234
                });
            }

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
