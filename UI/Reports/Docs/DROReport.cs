using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;

using Bll;
using Bll.Services;
using Dto.Extensions;
using Dto.Lists;
using Dto.Models;
using Reports;
using UI.Properties;

namespace UI.Reports.Docs {
  public class DROReport : ReportBase {
    private readonly int _companyId;
    private readonly int _year;
    private readonly int _month;

    public DROReport(int companyId, int? year, int? month) : base(Resources.DROReport) {
      _companyId = companyId;
      _year = year ?? DateTime.Today.Year;
      _month = month ?? DateTime.Today.Month;
    }

    public Document CreateDocument() {
      DefineStyles();
      Paragraph paragraph;

      using Services<Empresa> empresa = new Services<Empresa>();
      string companyName = empresa.GetById(_companyId).Razao;
      string companyLogo = empresa.GetById(_companyId)?.Logo;

      decimal totalCr;

      /*
       * Demonstrativo de Resultado da Operacao
       * Receitas Operacionais no Periodo
       */
      using (Services<ReceitaCo> receitas = new Services<ReceitaCo>()) {
        Expression<Func<ReceitaCo, bool>> condition = 
            co => (co.EmpresaId == _companyId) && (co.Ano == _year) && (co.Mes == _month);
        ReceitaCo receita = receitas.GetFirst(condition);
        totalCr = receita.Liquida;

        AddSection(Orientation.Portrait);
        Header(this.section, new List<string>() { companyName, document.Info.Title, $"{Mes.Items[_month]}/{_year}" }, companyLogo);
        Footer(this.section);
        AddTable(this.section);

        Unit[] colSize = new Unit[5] { "8 cm", "2,75 cm", "2.75 cm", "1.75 cm", "2,75 cm" };
        Column column;
        for (int k = 0; k < colSize.Length; k++) {
          column = table.AddColumn(colSize[k]);
          column.Format.Alignment = ParagraphAlignment.Right;
        }

        Row row = table.AddRow();
        row.Height = "1 cm";
        row.Format.Font.Bold = true;
        row.Format.Alignment = ParagraphAlignment.Center;
        row.VerticalAlignment = VerticalAlignment.Center;

        row.Cells[1].AddParagraph($"{Resources.Receita} {Resources.Bruta} {NumberFormatInfo.CurrentInfo.CurrencySymbol}");

        row.Cells[2].MergeRight = 1;
        row.Cells[2].AddParagraph(Resources.Impostos);

        row.Cells[4].AddParagraph($"{Resources.Receita} {Resources.Liquida} {NumberFormatInfo.CurrentInfo.CurrencySymbol}");
        row.Borders.Bottom.Visible = true;

        row = table.AddRow();
        row.Height = "1 cm";
        row.Format.Font.Bold = true;
        row.VerticalAlignment = VerticalAlignment.Center;

        row.Cells[0].AddParagraph($"[A] - {Resources.ReceitasCo}");
        row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

        row.Cells[1].AddParagraph($"{receita.Receita:#,##0.00}");
        row.Cells[2].AddParagraph($"{receita.Impostos:#,##0.00}");
        row.Cells[3].AddParagraph($"{receita.Aliquota:P2}");
        row.Cells[4].AddParagraph($"{receita.Liquida:#,##0.00}");
        row.Borders.Bottom.Visible = true;

        paragraph = document.LastSection.AddParagraph();
      }

      /*
       * Demonstrativo de Resultado da Operacao
       * Custos Operacionais no Periodo
       */
      using (CustoCoService custos = new CustoCoService()) {
        Expression<Func<CustoCo, bool>> condition = 
            co => (co.EmpresaId == _companyId) && (co.Ano == _year) && (co.Mes == _month);

        AddTable(this.section);
        Unit[] colSize = new Unit[4] { "9 cm", "3 cm", "3 cm", "3 cm" };
        Column column;
        for (int k = 0; k < colSize.Length; k++) {
          column = table.AddColumn(colSize[k]);
          column.Format.Alignment = ParagraphAlignment.Right;
        }

        Row row = table.AddRow();
        row.Height = "1 cm";
        row.Format.Font.Bold = true;
        row.Format.Alignment = ParagraphAlignment.Center;
        row.VerticalAlignment = VerticalAlignment.Center;

        row.Cells[1].AddParagraph(Resources.PercursoMedio);
        row.Cells[2].AddParagraph(Resources.Coeficiente);
        row.Cells[3].AddParagraph(Resources.Custo);
        row.Borders.Bottom.Visible = true;

        // Custo Total
        row = table.AddRow();
        row.Height = "1 cm";
        row.Format.Font.Bold = true;
        row.VerticalAlignment = VerticalAlignment.Center;

        row.Cells[0].AddParagraph($"[B] - {Resources.CustoCo}");
        row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

        row.Cells[2].AddParagraph($"{custos.GetQuery(condition).Sum(p => p.Coeficiente):#,##0.00000}");
        row.Cells[3].AddParagraph($"{custos.GetQuery(condition).Sum(p => p.Custo):#,##0.00}");
        row.Borders.Bottom.Visible = true;

        int[] titulos = custos.GetQuery(condition)
                            .Select(p => p.TotalId).Distinct().ToArray();
        foreach (int tituloId in titulos) {
          int[] subtitulos = custos.GetQuery(Predicate.And(condition, p => p.TotalId == tituloId))
                                 .Select(p => p.SubtotalId).Distinct().ToArray();
          decimal?[] totais = new decimal?[2] {
              custos.GetQuery(Predicate.And(condition, p => p.TotalId == tituloId)).Sum(q => q.Coeficiente),
              custos.GetQuery(Predicate.And(condition, p => p.TotalId == tituloId)).Sum(q => q.Custo)
          };
          row = table.AddRow();
          row.Height = "0.8 cm";
          row.Format.Font.Bold = true;
          row.VerticalAlignment = VerticalAlignment.Center;

          row.Cells[0].AddParagraph(new Services<Rubrica>().GetById(tituloId).Denominacao);
          row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

          row.Cells[2].AddParagraph($"{totais[0]:#,##0.00000}");
          row.Cells[3].AddParagraph($"{totais[1]:#,##0.00}");

          foreach (int subtituloId in subtitulos) {
            totais = new decimal?[2] {
                custos.GetQuery(Predicate.And(condition, p => (p.SubtotalId == subtituloId) &&
                                                              (p.TotalId == tituloId))).Sum(q => q.Coeficiente),
                custos.GetQuery(Predicate.And(condition, p => (p.SubtotalId == subtituloId) &&
                                                              (p.TotalId == tituloId))).Sum(q => q.Custo)
            };

            row = table.AddRow();
            row.Height = "0.7 cm";
            row.Format.Font.Bold = true;
            row.VerticalAlignment = VerticalAlignment.Center;

            row.Cells[0].Format.LeftIndent = "0.25 in";
            row.Cells[0].AddParagraph(new Services<Rubrica>().GetById(subtituloId).Denominacao);
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

            row.Cells[2].AddParagraph($"{totais[0]:#,##0.00000}");
            row.Cells[3].AddParagraph($"{totais[1]:#,##0.00}");

            foreach (CustoCo item in custos.GetQuery(Predicate.And(
                                                         condition, p => (p.SubtotalId == subtituloId) && 
                                                                         (p.TotalId == tituloId)),
                                                     p => p.OrderBy(q => q.RubricaId))) {
              row = table.AddRow();
              row.Height = "0.7 cm";
              row.VerticalAlignment = VerticalAlignment.Center;

              row.Cells[0].Format.LeftIndent = "0.5 in";
              row.Cells[0].AddParagraph(item.Rubrica.Denominacao);
              row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

              row.Cells[1].AddParagraph($"{item.Percurso:#,##0.0}");
              row.Cells[2].AddParagraph($"{item.Coeficiente:#,##0.00000}");
              row.Cells[3].AddParagraph($"{item.Custo:#,##0.00}");
            }
          }
        }
        paragraph = document.LastSection.AddParagraph();
        paragraph.Format.Borders.Bottom = new Border() { Width = "0.1 mm", Color = Colors.Black };
        paragraph.Format.SpaceAfter = "0.1 in";

        AddTable(this.section);
        colSize = new Unit[2] { "14 cm", "4 cm" };
        for (int k = 0; k < colSize.Length; k++) {
          column = table.AddColumn(colSize[k]);
          column.Format.Alignment = ParagraphAlignment.Right;
        }

        row = table.AddRow();
        row.Height = "0.7 cm";
        row.VerticalAlignment = VerticalAlignment.Center;

        row.Cells[0].AddParagraph($"{Resources.ReceitasCo} {Resources.Liquida}");
        row.Cells[1].AddParagraph($"{totalCr:#,##0.00}");

        row = table.AddRow();
        row.Height = "0.7 cm";
        row.VerticalAlignment = VerticalAlignment.Center;

        row.Cells[0].AddParagraph($"{Resources.CustoCo}");
        row.Cells[1].AddParagraph($"{custos.GetQuery(condition).Sum(p => p.Custo):#,##0.00}");

        row = table.AddRow();
        row.Height = "0.7 cm";
        row.Format.Font.Bold = true;
        row.VerticalAlignment = VerticalAlignment.Center;

        string aux = ((totalCr - custos.GetQuery(condition).Sum(p => p.Custo)) < 0) ? Resources.Deficit : Resources.Superavit;
        if ((totalCr - custos.GetQuery(condition).Sum(p => p.Custo)) < 0) {
          row.Format.Font.Color = Colors.Red;
        }
        row.Cells[0].AddParagraph($"[A] - [B] {aux}");
        row.Cells[1].AddParagraph($"{totalCr - custos.GetQuery(condition).Sum(p => p.Custo):#,##0.00;(#,##0.00)}");

        paragraph = document.LastSection.AddParagraph();
        paragraph.Format.Borders.Bottom = new Border() { Width = "0.1 mm", Color = Colors.Black };
      }

      /*
       * Demonstrativo de Resultado da Linha
       */
      using (CustoLnService cstLinhas = new CustoLnService()) {
        Expression<Func<CustoLn, bool>> condition =
            ln => (ln.EmpresaId == _companyId) && (ln.Ano == _year) && (ln.Mes == _month);

        int[] linhas = cstLinhas.GetQuery(condition).Select(p => p.LinhaId).Distinct().ToArray();
        foreach (int linhaId in linhas) {
          Linha linha = new Services<Linha>().GetById(linhaId);

          AddSection(Orientation.Portrait);
          Header(this.section, new List<string>() { companyName, Resources.DROReportLn, $"{Mes.Items[_month]}/{_year}" }, companyLogo);
          Footer(this.section, $"{linha.Prefixo} - {linha.Denominacao}");
          paragraph = document.LastSection.AddParagraph();

          Unit[] colSize = new Unit[2] { "2 cm", "16 cm" };
          Column column;
          AddTable(this.section);
          for (int k = 0; k < colSize.Length; k++) {
            column = table.AddColumn(colSize[k]);
            column.Format.Alignment = ParagraphAlignment.Left;
          }

          Row row = table.AddRow();
          row.Height = "1 cm";
          row.VerticalAlignment = VerticalAlignment.Center;
          row.Format.Font.Bold = true;

          row.Cells[0].AddParagraph(Resources.LinhaId);
          row.Cells[1].AddParagraph($"{linha.Prefixo} - {linha.Denominacao}");
          row.Borders.Bottom.Visible = true;

          // Receitas Operacionais no Periodo
          using Services<ReceitaLn> receitas = new Services<ReceitaLn>();
          Expression<Func<ReceitaLn, bool>> filter =
              ln => (ln.LinhaId == linhaId) && (ln.Ano == _year) && (ln.Mes == _month);
          ReceitaLn receita = receitas.GetFirst(filter);
          totalCr = receita.Liquida;

          AddTable(this.section);
          colSize = new Unit[5] { "8 cm", "2,75 cm", "2.75 cm", "1.75 cm", "2,75 cm" };
          for (int k = 0; k < colSize.Length; k++) {
            column = table.AddColumn(colSize[k]);
            column.Format.Alignment = ParagraphAlignment.Right;
          }

          row = table.AddRow();
          row.Height = "1 cm";
          row.Format.Font.Bold = true;
          row.Format.Alignment = ParagraphAlignment.Center;
          row.VerticalAlignment = VerticalAlignment.Center;

          row.Cells[1].AddParagraph($"{Resources.Receita} {Resources.Bruta} {NumberFormatInfo.CurrentInfo.CurrencySymbol}");

          row.Cells[2].MergeRight = 1;
          row.Cells[2].AddParagraph(Resources.Impostos);

          row.Cells[4].AddParagraph($"{Resources.Receita} {Resources.Liquida} {NumberFormatInfo.CurrentInfo.CurrencySymbol}");
          row.Borders.Bottom.Visible = true;

          row = table.AddRow();
          row.Height = "1 cm";
          row.Format.Font.Bold = true;
          row.VerticalAlignment = VerticalAlignment.Center;

          row.Cells[0].AddParagraph($"[A] - {Resources.ReceitasCo}");
          row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

          row.Cells[1].AddParagraph($"{receita.Receita:#,##0.00}");
          row.Cells[2].AddParagraph($"{receita.Impostos:#,##0.00}");
          row.Cells[3].AddParagraph($"{receita.Aliquota:P2}");
          row.Cells[4].AddParagraph($"{receita.Liquida:#,##0.00}");
          row.Borders.Bottom.Visible = true;

          paragraph = document.LastSection.AddParagraph();

          // Custos Operacionais no Periodo
          Expression<Func<CustoLn, bool>> where =
              ln => (ln.LinhaId == linhaId) && (ln.Ano == _year) && (ln.Mes == _month);

          AddTable(this.section);
          colSize = new Unit[4] { "9 cm", "3 cm", "3 cm", "3 cm" };
          for (int k = 0; k < colSize.Length; k++) {
            column = table.AddColumn(colSize[k]);
            column.Format.Alignment = ParagraphAlignment.Right;
          }

          row = table.AddRow();
          row.Height = "1 cm";
          row.Format.Font.Bold = true;
          row.Format.Alignment = ParagraphAlignment.Center;
          row.VerticalAlignment = VerticalAlignment.Center;

          row.Cells[1].AddParagraph(Resources.PercursoMedio);
          row.Cells[2].AddParagraph(Resources.Coeficiente);
          row.Cells[3].AddParagraph(Resources.Custo);
          row.Borders.Bottom.Visible = true;

          // Custo Total
          row = table.AddRow();
          row.Height = "1 cm";
          row.Format.Font.Bold = true;
          row.VerticalAlignment = VerticalAlignment.Center;

          row.Cells[0].AddParagraph($"[B] - {Resources.CustoCo}");
          row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

          row.Cells[2].AddParagraph($"{cstLinhas.GetQuery(where).Sum(p => p.Coeficiente):#,##0.00000}");
          row.Cells[3].AddParagraph($"{cstLinhas.GetQuery(where).Sum(p => p.Custo):#,##0.00}");
          row.Borders.Bottom.Visible = true;

          int[] titulos = cstLinhas.GetQuery(where)
                              .Select(p => p.TotalId).Distinct().ToArray();
          foreach (int tituloId in titulos) {
            int[] subtitulos = cstLinhas.GetQuery(Predicate.And(where, p => p.TotalId == tituloId))
                                   .Select(p => p.SubtotalId).Distinct().ToArray();
            decimal?[] totais = new decimal?[2] {
                cstLinhas.GetQuery(Predicate.And(where, p => p.TotalId == tituloId)).Sum(q => q.Coeficiente),
                cstLinhas.GetQuery(Predicate.And(where, p => p.TotalId == tituloId)).Sum(q => q.Custo)
            };
            row = table.AddRow();
            row.Height = "0.8 cm";
            row.Format.Font.Bold = true;
            row.VerticalAlignment = VerticalAlignment.Center;

            row.Cells[0].AddParagraph(new Services<Rubrica>().GetById(tituloId).Denominacao);
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

            row.Cells[2].AddParagraph($"{totais[0]:#,##0.00000}");
            row.Cells[3].AddParagraph($"{totais[1]:#,##0.00}");

            foreach (int subtituloId in subtitulos) {
              totais = new decimal?[2] {
                  cstLinhas.GetQuery(Predicate.And(where, p => (p.SubtotalId == subtituloId) &&
                                                               (p.TotalId == tituloId))).Sum(q => q.Coeficiente),
                  cstLinhas.GetQuery(Predicate.And(where, p => (p.SubtotalId == subtituloId) &&
                                                               (p.TotalId == tituloId))).Sum(q => q.Custo)
              };

              row = table.AddRow();
              row.Height = "0.7 cm";
              row.Format.Font.Bold = true;
              row.VerticalAlignment = VerticalAlignment.Center;

              row.Cells[0].Format.LeftIndent = "0.25 in";
              row.Cells[0].AddParagraph(new Services<Rubrica>().GetById(subtituloId).Denominacao);
              row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

              row.Cells[2].AddParagraph($"{totais[0]:#,##0.00000}");
              row.Cells[3].AddParagraph($"{totais[1]:#,##0.00}");

              foreach (CustoLn item in cstLinhas.GetQuery(Predicate.And(
                                                              where, p => (p.SubtotalId == subtituloId) &&
                                                                          (p.TotalId == tituloId)),
                                                          p => p.OrderBy(q => q.RubricaId))) {
                row = table.AddRow();
                row.Height = "0.7 cm";
                row.VerticalAlignment = VerticalAlignment.Center;

                row.Cells[0].Format.LeftIndent = "0.5 in";
                row.Cells[0].AddParagraph(item.Rubrica.Denominacao);
                row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

                row.Cells[1].AddParagraph($"{item.Percurso:#,##0.0}");
                row.Cells[2].AddParagraph($"{item.Coeficiente:#,##0.00000}");
                row.Cells[3].AddParagraph($"{item.Custo:#,##0.00}");
              }
            }
          }
          paragraph = document.LastSection.AddParagraph();
          paragraph.Format.Borders.Bottom = new Border() { Width = "0.1 mm", Color = Colors.Black };
          paragraph.Format.SpaceAfter = "0.1 in";

          AddTable(this.section);
          colSize = new Unit[2] { "14 cm", "4 cm" };
          for (int k = 0; k < colSize.Length; k++) {
            column = table.AddColumn(colSize[k]);
            column.Format.Alignment = ParagraphAlignment.Right;
          }

          row = table.AddRow();
          row.Height = "0.7 cm";
          row.VerticalAlignment = VerticalAlignment.Center;

          row.Cells[0].AddParagraph($"{Resources.ReceitasCo} {Resources.Liquida}");
          row.Cells[1].AddParagraph($"{totalCr:#,##0.00}");

          row = table.AddRow();
          row.Height = "0.7 cm";
          row.VerticalAlignment = VerticalAlignment.Center;

          row.Cells[0].AddParagraph($"{Resources.CustoCo}");
          row.Cells[1].AddParagraph($"{cstLinhas.GetQuery(where).Sum(p => p.Custo):#,##0.00}");

          row = table.AddRow();
          row.Height = "0.7 cm";
          row.Format.Font.Bold = true;
          row.VerticalAlignment = VerticalAlignment.Center;

          string aux = ((totalCr - cstLinhas.GetQuery(where).Sum(p => p.Custo)) < 0) ? Resources.Deficit : Resources.Superavit;
          if ((totalCr - cstLinhas.GetQuery(where).Sum(p => p.Custo)) < 0) {
            row.Format.Font.Color = Colors.Red;
          }
          row.Cells[0].AddParagraph($"[A] - [B] {aux}");
          row.Cells[1].AddParagraph($"{totalCr - cstLinhas.GetQuery(where).Sum(p => p.Custo):#,##0.00;(#,##0.00)}");

          paragraph = document.LastSection.AddParagraph();
          paragraph.Format.Borders.Bottom = new Border() { Width = "0.1 mm", Color = Colors.Black };
        }
      }
      return this.document;
    }

