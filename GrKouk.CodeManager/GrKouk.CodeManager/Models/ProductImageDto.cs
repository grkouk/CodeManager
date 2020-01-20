using System;
using System.Collections.Generic;
using System.Text;
using Syncfusion.Data;

namespace GrKouk.CodeManager.Models
{
    public class ProductImageDto
    {
        public string MimeType { get; set; }
        public string SeoFilename { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }
        public int DisplayOrder { get; set; }
    }
}
