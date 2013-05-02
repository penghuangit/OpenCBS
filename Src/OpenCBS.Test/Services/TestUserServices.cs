﻿// LICENSE PLACEHOLDER

using System.Collections.Generic;
using OpenCBS.Services;
using OpenCBS.CoreDomain;
using NUnit.Framework;
using NUnit.Mocks;
using OpenCBS.ExceptionsHandler;
using OpenCBS.Manager;

namespace OpenCBS.Test.Services
{
    [TestFixture]
    public class TestUserServices
    {
        private DynamicMock _mockUserManagement;

        [SetUp]
        public void SetUp()
        {
            _mockUserManagement = new DynamicMock(typeof(UserManager));
        }

        private static User _AddUser(int pId, string pUserName, string pPassword, string pRole)
        {
            User user = new User {Id = pId, UserName = pUserName, Password = pPassword};
            user.SetRole(pRole);
            return user;
        }

        private static UserServices _SetMockManager(DynamicMock pDynamicMock)
        {
            UserManager userManager = (UserManager)pDynamicMock.MockInstance;
            return new UserServices(userManager);
        }

        [Test]
        public void TestDeleteUser()
        {
            Assert.Ignore();
            List<User> users = new List<User>();
            User user = _AddUser(3, "nicolas", "nicolas", "ADMIN");
            users.Add(new User());

            _mockUserManagement.SetReturnValue("SelectAllUsers", users);
            _mockUserManagement.ExpectAndReturn("Delete", users, user);

            UserServices userServices = _SetMockManager(_mockUserManagement);

            Assert.AreEqual(true, userServices.Delete(user));
            List<User> list = userServices.FindAll(true);
            Assert.AreEqual(1, list.Count);
        }

        [Test]
        [ExpectedException(typeof(OctopusUserDeleteException))]
        public void TestDeleteUserWhenUserIsNull()
        {
            List<User> users = new List<User>();
            User user = _AddUser(0, "nicolas", "nicolas", "ADMIN");
            users.Add(new User());

            _mockUserManagement.SetReturnValue("SelectAllActiveUsers", users);
            _mockUserManagement.ExpectAndReturn("Delete", users, user);

            UserServices userServices = _SetMockManager(_mockUserManagement);
            
            userServices.Delete(user);
        }

        [Test]
        [ExpectedException(typeof(OctopusUserDeleteException))]
        public void TestDeleteAdministratorUser()
        {
            List<User> users = new List<User>();
            User user = _AddUser(1, "nicolas", "nicolas", "ADMIN");
            users.Add(new User());

            _mockUserManagement.SetReturnValue("SelectAllActiveUsers", users);
            _mockUserManagement.ExpectAndReturn("Delete", users, user);

            UserServices userServices = _SetMockManager(_mockUserManagement);

            userServices.Delete(user);
        }

        public void TestSaveUserWhenUserNameIsNull()
        {
            User user =_AddUser(0, null, "nicolas", "LOF");

            _mockUserManagement.ExpectAndReturn("AddUser", 1, user);

            UserServices userServices = _SetMockManager(_mockUserManagement);

            Assert.IsTrue(userServices.SaveUser(user).FindError);
        }

        [Test]
        public void TestSaveUserWhenUserAlreadyExits()
        {
            User user = _AddUser(0, "nicolas", "nicolas", "LOF");

            _mockUserManagement.ExpectAndReturn("AddUser", 1, user);
            _mockUserManagement.ExpectAndReturn("SelectUser", user, "nicolas", "nicolas");
            UserServices userServices = _SetMockManager(_mockUserManagement);

            Assert.IsTrue(userServices.SaveUser(user).FindError);
        }

        [Test]
        public void TestSaveUserWhenPasswordIsNull()
        {
            User user = _AddUser(0, "nicolas", null, "ADMIN");

            _mockUserManagement.ExpectAndReturn("AddUser", 1, user);
            UserServices userServices = _SetMockManager(_mockUserManagement);

            Assert.IsTrue(userServices.SaveUser(user).FindError);
        }

