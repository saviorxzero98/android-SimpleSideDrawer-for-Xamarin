using Android.Views;

namespace AndroidAdapter.NavDrawer.DrawerDragAction
{
    public abstract class AbstractDragAction
    {
        protected SimpleSideDrawer _drawer { get; set; }

        public float LastMotionX { get; set; } = 0;
        public bool Opening { get; set; } = false;
        public bool Draggable { get; set; } = false;

        public abstract AbstractDragAction BindView(SimpleSideDrawer drawer);

        public abstract bool OnTouchEvent(MotionEvent motionEvent);


    }
}