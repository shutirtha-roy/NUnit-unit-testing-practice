using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using UnitLibrary.ClassesWithDependencies;
using UnitLibrary.ClassesWithDependencies.Storage;
using Microsoft.EntityFrameworkCore;

namespace Unit.Dependency.Tests
{
    public class EmployeeControllerTests
    {
        private Mock<IEmployeeStorage> _employeeStorage;
        private EmployeeController _employeeController;

        [SetUp]
        public void Setup()
        {
            _employeeStorage = new Mock<IEmployeeStorage>();
            _employeeController = new EmployeeController(_employeeStorage.Object);
        }

        [Test]
        public void DeleteEmployee_WhenNoEmployeeDeletes_ThrowException()
        {
            _employeeStorage.Setup(employeeStorage => employeeStorage.DeleteEmployee(It.IsAny<int>())).Throws(new Exception());

            Should.Throw<Exception>(() =>
                _employeeController.DeleteEmployee(It.IsAny<int>())
            );
        }

        [Test]
        public void DeleteEmployee_WhenCalled_DeleteEmployee()
        {
            _employeeController.DeleteEmployee(It.IsAny<int>());

            _employeeStorage.Verify(storage => storage.DeleteEmployee(It.IsAny<int>()));
        }

        [Test]
        public void DeleteEmployee_WhenEmployeeDeletes_ReturnAction()
        {
            var result = _employeeController.DeleteEmployee(It.IsAny<int>());

            Assert.That(result, Is.TypeOf<RedirectResult>());
        }
    }
}
