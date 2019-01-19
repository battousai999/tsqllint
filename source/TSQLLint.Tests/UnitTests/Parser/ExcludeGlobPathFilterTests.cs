using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using NUnit.Framework;
using TSQLLint.Core.Interfaces;
using TSQLLint.Infrastructure.Parser;

namespace TSQLLint.Tests.UnitTests.Parser
{
    [TestFixture]
    public class ExcludeGlobPathFilterTests
    {
        [Test]
        public void IsFilePathAllowed_NoGlobs_AllowsFiles()
        {
            // arrange
            var configReader = Substitute.For<IConfigReader>();

            configReader.GetExcludeFilePaths().Returns(new List<string>());

            var filter = new ExcludeGlobPathFilter(configReader);

            // act
            var isAllowed = filter.IsFilePathAllowed(@"c:\dbscripts\file1.sql");

            // assert
            Assert.IsTrue(isAllowed);
        }

        [Test]
        public void IsFilePathAllowed_WithGlob_AllowsFilesNotMatchingGlob()
        {
            // arrange
            var configReader = Substitute.For<IConfigReader>();

            configReader.GetExcludeFilePaths().Returns(new List<string>
            {
                @"**\temp\*"
            });

            var filter = new ExcludeGlobPathFilter(configReader);

            // act
            var isAllowed = filter.IsFilePathAllowed(@"c:\dbscripts\file1.sql");

            // assert
            Assert.IsTrue(isAllowed);
        }

        [Test]
        public void IsFilePathAllowed_WithGlob_DisallowsFilesMatchingGlob()
        {
            // arrange
            var configReader = Substitute.For<IConfigReader>();

            configReader.GetExcludeFilePaths().Returns(new List<string>
            {
                @"**\temp\*"
            });

            var filter = new ExcludeGlobPathFilter(configReader);

            // act
            var isAllowed = filter.IsFilePathAllowed(@"c:\temp\file1.sql");

            // assert
            Assert.IsFalse(isAllowed);
        }

        [Test]
        public void IsFilePathAllowed_WithGlob_DisallowFilesMatchingGlobAssumingCaseInsensitivity()
        {
            // arrange
            var configReader = Substitute.For<IConfigReader>();

            configReader.GetExcludeFilePaths().Returns(new List<string>
            {
                @"**\TEMP\*"
            });

            var filter = new ExcludeGlobPathFilter(configReader);

            // act
            var isAllowed = filter.IsFilePathAllowed(@"c:\temp\file1.sql");

            // assert
            Assert.IsFalse(isAllowed);
        }

        [Test]
        public void IsFilePathAllowed_WithMultipleGlobs_AllowsFilesNotMatchingGlobs()
        {
            // arrange
            var configReader = Substitute.For<IConfigReader>();

            configReader.GetExcludeFilePaths().Returns(new List<string>
            {
                @"**\temp\*",
                @"**\generated\*"
            });

            var filter = new ExcludeGlobPathFilter(configReader);

            // act
            var isAllowed = filter.IsFilePathAllowed(@"c:\dbscripts\file1.sql");

            // assert
            Assert.IsTrue(isAllowed);
        }

        [Test]
        public void IsFilePathAllowed_WithMultipleGlobs_DisallowsFilesMatchingGlobs()
        {
            // arrange
            var configReader = Substitute.For<IConfigReader>();

            configReader.GetExcludeFilePaths().Returns(new List<string>
            {
                @"**\temp\*",
                @"**\generated\*"
            });

            var filter = new ExcludeGlobPathFilter(configReader);

            // act
            var isAllowed1 = filter.IsFilePathAllowed(@"c:\temp\file1.sql");
            var isAllowed2 = filter.IsFilePathAllowed(@"c:\generated\file2.sql");

            // assert
            Assert.IsFalse(isAllowed1);
            Assert.IsFalse(isAllowed2);
        }
    }
}
