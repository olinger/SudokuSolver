# Sudoku Solver

A C# implementation of the dancing links exact cover solving algorithm applied to Sudokus.

## Usage

You can build and run this application using the Visual Studio solution provided in the repo here or you can run the executable found here.

There are two modes the program runs in, solve and test. When running in solve mode you must provide the path to a Sudoku file in the valid format. There is also an optional parameter for an output directory. This will save your solution as a text file in the provided directory. If you don't provide an output directory the output will not be saved.

Examples
```
./SudokuSolver.exe --solve Tests\easy.txt --output Solutions/
```
```
./SudokuSolver.exe --solve "C:\code\Sudoku"
```
```
./SudokuSolver.exe --test
```
When running in test mode, the provided Tests folder must be in the root directory where you are running from.

If you run the program without providing any parameters it will prompt you for user input.

## Features

This program will take a Sudoku from a file in the following format:
```
XXX15XX7X
1X6XXX82X
3XX86XX4X
9XX4XX567
XX47X83XX
732XX6XX4
X4XX81XX9
X17XXX2X8
X5XX37XXX
```

It will output the solution to the Sudoku (if one exists) as well as the time (in milliseconds) that it took to solve. The solution will be saved to a file (if output parameter is provided) in the following format:
```
428159673
196374825
375862941
981423567
564718392
732596184
243681759
617945238
859237416
```

Although the algorithm used could be extended to solve Sudoku-like grid problems such as 16x16, for the purposes of this it will only accept 9x9 grids.

The algoritm is capable of finding all valid solutions for a Sudoku puzzle, however since a Sudoku is not considered valid if it does not have only one unique solution, the solver will abort if more than one solution is found and the puzzle will be considered invalid.

When running in test mode the program will run through 9 test files contained in the Test folder, some which are designed to pass and some designed to fail, and output the result of each test.

## Dancing Links Algorithm

