using System;

namespace CoreHal.Reader
{
    public class ProblemException : Exception
    {
        public Problem Problem { get; set; }

        public ProblemException(Problem problem)
        {
            Problem = problem;
        }
    }
}