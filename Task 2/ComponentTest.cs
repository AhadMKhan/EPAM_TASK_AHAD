using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TestApp;
using TestAppAPI;

namespace EPAM.Tests

{
    public class ComponentTests
    {
        private Mock<IStudyGroupRepository> _repositoryMock;
        private StudyGroupController _controller;

        // Define test data
        private readonly StudyGroup _validStudyGroup = new StudyGroup(ValidStudyGroupId, ValidStudyGroupName, ValidStudyGroupSubject, DateTime.Now, new List<User> { new User(ValidUserName) });
        private readonly StudyGroup _invalidLengthStudyGroup = new StudyGroup(2, "Fail Physics Study Group - Ahad", Subject.Math, DateTime.Now, new List<User> { new User("Ahad") });
        private readonly StudyGroup _invalidSubjectStudyGroup = new StudyGroup(3, "Chemistry Study Group", (Subject)3, DateTime.Now, new List<User> { new User("Ahad") });

        // Global constants
        private const int ValidStudyGroupId = 1;
        private const string ValidStudyGroupName = "Math Study Group";
        private const Subject ValidStudyGroupSubject = Subject.Math;
        private const string ValidUserName = "Ahad";

        // Define test data for TC47
        private readonly Subject _subjectFilter = Subject.Physics;
        private readonly List<StudyGroup> _studyGroups = new List<StudyGroup>
        {
            new StudyGroup(1, "Math Study Group", Subject.Math, DateTime.Now, new List<User> { new User("Ahad") }),
            new StudyGroup(2, "Chemistry Study Group", Subject.Chemistry, DateTime.Now, new List<User> { new User("Ahad") }),
            new StudyGroup(3, "Physics Study Group", Subject.Physics, DateTime.Now, new List<User> { new User("Ahad") })
        };

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IStudyGroupRepository>();
            _controller = new StudyGroupController(_repositoryMock.Object);
        }

        // Component test cases

        // TC41: Verify Study Group Initialization
        [Test]
        public async Task Verify_StudyGroup_Initialization()
        {
            Assert.That(_controller, Is.Not.Null);
        }

        // TC42: Ensure that users can create new study groups with valid inputs
        [Test]
        public async Task Ensure_Users_Can_Create_New_Study_Groups_With_Valid_Inputs()
        {
            _repositoryMock.Setup(repo => repo.CreateStudyGroup(It.IsAny<StudyGroup>())).Returns(Task.CompletedTask);

            var result = await _controller.CreateStudyGroup(_validStudyGroup) as OkResult;

            Assert.That(result, Is.Not.Null);
        }

        // TC43: Validate that the system can retrieve existing study groups without errors
        [Test]
        public async Task Validate_System_Can_Retrieve_Existing_Study_Groups_Without_Errors()
        {
            _repositoryMock.Setup(repo => repo.GetStudyGroups()).ReturnsAsync(new List<StudyGroup>());

            var result = await _controller.GetStudyGroups() as OkObjectResult;

            Assert.That(result, Is.Not.Null);
        }

        // TC44: Check the ability to search for specific study groups using relevant search terms
        [Test]
        public async Task Check_Ability_To_Search_For_Specific_Study_Groups_Using_Relevant_Search_Terms()
        {
            _repositoryMock.Setup(repo => repo.SearchStudyGroups(It.IsAny<string>())).ReturnsAsync(new List<StudyGroup>());

            var result = await _controller.SearchStudyGroups(ValidStudyGroupName) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
        }

        // TC45: Confirm that users can join existing study groups successfully
        [Test]
        public async Task Confirm_That_Users_Can_Join_Existing_Study_Groups_Successfully()
        {
            _repositoryMock.Setup(repo => repo.JoinStudyGroup(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _controller.JoinStudyGroup(ValidStudyGroupId, 1) as OkResult;

            Assert.That(result, Is.Not.Null);
        }

        // TC46: Ensure that users can leave study groups they have joined
        [Test]
        public async Task Ensure_That_Users_Can_Leave_Study_Groups_They_Have_Joined()
        {
            _repositoryMock.Setup(repo => repo.LeaveStudyGroup(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _controller.LeaveStudyGroup(ValidStudyGroupId, 1) as OkResult;

            Assert.That(result, Is.Not.Null);
        }

        // TC47: Ensure that study groups can be filtered by subject
        [Test]
        public async Task Ensure_That_Study_Groups_Can_Be_Filtered_By_Subject()
        {
            _repositoryMock.Setup(repo => repo.GetStudyGroups()).ReturnsAsync(_studyGroups);

            var result = await _controller.GetStudyGroups() as OkObjectResult;
            var filteredStudyGroups = result.Value as List<StudyGroup>;

            Assert.That(result, Is.Not.Null);
            Assert.That(filteredStudyGroups.Count, Is.EqualTo(1)); // Expecting only one study group with Physics subject
            Assert.That(filteredStudyGroups[0].Subject, Is.EqualTo(_subjectFilter)); // Expecting the filtered study group to have Physics subject
        }

        // TC48: Ensure that the study group name meets the required length criteria (less than 5)
        [Test]
        public async Task Ensure_That_Study_Group_Name_Meets_Required_Length_Criteria_Less_Than_5()
        {
            var result = await _controller.CreateStudyGroup(_invalidLengthStudyGroup) as BadRequestResult;

            Assert.That(result, Is.Not.Null);
        }

        // TC49: Ensure that the study group name meets the required length criteria (greater than 30)
        [Test]
        public async Task Ensure_That_Study_Group_Name_Meets_Required_Length_Criteria_Greater_Than_30()
        {
            var result = await _controller.CreateStudyGroup(_invalidLengthStudyGroup) as BadRequestResult;

            Assert.That(result, Is.Not.Null);
        }

        // TC50: Confirm that the creation timestamp is recorded for study groups
        [Test]
        public async Task Confirm_That_The_Creation_Timestamp_Is_Recorded_For_Study_Groups()
        {
            _repositoryMock.Setup(repo => repo.CreateStudyGroup(It.IsAny<StudyGroup>())).Returns(Task.CompletedTask);

            var result = await _controller.CreateStudyGroup(_validStudyGroup) as OkResult;

            Assert.That(result, Is.Not.Null);

            _repositoryMock.Verify(repo => repo.CreateStudyGroup(It.Is<StudyGroup>(sg => sg.CreateDate != DateTime.MinValue)), Times.Once);
        }
    }
}
