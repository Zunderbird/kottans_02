using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowReductionAlgorithm
{
    public class LinearEquation
    {
        List<double> _mCoefficients;
        public double Sum { get; set; }
        public int FirstNonzeroIndex { get; private set; }

        public LinearEquation()
        {
            _mCoefficients = new List<double>();
            Sum = 0;
        }

        public LinearEquation(List<double> i_coefficients, double i_sum)
        {
            _mCoefficients = new List<double>(i_coefficients);
            Sum = i_sum;
            FirstNonzeroIndex = FirstNonzero();
        }

        public int CoeffCount
        {
            get
            {
                return _mCoefficients.Count;
            }
        }

        public void AddCoeff(double i_coeff)
        {
            if (FirstNonzeroIndex == _mCoefficients.Count && i_coeff == 0) FirstNonzeroIndex++;
            _mCoefficients.Add(i_coeff);           
        }

        public double GetCoeff(int i_index)
        {
            if (i_index >= CoeffCount) return 0;
            else return (double)_mCoefficients[i_index];
        }

        public static LinearEquation operator +(LinearEquation i_firstLinEq, LinearEquation i_secondLinEq)
        {
            int count = Math.Max(i_firstLinEq.CoeffCount, i_secondLinEq.CoeffCount);
            var coeff = new List<double>();

            for (int i = 0; i < count; i++)
            {
                coeff.Add(i_firstLinEq.GetCoeff(i) + i_secondLinEq.GetCoeff(i));
            }

            double sum = i_firstLinEq.Sum + i_secondLinEq.Sum;

            return new LinearEquation(coeff, sum);
        }

        public static LinearEquation operator -(LinearEquation i_firstLinEq, LinearEquation i_secondLinEq)
        {
            int count = Math.Max(i_firstLinEq.CoeffCount, i_secondLinEq.CoeffCount);
            var coeff = new List<double>();

            for (int i = 0; i < count; i++)
            {
                coeff.Add(i_firstLinEq.GetCoeff(i) - i_secondLinEq.GetCoeff(i));
            }

            double sum = i_firstLinEq.Sum - i_secondLinEq.Sum;

            return new LinearEquation(coeff, sum);
        }


        public static LinearEquation operator *(LinearEquation i_linEq, double i_coeff)
        {
           var coeff = new List<double>();

            for (int i = 0; i < i_linEq.CoeffCount; i++)
            {
                coeff.Add(i_linEq.GetCoeff(i) * i_coeff);
            }
            return new LinearEquation(coeff, i_linEq.Sum * i_coeff);
        }

        public static LinearEquation operator /(LinearEquation i_linEq, double i_coeff)
        {
            var coeff = new List<double>();

            for (int i = 0; i < i_linEq.CoeffCount; i++)
            {
                coeff.Add(i_linEq.GetCoeff(i) / i_coeff);
            }
            return new LinearEquation(coeff, i_linEq.Sum / i_coeff);
        }

        public static Tuple<LinearEquation, double, double> Deduct(LinearEquation i_firstLinEq, LinearEquation i_secondLinEq)
        {
            if (i_firstLinEq.FirstNonzeroIndex == i_secondLinEq.FirstNonzeroIndex)
            {
                int index = i_firstLinEq.FirstNonzeroIndex;

                double LCMultiple = LeastCommonMultiple(i_firstLinEq.GetCoeff(index), i_secondLinEq.GetCoeff(index));

                double firstCoeff = LCMultiple / i_firstLinEq.GetCoeff(index);
                double secondCoeff = LCMultiple / i_secondLinEq.GetCoeff(index);

                LinearEquation equation = i_firstLinEq * firstCoeff - i_secondLinEq * secondCoeff;
                return new Tuple<LinearEquation, double, double>(equation, firstCoeff, secondCoeff);
            
            }
            return null;
        }
        private static bool IsInteger(double i_number)
        {
            return i_number - Math.Truncate(i_number) == 0;
        }

        private static double LeastCommonMultiple(double i_firstNum, double i_secondNum)
        {
            if (IsInteger(i_firstNum) && IsInteger(i_secondNum))
            {
                int step = Math.Max((int)Math.Abs(i_firstNum), (int)Math.Abs(i_secondNum));

                for (int i = 0; i <= Math.Abs(i_firstNum * i_secondNum); i += step)
                {
                    if (i % i_firstNum == 0 && i % i_secondNum == 0 && i != 0)
                    {
                        return i;
                    }
                }
            }
            return Math.Abs(i_firstNum * i_secondNum);
        }

        public void ReduceNumbers()
        {
            double minCoeff = ArithmeticMean();

            for (int i = 0; i < _mCoefficients.Count; i++)
            {
                _mCoefficients[i] /= minCoeff;
            }
            Sum /= minCoeff;
        }

        public double MaxAbsCoeff()
        {
            double maxCoeff = Math.Abs(_mCoefficients[0]);
            foreach (double coeff in _mCoefficients)
                if (Math.Abs(coeff) > maxCoeff) maxCoeff = Math.Abs(coeff);
            return maxCoeff;
        }

        public double ArithmeticMean()
        {
            double arithmeticMean = 0;
            foreach (double _coeff in _mCoefficients)
            {
                arithmeticMean += _coeff / (_mCoefficients.Count + 1);
            }
            arithmeticMean += Sum / (_mCoefficients.Count + 1);
            return arithmeticMean;
        }

        private int FirstNonzero()
        {
            int zeroCounter = 0;
            foreach (double coeff in _mCoefficients)
            {
                if (coeff == 0) zeroCounter++;
                else return zeroCounter;
            }
            return zeroCounter;
        }

        public bool IsIntegers()
        {
            foreach (double number in _mCoefficients)
                if (!IsInteger(number)) return false;
            return true;
        }

        public bool IsCompatible()
        {
            return !(FirstNonzeroIndex == _mCoefficients.Count && Sum != 0);
        }

        public bool IsNulls()
        {
            return (FirstNonzeroIndex == _mCoefficients.Count && Sum == 0);
        }      
    }
}
