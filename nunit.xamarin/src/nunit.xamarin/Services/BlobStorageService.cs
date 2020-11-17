using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using nunit.xamarin.Helpers;
using Xamarin.Essentials;

namespace nunit.xamarin.Services
{
    public class BlobStorageService
    {
        public BlobStorageService()
        {
        }

        public static async Task performBlobOperation(TestDataBlobReference blobToUpload)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Constants.AzureStorageConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("testrunresults");

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync().ConfigureAwait(false);

            //reading file name & file extention    
            //string file_extension = Path.GetExtension(fileToUpload);
            //string filename_withExtension = Path.GetFileName(fileToUpload);

            //CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filename_withExtension);
            ////cloudBlockBlob.Properties.ContentType = file_extension;

            //// << reading the file as filestream from local machine >>    
            //Stream file = new FileStream(fileToUpload, FileMode.Open);
            //await cloudBlockBlob.UploadFromStreamAsync(file); // << Uploading the file to the blob >>

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobToUpload.Filename);
            await blockBlob.UploadTextAsync(blobToUpload.Json);

            Console.WriteLine("Upload Completed!");
        }
    }
}
