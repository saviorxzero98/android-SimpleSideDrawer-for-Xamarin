using Android.Views;

namespace AndroidAdapter.NavDrawer.DrawerDragAction
{
    public class LeftDragAction : AbstractDragAction
    {
        public override AbstractDragAction BindView(SimpleSideDrawer drawer)
        {
            _drawer = drawer;
            return this;
        }

        public override bool OnTouchEvent(MotionEvent ev)
        {
            int action = (int)ev.Action & (int)MotionEventActions.Mask;
            switch (action)
            {
                case (int)MotionEventActions.Down:
                    _drawer.LeftDragAction.LastMotionX = ev.GetX();
                    _drawer.LeftDragAction.Draggable = _drawer.AboveView.ScrollX != 0;
                    break;
                case (int)MotionEventActions.Up:
                    if (_drawer.LeftDragAction.Draggable)
                    {
                        int currentX = _drawer.AboveView.ScrollX;
                        int diffX = 0;
                        if (_drawer.LeftDragAction.Opening)
                        {
                            diffX = -(_drawer.LeftBehindViewWidth + currentX);
                        }
                        else
                        {
                            diffX = -currentX;
                        }
                        _drawer.Scroller.StartScroll(currentX, 0, diffX, 0, _drawer.DurationLeft);
                        _drawer.Invalidate();
                    }
                    break;
                case (int)MotionEventActions.Move:
                    if (!_drawer.LeftDragAction.Draggable) return false;

                    float newX = ev.GetX();
                    float difffX = -(newX - _drawer.LeftDragAction.LastMotionX);
                    int x = _drawer.AboveView.ScrollX;
                    _drawer.LeftDragAction.Opening = _drawer.LeftDragAction.LastMotionX < newX;
                    _drawer.LeftDragAction.LastMotionX = newX;
                    float nextX = x + difffX;
                    if (0 < nextX)
                    {
                        _drawer.AboveView.ScrollTo(0, 0);
                    }
                    else
                    {
                        if (nextX < -_drawer.LeftBehindViewWidth)
                        {
                            _drawer.AboveView.ScrollTo(-_drawer.LeftBehindViewWidth, 0);
                        }
                        else
                        {
                            _drawer.AboveView.ScrollBy((int)difffX, 0);
                        }
                    }
                    break;
            }
            return false;
        }
    }
}