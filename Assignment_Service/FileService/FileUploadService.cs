using Assigment_Repo.Abstract;
using Assigment_Repo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Service.FileUploadService
{
    public class FileUploadService
    {
        private readonly IFileRepository repository;

        public FileUploadService(IFileRepository repo)
        {
            repository = repo;
        }

        public async Task DeleteFile(int id)
        {
            await repository.DeleteFile(id);
        }

        public async Task<IEnumerable<FileUploaded>> GetAllFiles()
        {
            return await repository.GetAllFiles();
        }

        public async Task<FileUploaded?> GetFileById(int id)
        {
            return await repository.GetFileById(id);
        }

        public async Task SaveFiles(IEnumerable<FileUploaded> files)
        {
            await repository.SaveFiles(files);
        }

        public async Task<FileUploaded> UpdateFile(int id, string fileName, string createdBy)
        {
            return await repository.UpdateFile(id,fileName,createdBy);

        }

    }
}
