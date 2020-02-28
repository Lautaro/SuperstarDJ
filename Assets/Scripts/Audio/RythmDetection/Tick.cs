
namespace SuperstarDJ.Audio.RythmDetection
    {
    public struct Tick
    {
        public int index;
        public readonly Beat parentBeat;
        double startsAt;
        internal double endsAt;
        public Tick( int _index, Beat _parentBeat, double ticksDuration, double parentStartsAt )
        {
            index = _index;
            startsAt = ticksDuration * ( index - 1 ) + parentStartsAt;
            endsAt = ( ticksDuration * index ) + parentStartsAt;
            parentBeat = _parentBeat;
        }

        public bool  HasPosition( double sampleTimePosition)
        {  
            bool checkStartPosition = sampleTimePosition >= startsAt;
            bool checkEndPosition =  sampleTimePosition  <= endsAt;
            return checkStartPosition && checkEndPosition;
        }
    }
}
