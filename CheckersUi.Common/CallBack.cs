using System;

namespace CheckersUiWf
{
    public class CallBack
    {
        public CallBack() { }

        //
        // Utility Callbacks
        //
        public virtual void Trace(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
        public virtual void OutPut(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
        public virtual void Panic(string text)
        {
            Trace($"Panic: {text}");
            throw new Exception(text);
        }

        //
        // Application Callbacks
        //
        public virtual void OnKeyPress(char key)
        {
            Trace($"OnKeyPress key {key}");
        }

        //
        // Board Callbacks
        //
        public virtual void MouseClick(int square)
        {
            Trace($"MouseClick Square {square}");
        }
        public virtual void MouseDoubleClick(int square)
        {
            Trace($"MouseDoubleClick Square {square}");
        }

        //
        // Move Callbacks
        //
        public virtual void MoveSelect(MoveId moveId)
        {
            Trace($"CallBack MoveSelect moveId={moveId}");
        }
        //
        // Menu Callbacks
        //
        public virtual bool MenuItemSelect(string menuName)
        {
            Trace($"Unhandled menu item click {menuName}");
            return false;
        }
    }
}
