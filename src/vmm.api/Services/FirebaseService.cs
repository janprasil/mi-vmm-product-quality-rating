using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using vmm.api.Models;
using Firebase.Database.Query;
using System.Drawing;
using Emgu.CV.Structure;

namespace vmm.api.Services
{
    public class FirebaseService : IDbManager
    {

        private static string FIREBASE_URL = "https://vmm-semestral.firebaseio.com/";
        private FirebaseClient firebase;

        public FirebaseService()
        {
            //var api = "AIzaSyDfevwNo9zhgMZ2__Bi0_-v-LKaQj3Jq90"; // API key - not used at this tiem
            this.firebase = new FirebaseClient(FirebaseService.FIREBASE_URL);
        }

        public async Task deleteAllAsync<T>()
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            await firebase.Child(type).OrderByKey().DeleteAsync();
        }

        public async Task deleteAsync<T>(string name)
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            await firebase.Child(type).OrderByKey().EqualTo(name).DeleteAsync();
        }

        public async Task<IReadOnlyCollection<FirebaseObject<T>>> getAllAsync<T>()
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            var items = firebase.Child(type);
            var items2 =  items.OrderByKey();
            var items3 = await items2.OnceAsync<T>();

            //.OrderByValue().OnceAsync<T>();
            return items3;
        }

        public async Task<FirebaseObject<T>> getAsync<T>(string name)
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            var items = await firebase.Child(type).OrderByKey().EqualTo(name).OnceAsync<T>();
            if (items.Count == 0) return null;
            return items.FirstOrDefault();
        }

        public async Task<FirebaseObject<T>> postAsync<T>(T o)
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            return await firebase.Child(type).PostAsync(o);
        }

        public async Task putAsync<T>(String name, T o)
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            await firebase.Child(type).Child(name).PutAsync(o);
        }

        private String getNodeName(Type t)
        {
            if (t.Equals(typeof(Shape)))
            {
                return "ReferenceSamples";
            }
            return null;
        } 
    }

    public class Shape2
    {
        public string ImageUrl { get; set; }
        public string ContourImageUrl { get; set; }
        public string LocalPath { get; set; }
        public IEnumerable<Point> Points { get; set; }
        public MCvPoint2D64f Center { get; set; }
        public IEnumerable<double> Timeline { get; set; }
    }
}
