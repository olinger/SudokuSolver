using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    internal class ColumnNode : DataNode
    {
        public int Size { get; set; }
        public ColumnNode(int id) : base(null, id)
        {
            Column = this;
            Size = 0;
        }
    }
}
