using System;
using System.Collections.Generic;

namespace SudokuSolver {

    //Sudoku solver class:
    public class Solver {

        #region Public interfaces

        /* ISolverInterface ====================================================
         * 
         *  An interface for the solver to access digit data from the puzzle
         *  grid in the UI.
         * 
         * ------------------------------------------------------------------ */
        public interface ISolverInterface {

            int GetCell(int RowIndex, int ColumnIndex);
            void SetCell(int RowIndex, int ColumnIndex, int CellValue);
        }
        // ---------------------------------------------------------------------

        #endregion

        #region Private members

        //Interface instance to access the puzzle grid in the UI.
        private ISolverInterface myInterface;

        //An array to represent the puzzel grid.
        private List<int>[,] myPuzzleArray;

        #endregion

        #region Constructors

        /* Solver ==============================================================
         * 
         *  Solver class constructor.
         * 
         *  Parameters:
         * 
         *      theInterface
         *      Interface instance to access cell digit data from the UI.
         * 
         *  Results:
         * 
         *      This constructor sets the interface instance to use when
         *  getting and setting cell digit data in the UI.
         * 
         * ------------------------------------------------------------------ */
        public Solver(ISolverInterface theInterface) {

            //Set puzzle grid UI access interface.
            myInterface = theInterface;
        }
        // ---------------------------------------------------------------------

        #endregion

        #region Public methods

        /* Setup ===============================================================
         * 
         *  Initialize the solver. 
         *
         *  Results:
         *
         *      This routine sets up the solver to begin solving a puzzle. 
         *  An array is created to represent the puzzle grid. Each cell in
         *  the grid is represented by a list of digits 1 - 9. The initial
         *  digits are then read from the puzzle grid in the UI. As the solver
         *  runs, digits will be removed from each cell's list until a cell
         *  only has one digit left. When all cells have only one digit, the
         *  puzzle is solved.
         * 
         * ------------------------------------------------------------------ */
        public void Setup() {

            //Loop counters.
            int xLoop;
            int yLoop;

            //Var to get initial digits from puzzle grid.
            int startingNum;

            //Init puzzle array.
            myPuzzleArray = new List<int>[9, 9];

            //Create initial array of possible number lists.
            for (xLoop = 0; xLoop < 9; xLoop++) {
                for (yLoop = 0; yLoop < 9; yLoop++) {
                    myPuzzleArray[xLoop, yLoop] = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                }
            }

            //Set values for cells that have already been solved.
            for (xLoop = 0; xLoop < 9; xLoop++) {
                for (yLoop = 0; yLoop < 9; yLoop++) {
                    startingNum = myInterface.GetCell(xLoop, yLoop);
                    if (startingNum != 0)
                        myPuzzleArray[xLoop, yLoop] = new List<int>() { startingNum };
                }
            }
        }
        // ---------------------------------------------------------------------

        /* SolveLoop -----------------------------------------------------------
         * 
         *  Solve the puzzle.
         *
         *  Results:
         *
         *  This routine loops through each cell performing two checks to
         *  eliminate possible numbers from unsolved cells. When there is
         *  only one number left in the list of possible numbers, then the
         *  cell is solved. 
         *
         * ------------------------------------------------------------------ */
        public void SolveLoop() {

            //Flag to check when the puzzle is solved.
            bool solvedCheck;

            //Calc vars.
            bool check1;
            bool check2;

            //Loop counters.
            int xLoop;
            int yLoop;

            do {

                //Init check.
                solvedCheck = false;

                //For all cells in the grid...
                for (xLoop = 0; xLoop < 9; xLoop++) {
                    for (yLoop = 0; yLoop < 9; yLoop++) {

                        //Init checks.
                        check1 = false;
                        check2 = false;

                        //If cell is unsolved, run firt check routine.
                        if (myPuzzleArray[xLoop, yLoop].Count > 1) {
                            check1 = checkCell1(xLoop, yLoop);
                        }

                        //If cell is still unsolved, run second check routine.
                        if (myPuzzleArray[xLoop, yLoop].Count > 1) {
                            check2 = checkCell2(xLoop, yLoop);
                        }

                        //Check for complete.
                        solvedCheck |= (check1 || check2);
                    }
                }

            //Go until both checks find nothing to change in all cells.
            } while (solvedCheck) ;
        }
        // ---------------------------------------------------------------------

