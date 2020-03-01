using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperstarDJ.Audio.RythmDetection
{
    public class RythmPattern
    {
        public string Name { get; set; }

        List<RythmPatternEvent> Events;
    }
}
