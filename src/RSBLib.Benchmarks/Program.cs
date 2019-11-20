using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSBLib.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var ssn = BenchmarkRunner.Run<SocialSecurityNumbersBenchmark>();
            //var selection = BenchmarkRunner.Run<SelectionFromGroupBenchmark>();
            var functions = BenchmarkRunner.Run<FunctionsBenchmark>();
            //var address = BenchmarkRunner.Run<CreateAddressBenchmark>();
            var culture = BenchmarkRunner.Run<CultureBenchmark>();
            //var password = BenchmarkRunner.Run<PasswordBenchmark>();
        }
    }
}