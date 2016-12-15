using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using vmm.api.Models;

namespace vmm.api.Services
{
    public interface IDbManager
    {
        Task<FirebaseObject<T>> PostAsync<T>(T o) where T : IModel;

        Task<KeyValuePair<string, IReadOnlyCollection<FirebaseObject<T>>>> PostAllAsync<T>(IEnumerable<T> o);

        Task PutAsync<T>(string s, T o);

        Task<FirebaseObject<T>> GetAsync<T>(string name);

        Task<IReadOnlyCollection<FirebaseObject<T>>> GetAllAsync<T>();

        Task DeleteAsync<T>(string name);

        Task DeleteAllAsync<T>();

        Task<KeyValuePair<string, IReadOnlyCollection<FirebaseObject<T>>>> PostAllAsync<T>(string[] path, IEnumerable<T> o);

        Task<FirebaseObject<T>> PostAsync<T>(string[] path, T o) where T : IModel;

        Task PutAsync(string[] s, object o);

        Task<IReadOnlyCollection<FirebaseObject<T>>> GetAllAsync<T>(string[] path);

        Task<T> GetAsync<T>(string[] path);

        Task DeleteAsync(string[] path);

    }
}