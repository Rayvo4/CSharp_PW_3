using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GradeNotifications;

namespace GradeNotifications.Tests
{
    // Unit-тести класу даних Grade
    [TestClass]
    public class GradeTests
    {
        // Тест 1: коректні дані зберігаються у властивостях
        [TestMethod]
        public void Constructor_ValidData_SetsScore()
        {
            // Arrange
            int expected = 85;

            // Act
            Grade grade = new Grade("Іван Петренко", "Програмування на C#", expected);

            // Assert
            Assert.AreEqual(expected, grade.Score);
        }

        // Тест 2: порожнє ім'я студента викликає ArgumentException
        [TestMethod]
        public void Constructor_EmptyStudentName_ThrowsArgumentException()
        {
            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => new Grade("", "Програмування на C#", 85));
        }

        // Тест 3: порожня назва дисципліни викликає ArgumentException
        [TestMethod]
        public void Constructor_EmptySubject_ThrowsArgumentException()
        {
            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => new Grade("Іван Петренко", " ", 85));
        }

        // Тест 4: оцінка більша за 100 викликає ArgumentOutOfRangeException
        [TestMethod]
        public void Constructor_ScoreAbove100_ThrowsArgumentOutOfRangeException()
        {
            // Act + Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Grade("Іван Петренко", "ОБД", 101));
        }

        // Тест 5: від'ємна оцінка викликає ArgumentOutOfRangeException
        [TestMethod]
        public void Constructor_NegativeScore_ThrowsArgumentOutOfRangeException()
        {
            // Act + Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Grade("Іван Петренко", "ОБД", -1));
        }

        // Тест 6: оцінка 59 є незадовільною
        [TestMethod]
        public void IsFailing_Score59_ReturnsTrue()
        {
            // Arrange
            Grade grade = new Grade("Іван Петренко", "ОБД", 59);

            // Act
            bool actual = grade.IsFailing;

            // Assert
            Assert.IsTrue(actual);
        }

        // Тест 7: оцінка 60 (межа) не є незадовільною
        [TestMethod]
        public void IsFailing_Score60_ReturnsFalse()
        {
            // Arrange
            Grade grade = new Grade("Іван Петренко", "ОБД", 60);

            // Act
            bool actual = grade.IsFailing;

            // Assert
            Assert.IsFalse(actual);
        }

        // Тест 8: оцінки з однаковими даними рівні
        [TestMethod]
        public void Equals_SameData_ReturnsTrue()
        {
            // Arrange
            Grade a = new Grade("Іван Петренко", "ОБД", 90);
            Grade b = new Grade("Іван Петренко", "ОБД", 90);

            // Act
            bool actual = a.Equals(b);

            // Assert
            Assert.IsTrue(actual);
        }

        // Тест 9: оцінки з різними балами не рівні
        [TestMethod]
        public void Equals_DifferentScore_ReturnsFalse()
        {
            // Arrange
            Grade a = new Grade("Іван Петренко", "ОБД", 90);
            Grade b = new Grade("Іван Петренко", "ОБД", 75);

            // Act
            bool actual = a.Equals(b);

            // Assert
            Assert.IsFalse(actual);
        }

        // Тест 10: порівняння з null повертає false
        [TestMethod]
        public void Equals_Null_ReturnsFalse()
        {
            // Arrange
            Grade grade = new Grade("Іван Петренко", "ОБД", 90);

            // Act
            bool actual = grade.Equals(null);

            // Assert
            Assert.IsFalse(actual);
        }

        // Тест 11: рівні оцінки мають однаковий хеш-код
        [TestMethod]
        public void GetHashCode_EqualGrades_ReturnsSameHash()
        {
            // Arrange
            Grade a = new Grade("Іван Петренко", "ОБД", 90);
            Grade b = new Grade("Іван Петренко", "ОБД", 90);

            // Act + Assert
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        // Тест 12: рядкове представлення оцінки
        [TestMethod]
        public void ToString_ValidGrade_ReturnsFormattedString()
        {
            // Arrange
            Grade grade = new Grade("Іван Петренко", "ОБД", 90);
            string expected = "Іван Петренко, «ОБД»: 90";

            // Act
            string actual = grade.ToString();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
