using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace UrielGuy.SyntaxHighlightingTextBox
{
    /// <summary>
    /// 
    /// </summary>
    public class FontNameIndex : Dictionary<string, int>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class RTFFontStyles
    {
        Dictionary<Font, string> m_Styles = new Dictionary<Font, string>();
        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            m_Styles.Clear();
        }

        /// <summary>
        /// Caches the font style.
        /// </summary>
        /// <param name="pFont">The p font.</param>
        /// <returns></returns>
        private string CacheFontStyle(Font pFont)
        {
            StringBuilder sFontStlye = new StringBuilder();

            if (pFont.Bold)
                sFontStlye.Append(@"\b");

            if (pFont.Italic)
                sFontStlye.Append(@"\i");

            if (pFont.Underline)
                sFontStlye.Append(@"\ul");

            if (pFont.Strikeout)
                sFontStlye.Append(@"\strike");

            string result = sFontStlye.ToString();
            if (!m_Styles.ContainsKey(pFont))
            {
                m_Styles.Add(pFont, result);
            }
            else
            {
                m_Styles[pFont] = result;
            }
            return result;
        }

        /// <summary>
        /// Gets the font style.
        /// </summary>
        /// <param name="pFont">The p font.</param>
        /// <returns></returns>
        public string GetFontStyle(Font pFont)
        {
            string result = null;
            if (!m_Styles.TryGetValue(pFont, out result))
                result = CacheFontStyle(pFont);
            return result;
        }
    }
}
