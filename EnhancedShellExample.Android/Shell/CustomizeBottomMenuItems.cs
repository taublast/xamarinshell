using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using EnhancedShellExample.Infrastructure;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Internal;
using SkiaSharp.Views.Android;
using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

namespace AppoMobi.Framework.Droid.Renderers
{
    /// <summary>
    /// BottomNavigationView is the whole TabBar
    /// </summary>
    public class CustomizeBottomMenuItems : ShellBottomNavViewAppearanceTracker
    {


        public override void ResetAppearance(BottomNavigationView bottomView)
        {
            base.ResetAppearance(bottomView);

            var appearence = _tabBar as IShellAppearanceElement;

            //SetAppearance(bottomView, appearence);
        }

        ColorStateList _colorStateList;

        ColorStateList MakeColorStateList(Color titleColor, Color disabledColor, Color unselectedColor)
        {
            var disabledInt = disabledColor.ToAndroid().ToArgb();

            var checkedInt = titleColor.ToAndroid().ToArgb();

            var defaultColor = unselectedColor.ToAndroid().ToArgb();

            return MakeColorStateList(checkedInt, disabledInt, defaultColor);
        }

        ColorStateList MakeColorStateList(int titleColorInt, int disabledColorInt, int defaultColor)
        {
            var states = new int[][] {
                new int[] { -Android.Resource.Attribute.StateEnabled },
                new int[] { Android.Resource.Attribute.StateChecked },
                new int[] { }
            };

            var colors = new[] { disabledColorInt, titleColorInt, defaultColor };

            return new ColorStateList(states, colors);
        }

        private int _lastChildrenCount;

        protected Typeface fontLabels;
        protected Typeface fontBadges;


        private int _lastEnabled = -1;
        public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            base.SetAppearance(bottomView, appearance);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                var p = bottomView.LayoutParameters as ViewGroup.MarginLayoutParams;

                var newheight = EnhancedShell.BottomTabsHeight;

                if (newheight > 0)
                {
                    p.Height = (int)(newheight * EnhancedShell.Screen.Density);
                    bottomView.LayoutParameters = p;
                }

                EnhancedShell.BottomTabsHeight = bottomView.LayoutParameters.Height / EnhancedShell.Screen.Density;
            }

            var check = bottomView.PaddingTop;

            var backgroudColor = Shell.GetTabBarBackgroundColor(_tabBar);

            bottomView.Background = new ColorDrawable(backgroudColor.ToAndroid());

            bottomView.SetElevation(16);
            //            bottomView.SetElevation(1*_density);

            if (_appShell == null)
                return;


            var menuView = bottomView.GetChildAt(0) as BottomNavigationMenuView;
            if (menuView == null)
            {
                System.Diagnostics.Debug.WriteLine("Unable to find BottomNavigationMenuView");
                return;
            }

            bool enableShiftMode = false;

            var tabs = _appShell.CurrentItem.Items.Where(x => x.CurrentItem != null && x.CurrentItem.IsVisible).ToArray();

            var countChildren = tabs.Length;

            if (_lastEnabled != countChildren)
            {
                _lastEnabled = countChildren;
                _appShell.TabsInvalidated = true;
            }

            if (_lastChildrenCount != menuView.ChildCount)
                _appShell.TabsInvalidated = true;

