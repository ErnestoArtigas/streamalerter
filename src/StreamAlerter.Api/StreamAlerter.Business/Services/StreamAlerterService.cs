using StreamAlerter.Core.Interfaces.Business;
using StreamAlerter.Core.Interfaces.DatabaseRepository;
using StreamAlerter.Core.Interfaces.HttpsRepository;
using StreamAlerter.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamAlerter.Business
{
    public class StreamAlerterService : IStreamAlerterService
    {
        private IStreamAlerterHttpsRepository streamAlerterHttpsRepository;
        private IStreamAlerterDatabaseRepository streamAlerterDatabaseRepository;
        public StreamAlerterService(IStreamAlerterHttpsRepository streamAlerterHttpsRepository, IStreamAlerterDatabaseRepository streamAlerterDatabaseRepository)
        {
            this.streamAlerterHttpsRepository = streamAlerterHttpsRepository;
            this.streamAlerterDatabaseRepository = streamAlerterDatabaseRepository;
        }

        public IEnumerable<Entities.Streamer> GetSavedStreamerDB()
        {
            return streamAlerterDatabaseRepository.SelectAll();
        }

        public Entities.Streamer Retrieve(string name)
        {
            return streamAlerterDatabaseRepository.Select(name);
        }

        public IEnumerable<Entities.Streamer> RetrieveAll()
        {
            return streamAlerterDatabaseRepository.SelectAll();
        }

        public Entities.Streamer Create(Entities.Streamer streamer)
        {
            streamer.InLive = false;
            return streamAlerterDatabaseRepository.Insert(streamer);
        }

        public Entities.Streamer Modify(string name, Entities.Streamer streamer)
        {
            return streamAlerterDatabaseRepository.Update(name, streamer);
        }

        public Entities.Streamer Upsert(Entities.Streamer streamer)
        {
            var streamerRetrieved = Retrieve(streamer.Name);
            if (streamerRetrieved == null)
                return Create(streamer);
            else
                return Modify(streamer.Name, streamer);
            throw new System.NotImplementedException();
        }

        public bool Remove(string name)
        {
            Entities.Streamer streamerToDelete = Retrieve(name);
            if (streamerToDelete == null)
                return false;
            return streamAlerterDatabaseRepository.Delete(streamerToDelete);
        }


        public async Task<IEnumerable<Streamer>> GetLiveStreamers()
        {
            var savedStreamerArray = GetSavedStreamerDB();
            List<Streamer> streamerInLiveArray = new List<Streamer>();

            foreach (Entities.Streamer element in savedStreamerArray)
            {
                var streamerArray = await streamAlerterHttpsRepository.GetTwitchUsers(element.Name);

                foreach (Streamer streamerElement in streamerArray)
                    // We need to do equals and not == because different cases
                    if (streamerElement.Name.Equals(element.Name, StringComparison.CurrentCultureIgnoreCase) && streamerElement.InLive)
                        streamerInLiveArray.Add(streamerElement);
            }

            return streamerInLiveArray;
        }
    }
}
