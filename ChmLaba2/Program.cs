using System;
using System.Globalization;

//public class Program
//{
//    const double EPSILON = 1e-6;

//    public class Vector
//    {
//        public double x1;
//        public double x2;
//        public double x3;
//        public double x4;

//        public Vector(double x1, double x2, double x3, double x4)
//        {
//            this.x1 = x1;
//            this.x2 = x2;
//            this.x3 = x3;
//            this.x4 = x4;
//        }

//        public static Vector CreateNextIterationVector(Vector x0)
//        {
//            double val1 = 1.0 / 3 * x0.x2 - 2.0 / 15 * x0.x3 - 2.0 / 15 * x0.x4 + 2.0 / 5;
//            double val2 = 5.0 / 13 * x0.x1 - 3.0 / 13 * x0.x3 + 4.0 / 13 * x0.x4 - 12.0 / 13;
//            double val3 = -2.0 / 13 * x0.x1 - 3.0 / 13 * x0.x4 + 1.0 / 13;
//            double val4 = -2.0 / 9 * x0.x1 + 4.0 / 9 * x0.x2 - 1.0 / 3 * x0.x3 + 1.0 / 3;
//            return new Vector(val1, val2, val3, val4);
//        }

//        public static Vector Subtraction(Vector x1, Vector x0)
//        {
//            return new Vector(
//                x1.x1 - x0.x1,
//                x1.x2 - x0.x2,
//                x1.x3 - x0.x3,
//                x1.x4 - x0.x4
//            );
//        }

//        public static double Norm(Vector subtracted)
//        {
//            return Math.Max(Math.Max(Math.Abs(subtracted.x1), Math.Abs(subtracted.x2)),
//                Math.Max(Math.Abs(subtracted.x3), Math.Abs(subtracted.x4)));
//        }
//    }

//    public static void Main()
//    {
//        Console.OutputEncoding = System.Text.Encoding.Unicode;
//        Console.WriteLine("Знайти наближено розв'язок системи методом Якобі, поданої у матричному вигляді:\n" +
//            "15  -5  2  2 | 6\n" +
//            "-5  13  3  -4| -12\n" +
//            "2    0  13 3 | 1\n" +
//            "2   -4  3  9 | 3\n" +
//            "з точністю eps = " + EPSILON);
//        Vector x0 = new Vector(0, 0, 0, 0);
//        Vector x1;
//        Vector x0Temp;
//        Vector subtracted;
//        int iterationNumber = 0;
//        Console.WriteLine();
//        do
//        {
//            x1 = Vector.CreateNextIterationVector(x0);
//            x0Temp = x0;
//            x0 = x1;
//            subtracted = Vector.Subtraction(x1, x0Temp);
//            iterationNumber++;
//            Console.WriteLine("Номер ітерації: " + iterationNumber + " X: x1: "
//                + x1.x1 + " x2: " + x1.x2 + " x3: " + x1.x3 + " x4: " + x1.x4
//                + " |X(" + iterationNumber + ") - X(" + (iterationNumber - 1) + ")|: " + Vector.Norm(subtracted));
//        } while (Vector.Norm(subtracted) > EPSILON);


//        Console.WriteLine();
//        Console.WriteLine("Результат обчислення:");
//        Console.WriteLine("x1 = " + x1.x1);
//        Console.WriteLine("x2 = " + x1.x2);
//        Console.WriteLine("x3 = " + x1.x3);
//        Console.WriteLine("x4 = " + x1.x4);
//    }z
//}


public class Program
{
    static double[][] S;
    static double[][] D;
    static double[][] A;
    static double[][] multiplications;

    public static void TransMatrix()
    {
        int n = A.Length;
        double[][] result = new double[n][];
        for (int i = 0; i < n; i++)
        {
            result[i] = new double[n];
            for (int j = 0; j < n; j++)
            {
                result[i][j] = S[j][i];
            }
        }
        S = result;
    }

