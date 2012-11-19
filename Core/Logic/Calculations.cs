using System.Collections.Generic;
using Core.Functions;

namespace Core.Logic
{
    public class Calculations
    {
        public double Lenth { private get; set; }

        public int N { private get; set; }

        public double Time { private get; set; }

        public int K { private get; set; }

        public IFunction Fxt { private get; set; }

        public IFunction U0 { private get; set; }

        public IFunction U1 { private get; set; }

        public IFunction U2 { private get; set; }

        public List<GraphPoint> Imlicite()
        {
            var tempResult = new double[N+1][];
            for(int i = 0; i <= N; i++)
            {
                tempResult[i] = new double[K+1];
            }

            // initialize board conditions
            for (int i = 0; i <= N; i++)
            {
                tempResult[i][0] = U0.Value(XArg(i));
            }
            for (int n = 0; n <= K-1; n++)
            {
                tempResult[0][n + 1] = U1.Value(TimeArg(n + 1));
                tempResult[N][n + 1] = U2.Value(TimeArg(n + 1));
            }


            for (int n = 0; n < K; n++)
            {
                for (int i = 1; i < N; i++)
                {
                    tempResult[i][n + 1] = tempResult[i][n] +
                                           Gamma*(tempResult[i + 1][n] - 2*tempResult[i][n] + tempResult[i - 1][n]) +
                                           Tao*Fxt.Value(XArg(i), TimeArg(n));
                }
            }

            var result = new List<GraphPoint>();
            for (int i = 0; i <= N; i++)
            {
                result.Add(new GraphPoint
                {
                    X = XArg(i),
                    Y = tempResult[i][K]
                });
            }

            return result;
        }

        public List<GraphPoint> NotImplicite()
        {
            var tempResult = new double[N + 1][];
            for (int i = 0; i <= N; i++)
            {
                tempResult[i] = new double[K + 1];
            }

            // initialize board conditions
            for (int i = 0; i <= N; i++)
            {
                tempResult[i][0] = U0.Value(XArg(i));
            }
            for (int n = 0; n <= K - 1; n++)
            {
                tempResult[0][n + 1] = U1.Value(TimeArg(n + 1));
                tempResult[N][n + 1] = U2.Value(TimeArg(n + 1));
            }


            var A = new double[N-2];
            var B = new double[N-2];
            var C = new double[N-2];
            var D = new double[N-2];
            double xi1 , xi2, mu1, mu2;
            for (int i = 1; i <= N - 1 - 2; i++)
            {
                A[i] = Gamma;
                B[i] = Gamma;
                C[i] = (1 + 2 * Gamma);
            }

            for (int n = 0; n <= K-1; n++)
            {
                for (int i = 2; i <= N - 1 - 1 ; i++)
                {
                    D[i-1] = tempResult[i][n] + Tao * Fxt.Value( XArg(i), TimeArg(n) );
                }
                xi1 = Gamma/(1 + 2*Gamma);
                mu1 = (tempResult[1][n] + Tao*Fxt.Value(XArg(1), TimeArg(n)) + Gamma * tempResult[0][n])/ (1 + 2*Gamma);
                xi2 = Gamma/(1 + 2*Gamma);
                mu2 = (tempResult[N-1][n] + Tao*Fxt.Value(XArg(N-1), TimeArg(n)) + Gamma * tempResult[N][n])/ (1 + 2*Gamma);

                var tempY = MetodProgonki(N - 2, A, B, C, D, xi1, mu1, xi2, mu2);

                for (int i = 1; i <= N-1; i++)
                {
                    tempResult[i][n + 1] = tempY[i - 1];
                }
            }

            var result = new List<GraphPoint>();
            for (int i = 0; i <= N; i++)
            {
                result.Add(new GraphPoint
                               {
                                   X = XArg(i),
                                   Y = tempResult[i][K]
                               });
            }

            return result;
        }

        public double[] MetodProgonki(int N, double[] A, double[] B, double[] C, double[] D, double xi1, double mu1, double xi2, double mu2)
        {
            var Y = new double[N+1];

            var alpha = new double[N+1];
            var beta = new double[N+1];

            // direct
            alpha[1] = xi1;
            beta[1] = mu1;
            for (int i = 1; i <= N-1; i++)
            {
                alpha[i + 1] = B[i]/(C[i] - A[i]*alpha[i]);
                beta[i + 1] = (A[i]*beta[i] + D[i])/(C[i] - A[i]*alpha[i]);
            }

            //reverse
            Y[N] = (xi2*beta[N] + mu2)/(1 - xi2*alpha[N]);

            for (int i = N-1; i >= 0; i--)
            {
                Y[i] = alpha[i + 1]*Y[i + 1] + beta[i + 1];
            }
            
            return Y;
        }

        private double TimeArg(int index)
        {
            return Time*index/K;
        }

        private double XArg(double index)
        {
            return Lenth*index/N;
        }

        private double Tao { get { return Time/K; } }

        private double Hdelta { get { return Lenth/N; } }

        private double Gamma { get { return Tao/Hdelta; } }

        public bool CheckInputsForImplicit()
        {
            return Gamma < 0.5d;
        }
    }

    public class GraphPoint
    {
        public double X { get; set; }

        public double Y { get; set; }
    }
}
