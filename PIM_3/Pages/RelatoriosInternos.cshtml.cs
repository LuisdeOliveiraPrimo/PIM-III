using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace PIM_3.Pages
{
    public class RelatoriosInternosModel : PageModel
    {
        public IActionResult OnGetDownload(string reportId, string format)
        {
            if (string.IsNullOrWhiteSpace(reportId) || string.IsNullOrWhiteSpace(format))
            {
                return BadRequest("Relatório ou formato inválido.");
            }

            var reportName = GetReportName(reportId);
            var fileExtension = GetFileExtension(format);
            var fileName = $"{reportName}_{DateTime.UtcNow:yyyyMMddHHmmss}.{fileExtension}";
            var (contentType, contentBytes) = CreateReportBytes(reportName, format);

            return File(contentBytes, contentType, fileName);
        }

        private static string GetReportName(string reportId) => reportId switch
        {
            "fechamento" => "Fechamento_Mensal",
            "impacto" => "Impacto_PVPS",
            "fornecedores" => "Auditoria_Fornecedor",
            "equipe" => "Performance_Equipe",
            "inventario" => "Inventario_Geral",
            _ => "Relatorio"
        };

        private static string GetFileExtension(string format) => format.ToLower() switch
        {
            "pdf" => "pdf",
            "xlsx" => "xlsx",
            "csv" => "csv",
            _ => "txt"
        };

        private static (string contentType, byte[] bytes) CreateReportBytes(string reportName, string format)
        {
            var content = new StringBuilder();
            content.AppendLine($"Relatorio: {reportName}");
            content.AppendLine($"Gerado em: {DateTime.UtcNow:dd/MM/yyyy HH:mm:ss} UTC");
            content.AppendLine();
            content.AppendLine("Este arquivo foi gerado automaticamente pelo Stoquee.me como demonstracao de download.");
            content.AppendLine();
            content.AppendLine("Resumo:");
            content.AppendLine("- Dados inventariados");
            content.AppendLine("- Indicadores de performance");
            content.AppendLine("- Valores consolidados e ajustes operacionais");

            var text = content.ToString();

            return format.ToLower() switch
            {
                "pdf" => ("application/pdf", CreatePdfBytes(text)),
                "xlsx" => ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", CreateXlsxBytes(reportName, text)),
                "csv" => ("text/csv", Encoding.UTF8.GetBytes('\uFEFF' + text)),
                _ => ("application/octet-stream", Encoding.UTF8.GetBytes(text))
            };
        }

        private static byte[] CreatePdfBytes(string content)
        {
            var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var streamBuilder = new StringBuilder();
            streamBuilder.AppendLine("BT");
            streamBuilder.AppendLine("/F1 12 Tf");
            streamBuilder.AppendLine("50 760 Td");

            foreach (var line in lines)
            {
                streamBuilder.AppendLine($"({EscapePdfText(line)}) Tj");
                streamBuilder.AppendLine("0 -16 Td");
            }

            streamBuilder.AppendLine("ET");
            var streamBytes = Encoding.ASCII.GetBytes(streamBuilder.ToString());

            using var pdf = new MemoryStream();
            var offsets = new List<int>();

            void Write(string text)
            {
                var bytes = Encoding.ASCII.GetBytes(text);
                pdf.Write(bytes, 0, bytes.Length);
            }

            Write("%PDF-1.4\n");
            offsets.Add((int)pdf.Position);
            Write("1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n");
            offsets.Add((int)pdf.Position);
            Write("2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n");
            offsets.Add((int)pdf.Position);
            Write("3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>\nendobj\n");
            offsets.Add((int)pdf.Position);
            Write("4 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>\nendobj\n");
            offsets.Add((int)pdf.Position);
            Write($"5 0 obj\n<< /Length {streamBytes.Length} >>\nstream\n");
            pdf.Write(streamBytes, 0, streamBytes.Length);
            Write("\nendstream\nendobj\n");

            var xrefPosition = (int)pdf.Position;
            Write("xref\n0 6\n0000000000 65535 f \n");
            foreach (var offset in offsets)
            {
                Write($"{offset:0000000000} 00000 n \n");
            }

            Write($"trailer\n<< /Size 6 /Root 1 0 R >>\nstartxref\n{xrefPosition}\n%%EOF");
            return pdf.ToArray();
        }

        private static string EscapePdfText(string text)
        {
            var safe = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(text));
            return safe.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)").Replace("\r", string.Empty).Replace("\n", "\\n");
        }

        private static byte[] CreateXlsxBytes(string reportName, string content)
        {
            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                AddEntry(archive, "[Content_Types].xml",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                    "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">" +
                    "<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>" +
                    "<Default Extension=\"xml\" ContentType=\"application/xml\"/>" +
                    "<Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>" +
                    "<Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>" +
                    "<Override PartName=\"/docProps/core.xml\" ContentType=\"application/vnd.openxmlformats-package.core-properties+xml\"/>" +
                    "<Override PartName=\"/docProps/app.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.extended-properties+xml\"/>" +
                    "</Types>");

                AddEntry(archive, "_rels/.rels",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                    "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                    "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>" +
                    "</Relationships>");

                AddEntry(archive, "xl/workbook.xml",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                    "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
                    "<sheets><sheet name=\"Relatorio\" sheetId=\"1\" r:id=\"rId1\"/></sheets>" +
                    "</workbook>");

                AddEntry(archive, "xl/_rels/workbook.xml.rels",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                    "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                    "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>" +
                    "</Relationships>");

                AddEntry(archive, "xl/worksheets/sheet1.xml", GenerateWorksheetXml(content));

                AddEntry(archive, "docProps/core.xml",
                    $"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                    "<cp:coreProperties xmlns:cp=\"http://schemas.openxmlformats.org/package/2006/metadata/core-properties\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:dcterms=\"http://purl.org/dc/terms/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                    $"<dc:title>{EscapeXml(reportName)}</dc:title>" +
                    "<dc:creator>Stoque.me</dc:creator>" +
                    "<cp:lastModifiedBy>Stoque.me</cp:lastModifiedBy>" +
                    $"<dcterms:created xsi:type=\"dcterms:W3CDTF\">{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}</dcterms:created>" +
                    $"<dcterms:modified xsi:type=\"dcterms:W3CDTF\">{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}</dcterms:modified>" +
                    "</cp:coreProperties>");

                AddEntry(archive, "docProps/app.xml",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                    "<Properties xmlns=\"http://schemas.openxmlformats.org/officeDocument/2006/extended-properties\">" +
                    "<Application>Microsoft Excel</Application>" +
                    "</Properties>");
            }

            return memoryStream.ToArray();
        }

        private static void AddEntry(ZipArchive archive, string entryName, string content)
        {
            var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
            using var writer = new StreamWriter(entry.Open(), Encoding.UTF8);
            writer.Write(content);
        }

        private static string GenerateWorksheetXml(string content)
        {
            var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var builder = new StringBuilder();
            builder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.AppendLine("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
            builder.AppendLine("  <sheetData>");

            for (var index = 0; index < lines.Length; index++)
            {
                builder.AppendLine($"    <row r=\"{index + 1}\">\n      <c r=\"A{index + 1}\" t=\"inlineStr\"><is><t>{EscapeXml(lines[index])}</t></is></c>\n    </row>");
            }

            builder.AppendLine("  </sheetData>");
            builder.AppendLine("</worksheet>");
            return builder.ToString();
        }

        private static string EscapeXml(string text)
        {
            return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }
    }
}