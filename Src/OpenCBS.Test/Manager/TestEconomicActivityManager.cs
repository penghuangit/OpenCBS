﻿// LICENSE PLACEHOLDER

using System.Collections.Generic;
using NUnit.Framework;
using OpenCBS.CoreDomain.EconomicActivities;
using OpenCBS.Manager;

namespace OpenCBS.Test.Manager
{
	/// <summary>
    /// Summary description for TestEconomicActivityManager.
	/// </summary>
	
	[TestFixture]
	public class TestEconomicActivityManager: BaseManagerTest
	{
	    private EconomicActivityManager _economicActivityManager;

        [Test]
        public void AddEconomicActivityWithoutChildren()
        {
            _economicActivityManager = (EconomicActivityManager) container["EconomicActivityManager"];
            EconomicActivity activity = new EconomicActivity
                                            {
                                                Name = "Services",
                                                Parent = null
                                            };
            activity.Id = _economicActivityManager.AddEconomicActivity(activity);
            Assert.AreNotEqual(0,activity.Id);
        }

        [Test]
        public void AddEconomicActivityWithParent()
        {
            _economicActivityManager = (EconomicActivityManager)container["EconomicActivityManager"];
            EconomicActivity activityParent = new EconomicActivity
            {
                Name = "Services",
                Parent = null
            };
            activityParent.Id = _economicActivityManager.AddEconomicActivity(activityParent);
            EconomicActivity activity = new EconomicActivity
            {
                Name = "Services",
                Parent = activityParent,
            };
            activity.Id = _economicActivityManager.AddEconomicActivity(activity);
            Assert.AreNotEqual(0, activity.Id);
        }

        [Test]
        public void SelectEconomicActivityByIdWithoutChildren()
        {
            _economicActivityManager = (EconomicActivityManager)container["EconomicActivityManager"];
            EconomicActivity activity = new EconomicActivity
            {
                Name = "Services",
                Parent = null
            };
            activity.Id = _economicActivityManager.AddEconomicActivity(activity);
            Assert.AreNotEqual(0, activity.Id);

            EconomicActivity selectedActivity = _economicActivityManager.SelectEconomicActivity(activity.Id);
            Assert.AreEqual(activity.Id,selectedActivity.Id);
            Assert.AreEqual("Services",selectedActivity.Name);
            Assert.AreEqual(false, selectedActivity.HasChildrens);
            Assert.AreEqual(0,selectedActivity.Childrens.Count);
        }

        [Test]
        public void UpdateEconomicActivity()
        {
            _economicActivityManager = (EconomicActivityManager)container["EconomicActivityManager"];
            EconomicActivity activity = new EconomicActivity
            {
                Name = "Services",
                Parent = null
            };
            activity.Id = _economicActivityManager.AddEconomicActivity(activity);

            activity.Name = "NewServices";
            activity.Deleted = true;
            _economicActivityManager.UpdateEconomicActivity(activity);

            EconomicActivity selectedActivity = _economicActivityManager.SelectEconomicActivity(activity.Id);
            Assert.AreEqual(activity.Id, selectedActivity.Id);
            Assert.AreEqual("NewServices", selectedActivity.Name);
            Assert.AreEqual(true, selectedActivity.Deleted);
        }

        [Test]
        public void ThisActivityAlreadyExistButInAAnotherActivity()
        {
            _economicActivityManager = (EconomicActivityManager)container["EconomicActivityManager"];
            EconomicActivity activityParentA = new EconomicActivity
            {
                Name = "Services",
                Parent = null
            };
            activityParentA.Id = _economicActivityManager.AddEconomicActivity(activityParentA);
            EconomicActivity activityParentB = new EconomicActivity
            {
                Name = "Services",
                Parent = null
            };
            activityParentB.Id = _economicActivityManager.AddEconomicActivity(activityParentB);
            EconomicActivity activityA = new EconomicActivity
            {
                Name = "ServicesA",
                Parent = activityParentA,
            };
            activityA.Id = _economicActivityManager.AddEconomicActivity(activityA);

            Assert.AreEqual(false, _economicActivityManager.ThisActivityAlreadyExist("ServicesA", activityParentB.Id));
        }

