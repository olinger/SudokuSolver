using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class SudokuGrid
    {
        public int[,] Grid;
        private List<List<int>> Columns;
        private List<List<int>> Rows;
        private List<List<int>> Squares;
        private int Clues;
        public bool Valid;
        public SudokuGrid(int[,] grid)
        {
            Grid = grid;
            Columns = columns_from_grid();
            Rows = rows_from_grid();
            Squares = squares_from_grid();
            Clues = count_clues_in_grid();
            Valid = Validate();
        }
        List<List<int>> columns_from_grid()
        {
            List<List<int>> cols = new List<List<int>>();
            for (int x = 0; x < 9; x++)
            {
                List<int> col = new List<int>();
                for (int y = 0; y < 9; y++)
                {
                    col.Add(Grid[x, y]);
                }
                cols.Add(col);
            }
            return cols;
        }

        public void print_grid()
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {

                    Console.Write(Grid[x, y] + " ");
                }
                Console.WriteLine();
            }
        }

        List<List<int>> rows_from_grid()
        {
            List<List<int>> rows = new List<List<int>>();
            for (int y = 0; y < 9; y++)
            {
                List<int> row = new List<int>();
                for (int x = 0; x < 9; x++)
                {
                    row.Add(Grid[x, y]);
                }
                rows.Add(row);
            }
            return rows;
        }

        List<List<int>> squares_from_grid()
        {
            List<List<int>> squares = new List<List<int>>();
            for (int x = 0; x < 9; x += 3)
            {
                for (int y = 0; y < 9; y += 3)
                {
                    List<int> square = new List<int>();
                    for (int dx = 0; dx < 3; dx++)
                    {
                        for (int dy = 0; dy < 3; dy++)
                        {
                            square.Add(Grid[x + dx, y + dy]);
                        }
                    }
                    squares.Add(square);
                }
            }
            return squares;
        }

        int count_clues_in_grid()
        {
            int clues = 0;
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (Grid[x, y] != 0)
                    {
                        clues++;
                    }
                }
            }
            return clues;
        }

        //ensure all rows, columns, and squares have no repeated values 
        bool is_valid(List<int> component)
        {
            var no_zero = component;
            no_zero.RemoveAll(i => i == 0);
            return no_zero.Count == no_zero.Distinct().Count();
        }

        public bool Validate()
        {
            if (!(Rows.TrueForAll(is_valid) && Columns.TrueForAll(is_valid) && Squares.TrueForAll(is_valid)))
            {
                Console.WriteLine("Sudoku is not valid: There is atleast one duplicate in a row, column, or square");
                return false;
            }
            if (Clues < 17)
            {
                //sudoku with 16 or less clues has no unique solution
                Console.WriteLine("Sudoku is not valid: Contains less than 17 clues therefore has no unique solution");
                return false;
            }
            return true;
        }
   
        public void Save_File(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach(var row in Rows)
                {
                    string line = "";
                    foreach (var x in row)
                    {
                        line += x.ToString();
                    }
                    writer.WriteLine(line);
                }
            }
        }
    }

}
