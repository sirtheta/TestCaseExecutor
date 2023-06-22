using Notifications.Wpf.Core;
using System;
using TestCaseExecutor.Common;

namespace TestCaseExecutor.ViewModels
{
    internal abstract class BaseViewModel : Notify
    {
        internal static void ShowNotification(string titel, string message, NotificationType type)
        {
            var notificationManager = new NotificationManager();
            notificationManager.ShowAsync(new NotificationContent { Title = titel, Message = message, Type = type },
                    areaName: "WindowArea", expirationTime: new TimeSpan(0, 0, 2));
        }
    }
}

