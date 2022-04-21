using Assigment_Repo.Abstract;
using Assigment_Repo.Data;
using Assigment_Repo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment_Repo.Concrete
{
    public class EFcoreFileRepository : IFileRepository
    {
        private readonly DataContext _context;

        public EFcoreFileRepository(DataContext ctx)
        {
            _context = ctx;
        }

        public async Task DeleteFile(int id)
        {
            var fileUpload = await _context.FileUploaded.FindAsync(id);
            _context.FileUploaded.Remove(fileUpload!);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FileUploaded>> GetAllFiles()
        {
            return await _context.FileUploaded.ToListAsync();
        }

        public async Task<FileUploaded?> GetFileById(int id)
        {
            return await _context.FileUploaded.FindAsync(id);
        }

        public async Task SaveFiles(IEnumerable<FileUploaded> files)
        {
            _context.FileUploaded.AddRange(files);
            await _context.SaveChangesAsync();
        }

        public async Task<FileUploaded> UpdateFile(int id, string fileName, string createdBy)
        {
            DateTime today = DateTime.Now;
            if (!FileUploadedExists(id))
            {
                return null!;
            }
            var fileUpload = await _context.FileUploaded.SingleOrDefaultAsync(f => f.FileId == id);
            fileUpload!.FileName = fileName;
            fileUpload.CreatedAt = today.Day.ToString() + '/' + today.Month.ToString();
            fileUpload.CreatedBy = createdBy;

            await _context.SaveChangesAsync();

            return fileUpload;
            
        }

        private bool FileUploadedExists(int id)
        {
            return _context.FileUploaded.Any(e => e.FileId == id);
        }
    }
}
