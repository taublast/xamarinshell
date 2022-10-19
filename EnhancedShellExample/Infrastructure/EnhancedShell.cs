using EnhancedShellExample.Infrastructure.Helpers;
using EnhancedShellExample.Infrastructure.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace EnhancedShellExample.Infrastructure
{
    public partial class EnhancedShell : Shell
    {

        #region STATIC


        private static Screen _screen;
        public static Screen Screen
        {
            get
            {
                if (_screen == null)
                    _screen = new Screen();
                return _screen;
            }
        }

        /// <summary>
        /// In DP
        /// </summary>
        public static double NavBarHeight { get; set; }

        /// <summary>
        /// In DP
        /// </summary>
        public static double StatusBarHeight { get; set; }

        /// <summary>
        /// In DP
        /// </summary>
        public static double BottomTabsHeight { get; set; }


        #endregion

        public EnhancedShell()
        {
            _instance = this;

        }

        public event EventHandler<IndexArgs> TabReselected;


        /// <summary>
        /// To be called by native renderer
        /// </summary>
        /// <param name="index"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetTabReselected(int index)
        {
            TabReselected?.Invoke(this, new IndexArgs
            {
                Index = index
            });
        }

        public static readonly BindableProperty IconScaleXProperty =
    BindableProperty.CreateAttached("IconScaleX", typeof(double), typeof(EnhancedShell),
        1.0, propertyChanged: OnAppearanceChanged);
        public static double GetIconScaleX(BindableObject target) =>
            (double)target.GetValue(IconScaleXProperty);

        public static void SetIconScaleX(BindableObject view, double value) =>
            view.SetValue(IconScaleXProperty, value);

        public static readonly BindableProperty IconScaleYProperty =
            BindableProperty.CreateAttached("IconScaleY", typeof(double), typeof(EnhancedShell),
                1.0, propertyChanged: OnAppearanceChanged);
        public static double GetIconScaleY(BindableObject target) =>
            (double)target.GetValue(IconScaleYProperty);

        public static void SetIconScaleY(BindableObject view, double value) =>
            view.SetValue(IconScaleYProperty, value);


        public static readonly BindableProperty TranslateIconXProperty =
            BindableProperty.CreateAttached("TranslateIconX", typeof(double), typeof(EnhancedShell),
                0.0, propertyChanged: OnAppearanceChanged);
        public static double GetTranslateIconX(BindableObject target) =>
            (double)target.GetValue(TranslateIconXProperty);

        public static void SetTranslateIconX(BindableObject view, double value) =>
            view.SetValue(TranslateIconXProperty, value);

        public static readonly BindableProperty TranslateIconYProperty =
            BindableProperty.CreateAttached("TranslateIconY", typeof(double), typeof(EnhancedShell),
                0.0, propertyChanged: OnAppearanceChanged);
        public static double GetTranslateIconY(BindableObject target) =>
            (double)target.GetValue(TranslateIconYProperty);

        public static void SetTranslateIconY(BindableObject view, double value) =>
            view.SetValue(TranslateIconYProperty, value);

        public static readonly BindableProperty SvgStringProperty =
            BindableProperty.CreateAttached("SvgString", typeof(string), typeof(EnhancedShell),
                string.Empty, propertyChanged: OnAppearanceChanged);
        public static string GetSvgString(BindableObject target) =>
            (string)target.GetValue(SvgStringProperty);

        public static void SetSvgString(BindableObject view, string value) =>
            view.SetValue(SvgStringProperty, value);


        #region BADGES

        public static readonly BindableProperty BadgeTextProperty =
            BindableProperty.CreateAttached("BadgeText", typeof(string), typeof(EnhancedShell),
                string.Empty, propertyChanged: OnAppearanceChanged);
        public static string GetBadgeText(BindableObject target) =>
            (string)target.GetValue(BadgeTextProperty);

        public static void SetBadgeText(BindableObject view, string value) =>
            view.SetValue(BadgeTextProperty, value);


        //-------------------------------------------------------------
        // TabsBadgeTextColor
        //-------------------------------------------------------------
        private const string nameTabsBadgeTextColor = "TabsBadgeTextColor";
        public static readonly BindableProperty TabsBadgeTextColorProperty = BindableProperty.Create(nameTabsBadgeTextColor, typeof(Color),
            typeof(EnhancedShell),
            Color.White,
            propertyChanged: OnAppearanceChanged); //, BindingMode.TwoWay
        public Color TabsBadgeTextColor
        {
            get { return (Color)GetValue(TabsBadgeTextColorProperty); }
            set { SetValue(TabsBadgeTextColorProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsBadgeColor
        //-------------------------------------------------------------
        private const string nameTabsBadgeColor = "TabsBadgeColor";
        public static readonly BindableProperty TabsBadgeColorProperty = BindableProperty.Create(nameTabsBadgeColor, typeof(Color),
            typeof(EnhancedShell),
            Color.Red,
            propertyChanged: OnAppearanceChanged); //, BindingMode.TwoWay
        public Color TabsBadgeColor
        {
            get { return (Color)GetValue(TabsBadgeColorProperty); }
            set { SetValue(TabsBadgeColorProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsBadgeTextFontFamily
        //-------------------------------------------------------------
        private const string nameTabsBadgeTextFontFamily = "TabsBadgeTextFontFamily";
        public static readonly BindableProperty TabsBadgeTextFontFamilyProperty = BindableProperty.Create(nameTabsBadgeTextFontFamily, typeof(string), typeof(EnhancedShell),
            null,
            propertyChanged: OnAppearanceChanged);
        /// <summary>
        /// In DP
        /// </summary>
        public string TabsBadgeTextFontFamily
        {
            get { return (string)GetValue(TabsBadgeTextFontFamilyProperty); }
            set { SetValue(TabsBadgeTextFontFamilyProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsBadgeTextSize
        //-------------------------------------------------------------
        private const string nameTabsBadgeTextSize = "TabsBadgeTextSize";
        public static readonly BindableProperty TabsBadgeTextSizeProperty = BindableProperty.Create(nameTabsBadgeTextSize, typeof(double), typeof(EnhancedShell),
            12.0,
            propertyChanged: OnAppearanceChanged);
        /// <summary>
        /// In DP
        /// </summary>
        public double TabsBadgeTextSize
        {
            get { return (double)GetValue(TabsBadgeTextSizeProperty); }
            set { SetValue(TabsBadgeTextSizeProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsBadgeTextMargin
        //-------------------------------------------------------------
        private const string nameTabsBadgeTextMargin = "TabsBadgeTextMargin";
        public static readonly BindableProperty TabsBadgeTextMarginProperty = BindableProperty.Create(nameTabsBadgeTextMargin, typeof(Thickness), typeof(EnhancedShell),
            new Thickness(2, 0, 2, 1),
            propertyChanged: OnAppearanceChanged);
        /// <summary>
        /// In DP
        /// </summary>
        public Thickness TabsBadgeTextMargin
        {
            get { return (Thickness)GetValue(TabsBadgeTextMarginProperty); }
            set { SetValue(TabsBadgeTextMarginProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsBadgeOffsetX
        //-------------------------------------------------------------
        private const string nameTabsBadgeOffsetX = "TabsBadgeOffsetX";
        public static readonly BindableProperty TabsBadgeOffsetXProperty = BindableProperty.Create(nameTabsBadgeOffsetX, typeof(double), typeof(EnhancedShell),
            4.0,
            propertyChanged: OnAppearanceChanged);
        /// <summary>
        /// In DP
        /// </summary>
        public double TabsBadgeOffsetX
        {
            get { return (double)GetValue(TabsBadgeOffsetXProperty); }
            set { SetValue(TabsBadgeOffsetXProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsBadgeOffsetY
        //-------------------------------------------------------------
        private const string nameTabsBadgeOffsetY = "TabsBadgeOffsetY";
        public static readonly BindableProperty TabsBadgeOffsetYProperty = BindableProperty.Create(nameTabsBadgeOffsetY, typeof(double), typeof(EnhancedShell),
            4.0,
            propertyChanged: OnAppearanceChanged);
        /// <summary>
        /// In DP
        /// </summary>
        public double TabsBadgeOffsetY
        {
            get { return (double)GetValue(TabsBadgeOffsetYProperty); }
            set { SetValue(TabsBadgeOffsetYProperty, value); }
        }

        #endregion

        #region TAB LABELS


        //-------------------------------------------------------------
        // TabBarHideLabels
        //-------------------------------------------------------------
        private const string nameTabBarHideLabels = "TabBarHideLabels";
        public static readonly BindableProperty TabBarHideLabelsProperty = BindableProperty.Create(nameTabBarHideLabels, typeof(bool), typeof(EnhancedShell),
            false,
            propertyChanged: OnAppearanceChanged);
        /// <summary>
        /// In DP
        /// </summary>
        public bool TabBarHideLabels
        {
            get { return (bool)GetValue(TabBarHideLabelsProperty); }
            set { SetValue(TabBarHideLabelsProperty, value); }
        }


        //-------------------------------------------------------------
        // TabBarLabelsFontFamily
        //-------------------------------------------------------------
        private const string nameTabBarLabelsFontFamily = "TabBarLabelsFontFamily";
        public static readonly BindableProperty TabBarLabelsFontFamilyProperty = BindableProperty.Create(nameTabBarLabelsFontFamily, typeof(string), typeof(EnhancedShell),
            null,
            propertyChanged: OnAppearanceChanged);
        /// <summary>
        /// In DP
        /// </summary>
        public string TabBarLabelsFontFamily
        {
            get { return (string)GetValue(TabBarLabelsFontFamilyProperty); }
            set { SetValue(TabBarLabelsFontFamilyProperty, value); }
        }

        //-------------------------------------------------------------
        // TabBarLabelTextSize
        //-------------------------------------------------------------
        private const string nameTabBarLabelTextSize = "TabBarLabelTextSize";
        public static readonly BindableProperty TabBarLabelTextSizeProperty = BindableProperty.Create(nameTabBarLabelTextSize, typeof(double), typeof(EnhancedShell),
            12.0,
            propertyChanged: OnAppearanceChanged);
        /// <summary>
        /// In DP
        /// </summary>
        public double TabBarLabelTextSize
        {
            get { return (double)GetValue(TabBarLabelTextSizeProperty); }
            set { SetValue(TabBarLabelTextSizeProperty, value); }
        }

        //-------------------------------------------------------------
        // TabBarSelectedLabelTextSize
        //-------------------------------------------------------------
        private const string nameTabBarSelectedLabelTextSize = "TabBarSelectedLabelTextSize";
        public static readonly BindableProperty TabBarSelectedLabelTextSizeProperty = BindableProperty.Create(nameTabBarSelectedLabelTextSize, typeof(double), typeof(EnhancedShell),
            7.0,
            propertyChanged: OnAppearanceChanged);
        /// <summary>
        /// In DP
        /// </summary>
        public double TabBarSelectedLabelTextSize
        {
            get { return (double)GetValue(TabBarSelectedLabelTextSizeProperty); }
            set { SetValue(TabBarSelectedLabelTextSizeProperty, value); }
        }


        //-------------------------------------------------------------
        // TabsSelectedLabelColor
        //-------------------------------------------------------------
        private const string nameTabsSelectedLabelColor = "TabsSelectedLabelColor";
        public static readonly BindableProperty TabsSelectedLabelColorProperty = BindableProperty.Create(nameTabsSelectedLabelColor, typeof(Color),
            typeof(EnhancedShell),
            Color.White,
            propertyChanged: OnAppearanceChanged); //, BindingMode.TwoWay
        public Color TabsSelectedLabelColor
        {
            get { return (Color)GetValue(TabsSelectedLabelColorProperty); }
            set { SetValue(TabsSelectedLabelColorProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsLabelColor
        //-------------------------------------------------------------
        private const string nameTabsLabelColor = "TabsLabelColor";
        public static readonly BindableProperty TabsLabelColorProperty = BindableProperty.Create(nameTabsLabelColor, typeof(Color),
            typeof(EnhancedShell),
            Color.Gray,
            propertyChanged: OnAppearanceChanged); //, BindingMode.TwoWay
        public Color TabsLabelColor
        {
            get { return (Color)GetValue(TabsLabelColorProperty); }
            set { SetValue(TabsLabelColorProperty, value); }
        }

        ////-------------------------------------------------------------
        //// TabsSeparatorColor
        ////-------------------------------------------------------------
        //private const string nameTabsSeparatorColor = "TabsSeparatorColor";
        //public static readonly BindableProperty TabsSeparatorColorProperty = BindableProperty.Create(nameTabsSeparatorColor, typeof(Color),
        //    typeof(EnhancedShell),
        //    Color.FromRgb(200, 200, 200),
        //    propertyChanged: OnAppearanceChanged); //, BindingMode.TwoWay
        //public Color TabsSeparatorColor
        //{
        //    get { return (Color)GetValue(TabsSeparatorColorProperty); }
        //    set { SetValue(TabsSeparatorColorProperty, value); }
        //}

        //-------------------------------------------------------------
        // TabsShadowColor
        //-------------------------------------------------------------
        private const string nameTabsShadowColor = "TabsShadowColor";
        public static readonly BindableProperty TabsShadowColorProperty = BindableProperty.Create(nameTabsShadowColor, typeof(Color),
            typeof(EnhancedShell),
            Color.FromRgb(200, 200, 200),
            propertyChanged: OnAppearanceChanged); //, BindingMode.TwoWay
        public Color TabsShadowColor
        {
            get { return (Color)GetValue(TabsShadowColorProperty); }
            set { SetValue(TabsShadowColorProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsShadowRadius
        //-------------------------------------------------------------
        private const string nameTabsShadowRadius = "TabsShadowRadius";
        public static readonly BindableProperty TabsShadowRadiusProperty = BindableProperty.Create(nameTabsShadowRadius, typeof(double), typeof(EnhancedShell),
            3.0,
            propertyChanged: OnAppearanceChanged);
        /// <summary>
        /// In DP
        /// </summary>
        public double TabsShadowRadius
        {
            get { return (double)GetValue(TabsShadowRadiusProperty); }
            set { SetValue(TabsShadowRadiusProperty, value); }
        }

        #endregion

        #region TAB ICONS

        //-------------------------------------------------------------
        // TabsSelectedIconColor
        //-------------------------------------------------------------
        private const string nameTabsSelectedIconColor = "TabsSelectedIconColor";
        public static readonly BindableProperty TabsSelectedIconColorProperty = BindableProperty.Create(nameTabsSelectedIconColor, typeof(Color),
            typeof(EnhancedShell),
            Color.White,
            propertyChanged: OnAppearanceChanged); //, BindingMode.TwoWay
        public Color TabsSelectedIconColor
        {
            get { return (Color)GetValue(TabsSelectedIconColorProperty); }
            set { SetValue(TabsSelectedIconColorProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsIconColor
        //-------------------------------------------------------------
        private const string nameTabsIconColor = "TabsIconColor";
        public static readonly BindableProperty TabsIconColorProperty = BindableProperty.Create(nameTabsIconColor, typeof(Color),
            typeof(EnhancedShell),
            Color.Gray,
            propertyChanged: OnAppearanceChanged); //, BindingMode.TwoWay
        public Color TabsIconColor
        {
            get { return (Color)GetValue(TabsIconColorProperty); }
            set { SetValue(TabsIconColorProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsIconGradientStartColor
        //-------------------------------------------------------------
        private const string nameTabsIconGradientStartColor = "TabsIconGradientStartColor";
        public static readonly BindableProperty TabsIconGradientStartColorProperty = BindableProperty.Create(nameTabsIconGradientStartColor, typeof(Color),
            typeof(EnhancedShell),
            Color.Transparent,
            propertyChanged: OnAppearanceChanged); //, BindingMode.TwoWay
        public Color TabsIconGradientStartColor
        {
            get { return (Color)GetValue(TabsIconGradientStartColorProperty); }
            set { SetValue(TabsIconGradientStartColorProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsIconGradientEndColor
        //-------------------------------------------------------------
        private const string nameTabsIconGradientEndColor = "TabsIconGradientEndColor";
        public static readonly BindableProperty TabsIconGradientEndColorProperty = BindableProperty.Create(nameTabsIconGradientEndColor, typeof(Color),
            typeof(EnhancedShell),
            Color.Transparent,
            propertyChanged: OnAppearanceChanged); //, BindingMode.TwoWay
        public Color TabsIconGradientEndColor
        {
            get { return (Color)GetValue(TabsIconGradientEndColorProperty); }
            set { SetValue(TabsIconGradientEndColorProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsIconGradientRotation
        //-------------------------------------------------------------
        private const string nameTabsIconGradientRotation = "TabsIconGradientRotation";
        public static readonly BindableProperty TabsIconGradientRotationProperty = BindableProperty.Create(nameTabsIconGradientRotation, typeof(double), typeof(EnhancedShell),
            0.0,
            propertyChanged: OnAppearanceChanged);
        public double TabsIconGradientRotation
        {
            get { return (double)GetValue(TabsIconGradientRotationProperty); }
            set { SetValue(TabsIconGradientRotationProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsSelectedIconGradientStartColor
        //-------------------------------------------------------------
        private const string nameTabsSelectedIconGradientStartColor = "TabsSelectedIconGradientStartColor";
        public static readonly BindableProperty TabsSelectedIconGradientStartColorProperty = BindableProperty.Create(nameTabsSelectedIconGradientStartColor, typeof(Color),
            typeof(EnhancedShell),
            Color.Transparent,
            propertyChanged: OnAppearanceChanged); //, BindingMode.TwoWay
        public Color TabsSelectedIconGradientStartColor
        {
            get { return (Color)GetValue(TabsSelectedIconGradientStartColorProperty); }
            set { SetValue(TabsSelectedIconGradientStartColorProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsSelectedIconGradientEndColor
        //-------------------------------------------------------------
        private const string nameTabsSelectedIconGradientEndColor = "TabsSelectedIconGradientEndColor";
        public static readonly BindableProperty TabsSelectedIconGradientEndColorProperty = BindableProperty.Create(nameTabsSelectedIconGradientEndColor, typeof(Color),
            typeof(EnhancedShell),
            Color.Transparent,
            propertyChanged: OnAppearanceChanged); //, BindingMode.TwoWay
        public Color TabsSelectedIconGradientEndColor
        {
            get { return (Color)GetValue(TabsSelectedIconGradientEndColorProperty); }
            set { SetValue(TabsSelectedIconGradientEndColorProperty, value); }
        }

        //-------------------------------------------------------------
        // TabsSelectedIconGradientRotation
        //-------------------------------------------------------------
        private const string nameTabsSelectedIconGradientRotation = "TabsSelectedIconGradientRotation";
        public static readonly BindableProperty TabsSelectedIconGradientRotationProperty = BindableProperty.Create(nameTabsSelectedIconGradientRotation, typeof(double),
            typeof(EnhancedShell),
            0.0,
            propertyChanged: OnAppearanceChanged);
        public double TabsSelectedIconGradientRotation
        {
            get { return (double)GetValue(TabsSelectedIconGradientRotationProperty); }
            set { SetValue(TabsSelectedIconGradientRotationProperty, value); }
        }

        //-------------------------------------------------------------
        // TabBarIconSize
        //-------------------------------------------------------------
        private const string nameTabBarIconSize = "TabBarIconSize";
        public static readonly BindableProperty TabBarIconSizeProperty = BindableProperty.Create(nameTabBarIconSize, typeof(double), typeof(EnhancedShell),
            24.0,
            propertyChanged: OnAppearanceChanged);
        /// <summary>
        /// In DP
        /// </summary>
        public double TabBarIconSize
        {
            get { return (double)GetValue(TabBarIconSizeProperty); }
            set { SetValue(TabBarIconSizeProperty, value); }
        }


        #endregion


        public bool TabsInvalidated;

        public static EnhancedShell _instance;

        public void InvalidateNavBar()
        {
            Element element = this;

            var shell = this;

            if (shell != null)
            {
                Element source = element;
                while (!Application.IsApplicationOrNull(element))
                {
                    IShellController shellController = element as IShellController;
                    if (shellController != null)
                    {
                        shellController.AppearanceChanged(source, appearanceSet: true);
                        break;
                    }
                    element = element.Parent;
                }

                OnNavBarInvalidated();
            }
        }

        public virtual void OnNavBarInvalidated()
        {

        }

        //protected override void OnChildAdded(Element child)
        //{
        //    base.OnChildAdded(child);

        //    InvalidateTabs();
        //}


        public void InvalidateTabs()
        {
            Element element = this;

            var shell = this;

            if (shell != null)
            {
                Element source = element;
                while (!Application.IsApplicationOrNull(element))
                {
                    IShellController shellController = element as IShellController;
                    if (shellController != null)
                    {
                        shell.TabsInvalidated = true;
                        shellController.AppearanceChanged(source, appearanceSet: true);
                        break;
                    }
                    element = element.Parent;
                }
                return;
            }
        }

        protected static void OnAppearanceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Element element = (Element)bindable;

            var shell = bindable as EnhancedShell;
            if (shell == null)
            {
                var content = bindable as ShellContent;
                if (content != null)
                {
                    //silly update tabs
                    shell = _instance;
                    element = shell;
                }
            }

            if (shell != null)
            {
                Element source = element;
                while (!Application.IsApplicationOrNull(element))
                {
                    IShellController shellController = element as IShellController;
                    if (shellController != null)
                    {
                        shell.TabsInvalidated = true;
                        shellController.AppearanceChanged(source, appearanceSet: true);
                        break;
                    }
                    element = element.Parent;
                }
                return;
            }


        }

        public int SelectedIndex
        {
            get
            {
                return CurrentItem.Items.IndexOf(CurrentItem.CurrentItem); ;
            }
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            Debug.WriteLine($"[SHELL NavigatingTo {args.Target}");
            base.OnNavigating(args);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "CurrentItem")
            {
                OnPropertyChanged("SelectedIndex");
            }
        }

        #region SVG ICONS for TabBar ShellContents

        public class RenderedIcon
        {
            public int Index { get; set; }
            public string Route { get; set; }
            public bool IsSelected { get; set; }
            public SKBitmap Bitmap { get; set; }
        }

        public List<RenderedIcon> RenderedTabBarIcons = new List<RenderedIcon>();

        private int lastTabForRenderedIcons = -1;

        public void RenderTabBarIcons(bool force = false)
        {
            if (!(_instance.CurrentItem is TabBar))
                return;

            int selectedIndex = _instance.SelectedIndex;

            if (force)
            {
                RenderedTabBarIcons.Clear();
            }
            else
            {
                if (lastTabForRenderedIcons == selectedIndex && RenderedTabBarIcons.Any())
                    return;
            }

            lastTabForRenderedIcons = selectedIndex;

            var points = SkiaHelper.LinearGradientAngleToPoints(TabsIconGradientRotation);
            var GradientStartXRatio = (float)points.X1;
            var GradientStartYRatio = (float)points.Y1;
            var GradientEndXRatio = (float)points.X2;
            var GradientEndYRatio = (float)points.Y2;


            var index = -1;
            foreach (var section in _instance.CurrentItem.Items)
            {
                if (section == null)
                    continue;



                var content = section.CurrentItem;
                if (content != null)
                {
                    index++;
                    bool isSelected = index == selectedIndex;

                    //this is used by Tab Reselection to pass the TabIndex
                    section.TabIndex = index;

                    //speed optimizations
                    //if (!force && RenderedTabBarIcons.Any())
                    //{
                    //    if (RenderedTabBarIcons
                    //        .Any(x => x.Index == index))
                    //    {
                    //        continue; //looks like nothing to change here
                    //    }
                    //}

                    while (true)
                    {
                        SKBitmap bitmap = null;
                        var adjustScaleX = (float)GetIconScaleX(content);
                        var adjustScaleY = (float)GetIconScaleY(content);

                        //Using SvgString prop as icon source
                        var svgString = GetSvgString(content);
                        if (!string.IsNullOrEmpty(svgString))
                        {
                            var scale = Screen.Density;
                            if (scale == 0)
                            {
                                throw new Exception("[AppShell] Internal scale is 0, check you have called framework platform initializer");
                            }
                            var width = TabBarIconSize * scale;
                            var height = width;

                            byte[] byteArray = Encoding.ASCII.GetBytes(svgString);
                            using (Stream stream = new MemoryStream(byteArray))
                            {

                                var svg = new SKSvg();

                                try
                                {
                                    svg.Load(stream);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    return;
                                }


                                SKRect contentSize = svg.Picture.CullRect;
                                float scaledContentWidth = (float)contentSize.Width;
                                float scaledContentHeight = (float)contentSize.Height;

                                //multipliers to reduce
                                float xRatio = (float)width / scaledContentWidth;
                                float yRatio = (float)height / scaledContentHeight;

                                var aspectX = xRatio;
                                var aspectY = yRatio;
                                var adjustX = 0f;
                                var adjustY = 0f;

                                var needMoreY = (float)(height - scaledContentHeight * xRatio);
                                var needMoreX = (float)(width - scaledContentWidth * yRatio);
                                var needMore = Math.Max(needMoreX, needMoreY);
                                if (needMore > 0)
                                {
                                    var moreX = needMore / scaledContentWidth;
                                    var moreY = needMore / scaledContentHeight;
                                    xRatio = xRatio + moreX;
                                    yRatio = yRatio + moreY;
                                }

                                if (width < height)
                                {
                                    aspectX = xRatio;
                                    aspectY = xRatio;
                                }
                                else
                                {
                                    aspectX = yRatio;
                                    aspectY = yRatio;
                                }

                                aspectX *= adjustScaleX;
                                aspectY *= adjustScaleY;

                                adjustX = (float)((width - scaledContentWidth * aspectX) / 2.0f + GetTranslateIconX(content) * scale);
                                adjustY = (float)((height - scaledContentHeight * aspectY) / 2.0f + GetTranslateIconY(content) * scale);


                                var matrix = new SKMatrix
                                {
                                    ScaleX = aspectX,
                                    SkewX = 0,
                                    TransX = 0 + adjustX,// + (float)(HorizontalOffset * scale),
                                    SkewY = 0,
                                    ScaleY = aspectY,
                                    TransY = 0 + adjustY,
                                    Persp0 = 0,
                                    Persp1 = 0,
                                    Persp2 = 1
                                };


                                using (var paint = new SKPaint
                                {
                                    IsAntialias = true,
                                    FilterQuality = SKFilterQuality.High
                                })
                                {
                                    //drop shadow
                                    //AddShadow(paint, scale);

                                    var info = new SKImageInfo((int)width, (int)height);

                                    bool Render(bool selected)
                                    {
                                        var color = TabsIconColor;
                                        if (selected)
                                            color = TabsSelectedIconColor;

                                        var startColor = color;
                                        var endColor = color;
                                        bool useGradient = false;

                                        if (selected)
                                        {
                                            if (TabsSelectedIconGradientStartColor != Color.Transparent &&
                                                TabsSelectedIconGradientEndColor != Color.Transparent)
                                            {
                                                startColor = TabsSelectedIconGradientStartColor;
                                                endColor = TabsSelectedIconGradientEndColor;
                                                useGradient = true;
                                            }
                                        }
                                        else
                                        {
                                            if (TabsIconGradientEndColor != Color.Transparent &&
                                                TabsIconGradientStartColor != Color.Transparent)
                                            {
                                                startColor = TabsIconGradientStartColor;
                                                endColor = TabsIconGradientEndColor;
                                                useGradient = true;
                                            }
                                        }

                                        paint.ColorFilter = SKColorFilter.CreateBlendMode(color.ToSKColor(), SKBlendMode.SrcIn);
                                        //paint.Shader = null;

                                        using (var surface = SKSurface.Create(info))
                                        {
                                            if (surface == null)
                                            {
                                                Debug.WriteLine("[RenderTabBarIcons] Couldn't create SKIA surface");
                                                return false;
                                            }

                                            paint.Color = color.ToSKColor();

                                            SKCanvas canvas = surface.Canvas;

                                            canvas.Clear(); //SKColor.Parse("00FF00")

                                            if (useGradient)
                                            {
                                                canvas.DrawPicture(svg.Picture, ref matrix, paint);

                                                //draw gradient rectangle above
                                                using (var gradient = new SKPaint
                                                {
                                                    IsAntialias = true
                                                })
                                                {
                                                    if (Device.RuntimePlatform == Device.iOS)
                                                        gradient.FilterQuality = SKFilterQuality.High;

                                                    //drop shadow
                                                    //                                                    gradient.IsAntialias = true;
                                                    gradient.Shader = SKShader.CreateLinearGradient(
                                                        new SKPoint(info.Width * GradientStartXRatio,
                                                            info.Height * GradientStartYRatio),
                                                        new SKPoint(info.Width * GradientEndXRatio,
                                                            info.Height * GradientEndYRatio),
                                                        new SKColor[]
                                                        {
                                                            startColor.ToSKColor(),
                                                            endColor.ToSKColor()
                                                        },
                                                        null,
                                                        SKShaderTileMode.Clamp);

                                                    gradient.BlendMode = SKBlendMode.SrcIn;
                                                    var destination = new SKRect(0, 0, info.Width, info.Height);
                                                    canvas.DrawRect(destination, gradient);
                                                }
                                            }
                                            else
                                            {
                                                paint.ColorFilter = SKColorFilter.CreateBlendMode(color.ToSKColor(), SKBlendMode.SrcIn);
                                                canvas.DrawPicture(svg.Picture, ref matrix, paint);
                                            }

                                            canvas.Flush();

                                            var img = surface.Snapshot();

                                            bitmap = SKBitmap.FromImage(img);
                                        }

                                        var add = new RenderedIcon
                                        {
                                            Index = index,
                                            Bitmap = bitmap,
                                            Route = content.Route,
                                            IsSelected = selected
                                        };
                                        RenderedTabBarIcons.Add(add);

                                        return true;
                                    }

                                    Render(false);
                                    Render(true);

                                }


                            }

                            break;
                        }

                        //ELSE
                        //Using FontImageSource as icon source

                        var textSource = content.Icon as FontImageSource;
                        if (textSource != null)
                        {
                            var text = textSource.Glyph;

                            //NOT IMPLEMENTED IN THIS EXAMPLE
                            // ...


                        }

                        break;
                    }


                    //if (bitmap != null)
                    //{
                    //    var add = new RenderedIcon
                    //    {
                    //        Index = index,
                    //        Bitmap = bitmap,
                    //        Route = content.Route, 
                    //        IsSelected = isSelected
                    //    };
                    //    RenderedTabBarIcons.Add(add);
                    //}
                }
                else
                {
                    var stop = 6;
                }
            }

        }

        #endregion



        public void KeyboardResized(double keyboardSize)
        {
            var size = OnKeyboardResized(keyboardSize);
            //OnKeyboardToggled(state);
            Keyboard = size;
        }

        public virtual double OnKeyboardResized(double size)
        {
            return size;
        }

        private double _Keyboard;
        public double Keyboard
        {
            get { return _Keyboard; }
            set
            {
                if (_Keyboard != value)
                {
                    _Keyboard = value;
                    OnPropertyChanged();
                    OnPropertyChanged("KeyboardOffset");
                }
            }
        }

        public double KeyboardOffset
        {
            get
            {
                if (Device.RuntimePlatform == Device.iOS)
                    return 0.0;

                if (_Keyboard < 0)
                    return 0;

                return _Keyboard;
            }
        }

        public bool TabsInitialized { get; set; }
    }
}
