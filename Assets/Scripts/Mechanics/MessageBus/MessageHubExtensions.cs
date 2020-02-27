using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace MessageSystem
{
    public static class MessageHubExtensions
    {
        public static void SubscribeWithId( this GameObject gameObject, MessageTopics messageName, UnityAction<Message> listener )
        {

        }
    }
}
