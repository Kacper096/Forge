namespace Forge.Persistence.Redis
{
    public interface IRService
    {
        void Add(string key, string value, int db = 1);
        void Add<RType>(string key, RType model, int db = 1) where RType : class;
        string Get(string key, int db = 1);
        RType Get<RType>(string key, int db = 1) where RType : class;
    }
}