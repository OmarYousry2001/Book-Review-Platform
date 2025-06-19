using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts.GeneralService.CMS
{
    public interface IFileUploadService
    {
        Task<byte[]> GetFileBytesAsync(IFormFile file);
        Task<byte[]> GetFileBytesAsync(string base64String);
        Task<string> UploadFileAsync(IFormFile file);
        Task<string> UploadFileAsync(byte[] fileBytes, string folderName, string? oldFileName = null);
        bool IsValidFile(IFormFile file);
        bool IsValidFile(string base64File, string fileName);
        (bool isValid, string errorMessage) ValidateFile(IFormFile file);
        (bool isValid, string errorMessage) ValidateFile(string base64String);
    }
}
