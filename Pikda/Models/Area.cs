using System.Drawing;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pikda.Models
{
    public class Area
    {
        [Key]
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Value { get; set; }
        public string Placeholder { get; set; } // string to be removed from the value
        public string Language { get; set; } = "ara";
        public float XFactor { get; private set; }
        public float YFactor { get; private set; }
        public float WidthFactor { get; private set; }
        public float HeightFactor { get; private set; }

        [ForeignKey(nameof(OcrModel))]
        public int OcrModelId { get; set; }

        public Area(string name, Rectangle imageRect, Rectangle newRect)
        {
            Id = default;
            Name = name;
            XFactor = ((float)Math.Min(newRect.X, newRect.X + newRect.Width)) / (imageRect.Width);
            YFactor = ((float)Math.Min(newRect.Y, newRect.Y + newRect.Height)) / (imageRect.Height);
            WidthFactor = (float)Math.Abs(newRect.Width) / (imageRect.Width);
            HeightFactor = (float)Math.Abs(newRect.Height) / (imageRect.Height);
        }
        protected Area() { }

        public bool SetValue(string value)
        {
            if(string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value)) return false;

            this.Value = value;
            return true;
        }

        public Rectangle ToRectangle(Rectangle currentImageRect)
        {
            return new Rectangle
                (
                    x: (int)(XFactor * currentImageRect.Width),
                    y: (int)(YFactor * currentImageRect.Height),
                    width: (int)(WidthFactor * currentImageRect.Width),
                    height: (int)(HeightFactor * currentImageRect.Height)
                );
        }
    }
}
