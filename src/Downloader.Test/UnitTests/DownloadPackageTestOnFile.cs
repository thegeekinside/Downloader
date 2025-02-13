﻿using Downloader.DummyHttpServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Downloader.Test.UnitTests
{
    [TestClass]
    public class DownloadPackageTestOnFile : DownloadPackageTest
    {
        private string _path;

        [TestInitialize]
        public override void Initial()
        {
            _path = Path.GetTempFileName();

            Package = new DownloadPackage() {
                FileName = _path,
                Address = DummyFileHelper.GetFileWithNameUrl(DummyFileHelper.SampleFile16KbName, DummyFileHelper.FileSize16Kb),
                TotalFileSize = DummyFileHelper.FileSize16Kb
            };
            base.Initial();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
            File.Delete(_path);
        }

        [TestMethod]
        public void BuildStorageTest()
        {
            BuildStorageTest(false);
        }

        [TestMethod]
        public void BuildStorageWithReserveSpaceTest()
        {
            BuildStorageTest(true);
        }

        private void BuildStorageTest(bool reserveSpace)
        {
            // arrange
            Cleanup();
            _path = Path.GetTempFileName();
            Package = new DownloadPackage() {
                FileName = _path,
                Address = DummyFileHelper.GetFileWithNameUrl(DummyFileHelper.SampleFile16KbName, DummyFileHelper.FileSize16Kb),
                TotalFileSize = DummyFileHelper.FileSize16Kb
            };

            // act
            Package.BuildStorage(reserveSpace, 1024*1024);

            // assert
            Assert.IsInstanceOfType(Package.Storage.OpenRead(), typeof(FileStream));
            Assert.AreEqual(reserveSpace? DummyFileHelper.FileSize16Kb : 0, Package.Storage.Length);
        }
    }
}
