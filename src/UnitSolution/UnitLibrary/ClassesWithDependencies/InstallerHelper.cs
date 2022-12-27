﻿using System.Net;
using UnitLibrary.ClassesWithDependencies.File;

namespace UnitLibrary.ClassesWithDependencies
{
    public class InstallerHelper
    {
        private readonly IFileDownloader _fileDownloader;
        private string _setupDestinationFile;

        public InstallerHelper(IFileDownloader fileDownloader = null)
        {
            _fileDownloader = fileDownloader ?? new FileDownloader();
        }

        public bool DownloadInstaller(string customerName, string installerName)
        {
            try
            {
                _fileDownloader.DownloadFile(
                    string.Format("http://example.com/{0}/{1}",
                    customerName, installerName), _setupDestinationFile);

                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }
    }
}
