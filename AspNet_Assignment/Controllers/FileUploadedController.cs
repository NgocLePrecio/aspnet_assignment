#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNet_Assignment;
using AspNet_Assignment.Models;
using Microsoft.AspNetCore.Authorization;

namespace AspNet_Assignment.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class FileUploadedController : ControllerBase
    {
        private readonly DataContext _context;

        public FileUploadedController(DataContext context)
        {
            _context = context;
        }

        // GET: FileUploaded
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileUploaded>>> GetFileUploaded()
        {
            return await _context.FileUploaded.ToListAsync();
        }

        //GET: FileUploaded/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FileUploaded>> GetFileUploaded(int id)
        {
            var fileUploaded = await _context.FileUploaded.FindAsync(id);

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

            _context.FileUploaded.AddRange(lst);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFileUploaded", lst);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FileUploaded>> PutFileUploaded(int id, [FromForm(Name = "fileName")] string fileName, [FromForm(Name = "createdBy")] string createdBy)
        { 
            var request = HttpContext.Request;
            
            if (fileName == null || fileName == "")
            {
                return BadRequest();
            }
            DateTime today = DateTime.Now;
            var fileUpload = await _context.FileUploaded.SingleOrDefaultAsync(f => f.FileId == id);

            fileUpload.FileName = fileName;
            fileUpload.CreatedAt = today.Day.ToString() + '/' + today.Month.ToString();
            fileUpload.CreatedBy = createdBy;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileUploadedExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return fileUpload;
        }

        // DELETE:
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFileUploaded(int id)
        {
            var fileUpload = await _context.FileUploaded.FindAsync(id);
            if (fileUpload == null)
            {
                return NotFound();
            }

            _context.FileUploaded.Remove(fileUpload);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FileUploadedExists(int id)
        {
            return _context.FileUploaded.Any(e => e.FileId == id);
        }

    }
}