        #endregion

        #region Private methods

        /* checkCell1 ==========================================================
         * 
         *  Check a cell's row, column and block for digits to eliminate.
         *
         *  Parameters:
         * 
         *      rowInd,
         *      colInd
         *      The row and column index of the cell to check.
         * 
         *  Results:
         * 
         *      An empty puzzle cell starts as a list of digits 1-9. Any
         *  cells in the same row, column, or block as a cell that are
         *  already solved can be used to eliminate possible values for
         *  the unsolved cell. Those values are removed from the cell's
         *  list of possible digits by this function. When there is only
         *  one possibility left, the cell has been solved.
         * 
         * ------------------------------------------------------------------ */
        private bool checkCell1(int rowInd, int colInd) {

            //A loop counter.
            int xLoop;

            //The result of this function.
            bool retVal;

            //The list of possible values for this cell.
            List<int> cellNums;

            //Calc vars.
            int t1;
            int t2;

            //Indeces to check the cell's block.
            int blockRow;
            int blockCol;

            //Init return.
            retVal = false;

            //Start at upper left corner of the cell's block.
            blockRow = rowInd - (rowInd % 3);
            blockCol = colInd - (colInd % 3);

            //Get the cell's list.
            cellNums = myPuzzleArray[rowInd, colInd];

            //For all 9 cells in the same row, column and block...
            for (xLoop = 0; xLoop < 9; xLoop++) {

                //Check row cells.
                if (xLoop != colInd)
                    removeCellNum(getCellNum(myPuzzleArray[rowInd, xLoop]), cellNums);

                //Check column cells.
                if (xLoop != rowInd)
                    removeCellNum(getCellNum(myPuzzleArray[xLoop, colInd]), cellNums);

                //Check block cells.
                t1 = (blockRow + (xLoop % 3));
                t2 = (blockCol + (xLoop / 3));
                if ((t1 != rowInd) || (t2 != colInd))
                    removeCellNum(getCellNum(myPuzzleArray[t1, t2]), cellNums);

                //If we have eliminated all but one possible number...
                if (cellNums.Count <= 1) {

                    //Show the number in the puzzle grid.
                    myInterface.SetCell(rowInd, colInd, getCellNum(cellNums));

                    //Set to return TRUE.
                    retVal = true;

                    //And we're done.
                    break;
                }

            }

            //Return the result.
            return (retVal);
        }
        // ---------------------------------------------------------------------

