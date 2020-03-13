
namespace SuperstarDJ.Audio.PositionTracking
{
    public struct Tick
    {
        public readonly int Id;
        public readonly double TickStartsAt;
        public readonly double TickEndsAt;
        public readonly int Measure;
        public readonly int Beat;
        public readonly int Index;
        public Tick( int _id, double tickStartsAt, double tickEndsAt, int beatIndex, int measureIndex, int _index )
        {
            Id = _id;
            Measure = measureIndex;
            Beat = beatIndex;
            Index = _index;
            TickStartsAt = tickStartsAt;
            TickEndsAt = tickEndsAt;
        }

        public override string ToString()
        {
            return $"[{Measure}:{Beat}:{Index}] Id: {Id}  ---  Starts@{TickStartsAt}";
        }
    }
}
