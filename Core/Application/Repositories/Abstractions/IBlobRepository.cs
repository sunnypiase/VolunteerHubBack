using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Abstractions
{
    public interface IBlobRepository    
    {
        Task<IBlobInfo> GetImageByName(string name);
        Task<IBlobInfo> UploadImage(string path,string name);
        Task<bool> DeleteImage(string name);

    }
}
