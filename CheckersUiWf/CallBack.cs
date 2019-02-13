using System;
using System.Windows.Forms;

namespace CheckersUiWf
{
    public class CallBack
    {
        public CallBack() { }

        //
        // Utility Callbacks
        //
        public virtual void InitializeCallout(MenuStrip menuStrip1)
        {
            Trace("Initialize User Callout and Menus");
        }
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
            Trace("Panic: " + text);
            throw new Exception(text);
        }
        //
        // Application Callbacks
        //
        public virtual void OnKeyPress(char key)
        {
            Trace("OnKeyPress key " + key.ToString());
        }
        public virtual bool OnCmdKey(Keys keyData)
        {
            Trace("OnCmdKey " + keyData.ToString());
            return false;
        }
        //
        // Board Callbacks
        //
        public virtual void MouseClick(int square)
        {
            Trace("MouseClick Square " + square.ToString());
        }
        public virtual void MouseDoubleClick(int square)
        {
            Trace("MouseDoubleClick Square " + square.ToString());
        }
        public virtual void BoardClick(int row, int col, int square)
        {
            //Trace(string.Format("BoardClick: row {0} col {1} square {2}", row, col, square));
        }
        //
        // Move Callbacks
        //
        public virtual void MoveSelect(MoveId moveId)
        {
            Trace("CallBack MoveSelect moveId=" + moveId.ToString());
        }
        //
        // Menu Callbacks
        //
        public virtual bool ToolStripMenuItem_DropDownItemClicked(
            object sender, ToolStripItemClickedEventArgs e)
        {
            Trace("Unhandled menu item click " + e.ClickedItem.Text);
            return false;
        }
    }
}
