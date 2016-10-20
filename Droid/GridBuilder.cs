using System;
using Android.App;
using Android.Widget;
using Android.Views;
using Android.Support.Percent;

namespace SudokuSolver.Droid {

    /* GridBuilder =============================================================
     * 
     * Class built on the FrameLayout class to create a sudoku puzzle grid.
     * This class also sets an event handler for clicks on individual grid 
     * cells to display a number picker dialog to set the cell digits.
     * 
     * ---------------------------------------------------------------------- */
    public class GridBuilder : FrameLayout, Solver.ISolverInterface{

        #region Private members

        //Reference to the main activity.
        private Activity myMainActivity;

        #endregion

        #region Constructors

        /* GridBuilder =========================================================
         * 
         * Constructor. Get the layout at the top of the view heirerchy and
         * add this grid builder to the beginning of the layout. Then build
         * the grid.
         * 
         * ------------------------------------------------------------------ */
        public GridBuilder(Activity theMainActivity) 
            : base(theMainActivity) {

            //Top level view.
            ViewGroup rootView;

            //Top level layout.
            LinearLayout topLayout;

            //Layout parameters for this.
            LinearLayout.LayoutParams thisParms;

            //Set activity reference.
            myMainActivity = theMainActivity;

            //Get root of view heirarchy.
            rootView = myMainActivity.FindViewById<ViewGroup>(Android.Resource.Id.Content);

            //Get top level layout.
            topLayout = (LinearLayout)rootView.GetChildAt(0);

            //Create layout parameters for this.
            thisParms = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent
            );

            //Set this object's layout parms.
            this.LayoutParameters = thisParms;

            //Add this grid builder to the top of the layout.
            topLayout.AddView(this, 0);

            //Build the grid.
            this.buildGrid();

        }
        // ---------------------------------------------------------------------

        #endregion

        #region Public methods

        /* GetCell =============================================================
         * 
         * ISolverInterface implementation method.
         * 
         * Return the value in a cell of the puzzle grid. If the cell is
         * empty, then 0 is returned. 
         * 
         * ------------------------------------------------------------------ */
        public int GetCell(int row, int col) {

            //The result of this function.
            int retVal;

            //The block that contains the desired cell.
            int blockNum;

            //The ID of the desired cell.
            int cellID;

            //Var to get the cell's text value.
            string cellText;

            //Get the block number of the cell.
            blockNum = (3 * (row / 3)) + (col / 3);

            //Get the ID of the desired cell.
            cellID = (blockNum * 9) + (3 * (row % 3)) + (col % 3);

            //Get the cell's text and parse to an int.
            cellText = this.FindViewById<TextView>(cellID).Text;
            if (!int.TryParse(cellText, out retVal)) {
                retVal = 0;
            }

            //Return the result.
            return (retVal);

        }
        // ---------------------------------------------------------------------

        /* SetCell =============================================================
         *
         * ISolverInterface implementation method.
         * 
         * Set the value of a cell in the puzzle grid.
         *
         * ------------------------------------------------------------------ */
        public void SetCell(int row, int col, int val) {

            //Var to get the desired cell's text view.
            TextView myCell;

            //ID of the desired cell.
            int cellID;

            //The block that contains the desired cell.
            int blockNum;

            //Get the block number of the cell.
            blockNum = (3 * (row / 3)) + (col / 3);

            //Get the ID of the cell.
            cellID = (blockNum * 9) + (3 * (row % 3)) + (col % 3);

            //Set the cell's text value.
            myCell = this.FindViewById<TextView>(cellID);

            myMainActivity.RunOnUiThread(() => {

                myCell.Text = val.ToString();
            });
        }
        // ---------------------------------------------------------------------

        #endregion

        #region Private methods

        /* buildGrid ===========================================================
         * 
         * Build the puzzle grid. This function creates the puzzle grid as a
         * 3x3 grid spaced evenly across the screen. Each grid cell contains
         * another 3x3 grid and each cell of the subgrids contains a textview
         * that can be accessed by IDs 0 - 80.
         * 
         * ------------------------------------------------------------------ */
        private void buildGrid() {

            //A var to assign unique IDs to each textview cell in the grid.
            int idCounter;

            //Loop counters.
            int xLoop;
            int yLoop;

            #region Top level grid layout:

            //Create a square percent layout to hold the main grid.
            SquarePercentLayout mainGridLayout = new SquarePercentLayout(myMainActivity);

            //Set main grid children to wrap.
            PercentRelativeLayout.LayoutParams mainGridLayoutParms = new 
                PercentRelativeLayout.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.MatchParent
                );

