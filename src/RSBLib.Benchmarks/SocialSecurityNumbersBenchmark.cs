using BenchmarkDotNet.Attributes;
using SMC.Utilities.RSG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSBLib.Benchmarks
{
    public class SocialSecurityNumbersBenchmark
    {
        Generator generator;

        public SocialSecurityNumbersBenchmark()
        {
            generator = new Generator("0(3)[-]0(2)[-]0(4)");
        }

        [Benchmark]
        public string SSN() => generator.GetString();
    }
}
