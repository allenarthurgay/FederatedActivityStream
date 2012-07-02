using System;
using System.Collections.Generic;

namespace ActivityEngine.Abstract
{
	public interface IActivityFeedRepository
	{
		IActivityFeed GetFeed(string uniqueId,DateTime dateTime);

		IActivityFeed GetMergedFeed(IEnumerable<string> uniqueIds, DateTime dateTime);

		void SendActivity(string uniqueId, Activity activity);
	}



	public interface IActivityFeed
	{
		string UniqueId { get; set; } //enterpriseid, proejctid, principalid

		List<Activity> Activities { get; set; }
	}

	public class Activity
	{
		public Actor Who { get; set; }

		public string Verb { get; set; }

		public string What { get; set; }

		public DateTime When { get; set; }		
	}

	public class Actor
	{
		public string Name { get; set; }

		public string Email { get; set; }

		public string UniqueId { get; set; }
	}
}
