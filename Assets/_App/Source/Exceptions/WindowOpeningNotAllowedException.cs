using System;

namespace Omega.Kulibin
{
    public class WindowOpeningNotAllowedException : Exception
    {
        public WindowOpeningNotAllowedException(IWindowPresenter window) :
            base($"{window.GetType()} opening not allowed at the moment")
        {
        }
    }
}