        /* checkCell2 ==========================================================
         * 
         *  Check to see if any of the possible digits for a cell only appear 
         *  in that cell for a row, column, or block.
         *
         *  Parameters:
         * 
         *      rowInd,
         *      colInd
         *      The row and column index of the cell to check.
         * 
         *  Results:
         *
         *      If a digit needed to complete a row, column, or block only
         *  appears in one cell in that row, column, or block, then that
         *  cell must be that digit and all others can be eliminated.
         * 
         * ------------------------------------------------------------------ */
        private bool checkCell2(int rowInd, int colInd) {

            //Var to get the list of possible digits for the cell.
            List<int> cellList;

            //A loop counter.
            int xLoop;

            //Flags to check row, column, and block.
            bool rowCheck;
            bool colCheck;
            bool blockCheck;

            //The result of this function.
            bool retVal;

            //Calc vars.
            int t1;
            int t2;

            //Indeces to check the cell's block.
            int blockRow;
            int blockCol;

            //Get the cell's list.
            cellList = myPuzzleArray[rowInd, colInd];

            //Init return.
            retVal = false;

            //Start at upper left corner of the cell's block.
            blockRow = rowInd / 3; //- (rowInd % 3);
            blockCol = colInd / 3; //- (colInd % 3);

            //For all possible digits in the cell...
            foreach (int cellNum in cellList) {

                //Init check flags.
                rowCheck = true;
                colCheck = true;
                blockCheck = true;

                //For all cells in the same row, column, and block...
                for (xLoop = 0; xLoop < 9; xLoop++) {

                    //Check row cells.
                    if (xLoop != colInd)
                        rowCheck &= !(myPuzzleArray[rowInd, xLoop].Contains(cellNum));

                    //Check column cells.
                    if (xLoop != rowInd)
                        colCheck &= !(myPuzzleArray[xLoop, colInd].Contains(cellNum));

                    //Check block cells.
                    t1 = ((blockRow * 3) + (xLoop % 3));
                    t2 = ((blockCol * 3)+ (xLoop / 3));
                    if ((t1 != rowInd) || (t2 != colInd))
                        blockCheck &= !(myPuzzleArray[t1, t2].Contains(cellNum));
                }

                //If any of the checks give us a result...
                if (rowCheck || colCheck || blockCheck) {

                    //Then we can solve the cell.
                    myPuzzleArray[rowInd, colInd] = new List<int>() { cellNum };

                    //Show the number in the puzzle grid.
                    myInterface.SetCell(rowInd, colInd, cellNum);

                    //Set to return TRUE.
                    retVal = true;

                    //And we're done.
                    break;
                }
            }

            //Return the result.
            return (retVal);
        }
        // ---------------------------------------------------------------------

        /* getCellNum ==========================================================
         * 
         *  Return the last remaining digit for a cell.
         * 
         *  Parameters:
         * 
         *      myList
         *      The list of possible digits for a cell.
         * 
         *  Returns:
         * 
         *      int
         *      The last remaining digit for a cell, or '0' if there is more
         *  than one possilble digit left.
         * 
         *  Results:
         * 
         *      If there is only one digit left in the cell's list of possible
         *  digits, then that digit is returned. Otherwise, 0 is returned.
         * 
         * ------------------------------------------------------------------ */
        private int getCellNum(List<int> myList) {

            //the result of this function.
            int retVal;

            //Init return.
            retVal = 0;

            //If the cell only has one digit left, then return that digit.
            if (myList.Count == 1) {
                retVal = myList[0];
            }

            //Return the result.
            return (retVal);
        }
        // ---------------------------------------------------------------------

        /* removeCellNum =======================================================
         * 
         *  Remove a digit from a cell's list of possible digits.
         * 
         *  Parameters:
         * 
         *      theNum
         *      The digit to remove.
         * 
         *      cellList
         *      The list to remove the digit from.
         * 
         *  Results:
         * 
         *      If the given list contains the given digit, then the digit
         *  is removed from the list.
         * 
         * ------------------------------------------------------------------ */
        private void removeCellNum(int theNum, List<int> cellList) {

            //If the cell's list contains the given digit, then remove that
            //digit.
            if (cellList.Contains(theNum))
                cellList.Remove(theNum);
        }
        // ---------------------------------------------------------------------

        /* cellHasNum ==========================================================
         * 
         *  Check if a cell's list of possible digits contains a digit.
         * 
         *  Parameters:
         * 
         *      theNum
         *      The digit to check.
         * 
         *      cellList
         *      The list to check.
         * 
         *  Returns:
         * 
         *      bool
         *      True if the list contains the digit.
         * 
         *  Results:
         * 
         *      If the given list contains the given digit, then true is 
         *  returned.
         * 
         * ------------------------------------------------------------------ */
        private bool cellHasNum(int theNum, List<int> cellList) {

            //Return true if the list contains the digit.
            return (cellList.Contains(theNum));
        }
        // ---------------------------------------------------------------------

        #endregion
    }
}
