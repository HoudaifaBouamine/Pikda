using Microsoft.EntityFrameworkCore;
using Pikda.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pikda
{
    public class OcrRepository
    {
        readonly AppDbContext db;
        readonly static string ImagePath = "..\\..\\Images";
        public OcrRepository(AppDbContext db) 
        {
            this.db = db;
        }

        //public List<OcrModel> GetOcrModels()
        //{
        //    var list = db.OcrModels.Include(o => o.Areas).ToList();

        //    list.ForEach(o =>
        //    {
        //        using (var reader = new FileStream(o.ImageUrl,FileMode.Open)) 
        //        {
        //            o.InitImage(Image.FromStream(reader));
        //        }
        //    });

        //    return list;
        //}

        public List<OcrModel> GetOcrModels()
        {
            var list = db.OcrModels.Include(o => o.Areas).ToList();

            list.ForEach(o =>
            {
                var reader = new FileStream(o.ImageUrl, FileMode.Open);
                o.InitImage(Image.FromStream(reader));
                
            });

            return list;
        }


        public async Task CreateOcrModelAsync(OcrModel model)
        {
            if(model.Image != null)
            {
                var fullPath = Path.Combine(ImagePath, model.Name + ".jpg");
                model.Image.Save(fullPath);
                model.ImageUrl = fullPath;
            }

            db.OcrModels.Add(model);

            await db.SaveChangesAsync();
        }

        public void UpdateOcrModel(OcrModel model)
        {
            if (model.Image != null)
            {
                if(model.IsImageChanged && string.IsNullOrEmpty(model.ImageUrl))
                {
                    var fullPath = Path.Combine(ImagePath, model.Name + ".jpeg");
                    Console.WriteLine($" --> Full Path => {fullPath}, Image => {model.Image}, w:{model.Image?.Width}, h:{model.Image?.Height}");

                    model.Image.Save(fullPath);
                    model.ImageUrl = fullPath;
                }
            }

            db.OcrModels.Update(model);
            db.SaveChanges();
        }
    }
}
