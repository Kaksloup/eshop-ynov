using Catalog.API.Enums;
using FluentValidation;
using OfficeOpenXml;

namespace Catalog.API.Features.Products.Commands.ImportProduct;

public class ExcelWorksheetValidator: AbstractValidator<ExcelWorksheet>
{
    public ExcelWorksheetValidator()
    {
        RuleFor(w => w)
            .Must(HaveValidHeader).WithMessage("The header must be populated with : Name, Description, Price, ImageFile, Categories");
    }

    private bool HaveValidHeader(ExcelWorksheet worksheet)
    {
        if (!worksheet.Dimension.Columns.Equals(Enum.GetValues<ProductColumns>().Length)) return false;
        
        for (int column = 1; column <= worksheet.Dimension.Columns; column++)
        {
            var header = worksheet.Cells[1, column].Value.ToString();
            var expectedHeader = Enum.GetValues<ProductColumns>()[column - 1].ToString();
            if (!string.Equals(header, expectedHeader)) return false;
        }
        return true;
    }
}