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
    public class CreateAddressBenchmark
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

        public CreateAddressBenchmark()
        {
            var ticks = Environment.TickCount;
            generator = new Generator("9(1,5)[ ]#Main,1st,8th#[ ]#Street,Avenue,Court#[, ]#Brooklyn,Newark#[, ]#NY,NJ#[ ]0(5)", new RandomGenerator(ticks));

            cryptoGenerator = new Generator("9(1,5)[ ]#Main,1st,8th#[ ]#Street,Avenue,Court#[, ]#Brooklyn,Newark#[, ]#NY,NJ#[ ]0(5)", new CryptoRandomGenerator());

            var random = new Random(Environment.TickCount);
            xeger = new Xeger("^[1-9]{1,5} (Main|1st|8th) (Street|Avenue|Court), (Brooklyn|Newark), (NY|NJ) \\d{5}$", random);
        }

        [Benchmark]
        public string RSG_SSN_RNG() => generator.GetString();

        [Benchmark]
        public string RSG_SSN_CRNG() => cryptoGenerator.GetString();

        [Benchmark]
        public string XEGER_SSN() => xeger.Generate();
    }
}
