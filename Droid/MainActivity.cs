using Android.App;
using Android.OS;
using Android.Widget;
using Java.Lang;

namespace SudokuSolver.Droid {

    [Activity(Label = "SudokuSolver", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity {

        //This object will programmatically build out puzzle grid UI layout.
        private GridBuilder myPuzzle;

        //The puzzle solver.
        private Solver mySolver;

        //A thread to run the solver in the background.
        private Thread solverThread;

        protected override void OnCreate(Bundle savedInstanceState) {

            //Base class OnCreate.
            base.OnCreate(savedInstanceState);

            //Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //Create our puzzle grid.
            myPuzzle = new GridBuilder(this);

            //Create our solver.
            mySolver = new Solver(myPuzzle);

            //Set 'Solve' button click event handler function.
            Button myButton = this.FindViewById<Button>(Resource.Id.solveButton);
            myButton.Click += (sender, e) => {
                solveClickHandler();
            };
        }

        private void solveClickHandler() {

            if (solverThread == null) {

                //Set up the solver.
                mySolver.Setup();

                //Set up new thread to run the solver loop.
                solverThread = new Thread(new Runnable(() => mySolver.SolveLoop()));

                //Start the new thread.
                solverThread.Start();
            }
        }
    }
}


