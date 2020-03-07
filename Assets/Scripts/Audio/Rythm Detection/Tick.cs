
namespace SuperstarDJ.Audio.RythmDetection
{
    public struct Tick
    {
        public readonly int Id;
        //public readonly Beat parentBeat;
        public readonly double TickStartsAt;
        public readonly double TickEndsAt;
        public readonly int Measure;
        public readonly  int Beat;
        public readonly int Index
            ;
        //double _hitRangeStart;
        //double _hitRangeEnd;
        public Tick( int _id, double tickStartsAt, double tickEndsAt,int beatIndex, int measureIndex, int _index )
        {
            Id = _id;
            Measure = measureIndex;
            Beat = beatIndex;
            Index = _index;
            TickStartsAt = tickStartsAt;
            TickEndsAt = tickEndsAt;

            //_hitRangeStart = hitRangeStart;
            //_hitRangeEnd = hitRangeEnd;
        }

        public override string ToString()
        {
            return $"[{Measure}:{Beat}:{Index}] Id: {Id}  ---  Starts@{TickStartsAt}";
        }

        //public bool IsWithinHitRange( double sampleTimePosition )
        //{
        //    return sampleTimePosition >= _hitRangeStart
        //        && sampleTimePosition <= _hitRangeEnd;
        //}
    }
}
