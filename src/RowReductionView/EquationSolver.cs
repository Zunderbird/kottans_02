using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowReductionAlgorithm
{
    public class EquationSolver
    {
        List<LinearEquation> _mEquations;
        List<Tuple<List<LinearEquation>, string>> _mHistory;
        
        List<double> _mXVector;
        List<double> _mErrorsVector;     

         double MAX_EQUATIONS_VALUE = Math.Sqrt(int.MaxValue);

        public bool SortZeroOnTop { get; set; }
        public bool ReduceByMainElement { get; set; }

        public int CoeffCount { get; private set; }

        public void Init(double[,] i_matrix)
        {
            _mEquations = new List<LinearEquation>();
            _mHistory = new List<Tuple<List<LinearEquation>, string>>();
            _mXVector = new List<double>();
            _mErrorsVector = new List<double>();

            CoeffCount = i_matrix.GetUpperBound(1);
            _mEquations = ConvertToEquations(i_matrix);
            
            _mHistory.Add(new Tuple<List<LinearEquation>, string>(new List<LinearEquation>(_mEquations), 
                "Initial linear equation system"));

            SortZeroOnTop = true;
            ReduceByMainElement = true;
        }
        
        public List<double> XVector
        {
            get
            {
                return new List<double>(_mXVector);
            }
        }

        public List<double> ErrorVector
        {
            get
            {
                return new List<double>(_mErrorsVector);
            }
        }

        public List<Tuple<List<LinearEquation>, string>> History
        {
            get
            {
                return new List<Tuple<List<LinearEquation>,string>>(_mHistory);
            }
        }

        public static List<LinearEquation> ConvertToEquations (double [,] i_matrix)
        {
            if (i_matrix.GetUpperBound(1) > 0)
            {
                var equations = new List<LinearEquation>();

                for (int i = 0; i < i_matrix.GetUpperBound(0) + 1; i++)
                {
                    var equation = new LinearEquation();

                    for (int j = 0; j < i_matrix.GetUpperBound(1); j++)
                    {
                        equation.AddCoeff(i_matrix[i, j]);
                    }
                    equation.Sum = i_matrix[i, i_matrix.GetUpperBound(1)];
                    equations.Add(equation);
                }
                return equations;
            }
            else return new List<LinearEquation>();
        }

        public static double[,] ConvertToMatrix(List<LinearEquation> i_equations, int i_coeffCount)
        {
            if (i_coeffCount > 0)
            {
                double[,] matrix = new double[i_coeffCount, i_coeffCount + 1];
                for (int i = 0; i < i_equations.Count; i++)
                {
                    for (int j = 0; j < i_coeffCount; j++)
                    {
                        matrix[i, j] = i_equations[i].GetCoeff(j);
                    }
                    matrix[i, i_coeffCount] = i_equations[i].Sum;
                }
                return matrix;
            }
            else return null;
        }

        public static int MaxCoeffCount(List<LinearEquation> i_equations)
        {
            int maxCoeffCount = 0;

            foreach (LinearEquation equation in i_equations)
            {
                if (maxCoeffCount < equation.CoeffCount)
                    maxCoeffCount = equation.CoeffCount;
            }
            return maxCoeffCount;
        }

        private void AddRecordToHistory(string i_text)
        {
            var historyEquations = new List<LinearEquation>(_mEquations);
            if (SortZeroOnTop == false) historyEquations.Reverse();
            _mHistory.Add(new Tuple<List<LinearEquation>, string>(historyEquations, i_text));
        }

        private bool Swap(int i_firstIndex, int i_secondIndex)
        {
            if (i_firstIndex >= _mEquations.Count || i_secondIndex >= _mEquations.Count) return false;
            else
            {
                LinearEquation tempEquation = _mEquations[i_firstIndex];
                _mEquations[i_firstIndex] = _mEquations[i_secondIndex];
                _mEquations[i_secondIndex] = tempEquation;
                return true;
            }
        }

        private void Reduce(int i_firstIndex, int i_second_index)
        {
            var reducedEquation = LinearEquation.Deduct(_mEquations[i_firstIndex], _mEquations[i_second_index]);

            _mEquations[i_firstIndex] = reducedEquation.Item1;

            int historyIndex = i_second_index;

            if (SortZeroOnTop == false)
                historyIndex = _mEquations.Count - historyIndex;

            string _text = "Reduced: equation " + historyIndex + " * (" + reducedEquation.Item2 +
                        ") - equation " + (historyIndex + 1) + " * (" + reducedEquation.Item3 + ")";
            AddRecordToHistory(_text);
        }

        private int GetMainElementIndex(int i_unknownIndex, int i_count)
        {
            int index = 0;
            for (int i = 0; i < i_count; i++)
            {
                if (Math.Abs(_mEquations[index].GetCoeff(i_unknownIndex)) < Math.Abs(_mEquations[i].GetCoeff(i_unknownIndex)))
                {
                    index = i;
                }
            }
            return index;
        }

        private void ReduceByMainElements()
        {
            for (int i = 0; i < this.CoeffCount; i++)
            {
                int mainIndex = this.GetMainElementIndex(i, _mEquations.Count - i);

                for (int j = 0; j < _mEquations.Count - i; j++)
                {
                    if (j != mainIndex && _mEquations[j].GetCoeff(i) != 0)
                    {
                        if (NormalizeValues())
                            AddRecordToHistory("Normalized");
                        Reduce(j, mainIndex);
                    }
                }
                Swap(mainIndex, _mEquations.Count - i - 1);
                AddRecordToHistory("Swap");
            }
        }

        private bool SortByZero()
        {
            bool isOrderChanged = false;

            for (int i = 1; i < _mEquations.Count; i++)
            {
                if (_mEquations[i].FirstNonzeroIndex > 0)
                    for (int j = 0; j < i; j++)
                        if (_mEquations[i].FirstNonzeroIndex > _mEquations[j].FirstNonzeroIndex)
                        {
                            Swap(i, j);
                            i = j;
                            isOrderChanged = true;
                            break;
                        }
            }
            return isOrderChanged;
        }

        private int GetNextIndexForReduce()
        {
            for (int i = 0; i < CoeffCount; i++)
                for (int j = 0; j < _mEquations.Count; j++)
                    if (i < _mEquations.Count - j - 1 && _mEquations[j].GetCoeff(i) != 0)
                        return j;
            return -1;
        }

        private void ReduceByCoeffCount()
        {
            int index;

            do
            {
                if (SortByZero())
                    AddRecordToHistory("Sorted");

                index = GetNextIndexForReduce();

                if (index != -1)
                {
                    //Check for over range and if it is true, normalized values 

                    if (NormalizeValues())
                        AddRecordToHistory("Normalized");

                    // Deduct one equation from another

                    Reduce(index, index + 1);
                }
            } while (index != -1);
        }    

        public string Solve()
        {
            if (_mEquations.Count != 0)
            {
                if (SortZeroOnTop == false) _mEquations.Reverse();

                if (ReduceByMainElement) ReduceByMainElements();
                else ReduceByCoeffCount();

                if (!this.IsCompatible()) return "There are no solutions!";
                if (!this.IsUniqueSolution()) return "There are infinitely many solutions!";

                _mXVector = CalcXVector();
                _mErrorsVector = CalcErrorsVector();

                return "There is unique solution!";               
            }
            return "Entering matrix is empty!";
        }

        private List<double> CalcXVector()
        {
            var XVector = new List<double>();

            if (_mEquations.Count >= CoeffCount)
            {
                for (int i = 0; i < _mEquations.Count; i++)
                {
                    double valuesSum = ValuesSum(XVector);
                    double value = (_mEquations[i].Sum - valuesSum) / _mEquations[i].GetCoeff(CoeffCount - 1 - i);
                    XVector.Add(value);
                }
                XVector.Reverse();
            }
            return XVector;
        }

        private double ValuesSum(List<double> i_result)
        {
            double sum = 0;

            for (int i = 0; i < i_result.Count; i++)
            {
                sum += i_result[i] * _mEquations[i_result.Count].GetCoeff(CoeffCount - i - 1);
            }
            return sum;
        }

        private List<double> CalcErrorsVector()
        {
            var errorsVector = new List<double>();

            var initialEquations = new List<LinearEquation>(_mHistory[0].Item1);

            foreach (LinearEquation equation in initialEquations)
            {
                double sum = 0;

                for (int i = 0; i < equation.CoeffCount; i++)
                {
                    sum += _mXVector[i] * equation.GetCoeff(i);
                }
                errorsVector.Add(equation.Sum - sum);
            }
            return errorsVector;
        }

        private bool NormalizeValues()
        {
            bool isDone = false;

            foreach (LinearEquation equation in _mEquations)
            {
                if (equation.MaxAbsCoeff() >= MAX_EQUATIONS_VALUE || Math.Abs(equation.Sum) >= MAX_EQUATIONS_VALUE)
                {
                    equation.ReduceNumbers();
                    isDone = true;
                }
            }
            return isDone;
        }

        private bool IsCompatible()
        {
            foreach (LinearEquation equation in _mEquations)
            {
                if (equation.IsCompatible() == false)
                    return false;
            }
            return true;
        }

        private bool IsUniqueSolution()
        {
            int nullEqCounter = 0;

            foreach (LinearEquation equation in _mEquations)
            {
                if (equation.IsNulls() == true)
                    nullEqCounter++;
            }
            return (_mEquations.Count - nullEqCounter >= CoeffCount);
        }
    }
}
