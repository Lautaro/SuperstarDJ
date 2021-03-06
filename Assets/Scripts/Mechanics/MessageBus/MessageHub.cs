﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;


namespace SuperstarDJ.MessageSystem
{
    public class MessageHub
    {
        static List<MessageType> messageRegistrations = new List<MessageType> ();
        static public bool DebugEnabled = false;

        public static void SubscribeWithId( MessageTopics messageName, UnityAction<Message> subscriber, string id )
        {
            // Make sure this id does not subsribe to this message alreaedy
            if ( messageRegistrations.Any ( s => s.Topic == messageName && s.CheckForSubscriber ( subscriber ) ) )
            {
                Debug.LogWarning ( $"subscriber is trying to subsribe to a topic that its already subscribed to! Subscriber ID :{id} Message name :{messageName}  " );
                return;
            }

            MessageType messageType = messageRegistrations.FirstOrDefault ( s => s.Topic == messageName );

            if ( messageType == null )
            {
                messageType = new MessageType ( messageName, id, subscriber );
                messageRegistrations.Add ( messageType );
                return;
            }

            messageType.AddSubscriber ( id, subscriber );

        }

        public static void Subscribe( MessageTopics messageName, UnityAction<Message> subscriber )
        {
            // Check if action is anonymous 
            if ( subscriber.Method.GetCustomAttributes ( typeof ( CompilerGeneratedAttribute ), false ).Any () )
            {
                Debug.LogError ( "An anonymous method is trying to register a message bus subscription without providing an ID. This is not allowed since it would be impossible to cancel subscription. Get your shit together!" );
            };

            SubscribeWithId ( messageName, subscriber, "" );

        }

        public static void UnSubscribe( MessageTopics messageName, UnityAction<Message> subscriber )
        {
            var subscription = messageRegistrations.FirstOrDefault ( s => s.Topic == messageName );
            subscription.RemoveSubscriberBysubscriber ( subscriber );
        }

        public static void UnSubscribeById( MessageTopics messageName, string id )
        {
            messageRegistrations.Where ( mr => mr.Topic == messageName ).ToList ()
                .ForEach ( s => s.RemoveSubscriberByKey ( id ) );
        }

        public static void PublishNews<T>( MessageTopics topic, T message )
        {
            var registration = messageRegistrations.FirstOrDefault ( s => s.Topic == topic );
            if ( registration == null )
            {
                if ( !GameSettings.Instance.MutedTopics.HasFlag ( topic ) )
                {
                    Debug.LogWarning ( $"Message has been posted but no one is listening! MessageName{topic}" );
                    return;
                }
            }


            registration.PublishThis<T> ( message );
        }
    }
}