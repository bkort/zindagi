﻿using MediatR;

namespace Zindagi.Domain.Common.SystemEvents
{
    public class SendTextNotification : INotification
    {
        public string MobileNumber { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
