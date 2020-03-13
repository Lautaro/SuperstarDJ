using Sirenix.OdinInspector;
using SuperstarDJ.Audio.PositionTracking;
using System;
using UnityEngine;

namespace SuperstarDJ.Audio.PatternDetection
{
    [Serializable]
    public struct PatternStep
    {
        [TableColumnWidth ( 50, false )]
        [ReadOnly]
        [HorizontalGroup ( " " )]
        [HideLabel]
        [ShowInInspector]
        public Color StateColor
        {
            get
            {
                if ( !Application.isPlaying || RythmManager.IsAllTracksStopped () ) return Color.gray;

                switch ( Status )
                {
                    case PatternStepStatus.Sucess:
                        return Color.green;
                    case PatternStepStatus.Failed:
                        return Color.red;
                    default:
                        return Color.gray;
                }
            }
        }

        [TableColumnWidth ( 50, false )]
        [ReadOnly]
        [HorizontalGroup ( " " )]
        [HideLabel]
        [ShowInInspector]
        public Color PlayerHitTrackerColor
        {
            get
            {
                return ( !Application.isPlaying || !StepWasHit || RythmManager.IsAllTracksStopped () ) ? Color.grey : Color.white;
            }
        }


        internal bool StepWasHit;

        internal bool IsCurrent;

        [TableColumnWidth ( 200, false )]
        [EnumToggleButtons]
        public PatternStepAction Action;

        [TableColumnWidth ( 50, false )]
        [DisplayAsString]
        public PatternStepStatus Status;

    }
}
