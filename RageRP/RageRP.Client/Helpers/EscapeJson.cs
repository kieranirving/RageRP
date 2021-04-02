using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Client.Helpers
{
    public class Json
    {
        public static string Escape(string text)
        {
            return text.Replace("'", "\\'");
        }
    }
}
