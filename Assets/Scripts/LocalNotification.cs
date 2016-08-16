using UnityEngine;
using NotificationServices = UnityEngine.iOS.NotificationServices;
using NotificationType = UnityEngine.iOS.NotificationType;

public class LocalNotification : MonoBehaviour {
    
    public static void ScheduleNotificationForiOSWithMessage (string text, System.DateTime fireDate) {
        
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            
            UnityEngine.iOS.LocalNotification notification = new UnityEngine.iOS.LocalNotification ();
            notification.fireDate = fireDate;
            notification.alertAction = "Alert";
            notification.alertBody = text;
            notification.hasAction = false;

            NotificationServices.ScheduleLocalNotification (notification);

            #if UNITY_IOS
            NotificationServices.RegisterForNotifications(NotificationType.Alert | NotificationType.Badge | NotificationType.Sound);
            #endif
        }        
    }
}