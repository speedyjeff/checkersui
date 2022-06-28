using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersUiWf
{
    class MyDataGridView : DataGridView
    {
        public MyDataGridView() 
        { }

        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {
            Moves.NavigateMovesByMouse(new MoveId(e.RowIndex, e.ColumnIndex));
        }
    }
}
