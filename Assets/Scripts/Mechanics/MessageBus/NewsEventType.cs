using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace NewsBoardMessaging
{
    public class NewsEventType { 
    
        public NewsTopics Topic;
        
        Dictionary<string, UnityAction<NewsEvent>> Subscribers = new Dictionary<string, UnityAction<NewsEvent>>();

        public NewsEventType(NewsTopics topic, string subscriberId, UnityAction<NewsEvent> subscriber)
        {
    
            Topic = topic;
            Subscribers.Add(subscriberId, subscriber);
            Debug.Log($"Added subscriber ID {subscriberId}. Total subscribers: {Subscribers.Count()}");
        }

        public void AddSubscriber(string subscriberId, UnityAction<NewsEvent> subscriber)
        {
            Subscribers.Add(subscriberId, subscriber);
            Debug.Log($"Added subscriber ID {subscriberId}. Total subscribers: {Subscribers.Count()}");
        }

        public bool CheckForId(string id)
        {
            return Subscribers.ContainsKey(id);
        }

        public bool CheckForSubscriber(UnityAction<NewsEvent> subscriber)
        {
            return Subscribers.ContainsValue(subscriber);
        }

        public void RemoveSubscriberByKey(string id)
        {
            Subscribers.Remove(id);
        }

        public void RemoveSubscriberBysubscriber(UnityAction<NewsEvent> subscriber)
        {
            foreach (var item in Subscribers.Where(kvp => kvp.Value == subscriber).ToList())
            {
                Subscribers.Remove(item.Key);
            }
        }

        public void PublishThis<T>(T message)
        {

            var callbackMessage = new NewsEvent()
            {
                Topic = Topic,
                Message = message
            };

            Subscribers.Values.ToList().ForEach(sub =>
            {
                sub.Invoke(callbackMessage);
            });
        }
    }
}
