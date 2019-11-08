using System;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.SongManager
{
    public class ReadOnlySearchableRichTextBox : RichTextBox
    {
        private string search;
        private bool bypassSelectionChangedHandler;

        public const string HELP =
            "[To search this text box, click anywhere in it and start typing! Use the Enter key to find the next match, and clear the selection to reset your search query.]";

        public ReadOnlySearchableRichTextBox()
        {
            search = "";
            ReadOnly = true;

            KeyPress += ReadOnlySearchableRichTextBox_KeyPress;
            KeyDown += (o, e) =>
            {
                if (e.Modifiers == Keys.None)
                {
                    e.Handled = true;
                }
            };
            SelectionChanged += (o, e) =>
            {
                if (!bypassSelectionChangedHandler)
                {
                    search = "";
                }
            };
        }

        private void SelectBypass(int index, int length)
        {
            bypassSelectionChangedHandler = true;
            Select(index, length);
            bypassSelectionChangedHandler = false;
        }

        private void SearchLook(int startIndex)
        {
            int index = Text.IndexOf(search, startIndex, StringComparison.InvariantCultureIgnoreCase);
            if (index >= 0)
            {
                SelectBypass(index, search.Length);
            }
            else if (startIndex > 0)
            {
                SearchLook(0);
            }
        }

        public void SearchClear()
        {
            search = "";
            SelectBypass(SelectionStart, 0);
        }

        public void SearchAdd(char c)
        {
            search += c;
            SearchLook(SelectionStart);
        }

        public void SearchBackspace()
        {
            if (search.Length == 0)
            {
                return;
            }

            search = search.Substring(0, search.Length - 1);
            SearchLook(SelectionStart);
        }

        public void SearchNext()
        {
            if (SelectionLength > 0)
            {
                SearchLook(SelectionStart + 1);
            }
        }

        private void ReadOnlySearchableRichTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (!char.IsControl(e.KeyChar))
            {
                SearchAdd(e.KeyChar);
            }
            else if (e.KeyChar == 8)
            {
                SearchBackspace();
            }
            else if (e.KeyChar == 27)
            {
                SearchClear();
            }
            else if (e.KeyChar == 13)
            {
                SearchNext();
            }
            else
            {
                Console.WriteLine((int) e.KeyChar);
                e.Handled = false;
            }
        }
    }
}