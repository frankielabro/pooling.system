using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class StatisticsViewModel
    {


        public int NumberOfApplicants { get; set; }
        public int HomeBased { get; set; }
        public int OfficeBased { get; set; }

       
        public int FacebookAds { get; set; }
        public int FacebookGroup { get; set; }
        public int LinkedIn { get; set; }
        public int Referral { get; set; }
        public int Others { get; set; }

        public int FrontendDeveloper{ get; set; }        
        public int BackendDeveloper { get; set; }
        public int DatabaseDeveloper { get; set; }
        public int MobileDeveloper { get; set; }
        public int FullStackWebDeveloper { get; set; }
        public int UXUIDesigner { get; set; }
        public int VirtualAssistant { get; set; }

        public int SeniorFrontendDeveloper { get; set; }
        public int SeniorBackendDeveloper { get; set; }
        public int SeniorDatabaseDeveloper { get; set; }
        public int SeniorFullStackDeveloper { get; set; }
        public int SeniorMobileDeveloper { get; set; }

        public int LeadFrontendDeveloper { get; set; }
        public int LeadBackendDeveloper { get; set; }
        public int LeadDatabaseDeveloper { get; set; }
        public int LeadFullStackDeveloper { get; set; }
        public int LeadMobileDeveloper { get; set; }
        //Test Statistics
        public int FrontEndTest { get; set; }
        public double FrontEndTestPassed { get; set; }
        public double FrontEndTestFailed { get; set; }
        public double FrontEndTestPassingRate { get; set; }

        public int BackendTest { get; set; }
        public double BackendTestPassed { get; set; }
        public double BackendTestFailed { get; set; }
        public double BackendTestPassingRate { get; set; }

        public int DatabaseTest { get; set; }
        public double DatabaseTestPassed { get; set; }
        public double DatabaseTestFailed { get; set; }
        public double DatabaseTestPassingRate { get; set; }

        public int CoreValuesTest { get; set; }
        public double CoreValuesTestPassed { get; set; }
        public double CoreValuesTestFailed { get; set; }
        public double CoreValuesTestPassingRate { get; set; }

        public int EnglishTest { get; set; }
        public double EnglishTestPassed { get; set; }
        public double EnglishTestFailed { get; set; }
        public double EnglishTestPassingRate { get; set; }
    }
}
