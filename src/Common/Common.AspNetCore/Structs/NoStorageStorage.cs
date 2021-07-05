using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Monitoring;
using DotNetCore.CAP.Persistence;
using DotNetCore.CAP.Serialization;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace DotNetCore.CAP.NoStorage
{
    internal class NoStorageStorage : IDataStorage
    {
        private readonly IOptions<CapOptions> _capOptions;
        private readonly ISerializer _serializer;

        public NoStorageStorage(IOptions<CapOptions> capOptions, ISerializer serializer)
        {
            _capOptions = capOptions;
            _serializer = serializer;
        }

        public static Dictionary<string, NoStoreMessage> PublishedMessages { get; } = new Dictionary<string, NoStoreMessage>();

        public static Dictionary<string, NoStoreMessage> ReceivedMessages { get; } = new Dictionary<string, NoStoreMessage>();

        public Task ChangePublishStateAsync(MediumMessage message, StatusName state)
        {
            // PublishedMessages[message.DbId].StatusName = state;
            // PublishedMessages[message.DbId].ExpiresAt = message.ExpiresAt;
            // PublishedMessages[message.DbId].Content = _serializer.Serialize(message.Origin);
            return Task.CompletedTask;
        }

        public Task ChangeReceiveStateAsync(MediumMessage message, StatusName state)
        {
            // ReceivedMessages[message.DbId].StatusName = state;
            //  ReceivedMessages[message.DbId].ExpiresAt = message.ExpiresAt;
            //  ReceivedMessages[message.DbId].Content = _serializer.Serialize(message.Origin);
            return Task.CompletedTask;
        }

        public MediumMessage StoreMessage(string name, Message content, object dbTransaction = null)
        {
            var message = new MediumMessage
            {
                DbId = content.GetId(),
                Origin = content,
                Content = _serializer.Serialize(content),
                Added = DateTime.Now,
                ExpiresAt = null,
                Retries = 0
            };

            //PublishedMessages[message.DbId] = new MemoryMessage()
            //{
            //    DbId = message.DbId,
            //    Name = name,
            //    Content = message.Content,
            //    Retries = message.Retries,
            //    Added = message.Added,
            //    ExpiresAt = message.ExpiresAt,
            //    StatusName = StatusName.Scheduled
            //};

            return message;
        }

        public void StoreReceivedExceptionMessage(string name, string group, string content)
        {
            var id = SnowflakeId.Default().NextId().ToString();

            //ReceivedMessages[id] = new MemoryMessage
            //{
            //    DbId = id,
            //    Group = group,
            //    Origin = null,
            //    Name = name,
            //    Content = content,
            //    Retries = _capOptions.Value.FailedRetryCount,
            //    Added = DateTime.Now,
            //    ExpiresAt = DateTime.Now.AddDays(15),
            //    StatusName = StatusName.Failed
            //};
        }

        public MediumMessage StoreReceivedMessage(string name, string @group, Message message)
        {
            var mdMessage = new MediumMessage
            {
                DbId = SnowflakeId.Default().NextId().ToString(),
                Origin = message,
                Added = DateTime.Now,
                ExpiresAt = null,
                Retries = 0
            };

            //ReceivedMessages[mdMessage.DbId] = new MemoryMessage
            //{
            //    DbId = mdMessage.DbId,
            //    Origin = mdMessage.Origin,
            //    Group = group,
            //    Name = name,
            //    Content = _serializer.Serialize(mdMessage.Origin),
            //    Retries = mdMessage.Retries,
            //    Added = mdMessage.Added,
            //    ExpiresAt = mdMessage.ExpiresAt,
            //    StatusName = StatusName.Scheduled
            //};
            return mdMessage;
        }

        public Task<int> DeleteExpiresAsync(string table, DateTime timeout, int batchCount = 1000, CancellationToken token = default)
        {
            var removed = 0;
            if (table == nameof(PublishedMessages))
            {
                var ids = PublishedMessages.Values
                    .Where(x => x.ExpiresAt < timeout)
                    .Select(x => x.DbId)
                    .Take(batchCount);

                foreach (var id in ids)
                {
                    if (PublishedMessages.Remove(id))
                    {
                        removed++;
                    }
                }
            }
            else
            {
                var ids = ReceivedMessages.Values
                    .Where(x => x.ExpiresAt < timeout)
                    .Select(x => x.DbId)
                    .Take(batchCount);

                foreach (var id in ids)
                {
                    if (ReceivedMessages.Remove(id))
                    {
                        removed++;
                    }
                }
            }

            return Task.FromResult(removed);
        }

        public Task<IEnumerable<MediumMessage>> GetPublishedMessagesOfNeedRetry()
        {
            var ret = PublishedMessages.Values
                .Where(x => x.Retries < _capOptions.Value.FailedRetryCount
                            && x.Added < DateTime.Now.AddSeconds(-10))
                .Take(200)
                .Select(x => (MediumMessage)x);

            foreach (var message in ret)
            {
                message.Origin = _serializer.Deserialize(message.Content);
            }

            return Task.FromResult(ret);
        }

        public Task<IEnumerable<MediumMessage>> GetReceivedMessagesOfNeedRetry()
        {
            var ret = ReceivedMessages.Values
                 .Where(x => x.Retries < _capOptions.Value.FailedRetryCount
                             && x.Added < DateTime.Now.AddSeconds(-10))
                 .Take(200)
                 .Select(x => (MediumMessage)x);

            foreach (var message in ret)
            {
                message.Origin = _serializer.Deserialize(message.Content);
            }

            return Task.FromResult(ret);
        }

        public IMonitoringApi GetMonitoringApi()
        {
            return new InMemoryMonitoringApi();
        }
    }

    internal class InMemoryMonitoringApi : IMonitoringApi
    {
        public Task<MediumMessage> GetPublishedMessageAsync(long id)
        {
            return Task.FromResult((MediumMessage)NoStorageStorage.PublishedMessages.Values.First(x => x.DbId == id.ToString(CultureInfo.InvariantCulture)));
        }

        public Task<MediumMessage> GetReceivedMessageAsync(long id)
        {
            return Task.FromResult((MediumMessage)NoStorageStorage.ReceivedMessages.Values.First(x => x.DbId == id.ToString(CultureInfo.InvariantCulture)));
        }

        public StatisticsDto GetStatistics()
        {
            var stats = new StatisticsDto
            { 
            };
            return stats;
        }
       public PagedQueryResult<MessageDto> Messages(MessageQueryDto queryDto)
        {
            return new PagedQueryResult<MessageDto>();
        }
        public IDictionary<DateTime, int> HourlyFailedJobs(MessageType type)
        {
            return new Dictionary<DateTime, int>();
        }

        public IDictionary<DateTime, int> HourlySucceededJobs(MessageType type)
        {
            return new Dictionary<DateTime, int>();
        }


        public int PublishedFailedCount()
        {
            return 0;
        }

        public int PublishedSucceededCount()
        {
            return 0;
        }

        public int ReceivedFailedCount()
        {
            return 0;
        }

        public int ReceivedSucceededCount()
        {
            return 0;
        }
         
    }

    internal class NoStoreMessage : MediumMessage
    {
        public string Name { get; set; }

        public StatusName StatusName { get; set; }

        public string Group { get; set; }
    }
}
