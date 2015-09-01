using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowReductionAlgorithm
{
    public class LinearEquation
    {
        List<double> m_coefficients;
        double m_sum;
        int m_firstNonzeroIndex;

        public LinearEquation()
        {
            m_coefficients = new List<double>();
            m_sum = 0;
        }

        public LinearEquation(List<double> i_coefficients, double i_sum)
        {
            m_coefficients = new List<double>(i_coefficients);
            m_sum = i_sum;
            m_firstNonzeroIndex = FirstNonzero();
        }

        public int CoeffCount
        {
            get
            {
                return m_coefficients.Count;
            }
        }

        public int FirstNonzeroIndex
        {
            get
            {
                return m_firstNonzeroIndex;
            }
        }

        public double Sum
        {
            get
            {
                return m_sum;
            }
            set
            {
                m_sum = value;
            }
        }

        public void AddCoeff(double i_coeff)
        {
            if (m_firstNonzeroIndex == m_coefficients.Count && i_coeff == 0) m_firstNonzeroIndex++;
            m_coefficients.Add(i_coeff);
            
        }

        public double GetCoeff(int i_index)
        {
            if (i_index >= CoeffCount) return 0;
            else return (double)m_coefficients[i_index];
        }

        public static LinearEquation operator +(LinearEquation i_firstLinEq, LinearEquation i_secondLinEq)
        {
            int _count = Math.Max(i_firstLinEq.CoeffCount, i_secondLinEq.CoeffCount);
            List<double> _coeff = new List<double>();

            for (int i = 0; i < _count; i++)
            {
                _coeff.Add(i_firstLinEq.GetCoeff(i) + i_secondLinEq.GetCoeff(i));
            }

            double _sum = i_firstLinEq.Sum + i_secondLinEq.Sum;

            return new LinearEquation(_coeff, _sum);
        }

        public static LinearEquation operator -(LinearEquation i_firstLinEq, LinearEquation i_secondLinEq)
        {
            int _count = Math.Max(i_firstLinEq.CoeffCount, i_secondLinEq.CoeffCount);
            List<double> _coeff = new List<double>();

            for (int i = 0; i < _count; i++)
            {
                _coeff.Add(i_firstLinEq.GetCoeff(i) - i_secondLinEq.GetCoeff(i));
            }

            double _sum = i_firstLinEq.Sum - i_secondLinEq.Sum;

            return new LinearEquation(_coeff, _sum);
        }


        public static LinearEquation operator *(LinearEquation i_linEq, double i_coeff)
        {
            List<double> _coeff = new List<double>();

            for (int i = 0; i < i_linEq.CoeffCount; i++)
            {
                _coeff.Add(i_linEq.GetCoeff(i) * i_coeff);
            }
            return new LinearEquation(_coeff, i_linEq.Sum * i_coeff);
        }

        public static LinearEquation operator /(LinearEquation i_linEq, double i_coeff)
        {
            List<double> _coeff = new List<double>();

            for (int i = 0; i < i_linEq.CoeffCount; i++)
            {
                _coeff.Add(i_linEq.GetCoeff(i) / i_coeff);
            }
            return new LinearEquation(_coeff, i_linEq.Sum / i_coeff);
        }

        public static Tuple<LinearEquation, double, double> Deduct(LinearEquation i_firstLinEq, LinearEquation i_secondLinEq)
        {
            if (i_firstLinEq.FirstNonzeroIndex == i_secondLinEq.FirstNonzeroIndex)
            {
                int _index = i_firstLinEq.FirstNonzeroIndex;
                double _LCMultiple = LeastCommonMultiple(i_firstLinEq.GetCoeff(_index), i_secondLinEq.GetCoeff(_index));

                double _firstCoeff = _LCMultiple / i_firstLinEq.GetCoeff(_index);
                double _secondCoeff = _LCMultiple / i_secondLinEq.GetCoeff(_index);

                LinearEquation _equation = i_firstLinEq * _firstCoeff - i_secondLinEq * _secondCoeff;
                return new Tuple<LinearEquation, double, double>(_equation, _firstCoeff, _secondCoeff);
            
            }
            return null;
        }
        private static bool isInteger(double i_number)
        {
            return i_number - Math.Truncate(i_number) == 0;
        }

        private static double LeastCommonMultiple(double i_firstNum, double i_secondNum)
        {
            if (isInteger(i_firstNum) && isInteger(i_secondNum))
            {
                int _step = Math.Max((int)Math.Abs(i_firstNum), (int)Math.Abs(i_secondNum));

                for (int i = 0; i <= Math.Abs(i_firstNum * i_secondNum); i += _step)
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
            double _minCoeff = ArithmeticMean();

            for (int i = 0; i < m_coefficients.Count; i++)
            {
                m_coefficients[i] /= _minCoeff;
            }
            m_sum /= _minCoeff;

        }

        public double MaxAbsCoeff()
        {
            double _maxCoeff = Math.Abs(m_coefficients[0]);
            foreach (double _coeff in m_coefficients)
                if (Math.Abs(_coeff) > _maxCoeff) _maxCoeff = Math.Abs(_coeff);

            return _maxCoeff;
        }

        public double ArithmeticMean()
        {
            double _arithmeticMean = 0;
            foreach (double _coeff in m_coefficients)
            {
                _arithmeticMean += _coeff / (m_coefficients.Count + 1);
            }
            _arithmeticMean += Sum / (m_coefficients.Count + 1);
            return _arithmeticMean;
        }

        private int FirstNonzero()
        {
            int _zeroCounter = 0;
            foreach (double _coeff in m_coefficients)
            {
                if (_coeff == 0) _zeroCounter++;
                else return _zeroCounter;
            }
            return _zeroCounter;
        }

        public bool isIntegers()
        {
            foreach (double _number in m_coefficients)
                if (!isInteger(_number)) return false;
            return true;
        }

        public bool isCompatible()
        {
            return !(m_firstNonzeroIndex == m_coefficients.Count && m_sum != 0);
        }

        public bool isNulls()
        {
            return (m_firstNonzeroIndex == m_coefficients.Count && m_sum == 0);
        }

        
    }
}
