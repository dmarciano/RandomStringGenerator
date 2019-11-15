using System;

namespace SMC.Utilities.RSG.Random
{
    public class RandomGenerator : System.Random, IRandom
    {
        public RandomGenerator() : base(Guid.NewGuid().GetHashCode()) { }
        public RandomGenerator(int seed) : base(seed) { }
    }
}