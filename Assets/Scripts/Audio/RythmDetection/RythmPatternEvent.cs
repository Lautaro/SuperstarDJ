using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperstarDJ.Audio.RythmDetection
{
    public class RythmPatternEvent
    {
        public RythmPatternEvents Event { get; set; }
        public RythmPosition Position { get; set; }
        internal SuccesState SuccesState { get; set; }
        public string TrackName { get; set; }



    }
}
