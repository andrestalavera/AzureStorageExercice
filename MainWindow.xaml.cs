using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace AzureStorageExercice
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void UploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog(this);
            Stream stream = openFileDialog.OpenFile();
            var config = new
            {
                AccountName = "softeamandrestalavera001",
                ImageContainer = "demo001",
                AccountKey = "T3ZmtK4kCoUUKWdiMTb2zYSHaWifC8Ikd4lqAFr487YpFxNLxCfTl5GCaLPJP8pIlNGOtcOcUnkx87JwAyhwyw=="
            };

            var fileName = Path.GetFileName(openFileDialog.FileName);

            var blobUri = new Uri("https://" +
                          config.AccountName +
                          ".blob.core.windows.net/" +
                          config.ImageContainer +
                          "/" + fileName);

            var storageCredentials = new StorageSharedKeyCredential(config.AccountName, config.AccountKey);
            var blobClient = new BlobClient(blobUri, storageCredentials);

            await blobClient.UploadAsync(stream, overwrite: true);
        }

        private async void ViewFilesButton_Click(object sender, RoutedEventArgs e)
        {
            var config = new
            {
                StorageAccountName = "softeamandrestalavera001",
                STORAGE_ABSOLUTE_URI = "https://softeamandrestalavera001.blob.core.windows.net/",
                STORAGE_ACCOUNT_CONTAINER = "demo001",
                STORAGE_ACCOUNT_ACCESS_KEY = "T3ZmtK4kCoUUKWdiMTb2zYSHaWifC8Ikd4lqAFr487YpFxNLxCfTl5GCaLPJP8pIlNGOtcOcUnkx87JwAyhwyw=="
            };
            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(config.StorageAccountName, config.STORAGE_ACCOUNT_ACCESS_KEY);
            UriBuilder uriBuilder = new UriBuilder(config.STORAGE_ABSOLUTE_URI);
            BlobServiceClient serviceClient = new BlobServiceClient(uriBuilder.Uri, sharedKeyCredential);
            BlobContainerClient container = serviceClient.GetBlobContainerClient(config.STORAGE_ACCOUNT_CONTAINER);

            FilesListView.Items.Clear();
            await foreach (BlobItem blobItem in container.GetBlobsAsync())
            {
                FilesListView.Items.Add(blobItem);
            }
        }

        private async void DeleteSelectedFileButton_Click(object sender, RoutedEventArgs e)
        {
            var config = new
            {
                StorageAccountName = "softeamandrestalavera001",
                STORAGE_ABSOLUTE_URI = "https://softeamandrestalavera001.blob.core.windows.net/",
                STORAGE_ACCOUNT_CONTAINER = "demo001",
                STORAGE_ACCOUNT_ACCESS_KEY = "T3ZmtK4kCoUUKWdiMTb2zYSHaWifC8Ikd4lqAFr487YpFxNLxCfTl5GCaLPJP8pIlNGOtcOcUnkx87JwAyhwyw=="
            };
            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(config.StorageAccountName, config.STORAGE_ACCOUNT_ACCESS_KEY);
            UriBuilder uriBuilder = new UriBuilder(config.STORAGE_ABSOLUTE_URI);
            BlobServiceClient serviceClient = new BlobServiceClient(uriBuilder.Uri, sharedKeyCredential);
            BlobContainerClient container = serviceClient.GetBlobContainerClient(config.STORAGE_ACCOUNT_CONTAINER);

            BlobItem selectedFileName = (BlobItem)FilesListView.SelectedItem;
            await container.DeleteBlobIfExistsAsync(selectedFileName.Name);
        }
    }
}
