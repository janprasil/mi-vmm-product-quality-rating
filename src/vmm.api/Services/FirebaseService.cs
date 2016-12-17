﻿using Firebase.Database;
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
            firebase = new FirebaseClient(FIREBASE_URL);
        }

        public async Task DeleteAllInSessionAsync(string type, string sessionId)
        {
            await firebase.Child(type).Child(sessionId).DeleteAsync();
        }

        public async Task DeleteAllAsync<T>()
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            await firebase.Child(type).DeleteAsync();
        }

        public async Task DeleteAsync(string[] path)
        {
            if (path.Count() < 1) return;
            var nesting = firebase.Child(path[0]);
            for (var i = 1; i < path.Count(); i++) nesting = nesting.Child(path[i]);
            await nesting.DeleteAsync();
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

        public async Task<IReadOnlyCollection<FirebaseObject<T>>> GetAllAsync<T>(string[] path)
        {
            if (path.Count() < 1) return null;

            var nesting = firebase.Child(path[0]);
            for (var i = 1; i < path.Count(); i++) nesting = nesting.Child(path[i]);
            var result = await nesting.OnceAsync<T>();
            return result;
        }

        public async Task<T> GetAsync<T>(string[] path)
        {
            if (path.Count() < 1) return default(T);
            var nesting = firebase.Child(path[0]);
            for (var i = 1; i < path.Count(); i++) nesting = nesting.Child(path[i]);
            //var result = await nesting.OrderByKey().EqualTo(path.ElementAt(path.Count() - 1)).OnceSingleAsync<T>();
            return await nesting.OnceSingleAsync<T>();
        }

        public async Task<FirebaseObject<T>> GetAsync<T>(string name)
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            var items = await firebase.Child(type).OrderByKey().EqualTo(name).OnceAsync<T>();
            if (items.Count == 0) return null;
            return items.FirstOrDefault();
        }

        public async Task<FirebaseObject<T>> PostAsync<T>(string[] path, T o) where T : IModel
        {
            if (path.Count() < 1) return null;
            var nesting = firebase.Child(path[0]);
            for (var i = 1; i < path.Count(); i++) nesting = nesting.Child(path[i]);
            return await nesting.PostAsync(o);
        }

        public async Task<FirebaseObject<T>> PostAsync<T>(T o) where T : IModel
        {
            var type = getNodeName(typeof(T));
            if (type == null) throw new ChildNotExists();
            return await firebase.Child(type).PostAsync(o);
        }

        public async Task PutAsync(string[] path, object o)
        {
            if (path.Count() < 1) return;
            var nesting = firebase.Child(path[0]);
            for (var i = 1; i < path.Count(); i++) nesting = nesting.Child(path[i]);
            await nesting.PutAsync(o);
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
            if (t.Equals(typeof(List<Shape>)))
            {
                return "Images";
            }
            return null;
        }

        public async Task<KeyValuePair<string, IReadOnlyCollection<FirebaseObject<T>>>> PostAllInSessionAsync<T>(string sessionId, IEnumerable<T> o)
        {
            var type = getNodeName(o.GetType());
            if (type == null) throw new ChildNotExists();
            foreach (var x in o) await firebase.Child(type).Child(sessionId).PostAsync(x);
            return new KeyValuePair<string, IReadOnlyCollection<FirebaseObject<T>>>(sessionId, await GetAllAsync<T>(new string[] { type, sessionId }));
        }

        public async Task<KeyValuePair<string, IReadOnlyCollection<FirebaseObject<T>>>> PostAllAsync<T>(IEnumerable<T> o)
        {
            var type = getNodeName(o.GetType());
            if (type == null) throw new ChildNotExists();
            var node = await firebase.Child(type).PostAsync(true);
            foreach (var x in o) await firebase.Child(type).Child(node.Key).PostAsync(x);
            return new KeyValuePair<string, IReadOnlyCollection<FirebaseObject<T>>>(node.Key, await GetAllAsync<T>(new string[] { type, node.Key }));
        }

        public async Task<KeyValuePair<string, IReadOnlyCollection<FirebaseObject<T>>>> PostAllAsync<T>(string[] path, IEnumerable<T> o)
        {
            if (path.Count() < 1) return new KeyValuePair<string, IReadOnlyCollection<FirebaseObject<T>>>(null, null);
            var nesting = firebase.Child(path[0]);
            for (var i = 1; i < path.Count(); i++) nesting = nesting.Child(path[i]);
            var node = await nesting.PostAsync(true);
            foreach (var x in o) await nesting.Child(node.Key).PostAsync(x);
            var pathWithKey = path.ToList();
            pathWithKey.Add(node.Key);
            return new KeyValuePair<string, IReadOnlyCollection<FirebaseObject<T>>>(node.Key, await GetAllAsync<T>(pathWithKey.ToArray()));
        }
    }
}