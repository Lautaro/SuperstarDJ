﻿using SuperstarDJ.Audio.PositionTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperstarDJ.Audio
{
    public struct RythmPosition
    {
        public readonly Tick Tick;
        public readonly Double Position;

        public RythmPosition(Tick tick, double position)
        {   
            Tick = tick;
            Position = position;
        }

        public override string ToString()
        {
            return $"({Tick.Id.ToString("D2")})  [{Tick.Measure}][{Tick.Beat}][{Tick.Index}]";
        }
    }
}
