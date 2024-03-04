using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationPresenter : MonoBehaviour
{

    public TextMeshProUGUI tmpro_topic;
    public TextMeshProUGUI tmpro_description;
    public Animator notificationDrawerAnimator;
    private List<Notification> pendingNotifications = new List<Notification>();


    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void addNotification(Notification notification)
    {
        if (pendingNotifications.Count >= 0)
            notification.dismissSeconds = 5;
        else
            notification.dismissSeconds = 1;
        pendingNotifications.Add(notification);
    }

    public void showNotification(string topic, string description)
    {
        notificationDrawerAnimator.Play("NotificationDrawerOpen");
        tmpro_topic.text = topic;
        tmpro_description.text = description;
    }

    public void closeNotification()
    {
        notificationDrawerAnimator.Play("NotificationDrawerClose");
        tmpro_topic.text = null;
        tmpro_description.text = null;
    }

    // Update is called once per frame
    void Update()
    {


        if (pendingNotifications.Count > 0)
        {
            var notify = pendingNotifications[0];
            notify.activationTimeCountdown += Time.deltaTime;
            pendingNotifications[0] = notify;

            if (notify.activationTimeCountdown >= (pendingNotifications.Count > 1 ? 1 : notify.dismissSeconds))
            {
                pendingNotifications.RemoveAt(0);
                closeNotification();
            }
            else
            {
                showNotification(notify.topic, notify.description);
            }
        }

        Debug.Log(pendingNotifications.Count + "|" + pendingNotifications);
    }
}
