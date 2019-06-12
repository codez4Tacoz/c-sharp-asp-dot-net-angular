using ASPNetCoreAngular7;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Tests
{
    public class HackerNewsControllerUnitTests
    {
        [SetUp]
        public void Setup()
        {
        }



        [Test]
        [TestCase(new int[] { }, 1, 0, TestName = "NP: Empty Ids List, pgSize > 0")]
        [TestCase(new int[] { }, 5, 0, TestName = "NP: Empty Ids List, pgSize > 1")]
        [TestCase(new int[] { 3 }, 10, 1, TestName = "NP: 1 id, pgSize10, should be 1 pages")]
        [TestCase(new int[] { 3 }, 1, 1,  TestName = "NP: 1 id, pgSize1, should be 1 pages")]
        [TestCase(new int[] { 3, 4, 1, 2, 5 }, 1, 5, TestName = "NP: 5 ids, pgSize1, should be 5 pages")]
        [TestCase(new int[] { 3, 4, 1, 2, 5 }, 2, 3, TestName = "NP: 5 ids, pgSize2, should be 3 pages")]
        [TestCase(new int[] { 3, 4, 1, 2, 5 }, 5, 1, TestName = "NP: 5 ids, pgSize5, should be 1 pages")]
        [TestCase(new int[] { 3, 4, 1, 2, 5 }, 10, 1, TestName = "NP: 5 ids, pgSize10, should be 1 pages")]
        public void NumPagesTest_Variable(int[] latestNewsIds, int pageSize, int expectedNumPages)
        {
            // Arrange
            var mockRepo = new Mock<INewsService>();
            mockRepo.Setup(repo => repo.GetIdsAsync())
                .ReturnsAsync(latestNewsIds);
            var controller = new HackerNewsController(mockRepo.Object);

            // Act
            int numPagesResult = controller.CalculateNumPages(latestNewsIds, pageSize);


            // Assert
            Assert.AreEqual(expectedNumPages, numPagesResult);
        }

        /**
         * Tests that the method returns an exception if called with an empty array of ids
         **/
        [Test]
        [TestCase(new int[] { }, 0, TestName = "NP_EXP: Empty Ids List")]
        public void NumPagesTest_ShouldThrowException(int[] latestNewsIds, int pageSize)
        {
            // Arrange
            var mockRepo = new Mock<INewsService>();
            mockRepo.Setup(repo => repo.GetIdsAsync())
                .ReturnsAsync(latestNewsIds);
            var controller = new HackerNewsController(mockRepo.Object);

            // Act & Assert
            Assert.That(() => controller.CalculateNumPages(latestNewsIds, pageSize), Throws.Exception);
        }


        /**
         * Tests that in the case of the number of pages less than the pageNumber passed-in
         * that the controller will adjust the pageNumber to the number of the last page 
         **/
        [Test]
        [TestCase(new int[] { }, 1, 1, 0, TestName = "RPN: Empty Ids List - pgNum should be 0")]
        [TestCase(new int[] { }, 5, 5, 0, TestName = "RPN: Empty Ids List, pageNum > 1, pgSize > 1 - pgNum should be 0")]
        [TestCase(new int[] { 3, 4, 1, 2, 5, 6 }, 1, 1, 1, TestName = "RPN: 6ids, pgNumArg1, pgSize1, should be pgNum1")]
        [TestCase(new int[] { 3, 4, 1, 2, 5, 6 }, 10, 1, 6, TestName = "RPN: 6ids, pgNumArg10,pgSize1, should be pgNum6")]
        [TestCase(new int[] { 3, 4, 1, 2, 5, 6 }, 2, 6, 1, TestName = "RPN: 6ids, pgNumArg2, pgSize6, should be pgNum1")]
        [TestCase(new int[] { 3, 4, 1, 2, 5, 6 }, 3, 3, 2, TestName = "RPN: 6ids, pgNumArg3, pgSize3, should be pgNum2")]
        public void CalculateRealPageNumberTest_Variable(int[] latestNewsIds,
                                                         int pageNum, 
                                                         int pageSize, 
                                                         int expectedPageNum)
        {
            // Arrange
            var mockRepo = new Mock<INewsService>();
            mockRepo.Setup(repo => repo.GetIdsAsync())
                .ReturnsAsync(latestNewsIds);
            var controller = new HackerNewsController(mockRepo.Object);

            // Act
            int realPageNumResult = controller.CalculateRealPageNumber(latestNewsIds, pageNum, pageSize);

            // Assert
            Assert.AreEqual(expectedPageNum, realPageNumResult);
        }

        /**
         * Tests that the returned array startIndex is correct, given the ids list, pageSize, and desired pageNum.
         **/
        [Test]
        [TestCase(new int[] { 3, 4 }, 1, 1, 0, TestName = "PSI: 2ids, pgNum1, pgSize1 (2 pgs), startIndex for pg1=0")]
        [TestCase(new int[] { 3, 4 }, 2, 1, 1, TestName = "PSI: 2ids, pgNum2, pgSize1 (2 pgs), startIndex for pg2=1")]
        [TestCase(new int[] { 3, 4, 1, 2, 5, 6 }, 6, 1, 5, TestName = "PSI: 6ids, pgNum6, pgSize1, startIndex for pg6=5")]
        [TestCase(new int[] { 3, 4, 1, 2, 5, 6 }, 2, 3, 3, TestName = "PSI: 6ids, pgNum2, pgSize3, startIndex for pg2=3")]
        [TestCase(new int[] { 3, 4, 1, 2, 5 },    2, 3, 3, TestName = "PSI: 5ids, pgNum2, pgSize3, startIndex for pg2=3")]
        public void CalculatePageStartIndex_Variable(int[] latestNewsIds,
                                                         int pageNum,
                                                         int pageSize,
                                                         int expectedStartIndex)
        {
            // Arrange
            var mockRepo = new Mock<INewsService>();
            mockRepo.Setup(repo => repo.GetIdsAsync())
                .ReturnsAsync(latestNewsIds);
            var controller = new HackerNewsController(mockRepo.Object);

            // Act
            int startIndexResult = controller.CalculatePageStartIndex(latestNewsIds, pageNum, pageSize);

            // Assert
            Assert.AreEqual(expectedStartIndex, startIndexResult);
        }


        /**
         * Tests that the method returns an exception if called with an empty array of ids
         **/
        [Test]
        [TestCase(new int[] { }, 1, 1, TestName = "PSI_EXP: Empty Ids List - should throw exception")]
        public void CalculatePageStartIndex_ShouldThrowException(int[] latestNewsIds,
                                                                 int pageNum,
                                                                 int pageSize)
        {
            // Arrange
            var mockRepo = new Mock<INewsService>();
            mockRepo.Setup(repo => repo.GetIdsAsync())
                .ReturnsAsync(latestNewsIds);
            var controller = new HackerNewsController(mockRepo.Object);

            // Act & Assert
            Assert.That(() => controller.CalculatePageStartIndex(latestNewsIds, pageNum, pageSize), Throws.InvalidOperationException);
        }


        /**
         * Tests that the returned array endIndex is correct, given the ids list, pageSize, and desired pageNum.
         **/
        [Test]
        [TestCase(new int[] { 3, 4 }, 1, 1, 0, TestName = "PEI: 2ids, pgNum1, pgSize1 (2 pgs), endIndex for pg1=0")]
        [TestCase(new int[] { 3, 4 }, 2, 1, 1, TestName = "PEI: 2ids, pgNum2, pgSize1 (2 pgs), endIndex for pg2=1")]
        [TestCase(new int[] { 3, 4, 1, 2, 5, 6 }, 6, 1, 5, TestName = "PEI: 6ids, pgNum6, pgSize1, endIndex for pg6=5")]
        [TestCase(new int[] { 3, 4, 1, 2, 5, 6 }, 2, 3, 5, TestName = "PEI: 6ids, pgNum2, pgSize3, endIndex for pg2=5")]
        [TestCase(new int[] { 3, 4, 1, 2, 5 }, 2, 3, 4, TestName = "PEI: 5ids, pgNum2, pgSize3, endIndex for pg2=4")]
        [TestCase(new int[] { 3, 4, 1, 2, 5 }, 3, 2, 4, TestName = "PEI: 5ids, pgNum3, pgSize2, endIndex for pg3=4")]
        public void CalculatePageEndIndex_Variable(int[] latestNewsIds,
                                                         int pageNum,
                                                         int pageSize,
                                                         int expectedEndIndex)
        {
            // Arrange
            var mockRepo = new Mock<INewsService>();
            mockRepo.Setup(repo => repo.GetIdsAsync())
                .ReturnsAsync(latestNewsIds);
            var controller = new HackerNewsController(mockRepo.Object);

            // Act
            int endIndexResult = controller.CalculatePageEndIndex(latestNewsIds, pageNum, pageSize);

            // Assert
            Assert.AreEqual(expectedEndIndex, endIndexResult);
        }

        /**
         * Tests that the method returns an exception if called with an empty array of ids
         **/
        [Test]
        [TestCase(new int[] { }, 1, 1, TestName = "PEI_EXP: Empty Ids List - should throw exception")]
        public void CalculatePageEndIndex_ShouldThrowException(int[] latestNewsIds,
                                                         int pageNum,
                                                         int pageSize)
        {
            // Arrange
            var mockRepo = new Mock<INewsService>();
            mockRepo.Setup(repo => repo.GetIdsAsync())
                .ReturnsAsync(latestNewsIds);
            var controller = new HackerNewsController(mockRepo.Object);

            // Act & Assert
            Assert.That(() => controller.CalculatePageEndIndex(latestNewsIds, pageNum, pageSize), Throws.InvalidOperationException);
        }
    }
}