using SuperstarDJ.Audio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuperstarDJ.UnityTools.Extensions
{
    public static class Extensions
    {
        public static void DebugLog( this object obj , string log)
        {
            Debug.Log ( log );;
        }

        public static void DebugWarning( this object obj, string log )
        {
            Debug.LogWarning ( log ); ;
        }

        public static void DebugError( this object obj, string log )
        {
            var x = obj.ToString ();
            Debug.LogError ( log + x ) ;
        }
    }
}