    public static void Multiplications()
    {
        double sum = 0;
        int n = A.Length;
        multiplications = new double[n][];
        for (int i = 0; i < n; i++)
        {
            multiplications[i] = new double[n];
            for (int j = 0; j < n; j++)
            {
                sum = 0;
                for (int k = 0; k < n; k++)
                {
                    sum += S[i][k] * D[k][j];
                    multiplications[i][j] = sum;
                }
            }
        }
    }
    public static void valueOfDMatrix(int index)
    {
        double d = A[index][index];
        for (int j = 0; j < index; j++)
        {
            d -= D[j][j] * Math.Pow(S[j][index], 2);
        }
        if (d > 0)
            d = 1;
        else if (d == 0)
            d = 0;
        else
            d = -1;
        D[index][index] = (int)d;
    }

    public static double[] YResults(double[] b)
    {
        int n = b.Length;
        double[] result = new double[n];
        for (int i = 0; i < n; i++)
        {
            double sum = 0.0;
            for (int j = 0; j < i; j++)
            {
                sum += S[i][j] * result[j];
            }
            result[i] = (b[i] - sum) / S[i][i];
        }
        return result;
    }

    public static double[] XResults(double[] y)
    {
        int n = y.Length;
        double[] result = new double[n];
        for (int i = n - 1; i >= 0; i--)
        {
            double sum = 0.0;
            for (int j = i + 1; j < n; j++)
            {
                sum += S[j][i] * result[j];
            }
            result[i] = (y[i] - sum) / S[i][i];
        }
        return result;
    }

    public static void TriangleMatrix(double[][] R)
    {
        A = R;
        int n = A.Length;
        S = new double[n][];
        D = new double[n][];
        for (int i = 0; i < n; i++)
        {
            S[i] = new double[n];
            D[i] = new double[n];
        }
        double valueS;
        for (int i = 0; i < n; i++)
        {
            valueOfDMatrix(i);
            for (int j = i; j < n; j++)
            {
                valueS = A[i][j];
                for (int k = 0; k < i; k++)
                {
                    if (i == j)
                    {
                        valueS -= D[k][k] * S[k][i] * S[k][i];
                    }
                    else
                    {
                        valueS -= S[k][i] * S[k][j] * D[k][k];
                    }
                }
                if (i == j)
                {
                    valueS = Math.Sqrt(Math.Abs(valueS));
                }
                else
                {
                    valueS = valueS / (D[i][i] * S[i][i]);
                }
                S[i][j] = valueS;
            }
        }
    }

    public static double det()
    {
        double result = 1;
        for (int i = 0; i < A.Length; i++)
        {
            result *= D[i][i] * Math.Pow(S[i][i], 2);
        }
        return result;
    }

    public static void Display(double[][] F)
    {
        for (int i = 0; i < A.Length; i++)
        {
            for (int j = 0; j < A.Length; j++)
            {
                Console.Write(F[i][j] + " ");
            }
            Console.WriteLine();
        }
    }

    public static void Main(string[] args)
    {
        double[][] A = {
        new double[] { 3, -5, 2, 2 },
        new double[] { -5, 1, 3, -4 },
        new double[] { 2, 0, 1, 3 },
        new double[] { 2, -4, 3, -3 }
    };
        double[] b = { 6, -12, 1, 3 };
        TriangleMatrix(A);
        Console.WriteLine("Matrix S");
        Display(S);
        Console.WriteLine("\nMatrix D");
        Display(D);
        TransMatrix();
        Console.WriteLine("\nTransposed matrix S ");
        Display(S);
        Multiplications();
        Console.WriteLine("\nTransposed matrix S * matrix D");
        Display(multiplications);
        Console.WriteLine("\nу value");
        double[] y = YResults(b);
        for (int i = 0; i < y.Length; i++)
        {
            Console.WriteLine("y[" + (i + 1) + "]=" + y[i] + "   ");
        }
        Console.WriteLine("\n=================================================\n");
        Console.WriteLine("\n Result\n");
        double[] x = XResults(y);
        for (int i = 0; i < x.Length; i++)
        {
            Console.WriteLine("x[" + (i + 1) + "]=" + x[i] + "   ");
        }
    }
}



