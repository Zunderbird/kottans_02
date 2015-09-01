using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowReductionAlgorithm
{
    public class EquationSolver
    {
        List<LinearEquation> m_equations;
        List<Tuple<List<LinearEquation>, string>> m_history;
        
        List<double> m_XVector;
        List<double> m_errorsVector;
        
        int m_coeffCount;
        bool m_sortZeroOnTop;

        double MAX_EQUATIONS_VALUE = Math.Sqrt(int.MaxValue);

        public void Init(double[,] i_matrix)
        {
            m_equations = new List<LinearEquation>();
            m_history = new List<Tuple<List<LinearEquation>, string>>();
            m_XVector = new List<double>();
            m_errorsVector = new List<double>();

            m_coeffCount = i_matrix.GetUpperBound(1);
            m_equations = ConvertToEquations(i_matrix);
            
            m_history.Add(new Tuple<List<LinearEquation>, string>(new List<LinearEquation>(m_equations), 
                "Initial linear equation system"));
            
            m_sortZeroOnTop = true;
        }

        public int CoeffCount
        {
            get
            {
                return m_coeffCount;
            }
        }
        
        public List<double> XVector
        {
            get
            {
                return new List<double>(m_XVector);
            }
        }

        public List<double> ErrorVector
        {
            get
            {
                return new List<double>(m_errorsVector);
            }
        }

        public List<Tuple<List<LinearEquation>, string>> History
        {
            get
            {
                return new List<Tuple<List<LinearEquation>,string>>(m_history);
            }
        }

        public bool SortZeroOnTop
        {
            get
            {
                return m_sortZeroOnTop;
            }
            set
            {
                m_sortZeroOnTop = value;
            }
        }

        public static List<LinearEquation> ConvertToEquations (double [,] i_matrix)
        {
            if (i_matrix.GetUpperBound(1) > 0)
            {
                List<LinearEquation> _equations = new List<LinearEquation>();

                for (int i = 0; i < i_matrix.GetUpperBound(0) + 1; i++)
                {
                    LinearEquation _equation = new LinearEquation();

                    for (int j = 0; j < i_matrix.GetUpperBound(1); j++)
                    {
                        _equation.AddCoeff(i_matrix[i, j]);
                    }

                    _equation.Sum = i_matrix[i, i_matrix.GetUpperBound(1)];
                    _equations.Add(_equation);
                }
                return _equations;
            }
            else return new List<LinearEquation>();
        }

        public static double[,] ConvertToMatrix(List<LinearEquation> i_equations, int i_coeffCount)
        {
            if (i_coeffCount > 0)
            {
                double[,] _matrix = new double[i_coeffCount, i_coeffCount + 1];
                for (int i = 0; i < i_equations.Count; i++)
                {
                    for (int j = 0; j < i_coeffCount; j++)
                    {
                        _matrix[i, j] = i_equations[i].GetCoeff(j);
                    }
                    _matrix[i, i_coeffCount] = i_equations[i].Sum;
                }
                return _matrix;
            }
            else return null;
        }

        public bool Swap(int i_firstIndex, int i_secondIndex)
        {
            if (i_firstIndex >= m_equations.Count || i_secondIndex >= m_equations.Count) return false;
            else
            {
                LinearEquation _tempEquation = m_equations[i_firstIndex];
                m_equations[i_firstIndex] = m_equations[i_secondIndex];
                m_equations[i_secondIndex] = _tempEquation;
                return true;
            }
        }

        private bool Sort()
        {
            bool _isOrderChanged = false;

            for (int i = 1; i < m_equations.Count; i++)
            {
                if (m_equations[i].FirstNonzeroIndex > 0)
                    for (int j = 0; j < i; j++)
                        if (m_equations[i].FirstNonzeroIndex > m_equations[j].FirstNonzeroIndex)
                        {
                            Swap(i, j);
                            i = j;
                            _isOrderChanged = true;
                            break;
                        }
            }
            return _isOrderChanged;
        }

        private int GetNextIndexForReduce()
        {
            for (int i = 0; i < CoeffCount; i++)
                for (int j = 0; j < m_equations.Count; j++)
                    if (i < m_equations.Count - j - 1 && m_equations[j].GetCoeff(i) != 0)
                        return j;
            return -1;
        }

        public void AddRecordToHistory(string i_text)
        {
            List<LinearEquation> _historyEquations = new List<LinearEquation>(m_equations);
            if (m_sortZeroOnTop == false) _historyEquations.Reverse();
            m_history.Add(new Tuple<List<LinearEquation>, string>(_historyEquations, i_text));
        }

        public void ReduceCoeffCount()
        {
            int _index;

            do
            {
                if (Sort())
                    AddRecordToHistory("Sorted");

                _index = GetNextIndexForReduce();

                if (_index != -1)
                {
                    //Check for over range and if it is true, normalized values 

                    if (NormalizeValues())
                        AddRecordToHistory("Normalized");

                    // Deduct one equation from another

                    Tuple<LinearEquation, double, double> _reducedEquation = LinearEquation.Deduct(m_equations[_index], m_equations[_index + 1]);
                    m_equations[_index] = _reducedEquation.Item1;

                    int _historyIndex = _index + 1;

                    if (m_sortZeroOnTop == false)
                        _historyIndex = m_equations.Count - _historyIndex;

                    string _text = "Reduced: equation " + _historyIndex + " * (" + _reducedEquation.Item2 +
                                ") - equation " + (_historyIndex + 1) + " * (" + _reducedEquation.Item3 + ")";
                    AddRecordToHistory(_text);

                }
            } while (_index != -1);
        }    

        public string Solve()
        {
            if (m_equations.Count != 0)
            {
                if (m_sortZeroOnTop == false) m_equations.Reverse();

                ReduceCoeffCount();

                if (!this.isCompatible()) return "There are no solutions!";
                if (!this.isUniqueSolution()) return "There are infinitely many solutions!";

                m_XVector = CalcXVector();
                m_errorsVector = CalcErrorsVector();

                return "There is unique solution!";
                
            }
            return "Entering matrix is empty!";
        }

        public List<double> CalcXVector()
        {
            List<double> _XVector = new List<double>();

            if (m_equations.Count >= CoeffCount)
            {
                for (int i = 0; i < m_equations.Count; i++)
                {
                    double _valuesSum = ValuesSum(_XVector);
                    double _value = (m_equations[i].Sum - _valuesSum) / m_equations[i].GetCoeff(CoeffCount - 1 - i);
                    _XVector.Add(_value);
                }
                _XVector.Reverse();
            }
            return _XVector;
        }

        private double ValuesSum(List<double> i_result)
        {
            double _sum = 0;

            for (int i = 0; i < i_result.Count; i++)
            {
                _sum += i_result[i] * m_equations[i_result.Count].GetCoeff(CoeffCount - i - 1);
            }
            return _sum;
        }

        public List<double> CalcErrorsVector()
        {
            List<double> _errorsVector = new List<double>();

            List<LinearEquation> _initialEquations = m_history[0].Item1;

            foreach (LinearEquation _equation in _initialEquations)
            {
                double _sum = 0;

                for (int i = 0; i < _equation.CoeffCount; i++)
                {
                    _sum += m_XVector[i] * _equation.GetCoeff(i);
                }
                _errorsVector.Add(_equation.Sum - _sum);
            }
            return _errorsVector;
        }

        public bool NormalizeValues()
        {
            bool _isDone = false;

            foreach (LinearEquation _equation in m_equations)
            {
                if (_equation.MaxAbsCoeff() >= MAX_EQUATIONS_VALUE || Math.Abs(_equation.Sum) >= MAX_EQUATIONS_VALUE)
                {
                    _equation.ReduceNumbers();
                    _isDone = true;
                }
            }
            return _isDone;

        }

        public bool isCompatible()
        {
            foreach (LinearEquation _equation in m_equations)
            {
                if (_equation.isCompatible() == false)
                    return false;
            }
            return true;
        }

        public bool isUniqueSolution()
        {
            int _nullEqCounter = 0;

            foreach (LinearEquation _equation in m_equations)
            {
                if (_equation.isNulls() == true)
                    _nullEqCounter++;
            }
            return (m_equations.Count - _nullEqCounter >= CoeffCount);
        }

        public static int MaxCoeffCount(List<LinearEquation> i_equations)
        {
            int _maxCoeffCount = 0;

            foreach (LinearEquation _equation in i_equations)
            {
                if ( _maxCoeffCount < _equation.CoeffCount) 
                    _maxCoeffCount = _equation.CoeffCount;
            }
            return _maxCoeffCount;
        }

        public static void PrintToConsole(List <LinearEquation> i_equations, string i_text)
        {
            Console.WriteLine(i_text);
            foreach (LinearEquation _linear in i_equations)
            {
                for (int i = 0; i < _linear.CoeffCount; i++)
                {
                    Console.Write("{0}\t", _linear.GetCoeff(i));
                }
                Console.Write("{0}\t", _linear.Sum);
                Console.WriteLine();
            }
        }

        

    }
}
