﻿using BenchmarkDotNet;
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
    public class SocialSecurityNumbersBenchmark
    {
        Generator generator;
        Generator cryptoGenerator;
        Generator mersenneGenerator;
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

        public SocialSecurityNumbersBenchmark()
        {
            var pattern = "0(3)[-]0(2)[-]0(4)";
            var ticks = Environment.TickCount;

            generator = new Generator(pattern, new RandomGenerator(ticks));
            mersenneGenerator = new Generator(pattern, new MersenneTwister(ticks));
            cryptoGenerator = new Generator(pattern, new CryptoRandomGenerator());

            var random = new Random(Environment.TickCount);
            xeger = new Xeger("^\\d{3}-\\d{2}-\\d{4}$", random);
        }

        [Benchmark]
        public string RSG_SSN_RNG() => generator.GetString();

        [Benchmark]
        public string RSG_SSN_MER() => mersenneGenerator.GetString();

        [Benchmark]
        public string RSG_SSN_CRNG() => cryptoGenerator.GetString();

        [Benchmark]
        public string XEGER_SSN() => xeger.Generate();
    }
}