        [Test]
        public void ThisActivityAlreadyExistInThisActivity()
        {
            _economicActivityManager = (EconomicActivityManager)container["EconomicActivityManager"];
            EconomicActivity activityParentA = new EconomicActivity
            {
                Name = "Services",
                Parent = null
            };
            activityParentA.Id = _economicActivityManager.AddEconomicActivity(activityParentA);
            EconomicActivity activityParentB = new EconomicActivity
            {
                Name = "Services",
                Parent = null
            };
            activityParentB.Id = _economicActivityManager.AddEconomicActivity(activityParentB);
            EconomicActivity activityA = new EconomicActivity
            {
                Name = "ServicesA",
                Parent = activityParentA,
            };
            activityA.Id = _economicActivityManager.AddEconomicActivity(activityA);

            Assert.AreEqual(true, _economicActivityManager.ThisActivityAlreadyExist("ServicesA", activityParentA.Id));
        }

	    [Test]
        public void SelectEconomicActivityByIdWithChildren()
        {
            _economicActivityManager = (EconomicActivityManager)container["EconomicActivityManager"];
            EconomicActivity activityParent = new EconomicActivity
            {
                Name = "Services",
                Parent = null
            };
            activityParent.Id = _economicActivityManager.AddEconomicActivity(activityParent);
            EconomicActivity activityA = new EconomicActivity
            {
                Name = "ServicesA",
                Parent = activityParent,
            };
            activityA.Id = _economicActivityManager.AddEconomicActivity(activityA);
            EconomicActivity activityB = new EconomicActivity
            {
                Name = "ServicesB",
                Parent = activityParent,
            };
            activityB.Id = _economicActivityManager.AddEconomicActivity(activityB);

            EconomicActivity selectedActivity = _economicActivityManager.SelectEconomicActivity(activityParent.Id);

            Assert.AreEqual(activityParent.Id, selectedActivity.Id);
            Assert.AreEqual("Services", selectedActivity.Name);
            Assert.AreEqual(true, selectedActivity.HasChildrens);
            Assert.AreEqual(2, selectedActivity.Childrens.Count);
            Assert.AreEqual(false, selectedActivity.Deleted);
        }


        [Test]
        public void SelectAllEconomicActivities()
        {
            _economicActivityManager = (EconomicActivityManager)container["EconomicActivityManager"];
            EconomicActivity activityParentA = new EconomicActivity
            {
                Name = "Services",
                Parent = null
            };
            activityParentA.Id = _economicActivityManager.AddEconomicActivity(activityParentA);
            EconomicActivity activityParentB = new EconomicActivity
            {
                Name = "Services",
                Parent = null
            };
            activityParentB.Id = _economicActivityManager.AddEconomicActivity(activityParentB);
            EconomicActivity activityA = new EconomicActivity
            {
                Name = "ServicesA",
                Parent = activityParentA,
            };
            activityA.Id = _economicActivityManager.AddEconomicActivity(activityA);
            EconomicActivity activityB = new EconomicActivity
            {
                Name = "ServicesB",
                Parent = activityParentA,
            };
            activityB.Id = _economicActivityManager.AddEconomicActivity(activityB);

            List<EconomicActivity> activities = _economicActivityManager.SelectAllEconomicActivities();

            Assert.AreEqual(4, activities.Count);

            Assert.AreEqual(false, activities[0].HasChildrens);
            Assert.AreEqual(0, activities[0].Childrens.Count);

            Assert.AreEqual(false, activities[1].HasChildrens);
            Assert.AreEqual(0, activities[1].Childrens.Count);

            Assert.AreEqual(true, activities[2].HasChildrens);
            Assert.AreEqual(2, activities[2].Childrens.Count);

            Assert.AreEqual(false, activities[3].HasChildrens);
            Assert.AreEqual(0, activities[3].Childrens.Count);
        }
	}
}
