using System;
using System.Collections.Generic;
using System.Linq;
using ActivityEngine.Abstract;
using ServiceStack.Redis;

namespace ActivityEngine.Concrete
{
	public class ActivityFeedRepository : IActivityFeedRepository
	{
		private readonly List<IActivityFeed> _activityFeeds;

		private string _host;
		private int port;

		public ActivityFeedRepository()
		{
			_activityFeeds = new List<IActivityFeed>();
		}

		public IActivityFeed GetFeed(string uniqueId, DateTime dateTime)
		{
			var retval = new ActivityFeed { UniqueId = uniqueId };

			WithRedisClient((redisClient) =>
			{
				var typed = redisClient.GetTypedClient<Activity>();
				
					var activities = typed.Lists[uniqueId];
					retval.Activities = activities
						.Where(a => a.When > dateTime)
						.OrderByDescending(a => a.When)
						.ToList();
				
			});

			return retval;
		}

		public IActivityFeed GetMergedFeed(IEnumerable<string> uniqueIds, DateTime dateTime)
		{
			var retval = new ActivityFeed { UniqueId = "I'm a list beyatch" };
			
			WithRedisClient((redisClient) =>
			{
				var typed = redisClient.GetTypedClient<Activity>();

				var list = new List<Activity>();
					foreach (string uniqueId in uniqueIds)
					{
						var activities = typed.Lists[uniqueId];
						var sel = activities.Where(a => a != null && a.When > dateTime).ToList();
						list.AddRange(sel);
					}
					
					retval.Activities = list
						.OrderByDescending(a => a.When)
						.ToList();
				
			});

			return retval;
		}

		public void SendActivity(string uniqueId, Activity activity)
		{			
			WithRedisClient((redisClient) =>
			{
				var typed = redisClient.GetTypedClient<Activity>();
				
					var activities = typed.Lists[uniqueId];
					activities.Add(activity);
					
				
			});			
		}

		private void WithRedisClient(Action<RedisClient> action)
		{
			using (var redisClient = new RedisClient("lab.redistogo.com", 9094, "1605d3ae3a864ab1317eabf0927328fd"))
			{
				action(redisClient);
			}
		}
	}
}
