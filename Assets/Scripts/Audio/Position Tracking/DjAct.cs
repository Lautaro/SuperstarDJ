
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperstarDJ.Audio.PositionTracking
{
    class DjAct
    {
        internal RythmPosition ActualPosition;
        internal Step? StepThatWastHit; // null if missed 
    }
}
