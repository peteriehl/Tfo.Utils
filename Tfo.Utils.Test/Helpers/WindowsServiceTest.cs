using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tfo.Utils.Io.BaseClasses;
using Tfo.Utils.Io.Helpers;
using System.Threading.Tasks;

namespace Tfo.Utils.Test.Helpers
{
    [TestClass]
    public class WindowsServiceTest
    {
        [TestMethod]
        public void TestBootstrapStart()
        {
            // Arrange
            Mock<Startable> mock = new Mock<Startable>();

            Bootstrapper b = new Bootstrapper(false);
            b.AddStartable(mock.Object);
            
            bool mockStartWasFired = false;
            mock.Object.StartableStarted += e => 
                { 
                    mockStartWasFired = true;
                };

            bool boostrapStartWasFired = false;
            b.StartableStarted += e =>
                {
                    boostrapStartWasFired = true;
                };

            // Act
            b.Start();

            // Assert
            Assert.AreEqual(true, mockStartWasFired);
            Assert.AreEqual(true, boostrapStartWasFired);
        }

        [TestMethod]
        public void TestBootstrapStartAsynch()
        {
            // Arrange
            Mock<Startable> mock = new Mock<Startable>();

            Bootstrapper b = new Bootstrapper(true);
            b.AddStartable(mock.Object);

            bool mockStartWasFired = false;
            mock.Object.StartableStarted += e =>
            {
                mockStartWasFired = true;
            };

            bool boostrapStartWasFired = false;
            b.StartableStarted += e =>
            {
                boostrapStartWasFired = true;
            };

            // Act
            b.Start();

            Task.WaitAll(b.TasksStarting.ToArray());

            // Assert
            Assert.AreEqual(true, mockStartWasFired);
            Assert.AreEqual(true, boostrapStartWasFired);
        }

        [TestMethod]
        public void TestBootstrapStop()
        {
            Mock<Startable> mock = new Mock<Startable>();

            Bootstrapper b = new Bootstrapper(false);
            b.AddStartable(mock.Object);

            bool stopWasFired = false;
            mock.Object.StartableStopped += e => { stopWasFired = true; };
            bool bootstrapStopWasFired = false;
            b.StartableStopped += e => { bootstrapStopWasFired = true; };

            // Act
            b.Stop();

            // Assert
            Assert.AreEqual(true, stopWasFired);
            Assert.AreEqual(true, bootstrapStopWasFired);
        }
    }
}
