using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDietPlan.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

public class ImportPic
{

    public static void Mains(string filepath, string username)
    {
        Task.Run(() => Main1Async(filepath, username)).GetAwaiter().GetResult();
    }

    static async void Main1Async(string filepath, string username)
    {
        var storageCredentials = new StorageCredentials("dietplannerresources", "sgMzUOBlZ1HpKC08yZjdAoA3TrBvyeo+QDFWfkQ2gz8NRG2SrIsXbt3CTtCgLSwslbVwTM4w41KynDlZ44RXkA==");
        var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
        var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

        var container = cloudBlobClient.GetContainerReference("eathealthyimages");
        await container.CreateIfNotExistsAsync();
        var newBlob = container.GetBlockBlobReference(username);
        await newBlob.UploadFromFileAsync(filepath);

    }


}

