using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1
{
    public class AzureStorage
    {
        private string url = "https://cinetecrestservice.blob.core.windows.net/cinetec-images/";
        private static string accountName = "cinetecrestservice";
        private static string accessKey = "F6hbRO/6tBWwfSHkJi3HfBIjTr7Ki8+P8WAo1QQ/SkVEj/2vJeCOyJXNOwBRXSkbYSZ4BcleAvWwCR86KD0flA==";

        /*public void load()
        {
            try
            {
                StorageCredentials credentials = new StorageCredentials(accountName, accessKey);
                CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, useHttps: true);
                CloudBlobClient client = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference("cinetec-images");
                container.CreateIfNotExists();
                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                CloudBlockBlob cloudBlock = container.GetBlockBlobReference("imagen.png");
                using (Stream file = System.IO.File.OpenRead(""))
                {
                    cloudBlock.UploadFromStream();
                }
                
            }
            catch(Exception e)
            {

            }
        }*/

    }
}