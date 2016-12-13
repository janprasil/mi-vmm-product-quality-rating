using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vmm.api.Models;

namespace vmm.api.Services
{
    public class FirebaseService : IDbManager
    {
        private const string FIREBASE_URL = "https://vmm-semestral.firebaseio.com/";
        private FirebaseClient firebase;

        public FirebaseService()
        {
            //var api = "AIzaSyDfevwNo9zhgMZ2__Bi0_-v-LKaQj3Jq90"; // API key - not used at this time
            firebase = new FirebaseClient(FIREBASE_URL);
        }

        public async Task DeleteAllAsync<T>()
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            await firebase.Child(type).OrderByKey().DeleteAsync();
        }

        public async Task DeleteAsync<T>(string name)
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            await firebase.Child(type).OrderByKey().EqualTo(name).DeleteAsync();
        }

        public async Task<IReadOnlyCollection<FirebaseObject<T>>> GetAllAsync<T>()
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            return await firebase.Child(type).OrderByKey().OnceAsync<T>();
        }

        public async Task<FirebaseObject<T>> GetAsync<T>(string name)
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            var items = await firebase.Child(type).OrderByKey().EqualTo(name).OnceAsync<T>();
            if (items.Count == 0) return null;
            return items.FirstOrDefault();
        }

        public async Task<FirebaseObject<T>> PostAsync<T>(T o)
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            return await firebase.Child(type).PostAsync(o);
        }

        public async Task PutAsync<T>(string name, T o)
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            await firebase.Child(type).Child(name).PutAsync(o);
        }

        private string getNodeName(Type t)
        {
            if (t.Equals(typeof(Shape)))
            {
                return "ReferenceSamples";
            }
            return null;
        }
    }
}