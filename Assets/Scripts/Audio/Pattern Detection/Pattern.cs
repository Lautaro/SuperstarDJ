using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace SuperstarDJ.Audio.PatternDetection
{
    [CreateAssetMenu ( menuName = "SUPERSTAR_DJ/RythmPattern" )]
    public class Pattern : SerializedScriptableObject
    {
        [ShowInInspector]
        public string PatternName { get; set; }

        [ShowInInspector]
        [ListDrawerSettings ( IsReadOnly = true, ShowItemCount = true, ShowIndexLabels = true, NumberOfItemsPerPage = 32, OnBeginListElementGUI = "OnBeginListElementGUI", OnEndListElementGUI = "OnAfterEnumisGUI" )]
        [EnumToggleButtons]
        [LabelWidth ( 50 )]
        public PatternAction[] PatternStates { get; set; } = new PatternAction[64];

        private void OnBeginListElementGUI( int index )
        {
            Sirenix.Utilities.Editor.GUIHelper.PushContentColor ( Color.red,true );

            if ( ( index % 4 ) == 0 )
                Sirenix.Utilities.Editor.GUIHelper.PushColor ( Color.cyan, true );

            if ( ( index % 16 ) == 0 )
                Sirenix.Utilities.Editor.GUIHelper.PushColor ( Color.magenta, true );

            if ( index == DateTime.Now.Second )
            {
                Sirenix.Utilities.Editor.GUIHelper.PushColor ( Color.yellow, true );
            }

        }

        private void OnAfterEnumisGUI( int index )
        {
            if ( ( index % 4 ) == 0 )
                Sirenix.Utilities.Editor.GUIHelper.PopColor ();

            if ( ( index % 16 ) == 0 )
                Sirenix.Utilities.Editor.GUIHelper.PopColor ();

            if ( index == DateTime.Now.Second )
            {
                Sirenix.Utilities.Editor.GUIHelper.PopColor ();
           
            }
            Sirenix.Utilities.Editor.GUIHelper.PopContentColor ();

        }
    }
}