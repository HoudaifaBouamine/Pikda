using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pikda.Models
{
    public class OcrModel
    {
        [Key]
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ImageUrl { get; set; }

        [NotMapped]
        public Image Image { get; private set; }

        public bool IsImageChanged { get; private set; }  = false;

        public void SetImage(Image image)
        {
            if(image == Image) return;
            IsImageChanged = true;
            Image = image;
        }

        public void InitImage(Image image)
        {
            if (Image != null) throw new System.Exception("must be null before init");
            Image = image;
        }

        // call this after you save the model
        public void ReportUpdating()
        {
            IsImageChanged = false;
        }
        public List<Area> Areas { get; private set; } = new List<Area>();

        public OcrModel(string name)
        {
            Name = name;
            Image = null;
            Id = default;
        }
        protected OcrModel() { }

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
