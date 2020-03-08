using MessageSystem;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperstarDJ.Audio.RythmDetection
{
    public class PatternAnalyzer
    {
        List<RythmPattern> patterns = new List<RythmPattern> ();
        Dictionary<string, SuccesState[]> successTrackers = new Dictionary<string, SuccesState[]>();

        public void Initialise(List<RythmPattern> patterns_)
        {
            patterns = patterns_;
            successTrackers.Clear ();

            foreach ( var pattern in patterns )
            {
                successTrackers[pattern.PatternName] = new SuccesState[64];                
            }

            MessageHub.Subscribe ( MessageTopics.NewRythmPosition, OnNewRythmPosition );
        }

        public void OnNewRythmPosition( Message rythmPositionMessage )
        {
            var newPosition = rythmPositionMessage.Open<RythmPosition> ();

        }

        public void SetSuccessForStep (string patternName, int index, SuccesState success )
        {
            successTrackers[patternName][index] = success; 
        }
        public void ClearAllSuccessTrackers()
        {
            foreach ( var dic in successTrackers )
            {
                dic.Value.ForEach ( ( ss ) => ss = SuccesState.Waiting );
            }
        }

        public bool IsPatternSuccessfull( string patternName )
        {
            return successTrackers[patternName].Any ( ss => ss != SuccesState.Sucess ); 

        }
    }
}
