using Android.Content;
using Android.Views;
using System;

namespace AndroidAdapter.NavDrawer.DrawerWidget
{
    public class OverlayView : View
    {
        public const float CLICK_RANGE = 3;
        protected float _downX { get; set; }
        protected float _downY { get; set; }
        protected IOnClickListener _clickListener { get; set; }
        protected SimpleSideDrawer _drawer { get; set; }

        public OverlayView(Context context) : base(context)
        {
        }

        public OverlayView BindView(SimpleSideDrawer drawer)
        {
            _drawer = drawer;
            return this;
        }

        public override void SetOnClickListener(IOnClickListener listener)
        {
            _clickListener = listener;
            base.SetOnClickListener(listener);
        }

        public override bool OnTouchEvent(MotionEvent ev)
        {
            ev.SetLocation(ev.GetX() - _drawer.AboveView.ScrollX, 0);
            _drawer.OnTouchEvent(ev);
            int action = (int)ev.Action & (int)MotionEventActions.Mask;
            float x = ev.GetX();
            float y = ev.GetY();
            if (action == (int)MotionEventActions.Down)
            {
                _downX = x;
                _downY = y;
            }
            else if (action == (int)MotionEventActions.Up)
            {
                if (_clickListener != null)
                {
                    if (Math.Abs(_downX - x) < CLICK_RANGE && Math.Abs(_downY - y) < CLICK_RANGE)
                    {
                        _clickListener.OnClick(this);
                    }
                }
            }
            return true;
        }

        public class OverlayOnClickListener : Java.Lang.Object, IOnClickListener
        {
            protected SimpleSideDrawer _drawer;

            public OverlayOnClickListener BindView(SimpleSideDrawer drawer)
            {
                _drawer = drawer;
                return this;
            }

            public void OnClick(View v)
            {
                if (_drawer.LeftBehindBase.Visibility != ViewStates.Gone)
                {
                    _drawer.CloseLeftSide();
                }
                else if (_drawer.RightBehindBase.Visibility != ViewStates.Gone)
                {
                    _drawer.CloseRightSide();
                }
            }
        }
    }
}