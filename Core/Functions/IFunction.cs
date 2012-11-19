using System;

namespace Core.Functions
{
    public interface IFunction
    {
        double Value(double arg);

        double Value(double arg1, double arg2);
    }

    public class FuncFxt : IFunction
    {
        public double Value(double arg)
        {
            throw new System.NotImplementedException();
        }

        public double Value(double arg1, double arg2)
        {
            return Math.Sin(arg1) + 2 * arg2;
        }
    }

    public class FuncU0X : IFunction
    {
        public double Value(double arg)
        {
            return arg*Math.Exp(arg) + 1;
        }

        public double Value(double arg1, double arg2)
        {
            throw new NotImplementedException();
        }
    }

    public class FuncU1X :IFunction
    {
        public double Value(double arg)
        {
            return arg + 1;
        }

        public double Value(double arg1, double arg2)
        {
            throw new NotImplementedException();
        }
    }

    public class FuncU2X : IFunction
    {
        public double Value(double arg)
        {
            return arg + 1 + Math.Exp(1 - arg);
        }

        public double Value(double arg1, double arg2)
        {
            throw new NotImplementedException();
        }
    }
}
