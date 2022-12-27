using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using UnitLibrary.ClassesWithDependencies;
using UnitLibrary.ClassesWithDependencies.Reader;
using UnitLibrary.ClassesWithDependencies.Repository;

namespace Unit.NonDependency.Tests
{
    public class VideoServiceTests
    {
        private Mock<IFileReader> _fileReader;
        private Mock<IVideoRepository> _videoRepository;
        private VideoService _videoService;

        [SetUp]
        public void Setup()
        {
            _fileReader = new Mock<IFileReader>();
            _videoRepository = new Mock<IVideoRepository>();
            _videoService = new VideoService(_fileReader.Object, _videoRepository.Object);
        }

        [Test]
        public void ReadTitle_EmptyFile_ThrowError()
        {
            _fileReader.Setup(fileReader => fileReader.Read("video.txt")).Returns("");

            var result = _videoService.ReadVideoTitle();

            Assert.That(result, Contains.Substring("error").IgnoreCase);
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_WhenNoUnprocessedVideos_ReturnEmptyString()
        {
            _videoRepository.Setup(videoRepository => videoRepository.GetUnProcessedVideos()).Returns(new List<Video>());

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.AreEqual(result, "");
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_WhenThereIsUnProcessedVideos_ReturnStrings()
        {
            _videoRepository.Setup(videoRepository => videoRepository.GetUnProcessedVideos()).Returns(new List<Video>()
            {
                new Video() { Id = 1, Title = "Mr.Bean", IsProcessed = true },
                new Video() { Id = 2, Title = "Dragonballz", IsProcessed = true },
                new Video() { Id = 3, Title = "Naruto", IsProcessed = true },
            });

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Contains.Substring("1,2,3"));
        }
    }
}
