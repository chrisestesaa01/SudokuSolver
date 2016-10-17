using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;

namespace SudokuSolver.Droid {
    [Activity(Label = "SudokuSolver", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            FrameLayout topLayout;

            // Get our button from the layout resource,
            // and attach an event to it
            //Button button = FindViewById<Button>(Resource.Id.myButton);
            //TextView myText = FindViewById<TextView>(Resource.Id.editText1);

            GridBuilder myGridbuilder = new GridBuilder(this);

            topLayout = this.FindViewById<FrameLayout>(Resource.Id.frameLayout1);


            myGridbuilder.buildGrid(topLayout);
  
            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); 
            //myText.Text = "1";
        


        }

        protected override void OnResume() {
            base.OnResume();

            //LinearLayout MainLayout = this.FindViewById<LinearLayout>(Resource.Id.MainLayout1);

        

            //MainLayout.Invalidate();



        }
         


    }
}

