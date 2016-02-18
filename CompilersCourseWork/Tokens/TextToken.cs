using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.Tokens
{
    public class TextToken : Token
    {
        private string text;

        public TextToken(string text)
        {
            this.text = text;
        }

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        protected override Tuple<string, string> GetStringRepresentation()
        {
            return new Tuple<String, String>("text", text);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var asText = obj as TextToken;
            if (asText == null || asText.Text != this.Text)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }
    }
}
