using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitLibrary.ClassesWithDependencies;
using UnitLibrary.ClassesWithDependencies.Codes;
using UnitLibrary.ClassesWithDependencies.Repository;
using UnitLibrary.ClassesWithDependencies.Storage;

namespace Unit.Dependency.Tests
{
    public class HouseKeeperServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IEmailSender> _emailSender;
        private Mock<IXtraMessageBox> _messageBox;
        private HouseKeeperService _houseKeeperService;

        private List<Housekeeper> GetAllHouseKeepers()
        {
            return new List<Housekeeper>()
            {
                new Housekeeper()
                {
                    Email = "samin@gmail.com",
                    Oid = 4,
                    FullName = "Samin Yasar Kabir",
                    StatementEmailBody = "Hello I am Samin."
                },
                new Housekeeper()
                {
                    Email = "mostafa@gmail.com",
                    Oid = 7,
                    FullName = "Mostafa",
                    StatementEmailBody = "Hello I am Mostafa."
                },
            };
        }
        private List<Housekeeper> GetAllHouseKeepersWithWorkingAndNullEmail()
        {
            return new List<Housekeeper>()
            {
                new Housekeeper()
                {
                    Email = null,
                    Oid = 4,
                    FullName = "Samin Yasar Kabir",
                    StatementEmailBody = "Hello I am Samin."
                },
                new Housekeeper()
                {
                    Email = null,
                    Oid = 7,
                    FullName = "Mostafa",
                    StatementEmailBody = "Hello I am Mostafa."
                },
            };
        }
        private List<Housekeeper> GetAllHouseKeepersWithEmptyEmail()
        {
            return new List<Housekeeper>()
            {
                new Housekeeper()
                {
                    Email = "",
                    Oid = 4,
                    FullName = "Samin Yasar Kabir",
                    StatementEmailBody = "Hello I am Samin."
                },
                new Housekeeper()
                {
                    Email = "",
                    Oid = 7,
                    FullName = "Mostafa",
                    StatementEmailBody = "Hello I am Mostafa."
                },
            };
        }

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _statementGenerator = new Mock<IStatementGenerator>();
            _emailSender = new Mock<IEmailSender>();
            _messageBox = new Mock<IXtraMessageBox>();
            _houseKeeperService = new HouseKeeperService(_unitOfWork.Object, _statementGenerator.Object, _emailSender.Object, _messageBox.Object);
        }

        [Test]
        public void SendStatementEmail_WhenCalled_GenerateAllStatements()
        {
            var houseKeepers = GetAllHouseKeepers();
            _unitOfWork.Setup(unitOfWork => unitOfWork.Query<Housekeeper>()).Returns(houseKeepers.AsQueryable());
            DateTime dateTime = new DateTime(2022, 12, 3);

            _houseKeeperService.SendStatementEmail(dateTime);

            _statementGenerator.Verify(stateGenerator => stateGenerator.SaveStatement(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()));
        }

        [Test]
        public void SendStatementEmail_WhenEmailIsNull_NotGenerateAnyStatements()
        {
            var houseKeepers = GetAllHouseKeepersWithWorkingAndNullEmail();
            _unitOfWork.Setup(unitOfWork => unitOfWork.Query<Housekeeper>()).Returns(houseKeepers.AsQueryable());
            DateTime dateTime = new DateTime(2022, 12, 3);

            _houseKeeperService.SendStatementEmail(dateTime);

            //Times.Never is used to Verify with it is not able to access statementGenerator
            _statementGenerator.Verify(stateGenerator => stateGenerator.SaveStatement(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public void SendStatementEmail_WhenEmailIsEmpty_NotGenerateAnyStatements()
        {
            var houseKeepers = GetAllHouseKeepersWithEmptyEmail();
            _unitOfWork.Setup(unitOfWork => unitOfWork.Query<Housekeeper>()).Returns(houseKeepers.AsQueryable());
            DateTime dateTime = new DateTime(2022, 12, 3);

            _houseKeeperService.SendStatementEmail(dateTime);

            _statementGenerator.Verify(stateGenerator => stateGenerator.SaveStatement(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
        }
    }
}
