using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.html;
using Azure;
using iTextSharp.tool.xml.html;
using System.Data;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateCVController : ControllerBase
    {

        private readonly IWebHostEnvironment _hostingEnvironment;

        public CreateCVController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        public async Task<IActionResult> GenerateCV(CvData cvData)
        {
            /*var pdfBytes = await PdfGenerator.GeneratePdfAsync(model);*/
           
            var htmlFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "styles", "index.html");
            var content = System.IO.File.ReadAllText(htmlFilePath);
            var replacedHtml = content.Replace("@cvData.Name", cvData.Name)
                                      .Replace("@cvData.Email", cvData.Email)
                                      .Replace("@cvData.PhoneNumber", cvData.PhoneNumber)
                                      .Replace("@cvData.Address", cvData.Address)
                                      .Replace("@cvData.Summary", cvData.Summary)
                                      .Replace("@cvData.GitUrl", cvData.GitUrl)
                                      .Replace("@cvData.Photo", cvData.Photo)
                                      .Replace("@cvData.Target", cvData.Target);
            string skillHtml = "";
            foreach (var skill in cvData.Skills)
            {
                skillHtml += $"<li><span>{skill.Name}</span></li>";
                
            }
            replacedHtml = replacedHtml.Replace("@cvData.Skill", skillHtml);

            string certiHtml = "";
            foreach (var cer in cvData.Certificates)
            {
                certiHtml += $"<li> {cer.Name}</li>";

            }
            replacedHtml = replacedHtml.Replace("@cvData.Certificate", certiHtml);

            string eduHtml = "";
            foreach (var edu in cvData.Education)
            {
                eduHtml += @"<li>
                                <div>
                                    <p class=""sanserif"">"+edu.Institution+ @"</p>
                                    <time>"+edu.Year+ @"</time>
                                </div>
                                <div>
                                    <span>"+edu.Major+ @"</span>
                                    <span>"+edu.GPA+ @"</span>
                                </div>
                            </li>";
            }
            replacedHtml = replacedHtml.Replace("@cvData.Education", eduHtml);

            

            string expHtml = "";
            foreach (var exp in cvData.WorkExperiences)
            {
                expHtml += @"<li>
                                <header>
                                    <p class=""sanserif"">"+exp.Company + @"</p>
                                    <time>"+exp.Year+ @"</time>
                                </header>
                                <span>"+exp.Title+ @"</span>
                                <ul style=""font-style: italic;"">";
                var descriptions = exp.Description.Split('.'); // Phân chia mô tả thành các câu

                // Tạo một thẻ <li> cho mỗi câu trong mô tả
                foreach (var description in descriptions)
                {
                    expHtml += @"<li>" + description.Trim() + @"</li>";
                }
                expHtml += @"</ul>
                            </li>";
            }
            replacedHtml = replacedHtml.Replace("@cvData.WorkExperiences", expHtml);


            string projectHtml = "";
            foreach (var proj in cvData.Projects)
            {
                projectHtml += @"<li>
                                    <header>
                                        <p class=""sanserif""> " + proj.Name + @" </p>
                                       
                                        <time> " + proj.Year + @" </time>
                                    </header>                                                                   
                                    <ul style=""font-style: italic;"">
                                        <li style=""overflow-wrap: break-word;"">Description: " + proj.Description + @"</li>
                                        <li>Features: " + proj.Features + @"</li>
                                        <li>Technologies: " + proj.Technology + @"</li>
                                        <li>Link: " + proj.Link + @"</li>
                                    </ul>
                                   </li>
                                    ";
                /*var achive = proj.Achivement.Split('.');
                foreach (var description in achive)
                {
                    projectHtml += @"<li>" + description.Trim() + @"</li>";
                }
                projectHtml += @"</ul>
                <span>Technologies: "+ proj.Technology + @"</span>
                </li>";  */                                     
                
            }
            replacedHtml = replacedHtml.Replace("@cvData.Projects", projectHtml);

            var cssPath = Path.Combine(_hostingEnvironment.WebRootPath, "styles", "style.css");
            var cssContent = System.IO.File.ReadAllText(cssPath);
            replacedHtml = replacedHtml.Replace("styles/style.css", "data:text/css;base64," + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(cssContent)));

           
            var cssBoot = Path.Combine(_hostingEnvironment.WebRootPath, "styles", "bootstrap.min.css");
            var cssBootContent = System.IO.File.ReadAllText(cssBoot);
            replacedHtml = replacedHtml.Replace("styles/bootstrap.min.css", "data:text/css;base64," + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(cssBootContent)));


            var Renderer = new IronPdf.HtmlToPdf();
            var PDF = Renderer.RenderHtmlAsPdf(replacedHtml);
            var pdfBytes = PDF.BinaryData;

            return File(pdfBytes, "application/pdf", "CV.pdf");
        }
    }
    public class PdfGenerator
    {
        public static async Task<byte[]> GeneratePdfAsync(CvData cvData)
        {
            using (var memoryStream = new MemoryStream())
            {

                Document document = new Document(PageSize.A4);               
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();


                /*// Font
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font titleFont = new Font(baseFont, 20, Font.BOLD);
                Font sectionTitleFont = new Font(baseFont, 16, Font.BOLD);
                Font normalFont = new Font(baseFont, 12, Font.NORMAL);

                // Title
                Paragraph title = new Paragraph("CV", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 10;
                document.Add(title);

                // Personal Information
                Paragraph personalInfoTitle = new Paragraph("Personal Information", sectionTitleFont);
                personalInfoTitle.SpacingAfter = 5;
                document.Add(personalInfoTitle);

                document.Add(new Paragraph($"Name: {cvData.Name}", normalFont));
                document.Add(new Paragraph($"Email: {cvData.Email}", normalFont));
                document.Add(new Paragraph($"Phone: {cvData.PhoneNumber}", normalFont));
                document.Add(new Paragraph($"Address: {cvData.Address}", normalFont));

                // Education
                Paragraph educationTitle = new Paragraph("Education", sectionTitleFont);
                educationTitle.SpacingBefore = 10;
                educationTitle.SpacingAfter = 5;
                document.Add(educationTitle);

                foreach (var edu in cvData.Education)
                {
                    document.Add(new Paragraph($"{edu.GPA} from {edu.Institution}, {edu.Major}, {edu.Year}", normalFont));
                }

                // Work Experience
                Paragraph experienceTitle = new Paragraph("Experience", sectionTitleFont);
                experienceTitle.SpacingBefore = 10;
                experienceTitle.SpacingAfter = 5;
                document.Add(experienceTitle);

                foreach (var exp in cvData.WorkExperiences)
                {
                    document.Add(new Paragraph($"{exp.Title} at {exp.Company}, {exp.Year}", normalFont));
                    document.Add(new Paragraph($"Description: {exp.Description}", normalFont));
                }

                // Skills
                Paragraph skillTitle = new Paragraph("Skill", sectionTitleFont);
                skillTitle.SpacingBefore = 10;
                skillTitle.SpacingAfter = 5;
                document.Add(skillTitle);

                foreach (var skill in cvData.Skills)
                {
                    document.Add(new Paragraph($"{skill.Name}, {skill.Level}", normalFont));
                }*/


                string htmlContent = @"

                  <html><head><style>body { font-family: Arial, sans-serif; }</style></head><body><h1>Hello, iText 7!</h1><p>This is a sample HTML to PDF conversion.</p></body></html>
                 ";

                var htmlString = @"
                    <div style=""color: red; "" class='title'>CV</div>
                    <div class='section-title'>Personal Information</div>
                    <div class='info' id='name'>Name: " + cvData.Name + @"</div>
                    <div class='info' id='email'>Email: " + cvData.Email + @"</div>
                    <div class='info' id='phone'>Phone: " + cvData.PhoneNumber + @"</div>
                    <div class='info' id='address'>Address: " + cvData.Address + @"</div>
                    <div class='section-title'>Education</div>";

                foreach (var edu in cvData.Education)
                {
                    htmlString += $"<div class='edu-info'>{edu.GPA} from {edu.Institution}, {edu.Major}, {edu.Year}</div>";
                }

                htmlString += @"<div class='section-title'>Experience</div>";

                foreach (var exp in cvData.WorkExperiences)
                {
                    htmlString += $"<div class='exp-info'>{exp.Title} at {exp.Company}, {exp.Year}<br/>Description: {exp.Description}</div>";
                }

                htmlString += @"<div class='section-title'>Skills</div>";

                foreach (var skill in cvData.Skills)
                {
                    htmlString += $"<div class='skill-info'>{skill.Name}</div>";
                }

                htmlString += "</div>";


                StyleSheet styles = new StyleSheet();
                styles.LoadTagStyle(HtmlTags.DIV, "font-family", "Arial, sans-serif");
           
                styles.LoadTagStyle("title", "text-align", "center");
               
                styles.LoadTagStyle(HtmlTags.DIV, "text-align", "center");

                var htmlContextElement = new HTMLWorker(document);
                htmlContextElement.SetStyleSheet(styles);
                htmlContextElement.Parse(new StringReader(htmlContent));

                document.Close();
                return memoryStream.ToArray();
            }
        }
    }


}