            if (_appShell.TabsInvalidated)
            {
                if (!string.IsNullOrEmpty(_appShell.TabBarLabelsFontFamily))
                {
                    var info = FontRegistrar.HasFont(_appShell.TabBarLabelsFontFamily);
                    if (info.hasFont)
                    {
                        fontLabels = Typeface.CreateFromFile(info.fontPath);//.createFromAsset(view.context.assets, "fonts/customFont");
                    }
                }

                if (!string.IsNullOrEmpty(_appShell.TabsBadgeTextFontFamily))
                {
                    var info = FontRegistrar.HasFont(_appShell.TabsBadgeTextFontFamily);
                    if (info.hasFont)
                    {
                        fontBadges = Typeface.CreateFromFile(info.fontPath);//.createFromAsset(view.context.assets, "fonts/customFont");
                    }
                }

                _lastChildrenCount = menuView.ChildCount;

                var backgroundColor = appearance.EffectiveTabBarBackgroundColor;
                var foregroundColor = appearance.EffectiveTabBarForegroundColor; // currently unused

                var disabledColor = appearance.EffectiveTabBarDisabledColor;
                var unselectedColor = appearance.EffectiveTabBarUnselectedColor;
                var titleColor = appearance.EffectiveTabBarTitleColor;

                if (_colorStateList == null || _appShell.TabsInvalidated)
                    _colorStateList = MakeColorStateList(titleColor, disabledColor, unselectedColor);


                float iconSizeDp = (float)_appShell.TabBarIconSize;
                int iconSizePixels = (int)(iconSizeDp * _density);

                //var shiftMode = menuView.Class.GetDeclaredField("mShiftingMode");
                //shiftMode.Accessible = true;
                //shiftMode.SetBoolean(menuView, enableShiftMode);
                //shiftMode.Accessible = false;
                //shiftMode.Dispose();

                for (int i = 0; i < menuView.ChildCount; i++)
                {
                    var item = menuView.GetChildAt(i) as BottomNavigationItemView;
                    if (item == null)
                        continue;

                    bool isSelected = item.ItemData.IsChecked;


                    //remove scaling
                    item.SetChecked(isSelected);

                    item.SetIconSize(iconSizePixels);

                    // var back = new ColorDrawable(Android.Graphics.Color.Red);
                    //back.SetColorFilter(Android.Graphics.Color.Beige, PorterDuff.Mode.DstIn);
                    //item.Background = back;

                    item.SetIconTintList(_colorStateList);
                    // item.SetIconTintList(null);

                    //if (isSelected)
                    //{
                    //        item.SetIconTintList(null);

                    //}
                    //else
                    //{
                    //    item.SetIconTintList(_colorStateList);
                    //}

                    //attach badge view

                    AttachBadgeView(item);

                }
            }

            //create skia icons
            _appShell.RenderTabBarIcons(_appShell.TabsInvalidated);

