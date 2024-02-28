using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExampleApp.Api.Interfaces;

namespace ExampleApp.Api.Services;

public class ExcelFileProcessorService : IFileProcessorService
{
    public bool CanProcess(string contentType, string extension)
    {
        return (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || contentType == "application/vnd.ms-excel")
            && (extension == ".xlsx" || extension == ".xls");
    }

    public List<T> Process<T>(IFormFile file) where T : class, new()
    {
        using var stream = file.OpenReadStream();
        using var document = SpreadsheetDocument.Open(stream, false);
        var workbookPart = document.WorkbookPart;
        var rows = GetRows(workbookPart);
        var columnIndexByName = GetColumnIndexByName(workbookPart, rows[0]);

        var records = new List<T>();

        for (int i = 1; i < rows.Count; i++)
        {
            var record = CreateRecord<T>(workbookPart, rows[i], columnIndexByName);
            records.Add(record);
        }

        return records;
    }

    private List<Row>? GetRows(WorkbookPart workbookPart)
    {
        var sheet = workbookPart?.Workbook.Descendants<Sheet>().First();
        var worksheetPart = (WorksheetPart?)workbookPart?.GetPartById(sheet.Id);
        return worksheetPart?.Worksheet.Descendants<Row>().ToList();
    }

    private Dictionary<string, int> GetColumnIndexByName(WorkbookPart workbookPart, Row headerRow)
    {
        var headerCells = headerRow.Elements<Cell>().ToList();
        return headerCells
            .Select((c, i) => new
            {
                ColumnName = GetCellValue(workbookPart, c),
                Index = i
            })
            .Where(x => x.ColumnName != null)
            .ToDictionary(x => x.ColumnName, x => x.Index);
    }

    private static T CreateRecord<T>(WorkbookPart workbookPart, Row row, Dictionary<string, int> columnIndexByName) where T : class, new()
    {
        var cells = row.Elements<Cell>().ToList();
        var record = new T();
        var recordType = record.GetType();

        foreach (var column in columnIndexByName)
        {
            var cell = cells.ElementAtOrDefault(column.Value);
            if (cell != null)
            {
                string? cellValue = GetCellValue(workbookPart, cell);
                var property = recordType.GetProperty(column.Key);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(record, Convert.ChangeType(cellValue, property.PropertyType));
                }
            }
        }

        return record;
    }

    private static string GetCellValue(WorkbookPart workbookPart, Cell cell)
    {
        string value = cell.InnerText;

        if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
        {
            var stringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

            if (stringTable != null)
            {
                value = stringTable.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
            }
        }

        return value;
    }
}
