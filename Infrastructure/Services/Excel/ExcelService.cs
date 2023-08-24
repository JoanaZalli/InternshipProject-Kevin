using Application.Exceptions.ClientErrors;
using Application.Interfaces.Excel;
using ClosedXML.Excel;
using Domain.Entities;
using InternshipProject.Localizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Services.Excel {
    public class ExcelService : IExcelService {

        private readonly IStringLocalizer<LocalizationResources> _localization;

        public ExcelService(IStringLocalizer<LocalizationResources> localization) {
            _localization = localization;
        }

        public FileStreamResult GenerateMatrixTemplate(Lender lender, Product product) {

            using (var workBook = new XLWorkbook()) {

                // declare headers
                var workSheet = workBook.AddWorksheet("Matrix");

                workSheet.Protect();

                workSheet.Cell(1, 1).Value = "LenderId";
                workSheet.Cell(1, 2).Value = "LenderName";
                workSheet.Cell(1, 3).Value = "ProductId";
                workSheet.Cell(1, 4).Value = "ProductName";
                workSheet.Cell(1, 5).Value = "Tenor";
                workSheet.Cell(1, 6).Value = "Spread";

                workSheet.Row(1).Style.Fill.SetBackgroundColor(XLColor.BabyBlue);

                for (int i = 2, tenor = 11; i <= 56; i++, tenor++) {
                    workSheet.Cell(i, 1).Value = lender.Id.ToString();
                    workSheet.Cell(i, 2).Value = lender.Name;
                    workSheet.Cell(i, 3).Value = product.Id.ToString();
                    workSheet.Cell(i, 4).Value = product.Name;
                    workSheet.Cell(i, 5).Value = tenor;
                    workSheet.Cell(i, 6).Value = "";
                }

                for (int i = 1; i <= 6; i++) {
                    workSheet.Column(i).AdjustToContents();
                }

                workSheet.Column(6).Style.Protection.SetLocked(false);

                workSheet.Cell(2, 6).Style.Fill.SetBackgroundColor(XLColor.Red);

                // download file
                var stream = new MemoryStream();
                workBook.SaveAs(stream);
                stream.Position = 0;

                return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    FileDownloadName = "Lender Matrix.xlsx"
                };
            }
        }

        public FileStreamResult GenerateMatrixTemplate(List<LenderMatrix> matrices, string lenderName, string productName) {

            using (var workBook = new XLWorkbook()) {

                // declare headers
                var workSheet = workBook.AddWorksheet("Matrix");

                workSheet.Protect();

                workSheet.Cell(1, 1).Value = "LenderId";
                workSheet.Cell(1, 2).Value = "LenderName";
                workSheet.Cell(1, 3).Value = "ProductId";
                workSheet.Cell(1, 4).Value = "ProductName";
                workSheet.Cell(1, 5).Value = "Tenor";
                workSheet.Cell(1, 6).Value = "Spread";

                workSheet.Row(1).Style.Fill.SetBackgroundColor(XLColor.BabyBlue);

                var i = 2;

                matrices = matrices.OrderBy(x => x.Tenor).ToList();

                foreach (var matrix in matrices) {
                    workSheet.Cell(i, 1).Value = matrix.LenderId.ToString();
                    workSheet.Cell(i, 2).Value = lenderName;
                    workSheet.Cell(i, 3).Value = matrix.ProductId.ToString();
                    workSheet.Cell(i, 4).Value = productName;
                    workSheet.Cell(i, 5).Value = matrix.Tenor;
                    workSheet.Cell(i, 6).Value = matrix.Spread;
                    i++;
                }

                for (i = 1; i <= 6; i++)
                    workSheet.Column(i).AdjustToContents();

                workSheet.Column(6).Style.Protection.SetLocked(false);

                workSheet.Cell(2, 6).Style.Fill.SetBackgroundColor(XLColor.Red);

                // download file
                var stream = new MemoryStream();
                workBook.SaveAs(stream);
                stream.Position = 0;

                return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    FileDownloadName = "Lender Matrix.xlsx"
                };
            }
        }

        public async Task<List<LenderMatrix>> ReadMatrix(IFormFile file, Guid lenderId, Guid productId) {

            using var stream = new MemoryStream();
            var list = new List<LenderMatrix>();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var workBook = new XLWorkbook(stream);
            var workSheet = workBook.Worksheet(1);

            var rows = workSheet.RangeUsed().RowsUsed().Skip(1);

            if (rows.Count() < 55)
                throw new InvalidRequestException(_localization.GetString("EmptyExcelCells").Value);

            if (workSheet.Columns().Count() < 6)
                throw new InvalidRequestException(_localization.GetString("EmptyExcelCells").Value);

            foreach (var row in rows) {

                var lenderIdCell = row.Cell(1).Value;
                var productIdCell = row.Cell(3).Value;
                var tenorCell = row.Cell(5).Value;
                var spreadCell = row.Cell(6).Value;

                if (IsCellNullOrEmpty(lenderIdCell) || IsCellNullOrEmpty(productIdCell) || IsCellNullOrEmpty(tenorCell) || IsCellNullOrEmpty(spreadCell))
                    throw new InvalidRequestException(_localization.GetString("EmptyExcelCells").Value);

                if (Guid.Parse(lenderIdCell.ToString()) == lenderId && Guid.Parse(productIdCell.ToString()) == productId) {
                    decimal spread;
                    int tenor;

                    try {
                        spread = decimal.Parse(spreadCell.ToString());
                    }
                    catch (FormatException ex) {
                        throw new InvalidRequestException(_localization.GetString("SpreadConstraint").Value);
                    }

                    if (spread < 0.03M || spread > 0.08M)
                        throw new InvalidRequestException(_localization.GetString("SpreadConstraint").Value);

                    try {
                        tenor = int.Parse(tenorCell.ToString());
                    }
                    catch (FormatException ex) {
                        throw new InvalidRequestException(_localization.GetString("TenorConstraint").Value);
                    }

                    if (tenor < 11 || tenor > 65)
                        throw new InvalidRequestException(_localization.GetString("TenorConstraint").Value);

                    list.Add(new LenderMatrix {
                        Id = Guid.NewGuid(),
                        LenderId = lenderId,
                        ProductId = productId,
                        Spread = spread,
                        Tenor = tenor
                    });
                }
            }

            if (list.Any() is false)
                throw new NotFoundException(string.Format(_localization.GetString("NoExcelRowExists").Value,
                                                                          lenderId, productId));

            return list;
        }

        private static bool IsCellNullOrEmpty(XLCellValue cell) {
            return cell.IsBlank || cell.ToString() == null || string.IsNullOrWhiteSpace(cell.ToString());
        }
    }
}
