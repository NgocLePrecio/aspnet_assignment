using Assigment_Repo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment_Repo.Abstract
{
    public interface IFileRepository
    {
        Task<IEnumerable<FileUploaded>> GetAllFiles();

        Task<FileUploaded?> GetFileById(int id);

        Task SaveFiles(IEnumerable<FileUploaded> files);

        Task<FileUploaded> UpdateFile(int id, string fileName, string createdBy);

        Task DeleteFile(int id);

    }
}
