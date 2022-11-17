using GlobalWarmingCellularAutomata.Automata.Entities;

namespace GlobalWarmingCellularAutomata.Automata
{
    internal class GlobalWarmimgRuleSet
    {
        private readonly Cell[,] cellsGrid;
        private readonly int rows;
        private readonly int columns;

        public GlobalWarmimgRuleSet(Cell[,] cellsGrid) 
        {
            this.cellsGrid = cellsGrid;
            rows = cellsGrid.GetLength(0);
            columns = cellsGrid.GetLength(1);
        }

        public void Tick()
        {
            var updatedCellsGrid = GetUpdatedCellsGrid();
            UpdateCellsGrid(updatedCellsGrid);
        }

        private Cell[,] GetUpdatedCellsGrid() 
        {
            var updatedCellsGrid = new Cell[rows, columns];


            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var cellNeighbors = GetCellNeighbors(i, j, rows, columns);
                    updatedCellsGrid[i, j] = GetUpdatedCell(cellsGrid[i,j], cellNeighbors);
                }
            }

            return updatedCellsGrid;
        }

        private Cell GetUpdatedCell(Cell currentCell, CellNeighbors cellNeighbors)
        {
            return new Cell();
        }

        private void UpdateCellsGrid(Cell[,] updatedCellsGrid)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    cellsGrid[i,j] = updatedCellsGrid[i,j];
                }
            }

        }

        private CellNeighbors GetCellNeighbors(int row, int column, int totalRows, int totalcolumns)
        {
            var cellNeighbors = new CellNeighbors();
            var (UpperNeighborRow, UpperNeighborColumn) = (row + 1, column);
            var (LowerNeighborRow, LowerNeighborColumn) = (row - 1, column);
            var (RightNeighborRow, RightNeighborColumn) = (row, column + 1);
            var (LeftNeighborRow, LeftNeighborColumn) = (row, column - 1);

            // end cases
            if (row == 0)
            {
                UpperNeighborRow = totalRows - 1;

            }else if (row == totalRows - 1)
            {
                LowerNeighborRow = 0;
            }

            if (column == 0)
            {
                LeftNeighborRow = totalcolumns - 1;

            }
            else if (column == totalcolumns - 1)
            {
                RightNeighborColumn = 0;
            }

            cellNeighbors.UpperNeighbor = cellsGrid[UpperNeighborRow, UpperNeighborColumn];
            cellNeighbors.LowerNeighbor = cellsGrid[LowerNeighborRow, LowerNeighborColumn];
            cellNeighbors.RightNeighbor = cellsGrid[RightNeighborRow, RightNeighborColumn];
            cellNeighbors.LeftNeighbor = cellsGrid[LeftNeighborRow, LeftNeighborColumn];
            return cellNeighbors;
        }
    }
}
