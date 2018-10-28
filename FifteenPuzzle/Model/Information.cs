namespace FifteenPuzzle.Data
{
    public class Information
    {
        public int SolutionLength { get; }
        public int StatesVisited { get; }
        public int StatesProcessed { get; }
        public int DeepestLevelReached { get; }
        public int ProcessingTime { get; }

        public Information(int solutionLength, int statesVisited, int statesProcessed, int deepestLevelReached,
            int processingTime)
        {
            SolutionLength = solutionLength;
            StatesVisited = statesVisited;
            StatesProcessed = statesProcessed;
            DeepestLevelReached = deepestLevelReached;
            ProcessingTime = processingTime;
        }
    }
}