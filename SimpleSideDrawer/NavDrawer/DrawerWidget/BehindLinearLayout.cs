using Android.Content;
using Android.Graphics;
using Android.Widget;

namespace AndroidAdapter.NavDrawer.DrawerWidget
{
    public class BehindLinearLayout : LinearLayout
    {
        protected SimpleSideDrawer _drawer { get; set; }

        public BehindLinearLayout(Context context) : base(context)
        {
        }

        public BehindLinearLayout BindView(SimpleSideDrawer drawer)
        {
            _drawer = drawer;
            return this;
        }

        public void FitDisplay(Rect rect)
        {
            _drawer.BehindView.SetPadding(rect.Left, rect.Top, 0, 0);
            RequestLayout();
        }
    }
}