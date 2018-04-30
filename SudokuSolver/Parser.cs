using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Parser
    {
        public string puzzle_name = "";
        public string file_name = "";
        public string output_dir = "";

        public int Parse_Args(string[] args)
        {
            if (args.Length == 0)
            {
                //prompt user for input
                while (true)
                {

                    Console.WriteLine("Welcome to the Sudoku solver. Here are some options you can choose from: ");
                    Console.WriteLine("solve --- Solves a Sudoku puzzle contained in a provided input file.");
                    Console.WriteLine("test --- Runs the Sudoku solver against the test inputs contained in this project.");
                    Console.Write("What would you like to do?: ");
                    var action = Console.ReadLine();
                    if (action == "solve")
                    {
                        Console.Write("File path of Sudoku to be solved: ");
                        file_name = Console.ReadLine();
                        Console.Write("Output directory to save solution (leave blank to not save): ");
                        output_dir = Console.ReadLine();
                        return 1;
                    }
                    if (action == "test")
                    {
                        return 2;
                    }
                    else
                    {
                        Console.WriteLine("Sorry, you must enter one of the required options, test or solve. Press any key to try again.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
            }
            //parse command line args
            else
            {
                try
                {
                    if ((args[0] == "--solve" || args[0] == "-s") && args.Length >= 2)
                    {
                        file_name = args[1];
                        if(args.Length == 4)
                        {
                            if(args[2] == "--output" || args[2] == "-o")
                            {
                                output_dir = args[3];
                            }
                        }
                        return 1;
                    }
                    else if(args[0] == "--test" || args[0] == "-t" && args.Length == 1)
                    {
                        return 2;
                    }
                }
                catch
                {
                    Console.WriteLine("Error: Could not parse command line input. Accepted options: '--solve <filename>' and (optional) '--output <filename>', or '--test'. Exiting.");
                    return -1;
                }
            }
            return -1;
        }

        public SudokuGrid Parse_File()
        {
            int[,] sudoku_grid = new int[9, 9];
            if (File.Exists(file_name))
            {
                puzzle_name = Path.GetFileNameWithoutExtension(file_name);
                var lines = File.ReadAllLines(file_name);
                int x = 0;
                int y = 0;
                string allowed = "X123456789";
                foreach (var line in lines)
                {
                    if (line.Length != 9)
                    {
                        Console.WriteLine("Parsing error: Line must contain 9 characters");
                        return null;
                    }
                    foreach (var c in line)
                    {
                        if (!allowed.Contains(c))
                        {
                            //illegal character found
                            Console.WriteLine(String.Format("Parsing error: Illegal character {0} found in input", c));
                            return null;
                        }
                        if (c == 'X')
                        {
                            sudoku_grid[x, y] = 0;
                        }
                        else
                        {
                            sudoku_grid[x, y] = c - '0';
                        }
                        x++;
                    }
                    x = 0;
                    y++;
                }
                return new SudokuGrid(sudoku_grid);
            }
            else
            {
                Console.WriteLine("Error: File not found!");
                return null;
            }
        }
    }
}
