using NUnit.Framework;
using System;
using System.Collections.Generic;
using TestApp;

namespace EPAM.Tests
{
    public class UnitTest
    {
        private StudyGroup _studyGroup;

        // Global test data
        private const int ValidStudyGroupId = 1;
        private const string ValidStudyGroupName = "Math Study Group";
        private const Subject ValidStudyGroupSubject = Subject.Math;
        private readonly DateTime ValidCreateDate = DateTime.Now;
        private readonly List<User> ValidUsers = new List<User> { new User("Ahad") };

        private const string InvalidShortName = "Test";
        private const string InvalidLongName = "Fail Physics Study Group - Ahad";
        private const Subject InvalidSubject = (Subject)3;
        private const Subject ValidSubject = Subject.Chemistry;
        private const string DuplicateUserName = "Ahad";

        private const string ChemistryStudyGroupName = "Chemistry Study Group";
        private const string PhysicsStudyGroupName = "Physics Study Group";

        [SetUp]
        public void Setup()
        {
            _studyGroup = new StudyGroup(ValidStudyGroupId, ValidStudyGroupName, ValidStudyGroupSubject, ValidCreateDate, ValidUsers);
        }

        // Unit test cases

        // TC51: Verify Study Group Constructor
        [Test]
        public void TC51_Verify_Study_Group_Constructor()
        {
            Assert.That(_studyGroup.StudyGroupId, Is.EqualTo(ValidStudyGroupId));
            Assert.That(_studyGroup.Name, Is.EqualTo(ValidStudyGroupName));
            Assert.That(_studyGroup.Subject, Is.EqualTo(ValidStudyGroupSubject));
            Assert.That(_studyGroup.CreateDate, Is.EqualTo(ValidCreateDate));
            CollectionAssert.AreEqual(ValidUsers, _studyGroup.Users);
        }

        // TC52: Verify Study Group Constructor with multiple subjects
        [Test]
        public void TC52_Verify_Study_Group_Constructor_With_Multiple_Subjects()
        {
            Assert.Throws<ArgumentException>(() => new StudyGroup(ValidStudyGroupId, ValidStudyGroupName, new List<Subject> { ValidStudyGroupSubject }, ValidCreateDate, ValidUsers));
        }

        // TC53: Verify Study Group Name Validation
        [Test]
        public void TC53_Verify_Study_Group_Name_Validation()
        {
            Assert.Throws<ArgumentException>(() => new StudyGroup(ValidStudyGroupId, InvalidShortName, ValidStudyGroupSubject, ValidCreateDate, ValidUsers));
        }

        // TC54: Verify Study Group Name Validation
        [Test]
        public void TC54_Verify_Study_Group_Name_Validation()
        {
            Assert.Throws<ArgumentException>(() => new StudyGroup(ValidStudyGroupId, InvalidLongName, ValidStudyGroupSubject, ValidCreateDate, ValidUsers));
        }

        // TC55: Verify Study Group Subject Validation
        [Test]
        public void TC55_Verify_Study_Group_Subject_Validation()
        {
            Assert.Throws<ArgumentException>(() => new StudyGroup(ValidStudyGroupId, ChemistryStudyGroupName, InvalidSubject, ValidCreateDate, ValidUsers));
        }

        // TC56: Verify Study Group Subject Validation
        [Test]
        public void TC56_Verify_Study_Group_Subject_Validation()
        {
            Assert.DoesNotThrow(() => new StudyGroup(ValidStudyGroupId, ChemistryStudyGroupName, ValidSubject, ValidCreateDate, ValidUsers));
        }

        // TC57: Verify Study Group Subject Validation
        [Test]
        public void TC57_Verify_Study_Group_Subject_Validation()
        {
            Assert.DoesNotThrow(() => new StudyGroup(ValidStudyGroupId, PhysicsStudyGroupName, Subject.Physics, ValidCreateDate, ValidUsers));
        }

        // TC58: Verify Study Group User Addition
        [Test]
        public void TC58_Verify_Study_Group_User_Addition()
        {
            var user = new User(DuplicateUserName);
            _studyGroup.AddUser(user);

            Assert.That(_studyGroup.Users.Count, Is.EqualTo(2));
            Assert.That(_studyGroup.Users[1].Name, Is.EqualTo(DuplicateUserName));
        }

        // TC59: Verify Study Group User Removal
        [Test]
        public void TC59_Verify_Study_Group_User_Removal()
        {
            var user = new User(DuplicateUserName);
            _studyGroup.RemoveUser(user);

            Assert.That(_studyGroup.Users.Count, Is.EqualTo(1));
        }


        // TC60: Verify Study Group Duplicate User Addition
        [Test]
        public void TC60_Verify_Study_Group_Duplicate_User_Addition()
        {
            var user = new User(DuplicateUserName);
            _studyGroup.AddUser(user);

            Assert.That(_studyGroup.Users.Count, Is.EqualTo(1));
        }

        // TC61: Verify Study Group All User Removal
        [Test]
        public void TC61_Verify_Study_Group_All_User_Removal()
        {
            _studyGroup.Users.Clear();

            Assert.That(_studyGroup.Users.Count, Is.EqualTo(0));
        }

        // TC62: Verify Study Group Creation Failure Handling
        [Test]
        public void TC62_Verify_Study_Group_Creation_Failure_Handling()
        {
            Assert.Throws<ArgumentException>(() => new StudyGroup(null, null, Subject.Biology, null, null));
        }

        // TC63: Verify Study Group Deletion Failure Handling
        [Test]
        public void TC63_Verify_Study_Group_Deletion_Failure_Handling()
        {
            Assert.Throws<ArgumentException>(() => new StudyGroup(null, null, Subject.Biology, null, null));
        }

        // TC64: Verify Study Group Update Failure Handling
        [Test]
        public void TC64_Verify_Study_Group_Update_Failure_Handling()
        {
            Assert.Throws<ArgumentException>(() => new StudyGroup(null, null, Subject.Biology, null, null));
        }
    }
}
