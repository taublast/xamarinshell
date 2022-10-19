using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using Xamarin.Forms;
using Color = Android.Graphics.Color;
using TextAlignment = Android.Views.TextAlignment;

namespace AppoMobi.Framework.Droid.Renderers
{
    public class BadgeView : FrameLayout
    {
        #region 


        public BadgeView(Context context, int id) : base(context)
        {
            Id = id;

            CreateView(context);
        }


        protected BadgeView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public BadgeView(Context context) : base(context)
        {
            CreateView(context);
        }

        public BadgeView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            CreateView(context);

        }

        public BadgeView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            CreateView(context);

        }

        public BadgeView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            CreateView(context);

        }

        #endregion

        public void CreateView(Context context)
        {
            _density = context.Resources.DisplayMetrics.Density;


            _widthPlaceholder = new LinearLayout(context);
            var layoutParams = new LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent);
            _widthPlaceholder.LayoutParameters = layoutParams;
            _widthPlaceholder.SetMinimumWidth((int)(MinWidthDp * _density));

            _label = new TextView(context);
            _label.SetTextColor(Color.White);
            //_label.Background = new ColorDrawable(Color.Black);
            _label.SetPadding(0, 0, 0, 0);//todo dp
            var labelLayout = new LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent);
            labelLayout.Gravity = GravityFlags.Center;
            _label.LayoutParameters = labelLayout;
            _label.SetMaxLines(1);
            _label.TextAlignment = TextAlignment.Center;
            _label.Gravity = GravityFlags.Center;
            this.AddView(_label);

            var globalLayout = new LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent);
            globalLayout.Gravity = GravityFlags.Center;
            this.LayoutParameters = globalLayout;
            this.AddView(_widthPlaceholder);

            ApplyMargins();

            SetBackgroundColor(Color.Red);
        }

        private Thickness _LabelMargins = new Thickness(2, 0, 2, 0);
        public Thickness LabelMargins
        {
            get
            {
                return _LabelMargins;
            }
            set
            {
                _LabelMargins = value;
                ApplyMargins();
            }
        }

        private float _minWidthDp = 10;
        public float MinWidthDp
        {
            get
            {
                return _minWidthDp;
            }
            set
            {
                _minWidthDp = value;
                _widthPlaceholder.SetMinimumWidth((int)(value * _density));
            }
        }

        private float _density;

        public void ApplyMargins()
        {
            SetPadding((int)(LabelMargins.Left * _density), (int)(LabelMargins.Top * _density), (int)(LabelMargins.Right * _density), (int)(LabelMargins.Bottom * _density));

            var layout = (MarginLayoutParams)this.LayoutParameters;

            layout.MarginStart = (int)(_density * OffsetX);
            layout.BottomMargin = -(int)(_density * OffsetY);
        }

        private float _offsetX = 20;
        /// <summary>
        /// DIP
        /// </summary>
        public float OffsetX
        {
            get
            {
                return _offsetX;
            }
            set
            {
                _offsetX = value;
                ApplyMargins();
            }
        }

        private float _offsetY = -20;
        /// <summary>
        /// DIP
        /// </summary>
        public float OffsetY
        {
            get
            {
                return _offsetY;
            }
            set
            {
                _offsetY = value;
                ApplyMargins();
            }
        }

        TextView _label;


        public TextView Label
        {
            get
            {
                return _label;
            }
        }

        public void SetValue(string value)
        {
            if (string.IsNullOrEmpty(value) || value == "0")
                SetVisible(false);
            else
                SetVisible(true);

            ApplyMargins();
            Label.Text = value;
        }

        public void SetVisible(bool value)
        {
            this.Visibility = value ? ViewStates.Visible : ViewStates.Gone;
        }


        private float _cornerRadius = 10;
        /// <summary>
        /// DIP
        /// </summary>
        public float CornerRadius
        {
            get
            {
                return _cornerRadius;
            }
            set
            {
                _cornerRadius = value;
                this.Background = CreateBadgeBackground(Context, BackgroundColor);
            }
        }

        private Color _backgroundColor = Color.Red;

        private LinearLayout _widthPlaceholder;

        public Color BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
                this.Background = CreateBadgeBackground(Context, value);
            }
        }

        private Color _TextColor = Color.White;
        public Color TextColor
        {
            get
            {
                return _TextColor;
            }
            set
            {
                _TextColor = value;
                _label.SetTextColor(value);
            }
        }


        protected PaintDrawable CreateBadgeBackground(Context context, Color color)
        {
            var badgeFrameLayoutBackground = new PaintDrawable();

            badgeFrameLayoutBackground.Paint.Color = color;
            badgeFrameLayoutBackground.Shape = new RectShape();
            badgeFrameLayoutBackground.SetCornerRadius(TypedValue.ApplyDimension(ComplexUnitType.Dip, CornerRadius,
                context.Resources.DisplayMetrics));

            return badgeFrameLayoutBackground;
        }

    }
}