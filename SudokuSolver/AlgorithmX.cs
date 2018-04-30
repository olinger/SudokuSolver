using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class AlgorithmX
    {
        public Stack<DataNode> answer = new Stack<DataNode>();
        public static int solutions = 0;
        public ColumnNode header;
        public SudokuGrid Final_Solution;

        public AlgorithmX(ColumnNode h)
        {
            header = h;
        }
        public void print_all()
        {
            for (var n = header.Right; n != header; n = n.Right)
            {
                Console.Write(n.ID + " => ");
            }
            Console.WriteLine("");
        }
        static void cover(DataNode column)
        {
            column.Right.Left = column.Left;
            column.Left.Right = column.Right;
            for (var i = column.Down; i != column; i = i.Down)
            {
                for (var j = i.Right; j != i; j = j.Right)
                {
                    j.Down.Up = j.Up;
                    j.Up.Down = j.Down;
                    j.Column.Size -= 1;
                }
            }
        }

        static void uncover(DataNode column)
        {
            for (var i = column.Up; i != column; i = i.Up)
            {
                for (var j = i.Left; j != i; j = j.Left)
                {
                    j.Column.Size += 1;
                    j.Down.Up = j;
                    j.Up.Down = j;
                }
            }
            column.Right.Left = column;
            column.Left.Right = column;

        }
        public ColumnNode choose_next_column()
        {
            int size = int.MaxValue;
            ColumnNode nextCol = new ColumnNode(-1);
            ColumnNode j = header.Right.Column;
            while (j != header)
            {
                if (j.Size < size)
                {
                    nextCol = j;
                    size = j.Size;
                }
                j = j.Right.Column;
            }
            return nextCol;
        }
        public void store_solution(Stack<DataNode> answer)
        {
            int[,] formatted_grid = new int[9, 9];
            var rows = new List<int>();
            foreach (var k in answer)
            {
                rows.Add(k.ID);
            }
            rows.Sort();
            var grid = new List<int>();
            foreach (var row in rows)
            {
                grid.Add(row % 9 + 1);
            }
            int x = 0;
            int y = 0;
            for (int i = 0; i < 81; i++)
            {
                formatted_grid[y, x] = grid[i];
                x++;
                if ((i + 1) % 9 == 0)
                {
                    y++;
                    x = 0;
                }
            }
            var sudoku = new SudokuGrid(formatted_grid);
            if (sudoku.Valid)
            {
                Final_Solution = sudoku;
            }
            else
            {
                Console.WriteLine("Something went wrong -- solution found did not validate.");
            }
        }

        public void search()
        {
            if(solutions > 1)
            {
                //found more than one solution, therefore invalid sudoku
                throw new Exception("Error: Sudoku is invalid. No unique solution.");
            }
            if (header.Right == header)
            {
                //all columns were removed
                solutions++;
                store_solution(answer);
                return;
            }
            else
            {
                var c = choose_next_column();
                cover(c);

                for (var r = c.Down; r != c; r = r.Down)
                {
                    answer.Push(r);
                    for (var j = r.Right; j != r; j = j.Right)
                    {
                        cover(j.Column);
                    }
                    search();
                    r = answer.Pop();
                    c = r.Column;

                    for (var j = r.Left; j != r; j = j.Left)
                    {
                        uncover(j.Column);
                    }
                }
                uncover(c);
            }
            return;
        }
    }

}
