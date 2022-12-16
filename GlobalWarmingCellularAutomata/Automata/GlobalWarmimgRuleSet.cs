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
            var newCell = new Cell(currentCell.cellType, currentCell.airPollution, currentCell.clouds, currentCell.temperature, currentCell.wind, currentCell.HotDaysCounter, currentCell.ColdDaysCounter, currentCell.RainDaysCounter);

            var neighborsWithWindDirectedAtThisCell = GetNeighborsWithWindDirectedAtThisCell(currentCell, cellNeighbors);

            switch (neighborsWithWindDirectedAtThisCell.Count)
            {
                case 0:
                    break;

                case 1:
                    newCell.clouds = neighborsWithWindDirectedAtThisCell[0].clouds;
                    newCell.temperature = neighborsWithWindDirectedAtThisCell[0].temperature;
                    newCell.wind.WindDirection = neighborsWithWindDirectedAtThisCell[0].wind.WindDirection;
                    newCell.wind.WindForceScale = neighborsWithWindDirectedAtThisCell[0].wind.WindForceScale;
                    newCell.airPollution = neighborsWithWindDirectedAtThisCell[0].airPollution;
                    break;

                default:
                    var HigherWindForceScaleDirectedNeighbor = GetHigherWindForceScaleDirectedNeighbor(neighborsWithWindDirectedAtThisCell);
                    newCell.wind.WindDirection = HigherWindForceScaleDirectedNeighbor.wind.WindDirection;

                    newCell.clouds = (CloudsRate)GetAverageEnumValue(neighborsWithWindDirectedAtThisCell.Select(neighbor => neighbor.clouds).ToList());
                    newCell.temperature = (TemperatureRate)GetAverageEnumValue(neighborsWithWindDirectedAtThisCell.Select(neighbor => neighbor.temperature).ToList());
                    newCell.wind.WindForceScale = (WindForceScale)GetAverageEnumValue(neighborsWithWindDirectedAtThisCell.Select(neighbor => neighbor.wind.WindForceScale).ToList());
                    newCell.airPollution = (AirPollutionRate)GetAverageEnumValue(neighborsWithWindDirectedAtThisCell.Select(neighbor => neighbor.airPollution).ToList());

                    if(newCell.wind.WindForceScale == WindForceScale.ExtreamWind)
                    {
                        newCell.wind.WindForceScale--;
                    }

                    break;
            }

            if (newCell.cellType == CellType.Sea || newCell.cellType == CellType.Glaciers)
            {
                if(newCell.temperature == TemperatureRate.ExtreamHot)
                {
                    newCell.temperature--;
                }
            }

            switch (newCell.clouds)
            {
                case CloudsRate.RainClouds:
                    newCell.RainDaysCounter++;
                    break;

                case CloudsRate.Cloudy:
                    if(newCell.temperature == TemperatureRate.Freezing)
                    {
                        newCell.clouds = CloudsRate.RainClouds;
                    }
                    break;

                case CloudsRate.None: 
                    if(newCell.RainDaysCounter > 0)
                    {
                        newCell.RainDaysCounter--;
                    }
                    break;
            }

            if(newCell.airPollution == AirPollutionRate.High && newCell.temperature != TemperatureRate.ExtreamHot)
            {
                newCell.temperature++;
            }

            if(newCell.temperature == TemperatureRate.ExtreamHot || newCell.temperature == TemperatureRate.Hot )
            {
                newCell.HotDaysCounter++;

                if(newCell.ColdDaysCounter > 0)
                {
                    newCell.ColdDaysCounter--;
                }
            }else if(newCell.temperature == TemperatureRate.Freezing )
            {
                newCell.ColdDaysCounter++;
            }

            switch (newCell.cellType)
            {
                case CellType.Sea:
                    if(newCell.HotDaysCounter >= 100)
                    {
                        newCell.HotDaysCounter = 0;
                        newCell.cellType = CellType.Land;
                    }

                    break;

                case CellType.Glaciers:
                    if (newCell.HotDaysCounter >= 50)
                    {
                        newCell.cellType = CellType.Sea;
                    }

                    break;

                case CellType.Land:
                case CellType.City:
                    if (newCell.RainDaysCounter >= 150)
                    {
                        newCell.cellType = CellType.Sea;
                    }

                    break;
            }

            return newCell;
        }

        private static Cell GetHigherWindForceScaleDirectedNeighbor(List<Cell> neighborsWithWindDirectedAtThisCell)
        {
            return neighborsWithWindDirectedAtThisCell.OrderBy(cell => cell.wind.WindForceScale).ToList().Last();
        }

        private static int GetAverageEnumValue<T>(List<T> enumList) where T: Enum
        {
            return (int)enumList.Average(enumValue => Convert.ToInt32(enumValue));
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
            var (UpperNeighborRow, UpperNeighborColumn) = (row - 1, column);
            var (LowerNeighborRow, LowerNeighborColumn) = (row + 1, column);
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
                LeftNeighborColumn = totalcolumns - 1;

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
