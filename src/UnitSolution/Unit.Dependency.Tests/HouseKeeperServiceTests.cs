using Moq;
using NUnit.Framework;
using Shouldly;
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
        private DateTime _dateTime;

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
            _dateTime = new DateTime(2022, 12, 3);
        }

        private void SetHouseKeepersInUnitOfWork(List<Housekeeper> houseKeepers)
        {
            _unitOfWork.Setup(unitOfWork => unitOfWork.Query<Housekeeper>()).Returns(houseKeepers.AsQueryable());
        }

        [Test]
        public void SendStatementEmail_WhenCalled_GenerateAllStatements()
        {
            SetHouseKeepersInUnitOfWork(GetAllHouseKeepers());

            _houseKeeperService.SendStatementEmail(_dateTime);

            _statementGenerator.Verify(stateGenerator => stateGenerator.SaveStatement(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()));
        }

        [Test]
        public void SendStatementEmail_WhenEmailIsNull_NotGenerateAnyStatements()
        {
            SetHouseKeepersInUnitOfWork(GetAllHouseKeepersWithWorkingAndNullEmail());

            _houseKeeperService.SendStatementEmail(_dateTime);

            //Times.Never is used to Verify with it is not able to access statementGenerator
            _statementGenerator.Verify(stateGenerator => stateGenerator.SaveStatement(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public void SendStatementEmail_WhenEmailIsEmpty_NotGenerateAnyStatements()
        {
            SetHouseKeepersInUnitOfWork(GetAllHouseKeepersWithEmptyEmail());

            _houseKeeperService.SendStatementEmail(_dateTime);

            _statementGenerator.Verify(stateGenerator => stateGenerator.SaveStatement(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public void SendStatementEmail_WhenCalled_SendStatementToEmail()
        {
            SetHouseKeepersInUnitOfWork(GetAllHouseKeepers());

            _statementGenerator.Setup(statementGenerator => statementGenerator.SaveStatement(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns("statement");

            _houseKeeperService.SendStatementEmail(_dateTime);

            _emailSender.Verify(emailSender => emailSender.EmailFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void SendStatementEmail_WhenStatementIsNull_NotSendStatementToEmail()
        {
            SetHouseKeepersInUnitOfWork(GetAllHouseKeepers());

            _statementGenerator.Setup(statementGenerator => statementGenerator.SaveStatement(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(() => null);

            _houseKeeperService.SendStatementEmail(_dateTime);

            _emailSender.Verify(emailSender => emailSender.EmailFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void SendStatementEmail_WhenStatementIsEmpty_NotSendStatementToEmail()
        {
            SetHouseKeepersInUnitOfWork(GetAllHouseKeepers());

            _statementGenerator.Setup(statementGenerator => statementGenerator.SaveStatement(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns("");

            _houseKeeperService.SendStatementEmail(_dateTime);

            _emailSender.Verify(emailSender => emailSender.EmailFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void SendStatementEmail_WhenEmailSenderIsValid_GoesToMessageBox()
        {
            SetHouseKeepersInUnitOfWork(GetAllHouseKeepers());

            _statementGenerator.Setup(statementGenerator => statementGenerator.SaveStatement(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns("statement");

            _emailSender.Setup(emailSender => emailSender.EmailFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();

            _houseKeeperService.SendStatementEmail(_dateTime);

            _messageBox.Verify(messageBox => messageBox.Show(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButtons.OK));
        }
    }
}
