using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExampleApp.Api.Controllers;
using ExampleApp.Api.Interfaces;

namespace ExampleApp.Api.Services;

public class ExcelFileProcessorService : IFileProcessorService
{
    public bool CanProcess(string contentType, string extension)
    {
        return (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || contentType == "application/vnd.ms-excel")
            && (extension == ".xlsx" || extension == ".xls");
    }

    public List<StudentEnrollmentCourseBulkRequestModel> Process(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        using var document = SpreadsheetDocument.Open(stream, false);
        var workbookPart = document.WorkbookPart;
        var sheet = workbookPart.Workbook.Descendants<Sheet>().First();
        var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
        var rows = worksheetPart.Worksheet.Descendants<Row>().ToList();

        var headerCells = rows[0].Elements<Cell>().ToList();
        var columnIndexByName = new Dictionary<string, int>
        {
            { "StudentName", headerCells.FindIndex(c => GetCellValue(workbookPart, c) == "StudentName") },
            { "StudentBadge", headerCells.FindIndex(c => GetCellValue(workbookPart, c) == "StudentBadge") },
            { "CourseId", headerCells.FindIndex(c => GetCellValue(workbookPart, c) == "CourseId") }
        };

        List<StudentEnrollmentCourseBulkRequestModel> records = new List<StudentEnrollmentCourseBulkRequestModel>();

        for (int i = 1; i < rows.Count; i++)
        {
            var row = rows[i];
            var cells = row.Elements<Cell>().ToList();

            var record = new StudentEnrollmentCourseBulkRequestModel
            {
                StudentName = GetCellValue(workbookPart, cells[columnIndexByName["StudentName"]]),
                StudentBadge = GetCellValue(workbookPart, cells[columnIndexByName["StudentBadge"]]),
                CourseId = GetCellValue(workbookPart, cells[columnIndexByName["CourseId"]])
            };

            records.Add(record);
        }

        return records;
    }

    private string? GetCellValue(WorkbookPart workbookPart, Cell cell)
    {
        var stringTablePart = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
        return cell.DataType != null && cell.DataType.Value == CellValues.SharedString
        ? stringTablePart.SharedStringTable.ChildElements[int.Parse(cell.CellValue.Text)].InnerText
        : (cell.CellValue?.Text);
    }
}
