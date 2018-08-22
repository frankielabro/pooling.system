using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UserPoolingApi.ViewModels;

namespace UserPoolingApi.Services
{
    public interface IGenerateCV
    {
        //The values here must be related to the functions created for Databale
        void CreatePDF(UserViewModel userVM, int UserId);
        void CreateCV(string strPdfPath, UserViewModel userVM);

    }
}


