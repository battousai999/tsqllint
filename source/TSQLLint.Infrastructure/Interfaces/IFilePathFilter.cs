using System;
using System.Collections.Generic;
using System.Text;

namespace TSQLLint.Infrastructure.Interfaces
{
    public interface IFilePathFilter
    {
        bool IsFilePathAllowed(string filePath);
    }
}
