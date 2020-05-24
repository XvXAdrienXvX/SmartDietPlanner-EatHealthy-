using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDietPlan.Models
{
    public static class FoodExtensions
    {

        //private const string CartSessionKey = "FoodStore";

        //public static List<Meal> GetCart(this ISession session)
        //{
        //    return session.GetObjectFromJson<List<Meal>>(CartSessionKey) ?? new List<Meal>();
        //}

        //public static void SaveCart(this ISession session, List<Meal> cart)
        //{
        //    session.SetObjectAsJson(CartSessionKey, cart);
        //}
        //public static void SetObjectAsJson(this ISession session, string key, object value)
        //{
        //    session.SetString(key, JsonConvert.SerializeObject(value));
        //}

        //public static T GetObjectFromJson<T>(this ISession session, string key)
        //{
        //    var value = session.GetString(key);

        //    return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        //}
        private const string CartSessionKey = "FoodList";
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static List<Meal> GetCart(this ISession session)
        {
            return session.GetObjectFromJson<List<Meal>>(CartSessionKey) ?? new List<Meal>();
        }

        public static void SaveCart(this ISession session, List<Meal> cart)
        {
            session.SetObjectAsJson(CartSessionKey, cart);
        }
    }
   
}
