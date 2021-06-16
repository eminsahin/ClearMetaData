
using ClearMetaData.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ClearMetaData.Models
{
    public class File
    {
        public HttpPostedFileBase FileName { get; set; }
        
        public byte[] Content { get; set; }
        public FileType FileType { get; set; }
    }
}