            //Set main grid layout parms.
            mainGridLayout.LayoutParameters = mainGridLayoutParms;

            //Set main grid background.
            mainGridLayout.SetBackgroundColor(Android.Graphics.Color.Black);

            //Add main grid to top layout.
            this.AddView(mainGridLayout);

            #endregion

            //Init ID counter.
            idCounter = 0;

            //For each subgrid in the main grid...
            for (xLoop = 0; xLoop < 9; xLoop++) {

                #region Subgrid outer frame: (LinearLayout)

                //Create subgrid outer frame.
                LinearLayout subGridFrame = new LinearLayout(myMainActivity);

                //Create parameters for subgrid frame.
                PercentRelativeLayout.LayoutParams subGridFrameLayoutParms = new
                    PercentRelativeLayout.LayoutParams(
                        ViewGroup.LayoutParams.WrapContent,
                        ViewGroup.LayoutParams.WrapContent
                    );

                //Get percent layout helper for subgrid frame.
                PercentLayoutHelper.PercentLayoutInfo pctLayoutHelper = 
                    subGridFrameLayoutParms.PercentLayoutInfo;

                //Set left and top margins.
                pctLayoutHelper.LeftMarginPercent = ((xLoop % 3) * (1/3f));
                pctLayoutHelper.TopMarginPercent = ((xLoop / 3) * (1/3f));

                //Set subgrid container width to 1/3 of parent.
                pctLayoutHelper.WidthPercent = (1/3f);

                //Set background and padding for large gridlines.
                subGridFrame.SetBackgroundColor(Android.Graphics.Color.Black);
                subGridFrame.SetPadding(2, 2, 2, 2);

                //Set outer frame parms.
                subGridFrame.LayoutParameters = subGridFrameLayoutParms;

                //Set to center child view.
                subGridFrame.SetGravity(GravityFlags.Center);

                //Add subgrid frame to main grid.
                mainGridLayout.AddView(subGridFrame);

                #endregion

                #region Subgrid layout: (SquarePercentLayout)

                //Create a new square percent layout for the subgrid.
                SquarePercentLayout subGridLayout = new SquarePercentLayout(myMainActivity);

                //Set subgrid layout to match parent.
                PercentRelativeLayout.LayoutParams subGridLayoutParms = new 
                    PercentRelativeLayout.LayoutParams(
                        ViewGroup.LayoutParams.WrapContent,
                        ViewGroup.LayoutParams.WrapContent
                    );

                //Set subgrid layout parms.
                subGridLayout.LayoutParameters = subGridLayoutParms;

                //Set subgrid background.
                subGridLayout.SetBackgroundColor(Android.Graphics.Color.Black);

                subGridFrame.RequestLayout();

                //Add subgrid to subgrid frame.
                subGridFrame.AddView(subGridLayout);

                #endregion

                //For each cell in subgrid...
                for (yLoop = 0; yLoop < 9; yLoop++) {

                    #region Number field outer frame: (LinearLayout)

                    //Create a linear layout for the num holder outer frame.
                    LinearLayout numFieldLayout = new LinearLayout(myMainActivity);

                    //Set subgrid layout to match parent.
                    PercentRelativeLayout.LayoutParams numFieldLayoutParms = new 
                        PercentRelativeLayout.LayoutParams(
                            ViewGroup.LayoutParams.WrapContent,
                            ViewGroup.LayoutParams.WrapContent
                        );

                    //Get subgrid layout helper.
                    PercentLayoutHelper.PercentLayoutInfo numFieldLayoutHelper = 
                        numFieldLayoutParms.PercentLayoutInfo;

                    //Set left and top margins.
                    numFieldLayoutHelper.LeftMarginPercent = ((yLoop % 3) * (1/3f));
                    numFieldLayoutHelper.TopMarginPercent = ((yLoop / 3) * (1/3f));

                    //Set subgrid container width.
                    numFieldLayoutHelper.WidthPercent = (1/3f);

                    //Set subgrid layout parms.
                    numFieldLayout.LayoutParameters = numFieldLayoutParms;

                    //Add frame to subgrid.
                    subGridLayout.AddView(numFieldLayout);

                    #endregion

                    #region Number field layout: (SquarePercentLayout)

                    //Create square percent layout for the num field.
                    SquarePercentLayout numFieldHolder = new SquarePercentLayout(myMainActivity);
                      
                    //Set num field holder layout to match parent.
                    PercentRelativeLayout.LayoutParams numfieldHolderParms = new 
                        PercentRelativeLayout.LayoutParams(
                            ViewGroup.LayoutParams.WrapContent,
                            ViewGroup.LayoutParams.WrapContent
                        );

                    //Set subgrid layout parms.
                    numFieldHolder.LayoutParameters = numfieldHolderParms;

                    //Set subgrid padding to show small gridlines.
                    numFieldHolder.SetPadding(2, 2, 2, 2);

                    //Add num field holder to layout frame.
                    numFieldLayout.AddView(numFieldHolder);

                    #endregion

                    #region Number field: (FitTextView)

                    //Create num field.
                    FitTextView numField = new FitTextView(myMainActivity);

                    //Set numfield ID.
                    numField.Id = idCounter;
                    idCounter++;

                    //Center num field.
                    numField.Gravity = GravityFlags.Center;

                    //Set numfield layout to match parent view.
                    numField.LayoutParameters = new ViewGroup.LayoutParams(
                        ViewGroup.LayoutParams.MatchParent,
                        ViewGroup.LayoutParams.MatchParent
                    );

                    //No font padding.
                    numField.SetIncludeFontPadding(false);

                    //Set so we don't lose numfield text on rotate.
                    numField.FreezesText = true;

                    //Set field text value.
                    setNumText(numField);

                    //Cells don't need focus because we are using our num picker.
                    numField.Focusable = false;

                    //Set callback for click to show our num picker.
                    numField.Click += (object sender, EventArgs e) => {
                        showNumPicker((FitTextView)sender);
                    };

                    //Set field background color.
                    numField.SetBackgroundColor(Android.Graphics.Color.DarkGreen);

                    //Lay out field.
                    numField.RequestLayout();

                    //Add field to holder.
                    numFieldHolder.AddView(numField);

                    #endregion
                }
            }
        }
        // ---------------------------------------------------------------------

        /* setNumText ==========================================================
         * 
         * Set num field text with defaults for test puzzle.
         * 
         * ------------------------------------------------------------------ */
        private void setNumText(FitTextView numField) {

            //Default puzzle:
            String[] defaults = new String[81];
            defaults[5] = "6";
            defaults[11] = "5";
            defaults[12] = "4";
            defaults[13] = "3";
            defaults[16] = "9";
            defaults[18] = "8";
            defaults[20] = "4";
            defaults[21] = "1";
            defaults[25] = "6";
            defaults[29] = "1";
            defaults[32] = "3";
            defaults[33] = "4";
            defaults[39] = "1";
            defaults[40] = "4";
            defaults[41] = "7";
            defaults[47] = "8";
            defaults[48] = "2";
            defaults[51] = "9";
            defaults[55] = "1";
            defaults[59] = "4";
            defaults[60] = "6";
            defaults[62] = "8";
            defaults[64] = "7";
            defaults[67] = "1";
            defaults[68] = "2";
            defaults[69] = "3";
            defaults[75] = "7";

            numField.Text = "_";
            //if (defaults[numField.Id] != null)
            //    numField.Text = defaults[numField.Id];
            
        }
        // ---------------------------------------------------------------------

        /* showNumPicker =======================================================
         * 
         * Show a number picker spinner to set the digit for a cell in the
         * puzzle.
         * 
         * ------------------------------------------------------------------ */
        private void showNumPicker(FitTextView numField) {

            //Create a new dialog.
            Dialog d = new Dialog(myMainActivity);

            //Set dialog title.
            d.SetTitle(Resource.String.numPickerTitle);

            //Set layout for our num picker.
            d.SetContentView(Resource.Layout.NumPickPopUp);

            //Set min and max values for num picker.
            NumberPicker np = d.FindViewById<NumberPicker>(Resource.Id.numberPicker1);
            np.MinValue = 1;
            np.MaxValue = 9;

            //Get the buttons.
            Button b1 = (Button)d.FindViewById<Button>(Resource.Id.button1);
            Button b2 = (Button)d.FindViewById<Button>(Resource.Id.button2);
            Button b3 = (Button)d.FindViewById<Button>(Resource.Id.button3);

            //'Set' button will set the text field's digit.
            b1.Click += (object sender, System.EventArgs e) => {
                numField.Text = np.Value.ToString();
                d.Dismiss();
            };

            //'Cancel' button just closes the dialog.
            b2.Click += (object sender, EventArgs e) => {
                d.Dismiss();
            };

            //'Clear' button resets text field to an underscore.
            b3.Click += (object sender, System.EventArgs e) => {

                numField.Text = "_";
                d.Dismiss();
            };

            //Show our dialog.
            d.Show();

        }
        // ---------------------------------------------------------------------

        #endregion
    }

}