            if (countChildren != menuView.ChildCount)
            {
                //need to wait for views to be updated
                Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
                {
                    _appShell.InvalidateNavBar();
                    return false;
                });
                return;
            }

            for (int i = 0; i < menuView.ChildCount; i++)
            {
                var item = menuView.GetChildAt(i) as BottomNavigationItemView;
                if (item == null)
                    continue;

                var content = tabs[i].CurrentItem;
                //var selectedContent = _appShell.CurrentItem.Items[_appShell.SelectedIndex];

                var isSelected = item.Selected;//_appShell.SelectedIndex == i;

                //do not apply any colors to our incoming bitmap
                item.SetIconTintList(null);

                //create icon bitmap by hand
                //if (i >= 0 && i < _appShell.RenderedTabBarIcons.Count)
                //{
                //    var rendered = _appShell.RenderedTabBarIcons
                //        .FirstOrDefault(x => x.IsSelected == isSelected && x.Index == i);
                //    if (rendered!=null)
                //    {
                //        var androidBitmap = rendered.Bitmap.ToBitmap();
                //        var iconDrawable = new BitmapDrawable(Android.App.Application.Context.Resources, androidBitmap);
                //        item.SetIcon(iconDrawable);
                //    }
                //}

                var rendered = _appShell.RenderedTabBarIcons
                    .FirstOrDefault(x => x.IsSelected == isSelected && x.Index == i);
                if (rendered != null)
                {
                    var androidBitmap = rendered.Bitmap.ToBitmap();
                    var iconDrawable = new BitmapDrawable(Android.App.Application.Context.Resources, androidBitmap);
                    item.SetIcon(iconDrawable);
                }
                else
                {
                    var stop = 6;
                }

                // labels
                if (_appShell.TabBarHideLabels)
                    item.SetLabelVisibilityMode(LabelVisibilityMode.LabelVisibilityUnlabeled);
                else
                {


                    //every BottomNavigationItemView has two children, first is an itemIcon and second is an itemTitle
                    var itemTitle = item.GetChildAt(1);
                    //every itemTitle has two children, first is a smallLabel and second is a largeLabel. these two are type of AppCompatTextView
                    var labelUnselected = (TextView)((BaselineLayout)itemTitle).GetChildAt(0);
                    var labelSelected = (TextView)((BaselineLayout)itemTitle).GetChildAt(1);

                    if (fontLabels != null)
                    {
                        labelSelected.SetTypeface(fontLabels, TypefaceStyle.Normal);
                        labelUnselected.SetTypeface(fontLabels, TypefaceStyle.Normal);
                    }

                    labelUnselected.SetTextSize(ComplexUnitType.Dip, (float)_appShell.TabBarLabelTextSize);
                    labelUnselected.SetTextColor(_appShell.TabsLabelColor.ToAndroid());

                    labelSelected.SetTextSize(ComplexUnitType.Dip, (float)_appShell.TabBarSelectedLabelTextSize);
                    labelSelected.SetTextColor(_appShell.TabsSelectedLabelColor.ToAndroid());

                    item.SetLabelVisibilityMode(LabelVisibilityMode.LabelVisibilityAuto);


                }

                //todo do something with this shit not working when labels are on
                item.SetShifting(enableShiftMode);

                var badge = GetBadgeView(item);
                if (badge != null)
                {
                    if (content != null)
                    {
                        var text = EnhancedShell.GetBadgeText(content);

                        badge?.SetValue(text);
                    }
                }


                //int menuItemId = bottomView.Menu.GetItem(i).ItemId;
                //BadgeDrawable badge = bottomView.GetOrCreateBadge(menuItemId);
                //badge.Number = 2+i;


            }


            _appShell.TabsInvalidated = false;
        }

        protected void AttachBadgeView(BottomNavigationItemView bottomNavigationView)
        {
            var badge = bottomNavigationView.GetChildrenOfType<BadgeView>().FirstOrDefault();

            if (badge == null)
            {
                bottomNavigationView.SetClipChildren(false);
                bottomNavigationView.SetClipToPadding(false);
            }
            else
            {
                bottomNavigationView.RemoveView(badge);
            }

            badge = new BadgeView(bottomNavigationView.Context);
            bottomNavigationView.AddView(badge);

            badge.Label.SetTypeface(fontBadges, TypefaceStyle.Normal);
            badge.Label.TextSize = (float)_appShell.TabsBadgeTextSize;
            badge.LabelMargins = _appShell.TabsBadgeTextMargin;

            badge.MinWidthDp = 14;
            badge.CornerRadius = 12;

            badge.TextColor = _appShell.TabsBadgeTextColor.ToAndroid();
            badge.BackgroundColor = _appShell.TabsBadgeColor.ToAndroid();
            badge.OffsetX = (float)_appShell.TabsBadgeOffsetX;
            badge.OffsetY = (float)_appShell.TabsBadgeOffsetY;

        }

        protected BadgeView GetBadgeView(BottomNavigationItemView item)
        {
            return item.GetChildrenOfType<BadgeView>().FirstOrDefault();
        }

        EnhancedShell _appShell;

        IShellContext _shellContext;

        private readonly float _density;

        private TabBar _tabBar;

        public CustomizeBottomMenuItems(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem)
        {
            //_shellItem = shellItem;

            var renderer = shellContext as AppShellRenderer;

            _tabBar = shellItem as TabBar;

            _appShell = renderer?.FormsControl;

            if (_appShell != null)
                _appShell.PropertyChanged += OnShellPropertyChanged;

            _density = Android.App.Application.Context.Resources.DisplayMetrics.Density;

        }

        private void OnShellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedIndex")
            {
                System.Diagnostics.Debug.WriteLine($"[NATIVE] selected tab is: {_appShell.SelectedIndex}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_appShell != null)
                _appShell.PropertyChanged -= OnShellPropertyChanged;

            base.Dispose(disposing);
        }
    }
}