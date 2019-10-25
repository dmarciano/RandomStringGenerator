using System;

namespace SMC.Utilities.RSG
{
    public class RandomGenerator : Random, IRandom
    {
        public RandomGenerator() : base(Guid.NewGuid().GetHashCode()) { }
        public RandomGenerator(int seed) : base(seed) { }
    }
}