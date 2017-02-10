using Android.Views;

namespace AndroidAdapter.NavDrawer.DrawerDragAction
{
    public class RightDragAction : AbstractDragAction
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
                    _drawer.RightDragAction.LastMotionX = ev.GetX();
                    _drawer.RightDragAction.Draggable = _drawer.AboveView.ScrollX != 0;
                    break;
                case (int)MotionEventActions.Up:
                    if (_drawer.RightDragAction.Draggable)
                    {
                        int currentX = _drawer.AboveView.ScrollX;
                        int diffX = 0;
                        if (_drawer.RightDragAction.Opening)
                        {
                            diffX = _drawer.RightBehindViewWidth - currentX;
                        }
                        else
                        {
                            diffX = -currentX;
                        }
                        _drawer.Scroller.StartScroll(currentX, 0, diffX, 0, _drawer.DurationRight);
                        _drawer.Invalidate();
                    }
                    break;
                case (int)MotionEventActions.Move:
                    if (!_drawer.RightDragAction.Draggable) return false;

                    float newX = ev.GetX();
                    float difffX = -(newX - _drawer.RightDragAction.LastMotionX);
                    int x = _drawer.AboveView.ScrollX;
                    _drawer.RightDragAction.Opening = newX < _drawer.RightDragAction.LastMotionX;
                    _drawer.RightDragAction.LastMotionX = newX;
                    float nextX = x + difffX;
                    if (nextX < 0)
                    {
                        _drawer.AboveView.ScrollTo(0, 0);
                    }
                    else
                    {
                        if (nextX < _drawer.RightBehindViewWidth)
                        {
                            _drawer.AboveView.ScrollBy((int)difffX, 0);
                        }
                        else
                        {
                            _drawer.AboveView.ScrollTo(_drawer.RightBehindViewWidth, 0);
                        }
                    }
                    break;
            }
            return false;
        }
    }
}