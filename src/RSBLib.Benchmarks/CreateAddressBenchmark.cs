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
    [Config(typeof(Config))]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class CreateAddressBenchmark
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

        public CreateAddressBenchmark()
        {
            var pattern = "9(1,5)[ ]#Main,1st,8th#[ ]#Street,Avenue,Court#[, ]#Brooklyn,Newark#[, ]#NY,NJ#[ ]0(5)";
            var ticks = Environment.TickCount;

            generator = new Generator(pattern, new RandomGenerator(ticks));
            mersenneGenerator = new Generator(pattern, new MersenneTwister(ticks));
            cryptoGenerator = new Generator(pattern, new CryptoRandomGenerator());

            var random = new Random(Environment.TickCount);
            xeger = new Xeger("^[1-9]{1,5} (Main|1st|8th) (Street|Avenue|Court), (Brooklyn|Newark), (NY|NJ) \\d{5}$", random);
        }

        [Benchmark]
        public string RSG_ADDRESS_RNG() => generator.GetString();

        [Benchmark]
        public string RSG_ADDRESS_MER() => mersenneGenerator.GetString();

        [Benchmark]
        public string RSG_ADDRESS_CRNG() => cryptoGenerator.GetString();

        [Benchmark]
        public string XEGER_ADDRESS() => xeger.Generate();
    }
}
