using StreamAlerter.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamAlerter.Core.Interfaces.HttpsRepository
{
    public interface IStreamAlerterHttpsRepository
    {
        public Task<IEnumerable<Streamer>> GetTwitchUsers(string streamerName);
    }
}
