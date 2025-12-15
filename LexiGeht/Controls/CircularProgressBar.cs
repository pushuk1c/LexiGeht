

using Microsoft.Maui.Graphics;

namespace LexiGeht.Controls
{
    public class CircularProgressBar: GraphicsView
    {

        public static readonly BindableProperty ProgressProperty =
            BindableProperty.Create(nameof(Progress), typeof(double), typeof(CircularProgressBar), 0.0, propertyChanged: OnProgressChanged);

        public double Progress 
        { 
            get => (double)GetValue(ProgressProperty); 
            set => SetValue(ProgressProperty, value);
        }

        public static readonly BindableProperty ProgressColorProperty =
           BindableProperty.Create(nameof(ProgressColor), typeof(Color), typeof(CircularProgressBar), Colors.Blue, propertyChanged: InvalidateOnChange);

        public Color ProgressColor
        {
            get => (Color)GetValue(ProgressColorProperty);
            set => SetValue(ProgressColorProperty, value);
        }

        public static readonly BindableProperty ProgressSizeProperty =
          BindableProperty.Create(nameof(ProgressSize), typeof(double), typeof(CircularProgressBar), 4.0, propertyChanged: InvalidateOnChange);

        public double ProgressSize
        {
            get => (double)GetValue(ProgressSizeProperty);
            set => SetValue(ProgressSizeProperty, value);
        }

        public static readonly BindableProperty PersentageFontSizeProperty =
          BindableProperty.Create(nameof(PersentageFontSize), typeof(double), typeof(CircularProgressBar), 10.0, propertyChanged: InvalidateOnChange);

        public double PersentageFontSize
        {
            get => (double)GetValue(PersentageFontSizeProperty);
            set => SetValue(PersentageFontSizeProperty, value);
        }

        private static void InvalidateOnChange(BindableObject bindable, object oldValue, object newValue)
        {
            if (!Equals(oldValue,newValue))
                ((CircularProgressBar)bindable).Invalidate();
        }

        private static void OnProgressChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var progressBar = (CircularProgressBar)bindable;

            var from = Math.Clamp((double)oldValue, 0, 1);
            var to   = Math.Clamp((double)newValue, 0, 1);

            if(!progressBar._initialized)
            {
                progressBar._initialized = true;
                progressBar._animatedProgress = to;
                progressBar.Invalidate();
                return;
            }

            if(Math.Abs(from - to) < 0.001)
            {
                progressBar._animatedProgress = to;
                progressBar.Invalidate();
                return;
            }

            var animation = new Animation(v =>
            {
                progressBar._animatedProgress = v;
                progressBar.Invalidate();
            }, from, to);

            animation.Commit(progressBar, "ProgressAnimation", 16, 800, Easing.SinInOut);

        }

        private double _animatedProgress;
        private bool _initialized;

        public CircularProgressBar()
        {
            _animatedProgress = Progress;
            Drawable = new CircularProgressBarDrawable(this);
        }

        internal double GetAnimationProgress() => _animatedProgress;

    }


    public class CircularProgressBarDrawable : IDrawable
    {

        private readonly CircularProgressBar _circularProgressBar;

        public CircularProgressBarDrawable(CircularProgressBar circularProgressBar)
        {
            _circularProgressBar = circularProgressBar;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {

            // float strokeWidth = 4F;
            float strokeWidth = (float)_circularProgressBar.ProgressSize;
            float radius = (float)(Math.Min(dirtyRect.Width, dirtyRect.Height) / 2f - strokeWidth);

            canvas.StrokeColor = Colors.LightGray;
            canvas.StrokeSize = strokeWidth;
            canvas.StrokeLineCap = LineCap.Round;
            canvas.DrawCircle(dirtyRect.Center.X, dirtyRect.Center.Y, radius);

            var progress = (float)_circularProgressBar.GetAnimationProgress();
            if(progress > 0f)
            {
                var startAngle = 270f;
                var endAngle = startAngle - 360f * progress;

                canvas.StrokeColor = _circularProgressBar.ProgressColor;
                //canvas.StrokeColor = Colors.Blue;
                canvas.DrawArc(dirtyRect.Center.X - radius, dirtyRect.Center.Y - radius, radius * 2f, radius * 2f, startAngle, endAngle, true, false);
            }

            var percent = (int)(_circularProgressBar.GetAnimationProgress() * 100);
            canvas.FontSize = (float)_circularProgressBar.PersentageFontSize;
            //canvas.FontSize = 10;
            canvas.FontColor = Colors.Black;
            var text = $"{percent}%";
            canvas.DrawString(text, dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height, HorizontalAlignment.Center, VerticalAlignment.Center);

        }
        
       
    }


}
