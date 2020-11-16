using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Xamarin.Essentials;

namespace nunit.xamarin.Services
{
    public class BlobStorageService
    {
        public BlobStorageService()
        {
        }

        public static async Task performBlobOperation(string fileToUpload)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Constants.AzureStorageConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("testcontainer");

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();

            //reading file name & file extention    
            string file_extension = Path.GetExtension(fileToUpload);
            string filename_withExtension = Path.GetFileName(fileToUpload);

            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filename_withExtension);
            //cloudBlockBlob.Properties.ContentType = file_extension;

            // << reading the file as filestream from local machine >>    
            Stream file = new FileStream(fileToUpload, FileMode.Open);
            await cloudBlockBlob.UploadFromStreamAsync(file); // << Uploading the file to the blob >>  

            Console.WriteLine("Upload Completed!");



            // Retrieve reference to a blob named "myblob".
            //CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            //blockBlob.Properties.ContentType = "xml";

            //// Create the "myblob" blob with the text "Hello, world!"
            //await blockBlob.UploadTextAsync("Hello, world!");


        }
    }
}
