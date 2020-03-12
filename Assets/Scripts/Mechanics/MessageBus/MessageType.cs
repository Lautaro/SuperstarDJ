﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace SuperstarDJ.MessageSystem
{
    public class MessageType { 
    
        public MessageTopics Topic;
        
        Dictionary<string, UnityAction<Message>> Subscribers = new Dictionary<string, UnityAction<Message>>();

        public MessageType(MessageTopics topic, string subscriberId, UnityAction<Message> subscriber)
        {
            Topic = topic;
            Subscribers.Add(subscriberId, subscriber);
            Debug.Log($"Added subscriber ID {subscriberId}. Total subscribers: {Subscribers.Count()}");
        }

        public void AddSubscriber(string subscriberId, UnityAction<Message> subscriber)
        {
            Subscribers.Add(subscriberId, subscriber);
            Debug.Log($"Added subscriber ID {subscriberId}. Total subscribers: {Subscribers.Count()}");
        }

        public bool CheckForId(string id)
        {
            return Subscribers.ContainsKey(id);
        }

        public bool CheckForSubscriber(UnityAction<Message> subscriber)
        {
            return Subscribers.ContainsValue(subscriber);
        }

        public void RemoveSubscriberByKey(string id)
        {
            Subscribers.Remove(id);
        }

        public void RemoveSubscriberBysubscriber(UnityAction<Message> subscriber)
        {
            foreach (var item in Subscribers.Where(kvp => kvp.Value == subscriber).ToList())
            {
                Subscribers.Remove(item.Key);
            }
        }

        public void PublishThis<T>(T message)
        {

            var callbackMessage = new Message()
            {
                Topic = Topic,
                Attachment = message
            };

            Subscribers.Values.ToList().ForEach(sub =>
            {
                sub.Invoke(callbackMessage);
            });
            Debug.Log ( $"*** Message posted: {callbackMessage.Topic.ToString()} to  {Subscribers.Count} subs" );
        }
    }
}