This solver implements an algorithm first described in the paper [Dancing links](https://arxiv.org/abs/cs/0011047) by Donald Knuth. This Dancing Links is an efficient way of implementing the algorithm (also described by Knuth) known as "Algorithm X", an algorithm which finds all solutions to an exact cover problem, which any Sudoku puzzle can be reduced to. 

### Exact Cover
Given a collection 'S' of subsets of a set 'X', an exact cover is a subcollection S* of S such that each element is X is contained in exactly one subset in S*.

More specific to our application, we are trying to find the exact cover given a binary matrix that represents all possible values for our Sudoku puzzle. The matrix to represent the Sudoku is massive, but here is a small example:

```
0 0 1 0 1 1 0
1 0 0 1 0 0 1
0 1 1 0 0 1 0
1 0 0 1 0 0 0
0 1 0 0 0 0 1
0 0 0 1 1 0 1
```

The exact cover of this matrix is a set of one or more rows in which only one '1' appears in each column. For this example the exact cover is:

```
0 0 1 0 1 1 0
1 0 0 1 0 0 1
0 1 0 0 0 0 1
```

Which are rows 1, 4 and 5 of our original matrix

### Algorithm X

Algorithm X is the obvious trial-and-error approach to solving the exact cover problem. It is a straightforward depth-first search with back tracking. The steps are as follows:

1. If the matrix A has no columns, the current partial solution is a valid solution; terminate successfully.
2. Otherwise choose a column c (deterministically).
3. Choose a row r such that A<sub>r, c</sub> = 1 (nondeterministically).
4. Include row r in the partial solution.
5. For each column j such that A<sub>r, j</sub> = 1,
	- for each row i such that A<sub>i, j</sub> = 1,
		- delete row i from matrix A.
	- delete column j from matrix A.
6. Repeat this algorithm recursively on the reduced matrix A.

This algorithm can be implemented more efficiently using the Dancing Links technique described below.

### Dancing Links

The idea of Dancing Links (DLX) is based on the observation that in a circular double linked list of nodes,
```
x.left.right <- x.right;
x.right.left <- x.left;
```
Will remove node x from the list, while
```
x.left.right <- x;
x.right.left <- x;
```
will restore x's position in the list.

The implementation of Algorithm X described above will spend a large amount of time searching the matrix for 1's. To improve the search time, Knuth implemented a sparse matrix where only 1's are stored.

Each node in the matrix points to nodes to the left, right, above, and below it. Each row and column is a circular doubly linked list of nodes. Each column has a special header node which points to the first node in the column, with a unique identifier and which tracks the number of nodes in its own column so that the algorithm can select an available column with the lowest node count to cover, which improves performance.

DLX is built around the idea of using the operation described above to "cover" and "uncover" columns in the matrix, rather than deleting them. Here is a representation of the simple example implemented as the linked list:

![Dancing Links](/img/links.png)

And here it what it will look like after A is covered:

![Cover A](/img/links.png)

And then after D and G are covered:

![Cover D G](/img/links.png)

Eventually this algorithm will find all solutions to any exact cover problem.

### Reducing Sudoku  

To create a sparse matrix to convert a Sudoku into an Exact Cover Problem and solve it with DLX, there are 4 constraints that need to be recognized:
- Position constraint: Only 1 number can occupy a cell
- Row constraint: Only 1 instance of a number can occur in each row
- Column constraint: Only 1 instance of a number can occur in each column
- Square constraint: Only 1 instance of a number can occur in each 3x3 square

The columns of our sparse matrix represent these four constrainst, and the rows represent every possible position for every number.

### Additional Improvement

An additional pre-processing step was added to check if the number of clues (filled in cells on provided Sudoku) is less than 17. This is because it was shown in 2012 by Gary McGuire at Unviersity College Dublin in [this paper](https://arxiv.org/abs/1201.0749) that any Sudoku with 16 or less clues cannot have a unique solution. 

## Testing

There are 9 tests provided in the Tests folder. They perform the following checks:

- easy & very hard - Two valid Sudokus that will return as solvable
- invalid chars - An input file with invalid characters in it
- invalid clues -  A Sudoku with less than 17 clues (therefore not valid and the algorithm will not attempt)
- invalid size 1 & 2 - Two input files with invalid sized rows / columns
- not unique - A sudoku which has more than one solution and is therefore not valid
- repear numbers - A sudoku with repeating numbers in a column / row / square therefore not valid

## Results

The solutions to the provided puzzles are contained text files in the Solutions folder. Each puzzle was solved in 4 milliseconds. Here is each solution:

### Puzzle 1
```
4 2 8 1 5 9 6 7 3
1 9 6 3 7 4 8 2 5
3 7 5 8 6 2 9 4 1
9 8 1 4 2 3 5 6 7
5 6 4 7 1 8 3 9 2
7 3 2 5 9 6 1 8 4
2 4 3 6 8 1 7 5 9
6 1 7 9 4 5 2 3 8
8 5 9 2 3 7 4 1 6
```

### Puzzle 2
```
9 2 1 7 6 8 5 4 3
4 6 3 5 1 9 8 7 2
8 7 5 4 3 2 9 6 1
5 9 4 2 8 3 6 1 7
7 1 2 6 4 5 3 8 9
6 3 8 9 7 1 4 2 5
3 4 9 8 2 7 1 5 6
2 5 6 1 9 4 7 3 8
1 8 7 3 5 6 2 9 4
```

### Puzzle 3
```
9 2 1 7 6 8 5 4 3
4 6 3 5 1 9 8 7 2
8 7 5 4 3 2 9 6 1
5 9 4 2 8 3 6 1 7
7 1 2 6 4 5 3 8 9
6 3 8 9 7 1 4 2 5
3 4 9 8 2 7 1 5 6
2 5 6 1 9 4 7 3 8
1 8 7 3 5 6 2 9 4
```

### Puzzle 4
```
5 3 4 6 7 8 9 1 2
6 7 2 1 9 5 3 4 8
1 9 8 3 4 2 5 6 7
8 5 9 7 6 1 4 2 3
4 2 6 8 5 3 7 9 1
7 1 3 9 2 4 8 5 6
9 6 1 5 3 7 2 8 4
2 8 7 4 1 9 6 3 5
3 4 5 2 8 6 1 7 9
```

### Puzzle 5
```
9 1 5 3 4 8 6 2 7
3 4 8 6 7 2 1 5 9
7 6 2 1 5 9 4 8 3
6 9 7 8 1 4 2 3 5
5 3 4 7 2 6 9 1 8
8 2 1 9 3 5 7 6 4
4 7 6 5 8 1 3 9 2
1 8 3 2 9 7 5 4 6
2 5 9 4 6 3 8 7 1
```