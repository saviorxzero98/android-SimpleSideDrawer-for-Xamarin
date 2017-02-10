using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using AndroidAdapter.NavDrawer.DrawerDragAction;
using AndroidAdapter.NavDrawer.DrawerWidget;
using static AndroidAdapter.NavDrawer.DrawerWidget.OverlayView;

namespace AndroidAdapter.NavDrawer
{
    public class SimpleSideDrawer : FrameLayout
    {
        protected Window _window { get; set; }
        public ViewGroup AboveView { get; set; }
        public BehindLinearLayout BehindView { get; set; }
        public LinearLayout LeftBehindBase { get; set; }
        public LinearLayout RightBehindBase { get; set; }
        protected View _overlay { get; set; }

        public Scroller Scroller { get; set; }
        protected View _leftBehindView { get; set; }   //menu of left-behind will be set
        protected View _rightBehindView { get; set; }  //menu of right-behind will be set
        protected Rect _leftPaddingRect { get; set; }
        protected Rect _rightPaddingRect { get; set; }
        public int DurationLeft { get; set; }
        public int DurationRight { get; set; }
        public int LeftBehindViewWidth { get; set; }
        public int RightBehindViewWidth { get; set; }

        public AbstractDragAction LeftDragAction { get; set; }
        public AbstractDragAction RightDragAction { get; set; }


        public SimpleSideDrawer(Activity act) : this(act, new DecelerateInterpolator(0.9f), 180)
        {
        }

        public SimpleSideDrawer(Activity act, IInterpolator ip, int duration) : base(act.ApplicationContext)
        {
            LeftDragAction = new LeftDragAction().BindView(this);
            RightDragAction = new RightDragAction().BindView(this);
            Context context = act.ApplicationContext;
            DurationLeft = duration;
            DurationRight = duration;
            _window = act.Window;
            Scroller = new Scroller(context, ip);

            int fp = LayoutParams.MatchParent;
            int wp = LayoutParams.WrapContent;
            //behind
            BehindView = new BehindLinearLayout(context).BindView(this);
            BehindView.LayoutParameters = new LinearLayout.LayoutParams(fp, fp);
            BehindView.Orientation = Orientation.Horizontal;
            //left-behind base
            LeftBehindBase = new BehindLinearLayout(context).BindView(this);
            BehindView.AddView(LeftBehindBase, new LinearLayout.LayoutParams(wp, fp));
            //behind adjusting view
            BehindView.AddView(new View(context), new LinearLayout.LayoutParams(0, fp, 1));
            //right-behind base
            RightBehindBase = new BehindLinearLayout(context).BindView(this);
            BehindView.AddView(RightBehindBase, new LinearLayout.LayoutParams(wp, fp));

            AddView(BehindView);

            //above
            AboveView = new FrameLayout(context);
            AboveView.LayoutParameters = new FrameLayout.LayoutParams(fp, fp);
            //overlay is used for controlling drag action, slid to close/open.
            _overlay = new OverlayView(Context).BindView(this);
            _overlay.LayoutParameters = new FrameLayout.LayoutParams(fp, fp, GravityFlags.Bottom);
            _overlay.Enabled = true;
            _overlay.Visibility = ViewStates.Gone;
            _overlay.SetOnClickListener(new OverlayOnClickListener().BindView(this));

            ViewGroup decor = (ViewGroup)_window.DecorView;
            ViewGroup above = (ViewGroup)decor.GetChildAt(0);//including actionbar
            decor.RemoveView(above);
            above.Background = decor.Background;        // TODO
            AboveView.AddView(above);
            AboveView.AddView(_overlay);
            decor.AddView(this);

            AddView(AboveView);
        }


        public View LeftBehindView
        {
            get
            {
                return LeftBehindBase.GetChildAt(0);
            }
        }

        public View RightBehindView
        {
            get
            {
                return RightBehindBase.GetChildAt(0);
            }
        }

        public View SetBehindContentView(int leftBehindLayout)
        {
            return SetLeftBehindContentView(leftBehindLayout);
        }

        public View SetLeftBehindContentView(int leftBehindLayout)
        {
            View content = ((LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService)).Inflate(leftBehindLayout, LeftBehindBase);
            _leftPaddingRect = new Rect(content.PaddingLeft, content.PaddingTop, content.PaddingRight, content.PaddingBottom);
            _leftBehindView = content;
            return content;
        }


