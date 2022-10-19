using CoreGraphics;
using EnhancedShellExample.Infrastructure;
using SkiaSharp;
using SkiaSharp.Views.iOS;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;

namespace AppoMobi.Framework.iOS.Renderers
{
    public class CustomizeBottomMenuItems : IShellTabBarAppearanceTracker, IDisposable
    {


        //titles offset
        public float AppleTitlesOffset = 0.0f;//-1.5f; // -4.0

        //icons 
        public float ios_tabs_yoffset = -0.5f;
        public float ios_tabs_yoffset_notxt = 5f;

        public CustomizeBottomMenuItems(AppShellRenderer renderer)
        {


            _appShell = renderer?.FormsControl;

            _density = (float)UIScreen.MainScreen.Scale;

            if (_appShell != null)
            {

                _appShell.PropertyChanged += OnShellPropertyChanged;
            }

            // Display.iZoomed = UIScreen.MainScreen.NativeScale > UIScreen.MainScreen.Scale;


        }

        EnhancedShell _appShell;

        private readonly float _density;


        public void Dispose()
        {
            if (_appShell != null)
                _appShell.PropertyChanged -= OnShellPropertyChanged;
        }

        public void ResetAppearance(UITabBarController controller)
        {
            onceHeight = false;
            _appShell.TabsInvalidated = true;
        }

        UIOffset GetTitlesOffset()
        {
            return new UIOffset(0, AppleTitlesOffset * _density);
        }

        protected UIStringAttributes GetBadgeStringAttributes()
        {
            var textAttributes = new UIStringAttributes();
            if (fontBadges != null)
                textAttributes.Font = fontBadges;
            textAttributes.ForegroundColor = _appShell.TabsBadgeTextColor.ToUIColor();//.ColorWithAlpha(AppColors.tabs_unselected_a); ;
            return textAttributes;
        }

        protected UIStringAttributes GetStringAttributes(bool selected = false)
        {
            var textAttributes = new UIStringAttributes();
            if (!selected)
            {
                //UNSELECTED
                //textAttributes.Font = UIFont.FromName(TextFont.Bold, FormsControl.ios_tabs_font_size); //BOOOLD BOLD !!!
                if (fontLabels != null)
                    textAttributes.Font = fontLabels;
                textAttributes.ForegroundColor = _appShell.TabsLabelColor.ToUIColor();//.ColorWithAlpha(AppColors.tabs_unselected_a); ;
            }
            else
            {
                //SELECTED
                //textAttributes.Font = UIFont.FromName(TextFont.Bold, FormsControl.ios_tabs_font_size);
                if (fontLabelsSelected != null)
                    textAttributes.Font = fontLabelsSelected;
                textAttributes.ForegroundColor = _appShell.TabsSelectedLabelColor.ToUIColor();//.ColorWithAlpha(tabs_selected);
            }
            return textAttributes;
        }

        protected UITextAttributes GetTextAttributes(bool selected = false)
        {
            UITextAttributes textAttributes = new UITextAttributes();
            if (!selected)
            {
                //UNSELECTED
                //textAttributes.Font = UIFont.FromName(TextFont.Bold, FormsControl.ios_tabs_font_size); //BOOOLD BOLD !!!
                if (fontLabels != null)
                    textAttributes.Font = fontLabels;
                textAttributes.TextColor = _appShell.TabsLabelColor.ToUIColor();//.ColorWithAlpha(AppColors.tabs_unselected_a); ;
            }
            else
            {
                //SELECTED
                //textAttributes.Font = UIFont.FromName(TextFont.Bold, FormsControl.ios_tabs_font_size);
                if (fontLabelsSelected != null)
                    textAttributes.Font = fontLabelsSelected;
                textAttributes.TextColor = _appShell.TabsSelectedLabelColor.ToUIColor();//.ColorWithAlpha(tabs_selected);
            }
            return textAttributes;
        }

        private int _lastChildrenCount;

        protected UIFont fontLabels;

        protected UIFont fontLabelsSelected;

        protected UIFont fontBadges;


        private bool imagesSet;

        private bool onceHeight;

