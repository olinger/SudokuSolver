using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sudoku
{
    class Test
    {
        public string test_dir = "Tests";
      
        bool run_test(string file, bool expected_result)
        {
            Console.WriteLine();
            try
            {
                Console.WriteLine("Running test file: " + file);
                var path = Path.Combine(test_dir, file);
                string expect = expected_result ? "Succeed" : "Fail";
                Console.WriteLine("Expected result: " + expect);
                var parser = new Parser
                {
                    file_name = path
                };
                var sudoku = parser.Parse_File();
                if (sudoku == null)
                {
                    Console.WriteLine("Could not parse provided Sudoku file.");
                    return false;
                }
                var s = new Solver();
                s.solve_sudoku(sudoku, parser.puzzle_name, parser.output_dir);
                return true;
            }
            catch
            {
                return false;
            }


        }
        public void run_all_tests()
        {
            Console.WriteLine("##### Running Sudoku Solver in Test Mode #####");
            int total_tests = 9;
            int successful_tests = 0;
            //run easy test
            if (run_test("easy.txt", true))
            {
                successful_tests++;
                Console.WriteLine("Test successful!");
            };

            //run very hard test
            if (run_test("very_hard.txt", true))
            {
                successful_tests++;
                Console.WriteLine("Test successful!");
            };

            //run empty test
            if (!run_test("empty.txt", false))
            {
                successful_tests++;
                Console.WriteLine("Test successful!");
            };

            //run invalid chars test
            if (!run_test("invalid_chars.txt", false))
            {
                successful_tests++;
                Console.WriteLine("Test successful!");
            };

            //run invalid clues test
            if (!run_test("invalid_clues.txt", false))
            {
                successful_tests++;
                Console.WriteLine("Test successful!");
            };

            //run invalid size test 1
            if (!run_test("invalid_size1.txt", false))
            {
                successful_tests++;
                Console.WriteLine("Test successful!");
            };

            //run invalid size test 2
            if (!run_test("invalid_size2.txt", false))
            {
                successful_tests++;
                Console.WriteLine("Test successful!");
            };

            //run not unique test
            if (!run_test("not_unique.txt", false))
            {
                successful_tests++;
                Console.WriteLine("Test successful!");
            };
            //run not unique test
            if (!run_test("repeat_numbers.txt", false))
            {
                successful_tests++;
                Console.WriteLine("Test successful!");
            };

            Console.WriteLine();
            Console.WriteLine(string.Format("###### {0} out of {1} tests completed successfully. ######", successful_tests, total_tests));
        }
    }
}
