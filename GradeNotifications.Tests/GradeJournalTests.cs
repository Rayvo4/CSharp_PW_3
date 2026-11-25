using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GradeNotifications;

namespace GradeNotifications.Tests
{
    // Компонентні тести журналу оцінок: спостерігачі замінені mock-об'єктами
    [TestClass]
    public class GradeJournalTests
    {
        private GradeJournal journal;
        private Mock<IGradeObserver> firstObserverMock;
        private Mock<IGradeObserver> secondObserverMock;
        private Grade grade;

        [TestInitialize]
        public void Init()
        {
            journal = new GradeJournal();
            firstObserverMock = new Mock<IGradeObserver>();
            secondObserverMock = new Mock<IGradeObserver>();
            grade = new Grade("Іван Петренко", "Програмування на C#", 92);
        }

        // Тест 13: підписка збільшує кількість спостерігачів
        [TestMethod]
        public void Attach_Observer_IncreasesObserverCount()
        {
            // Act
            journal.Attach(firstObserverMock.Object);

            // Assert
            Assert.AreEqual(1, journal.ObserverCount);
        }

        // Тест 14: підписка null викликає ArgumentNullException
        [TestMethod]
        public void Attach_Null_ThrowsArgumentNullException()
        {
            // Act + Assert
            Assert.ThrowsException<ArgumentNullException>(() => journal.Attach(null));
        }

        // Тест 15: повторна підписка того самого спостерігача викликає InvalidOperationException
        [TestMethod]
        public void Attach_SameObserverTwice_ThrowsInvalidOperationException()
        {
            // Arrange
            journal.Attach(firstObserverMock.Object);

            // Act + Assert
            Assert.ThrowsException<InvalidOperationException>(() => journal.Attach(firstObserverMock.Object));
        }

        // Тест 16: відписка зменшує кількість спостерігачів
        [TestMethod]
        public void Detach_SubscribedObserver_DecreasesObserverCount()
        {
            // Arrange
            journal.Attach(firstObserverMock.Object);

            // Act
            journal.Detach(firstObserverMock.Object);

            // Assert
            Assert.AreEqual(0, journal.ObserverCount);
        }

        // Тест 17: відписка null викликає ArgumentNullException
        [TestMethod]
        public void Detach_Null_ThrowsArgumentNullException()
        {
            // Act + Assert
            Assert.ThrowsException<ArgumentNullException>(() => journal.Detach(null));
        }

        // Тест 18: нова оцінка надходить кожному підписаному спостерігачу рівно один раз
        [TestMethod]
        public void AddGrade_TwoObservers_EachNotifiedOnce()
        {
            // Arrange
            journal.Attach(firstObserverMock.Object);
            journal.Attach(secondObserverMock.Object);

            // Act
            journal.AddGrade(grade);

            // Assert
            firstObserverMock.Verify(o => o.Update(grade), Times.Once());
            secondObserverMock.Verify(o => o.Update(grade), Times.Once());
        }

        // Тест 19: відписаний спостерігач не отримує сповіщень
        [TestMethod]
        public void AddGrade_DetachedObserver_IsNotNotified()
        {
            // Arrange
            journal.Attach(firstObserverMock.Object);
            journal.Detach(firstObserverMock.Object);

            // Act
            journal.AddGrade(grade);

            // Assert
            firstObserverMock.Verify(o => o.Update(It.IsAny<Grade>()), Times.Never());
        }

        // Тест 20: виставлена оцінка стає останньою
        [TestMethod]
        public void AddGrade_ValidGrade_SetsLastGrade()
        {
            // Act
            journal.AddGrade(grade);

            // Assert
            Assert.AreEqual(grade, journal.LastGrade);
        }

        // Тест 21: виставлені оцінки підраховуються
        [TestMethod]
        public void AddGrade_TwoGrades_GradeCountIs2()
        {
            // Act
            journal.AddGrade(grade);
            journal.AddGrade(new Grade("Марія Коваль", "ОБД", 55));

            // Assert
            Assert.AreEqual(2, journal.GradeCount);
        }

        // Тест 22: виставлення null викликає ArgumentNullException
        [TestMethod]
        public void AddGrade_Null_ThrowsArgumentNullException()
        {
            // Act + Assert
            Assert.ThrowsException<ArgumentNullException>(() => journal.AddGrade(null));
        }

        // Тест 23: спостерігач, що відписується під час сповіщення, не ламає розсилку іншим
        [TestMethod]
        public void Notify_ObserverDetachesDuringUpdate_OthersStillNotified()
        {
            // Arrange
            firstObserverMock
                .Setup(o => o.Update(It.IsAny<Grade>()))
                .Callback(() => journal.Detach(firstObserverMock.Object));
            journal.Attach(firstObserverMock.Object);
            journal.Attach(secondObserverMock.Object);

            // Act
            journal.Notify(grade);

            // Assert
            secondObserverMock.Verify(o => o.Update(grade), Times.Once());
        }

        // Тест 24: сповіщення про null викликає ArgumentNullException
        [TestMethod]
        public void Notify_Null_ThrowsArgumentNullException()
        {
            // Act + Assert
            Assert.ThrowsException<ArgumentNullException>(() => journal.Notify(null));
        }
    }
}
