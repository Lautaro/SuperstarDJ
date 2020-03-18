
namespace SuperstarDJ.Audio.PositionTracking
{
    public struct Step
    {
        public readonly int Id;
        public readonly double StepStartsAt;
        public readonly double StepEndsAt;
        public readonly int Measure;
        public readonly int Beat;
        public readonly int Index;
        public Step( int _id, double stepStartsAt, double stepEndsAt, int beatIndex, int measureIndex, int _index )
        {
            Id = _id;
            Measure = measureIndex;
            Beat = beatIndex;
            Index = _index;
            StepStartsAt = stepStartsAt;
            StepEndsAt = stepEndsAt;
        }

        public override string ToString()
        {
            return $"[{Measure}:{Beat}:{Index}] Id: {Id}  ---  Starts@{StepStartsAt}";
        }
    }
}
