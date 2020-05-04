using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace SuperstarDJ.Audio.PatternDetection
{
    [Serializable]
    public struct PatternStep
    {
    
        internal bool StepWasHit;
        internal RythmPosition HitAtPosition;
        internal bool IsCurrent;

        [TableColumnWidth ( 200, false )]
        [EnumToggleButtons]
        public PatternStepAction Action;

        [TableColumnWidth ( 50, false )]
        [DisplayAsString]
        public PatternStepStatus Status;

        [TableColumnWidth ( 200, false )]
        [ShowInInspector]
        public string HitAt { get {
                if ( StepWasHit)
                {
                    return $"{HitAtPosition.ToString()} - Raw:{HitAtPosition.RawPosition} ({HitAtPosition.RawPosition})";
                }
                return "";
            }  }

    }
}
