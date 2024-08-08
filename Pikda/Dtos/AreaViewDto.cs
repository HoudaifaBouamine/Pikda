using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pikda.Dtos
{
    internal class AreaViewDto
    {
        public string Prop { get; set; }

        private string _value_without_placeholder;
        public string Value 
        {
            get
            {
                var val = _value_without_placeholder
                    .Replace("\n", "")
                    .Replace(": ", ":")
                    .Replace(" :", ":")
                    .Replace("::", ":")
                    .Replace(". ", ".")
                    .Replace(" .", ".")
                    .Trim(new char[] { ' ', ':' });

                var parts = val.Split(':');
                if (parts.Length == 2)
                    return parts[1];

                return val;
            }
            set
            {
                _value_without_placeholder = value.Trim();
            }
        }
        public string Language { get; set; }
    }
}
