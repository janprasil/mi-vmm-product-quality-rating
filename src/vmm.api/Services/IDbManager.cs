using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace vmm.api.Services
{
    public interface IDbManager
    {
        Task<FirebaseObject<T>> PostAsync<T>(T o);

        Task PutAsync<T>(string s, T o);

        Task<FirebaseObject<T>> GetAsync<T>(string name);

        Task<IReadOnlyCollection<FirebaseObject<T>>> GetAllAsync<T>();

        Task DeleteAsync<T>(string name);

        Task DeleteAllAsync<T>();
    }
}