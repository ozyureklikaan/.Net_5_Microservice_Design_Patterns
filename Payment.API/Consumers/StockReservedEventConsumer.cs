using MassTransit;
using Microsoft.Extensions.Logging;
using Shared;
using System;
using System.Threading.Tasks;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly ILogger<StockReservedEventConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(ILogger<StockReservedEventConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var balance = 3000M;

            if (balance > context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{ context.Message.Payment.TotalPrice } TL was withdraw from credit card for buyer id : { context.Message.BuyerId }");

                await _publishEndpoint.Publish(new PaymentCompletedEvent()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId
                });
            }
            else
            {
                _logger.LogInformation($"{ context.Message.Payment.TotalPrice } TL was not withdraw from credit card for user id : { context.Message.BuyerId }");

                await _publishEndpoint.Publish(new PaymentFailedEvent()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    Message = "Not enough balance",
                    OrderItems = context.Message.OrderItems
                });
            }
        }
    }
}
