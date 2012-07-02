using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ActivityEngine.Abstract;
using ActivityEngine.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityTests
{
	[TestClass]
	public class UnitTest1
	{
		private IActivityFeedRepository _activityRepo;
		private Actor _actor;
		private string _feedId;

		[TestInitialize]
		public void Setup()
		{
			_activityRepo = new ActivityFeedRepository();
			_feedId = "blahblah";
			_actor = new Actor
				{
					Email = "yourface@gmail.com",
					Name = "Your face",
					UniqueId = Guid.NewGuid().ToString()
				};
		}

		[TestMethod]
		public void SendActivity()
		{
			_activityRepo.SendActivity(_feedId, new SimpleActivity
			{
				What = "my butt",
				When = DateTime.Now,
				Who = _actor 
			});
		}

		[TestMethod]
		public void GetAllActivities()
		{
			_activityRepo.SendActivity(_feedId, new SimpleActivity
			{
				What = "my butt",
				When = DateTime.Now,
				Who = _actor
			});

			var activities = _activityRepo.GetFeed(_feedId, DateTime.MinValue);
			Assert.IsTrue(activities.Activities.Any());

			var activity = activities.Activities.FirstOrDefault(a=> a.What.ToString()=="my butt");
			Assert.IsNotNull(activity);
		}

		[TestMethod]
		public void GetActivitiesAfterDate()
		{
			var oldActivity = new SimpleActivity
			{
				What = "oldActivity",
				When = DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0)),
				Who = _actor
			};

			var newActivity = new SimpleActivity
			{
				What = "newActivity",
				When = DateTime.Now,
				Who = _actor
			};
			_activityRepo.SendActivity(_feedId, oldActivity);
			_activityRepo.SendActivity(_feedId, newActivity);

			var feed = _activityRepo.GetFeed(_feedId, DateTime.Now.Subtract(new TimeSpan(1, 0, 0)));

			var activity = feed.Activities.FirstOrDefault(a => a.What.ToString() == "newActivity");
			Assert.IsNotNull(activity);

			var ooactivity = feed.Activities.FirstOrDefault(a => a.What.ToString() == "oldActivity");
			Assert.IsNull(ooactivity);
		}

	}
}
