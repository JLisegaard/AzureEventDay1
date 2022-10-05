using System;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using Azure.Storage.Blobs;
using static System.Net.Mime.MediaTypeNames;

namespace AzureKursusfunction
{
    public static class FileUploadFunction
    {
        [FunctionName("FileUpload")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var containerName = Environment.GetEnvironmentVariable("ContainerName");
            var conn = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            try
            {
                //Get File from request and display name and size
                var formdata = await req.ReadFormAsync();
                var file = req.Form.Files["file"];

                //Now we have the file as a pointer to a datastream
                //Create an image from the file variable

                // --- Hint you could use the System.Drawing.Image
                // --- And convert the file variable to a stream

                // --- For resizing on could use new Bitmap(image, size)
                // --- Other example could be the ImageSharp package from nuget.org                                

                // --- Save the resized image to a variable of type: MemoryStream (var imageStream = new MemoryStream())
                // --- OPS. set stream position to 0 before uploading image

                // We need to save the image to azure,
                // and we've chosen to save it in a blobstorage
                // comment here

                // --- First create a BlobServiceClient (from the Azure.Storage.Blobs library)
                // --- From the BlobServiceClient Create a new Blob container with a proper name (like pics, images, etc.)

                // When the blob is created we need to uploade the image to it

                // --- Use the BlobContainerClient (From same library as before) to first
                // --- get the blob client using .GetBlobClient
                // --- afterwards Upload the file to the blob

                var image = Image.FromStream(file.OpenReadStream());
                var resized = new Bitmap(image, new Size(256, 256));

                using var imageStream = new MemoryStream();
                resized.Save(imageStream, ImageFormat.Jpeg);
                imageStream.Position = 0;

                var blobService = new BlobServiceClient(conn);
                var blobClient = new BlobContainerClient(conn, containerName);
                var blob = blobClient.GetBlobClient(file.FileName);
                await blob.UploadAsync(imageStream);

                return new OkObjectResult(file.FileName + " - " + file.Length.ToString() + " bytes");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}