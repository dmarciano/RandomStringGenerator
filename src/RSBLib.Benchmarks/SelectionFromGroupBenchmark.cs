using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Fare;
using SMC.Utilities.RSG;
using SMC.Utilities.RSG.Random;
using System;

namespace RSBLib.Benchmarks
{
    //[Config(typeof(Config))]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class SelectionFromGroupBenchmark
    {
        Generator generator;
        Generator cryptoGenerator;
        Xeger xeger;

        private class Config : ManualConfig
        {
            public Config()
            {
                Add(Job.LegacyJitX64);
                Add(Job.RyuJitX64);
            }
        }

        public SelectionFromGroupBenchmark()
        {
            var ticks = Environment.TickCount;
            generator = new Generator("#First,Fizzy,Fuzzy#(2,5)", new RandomGenerator(ticks));

            cryptoGenerator = new Generator("#First,Fizzy,Fuzzy#(2,5)", new CryptoRandomGenerator());

            var random = new Random(Environment.TickCount);
            xeger = new Xeger("^(First|Fizzy|Fuzzy){2,5}$", random);
        }

        [Benchmark]
        public string RSG_SELECT_RNG() => generator.GetString();

        [Benchmark]
        public string RSG_SELECT_CRNG() => cryptoGenerator.GetString();

        [Benchmark]
        public string XEGER_SELECT() => xeger.Generate();
    }
}