        public View SetRightBehindContentView(int rightBehindLayout)
        {

            View content = ((LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService)).Inflate(rightBehindLayout, RightBehindBase);
            _rightPaddingRect = new Rect(content.PaddingLeft, content.PaddingTop, content.PaddingRight, content.PaddingBottom);
            _rightBehindView = content;
            return content;
        }


        public void SetScrollInterpolator(IInterpolator ip)
        {
            Scroller = new Scroller(Context, ip);
        }


        public void SetAnimationDuration(int msec)
        {
            AnimationDurationLeft = msec;
        }


        public int AnimationDurationLeft
        {
            set
            {
                DurationLeft = value;
            }
        }

        public int AnimationDurationRight
        {
            set
            {
                DurationRight = value;
            }
        }

        public void Close()
        {
            CloseLeftSide();
        }

        public void CloseLeftSide()
        {
            int curX = -LeftBehindViewWidth;//mAboveView.getScrollX();
            Scroller.StartScroll(curX, 0, -curX, 0, DurationLeft);
            Invalidate();
        }


        public void CloseRightSide()
        {
            int curX = RightBehindViewWidth;//mAboveView.getScrollX();
            Scroller.StartScroll(curX, 0, -curX, 0, DurationRight);
            Invalidate();
        }


        public void Open()
        {
            OpenLeftSide();
        }


        public void OpenLeftSide()
        {
            LeftBehindBase.Visibility = ViewStates.Visible;
            RightBehindBase.Visibility = ViewStates.Gone;

            int curX = AboveView.ScrollX;
            Scroller.StartScroll(curX, 0, -LeftBehindViewWidth, 0, DurationLeft);
            Invalidate();
        }

        public void OpenRightSide()
        {
            RightBehindBase.Visibility = ViewStates.Visible;
            LeftBehindBase.Visibility = ViewStates.Gone;

            int curX = AboveView.ScrollX;
            Scroller.StartScroll(curX, 0, RightBehindViewWidth, 0, DurationRight);
            Invalidate();
        }

        public void ToggleDrawer()
        {
            ToggleLeftDrawer();
        }


        public void ToggleLeftDrawer()
        {
            if (IsClosed)
            {
                OpenLeftSide();
            }
            else
            {
                CloseLeftSide();
            }
        }

        public void ToggleRightDrawer()
        {
            if (IsClosed)
            {
                OpenRightSide();
            }
            else
            {
                CloseRightSide();
            }
        }

        public bool IsClosed
        {
            get
            {
                return AboveView != null && AboveView.ScrollX == 0;
            }
        }

        private bool IsLeftSideOpened
        {
            get
            {
                return LeftBehindBase.Visibility == ViewStates.Visible && RightBehindBase.Visibility == ViewStates.Gone;
            }
        }

        private bool IsRightSideOpened
        {
            get
            {
                return RightBehindBase.Visibility == ViewStates.Visible && LeftBehindBase.Visibility == ViewStates.Gone;
            }
        }


        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            LeftBehindViewWidth = LeftBehindBase.MeasuredWidth;
            RightBehindViewWidth = RightBehindBase.MeasuredWidth;

            //adjust the behind display area
            ViewGroup decor = (ViewGroup)_window.DecorView;
            Rect rect = new Rect();
            decor.GetWindowVisibleDisplayFrame(rect);
            BehindView.FitDisplay(rect);
        }


        public override void ComputeScroll()
        {
            if (Scroller.ComputeScrollOffset())
            {
                AboveView.ScrollTo(Scroller.CurrX, Scroller.CurrY);
                Invalidate();
            }
            else
            {
                if (AboveView.ScrollX == 0)
                {
                    _overlay.Visibility = ViewStates.Gone;
                    LeftBehindBase.Visibility = ViewStates.Gone;
                    RightBehindBase.Visibility = ViewStates.Gone;
                }
                else
                {
                    _overlay.Visibility = ViewStates.Visible;
                }
            }
        }

        public override bool OnTouchEvent(MotionEvent ev)
        {
            if (IsLeftSideOpened)
            {
                return LeftDragAction.OnTouchEvent(ev);
            }
            else if (IsRightSideOpened)
            {
                return RightDragAction.OnTouchEvent(ev);
            }
            else
            {
                return true;
            }
        }
    }
}