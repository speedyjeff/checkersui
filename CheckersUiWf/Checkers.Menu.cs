using System;
using System.IO;
using System.Windows.Forms;
using static CheckersUiWf.Boundary;

namespace CheckersUiWf
{
    partial class Checkers
    {
        private void InitializeMenu(MenuStrip menuStrip1)
        {
            // openToolStripMenuItem
            var openToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            openToolStripMenuItem.Name = OpenMenuItemName;
            openToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.O)));
            openToolStripMenuItem.Text = "&Open";

            // toolStripSeparator1
            var toolStripSeparator1 = new ToolStripSeparator();
            toolStripSeparator1.Name = "toolStripSeparator1";

            // exitToolStripMenuItem
            var exitToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem.Name = ExitMenuItemName;
            exitToolStripMenuItem.Text = "E&xit";

            // fileToolStripMenuItem
            var fileToolStripMenuItem = new ToolStripMenuItem();
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                openToolStripMenuItem,
                toolStripSeparator1,
                exitToolStripMenuItem});
            fileToolStripMenuItem.Text = "&File";

            // menuStrip1
            menuStrip1.Items.Add(fileToolStripMenuItem);

            // user menu items
            gCallBack.InitializeMenu(menuStrip1);

            var eventHandler = 
                new ToolStripItemClickedEventHandler(ToolStripMenuItem_DropDownItemClicked);
            foreach(ToolStripMenuItem item in menuStrip1.Items)
            {
                item.DropDownItemClicked += eventHandler;
            }
        }

        private void ToolStripMenuItem_DropDownItemClicked(
            object sender, ToolStripItemClickedEventArgs e)
        {
            gCallBack.Trace(e.ClickedItem.Name);

            bool handled = gCallBack.ToolStripMenuItem_DropDownItemClicked(sender, e);

            if (!handled)
            {
                switch (e.ClickedItem.Name)
                {
                    case OpenMenuItemName:
                        break;
                    case ExitMenuItemName:
                        Application.Exit();
                        break;
                }
            }
            UpdateLeftStatus(String.Format("{0} selected",e.ClickedItem.Text));
        }

    }
}