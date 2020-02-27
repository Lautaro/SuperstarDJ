
namespace SuperstarDJ.Audio.RythmDetection
    {
    public struct Tick
    {
        static int nextId = 0;
        public int positionInBeat;
        public readonly Beat parentBeat;
        double startsAt;
        internal double endsAt;
        public int Id;
        public Tick( int _index, Beat _parentBeat, double ticksDuration, double parentStartsAt )
        {
            Id = nextId++;
            positionInBeat = _index;
            startsAt = ticksDuration * ( positionInBeat - 1 ) + parentStartsAt;
            endsAt = ( ticksDuration * positionInBeat ) + parentStartsAt;
            parentBeat = _parentBeat;
        }

        public bool  HasPosition( double sampleTimePosition )
        {
            return sampleTimePosition >= startsAt && sampleTimePosition <= endsAt;
        }
    }
}