    public override void Header(Section section, List<string> text, string image = null) {
      Style style = document.Styles[StyleNames.Header];
      style.ParagraphFormat.Font.Name = "Verdana";
      style.ParagraphFormat.Font.Size = 10;

      string fileName = $@"C:\Temp\OptCo\{image}";
      if (File.Exists(fileName)) {
        Image logo = section.Headers.Primary.AddImage(fileName);
        logo.Height = "0.75 in";
        logo.Width = "1 in";
        logo.Top = ShapePosition.Top;
        logo.Left = ShapePosition.Left;
        logo.WrapFormat.Style = WrapStyle.Through;
      }

      for (int i = 0; i < text.Count; i++) {
        Paragraph paragraph = section.Headers.Primary.AddParagraph();
        paragraph.AddFormattedText(text[i], TextFormat.Bold);
        paragraph.Format.Alignment = ParagraphAlignment.Center;
      }
    }

    public override void Footer(Section section, string text = null) {
      DefaultFooter(section);

      Row row = table.AddRow();
      row.Height = "0.25 in";
      row.VerticalAlignment = VerticalAlignment.Center;
      row.Borders.Top.Visible = true;

      Paragraph paragraph = row.Cells[0].AddParagraph();
      paragraph.Format.SpaceBefore = "0.01 in";
      paragraph.AddText($"{Mes.Items[_month]}/{_year}");

      paragraph = row.Cells[1].AddParagraph();
      paragraph.Format.SpaceBefore = "0.01 in";
      if (!string.IsNullOrWhiteSpace(text)) {
        paragraph.AddText(text);
      }

      paragraph = row.Cells[2].AddParagraph();
      paragraph.Format.SpaceBefore = "0.01 in";
      AddPageNumber(paragraph);
    }
  }
}
