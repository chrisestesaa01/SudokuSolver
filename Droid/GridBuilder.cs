using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Xamarin;
using Android.Views;
using Android.Support.Percent;

namespace SudokuSolver.Droid {



    public class GridBuilder : RelativeLayout{

  
        private Activity myMainActivity;

        public GridBuilder(Activity theMainActivity) : base(theMainActivity) {
            myMainActivity = theMainActivity;
        }



        public void buildGrid(FrameLayout topLayout2) {

            int idCounter;
            int xLoop;
            int yLoop;

            idCounter = 0;


 

            //Top level layout and parameters.
            FrameLayout topLayout;
            LinearLayout.LayoutParams topLayoutParms;

            //Main grid layout and parameters.
            SquarePercentLayout mainGridLayout;
            PercentRelativeLayout.LayoutParams mainGridLayoutParms;

            //Subgrid layout and paramters.
            SquarePercentLayout subGridLayout;
            PercentRelativeLayout.LayoutParams subGridLayoutParms;

            //Layout helper to set percent widths of subgrids.
            PercentLayoutHelper.PercentLayoutInfo subGridLayoutHelper;

            //Layouts for creating a margin around the subgrids.
            LinearLayout subGridMarginLayout1;
            SquarePercentLayout subGridMarginLayout2;

            //Num field layout and paramters.
            SquarePercentLayout numFieldLayout;
            PercentRelativeLayout.LayoutParams numFieldLayoutParms;

            //Layout helper to set percent widths of num fields.
            PercentLayoutHelper.PercentLayoutInfo numFieldLayoutHelper;

            //Num field.
            FitEditText numField;

            //Get top layout.
            topLayout = myMainActivity.FindViewById<FrameLayout>(Resource.Id.frameLayout1);

            //Set top layout to match parent and center child views.
            topLayoutParms = new LinearLayout.LayoutParams(
                FrameLayout.LayoutParams.WrapContent,
                FrameLayout.LayoutParams.WrapContent
            );

            topLayoutParms.Gravity = GravityFlags.CenterHorizontal;


            //Set top layout parms.
            topLayout.LayoutParameters = topLayoutParms;

            //Create a square percent layout to hold the main grid.
            mainGridLayout = new SquarePercentLayout(myMainActivity);

            //Set main grid children to wrap.
            mainGridLayoutParms = new PercentRelativeLayout.LayoutParams(
                PercentRelativeLayout.LayoutParams.WrapContent,
                PercentRelativeLayout.LayoutParams.WrapContent
            );

            //Set main grid layout parms.
            mainGridLayout.LayoutParameters = mainGridLayoutParms;

            //Set main grid background.
            mainGridLayout.SetBackgroundColor(Android.Graphics.Color.Black);

            //Add main grid to top layout.
            topLayout.AddView(mainGridLayout);




            ///testing
            Android.Graphics.Color Mycolor = Android.Graphics.Color.Blue;
            ///testing



            //For each cell in the main grid...
            for (xLoop = 0; xLoop < 9; xLoop++) {

                //Create a new square percent layout for the subgrid.
                subGridLayout = new SquarePercentLayout(myMainActivity);

          
           

                ///testing
                Mycolor.B -= (byte)(30* xLoop);
                //Mycolor.R = 0;
                ///testing
          


                //Set subgrid layout to match parent.
                subGridLayoutParms = new PercentRelativeLayout.LayoutParams(
                    PercentRelativeLayout.LayoutParams.MatchParent,
                    PercentRelativeLayout.LayoutParams.MatchParent
                );

                //Get subgrid layout helper.
                subGridLayoutHelper = subGridLayoutParms.PercentLayoutInfo;

                //Set left and top margins.
                subGridLayoutHelper.LeftMarginPercent = ((xLoop % 3) * (0.33f)); // + 0.006f;
                subGridLayoutHelper.TopMarginPercent = ((xLoop / 3) * (0.33f));

                //Set subgrid container width to 1/3 of parent.
                subGridLayoutHelper.WidthPercent = (0.33f);

                //Set subgrid layout parms.
                subGridLayout.LayoutParameters = subGridLayoutParms;

                //Layout subgrid.
                subGridLayout.RequestLayout();

                //Set subgrid background.
                subGridLayout.SetBackgroundColor(Android.Graphics.Color.Black);

                //Add subgrid to main grid.
                mainGridLayout.AddView(subGridLayout);

                //Nested percent and linear layout to creeate margins for
                //subgrids.
                subGridMarginLayout1 = new LinearLayout(myMainActivity);
                subGridLayout.AddView(subGridMarginLayout1);
                subGridMarginLayout2 = new SquarePercentLayout(myMainActivity);
                subGridLayoutParms.SetMargins(4, 4, 4, 4);
                subGridMarginLayout2.LayoutParameters = subGridLayoutParms;
                subGridMarginLayout1.AddView(subGridMarginLayout2);

                //For each cell in subgrid...
                for (yLoop = 0; yLoop < 9; yLoop++) {

                    //Create a new square percent layout for the subgrid.
                    numFieldLayout = new SquarePercentLayout(myMainActivity);

                    //Set subgrid layout to match parent.
                    numFieldLayoutParms = new PercentRelativeLayout.LayoutParams(
                        PercentRelativeLayout.LayoutParams.MatchParent,
                        PercentRelativeLayout.LayoutParams.MatchParent
                    );

                    //Get subgrid layout helper.
                    numFieldLayoutHelper = numFieldLayoutParms.PercentLayoutInfo;

                    //Set left and top margins.
                    numFieldLayoutHelper.LeftMarginPercent = ((yLoop % 3) * (1f / 3)); // + 0.006f;
                    numFieldLayoutHelper.TopMarginPercent = ((yLoop / 3) * (1f / 3));

                    //Set subgrid container width to 1/3 of parent.
                    numFieldLayoutHelper.WidthPercent = (1f / 3);

                    //Set subgrid layout parms.
                    numFieldLayout.LayoutParameters = numFieldLayoutParms;

                    //Layout subgrid.
                    numFieldLayout.RequestLayout();


                    ///testing
                    Mycolor.R -= (byte)(30 * yLoop);
                    numFieldLayout.SetBackgroundColor(Mycolor);
                    ///testing


                    //Create num field.
                    numField = new FitEditText(myMainActivity);

                    //Set numfield ID.
                    numField.Id = idCounter;
          


                    ///testing
                    numField.SetBackgroundColor(Android.Graphics.Color.Transparent);
                    ///testing
      


                    //Center num field.
                    numField.Gravity = GravityFlags.Center;

                    //Set numfield layout to match parent view.
                    numField.LayoutParameters = new ViewGroup.LayoutParams(
                        ViewGroup.LayoutParams.MatchParent,
                        ViewGroup.LayoutParams.MatchParent
                    );


      
                    numField.SetPadding(0, 0, 0, 0);
                    numField.SetIncludeFontPadding(false);

       
                    numField.Text = "_";
                    numField.Focusable = false;


                    //Set callback for click to show our num picker.
                    numField.Click += (object sender, EventArgs e) => {
                        showNumPicker((FitEditText)sender);
                    };

                    //
                    numField.RequestLayout();

                    idCounter++;
                    numFieldLayout.AddView(numField);
       




                    subGridMarginLayout2.AddView(numFieldLayout);

                }


            }



        }


        public void showNumPicker(FitEditText numField) {

            Dialog d = new Dialog(myMainActivity);

            d.SetTitle("Select number");

            d.SetContentView(Resource.Layout.NumPickPopUp);

            NumberPicker np = d.FindViewById<NumberPicker>(Resource.Id.numberPicker1);
            np.MinValue = 1;
            np.MaxValue = 9;

            Button b1 = (Button)d.FindViewById<Button>(Resource.Id.button1);
            Button b2 = (Button)d.FindViewById<Button>(Resource.Id.button2);
            Button b3 = (Button)d.FindViewById<Button>(Resource.Id.button3);


            b1.Click += (object sender, System.EventArgs e) => {

                    numField.Text = np.Value.ToString();
                d.Dismiss();
            };

                b3.Click += (object sender, System.EventArgs e) => {

                    numField.Text = "_";
                    d.Dismiss();
                };

            b2.Click += (object sender, EventArgs e) => {
                d.Dismiss();
            };

            d.Show();
        }


    }

   
}