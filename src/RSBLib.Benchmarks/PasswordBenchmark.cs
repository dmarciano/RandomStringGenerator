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
    [Config(typeof(Config))]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class PasswordBenchmark
    {
         Generator generator;
         Generator mersenneGenerator;
         Generator cryptoGenerator;
         Xeger xeger;

        private class Config : ManualConfig
        {
            public Config()
            {
                Add(Job.LegacyJitX64);
                Add(Job.LegacyJitX86);
                Add(Job.RyuJitX64);
            }
        }

        public PasswordBenchmark()
        {
            var pattern = "a*(11,15)";
            var ticks = Environment.TickCount;

            generator = new Generator(pattern, new RandomGenerator(ticks));
            mersenneGenerator = new Generator(pattern, new MersenneTwister(ticks));
            cryptoGenerator = new Generator(pattern, new CryptoRandomGenerator());

            var random = new Random(Environment.TickCount);
            xeger = new Xeger("^[a-zA-Z0-9][!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~]{11,15}", random);
        }

        [Benchmark]
        public string RSG_PASSWORD_RNG() => generator.GetString();

        [Benchmark]
        public string RSG_PASSWORD_MER() => mersenneGenerator.GetString();

        [Benchmark]
        public string RSG_PASSWORD_CRNG() => cryptoGenerator.GetString();

        [Benchmark]
        public string XEGER_PASSWORD() => xeger.Generate();
    }
}
