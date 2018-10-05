using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace QRCode.Controllers
{
    //[Route("demo")]
    public class DemoController : Controller
    {
        /*[Route("index")]
        public IActionResult Index()
        {
            return View();
        }*/

        //[Route("index")]
        public IActionResult index(string productId)
        {
            ViewBag.productId = productId;
            return View("Index");
        }
        [Route("demo")]
        public ActionResult GetImage()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("hello", QRCodeGenerator.ECCLevel.Q);
            QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData);
            Bitmap qrCodeImageBitMap = qrCode.GetGraphic(20);
            byte[] imgData;
            using (var stream = new MemoryStream())
            {
                qrCodeImageBitMap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                imgData = stream.ToArray();
            }
            return File(imgData, "image/png");
        }
        /*
                [Route("serve")]
                public async Task<ActionResult> GetFromCloud()
                {
                    //while(IsC)
                }
                 */

        [Route("serve")]
        public async Task<HttpResponseMessage> Get()
        {
            string endpoint = "http://localhost:30752/CC/api/v1_3/PackagedDrugs/00002300475/QRCodeLink/?CallSystemName=Dr.+Sprenkle+EHR&CallID=1234&DeptName=Neurology&returnSSLLink=false";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(endpoint);
            client.DefaultRequestHeaders.Add("Accept", "image/jpeg");
            client.DefaultRequestHeaders.Add("Authorization", "SHAREDKEY 4:2uMA90Y/9UZ+FYC9S8p0mH/U1+DUcK14SWDDvSXAbio=");
            HttpResponseMessage response = await client.GetAsync(endpoint);
            return null;
        }
    }

}