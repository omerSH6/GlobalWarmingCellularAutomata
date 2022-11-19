using System.ComponentModel;
using GlobalWarmingCellularAutomata.Automata.Entities;
using GlobalWarmingCellularAutomata.Automata.Entities.Data;
using GlobalWarmingCellularAutomata.Automata.Enums;

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
            CellType cellType = currentCell.cellType;
            CloudsRate clouds = currentCell.clouds;
            TemprateureRate temprateure = currentCell.temperature;
            WindDirection windDirection = currentCell.wind.WindDirection;
            WindForceScale windForceScale = currentCell.wind.WindForceScale;
            AirPollutionRate airPollution = currentCell.airPollution;


            var neighborsWithWindDirectedAtThisCell = GetNeighborsWithWindDirectedAtThisCell(currentCell, cellNeighbors);

            switch (neighborsWithWindDirectedAtThisCell.Count)
            {
                case 0:
                    break;

                case 1:
                    clouds = neighborsWithWindDirectedAtThisCell[0].clouds;
                    temprateure = neighborsWithWindDirectedAtThisCell[0].temperature;
                    windDirection = neighborsWithWindDirectedAtThisCell[0].wind.WindDirection;
                    windForceScale = neighborsWithWindDirectedAtThisCell[0].wind.WindForceScale;
                    airPollution = neighborsWithWindDirectedAtThisCell[0].airPollution;
                    break;

                default:
                    var HigherWindForceScaleDirectedNeighbor = GetHigherWindForceScaleDirectedNeighbor(neighborsWithWindDirectedAtThisCell);
                    windDirection = HigherWindForceScaleDirectedNeighbor.wind.WindDirection;

                   
                    /*
                    clouds = neighborsWithWindDirectedAtThisCell.s
                    temprateure = neighborsWithWindDirectedAtThisCell[0].temperature;
                    windForceScale = neighborsWithWindDirectedAtThisCell[0].wind.WindForceScale;
                    airPollution = neighborsWithWindDirectedAtThisCell[0].airPollution;
                    */

                    break;
            }


            return new Cell(cellType, airPollution, clouds, temprateure, new Wind(windDirection, windForceScale));
        }

        private static Cell GetHigherWindForceScaleDirectedNeighbor(List<Cell> neighborsWithWindDirectedAtThisCell)
        {
            return neighborsWithWindDirectedAtThisCell.OrderBy(cell => cell.wind.WindForceScale).ToList().Last();
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

        private static List<Cell> GetNeighborsWithWindDirectedAtThisCell(Cell currentCell, CellNeighbors cellNeighbors)
        {
            var neighborsWithWindDirectedAtThisCell = new List<Cell>();

            if (cellNeighbors.UpperNeighbor.wind.WindDirection == WindDirection.South)
            {
                neighborsWithWindDirectedAtThisCell.Add(cellNeighbors.UpperNeighbor);
            }

            if (cellNeighbors.LowerNeighbor.wind.WindDirection == WindDirection.North)
            {
                neighborsWithWindDirectedAtThisCell.Add(cellNeighbors.LowerNeighbor);
            }

            if (cellNeighbors.LeftNeighbor.wind.WindDirection == WindDirection.East)
            {
                neighborsWithWindDirectedAtThisCell.Add(cellNeighbors.LeftNeighbor);
            }

            if (cellNeighbors.RightNeighbor.wind.WindDirection == WindDirection.West)
            {
                neighborsWithWindDirectedAtThisCell.Add(cellNeighbors.RightNeighbor);
            }

            return neighborsWithWindDirectedAtThisCell;
        }
    }
}
