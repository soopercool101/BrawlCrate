using System;
using System.Drawing;

namespace UrielGuy.SyntaxHighlightingTextBox
{
    public class HighlightDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightDescriptor"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="color">The color.</param>
        /// <param name="font">The font.</param>
        /// <param name="descriptorType">Type of the descriptor.</param>
        /// <param name="dr">The dr.</param>
        /// <param name="useForAutoComplete">if set to <c>true</c> [use for auto complete].</param>
        public HighlightDescriptor(string token, Color color, Font font, DescriptorType descriptorType, DescriptorRecognition dr, bool useForAutoComplete)
        {
            if (descriptorType == DescriptorType.ToCloseToken)
            {
                throw new ArgumentException("You may not choose ToCloseToken DescriptorType without specifing an end token.");
            }
            Color = color;
            Font = font;
            Token = token;
            DescriptorType = descriptorType;
            DescriptorRecognition = dr;
            CloseToken = null;
            UseForAutoComplete = useForAutoComplete;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightDescriptor"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="closeToken">The close token.</param>
        /// <param name="descriptorType">Type of the descriptor.</param>
        /// <param name="dr">The dr.</param>
        /// <param name="color">The color.</param>
        /// <param name="font">The font.</param>
        /// <param name="useForAutoComplete">if set to <c>true</c> [use for auto complete].</param>
        public HighlightDescriptor(string token, string closeToken, DescriptorType descriptorType, DescriptorRecognition dr, Color color, Font font, bool useForAutoComplete)
        {
            if (dr == DescriptorRecognition.RegEx)
            {
                throw new ArgumentException("You may not choose RegEx DescriptorType with an end token.");
            }
            Color = color;
            Font = font;
            Token = token;
            DescriptorType = descriptorType;
            CloseToken = closeToken;
            DescriptorRecognition = dr;
            UseForAutoComplete = useForAutoComplete;
        }

        public readonly Color Color;
        public readonly Font Font;
        public readonly string Token;
        public readonly string CloseToken;
        public readonly DescriptorType DescriptorType;
        public readonly DescriptorRecognition DescriptorRecognition;
        public readonly bool UseForAutoComplete;
    }

    public enum DescriptorType
    {
        /// <summary>
        /// Causes the highlighting of a single word
        /// </summary>
        Word,
        /// <summary>
        /// Causes the entire line from this point on the be highlighted, regardless of other tokens
        /// </summary>
        ToEOL,
        /// <summary>
        /// Highlights all text until the end token;
        /// </summary>
        ToCloseToken,
        /// <summary>
        /// To end of word.
        /// </summary>
        ToEOW
    }

    public enum DescriptorRecognition
    {
        /// <summary>
        /// Only if the whole token is equal to the word
        /// </summary>
        WholeWord,
        /// <summary>
        /// If the word starts with the token
        /// </summary>
        StartsWith,
        /// <summary>
        /// If the word contains the Token
        /// </summary>
        Contains,
        /// <summary>
        /// the start token is a RegEx.
        /// Stoptoken is not used
        /// </summary>
        RegEx
    }
}
