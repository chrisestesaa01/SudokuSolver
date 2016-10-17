using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;

namespace SudokuSolver.Droid {

    /* FitEditText =============================================================
     * 
     * A class that extends EditText to automatically scale the font size to
     * fit the text to the view.
     * 
     ------------------------------------------------------------------------ */
    public class FitEditText : TextView {

        //Paint object to use for scalling text.
        private Paint mTestPaint;

        /* Constructors ===================================================== */
        public FitEditText(Context context)
            : base(context) {
            initialise();
        }

        public FitEditText(Context context, IAttributeSet attrs)
            : base(context, attrs) {
            initialise();
        }
        /* ------------------------------------------------------------------ */

        /* initialise ==========================================================
         * 
         * Set up a Paint object to be used by the 'refitText' function to
         * measure text dimensions with different text size settings.
         * 
         -------------------------------------------------------------------- */
        private void initialise() {

            //Add callback to textchanged event handler.
            this.TextChanged += OnTextChanged;

            //Create test paint object and set it to this text view's paint.
            mTestPaint = new Paint();
            mTestPaint.Set(this.Paint);
        }
        /* ------------------------------------------------------------------ */

        /* refitText ===========================================================
         * 
         * Scales the size of the field's text until it fits within the field.
         *
         -------------------------------------------------------------------- */
        private void refitText(String text, int textWidth, int textHeight) {

            //Only do stuff if width != 0.
            if (textWidth > 0) {

                //Width and height of text field minus any padding.
                int targetWidth = textWidth - this.PaddingLeft - this.PaddingRight;
                int targetHeight = textHeight - this.PaddingTop - this.PaddingBottom;


                //Vers to get width and height of rendered text in new font size.
                float myMeasuredWidth;
                float myMeasuredHeight;

                //Var to calculate new font size.
                float size;

                //Max and min font sizes.
                float hi = 10000;
                float lo = 2;

                //Threshold for acceptable font size.
                float threshold = 0.5f;

                //Flag to check for good font size.
                bool tooBig;

                //For measuring the font height.
                Paint.FontMetrics myFontMetric;

                //Until we have zeroed in on a good font size...
                while ((hi - lo) > threshold) {

                    //Split the difference for the new font size.
                    size = (hi + lo) / 2;

                    //Set size in test paint.
                    mTestPaint.TextSize = size;

                    //Get text width.
                    myMeasuredWidth = mTestPaint.MeasureText(text);

                    //Get text height.
                    myFontMetric = mTestPaint.GetFontMetrics();
                    myMeasuredHeight = myFontMetric.Bottom - myFontMetric.Top;

                    //Check size.
                    tooBig = (
                        (myMeasuredWidth >= targetWidth) ||
                        (myMeasuredHeight >= targetHeight)
                    );

                    //If width or height is too big...
                    if (tooBig) {

                        //Update hi size.
                        hi = size;
                    }

                    //If too small...
                    else {

                        //Update lo size.
                        lo = size;
                    }
                }

                //Set new text size.
                this.SetTextSize(ComplexUnitType.Px , lo);
            }
        }
        /* ------------------------------------------------------------------ */

        /* OnTextChanged =======================================================
         * 
         * Callback to execute when the field's text is changed.
         *
         -------------------------------------------------------------------- */
        private void OnTextChanged(object sender, Android.Text.TextChangedEventArgs e) {

            //Call refit when text changes.
            refitText(this.Text, this.Width, this.Height);
        }
        /* ------------------------------------------------------------------ */

        /* OnSizeChanged =======================================================
         * 
         * Callback to execute when the field's size is changed.
         *
         -------------------------------------------------------------------- */
        override protected void OnSizeChanged(int w, int h, int oldw, int oldh) {

            //Refit text to new size.
            refitText(this.Text, w, h);
        }
        /* ------------------------------------------------------------------ */

    }

}

