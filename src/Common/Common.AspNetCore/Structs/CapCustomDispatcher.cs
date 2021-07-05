using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Persistence;
using DotNetCore.CAP.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options; 

namespace Common.AspNetCore
{
    public class CapCustomDispatcher : IDispatcher, IDisposable
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly IMessageSender _sender;
        private readonly ISubscribeDispatcher _executor;
        private readonly ILogger<CapCustomDispatcher> _logger;
        private readonly string[] _group;
        private readonly string[] _expp;

        private readonly Channel<MediumMessage> _publishedChannel;
        private readonly Channel<MediumMessage> _publishedChannel2;
        private readonly Channel<(MediumMessage, ConsumerExecutorDescriptor)> _receivedChannel;
        private readonly Channel<(MediumMessage, ConsumerExecutorDescriptor)> _receivedChannel2;

        public CapCustomDispatcher(ILogger<CapCustomDispatcher> logger,
            IMessageSender sender, IConfiguration configuration,
            IOptions<CapOptions> options,
            ISubscribeDispatcher executor)
        {
            _logger = logger;
            _sender = sender;
            _executor = executor;

            var eg = configuration["MQ:ExtraGroup"];
            var expp = configuration["MQ:ExtraName"];
            _group = !string.IsNullOrEmpty(eg) ? eg.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) : null;
            _expp = !string.IsNullOrEmpty(expp) ? expp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) : null;

            _publishedChannel = Channel.CreateUnbounded<MediumMessage>(new UnboundedChannelOptions() { SingleReader = false, SingleWriter = true });
            if (_expp != null && _expp.Any())
                _publishedChannel2 = Channel.CreateUnbounded<MediumMessage>(new UnboundedChannelOptions() { SingleReader = false, SingleWriter = true });
            _receivedChannel = Channel.CreateUnbounded<(MediumMessage, ConsumerExecutorDescriptor)>();
            if (_group != null && _group.Any())
                _receivedChannel2 = Channel.CreateUnbounded<(MediumMessage, ConsumerExecutorDescriptor)>();

            Task.WhenAll(Enumerable.Range(0, options.Value.ProducerThreadCount)
                .Select(_ => Task.Factory.StartNew(Sending, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default)).ToArray());
            if (_expp != null && _expp.Any())
                Task.WhenAll(Enumerable.Range(0, options.Value.ProducerThreadCount)
                      .Select(_ => Task.Factory.StartNew(Sending2, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default)).ToArray());

            Task.WhenAll(Enumerable.Range(0, options.Value.ConsumerThreadCount)
                .Select(_ => Task.Factory.StartNew(Processing, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default)).ToArray());
            if (_group != null && _group.Any())
                Task.WhenAll(Enumerable.Range(0, options.Value.ConsumerThreadCount)
                    .Select(_ => Task.Factory.StartNew(Processing2, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default)).ToArray());
        }

        public void EnqueueToPublish(MediumMessage message)
        {
            var pp = message.Origin.GetName();
            if (!string.IsNullOrEmpty(pp) && _expp != null && _expp.Any(t => string.Compare(t, pp, true) == 0))
                _publishedChannel2.Writer.TryWrite(message);
            else
                _publishedChannel.Writer.TryWrite(message);
        }

        public void EnqueueToExecute(MediumMessage message, ConsumerExecutorDescriptor descriptor)
        {
            var pp = message.Origin.GetGroup();
            if (!string.IsNullOrEmpty(pp) && _group != null && _group.Any(t => pp.StartsWith(t,StringComparison.OrdinalIgnoreCase)))
                _receivedChannel2.Writer.TryWrite((message, descriptor));
            else
                _receivedChannel.Writer.TryWrite((message, descriptor));
        }

        public void Dispose()
        {
            _cts.Cancel();
        }

        private async Task Sending()
        {
            try
            {
                while (await _publishedChannel.Reader.WaitToReadAsync(_cts.Token))
                {
                    while (_publishedChannel.Reader.TryRead(out var message))
                    {
                        try
                        {
                            var result = await _sender.SendAsync(message);
                            if (!result.Succeeded)
                            {
                                _logger.MessagePublishException(message.Origin.GetId(), result.ToString(),
                                    result.Exception);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex,
                                $"An exception occurred when sending a message to the MQ. Id:{message.DbId}");
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // expected
            }
        }

        private async Task Sending2()
        {
            try
            {
                while (await _publishedChannel2.Reader.WaitToReadAsync(_cts.Token))
                {
                    while (_publishedChannel2.Reader.TryRead(out var message))
                    {
                        try
                        {
                            var result = await _sender.SendAsync(message);
                            if (!result.Succeeded)
                            {
                                _logger.MessagePublishException(message.Origin.GetId(), result.ToString(),
                                    result.Exception);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex,
                                $"An exception occurred when sending a message to the MQ. Id:{message.DbId}");
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // expected
            }
        }


        private async Task Processing()
        {
            try
            {
                while (await _receivedChannel.Reader.WaitToReadAsync(_cts.Token))
                {
                    while (_receivedChannel.Reader.TryRead(out var message))
                    {
                        await _executor.DispatchAsync(message.Item1, message.Item2, _cts.Token);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // expected
            }
        }

        private async Task Processing2()
        {
            try
            {
                while (await _receivedChannel2.Reader.WaitToReadAsync(_cts.Token))
                {
                    while (_receivedChannel2.Reader.TryRead(out var message))
                    {
                        await _executor.DispatchAsync(message.Item1, message.Item2, _cts.Token);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // expected
            }
        }
    }
}
