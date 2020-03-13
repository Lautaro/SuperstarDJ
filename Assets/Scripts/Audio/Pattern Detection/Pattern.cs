using Sirenix.OdinInspector;
using SuperstarDJ.Audio.PositionTracking;
using System.Linq;
using UnityEngine;

namespace SuperstarDJ.Audio.PatternDetection
{
    [CreateAssetMenu ( menuName = "SUPERSTAR_DJ/RythmPattern" )]
    public class Pattern : SerializedScriptableObject
    {
        [BoxGroup ( ShowLabel = false )]
        [Title ( "NAME" )]
        [HideLabel]
        [ShowInInspector]
        public string PatternName { get; set; }

        [BoxGroup ( ShowLabel = false )]
        [Title ( "STEPS" )]
        [HideLabel]
        [ShowInInspector]
        [TableList (
            HideToolbar = true,
            AlwaysExpanded = true,
            IsReadOnly = true,
            NumberOfItemsPerPage = 64,
            ShowIndexLabels = true )]
        public PatternStep[] Steps { get; set; } = new PatternStep[64];
        public void ResetStepStatus()
        {
            for ( int i = 0; i < Steps.Length; i++ )
            {
                Steps[i].Status = PatternStepStatus.Waiting;
                Steps[i].StepWasHit = false;
            }
        }
        public PatternStepStatus[] StepStatuses
        {
            get
            {
                return Steps.Select ( s => s.Status ).ToArray ();
            }
        }

        internal void SetCurrentStepIndex( int index )
        {
            for ( int i = 0; i < Steps.Length; i++ )
            {
                Steps[i].IsCurrent = i == index;
            }
        }

        /// <summary>
        /// register hit for easier debugging in editor window
        /// </summary>
        internal void SetHitStepIndex( int index )
        {
            Steps[index].StepWasHit = true;
        }


        public PatternStepAction[] StepActions
        {
            get
            {
                return Steps.Select ( s => s.Action ).ToArray ();
            }
        }
        public void SetStatusOfStep( int index, PatternStepStatus Status )
        {
            Steps[index].Status = Status;
        }
    }
}
