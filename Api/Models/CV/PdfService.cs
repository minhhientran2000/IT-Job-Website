namespace Api.Models.CV
{
    public class PdfService
    {
        public byte[] GeneratePdf(string htmlContent)
        {
            var Renderer = new IronPdf.HtmlToPdf();
            var PDF = Renderer.RenderHtmlAsPdf(htmlContent);
            return PDF.BinaryData;
        }
    }
}
