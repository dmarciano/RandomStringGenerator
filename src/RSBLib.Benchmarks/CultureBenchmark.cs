
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using SMC.Utilities.RSG;
using SMC.Utilities.RSG.Random;
using System;
using System.Linq;

namespace RSBLib.Benchmarks
{
    [Config(typeof(Config))]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class CultureBenchmark
    {
         Generator generator;
        private Generator mersenneGenerator;
        Generator cryptoGenerator;

        private class Config : ManualConfig
        {
            public Config()
            {
                Add(Job.LegacyJitX64);
                Add(Job.LegacyJitX86);
                Add(Job.RyuJitX64);
            }
        }

        public CultureBenchmark()
        {
            var uppercase_Swedish = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ".ToList();
            var lowercase_Swedish = "abcdefghijklmnopqrstuvwxyzåäö".ToList();

            var pattern = "9a&sv&0a";
            var ticks = Environment.TickCount;

            generator = new Generator(pattern, new RandomGenerator(ticks));
            generator.AddCulture("sv", uppercase_Swedish, lowercase_Swedish);

            mersenneGenerator = new Generator(pattern, new MersenneTwister(ticks));
            mersenneGenerator.AddCulture("sv", uppercase_Swedish, lowercase_Swedish);

            cryptoGenerator = new Generator(pattern, new CryptoRandomGenerator());
            cryptoGenerator.AddCulture("sv", uppercase_Swedish, lowercase_Swedish);
        }

        [Benchmark]
        public string RSG_CULTURE_RNG() => generator.GetString();

        [Benchmark]
        public string RSG_CULTURE_MER() => mersenneGenerator.GetString();

        [Benchmark]
        public string RSG_CULTURE_CRNG() => cryptoGenerator.GetString();
    }
}
