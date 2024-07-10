﻿
using System.Drawing;
using System;

namespace Pikda.Models
{

    public class Area
    { 
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Value { get; set; }
        public float XFactor { get; private set; }
        public float YFactor { get; private set; }
        public float WidthFactor { get; private set; }
        public float HeightFactor { get; private set; }
        public OcrModel OcrModel { get; set; }

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
