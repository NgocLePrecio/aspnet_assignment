#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNet_Assignment;
using Assigment_Repo.Models;
using Microsoft.AspNetCore.Authorization;
using Assigment_Repo.Abstract;
using Assignment_Service.FileUploadService;

namespace AspNet_Assignment.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class FileUploadedController : ControllerBase
    {
        private readonly FileUploadService fileService;

        public FileUploadedController(FileUploadService repo)
        {
            fileService = repo;
        }

        // GET: FileUploaded
        [HttpGet]
        public async Task<IEnumerable<FileUploaded>> GetFileUploaded()
        {
            //return await _context.FileUploaded.ToListAsync();
            return await fileService.GetAllFiles();
        }

        //GET: FileUploaded/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FileUploaded>> GetFileUploaded(int id)
        {
            var fileUploaded = await fileService.GetFileById(id);

            if (fileUploaded == null)
            {
                return NotFound();
            }

            byte[] byteArray = fileUploaded.FileData;

            return File(byteArray, fileUploaded.FileType, fileUploaded.FileName);
        }

        // POST: FileUploaded
        [HttpPost]
        public async Task<ActionResult<FileUploaded>> PostFileUploaded([FromForm(Name = "username")] string username)
        {
            DateTime today = DateTime.Now;
            
            var request = HttpContext.Request;
            List<FileUploaded> lst = new List<FileUploaded>();
            for (int i = 0; i < request.Form.Files.Count; i++)
            {
                var fileUploaded = new FileUploaded();
                var file = request.Form.Files[i];
                fileUploaded.FileName = file.FileName;
                fileUploaded.FileData = new BinaryReader(file.OpenReadStream()).ReadBytes((int)file.Length);
                fileUploaded.CreatedAt = today.Day.ToString() + '/' + today.Month.ToString();
                fileUploaded.CreatedBy = username;
                fileUploaded.FileType = file.ContentType;
                lst.Add(fileUploaded);
            }

            await fileService.SaveFiles(lst);
            return CreatedAtAction("GetFileUploaded", lst);

        }

        // PUT: FileUploaded/5
        [HttpPut("{id}")]
        public async Task<ActionResult<FileUploaded>> PutFileUploaded(int id, [FromForm(Name = "fileName")] string fileName, [FromForm(Name = "createdBy")] string createdBy)
        {             
            if (fileName == null || fileName == "")
            {
                return BadRequest();
            }
            DateTime today = DateTime.Now;
            FileUploaded fileUpload = await fileService.UpdateFile(id,fileName,createdBy);
            if (fileUpload == null)
            {
                return NotFound();
            }

            return fileUpload;
        }

        // DELETE:
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFileUploaded(int id)
        {
            await fileService.DeleteFile(id);

            return NoContent();
        }

        

    }
}
