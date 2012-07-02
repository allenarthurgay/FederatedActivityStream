using System.Collections.Generic;
using ActivityEngine.Abstract;

namespace ActivityEngine.Concrete
{
	public class ActivityFeed : IActivityFeed
	{
		public ActivityFeed()
		{
			Activities = new List<Activity>();
		}
		public string UniqueId { get; set; }
		public List<Activity> Activities { get; set; }
	}
}
