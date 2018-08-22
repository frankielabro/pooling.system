using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UserPoolingApi.Services;
using UserPoolingApi.ViewModels;
using System.Globalization;
using iTextSharp.text.pdf.draw;
using Microsoft.EntityFrameworkCore;
using UserPoolingApi.Enums;
using System.Text.RegularExpressions;
using UserPoolingApi.EntityFramework;

namespace UserPoolingApi.Services
{
    public class GenerateCV : IGenerateCV
    {
        public IConfiguration _configuration { get; }
        public DataContext _context;


        public GenerateCV(DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        //The title and the Id of the user will be pased here
        public void CreatePDF(UserViewModel userVM, int UserId)
        {
            try
            {
                //Getting the directory/path where the PDFs will be stored
                var dir = _configuration.GetSection("Directory:ForLocalGeneratedCV").Value;
                //Calling the function that will put the values of dtbl, directory path, and the headertitle
                CreateCV(@dir + "\\" + UserId + "\\" + userVM.FirstName + "_" + userVM.LastName.TrimStart().Substring(0, 1) + "_CV.pdf", userVM); //
                
            }
            catch (Exception ex)
            {
                
            }
        }

        //DataTable will be passed in here including the Directory path and the header title
        public void CreateCV(string strPdfPath, UserViewModel userVM)
        {
            //FONTS
            Font boldStyle = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font arial = FontFactory.GetFont("Arial", 21, Font.BOLD);
            Font boldItalicStyle = FontFactory.GetFont("Arial", 12, Font.BOLDITALIC);
            Font italicStyle = FontFactory.GetFont("Arial", 12, Font.ITALIC);
            Font timesBoldSmall = FontFactory.GetFont("Times New Roman", 9, Font.BOLD);
            Font timesItalicSmall = FontFactory.GetFont("Times New Roman", 8, Font.ITALIC);
            Font timesNormalSmall = FontFactory.GetFont("Times New Roman", 8, Font.NORMAL);
            Chunk glue = new Chunk(new VerticalPositionMark());
            //Get the values from the userVM
            var id = userVM.UserId;
            var name = userVM.FirstName + " " + userVM.LastName.TrimStart().Substring(0, 1);
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            name = name.ToLower();
            name = textInfo.ToTitleCase(name);
            System.IO.FileStream fs = new FileStream(strPdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document(PageSize.A4, 60, 60, 36, 36);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            Image logo = Image.GetInstance("C://UploadFiles"+ "/logo.jpg");
            logo.ScalePercent(20f);
            //logo.SetAbsolutePosition(document.PageSize.Width - 36f - 72f, document.PageSize.Height - 36f -72f);
            //Your Remote Resources Partner
            //Sourcing experts for on time, on spec and on budget solutions
            //www.devpartners.co
            logo.ScaleToFit(70f, 70f);
            logo.Alignment = Image.ALIGN_RIGHT;
            logo.IndentationLeft = 9f;
            logo.SpacingAfter = 9f;
            logo.BorderWidthTop = 36f;
            logo.BorderColorTop = BaseColor.White;
            document.Add(logo);
            //Header: Title of the Document
            //Created an instance of paragraph
            Paragraph prgHeading = new Paragraph();
            //Setting the prgHeading Alignment
            prgHeading.Alignment = Element.ALIGN_LEFT;
            //Added to the paragraph the strHeader with the fntHead style //NAME OF THE USER
            prgHeading.Add(new Chunk(name, arial));
            //added the prgHeading as the first element.
            document.Add(prgHeading);
            arial.Size = 16;
            arial.SetStyle(Font.ITALIC);
            arial.SetColor(70, 70, 70);
            Paragraph prgPosition = new Paragraph();
            prgPosition.Alignment = Element.ALIGN_LEFT;
            var positionDesired = _context.PositionDesired.FirstOrDefault(p => p.PositionDesiredId == userVM.PositionDesiredId);
            //var positionDesiredString = Regex.Replace(posit  ionDesired + "", "(\\B[A-Z])", " $1") + ""; 
            prgPosition.Add(new Chunk(positionDesired.PositionName, arial));//The "position" should be dynamic------------------------------
            document.Add(prgPosition);
            document.Add(new Chunk("\n", arial));
            Paragraph linebreak = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
            //SUMMARY SECTION
            if (userVM.Summaries != null)
            {
                arial.Size = 12;
                arial.SetStyle(Font.BOLD);
                arial.SetColor(51, 102, 153);
                Paragraph pgHeader1 = new Paragraph();
                pgHeader1.Alignment = Element.ALIGN_LEFT;
                pgHeader1.Add(new Chunk("Summary", arial));
                document.Add(pgHeader1);
                document.Add(linebreak);
                arial.Size = 12;
                arial.SetStyle(Font.NORMAL);
                arial.SetColor(0, 0, 0);
                //List of Summaries
                List list = new List(List.UNORDERED, 10f); //the second parameter is the symbol size.
                list.ListSymbol = new Chunk("\u2022"); //no squares available
                list.IndentationLeft = 15f; //indentation
                if (userVM.Summaries.Count > 0)
                {
                    for (int x = 0; x < userVM.Summaries.Count; x++)
                    {
                        list.Add(userVM.Summaries[x].Sentence);
                    }
                    document.Add(list);
                    document.Add(new Chunk("\n", arial));
                }
            }
            //SKILLS AND EXPERTIES
            if (userVM.UserSkills != null) //this must be changed into Skills
            {
                int[] skillId = new int[userVM.UserSkills.Count];
                for (int x = 0; x < skillId.Length; x++)
                {
                    skillId[x] = userVM.UserSkills[x].SkillId;
                }
                arial.Size = 12;
                arial.SetStyle(Font.BOLD);
                arial.SetColor(51, 102, 153);
                Paragraph pgHeader2 = new Paragraph();
                pgHeader2.Alignment = Element.ALIGN_LEFT;
                pgHeader2.Add(new Chunk("Skills & Expertise", arial));
                document.Add(pgHeader2);

                {
                    /*
                        document.Add(linebreak);
                        //List of Skills and Expertise 
                        List listSkills = new List(List.UNORDERED, 10f); //the second parameter is the symbol size.
                        listSkills.SetListSymbol("\u2022"); //circle
                        listSkills.IndentationLeft = 15f; //indentation
                        //Group the skills
                        int[] skillGroup = new int[skillId.Length];
                        int skillGroupCount = 0;
                        for (int x = 1; x <= 6; x++)
                        {
                            for (int y = 0; y < skillId.Length; y++)
                            {
                                var skill = _context.Skills.FirstOrDefault(u => u.SkillId == skillId[y]); //the [0] must be dynamic within for loop
                                 var skilltype = _context.SkillTypes.FirstOrDefault(u => u.SkillTypeId == skill.SkillTypeId);

                                if (skilltype.SkillTypeId == x)
                                {
                                    skillGroup[skillGroupCount++] = skillId[y];
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    //foreach (var x in skillGroup)
                    {
                        Console.WriteLine(x);
                    }
                    int currIndex = 0;
                    SkillType Loop
                    for (int x = 0; x <= 6; x++)
                    {
                        Phrase lineSkills = new Phrase();
                        string skillPhrase = "";
                        int phraseSkillCount = 0;
                        for (int y = currIndex; y < skillId.Length; y++)
                        {
                            if (y > skillId.Length) break;//y =4 this one should be compared with skillGroup not SkillIdLength
                            var skill = _context.Skills.FirstOrDefault(u => u.SkillId == skillGroup[y]); //the [0] must be dynamic within for loop
                            var skilltype = _context.SkillTypes.FirstOrDefault(u => u.SkillTypeId == skill.SkillTypeId);
                            if (skilltype.SkillTypeId == x + 1)
                            {
                                Chunk skillType = new Chunk(skilltype.SkillTypeName + ": ", boldItalicStyle);
                                lineSkills.Add(skillType);
                                for (int z = x; z <= skillId.Length; z++)
                                {
                                    if (skilltype.SkillTypeId == x + 1)
                                    {
                                        skillPhrase += skill.SkillName + ", ";
                                        y++;
                                        phraseSkillCount++;
                                        //so that the last index of SkillGroup will still be stored in the skills
                                        if (y >= skillId.Length)
                                        {
                                            if (phraseSkillCount > 0) skillPhrase = skillPhrase.Substring(0, skillPhrase.Length - 2);
                                            Chunk skills = new Chunk(skillPhrase, italicStyle);
                                            lineSkills.Add(skills);
                                            break;
                                        } //must be >=
                                        skill = _context.Skills.FirstOrDefault(u => u.SkillId == skillGroup[y]);
                                        skilltype = _context.SkillTypes.FirstOrDefault(u => u.SkillTypeId == skill.SkillTypeId);
                                    }
                                    else
                                    {
                                        //to remove the comma at the end of the sentence
                                        if (phraseSkillCount > 0) skillPhrase = skillPhrase.Substring(0, skillPhrase.Length - 2);
                                        Chunk skills = new Chunk(skillPhrase, italicStyle);
                                        lineSkills.Add(skills);
                                        y--; //for jumping index if found false
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                currIndex = y;
                                break;
                            }
                        }
                        if (lineSkills != null) listSkills.Add(new ListItem(lineSkills));
                    }
                    */
                }

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = 480f;
                //fix the absolute width of the table
                table.LockedWidth = true;
                //relative col widths in proportions - 1/3 and 2/3
                float[] widths = new float[] { 6f, 4f, 5f };
                table.SetWidths(widths);
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 20f;
                PdfPCell cell = new PdfPCell(new Phrase("Skill", boldStyle));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderColor = BaseColor.LightGray;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Level", boldStyle));
                cell.HorizontalAlignment = 1;
                cell.BorderColor = BaseColor.LightGray;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Year/s of Experience", boldStyle));
                cell.HorizontalAlignment = 1;
                cell.BorderColor = BaseColor.LightGray;
                table.AddCell(cell);

                skillId = new int[userVM.UserSkills.Count];
                
                for (int x = 0; x < skillId.Length; x++) {

                    var skill = _context.Skills.FirstOrDefault(u => u.SkillId == userVM.UserSkills[x].SkillId);

                    cell = new PdfPCell(new Phrase("  "+skill.SkillName));
                    cell.BorderColor = BaseColor.LightGray;
                    table.AddCell(cell);
                    userVM.UserSkills[x].SkillLevel = userVM.UserSkills[x].SkillLevel.ToLower();
                    userVM.UserSkills[x].SkillLevel = textInfo.ToTitleCase(userVM.UserSkills[x].SkillLevel);
                    cell = new PdfPCell(new Phrase(userVM.UserSkills[x].SkillLevel));
                    cell.BorderColor = BaseColor.LightGray;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("" + userVM.UserSkills[x].YearsOfExperience));
                    cell.BorderColor = BaseColor.LightGray;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                if (userVM.CustomSkills != null)
                {
                    var customSkill = new int[userVM.CustomSkills.Count];
                    for (int x = 0; x < customSkill.Length; x++)
                    {

                        cell = new PdfPCell(new Phrase("  " + userVM.CustomSkills[x].SkillName));
                        cell.BorderColor = BaseColor.LightGray;
                        table.AddCell(cell);
                        userVM.CustomSkills[x].SkillLevel = userVM.CustomSkills[x].SkillLevel.ToLower();
                        userVM.CustomSkills[x].SkillLevel = textInfo.ToTitleCase(userVM.CustomSkills[x].SkillLevel);
                        cell = new PdfPCell(new Phrase(userVM.CustomSkills[x].SkillLevel));
                        cell.BorderColor = BaseColor.LightGray;
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);
                        cell = new PdfPCell(new Phrase("" + userVM.CustomSkills[x].YearsOfExperience));
                        cell.BorderColor = BaseColor.LightGray;
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);
                    }
                }


                document.Add(table);
                //document.Add(listSkills);
            }
            
            //Header: Professional Background
            if (userVM.WorkExperiences != null)
            {
                document.Add(new Chunk("\n", arial));
                arial.Size = 12;
                arial.SetStyle(Font.BOLD);
                arial.SetColor(51, 102, 153);
                Paragraph pgHeader3 = new Paragraph();
                pgHeader3.Alignment = Element.ALIGN_LEFT;
                pgHeader3.Add(new Chunk("Professional Experience", arial));
                document.Add(pgHeader3);
                document.Add(linebreak);
                arial.Size = 12;
                arial.SetStyle(Font.BOLD);
                arial.SetColor(0, 0, 0);
                
                //List of Professional Experiences
                foreach (var workExperience in userVM.WorkExperiences)
                {
                    List workDescription = new List(List.UNORDERED, 10f); //the second parameter is the symbol size.
                    Paragraph prgCompanyName = new Paragraph();
                    prgCompanyName.Alignment = Element.ALIGN_LEFT;
                    glue = new Chunk(new VerticalPositionMark());
                    Phrase WorkExperience = new Phrase();

                    Chunk companyName = new Chunk(workExperience.CompanyName, boldStyle);
                    Chunk WorkDateInclusive = new Chunk(
                        String.Format("{0: yyyy MMM}", workExperience.FromDate)
                        + " - " +
                        String.Format("{0: yyyy MMM}", workExperience.ToDate),
                        boldStyle);
                    Chunk position = new Chunk(workExperience.Position, italicStyle);
                    WorkExperience.Add(companyName);
                    WorkExperience.Add(glue);
                    WorkExperience.Add(WorkDateInclusive);
                    WorkExperience.Add(new Chunk("\n", arial));
                    WorkExperience.Add(position);
                    document.Add(WorkExperience);
                    workDescription.ListSymbol = new Chunk("\u2022");
                    workDescription.IndentationLeft = 15f; //indentation
                    workDescription.Add(new ListItem(workExperience.WorkDescription));
                    document.Add(workDescription);
                    document.Add(new Chunk("\n", arial));
                }
            }
            //List of Schools/Universities Attended
            if (userVM.Educations != null)
            {
                //Header: Educational Background
                arial.Size = 12;
                arial.SetStyle(Font.BOLD);
                arial.SetColor(51, 102, 153);
                Paragraph pgHeader4 = new Paragraph();
                pgHeader4.Alignment = Element.ALIGN_LEFT;
                pgHeader4.Add(new Chunk("Education", arial));
                document.Add(pgHeader4);
                document.Add(linebreak);
                arial.Size = 12;
                arial.SetStyle(Font.BOLD);
                arial.SetColor(0, 0, 0);
                //List of Educational Background
                foreach (var university in userVM.Educations)
                {
                    Paragraph prgEducation = new Paragraph();
                    prgEducation.Alignment = Element.ALIGN_LEFT;
                    Phrase School = new Phrase();
                    Chunk schoolName = new Chunk(university.SchoolName, boldStyle);

                    Chunk schoolDateInclusive = new Chunk(
                            String.Format("{0: yyyy}", university.FromDate)
                            + " - " +
                            String.Format("{0: yyyy}", university.ToDate),
                            boldStyle);
                    Chunk course = new Chunk(university.Course, italicStyle);
                    School.Add(schoolName);
                    School.Add(glue);
                    School.Add(schoolDateInclusive);
                    School.Add(new Chunk("\n", arial));
                    School.Add(course);
                    document.Add(School);
                    document.Add(new Chunk("\n", arial));
                    document.Add(new Chunk("\n", arial));
                }
                //List of Certificates
                if (userVM.Certificates != null)
                {
                    try
                    {
                        if(userVM.Certificates[0].CompanyName != null && userVM.Certificates[0].CompanyName != "")
                        {
                            //Header: Certificates
                            arial.Size = 12;
                            arial.SetStyle(Font.BOLD);
                            arial.SetColor(51, 102, 153);
                            Paragraph pgHeader5 = new Paragraph();
                            pgHeader5.Alignment = Element.ALIGN_LEFT;
                            pgHeader5.Add(new Chunk("Certificates", arial));
                            document.Add(pgHeader5);
                            document.Add(linebreak);
                            arial.Size = 12;
                            arial.SetStyle(Font.BOLD);
                            arial.SetColor(0, 0, 0);
                            //List of Educational Background
                            foreach (var cert in userVM.Certificates)
                            {
                                Paragraph prgEducation = new Paragraph();
                                prgEducation.Alignment = Element.ALIGN_LEFT;
                                Phrase certificateReceived = new Phrase();
                                Chunk companyName = new Chunk(cert.CompanyName, boldStyle);
                                Chunk dateIssued = new Chunk(String.Format("{0: MMM dd, yyyy}", cert.DateIssued), boldStyle);
                                Chunk desc = new Chunk(cert.Description, italicStyle);
                                certificateReceived.Add(companyName);
                                certificateReceived.Add(glue);
                                certificateReceived.Add(dateIssued);
                                certificateReceived.Add(new Chunk("\n", arial));
                                certificateReceived.Add(desc);
                                document.Add(certificateReceived);
                                document.Add(new Chunk("\n", arial));
                                document.Add(new Chunk("\n", arial));
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                document.Close();
                writer.Close();
                fs.Close();
            }

        }
    }
}