        UIImage ColorForNavBar(UIColor color)
        {
            var rect = new CGRect(x: 0.0, y: 0.0, width: 1.0, height: 1.0);
            //    Or if you need a thinner border :
            //    let rect = CGRect(x: 0.0, y: 0.0, width: 1.0, height: 0.5)

            UIGraphics.BeginImageContext(rect.Size);
            var context = UIGraphics.GetCurrentContext();

            context.SetFillColor(color.CGColor);
            context.FillRect(rect);

            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return image;
        }

        private void OnShellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedIndex")
            {
                System.Diagnostics.Debug.WriteLine($"[NATIVE] selected tab is: {_appShell.SelectedIndex}");
            }
        }

        public void SetAppearance(UITabBarController viewController, ShellAppearance appearance)
        {
            //           base.SetAppearance(controller, appearance);
            _appearence = appearance;

            UITabBar tabBar = viewController.TabBar;


            bool legacy = !UIDevice.CurrentDevice.CheckSystemVersion(15, 0);

            if (tabBar.Items != null)
            {
                //init once
                if (_lastChildrenCount != tabBar.Items.Length || _appShell.TabsInvalidated || !onceHeight)
                {

                    _appShell.TabsInitialized = true;

                    _views = tabBar.Subviews.Where(x => x.UserInteractionEnabled).OrderBy(o => o.Frame.X).ToArray();

                    var shadowColor = _appShell.TabsShadowColor.ToUIColor();

                    tabBar.BarStyle = UIBarStyle.Default;
                    tabBar.Translucent = false;

                    if (_appShell.TabsShadowRadius > 0)
                    {
                        tabBar.Layer.ShadowOffset = new CGSize(width: 0, height: 0);
                        tabBar.Layer.ShadowRadius = (nfloat)_appShell.TabsShadowRadius;
                        tabBar.Layer.ShadowColor = shadowColor.CGColor;
                        tabBar.Layer.ShadowOpacity = 1.0f;
                    }

                    //tabBar.ShadowImage = ColorForNavBar(UIColor.FromRGB(250, 250, 250));

                    if (legacy)
                    {
                        var tint = appearance.TabBarBackgroundColor.ToUIColor();
                        Debug.WriteLine($"[TABS] Color: {appearance.TabBarBackgroundColor.ToHex()}");
                        tabBar.BarTintColor = tint;
                    }
                    //else
                    //{
                    //    var newheight = Super.BottomTabsHeight;
                    //    if (newheight > 0)
                    //    {
                    //        tabBar.Frame = new CGRect(0, 0, tabBar.Frame.Width, (int)(newheight * Super.Screen.Density));
                    //        Super.BottomTabsHeight = tabBar.Frame.Height / Super.Screen.Density;
                    //    }
                    //}

                    if (!onceHeight)
                    {
                        onceHeight = true;
                        var newheight = EnhancedShell.BottomTabsHeight;
                        if (newheight > 0)
                        {
                            if (_frame == CGRect.Empty)
                            {
                                var height = (int)(newheight + EnhancedShell.Screen.BottomInset);

                                var oldHeight = tabBar.Frame.Height;
                                var change = height - oldHeight;

                                _frame = new CGRect(0, tabBar.Frame.Y - change, tabBar.Frame.Width, height);
                            }

                            tabBar.Frame = _frame;
                        }
                        EnhancedShell.BottomTabsHeight = tabBar.Frame.Height / EnhancedShell.Screen.Density;
                    }


                    _lastChildrenCount = tabBar.Items.Length;

                    fontBadges = _ToNativeFont(_appShell.TabsBadgeTextFontFamily, (float)_appShell.TabsBadgeTextSize, FontAttributes.None);
                    fontLabels = _ToNativeFont(_appShell.TabBarLabelsFontFamily, (float)_appShell.TabBarLabelTextSize, FontAttributes.None);
                    fontLabelsSelected = _ToNativeFont(_appShell.TabBarLabelsFontFamily, (float)_appShell.TabBarSelectedLabelTextSize, FontAttributes.None);


                    if (!legacy)
                    {
                        var tabsAppearance = new UITabBarAppearance();
                        tabsAppearance.ConfigureWithOpaqueBackground();
                        tabsAppearance.BackgroundColor = appearance.TabBarBackgroundColor.ToUIColor();

                        tabsAppearance.StackedLayoutAppearance.Normal.BadgeBackgroundColor = _appShell.TabsBadgeColor.ToUIColor();
                        tabsAppearance.StackedLayoutAppearance.Normal.BadgeTextAttributes = GetBadgeStringAttributes();
                        tabsAppearance.StackedLayoutAppearance.Selected.BadgeBackgroundColor = tabsAppearance.StackedLayoutAppearance.Normal.BadgeBackgroundColor;
                        tabsAppearance.StackedLayoutAppearance.Selected.BadgeTextAttributes = tabsAppearance.StackedLayoutAppearance.Normal.BadgeTextAttributes;
                        tabsAppearance.InlineLayoutAppearance.Selected.BadgeBackgroundColor = tabsAppearance.StackedLayoutAppearance.Normal.BadgeBackgroundColor;
                        tabsAppearance.InlineLayoutAppearance.Selected.BadgeTextAttributes = tabsAppearance.StackedLayoutAppearance.Normal.BadgeTextAttributes;
                        tabsAppearance.CompactInlineLayoutAppearance.Selected.BadgeBackgroundColor = tabsAppearance.StackedLayoutAppearance.Normal.BadgeBackgroundColor;
                        tabsAppearance.CompactInlineLayoutAppearance.Selected.BadgeTextAttributes = tabsAppearance.StackedLayoutAppearance.Normal.BadgeTextAttributes;

                        tabsAppearance.StackedLayoutAppearance.Normal.TitleTextAttributes = GetStringAttributes(false);
                        tabsAppearance.StackedLayoutAppearance.Selected.TitleTextAttributes = GetStringAttributes(true);
                        tabsAppearance.InlineLayoutAppearance.Normal.TitleTextAttributes = tabsAppearance.StackedLayoutAppearance.Normal.TitleTextAttributes;
                        tabsAppearance.InlineLayoutAppearance.Selected.TitleTextAttributes = tabsAppearance.StackedLayoutAppearance.Selected.TitleTextAttributes;
                        tabsAppearance.CompactInlineLayoutAppearance.Normal.TitleTextAttributes = tabsAppearance.StackedLayoutAppearance.Normal.TitleTextAttributes;
                        tabsAppearance.CompactInlineLayoutAppearance.Selected.TitleTextAttributes = tabsAppearance.StackedLayoutAppearance.Selected.TitleTextAttributes;

                        var moveLabels = GetTitlesOffset();
                        tabsAppearance.StackedLayoutAppearance.Normal.TitlePositionAdjustment = moveLabels;
                        tabsAppearance.StackedLayoutAppearance.Selected.TitlePositionAdjustment = moveLabels;
                        tabsAppearance.InlineLayoutAppearance.Normal.TitlePositionAdjustment = moveLabels;
                        tabsAppearance.InlineLayoutAppearance.Selected.TitlePositionAdjustment = moveLabels;
                        tabsAppearance.CompactInlineLayoutAppearance.Normal.TitlePositionAdjustment = moveLabels;
                        tabsAppearance.CompactInlineLayoutAppearance.Selected.TitlePositionAdjustment = moveLabels;

                        tabsAppearance.ShadowColor = shadowColor;

                        tabBar.StandardAppearance = tabsAppearance;
                        tabBar.ScrollEdgeAppearance = tabBar.StandardAppearance;
                    }



                }

                var tabs = _appShell.CurrentItem.Items.Where(x => x.CurrentItem != null && x.CurrentItem.IsVisible).ToArray();
                var countChildren = tabs.Length;
                if (_lastEnabled != countChildren)
                {
                    _lastEnabled = countChildren;
                    _appShell.TabsInvalidated = true;
                }


                if (_appShell.TabsInvalidated)
                {
                    imagesSet = false;
                }

                //create skia icons
                _appShell.RenderTabBarIcons(_appShell.TabsInvalidated);

                if (countChildren != tabBar.Items.Length)
                {
                    //need to wait for views to be updated
                    Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
                    {
                        _appShell.InvalidateNavBar();
                        return false;
                    });
                    return;
                }

                for (int i = 0; i < tabBar.Items.Length; i++)
                {
                    var item = tabBar.Items[i];

                    var isSelected = item.Equals(tabBar.SelectedItem);// _appShell.SelectedIndex == i;

                    if (true)//(!imagesSet)
                    {
                        UIImage PrepareImage(EnhancedShell.RenderedIcon rendered)
                        {
                            UIImage iosBitmap = null;
                            if (rendered != null)
                            {
                                float iconSizeDp = (float)_appShell.TabBarIconSize;

                                var width = rendered.Bitmap.Width;
                                var height = rendered.Bitmap.Height;
                                //int iconSizePixels = (int)(iconSizeDp * _density);
                                var max = (float)Math.Max(rendered.Bitmap.Width, rendered.Bitmap.Height);
                                var widthAspect = width / max;
                                var heightAspect = height / max;
                                var bitmap = new SKBitmap(new SKImageInfo((int)(iconSizeDp * widthAspect), (int)(iconSizeDp * heightAspect)));
                                rendered.Bitmap.ScalePixels(bitmap, SKFilterQuality.High);

                                iosBitmap = bitmap.ToUIImage().ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                            }
                            return iosBitmap;
                        }

                        var unselected = _appShell.RenderedTabBarIcons
                            .FirstOrDefault(x =>
                                x.IsSelected == false &&
                                x.Index == i);
                        item.Image = PrepareImage(unselected);

                        var selected = _appShell.RenderedTabBarIcons
                            .FirstOrDefault(x =>
                                x.IsSelected == true &&
                                x.Index == i);
                        item.SelectedImage = PrepareImage(selected);

                        if (item.Image == null)
                        {
                            Debug.WriteLine($"[BUG] Still bugging at index {i} selected False...");
                        }
                        else
                        {
                            Debug.WriteLine($"[BUG] Set OK at index {i} selected False...");
                        }

                        if (item.SelectedImage == null)
                        {
                            Debug.WriteLine($"[BUG] Still bugging at index {i} selected True...");
                        }
                        else
                        {
                            Debug.WriteLine($"[BUG] Set OK at index {i} selected True...");
                        }
                    }

                    var content = tabs[i].CurrentItem;
                    if (content != null)
                    {
                        //BADGES
                        var text = EnhancedShell.GetBadgeText(content);
                        if (!string.IsNullOrEmpty(text) && text != "0")
                        {
                            if (legacy)
                            {
                                item.BadgeColor = _appShell.TabsBadgeColor.ToUIColor();
                                item.SetBadgeTextAttributes(GetBadgeStringAttributes(), UIControlState.Normal);
                                item.SetBadgeTextAttributes(GetBadgeStringAttributes(), UIControlState.Selected);
                                if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
                                {
                                    //On iOS 12, it seems setting the font using setBadgeTextAttributes does not work
                                    //var tab = _views[i];
                                    //foreach (var subview in tab.Subviews)
                                    //{
                                    //   var describingType = subview.GetType().Name;

                                    //   Debug.WriteLine($"[VIEWS] {i}:{describingType}");

                                    //    if (describingType == "_UIBadgeView")
                                    //    {
                                    //        foreach (var view in subview.Subviews)
                                    //        {
                                    //            var label = view as UILabel;
                                    //            if (label != null)
                                    //            {
                                    //                label.Font = fontBadges;
                                    //                break;
                                    //            }
                                    //        }
                                    //        break;
                                    //    }
                                    //}
                                }
                            }
                            item.BadgeValue = text;
                        }
                        else
                        {
                            item.BadgeValue = null;
                        }

                        //TITLES
                        if (!_appShell.TabBarHideLabels)
                        {
                            if (legacy)
                            {
                                UITextAttributes normalTextAttributes = GetTextAttributes(isSelected);
                                item.SetTitleTextAttributes(normalTextAttributes, UIControlState.Normal);
                                item.TitlePositionAdjustment = new UIOffset(0, AppleTitlesOffset * _density);
                                item.ImageInsets = new UIEdgeInsets(ios_tabs_yoffset, 0, -ios_tabs_yoffset, 0);
                            }
                            item.Title = content.Title;
                        }
                        else
                        {
                            // if (legacy)
                            item.ImageInsets = new UIEdgeInsets(ios_tabs_yoffset_notxt, 0, -ios_tabs_yoffset_notxt, 0);
                            item.Title = null;
                        }
                    }
                    else
                    {
                        item.BadgeValue = null;
                    }


                }
                imagesSet = true;

                _appShell.TabsInvalidated = false;
            }

        }

        void Animate(UIView view)
        {
            if (view != null)
            {
                var animation = CGAffineTransform.MakeScale(0.95f, 0.95f);
                UIView.AnimateNotify(0.4, 0.0,
                    0.5f, 3f,
                    UIViewAnimationOptions.CurveEaseInOut,
                    () => { view.Transform = animation; },
                    null);
            }
        }



        public void UpdateLayout(UITabBarController controller)
        {
            if (_appearence != null)
            {
                SetAppearance(controller, _appearence);
            }
        }




        static readonly string _defaultFontName = UIFont.SystemFontOfSize(12).Name;

        private UIView[] _views;
        private ShellAppearance _appearence;
        private int _lastEnabled;

        static CGRect _frame = CGRect.Empty;

        static UIFont _ToNativeFont(string family, float size, FontAttributes attributes)
        {
            var bold = (attributes & FontAttributes.Bold) != 0;
            var italic = (attributes & FontAttributes.Italic) != 0;

            if (family != null && family != _defaultFontName)
            {
                try
                {
                    UIFont result = null;
                    if (UIFont.FamilyNames.Contains(family))
                    {
                        var descriptor = new UIFontDescriptor().CreateWithFamily(family);

                        if (bold || italic)
                        {
                            var traits = (UIFontDescriptorSymbolicTraits)0;
                            if (bold)
                                traits = traits | UIFontDescriptorSymbolicTraits.Bold;
                            if (italic)
                                traits = traits | UIFontDescriptorSymbolicTraits.Italic;

                            descriptor = descriptor.CreateWithTraits(traits);
                            result = UIFont.FromDescriptor(descriptor, size);
                            if (result != null)
                                return result;
                        }
                    }

                    var cleansedFont = CleanseFontName(family);
                    result = UIFont.FromName(cleansedFont, size);
                    if (family.StartsWith(".SFUI", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        var fontWeight = family.Split('-').LastOrDefault();

                        if (!string.IsNullOrWhiteSpace(fontWeight) && System.Enum.TryParse<UIFontWeight>(fontWeight, true, out var uIFontWeight))
                        {
                            result = UIFont.SystemFontOfSize(size, uIFontWeight);
                            return result;
                        }

                        result = UIFont.SystemFontOfSize(size, UIFontWeight.Regular);
                        return result;
                    }
                    if (result == null)
                        result = UIFont.FromName(family, size);
                    if (result != null)
                        return result;
                }
                catch
                {
                    Debug.WriteLine("Could not load font named: {0}", family);
                }
            }

            if (bold && italic)
            {
                var defaultFont = UIFont.SystemFontOfSize(size);

                var descriptor = defaultFont.FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold | UIFontDescriptorSymbolicTraits.Italic);
                return UIFont.FromDescriptor(descriptor, 0);
            }

            if (italic)
                return UIFont.ItalicSystemFontOfSize(size);

            if (bold)
                return UIFont.BoldSystemFontOfSize(size);

            return UIFont.SystemFontOfSize(size);
        }

        internal static string CleanseFontName(string fontName)
        {

            //First check Alias
            var (hasFontAlias, fontPostScriptName) = FontRegistrar.HasFont(fontName);
            if (hasFontAlias)
                return fontPostScriptName;

            var fontFile = FontFile.FromString(fontName);

            if (!string.IsNullOrWhiteSpace(fontFile.Extension))
            {
                var (hasFont, filePath) = FontRegistrar.HasFont(fontFile.FileNameWithExtension());
                if (hasFont)
                    return filePath ?? fontFile.PostScriptName;
            }
            else
            {
                foreach (var ext in FontFile.Extensions)
                {

                    var formated = fontFile.FileNameWithExtension(ext);
                    var (hasFont, filePath) = FontRegistrar.HasFont(formated);
                    if (hasFont)
                        return filePath;
                }
            }
            return fontFile.PostScriptName;
        }


    }
}