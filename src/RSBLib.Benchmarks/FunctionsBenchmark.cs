using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Fare;
using SMC.Utilities.RSG;
using System;

namespace RSBLib.Benchmarks
{
    //[Config(typeof(Config))]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class FunctionsBenchmark
    {
        private Generator generator;
        private Generator cryptoGenerator;

        private class Config : ManualConfig
        {
            public Config()
            {
                Add(Job.LegacyJitX64);
                Add(Job.RyuJitX64);
            }
        }

        public FunctionsBenchmark()
        {
            var ticks = Environment.TickCount;
            generator = new Generator(new RandomGenerator(ticks));

            cryptoGenerator = new Generator(new CryptoRandomGenerator());
        }

        [Benchmark]
        public string TIME()
        {
            generator.SetPattern("{T}");
            return generator.GetString();
        }

        [Benchmark]
        public string TIME_FORCE()
        {
            generator.SetPattern("{T?}");
            return generator.GetString();
        }

        [Benchmark]
        public string TIME_FORMAT()
        {
            generator.SetPattern("{T:MMMM dd, yyyy}");
            return generator.GetString();
        }

        [Benchmark]
        public string TIME_FORMAT_FORCE()
        {
            generator.SetPattern("{T:MMMM dd, yyyy?}");
            return generator.GetString();
        }

        [Benchmark]
        public string GUID()
        {
            generator.SetPattern("{G}");
            return generator.GetString();
        }

        [Benchmark]
        public string GUID_FORMAT()
        {
            generator.SetPattern("{G:D}");
            return generator.GetString();
        }

        [Benchmark]
        public string GUID_FORCE()
        {
            generator.SetPattern("{G?}");
            return generator.GetString();
        }

        [Benchmark]
        public string GUID_FORMAT_FORCE()
        {
            generator.SetPattern("{G:D?}");
            return generator.GetString();
        }



        [Benchmark]
        public string CRYPTO_TIME()
        {
            generator.SetPattern("{T}");
            return generator.GetString();
        }

        [Benchmark]
        public string CRYPTO_TIME_FORCE()
        {
            generator.SetPattern("{T?}");
            return generator.GetString();
        }

        [Benchmark]
        public string CRYPTO_TIME_FORMAT()
        {
            generator.SetPattern("{T:MMMM dd, yyyy}");
            return generator.GetString();
        }

        [Benchmark]
        public string CRYPTO_TIME_FORMAT_FORCE()
        {
            generator.SetPattern("{T:MMMM dd, yyyy?}");
            return generator.GetString();
        }

        [Benchmark]
        public string CRYPTO_GUID()
        {
            generator.SetPattern("{G}");
            return generator.GetString();
        }

        [Benchmark]
        public string CRYPTO_GUID_FORCE()
        {
            generator.SetPattern("{G?}");
            return generator.GetString();
        }

        [Benchmark]
        public string CRYPTO_GUID_FORMAT()
        {
            generator.SetPattern("{G:D}");
            return generator.GetString();
        }

        [Benchmark]
        public string CRYPTO_GUID_FORMAT_FORCE()
        {
            generator.SetPattern("{G:D?}");
            return generator.GetString();
        }
    }
}
