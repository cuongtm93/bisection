using System;
using System.Collections.Generic;
using System.Text;

namespace EqSolve
{
    public static class ZeroExtension
    {
        public static void Print(this double? zero)
        {
            if (zero.HasValue)
                Console.WriteLine("+nghiệm x= " + zero);
            else
                Console.WriteLine("Vô nghiệm");
        }
    }
    public class Interval
    {
        public double Begin;
        public double End;
        public Interval(double begin, double end)
        {
            this.Begin = begin;
            this.End = end;
        }

        public double MidPoint()
        {
            return (Begin + End) / 2;
        }
    }
    public class Term
    {
        public Term(double coefficent, double power)
        {
            this.coefficent = coefficent;
            this.power = power;
        }
        public double coefficent;
        public double power;
    }
    public class Polynomial : List<Term>
    {
        public double f(double x)
        {
            double value = 0;
            foreach (Term term in this)
                value += term.coefficent * Math.Pow(x, term.power);

            return value;
        }

        bool f_has_a_zero_in(Interval _range)
        {
            var f_a = f(_range.Begin);
            var f_b = f(_range.End);
            return (f_a > 0 && f_b < 0) || (f_a < 0 & f_b > 0)
                || f_a == 0 || f_b == 0;
        }

        public double? Solve(Interval range, double epsilon)
        {
            var workingRange = range;
            var counter = 0;

            Interval Divide2(Interval _range)
            {
                var midPoint = _range.MidPoint();
                var range1 = new Interval(_range.Begin, midPoint);
                var range2 = new Interval(midPoint, _range.End);

                if (f_has_a_zero_in(range1))
                    return range1;
                else
                    return range2;
            }

            double error() => Math.Abs(f(workingRange.MidPoint()));

            if (!f_has_a_zero_in(workingRange))
                return null;

            while (error() > epsilon)
            {
                workingRange = Divide2(workingRange);
                counter++;
            }

            Console.WriteLine($"Trong khoảng [{range.Begin},{range.End}], epsilon = {epsilon} , số lần lặp {counter}");
            return workingRange.MidPoint();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Tìm nghiệm của đa thức bằng phương pháp bisection");

            // p(x) = x^2 +5x + 6
            var p = new Polynomial
            {
                new Term(1,2),
                new Term(5,1),
                new Term(6,0),
            };

            Console.WriteLine("Đa thức p(x) = x^2 + 5x + 6");

            const double epsilon = 0;

            var zero1 = p.Solve(new Interval(-2.5, 0), epsilon);
            var zero2 = p.Solve(new Interval(-2.5, -4), epsilon);

            zero1.Print();
            zero2.Print();

            Console.ReadKey();
        }
    }
}


