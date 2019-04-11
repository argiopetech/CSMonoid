using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    interface IMonoid<T, ContainedType>
    {
        ContainedType Val { get; set; }
        T mempty { get; }
        T mappend(T b);
    }

    class SumInt : IMonoid<SumInt, int>
    {
        public int Val { get; set; }

        public SumInt mempty => new SumInt() { Val = 0 };

        public SumInt mappend(SumInt b) =>
            new SumInt() { Val = Val + b.Val };
    }

    class ProductInt : IMonoid<ProductInt, int>
    {
        public int Val { get; set; }

        public ProductInt mempty => new ProductInt() { Val = 1 };

        public ProductInt mappend(ProductInt b) =>
            new ProductInt() { Val = Val * b.Val };
    }

    class StringMonoid : IMonoid<StringMonoid, string>
    {
        public string Val { get; set; }

        public StringMonoid mempty => new StringMonoid() { Val = "" };

        public StringMonoid mappend(StringMonoid b) =>
            new StringMonoid() { Val = Val + b.Val };
    }

    class Program
    {
        private static IMonoid<T, ContainedType> mconcat<T, ContainedType>(IEnumerable<IMonoid<T, ContainedType>> vals2)
            where T : IMonoid<T, ContainedType>, new()
        {
            var accum = new T().mempty;

            foreach (var v in vals2)
                accum = accum.mappend(new T() { Val = v.Val });

            return accum;
        }
        
        static void Main(string[] args)
        {
            var vals1 = new[]                { 1, 2 }.Select(i => new ProductInt() { Val = i });
            var vals2 = new[]                { 1, 2 }.Select(i => new SumInt()     { Val = i });
            var vals3 = new[] { "Hello, ", "World!" }.Select(s => new StringMonoid { Val = s });

            Console.WriteLine(mconcat(vals1).Val);
            Console.WriteLine(mconcat(vals2).Val);
            Console.WriteLine(mconcat(vals3).Val);
        }
    }
}
