using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClearMetaData.Models
{
    public class CleanFile
    {
        public void ExifTool(string fileName)
        {
            string Todaysdate = DateTime.Now.ToString("dd-MM-yyyy");
            string dir = (@"C:\\Users\\Emin\\source\\repos\\exifdeneme\\exifdeneme\\UploadedFiles\" + Todaysdate);
            string arg = dir + "\\" + fileName;

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            process.StartInfo = startInfo;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/K exiftool -all= " + arg;
            startInfo.Verb = "runas";
            process.Start();




            //string _FileName = Path.GetFileName(fileName);
            //string _Path = Path.Combine(Server.MapPath("~/UploadedFiles/" + Todaysdate), _FileName);
            //Response.Buffer = true;
            //Response.AppendHeader("Content-Disposition", "attachment; filename= " + _FileName);
            //Response.TransmitFile(_Path);
            //Response.Flush();
            //Response.Close();
            //Response.End();




            // eğer ki başarılıysa işlem dosya orjinal ve clearlanmış dosya download edilebilecek 

        }
    }
}