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
                if (string.IsNullOrEmpty(PlaceHolder)) 
                    return _value_without_placeholder.Trim();

                return _value_without_placeholder.Replace(PlaceHolder, string.Empty).Trim();
            }
            set
            {
                _value_without_placeholder = value.Trim();
            }
        }
        public string PlaceHolder { get; set; } = string.Empty;
    }
}
