using Android.App;
using Android.Support.Percent;

namespace SudokuSolver.Droid {

    /*
     * 
     * Wrapper for the PercentRelativeLayout class to create a square layout
     * object whose width is set by percent of parent layout.
     * 
     */

    public class SquarePercentLayout : PercentRelativeLayout {

        public SquarePercentLayout(Activity theActivity)
            : base(theActivity) { }
        
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {

            //Call base class OnMeasure method.
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            //Get width and set height to match.
            this.LayoutParameters.Height = MeasureSpec.GetSize(widthMeasureSpec);
        }
    }
}
