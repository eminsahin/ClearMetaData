using ClearMetaData.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClearMetaData.Controllers
{
    public class HomeController : Controller
    { 

        public ActionResult Index(HttpPostedFileBase file)
        {
            // upload metod clear metadata (exif)
            string Todaysdate = DateTime.Now.ToString("dd-MM-yyyy");
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + Todaysdate;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            try
            {
                if (file.ContentLength > 0)
                {

                    string _FileName = Path.GetFileName(file.FileName);
                    string _Path = Path.Combine(dir, _FileName);
                    file.SaveAs(_Path);

                    CleanMetadata(file);
                    DownloadFile(file);
                    


                    
                    //return File(Url.Content(_Path), "application/pdf", _FileName);

                    

                    ExifTool(_FileName);
                    
                    //return ExifTool(File);



                }

                Response.Write("<script lang='JavaScript'>alert('File Uploaded Succesfully!!');</script>"); // generic response model en son
                return View();
            }
            catch
            {
                Response.Write("<script lang='JavaScript'>alert('File Upload Failed!!');</script>");
                return View();
            }

            

        }
        
        public string OpenModelPopup()
        {
            //can send some data also.  
            return "<h1>This is Modal Popup Window</h1>";
        }

        [HttpPost]
        public ActionResult Download(string fileName)
        {
            string Todaysdate = DateTime.Now.ToString("dd-MM-yyyy");
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + Todaysdate;
            string arg = dir + "\\" + fileName;
            string _FileName = Path.GetFileName(fileName);
            string _Path = Path.Combine(dir, _FileName);
            //PdfFiles is the name of the folder where these pdf files are located
            
            var memory = new MemoryStream();
            using (var stream = new FileStream(_Path, FileMode.Open))
            {
                stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/pdf", Path.GetFileName(_Path));
        }
    
        public void ExifTool(string fileName)
        {
            string Todaysdate = DateTime.Now.ToString("dd-MM-yyyy");
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + Todaysdate;
            string arg = dir + "\\" + fileName;

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            process.StartInfo = startInfo;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/K exiftool -all= " + arg;
            startInfo.Verb = "runas";
            process.Start();

            string _FileName = Path.GetFileName(fileName);
            string _Path = Path.Combine(dir, _FileName);


            byte[] fileBytes = System.IO.File.ReadAllBytes(_Path);
            File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _FileName);
            //Response.Buffer = true;
            //Response.AppendHeader("Content-Disposition", "attachment; filename= " + _FileName);
            //Response.TransmitFile(_Path);
            //Response.Flush();
            //Response.SuppressContent = true;
            
            
        }

        public ActionResult DownloadFile(HttpPostedFileBase file)
        {


            string Todaysdate = DateTime.Now.ToString("dd-MM-yyyy");
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + Todaysdate;
            string _FileName = Path.GetFileName(file.FileName);
            string _Path = Path.Combine(dir, _FileName);

            return File(_Path, "application/pdf", _FileName);


            //byte[] filedata = System.IO.File.ReadAllBytes(_Path);
            //string contentType = MimeMapping.GetMimeMapping(_Path);

            //var cd = new System.Net.Mime.ContentDisposition
            //{
            //    FileName = _FileName,
            //    Inline = true,
            //};

            //Response.AppendHeader("Content-Disposition", cd.ToString());

            //return File(filedata, contentType);
        }
        public ActionResult CleanMetadata(HttpPostedFileBase file)
        {
            string Todaysdate = DateTime.Now.ToString("dd-MM-yyyy");
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + Todaysdate;
            string _FileName = Path.GetFileName(file.FileName);
            string _Path = Path.Combine(dir, _FileName);
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "cmd /c exiftool" + " " + _Path;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            //* Read the output (or the error)
            string Output = process.StandardOutput.ReadToEnd();
            //Output.Split(',').LastOrDefault();
            Response.Write(Output);

            string Err = process.StandardError.ReadToEnd();
            Response.Write(Err);

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}