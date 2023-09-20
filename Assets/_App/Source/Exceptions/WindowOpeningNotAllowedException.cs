using System;

namespace MaxFluff.Prototypes
{
    public class WindowOpeningNotAllowedException : Exception
    {
        public WindowOpeningNotAllowedException(IWindowPresenter window) :
            base($"{window.GetType()} opening not allowed at the moment")
        {
        }
    }
}