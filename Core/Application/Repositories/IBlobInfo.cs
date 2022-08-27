using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IBlobInfo
    {
        public Stream Content { get; init; }
        public string ContentType { get; init; }
    }
}
