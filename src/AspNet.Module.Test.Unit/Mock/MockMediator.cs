using System.Diagnostics.CodeAnalysis;
using MediatR;
using NSubstitute;

namespace AspNet.Module.Test.Unit.Mock;

public class MockMediator
{
    [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
    private readonly INotificationHandler<INotification> _notificationHandlerMock;

    public MockMediator()
    {
        _notificationHandlerMock = Substitute.For<INotificationHandler<INotification>>();
        _notificationHandlerMock.Handle(Arg.Any<INotification>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(x =>
            {
                Notifications.Add(x.Arg<INotification>());
                return Task.CompletedTask;
            });
        
        Value = Substitute.For<IMediator>();
        Value.Publish(Arg.Any<INotification>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(x =>
            {
                _notificationHandlerMock.Handle(x.Arg<INotification>(), x.Arg<CancellationToken>());
                return Task.CompletedTask;
            });
    }

    [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")]
    public List<INotification> Notifications { get; } = new();

    public IMediator Value { get; }
}