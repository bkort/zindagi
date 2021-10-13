using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Zindagi.Domain.Common.SystemEvents;
using Zindagi.SeedWork;

namespace Zindagi.Domain.Common.SystemEventsHandler
{
    public class SendEmailNotificationHandler : INotificationHandler<SendEmailNotification>
    {
        private readonly IMessaging _messaging;

        public SendEmailNotificationHandler(IMessaging messaging) => _messaging = messaging;

        public async Task Handle(SendEmailNotification notification, CancellationToken cancellationToken) =>
            await _messaging.SendEmail(notification.To, notification.Subject, notification.HtmlContent);
    }
}
