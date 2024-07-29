using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pikda.Dtos
{
    internal class AreaViewClientDto
    {
        public string Prop { get; set; }

        private string _value_without_placeholder;
        public string Value 
        {
            get
            {
                if (string.IsNullOrEmpty(_placeHolder)) 
                    return _value_without_placeholder.Trim();

                return _value_without_placeholder.Replace(_placeHolder, string.Empty).Trim();
            }
            set
            {
                _value_without_placeholder = value.Trim();
            }
        }
        private string _placeHolder { get; set; } = string.Empty;

        public void SetPlaceholder(string placehoder)
        {
            _placeHolder = placehoder;
        }
    }
}
