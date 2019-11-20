using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using SMC.Utilities.RSG;
using SMC.Utilities.RSG.Random;
using System;

namespace RSBLib.Benchmarks
{
    [Config(typeof(Config))]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class FunctionsBenchmark
    {
        private Generator generatorTime;
        private Generator generatorTimeForce;
        private Generator generatorTimeFormat;
        private Generator generatorTimeFormatForce;
        private Generator generatorGuid;
        private Generator generatorGuidForce;
        private Generator generatorGuidFormat;
        private Generator generatorGuidFormatForce;
        private Generator generatorUDFMethod;
        private Generator generatorUDFDelegate;

        private Generator mersenneGeneratorTime;
        private Generator mersenneGeneratorTimeForce;
        private Generator mersenneGeneratorTimeFormat;
        private Generator mersenneGeneratorTimeFormatForce;
        private Generator mersenneGeneratorGuid;
        private Generator mersenneGeneratorGuidForce;
        private Generator mersenneGeneratorGuidFormat;
        private Generator mersenneGeneratorGuidFormatForce;
        private Generator mersenneGeneratorUDFMethod;
        private Generator mersenneGeneratorUDFDelegate;

        private Generator cryptoGeneratorTime;
        private Generator cryptoGeneratorTimeForce;
        private Generator cryptoGeneratorTimeFormat;
        private Generator cryptoGeneratorTimeFormatForce;
        private Generator cryptoGeneratorGuid;
        private Generator cryptoGeneratorGuidForce;
        private Generator cryptoGeneratorGuidFormat;
        private Generator cryptoGeneratorGuidFormatForce;
        private Generator cryptoGeneratorUDFMethod;
        private Generator cryptoGeneratorUDFDelegate;

        private class Config : ManualConfig
        {
            public Config()
            {
                Add(Job.LegacyJitX64);
                Add(Job.LegacyJitX86);
                Add(Job.RyuJitX64);
            }
        }

        public FunctionsBenchmark()
        {
            var ticks = Environment.TickCount;

            generatorTime = new Generator("{T}", new RandomGenerator(ticks));
            generatorTimeForce = new Generator("{T?}", new RandomGenerator(ticks));
            generatorTimeFormat = new Generator("{T:MMMM dd, yyyy}", new RandomGenerator(ticks));
            generatorTimeFormatForce = new Generator("{T:MMMM dd, yyyy?}", new RandomGenerator(ticks));
            generatorGuid = new Generator("{G}", new RandomGenerator(ticks));
            generatorGuidForce = new Generator("{G?}", new RandomGenerator(ticks));
            generatorGuidFormat = new Generator("{G:D}", new RandomGenerator(ticks));
            generatorGuidFormatForce = new Generator("{G:D?}", new RandomGenerator(ticks));
            generatorUDFMethod = new Generator("{My}", new RandomGenerator(ticks));
            generatorUDFMethod.AddFunction("My", ReturnString);
            generatorUDFDelegate = new Generator("{My}", new RandomGenerator(ticks));
            generatorUDFDelegate.AddFunction("My", () => { return "55"; });

            mersenneGeneratorTime = new Generator("{T}", new MersenneTwister(ticks));
            mersenneGeneratorTimeForce = new Generator("{T?}", new MersenneTwister(ticks));
            mersenneGeneratorTimeFormat = new Generator("{T:MMMM dd, yyyy}", new MersenneTwister(ticks));
            mersenneGeneratorTimeFormatForce = new Generator("{T:MMMM dd, yyyy?}", new MersenneTwister(ticks));
            mersenneGeneratorGuid = new Generator("{G}", new MersenneTwister(ticks));
            mersenneGeneratorGuidForce = new Generator("{G?}", new MersenneTwister(ticks));
            mersenneGeneratorGuidFormat = new Generator("{G:D}", new MersenneTwister(ticks));
            mersenneGeneratorGuidFormatForce = new Generator("{G:D?}", new MersenneTwister(ticks));
            mersenneGeneratorUDFMethod = new Generator("{My}", new MersenneTwister(ticks));
            mersenneGeneratorUDFMethod.AddFunction("My", ReturnString);
            mersenneGeneratorUDFDelegate = new Generator("{My}", new MersenneTwister(ticks));
            mersenneGeneratorUDFDelegate.AddFunction("My", () => { return "55"; });

            cryptoGeneratorTime = new Generator("{T}", new CryptoRandomGenerator());
            cryptoGeneratorTimeForce = new Generator("{T?}", new CryptoRandomGenerator());
            cryptoGeneratorTimeFormat = new Generator("{T:MMMM dd, yyyy}", new CryptoRandomGenerator());
            cryptoGeneratorTimeFormatForce = new Generator("{T:MMMM dd, yyyy?}", new CryptoRandomGenerator());
            cryptoGeneratorGuid = new Generator("{G}", new CryptoRandomGenerator());
            cryptoGeneratorGuidForce = new Generator("{G?}", new CryptoRandomGenerator());
            cryptoGeneratorGuidFormat = new Generator("{G:D}", new CryptoRandomGenerator());
            cryptoGeneratorGuidFormatForce = new Generator("{G:D?}", new CryptoRandomGenerator());
            cryptoGeneratorUDFMethod = new Generator("{My}", new CryptoRandomGenerator());
            cryptoGeneratorUDFMethod.AddFunction("My", ReturnString);
            cryptoGeneratorUDFDelegate = new Generator("{My}", new CryptoRandomGenerator());
            cryptoGeneratorUDFDelegate.AddFunction("My", () => { return "55"; });
        }

        [Benchmark]
        public string TIME() => generatorTime.GetString();
        
        [Benchmark]
        public string TIME_FORCE() => generatorTimeForce.GetString();

        [Benchmark]
        public string TIME_FORMAT() => generatorTimeFormat.GetString();

        [Benchmark]
        public string TIME_FORMAT_FORCE() => generatorTimeFormatForce.GetString();

        [Benchmark]
        public string GUID() => generatorGuid.GetString();

        [Benchmark]
        public string GUID_FORCE() => generatorGuidForce.GetString();

        [Benchmark]
        public string GUID_FORMAT() => generatorGuidFormat.GetString();

        [Benchmark]
        public string GUID_FORMAT_FORCE() => generatorGuidFormatForce.GetString();

        [Benchmark]
        public string UDF_METHOD() => generatorUDFMethod.GetString();

        [Benchmark]
        public string UDF_DELEGATE() => generatorUDFDelegate.GetString();


        [Benchmark]
        public string MER_TIME() => mersenneGeneratorTime.GetString();

        [Benchmark]
        public string MER_TIME_FORCE() => mersenneGeneratorTimeForce.GetString();

        [Benchmark]
        public string MER_TIME_FORMAT() => mersenneGeneratorTimeFormat.GetString();

        [Benchmark]
        public string MER_TIME_FORMAT_FORCE() => mersenneGeneratorTimeFormatForce.GetString();

        [Benchmark]
        public string MER_GUID() => mersenneGeneratorGuid.GetString();

        [Benchmark]
        public string MER_GUID_FORCE() => mersenneGeneratorGuidForce.GetString();

        [Benchmark]
        public string MER_GUID_FORMAT() => mersenneGeneratorGuidFormat.GetString();

        [Benchmark]
        public string MER_GUID_FORMAT_FORCE() => mersenneGeneratorGuidFormatForce.GetString();

        [Benchmark]
        public string MER_UDF_METHOD() => mersenneGeneratorUDFMethod.GetString();

        [Benchmark]
        public string MER_UDF_DELEGATE() => mersenneGeneratorUDFDelegate.GetString();



        [Benchmark]
        public string CRYPTO_TIME() => cryptoGeneratorTime.GetString();

        [Benchmark]
        public string CRYPTO_TIME_FORCE() => cryptoGeneratorTimeForce.GetString();

        [Benchmark]
        public string CRYPTO_TIME_FORMAT() => cryptoGeneratorTimeFormat.GetString();

        [Benchmark]
        public string CRYPTO_TIME_FORMAT_FORCE() => cryptoGeneratorTimeFormatForce.GetString();

        [Benchmark]
        public string CRYPTO_GUID() => cryptoGeneratorGuid.GetString();

        [Benchmark]
        public string CRYPTO_GUID_FORCE() => cryptoGeneratorGuidForce.GetString();

        [Benchmark]
        public string CRYPTO_GUID_FORMAT() => cryptoGeneratorGuidFormat.GetString();

        [Benchmark]
        public string CRYPTO_GUID_FORMAT_FORCE() => cryptoGeneratorGuidFormatForce.GetString();

        [Benchmark]
        public string CRYPTO_UDF_METHOD() => cryptoGeneratorUDFMethod.GetString();

        [Benchmark]
        public string CRYPTO_UDF_DELEGATE() => cryptoGeneratorUDFDelegate.GetString();

        private string ReturnString() => "55";
    }
}