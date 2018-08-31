using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
//using ImageMagick;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using System;
using SixLabors.ImageSharp.Processing.Processors.Effects;
using SixLabors.ImageSharp.Processing;


namespace imageprocessorazure
{
    public static class imageprocess
    {
        [FunctionName("imageprocess")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            byte[] photoBytes = Convert.FromBase64String(requestBody);
           
            MemoryStream memoryStream = new MemoryStream();
            using (Image<Rgba32> image = Image.Load<Rgba32>(photoBytes)){
                image.Mutate(x => x.Grayscale());
                image.SaveAsJpeg(memoryStream); 
            }
            // image
            //       .Resize(new ResizeOptions
            // {
            //     Size = new Size(100, 100),
            //     Mode = ResizeMode.Max
            // });
            //   var image = new Image(photoBytes)
            // .Resize(new ResizeOptions
            // {
            //     Size = new Size(100, 100),
            //     Mode = ResizeMode.Max
            // });
                               
            
            byte[] outputimagebyte = memoryStream.ToArray();
            string outStream = Convert.ToBase64String(outputimagebyte);
                       
            return outStream != null
                ? (ActionResult)new OkObjectResult($"{outStream}")
                : new BadRequestObjectResult("Please pass a image on the query string or in the request body");
        }
    }
}
