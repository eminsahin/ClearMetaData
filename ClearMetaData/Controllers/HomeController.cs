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
                    //_Path = ClearMetaData.Models.File.FileName();
                    MetaDataView(_FileName);
                    ExifTool(_FileName);
                    CleanMetaDataView(_FileName);
                    DosyaIndir(_Path);
                    


                    
                    //return File(Url.Content(_Path), "application/pdf", _FileName);

                    

                    
                    
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

        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase files)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if (files != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(files.FileName));
                        files.SaveAs(path);

                    }
                    ViewBag.FileStatus = "File uploaded successfully.";
                }
                catch (Exception)
                {

                    ViewBag.FileStatus = "Error while file uploading.";
                }

            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult Download(string file)
        {
            string Todaysdate = DateTime.Now.ToString("dd-MM-yyyy");
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + Todaysdate;
            string arg = dir + "\\" + file;
            string _FileName = Path.GetFileName(file);
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
            process.WaitForExit();



        }

        public void DosyaIndir(string fileName)
        {
            string Todaysdate = DateTime.Now.ToString("dd-MM-yyyy");
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + Todaysdate;
            string arg = dir + "\\" + fileName;
            string _FileName = Path.GetFileName(fileName);
            string _Path = Path.Combine(dir, _FileName);

            Response.Clear();
            Response.ClearHeaders();
            //byte[] fileBytes = System.IO.File.ReadAllBytes(_Path);
            //File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _FileName);
            Response.Buffer = true;
            Response.AppendHeader("Content-Disposition", "attachment; filename= " + _FileName);
            Response.TransmitFile(_Path);
            Response.Flush();
            Response.SuppressContent = true;
            Response.End();
        }
        public void MetaDataView(string fileName)
        {
            string Todaysdate = DateTime.Now.ToString("dd-MM-yyyy");
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + Todaysdate;
            string arg = dir + "\\" + fileName;
            
            
            Process process = new Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            process.StartInfo = startInfo;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "cmd /c exiftool" + " " + arg + " " + "> C:/Users/Emin/source/repos/ClearMetaData/ClearMetaData/UploadedFiles/output.txt | type C:/Users/Emin/source/repos/ClearMetaData/ClearMetaData/UploadedFiles/output.txt";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            startInfo.Verb = "runas";
            process.Start();
            process.WaitForExit();

            //* Read the output (or the error)
            string Output = process.StandardOutput.ReadToEnd();
            //Output.Split(',').LastOrDefault();
            Response.Write(Output);

            string Err = process.StandardError.ReadToEnd();
            Response.Write(Err);
        }
        public void CleanMetaDataView(string fileName)
        {
            string Todaysdate = DateTime.Now.ToString("dd-MM-yyyy");
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + Todaysdate;
            string arg = dir + "\\" + fileName;


            Process process = new Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            process.StartInfo = startInfo;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "cmd /c exiftool" + " " + arg + " " + "> C:/Users/Emin/source/repos/ClearMetaData/ClearMetaData/UploadedFiles/cleanoutput.txt | type C:/Users/Emin/source/repos/ClearMetaData/ClearMetaData/UploadedFiles/cleanoutput.txt";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            startInfo.Verb = "runas";
            process.Start();
            process.WaitForExit();

            //* Read the output (or the error)
            string Output = process.StandardOutput.ReadToEnd();
            //Output.Split(',').LastOrDefault();
            Response.Write(Output);

            string Err = process.StandardError.ReadToEnd();
            Response.Write(Err);
        }
        public ActionResult DownloadFile(HttpPostedFileBase file)
        {
            return View();
            
            //string Todaysdate = DateTime.Now.ToString("dd-MM-yyyy");
            //string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + Todaysdate;
            //string _FileName = Path.GetFileName(file.FileName);
            //string _Path = Path.Combine(dir, _FileName);

            //var memory = new MemoryStream();
            //using (var stream = new FileStream(_Path, FileMode.Open))
            //{
            //    stream.CopyToAsync(memory);
            //}
            //memory.Position = 0;

           

            //return File(memory, "application/pdf", Path.GetFileName(_Path));
            
            
            //byte[] fileBytes = System.IO.File.ReadAllBytes(_Path);
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _FileName);

            //return File(_Path, "application/pdf", _FileName);


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
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            process.StartInfo = startInfo;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "cmd /c exiftool" + " " + _Path +" "+  "> C:/Users/Emin/source\repos/ClearMetaData/ClearMetaData/UploadedFiles/output.txt | type C:/Users/Emin/source/repos/ClearMetaData/ClearMetaData/UploadedFiles/output.txt";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            startInfo.Verb = "runas";
            process.Start();
            process.WaitForExit();

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