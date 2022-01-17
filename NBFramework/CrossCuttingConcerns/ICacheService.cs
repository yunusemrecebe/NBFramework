namespace NbFramework.CrossCuttingConcerns 
{ 
    public interface ICacheService
    {
        T Get<T>(string key);
        object Get(string key);
        void Add(string key, object data, int duration);
        void Remove(string key);
        void RemoveByPattern(string pattern);
        bool IsExist(string key);
    }
}
