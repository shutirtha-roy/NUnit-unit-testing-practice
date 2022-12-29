using Moq;
using NUnit.Framework;
using System;
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
        public void SendStatementEmail_WhenCalled_GenerateStatement()
        {
            Housekeeper houseKeeper = new Housekeeper()
            {
                Email = "samin@gmail.com",
                Oid = 4,
                FullName = "Samin Yasar Kabir",
                StatementEmailBody = "Hello I am samin"
            };

            DateTime dateTime = new DateTime(2022, 12, 3);

            _statementGenerator.Setup(stateGenerator => stateGenerator.SaveStatement(houseKeeper.Oid, houseKeeper.FullName, dateTime)).Returns(It.IsAny<string>());

            _houseKeeperService.SendStatementEmail(dateTime);

            _statementGenerator.Verify(stateGenerator => stateGenerator.SaveStatement(houseKeeper.Oid, houseKeeper.FullName, dateTime));
        }
    }
}
