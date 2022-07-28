using Forge.Application.Messages;
using Forge.MediatR.CQRS.Commands;
using Forge.MessageBroker.RabbitMQ;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Application.Commands;

internal class TestRabbitCommandHandler : ICommandHandler<TestRabbitCommand>
{
    private readonly IRabbitMqClient _rabbitSender;

    public TestRabbitCommandHandler(IRabbitMqClient rabbitSender)
    {
        _rabbitSender = rabbitSender;
    }

    public Task<Unit> Handle(TestRabbitCommand command, CancellationToken cancellationToken)
    {
        _rabbitSender.Send(new RabbitTestMessage()
        {
            Name = command.Name
        });
        return Unit.Task;
    }
}
