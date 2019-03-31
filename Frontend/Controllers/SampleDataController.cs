using Microsoft.AspNetCore.Mvc;
using Frontend.Helpers;

namespace Frontend.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {

        public SampleDataController(EncryptionHelper encryptionHelper)
        {
        }

        //[HttpPost("[action]"), DisableRequestSizeLimit]
        //public ActionResult UploadFile()
        //{
        //    var file = Request.Form.Files[0];
        //    if (file.Length > 0)
        //    {
                
        //    }
        //}
    }
}
