
namespace SuperstarDJ.Audio.RythmDetection
{
    public struct Tick
    {
        public readonly int Index;
        //public readonly Beat parentBeat;
        public readonly double TickStartsAt;
        public readonly int Measure;
        public readonly  int Beat; 
        //double _hitRangeStart;
        //double _hitRangeEnd;
        public Tick( int _index, double tickStartsAt,int beatIndex, int measureIndex )
        {
            Index = _index;
            Measure = measureIndex;
            Beat = beatIndex;
            TickStartsAt = tickStartsAt;

            //_hitRangeStart = hitRangeStart;
            //_hitRangeEnd = hitRangeEnd;
        }

        //public bool IsWithinHitRange( double sampleTimePosition )
        //{
        //    return sampleTimePosition >= _hitRangeStart
        //        && sampleTimePosition <= _hitRangeEnd;
        //}
    }
}
