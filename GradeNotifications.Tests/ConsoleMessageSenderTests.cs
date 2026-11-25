using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GradeNotifications;

namespace GradeNotifications.Tests
{
    // Unit-тест консольного каналу доставки: вивід перехоплюється в StringWriter
    [TestClass]
    public class ConsoleMessageSenderTests
    {
        // Тест 42: повідомлення виводиться в консоль з іменем отримувача
        [TestMethod]
        public void Send_Message_WritesRecipientAndMessageToConsole()
        {
            // Arrange
            ConsoleMessageSender sender = new ConsoleMessageSender();
            StringWriter output = new StringWriter();
            TextWriter original = Console.Out;
            Console.SetOut(output);

            // Act
            sender.Send("Деканат", "Тестове повідомлення");
            Console.SetOut(original);

            // Assert
            Assert.IsTrue(output.ToString().Contains("[Деканат] Тестове повідомлення"));
        }
    }
}
