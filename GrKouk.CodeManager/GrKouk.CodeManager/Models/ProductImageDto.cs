using System;
using System.Collections.Generic;
using System.Text;
using Syncfusion.Data;

namespace GrKouk.CodeManager.Models
{
    public class ProductImageDto
    {
        public int PictureId { get; set; }
        public int ProductId { get; set; }
        public string MimeType { get; set; }
        public string SeoFilename { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }
        public int DisplayOrder { get; set; }
    }
    
}
