using System;
using System.IO;
namespace Sudoku
{
    class Solver
    {
        public void solve_sudoku(SudokuGrid sudoku, string puzzle_name, string output_dir = "")
        {
            if (!sudoku.Valid)
            {
                throw new Exception(("Invalid Sudoku input."));
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var dlx = new DLX(new ColumnNode(-1));
            dlx.createLinkedList(sudoku.Grid);
            var algX = new AlgorithmX(dlx.h);
            algX.search();

            watch.Stop();

            SudokuGrid solution = algX.Final_Solution;
            var elapsedMs = watch.ElapsedMilliseconds;
            if (solution != null)
            {
                Console.WriteLine(string.Format("Puzzle \"{0}\" Solution:", puzzle_name));
                solution.print_grid();
                Console.WriteLine(string.Format("Puzzle \"{0}\" was solved in: {1} milliseconds", puzzle_name, elapsedMs));
                if (!String.IsNullOrWhiteSpace(output_dir))
                {
                    Directory.CreateDirectory(output_dir);
                    var filename = string.Format("{0}.sln.txt", puzzle_name);
                    string path = Path.Combine(output_dir, filename);
                    Console.WriteLine(String.Format("Your solved puzzle has been saved to the file: {0}", Path.GetFullPath(path)));
                    solution.Save_File(path);
                }
            }
            else
            {
                Console.WriteLine("Something went wrong and your puzzle could not be solved. Press any key to exit...");
                Console.ReadLine();
            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser();
            var option = parser.Parse_Args(args);
            try
            {
                if (option == 1)
                {
                    SudokuGrid mySudoku = parser.Parse_File();
                    if (mySudoku == null)
                    {
                        Console.WriteLine("Could not parse provided Sudoku file. Press enter to exit...");
                        Console.ReadLine();
                        return;
                    }
                    var s = new Solver();
                    s.solve_sudoku(mySudoku, parser.puzzle_name, parser.output_dir);
                }
                else if (option == 2)
                {
                    var tester = new Test();
                    tester.run_all_tests();
                    //run test
                }
                else
                {
                    throw new Exception("Failed to parse user input. Expected --solve or --test.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Sudoku Solver completed! Press enter to exit.");
            Console.Read();
        }
    }
}
