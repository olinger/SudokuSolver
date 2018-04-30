using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    internal class DataNode
    {
        public ColumnNode Column { get; set; }
        public int ID { get; set; }
        public DataNode Left { get; set; }
        public DataNode Right { get; set; }
        public DataNode Up { get; set; }
        public DataNode Down { get; set; }

        public DataNode(ColumnNode column, int id)
        {
            Column = column;
            ID = id;
            Left = this;
            Right = this;
            Up = this;
            Down = this;

        }
    }
}