        [Test]
        public void TestSaveUserWhenRoleIsNull()
        {
            User user = _AddUser(0, "nicolas", "nicolas", null);

            _mockUserManagement.ExpectAndReturn("AddUser", 1, user);
            UserServices userServices = _SetMockManager(_mockUserManagement);

            Assert.IsTrue(userServices.SaveUser(user).FindError);
        }

        [Test]
        public void TestSaveUserWithMaximumAttributs()
        {
            User user = _AddUser(0, "nicolas", "nicolas", "LOF");
            user.FirstName = "nicolas";
            user.LastName = "Nicolas";
            user.Mail = "Not Set";
            user.UserRole = new Role {Id = 1, RoleName = "LOF"};
            _mockUserManagement.ExpectAndReturn("AddUser", 1, user);
            _mockUserManagement.ExpectAndReturn("Find", null, "nicolas", "nicolas");
            _mockUserManagement.ExpectAndReturn("SelectAll", new List<User>());
            UserServices userServices = _SetMockManager(_mockUserManagement);

            Assert.IsFalse(userServices.SaveUser(user).FindError);
        }

        [Test]
        public void TestFindUserByIdWhenReturnIsNull()
        {
            Assert.Ignore();
            _mockUserManagement.ExpectAndReturn("SelectUser", null, 0,false);
            _mockUserManagement.ExpectAndReturn("LoadUsers", new List<User>());
            UserServices userServices = _SetMockManager(_mockUserManagement);
            Assert.IsNull(userServices.Find(0));
        }

        [Test]
        public void TestFindUserByIdWhenReturnIsNotNull()
        {
            Assert.Ignore();
            User user = _AddUser(1, "mariam", "nicolas", "ADMIN");
            user.FirstName = "nicolas";

            _mockUserManagement.ExpectAndReturn("SelectUser", user, user.Id, false);
            _mockUserManagement.ExpectAndReturn("SelectAll", new List<User>());
            UserServices userServices = _SetMockManager(_mockUserManagement);
            Assert.AreEqual("mariam", userServices.Find(user.Id).UserName);
        }

        [Test]
        public void TestFindUserByUserNameWhenReturnIsNotNull()
        {
            Assert.Ignore();
            User user = _AddUser(1, "nicolas", "nicolas", "ADMIN");

            _mockUserManagement.ExpectAndReturn("SelectUser", user, user.Id, false);
            UserServices userServices = _SetMockManager(_mockUserManagement);
            Assert.AreEqual("nicolas", userServices.Find(user.Id).UserName);
        }
        [Test]
        public void TestFindUserByUserNameAndPasswordWhenReturnIsNull()
        {
            Assert.Ignore();
            _mockUserManagement.ExpectAndReturn("SelectUser", null, "neverUse", "neverUse");
            UserServices userServices = _SetMockManager(_mockUserManagement);
            Assert.IsNull(userServices.Find("neverUse", "neverUse"));
        }

        [Test]
        public void TestFindUserByUserNameAndPasswordWhenReturnIsNotNull()
        {
            Assert.Ignore();
            User user = _AddUser(1, "nicolas", "nicolas", "LOF");

            _mockUserManagement.ExpectAndReturn("SelectUser", null, user.UserName, user.Password);
            _mockUserManagement.ExpectAndReturn("SelectAll", new List<User>());
            UserServices userServices = _SetMockManager(_mockUserManagement);
            Assert.IsNull(userServices.Find(user.UserName, user.Password));
        }

        [Test]
        public void TestFindAllUsersWhenReturnIsNull()
        {
            Assert.Ignore();
            List<User> users = new List<User>();

            _mockUserManagement.SetReturnValue("SelectAllUsers", users);
            _mockUserManagement.ExpectAndReturn("SelectAll", new List<User>());
            UserServices userServices = _SetMockManager(_mockUserManagement);
            Assert.AreEqual(0, userServices.FindAll(true).Count);
        }

        [Test]
        public void TestFindAllUsersWhenReturnIsNotNull()
        {
            Assert.Ignore();
            List<User> users = new List<User> {new User()};

            _mockUserManagement.SetReturnValue("SelectAllUsers", users);
            UserServices userServices = _SetMockManager(_mockUserManagement);
            Assert.AreEqual(1, userServices.FindAll(true).Count);
        }
    }
}
