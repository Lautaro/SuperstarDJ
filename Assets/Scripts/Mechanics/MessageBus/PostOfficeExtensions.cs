using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace NewsBoardMessaging
{
    public static class PostOfficeExtensions
    {
        public static void SubscribeWithId( this GameObject gameObject, NewsTopics messageName, UnityAction<NewsEvent> listener )
        {

        }
    }
}
