using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    internal class DLX
    {
        //root node h
        public ColumnNode h;

        public DLX(ColumnNode h)
        {
            this.h = h;
        }

        class MatrixRow
        {
            public DataNode Position { get; set; }
            public DataNode Row { get; set; }
            public DataNode Column { get; set; }
            public DataNode Square { get; set; }

            public MatrixRow(DataNode pos, DataNode row, DataNode col, DataNode sqr)
            {
                Position = pos;
                Row = row;
                Column = col;
                Square = sqr;
            }
        }
        delegate int position_constraint(int x, int y);
        delegate int column_constriant(int y, int d);
        delegate int row_constraint(int x, int d);
        delegate int square_constraint(int x, int y, int d);
        delegate int row_index(int x, int y, int d);

        //constraints
        position_constraint pos_con = (x, y) => x * 9 + y;
        column_constriant col_con = (y, d) => 162 + y * 9 + d;
        row_constraint row_con = (x, d) => 81 + x * 9 + d;
        square_constraint sqr_con = (x, y, d) => 243 + (3 * (x / 3) + y / 3) * 9 + d;
        row_index row_id = (x, y, d) => x * 81 + y * 9 + d;

        void link_row(MatrixRow d)
        {
            //create linked list of the segments created be each constraint
            //  -> pos -> col -> row -> square -> 
            d.Position.Right = d.Column;
            d.Position.Left = d.Square;
            d.Column.Right = d.Row;
            d.Column.Left = d.Position;
            d.Row.Right = d.Square;
            d.Row.Left = d.Column;
            d.Square.Right = d.Position;
            d.Square.Left = d.Row;
        }
        
        void link_row_to_column(DataNode section)
        {
            //links a section of a row to its column
            var col = section.Column;
            col.Size += 1;
            section.Down = col;
            section.Up = col.Up;
            col.Up.Down = section;
            col.Up = section;
        }

        void form_links(List<ColumnNode> cols, int x, int y, int d)
        {
            //form links for all rows and columns
            var pos = new DataNode(cols[pos_con(x, y)], row_id(x, y, d));
            var row = new DataNode(cols[row_con(x, d)], row_id(x, y, d));
            var col = new DataNode(cols[col_con(y, d)], row_id(x, y, d));
            var sqr = new DataNode(cols[sqr_con(x, y, d)], row_id(x, y, d));

            var matrix_row = new MatrixRow(pos, row, col, sqr);
            link_row(matrix_row);
            link_row_to_column(matrix_row.Position);
            link_row_to_column(matrix_row.Row);
            link_row_to_column(matrix_row.Column);
            link_row_to_column(matrix_row.Square);
        }

        public ColumnNode createLinkedList(int[,] grid)
        {
            //create a circular doubly linked link representing the given sudoku
            var columns = new List<ColumnNode>();
            for(int col_ind = 0; col_ind < 324; col_ind++)
            {
                ColumnNode col = new ColumnNode(col_ind);
                col.Right = h;
                col.Left = h.Left;
                h.Left.Right = col;
                h.Left = col;
                columns.Add(col);
            }

            for(int x = 0; x < 9; x++)
            {
                for(int y = 0; y < 9; y++)
                {
                    if(grid[x, y] == 0)
                    {
                        for(int d = 0; d < 9; d++)
                        {
                            //this cell is blank so add all possibilities
                            form_links(columns, x, y, d);
                        }
                    }
                    else
                    {
                        //clue exists in this location so add only for that possibility
                        int d = grid[x, y] - 1;
                        form_links(columns, x, y, d);
                    }
                }
            }
            return h;
        }
    }

}
