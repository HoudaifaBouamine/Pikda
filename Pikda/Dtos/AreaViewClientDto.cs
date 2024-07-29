using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisioForge.Libs.MediaFoundation;

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
                    return _value_without_placeholder
                        .Replace(": ", ":")
                        .Replace(" :", ":")
                        .Replace(". ", ".")
                        .Replace(" .", ".")
                        .Trim(new char[] { ' ', ':' });

                return _value_without_placeholder
                    .Replace(_placeHolder, string.Empty)
                    .Replace(": ", ":")
                    .Replace(" :", ":")
                    .Replace(". ", ".")
                    .Replace(" .", ".")
                    .Trim(new char[] { ' ', ':' });
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
