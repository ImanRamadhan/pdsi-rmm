using DevExpress.Web;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxGridView.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Kairos.Library.ControlHelper
{
    class ASPxGridViewHelper
    {
        ASPxGridView grid;
        Dictionary<GridViewDataColumn, TableCell> mergedCells = new Dictionary<GridViewDataColumn, TableCell>();
        Dictionary<TableCell, int> cellRowSpans = new Dictionary<TableCell, int>();

        public ASPxGridViewHelper(ASPxGridView grid)
        {
            this.grid = grid;
            //Grid.HtmlRowCreated += new ASPxGridViewTableRowEventHandler(grid_HtmlRowCreated);
            Grid.HtmlDataCellPrepared += new ASPxGridViewTableDataCellEventHandler(grid_HtmlDataCellPrepared);
        }

        public ASPxGridView Grid { get { return grid; } }
        void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            //add the attribute that will be used to find which column the cell belongs to
            e.Cell.Attributes.Add("ci", e.DataColumn.VisibleIndex.ToString());

            if (cellRowSpans.ContainsKey(e.Cell))
            {
                e.Cell.RowSpan = cellRowSpans[e.Cell];
            }
        }
        void grid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (Grid.GetRowLevel(e.VisibleIndex) != Grid.GroupCount) return;
            for (int i = e.Row.Cells.Count - 1; i >= 0; i--)
            {
                //DevExpress.Web.ASPxGridView.Rendering.GridViewTableDataCell dataCell = e.Row.Cells[i] as DevExpress.Web.ASPxGridView.Rendering.GridViewTableDataCell;
                GridViewTableDataCell dataCell = e.Row.Cells[i] as GridViewTableDataCell;
                if (dataCell != null)
                {
                    MergeCells(dataCell.DataColumn, e.VisibleIndex, dataCell);
                }
            }
        }

        public void MergeCells(GridViewDataColumn column, int visibleIndex, TableCell cell)
        {
            bool isNextTheSame = IsNextRowHasSameData(column, visibleIndex);
            if (isNextTheSame)
            {
                ((TableRow)cell.Parent).Cells.Remove(cell);
                if (mergedCells.ContainsKey(column))
                {
                    TableCell mergedCell = mergedCells[column];
                    if (!cellRowSpans.ContainsKey(mergedCell))
                    {
                        cellRowSpans[mergedCell] = 1;
                    }
                    cellRowSpans[mergedCell] = cellRowSpans[mergedCell] + 1;
                }
            }
            if (IsPrevRowHasSameData(column, visibleIndex))
            {
                if (!mergedCells.ContainsKey(column))
                {
                    mergedCells[column] = cell;
                }
            }
            if (!isNextTheSame)
            {
                mergedCells.Remove(column);
            }
        }
        bool IsNextRowHasSameData(GridViewDataColumn column, int visibleIndex)
        {
            //is it the last visible row
            if (visibleIndex >= Grid.VisibleRowCount - 1)
                return false;

            return IsSameData(column.FieldName, visibleIndex, visibleIndex + 1);
        }
        bool IsPrevRowHasSameData(GridViewDataColumn column, int visibleIndex)
        {
            ASPxGridView grid = column.Grid;
            //is it the first visible row
            if (visibleIndex <= Grid.VisibleStartIndex)
                return false;

            return IsSameData(column.FieldName, visibleIndex, visibleIndex - 1);
        }
        bool IsSameData(string fieldName, int visibleIndex1, int visibleIndex2)
        {
            // is it a group row?
            if (Grid.GetRowLevel(visibleIndex2) != Grid.GroupCount)
                return false;

            return object.Equals(Grid.GetRowValues(visibleIndex1, fieldName), Grid.GetRowValues(visibleIndex2, fieldName));
        }
    }
}
