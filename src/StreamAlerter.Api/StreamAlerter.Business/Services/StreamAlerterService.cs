using StreamAlerter.Core.Interfaces.Business;
using StreamAlerter.Core.Interfaces.DatabaseRepository;
using StreamAlerter.Core.Interfaces.HttpsRepository;
using StreamAlerter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreamAlerter.Business
{
    public class StreamAlerterService : IStreamAlerterService
    {
        private IStreamAlerterHttpsRepository streamAlerterHttpsRepository;
        private IStreamAlerterDatabaseRepository streamAlerterDatabaseRepository;
        private static Dictionary<string, bool> alreadyNotifiedStreamer = new Dictionary<string, bool>();
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

        public bool isStreamerCached(string streamerName)
        {
            return alreadyNotifiedStreamer.ContainsKey(streamerName);
        }

        public async Task<IEnumerable<Entities.Streamer>> GetLiveStreamers()
        {
            var savedStreamerArray = GetSavedStreamerDB();
            List<Entities.Streamer> streamerInLiveArray = new List<Streamer>();

            // need to put all alreadyNotifiedStreamer to false, then if they are still in live we put them true, at the end if they are not in live anymore we remove them from the list
            foreach (var element in alreadyNotifiedStreamer)
                alreadyNotifiedStreamer[element.Key] = false;

            foreach (Entities.Streamer element in savedStreamerArray)
            {
                var streamerArray = await streamAlerterHttpsRepository.GetTwitchUsers(element.Name);

                foreach (Entities.Streamer streamerElement in streamerArray)
                    if (streamerElement.Name.Equals(element.Name, StringComparison.CurrentCultureIgnoreCase) && streamerElement.InLive)
                    {
                        if (!isStreamerCached(streamerElement.Name))
                        {
                            alreadyNotifiedStreamer.Add(streamerElement.Name, true);
                            streamerInLiveArray.Add(streamerElement);
                        }
                        else
                            alreadyNotifiedStreamer[streamerElement.Name] = true;
                    }
            }

            List<string> notifiedStreamerToDelete = new List<string>();
            foreach (var element in alreadyNotifiedStreamer)
                if (alreadyNotifiedStreamer[element.Key] == false)
                    notifiedStreamerToDelete.Add(element.Key);

            foreach (var element in notifiedStreamerToDelete)
                alreadyNotifiedStreamer.Remove(element);

            return streamerInLiveArray;
        }
    }
}
