using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PilotScheduler.Repository.Interfaces;
using PilotScheduler.Repository.Models;
using PilotScheduler.Service.Controllers;
using PilotScheduler.Service.Models.Classes;
using PilotScheduler.Service.Models.Interfaces;

namespace PilotScheduler.Tests
{
    public class ScheduleControllerTests
    {
        private ScheduleController _scheduleController;
        private Mock<IDataConfiguration> _crewConfigurationMock;
        private Mock<IDummyDatabaseRepository> _dummyDatabaseRepository;

        [SetUp]
        public void Setup()
        {
            _crewConfigurationMock = new Mock<IDataConfiguration>();
            _dummyDatabaseRepository = new Mock<IDummyDatabaseRepository>();
            
            _scheduleController = new ScheduleController(_dummyDatabaseRepository.Object);
        }

        [Test]
        public void RequestPilot_With_Invalid_Location_Returns_Bad_Request()
        {
            //arrange
            var location = string.Empty;
            var depDateTime = DateTime.Now.ToString();
            var returnDateTime = DateTime.Now.AddHours(4).ToString();
            
            //act
            var pilots = (BadRequestObjectResult)_scheduleController.RequestPilot(location, depDateTime, returnDateTime);
            
            //assert
            Assert.AreEqual(400, pilots.StatusCode);
        }
        
        [Test]
        public void RequestPilot_With_Invalid_DateTime_Returns_Bad_Request()
        {
            //arrange
            var location = "Munich";
            var depDateTime = string.Empty;
            var returnDateTime = string.Empty;
            
            //act
            var pilots = (BadRequestObjectResult)_scheduleController.RequestPilot(location, depDateTime, returnDateTime);
            
            //assert
            Assert.AreEqual(400, pilots.StatusCode);
        }

        [Test] 
        public void RequestPilot_With_Valid_Data_Returns_Available_Pilot()
        {
            //arrange
            var location = "Munich";
            var depDateTime = DateTime.Now;
            var returnDateTime = DateTime.Now.AddHours(5);

            _crewConfigurationMock.Setup(x => x.Crew).Returns(It.IsAny<Crew[]>());
            _dummyDatabaseRepository
                .Setup(x => x.RequestPilot("Munich", depDateTime, returnDateTime))
                .Returns(() => new DummyPilotRequestRecord()
                {
                    Id = It.IsAny<Guid>(),
                    PilotId = It.IsAny<int>(),
                    RequestCount = 1
                });
            
            //act
            var pilots = (OkObjectResult)_scheduleController.RequestPilot(location, depDateTime.ToString(), returnDateTime.ToString());
            
            //assert
            Assert.AreEqual(200, pilots.StatusCode);
        }

        [Test]
        public void ScheduleFlight_Returns_OkObject_Result_If_Pilot_Exists()
        {
            //arrange
            var schedulRequestModel = new ScheduleRequestModel()
            {
                PilotId = 1, DepDateTime = DateTime.Now, ReturnDateTime = DateTime.Now.AddHours(5)
            };

            _dummyDatabaseRepository
                .Setup(x => x.ScheduleFlight(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new Crew()
                {
                    Id = 1, Base = "London", Name = "Ife", WorkDays = new []{"Monday", "Tuesday"}
                });
            
            //act
            var result = (OkObjectResult) _scheduleController.ScheduleFlight(schedulRequestModel);
            
            //assert
            Assert.AreEqual(200, result.StatusCode);
        }
        
        [Test]
        public void ScheduleFlight_Returns_NotFound_Result_If_Pilot_Does_Not_Exists()
        {
            //arrange
            var schedulRequestModel = new ScheduleRequestModel()
            {
                PilotId = 1, DepDateTime = DateTime.Now, ReturnDateTime = DateTime.Now.AddHours(5)
            };

            _dummyDatabaseRepository
                .Setup(x => x.ScheduleFlight(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(It.IsAny<Crew>());
            
            //act
            var result = (NotFoundObjectResult) _scheduleController.ScheduleFlight(schedulRequestModel);
            
            //assert
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}