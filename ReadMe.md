# Sudoku Solver

(desc.)

## Usage

-- build from soln
-- use executable

cmd line:
SudokuSolver.exe --solve <filename> (or -s)
SudokuSolver.exe --test (or -t)
If run without any arguments the program will prompt the user for input.

## Features

This program will take a Sudoku from a file in the following format:

(example)

It will output the solution to the Sudoku (if one exists) as well as the time it took to solve. The solution will be saved to a file in the following format:

(example)

Although the algorithm used could be extended to solve Sudoku-like grid problems such as 16x16, for the purposes of this it will only accept 9x9 grids.

The can find all valid solutions for a Sudoku puzzle, however since a Sudoku is not considered valid if it does not have only one unique solution, the solver will abort if more than one solution is found and the puzzle will be considered invalid.

## Dancing Links Algorithm

This solver implements an algorithm first described by (knuth guy) in the paper (link to paper). This Dancing Links is an efficient way of implementing the algorithm (also described by Knuth) known as "Algorithm X", an algorithm which finds all solutions to an exact cover problem, which any Sudoku puzzle can be reduced to. 

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
```
Will remove node x from the list, while
```
```
will restore x's position in the list.

The implementation of Algorithm X described above will spend a large amount of time searching the matrix for 1's. To improve the search time, Knuth implemented a sparse matrix where only 1's are stored.

Each node in the matrix points to nodes to the left, right, above, and below it. Each row and column is a circular doubly linked list of nodes. Each column has a special header node which points to the first node in the column, with a unique identifier and which tracks the number of nodes in its own column so that the algorithm can select an available column with the lowest node count to cover, which improves performance.

DLX is built around the idea of using the operation described above to "cover" and "uncover" columns in the matrix, rather than deleting them. Here is a representation of the simple example implemented as the linked list:

(image)

And here it what it will look like after A is covered:

(image)

And then after D and G are covered:

(image)

Eventually this algorithm will find all solutions to any exact cover problem.

### Reducing Sudoku  

To create a sparse matrix to convert a Sudoku into an Exact Cover Problem and solve it with DLX, there are 4 constraints that need to be recognized:
- Position constraint: Only 1 number can occupy a cell
- Row constraint: Only 1 instance of a number can occur in each row
- Column constraint: Only 1 instance of a number can occur in each column
- Square constraint: Only 1 instance of a number can occur in each 3x3 square

The columns of our sparse matrix represent these four constrainst, and the rows represent every possible position for every number.

### Additional Improvement

An additional pre-processing step was added to check if the number of clues (filled in cells on provided Sudoku) is less than 17. This is because it has been (link)proven by (person) that any Sudoku with 16 or less clues does not have a unique solution.

## Testing