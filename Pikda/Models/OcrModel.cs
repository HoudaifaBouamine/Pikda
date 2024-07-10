using System.Drawing;
using System.Collections.Generic;

namespace Pikda.Models
{
    public class OcrModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Image Image { get;  set; }
        public List<Area> Areas { get; private set; }

        public OcrModel(string name)
        {
            Name = name;
            Areas = new List<Area>();
            Image = null;
            Id = default;
        }
        protected OcrModel() { }

        public void SetImage(Image image)
        {
            Image = image; 
        }

        public void AddArea(Area area)
        {
            Areas.Add(area);
        }
        public override string ToString()
        {
            return $"Id = {Id}, Name = {Name}, Image = {Image}, Areas Count = {Areas.Count}, Areas = {Areas}";
        }
    }
}
