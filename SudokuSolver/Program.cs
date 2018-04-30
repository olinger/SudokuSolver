#if false
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sudoku
{
    public class Program
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
                    
                    solve_sudoku(mySudoku, parser.puzzle_name, parser.output_dir);
                }
                else if (option == 2)
                {
                    //run test
                }
                else
                {
                    Console.WriteLine("Failed to parse user input. Expected --solve or --test. Press enter to exit.");
                    Console.ReadLine();
                    return;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Sudoku Solver completed! Press enter to exit.");
            Console.Read();
        }
    }
}
#endif