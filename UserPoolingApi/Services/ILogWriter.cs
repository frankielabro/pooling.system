using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.Services
{
    public interface ILogWriter
    {
        void LogWrite(string logMessage);
        void Log(string logMessage, TextWriter txtWriter);
    }
}
