using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ZXing.QrCode;
using System.Drawing;
using System.IO;
using ZXing;
using System.Net.Http;
using System.Reflection.Emit;

namespace QRCode.Tags
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("qrcode")]
    public class QRCodeTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var content = context.AllAttributes["content"].Value.ToString();
            var width = int.Parse(context.AllAttributes["width"].Value.ToString());
            var height = int.Parse(context.AllAttributes["height"].Value.ToString());
            var barcodeWriterPixelData = new ZXing.BarcodeWriterPixelData {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = 0
                }
            };
            var pixelData = barcodeWriterPixelData.Write(content);
            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                using (var memoryStream = new MemoryStream())
                {
                    var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    try
                    {
                        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                    }
                    finally
                    {
                        bitmap.UnlockBits(bitmapData);
                    }
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    output.TagName = "img";
                    output.Attributes.Clear();
                    output.Attributes.Add("width", width);
                    output.Attributes.Add("height", height);
                    output.Attributes.Add("src", String.Format("data:image/png;base64,{0}",Convert.ToBase64String(memoryStream.ToArray())));

                    //Without Razor
                    //IBarcodeWriter barCodeWriter = new IBarcodeWriter();
                    //var writerSvg = new ZXing.IBarcodeWriterSvg
                }
            }
        }
    }
    [HtmlTargetElement("serve")]
    public class CloudTagHelper : TagHelper
    {
        public async override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string endpoint = "http://localhost:30752/CC/api/v1_3/PackagedDrugs/00002300475/QRCodeLink/?CallSystemName=Dr.+Sprenkle+EHR&CallID=1234&DeptName=Neurology&returnSSLLink=false";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(endpoint);
            client.DefaultRequestHeaders.Add("Accept", "image/jpeg");
            client.DefaultRequestHeaders.Add("Authorization", "SHAREDKEY 4:2uMA90Y/9UZ+FYC9S8p0mH/U1+DUcK14SWDDvSXAbio=");
            HttpResponseMessage response = await client.GetAsync(endpoint);
            string thing = response.ToString();
            output.TagName = "p";
            output.Attributes.Clear();
            output.Attributes.Add("src", thing);

        }
    }
}
