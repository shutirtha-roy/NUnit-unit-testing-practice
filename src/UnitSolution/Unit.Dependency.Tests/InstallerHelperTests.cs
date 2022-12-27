using Moq;
using NUnit.Framework;
using System.Net;
using UnitLibrary.ClassesWithDependencies;
using UnitLibrary.ClassesWithDependencies.File;

namespace Unit.Dependency.Tests
{
    public class InstallerHelperTests
    {
        private Mock<IFileDownloader> _fileDownloader;
        private InstallerHelper _installerHelper;

        [SetUp]
        public void Setup()
        {
            _fileDownloader = new Mock<IFileDownloader>();
            _installerHelper = new InstallerHelper(_fileDownloader.Object);
        }

        [Test]
        public void DownloadInstaller_DownloadInvalid_ReturnFalse()
        {
            _fileDownloader.Setup(fileDownloader =>
                fileDownloader.DownloadFile(It.IsAny<string>(), It.IsAny<string>()
             )).Throws<WebException>();

            var result = _installerHelper.DownloadInstaller(It.IsAny<string>(), It.IsAny<string>());

            Assert.IsFalse(result);
        }

        [Test]
        public void DownloadInstaller_DownloadFile_ReturnTrue()
        {
            var result = _installerHelper.DownloadInstaller(It.IsAny<string>(), It.IsAny<string>());

            Assert.IsTrue(result);
        }
    }
}
