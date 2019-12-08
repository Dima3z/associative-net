using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AssociativeNetwork.Helpers
{
    public static class BaseHelper
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void SaveJson<T>(this T obj, string filename)
        {
            var jobject = JObject.FromObject(
                obj,
                new JsonSerializer {NullValueHandling = NullValueHandling.Ignore});
            File.WriteAllText(filename, jobject.ToString());
        }

        public static T LoadJson<T>(string filename)
        {
            var jobject = JObject.Parse(File.ReadAllText(filename));
            return jobject.ToObject<T>();
        }
    }
}