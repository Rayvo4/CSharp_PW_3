using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GradeNotifications;

namespace GradeNotifications.Tests
{
    // Компонентні тести спостерігачів: канал доставки замінений mock-об'єктом
    [TestClass]
    public class ObserverTests
    {
        private Mock<IMessageSender> senderMock;
        private Grade ivanGood;
        private Grade ivanFailing;
        private Grade mariaGood;

        [TestInitialize]
        public void Init()
        {
            senderMock = new Mock<IMessageSender>();
            ivanGood = new Grade("Іван Петренко", "Програмування на C#", 92);
            ivanFailing = new Grade("Іван Петренко", "ОБД", 45);
            mariaGood = new Grade("Марія Коваль", "ОБД", 88);
        }

        // Тест 25: студент отримує повідомлення про власну оцінку
        [TestMethod]
        public void StudentObserver_OwnGrade_SendsMessageToStudent()
        {
            // Arrange
            StudentObserver student = new StudentObserver("Іван Петренко", senderMock.Object);

            // Act
            student.Update(ivanGood);

            // Assert
            senderMock.Verify(s => s.Send("Іван Петренко", It.Is<string>(m => m.Contains("92"))), Times.Once());
        }

        // Тест 26: студент не отримує оцінок інших студентів
        [TestMethod]
        public void StudentObserver_OtherStudentGrade_DoesNotSend()
        {
            // Arrange
            StudentObserver student = new StudentObserver("Іван Петренко", senderMock.Object);

            // Act
            student.Update(mariaGood);

            // Assert
            senderMock.Verify(s => s.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        // Тест 27: лічильник сповіщень студента збільшується лише для власних оцінок
        [TestMethod]
        public void StudentObserver_OwnAndOtherGrades_CountsOnlyOwn()
        {
            // Arrange
            StudentObserver student = new StudentObserver("Іван Петренко", senderMock.Object);

            // Act
            student.Update(ivanGood);
            student.Update(mariaGood);
            student.Update(ivanFailing);

            // Assert
            Assert.AreEqual(2, student.NotificationCount);
        }

        // Тест 28: створення студента без каналу доставки викликає ArgumentNullException
        [TestMethod]
        public void StudentObserver_NullSender_ThrowsArgumentNullException()
        {
            // Act + Assert
            Assert.ThrowsException<ArgumentNullException>(() => new StudentObserver("Іван Петренко", null));
        }

        // Тест 29: батьки отримують оцінку своєї дитини
        [TestMethod]
        public void ParentObserver_ChildGrade_SendsMessageToParent()
        {
            // Arrange
            ParentObserver parent = new ParentObserver("Олена Петренко", "Іван Петренко", senderMock.Object);

            // Act
            parent.Update(ivanGood);

            // Assert
            senderMock.Verify(s => s.Send("Олена Петренко", It.IsAny<string>()), Times.Once());
        }

        // Тест 30: незадовільну оцінку дитини батьки отримують з позначкою
        [TestMethod]
        public void ParentObserver_ChildFailingGrade_MessageMarkedUnsatisfactory()
        {
            // Arrange
            ParentObserver parent = new ParentObserver("Олена Петренко", "Іван Петренко", senderMock.Object);

            // Act
            parent.Update(ivanFailing);

            // Assert
            senderMock.Verify(s => s.Send("Олена Петренко", It.Is<string>(m => m.Contains("незадовільно"))), Times.Once());
        }

        // Тест 31: батьки не отримують оцінок чужих дітей
        [TestMethod]
        public void ParentObserver_OtherStudentGrade_DoesNotSend()
        {
            // Arrange
            ParentObserver parent = new ParentObserver("Олена Петренко", "Іван Петренко", senderMock.Object);

            // Act
            parent.Update(mariaGood);

            // Assert
            senderMock.Verify(s => s.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        // Тест 32: деканат отримує незадовільну оцінку
        [TestMethod]
        public void DeanOfficeObserver_FailingGrade_SendsMessageToDeanOffice()
        {
            // Arrange
            DeanOfficeObserver dean = new DeanOfficeObserver(senderMock.Object);

            // Act
            dean.Update(ivanFailing);

            // Assert
            senderMock.Verify(s => s.Send(DeanOfficeObserver.Recipient, It.Is<string>(m => m.Contains("Іван Петренко"))), Times.Once());
        }

        // Тест 33: деканат не отримує задовільних оцінок
        [TestMethod]
        public void DeanOfficeObserver_PassingGrade_DoesNotSend()
        {
            // Arrange
            DeanOfficeObserver dean = new DeanOfficeObserver(senderMock.Object);

            // Act
            dean.Update(ivanGood);

            // Assert
            senderMock.Verify(s => s.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        // Тест 34: журнал з реальними спостерігачами: незадовільна оцінка доходить до трьох отримувачів
        [TestMethod]
        public void Journal_FailingGradeWithAllObservers_SendsThreeMessages()
        {
            // Arrange
            GradeJournal journal = new GradeJournal();
            journal.Attach(new StudentObserver("Іван Петренко", senderMock.Object));
            journal.Attach(new ParentObserver("Олена Петренко", "Іван Петренко", senderMock.Object));
            journal.Attach(new DeanOfficeObserver(senderMock.Object));

            // Act
            journal.AddGrade(ivanFailing);

            // Assert
            senderMock.Verify(s => s.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(3));
        }

        // Тест 35: сповіщення без оцінки (null) студент ігнорує
        [TestMethod]
        public void StudentObserver_NullGrade_DoesNotSend()
        {
            // Arrange
            StudentObserver student = new StudentObserver("Іван Петренко", senderMock.Object);

            // Act
            student.Update(null);

            // Assert
            senderMock.Verify(s => s.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        // Тест 36: порожнє ім'я студента в спостерігачі викликає ArgumentException
        [TestMethod]
        public void StudentObserver_EmptyName_ThrowsArgumentException()
        {
            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => new StudentObserver("", senderMock.Object));
        }

        // Тест 37: порожнє ім'я батьків викликає ArgumentException
        [TestMethod]
        public void ParentObserver_EmptyParentName_ThrowsArgumentException()
        {
            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => new ParentObserver("", "Іван Петренко", senderMock.Object));
        }

        // Тест 38: порожнє ім'я дитини викликає ArgumentException
        [TestMethod]
        public void ParentObserver_EmptyChildName_ThrowsArgumentException()
        {
            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => new ParentObserver("Олена Петренко", "", senderMock.Object));
        }

        // Тест 39: створення батьків без каналу доставки викликає ArgumentNullException
        [TestMethod]
        public void ParentObserver_NullSender_ThrowsArgumentNullException()
        {
            // Act + Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ParentObserver("Олена Петренко", "Іван Петренко", null));
        }

        // Тест 40: створення деканату без каналу доставки викликає ArgumentNullException
        [TestMethod]
        public void DeanOfficeObserver_NullSender_ThrowsArgumentNullException()
        {
            // Act + Assert
            Assert.ThrowsException<ArgumentNullException>(() => new DeanOfficeObserver(null));
        }

        // Тест 41: лічильник деканату враховує лише незадовільні оцінки
        [TestMethod]
        public void DeanOfficeObserver_MixedGrades_CountsOnlyFailing()
        {
            // Arrange
            DeanOfficeObserver dean = new DeanOfficeObserver(senderMock.Object);

            // Act
            dean.Update(ivanGood);
            dean.Update(ivanFailing);
            dean.Update(null);

            // Assert
            Assert.AreEqual(1, dean.NotificationCount);
        }
    }
}
