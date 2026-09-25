using System.Text.Json;
using Mercado.Models;

namespace Mercado.Helpers
{
    public static class RecentlyViewedTracker
    {
        private const int MaxItems = 5;

        public static void Track(ISession session, string entityKey, int id, string name)
        {
            string sessionKey = $"RecentlyViewed_{entityKey}";

            var list = GetRecent(session, entityKey);

            list.RemoveAll(x => x.Id == id);
            list.Insert(0, new RecentlyViewedItem { Id = id, Name = name, ViewedAt = DateTime.Now });

            if (list.Count > MaxItems)
                list = list.Take(MaxItems).ToList();

            session.SetString(sessionKey, JsonSerializer.Serialize(list));
        }

        public static List<RecentlyViewedItem> GetRecent(ISession session, string entityKey)
        {
            var json = session.GetString($"RecentlyViewed_{entityKey}");
            if (string.IsNullOrEmpty(json))
                return new List<RecentlyViewedItem>();

            return JsonSerializer.Deserialize<List<RecentlyViewedItem>>(json) ?? new List<RecentlyViewedItem>();
        }
    }
}