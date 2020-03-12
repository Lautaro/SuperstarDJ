using SuperstarDJ.MessageSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SuperstarDJ.UI
{
    public class FX_UI : MonoBehaviour
    {
        ParticleSystem[] Fx;
        // Start is called before the first frame update
        void Start()
        {
            Fx = GetComponentsInChildren<ParticleSystem> ();
            MessageHub.Subscribe ( MessageTopics.DisplayUI_FX_string, DisplayFx );
        }

        public void DisplayFx( Message displayFxMessage )
        {
            var fxName = displayFxMessage.Open<string> ();
            PlayFx ( fxName );
        }

        void PlayFx( string name )
        {
            var fx = Fx.FirstOrDefault ( f => f.name == name );
            if ( fx == null )
            {
                Debug.LogError ( $"UI FX not found! There is no UI FX called {name}" );
                return;
            }

            fx.Play ();

        }
    }
}