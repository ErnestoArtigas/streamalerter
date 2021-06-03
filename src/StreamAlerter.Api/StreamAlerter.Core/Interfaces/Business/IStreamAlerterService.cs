using StreamAlerter.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamAlerter.Core.Interfaces.Business
{
    public interface IStreamAlerterService
    {
        public IEnumerable<Entities.Streamer> GetSavedStreamerDB();
        public Entities.Streamer Create(Entities.Streamer streamer);
        public Task<IEnumerable<Streamer>> GetLiveStreamers();
        public Entities.Streamer Upsert(Entities.Streamer streamer);
        public Entities.Streamer Retrieve(string name);
        public IEnumerable<Entities.Streamer> RetrieveAll();
        public Entities.Streamer Modify(string name, Entities.Streamer streamer);
        public bool Remove(string name);
    }
}
