using System;

namespace System.Enginee
{
    public static class ExceptionHelper
    {
        public static void IgnorlException(this Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
                //ignorl
            }
        }
    }
}