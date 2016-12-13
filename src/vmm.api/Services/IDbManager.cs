using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vmm.api.Services
{
    public interface IDbManager
    {
        Task<FirebaseObject<T>> postAsync<T>(T o);
        Task putAsync<T>(String name, T o);
        Task<FirebaseObject<T>> getAsync<T>(String name);
        Task<IReadOnlyCollection<FirebaseObject<T>>> getAllAsync<T>();
        Task deleteAsync<T>(String name);
        Task deleteAllAsync<T>();
    }
}
