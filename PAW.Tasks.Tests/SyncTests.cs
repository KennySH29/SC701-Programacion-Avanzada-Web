namespace PAW.Tasks.Tests
{
    [TestClass]
    public sealed class SyncTests
    {
        [TestMethod]
        public async Task Execute_ShouldCompleteSuccessfully()
        {
            //Arrange
            int secs = 3;
            string identifier = "AAA";

            //Act
            Task task = Sync.Execute(secs, identifier);

            await task;

            //Assert
            Assert.IsTrue(task.IsCompletedSuccessfully);
        }

        [TestMethod]
        public async Task ExecuteEval_ShouldCompleteSuccessfully()
        {
            //Arrange
            decimal secs = 10;
            string identifier = "BBB";

            //Act
            Task task = Sync.ExecuteEval(secs, identifier);

            await task;

            //Assert
            Assert.IsTrue(task.IsCompletedSuccessfully);    
        }

        [TestMethod]
        public async Task ExecuteItems_ShouldReturnCorrectNumberOfItems()
        {
            //Arrange
            int sects = 3;
            string identifier = "TEST";

            //Act
            var result = await Sync.ExecuteItems(sects, identifier);
            var items = result.ToList();

            //Assert
            Assert.AreEqual(sects, items.Count);
        }

        [TestMethod]
        public async Task ExecuteItems_ShouldReturnCorrectContent()
        {
            // Arrange
            int secs = 3;
            string identifier = "TEST";

            // Act
            var result =
                await Sync.ExecuteItems(secs, identifier);

            var items = result.ToList();

            // Assert
            Assert.AreEqual("TEST - Second 1", items[0]);
            Assert.AreEqual("TEST - Second 2", items[1]);
            Assert.AreEqual("TEST - Second 3", items[2]);
        }

        [TestMethod]
        public async Task ExecuteItems_ZeroSeconds_ShouldReturnEmptyCollection()
        {
            // Arrange
            int secs = 3;
            string identifier = "TEST";

            // Act
            var result =
                await Sync.ExecuteItems(secs, identifier);

            var items = result.ToList();

            // Assert
            Assert.AreEqual(3, items.Count);
        }
    }
}
