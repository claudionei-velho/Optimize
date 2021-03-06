﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

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
  public class FichaTecnicaReport : ReportBase {
    private readonly Expression<Func<Tecnical, bool>> filter;

    public FichaTecnicaReport(Expression<Func<Tecnical, bool>> _filter) : base(Resources.FichaTecnicaPreview) {
      this.filter = _filter;
    }

    public Document CreateDocument() {
      char[] charsToTrim = new char[] { ' ', ';', '/' };
      Dictionary<int, int> listAtt = new Dictionary<int, int>();
      StringBuilder concat = new StringBuilder();

      DefineStyles();
      Paragraph paragraph;

      /*
       * Dimensionamento da Frota Operacional (Dias Uteis)
       */
      using TecnicalService lines = new TecnicalService();
      int companyId = lines.GetFirst(this.filter).EmpresaId;

      using Services<Empresa> empresa = new Services<Empresa>();
      string companyName = empresa.GetById(companyId).Razao;
      string companyLogo = empresa.GetById(companyId)?.Logo;
/*
      using (Services<FuUtil> utils = new Services<FuUtil>()) {
        Expression<Func<FuUtil, bool>> filter = f => f.EmpresaId == companyId;

        if (utils.Exists(filter)) {
          AddSection(Orientation.Portrait);
          Header(this.section, new List<string>() { companyName, document.Info.Title }, companyLogo);
          Footer(this.section);
          AddTable(this.section);

          Unit[] colSize = new Unit[7] { "3.5 cm", "2.4 cm", "2.4 cm", "2.4 cm", "2.4 cm", "2.4 cm", "2.5 cm" };
          Column column;
          for (int k = 0; k < colSize.Length; k++) {
            column = table.AddColumn(colSize[k]);
            column.Format.Alignment = ParagraphAlignment.Center;
          }

          Row row = table.AddRow();
          row.Height = "1 cm";
          row.Format.Font.Bold = true;

          row.Cells[0].AddParagraph(Resources.FaixaHoraria);
          row.Cells[1].AddParagraph(Resources.LinhaCap);
          row.Cells[2].AddParagraph(Resources.TotalViagens);
          row.Cells[3].AddParagraph(Resources.ExtensaoCap);
          row.Cells[4].AddParagraph(Resources.Veiculos);
          row.Cells[5].AddParagraph(Resources.PercursoCap);
          row.Cells[6].AddParagraph(Resources.VelocidadeComl);

          foreach (FuUtil item in utils.GetQuery(filter)) {
            row = table.AddRow();
            row.Height = "0.75 cm";
            row.Format.Font.Bold = item.Veiculos == utils.GetQuery(filter).Max(p => p.Veiculos);

            row.Cells[0].AddParagraph(item.Faixa);
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

            row.Cells[1].AddParagraph($"{item.QtdLinhas:#,##0}");
            row.Cells[2].AddParagraph($"{item.Viagens:#,##0}");
            row.Cells[3].AddParagraph($"{item.Extensao:#,##0.00}");
            row.Cells[4].AddParagraph($"{item.Veiculos:#,##0}");

            row.Cells[5].AddParagraph($"{item.Percurso:#,##0.0}");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Right;

            row.Cells[6].AddParagraph($"{$"{NumericExtensions.SafeDivision(item.Percurso, (decimal)item.Veiculos):#,##0.0}"} {Resources.SpeedCap}");
            row.Cells[6].Format.Alignment = ParagraphAlignment.Right;
          }
          row = table.AddRow();
          row.Height = "1 cm";
          row.Format.Font.Bold = true;

          row.Cells[1].AddParagraph($"{utils.GetQuery(filter).Max(p => p.QtdLinhas):#,##0}");
          row.Cells[2].AddParagraph($"{utils.GetQuery(filter).Sum(p => p.Viagens):#,##0}");
          row.Cells[3].AddParagraph($"{NumericExtensions.SafeDivision(utils.GetQuery(filter).Sum(p => p.Percurso), utils.GetQuery(filter).Sum(p => p.Viagens)):#,##0.00}");
          row.Cells[4].AddParagraph($"{utils.GetQuery(filter).Max(p => p.Veiculos):#,##0}");

          row.Cells[5].AddParagraph($"{utils.GetQuery(filter).Sum(p => p.Percurso):#,##0.0}");
          row.Cells[5].Format.Alignment = ParagraphAlignment.Right;

          decimal result = NumericExtensions.SafeDivision(utils.GetQuery(filter).Sum(p => p.Percurso),
                                                          (decimal)utils.GetQuery(filter).Sum(p => p.Veiculos));
          row.Cells[6].AddParagraph($"{$"{result:#,##0.0}"} {Resources.SpeedCap}");
          row.Cells[6].Format.Alignment = ParagraphAlignment.Right;

          result = NumericExtensions.SafeDivision(utils.GetQuery(filter).Sum(p => p.Veiculos),
                                                  (decimal)utils.GetQuery(filter).Max(p => p.Veiculos));

          paragraph = document.LastSection.AddParagraph();
          paragraph.Format.Alignment = ParagraphAlignment.Left;
          paragraph.Format.SpaceBefore = "0.1 in";
          paragraph.Format.Font.Name = "Lucida Console";
          paragraph.Format.Font.Size = 10;
          paragraph.AddFormattedText($"{Resources.HorasOperacao}: {$"{result:#.00}"}", TextFormat.Bold);
        }
      } */

      /*
       * Resumo Executivo do Sistema
       */
      using (TecnicalService linhas = new TecnicalService()) {
        companyId = linhas.GetFirst(this.filter).EmpresaId;
        decimal?[,] values = new decimal?[3, 6] { { 0, 0, 0, 0, 0, 0 },
                                                  { 0, 0, 0, 0, 0, 0 },
                                                  { 0, 0, 0, 0, 0, 0 } };

        foreach (var groupItem in linhas.GetQuery(this.filter).Select(
                                      p => new { p.EmpresaId, p.Classificacao }).Distinct()) {
          companyName = empresa.GetById(companyId).Razao;
          companyLogo = empresa.GetById(companyId)?.Logo;

          using CLinhaService cLinha = new CLinhaService();
          string catName = cLinha.GetById(groupItem.Classificacao)?.ClassLinha.Denominacao;

          AddSection(Orientation.Landscape);
          Header(this.section, new List<string>() { companyName, document.Info.Title }, companyLogo);
          Footer(this.section);
          AddTable(this.section);

          Unit[] colSize = new Unit[12] { "0.8 cm", "6.5 cm", "2.1 cm", "2 cm", "2.35 cm", "2.1 cm",
                                          "1.75 cm", "2.1 cm", "1.75 cm", "1.75 cm", "1.8 cm", "1.8 cm" };
          Column column;
          for (int k = 0; k < colSize.Length; k++) {
            column = table.AddColumn(colSize[k]);
            column.Format.Alignment = ParagraphAlignment.Right;
          }

          Row row = table.AddRow();
          row.Height = "1 cm";
          row.Format.Font.Bold = true;

          for (int k = 0; k < colSize.Length; k++) {
            row.Cells[k].Format.Alignment = ParagraphAlignment.Center;
          }

          row.Cells[0].MergeRight = 1;
          row.Cells[0].AddParagraph(Resources.LinhaId);

          row.Cells[2].AddParagraph(Resources.Extensao);
          row.Cells[3].AddParagraph($"{Resources.Viagens} {Resources.Mensal}");
          row.Cells[4].AddParagraph($"{Resources.Percurso} {Resources.Mensal}");

          row.Cells[5].MergeRight = 1;
          row.Cells[5].AddParagraph($"{Resources.Passageiros}*");

          row.Cells[7].MergeRight = 2;
          row.Cells[7].AddParagraph(Resources.Equivalencia);

          row.Cells[10].AddParagraph(Resources.IPK);
          row.Cells[11].AddParagraph(Resources.IPKe);

          row = table.AddRow();
          row.Height = "0.75 cm";

          row.Cells[0].MergeRight = 1;
          row.Cells[0].AddParagraph(catName);
          row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
          row.Cells[0].Format.Font.Bold = true;

          foreach (Tecnical item in linhas.GetQuery(Predicate.And(
                                                        this.filter,
                                                        p => p.Classificacao == groupItem.Classificacao)
                                                    ).OrderBy(p => p.Id)) {
            row = table.AddRow();
            row.Height = "0.75 cm";

            row.Cells[1].AddParagraph($"{item.Prefixo} - {item.Denominacao}");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;

            // Total de Viagens e Percurso Medio Mensal            
            using Services<ViagemLinha> viagens = new Services<ViagemLinha>();
            values[0, 0] = viagens.GetQuery(q => q.LinhaId == item.Id).Sum(p => p.ViagensAno);
            values[0, 1] = viagens.GetQuery(q => q.LinhaId == item.Id).Sum(p => p.PercursoAno);

            try {
              row.Cells[2].AddParagraph($"{values[0, 1] / values[0, 0]:#,##0.00}");
            }
            catch (DivideByZeroException) { }

            row.Cells[3].AddParagraph($"{NumericExtensions.SafeDivision(values[0, 0] ?? 0, CustomCalendar.MonthsPerYear):#,#}");
            row.Cells[4].AddParagraph($"{NumericExtensions.SafeDivision(values[0, 1] ?? 0, CustomCalendar.MonthsPerYear):#,##0.0}");

            // Demanda Mensal da Linha
            using Services<DemandaMod> demanda = new Services<DemandaMod>();
            Expression<Func<DemandaMod, bool>> filter = q => q.LinhaId == item.Id;
            values[0, 2] = (decimal)(demanda.GetQuery(filter)?.Average(p => p.Passageiros) ?? 0);
            values[0, 3] = (decimal)(demanda.GetQuery(filter)?.Average(p => p.Equivalente) ?? 0);
            values[0, 4] = demanda.GetQuery(filter)?.Sum(p => p.Passageiros) ?? 0;
            values[0, 5] = demanda.GetQuery(filter)?.Sum(p => p.Equivalente) ?? 0;

            row.Cells[5].AddParagraph($"{values[0, 2]:#,#}");
            row.Cells[6].AddParagraph($"{NumericExtensions.SafeDivision(values[0, 2], NumericExtensions.SafeDivision(values[0, 0], CustomCalendar.MonthsPerYear)):#,#}");
            row.Cells[7].AddParagraph($"{values[0, 3]:#,#}");
            row.Cells[8].AddParagraph($"{NumericExtensions.SafeDivision(values[0, 3], NumericExtensions.SafeDivision(values[0, 0], CustomCalendar.MonthsPerYear)):#,#}");

            decimal aux = NumericExtensions.SafeDivision((decimal)values[0, 3], (decimal)values[0, 2]);
            if (aux != 0) {
              row.Cells[9].AddParagraph($"{aux:P1}");
            }
            aux = NumericExtensions.SafeDivision(values[0, 2], NumericExtensions.SafeDivision(values[0, 1], CustomCalendar.MonthsPerYear));
            if (aux != 0) {
              row.Cells[10].AddParagraph($"{aux:0.000}");
            }
            aux = NumericExtensions.SafeDivision(values[0, 3], NumericExtensions.SafeDivision(values[0, 1], CustomCalendar.MonthsPerYear));
            if (aux != 0) {
              row.Cells[11].AddParagraph($"{aux:0.000}");
            }

            for (int j = 0; j < values.GetLength(1); j++) {
              values[1, j] += values[0, j] ?? 0;
            }
          }

          // Subtotais do Grupo
          using Services<DemandaMod> pass = new Services<DemandaMod>();
          int month = pass.GetQuery(d => d.EmpresaId == companyId).Select(p => new { p.Ano, p.Mes }).Distinct().Count();

          if (linhas.GetQuery(Predicate.And(this.filter, p => p.Classificacao == groupItem.Classificacao)).Count() > 1) {
            row = table.AddRow();
            row.Height = "0.75 cm";
            row.Format.Font.Bold = true;

            row.Cells[1].AddParagraph($"{Resources.Subtotal}: {catName}");
            try {
              row.Cells[2].AddParagraph($"{values[1, 1] / values[1, 0]:#,##0.00}");
            }
            catch (DivideByZeroException) { }

            row.Cells[3].AddParagraph($"{NumericExtensions.SafeDivision(values[1, 0] ?? 0, CustomCalendar.MonthsPerYear):#,#}");
            row.Cells[4].AddParagraph($"{NumericExtensions.SafeDivision(values[1, 1] ?? 0, CustomCalendar.MonthsPerYear):#,##0.0}");

            values[1, 2] = NumericExtensions.SafeDivision(values[1, 4], month);
            row.Cells[5].AddParagraph($"{values[1, 2]:#,#}");
            row.Cells[6].AddParagraph($"{NumericExtensions.SafeDivision(values[1, 2], NumericExtensions.SafeDivision(values[1, 0], CustomCalendar.MonthsPerYear)):#,#}");

            values[1, 3] = NumericExtensions.SafeDivision(values[1, 5], month);
            row.Cells[7].AddParagraph($"{values[1, 3]:#,#}");
            row.Cells[8].AddParagraph($"{NumericExtensions.SafeDivision(values[1, 3], NumericExtensions.SafeDivision(values[1, 0], CustomCalendar.MonthsPerYear)):#,#}");

            decimal aux = NumericExtensions.SafeDivision((decimal)values[1, 3], (decimal)values[1, 2]);
            if (aux != 0) {
              row.Cells[9].AddParagraph($"{aux:P1}");
            }

            aux = NumericExtensions.SafeDivision(values[1, 2], NumericExtensions.SafeDivision(values[1, 1], CustomCalendar.MonthsPerYear));
            if (aux != 0) {
              row.Cells[10].AddParagraph($"{aux:0.000}");
            }

            aux = NumericExtensions.SafeDivision(values[1, 3], NumericExtensions.SafeDivision(values[1, 1], CustomCalendar.MonthsPerYear));
            if (aux != 0) {
              row.Cells[11].AddParagraph($"{aux:0.000}");
            }
          }
          for (int j = 0; j < values.GetLength(1); j++) {
            values[2, j] += values[1, j] ?? 0;
            values[1, j] = 0;
          }
        }

        // Totais Gerais
        using Services<DemandaMod> demands = new Services<DemandaMod>();
        int months = demands.GetQuery(d => d.EmpresaId == companyId).Select(p => new { p.Ano, p.Mes }).Distinct().Count();

        if (linhas.GetQuery(this.filter).Select(p => new { p.EmpresaId, p.Classificacao }).Distinct().Count() > 1) {
          Row row = table.AddRow();
          row.Height = "0.75 cm";
          row.Format.Font.Bold = true;

          row.Cells[1].AddParagraph(Resources.GranTotal);
          try {
            row.Cells[2].AddParagraph($"{values[2, 1] / values[2, 0]:#,##0.00}");
          }
          catch (DivideByZeroException) { }

          row.Cells[3].AddParagraph($"{NumericExtensions.SafeDivision(values[2, 0] ?? 0, CustomCalendar.MonthsPerYear):#,#}");
          row.Cells[4].AddParagraph($"{NumericExtensions.SafeDivision(values[2, 1] ?? 0, CustomCalendar.MonthsPerYear):#,##0.0}");

          values[2, 2] = NumericExtensions.SafeDivision(values[2, 4], months);
          row.Cells[5].AddParagraph($"{values[2, 2]:#,#}");
          row.Cells[6].AddParagraph($"{NumericExtensions.SafeDivision(values[2, 2], NumericExtensions.SafeDivision(values[2, 0], CustomCalendar.MonthsPerYear)):#,#}");

          values[2, 3] = NumericExtensions.SafeDivision(values[2, 5], months);
          row.Cells[7].AddParagraph($"{values[2, 3]:#,#}");
          row.Cells[8].AddParagraph($"{NumericExtensions.SafeDivision(values[2, 3], NumericExtensions.SafeDivision(values[2, 0], CustomCalendar.MonthsPerYear)):#,#}");

          decimal aux = NumericExtensions.SafeDivision((decimal)values[2, 3], (decimal)values[2, 2]);
          if (aux != 0) {
            row.Cells[9].AddParagraph($"{aux:P1}");
          }

          aux = NumericExtensions.SafeDivision(values[2, 2], NumericExtensions.SafeDivision(values[2, 1], CustomCalendar.MonthsPerYear));
          if (aux != 0) {
            row.Cells[10].AddParagraph($"{aux:0.000}");
          }

          aux = NumericExtensions.SafeDivision(values[2, 3], NumericExtensions.SafeDivision(values[2, 1], CustomCalendar.MonthsPerYear));
          if (aux != 0) {
            row.Cells[11].AddParagraph($"{aux:0.000}");
          }

          if (demands.Exists(d => d.EmpresaId == companyId)) {
            int? minYear = demands.GetQuery(d => d.EmpresaId == companyId).Min(p => p.Ano);
            int? maxYear = demands.GetQuery(d => d.EmpresaId == companyId).Max(p => p.Ano);
            int[] times = new int[2] {
                demands.GetQuery(d => d.EmpresaId == companyId && d.Ano == minYear).Min(p => p.Mes),
                demands.GetQuery(d => d.EmpresaId == companyId && d.Ano == maxYear).Max(p => p.Mes)
            };

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.Format.SpaceBefore = "0.1 in";
            paragraph.Format.Font.Name = "Lucida Console";
            paragraph.Format.Font.Size = 9;
            paragraph.Format.LeftIndent = "0.8 cm";
            paragraph.AddFormattedText($"(*) {Resources.FooterNote} {Mes.Short[times[0]].ToLower()}/{minYear} e {Mes.Short[times[1]].ToLower()}/{maxYear}", TextFormat.Italic);
          }
        }
      }

      /*
       * Identificacao e Qualificacao da Linha
       */
      using (TecnicalService linhas = new TecnicalService()) {
        foreach (Tecnical item in linhas.GetQuery(this.filter, q => q.OrderBy(p => p.Id))) {
          concat = new StringBuilder($"{Resources.LinhaId}: {item.Prefixo} - {item.Denominacao}");

          AddSection(Orientation.Portrait);
          Header(this.section, new List<string>() { item.Empresa.Razao, document.Info.Title }, item.Empresa.Logo);
          Footer(this.section, concat.ToString());
          AddTable(this.section);

          Unit[] colSize = new Unit[4] { "4 cm", "4.5 cm", "4.5 cm", "5 cm" };
          Column column;
          for (int k = 0; k < colSize.Length; k++) {
            column = table.AddColumn(colSize[k]);
            column.Format.Alignment = ParagraphAlignment.Left;
          }

          // Prefixo da Linha
          Row row = table.AddRow();
          row.Height = "0.6 cm";
          row.Format.Font.Bold = true;

          row.Cells[0].AddParagraph(Resources.Prefixo);
          row.Cells[1].AddParagraph(item.Prefixo);

          // Denominacao da Linha
          row = table.AddRow();
          row.Height = "0.6 cm";
          row.Format.Font.Bold = true;

          row.Cells[0].AddParagraph(Resources.Denominacao);
          row.Cells[1].MergeRight = 2;
          row.Cells[1].AddParagraph(item.Denominacao);
          row.Cells[1].Format.Font.Bold = false;

          if (item.Viagem != null) {
            row = table.AddRow();
            row.Height = "0.6 cm";
            row.Format.Font.Bold = true;

            row.Cells[0].AddParagraph(Resources.Viagem);
            row.Cells[1].MergeRight = 2;
            row.Cells[1].AddParagraph(item.Viagem);
            row.Cells[1].Format.Font.Bold = false;
          }

          // Dias de Operacao
          row = table.AddRow();
          row.Height = "0.6 cm";
          row.Format.Font.Bold = true;

          row.Cells[0].AddParagraph(Resources.DiaId);
          row.Cells[1].MergeRight = 2;
          row.Cells[1].AddParagraph(item.DiasOp);
          row.Cells[1].Format.Font.Bold = false;

          // Jurisdicao (Dominio) e Extensao AB
          row = table.AddRow();
          row.Height = "0.6 cm";
          row.Format.Font.Bold = true;

          row.Cells[0].AddParagraph(Resources.DominioId);
          row.Cells[1].AddParagraph(item.EDominio.Dominio.Denominacao);
          row.Cells[1].Format.Font.Bold = false;

          row.Cells[2].AddParagraph(Resources.ExtensaoAB);
          row.Cells[3].AddParagraph($"{item.ExtensaoAB:#,##0.00}");
          row.Cells[3].Format.Font.Bold = false;

          // Operacao e Extensao BA
          row = table.AddRow();
          row.Height = "0.6 cm";
          row.Format.Font.Bold = true;

          row.Cells[0].AddParagraph(Resources.OperacaoId);
          row.Cells[1].AddParagraph(item.Operacao.OperLinha.Denominacao);
          row.Cells[1].Format.Font.Bold = false;

          row.Cells[2].AddParagraph(Resources.ExtensaoBA);
          row.Cells[3].AddParagraph($"{item.ExtensaoBA:#,##0.00}");
          row.Cells[3].Format.Font.Bold = false;

          // Classificacao e Extensao Total
          row = table.AddRow();
          row.Height = "0.6 cm";
          row.Format.Font.Bold = true;

          row.Cells[0].AddParagraph(Resources.Classificacao);
          row.Cells[1].AddParagraph(item.CLinha.ClassLinha.Denominacao);
          row.Cells[1].Format.Font.Bold = false;

          row.Cells[2].AddParagraph(Resources.Extensao);
          row.Cells[3].AddParagraph($"{item.Extensao:#,##0.00}");
          row.Cells[3].Format.Font.Bold = false;

          // Pontos Inicial e Final, AB e BA
          foreach (KeyValuePair<string, string> way in Sentido.Items) {
            using ReferenciaService referencias = new ReferenciaService();
            Expression<Func<Referencia, bool>> locate =
                p => (p.LinhaId == item.Id) && (!p.AtendimentoId.HasValue) && p.Sentido.Equals(way.Key);
            if (referencias.GetFirst(locate) == null) {
              continue;
            }

            row = table.AddRow();
            row.Height = "0.8 cm";
            row.Format.Font.Bold = true;

            row.Cells[0].AddParagraph($"{Resources.PontoInicial} {way.Key}");            
            row.Cells[1].AddParagraph(referencias.GetFirst(locate).PInicio.Endereco);
            row.Cells[1].Format.Font.Bold = false;

            row.Cells[2].AddParagraph($"{Resources.PontoFinal} {way.Key}");
            row.Cells[3].AddParagraph(referencias.GetFirst(locate).PTermino.Endereco);
            row.Cells[3].Format.Font.Bold = false;
          }

          // PMM (Percurso Medio Mensal) da Linha
          decimal?[] pmmKm;
          row = table.AddRow();
          row.Height = "0.6 cm";
          row.Format.Font.Bold = true;

          row.Cells[0].AddParagraph(Resources.PMM);
          using (Services<ViagemLinha> viagens = new Services<ViagemLinha>()) {
            Expression<Func<ViagemLinha, bool>> condition = q => q.LinhaId == item.Id;
            pmmKm = new decimal?[2] {
                viagens.GetQuery(condition).Sum(q => q.PercursoAno),
                NumericExtensions.SafeDivision(viagens.GetQuery(condition).Sum(q => q.PercursoAno) ?? 0, CustomCalendar.MonthsPerYear)
            };
            concat = new StringBuilder($"{pmmKm[1]:#,##0.0}");
            row.Cells[1].AddParagraph(concat.ToString());
          }

          /*
           * Atendimentos da Linha
           */
          using (Services<Atendimento> atendimentos = new Services<Atendimento>()) {
            if (atendimentos.Exists(q => q.LinhaId == item.Id)) {
              paragraph = document.LastSection.AddParagraph();
              paragraph.Format.Alignment = ParagraphAlignment.Left;
              paragraph.Format.SpaceBefore = "0.4 cm";
              paragraph.AddFormattedText(Resources.AtendimentoViewModel, TextFormat.Bold);
              AddTable(this.section);

              colSize = new Unit[6] { "1 cm", "2 cm", "7.5 cm", "2.5 cm", "2.5 cm", "2.5 cm" };
              for (int k = 0; k < colSize.Length; k++) {
                column = table.AddColumn(colSize[k]);
              }

              row = table.AddRow();
              row.Height = "1 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              row.Cells[0].MergeRight = 1;
              row.Cells[0].AddParagraph(Resources.Prefixo);

              row.Cells[2].AddParagraph(Resources.Denominacao);
              row.Cells[3].AddParagraph(Resources.ExtensaoAB);
              row.Cells[4].AddParagraph(Resources.ExtensaoBA);
              row.Cells[5].AddParagraph(Resources.Extensao);

              int aux = 0;
              foreach (Atendimento aItem in atendimentos.GetQuery(q => q.LinhaId == item.Id)) {
                listAtt.Add(aItem.Id, ++aux);

                row = table.AddRow();
                row.Height = "0.55 cm";
                row.Format.Alignment = ParagraphAlignment.Right;

                row.Cells[0].AddParagraph($"{aux:#0}");
                row.Cells[1].AddParagraph(aItem.Prefixo);
                row.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row.Cells[2].AddParagraph(aItem.Denominacao);
                row.Cells[2].Format.Alignment = ParagraphAlignment.Left;

                row.Cells[3].AddParagraph($"{aItem.ExtensaoAB:#,##0.00}");
                row.Cells[4].AddParagraph($"{aItem.ExtensaoBA:#,##0.00}");
                row.Cells[5].AddParagraph($"{aItem.Extensao:#,##0.00}");
              }
            }
          }

          /*
           * Quadro de Viagens/Hora da Linha
           */
          paragraph = document.LastSection.AddParagraph();
          paragraph.Format.Alignment = ParagraphAlignment.Left;
          paragraph.Format.SpaceBefore = "0.4 cm";
          paragraph.AddFormattedText(Resources.ViagemHoraViewModel, TextFormat.Bold);

          using (Services<ViagemHora> viagens = new Services<ViagemHora>()) {
            if (viagens.Exists(q => q.LinhaId == item.Id)) {
              AddTable(this.section);

              colSize = new Unit[10];
              for (int k = 0; k < colSize.Length; k++) {
                colSize[k] = (k == 0) ? "3.6 cm" : "1.6 cm";
                table.AddColumn(colSize[k]);
              }

              row = table.AddRow();
              row.Height = "0.8 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              row.Cells[0].AddParagraph(Resources.FaixaHoraria);

              row.Cells[1].MergeRight = 2;
              row.Cells[1].AddParagraph(Workday.Items[1]);

              row.Cells[4].MergeRight = 2;
              row.Cells[4].AddParagraph(Workday.Items[2]);

              row.Cells[7].MergeRight = 2;
              row.Cells[7].AddParagraph(Workday.Items[3]);

              row = table.AddRow();
              row.Height = "0.8 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              int[,] cols = new int[,] { { 1, 2 }, { 4, 5 }, { 7, 8 } };
              for (int j = 0; j <= Sentido.Items.Count; j++) {
                for (int k = 0; k < Sentido.Items.Count; k++) {
                  if ((k % 2) == 0) {
                    row.Cells[cols[j, k]].AddParagraph(Sentido.Items["AB"]);
                  }
                  else {
                    row.Cells[cols[j, k]].AddParagraph(Sentido.Items["BA"]);
                  }
                }
              }

              foreach (ViagemHora viagem in viagens.GetQuery(q => q.LinhaId == item.Id)) {
                row = table.AddRow();
                row.Height = "0.55 cm";
                row.Format.Alignment = ParagraphAlignment.Center;

                row.Cells[0].AddParagraph(viagem.HoraCat);
                row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

                row.Cells[1].AddParagraph($"{viagem.UteisAB:#,#}");
                row.Cells[2].AddParagraph($"{viagem.UteisBA:#,#}");
                row.Cells[3].AddParagraph($"{(viagem.UteisAB ?? 0) + (viagem.UteisBA ?? 0):#,#}");

                row.Cells[4].AddParagraph($"{viagem.SabadosAB:#,#}");
                row.Cells[5].AddParagraph($"{viagem.SabadosBA:#,#}");
                row.Cells[6].AddParagraph($"{(viagem.SabadosAB ?? 0) + (viagem.SabadosBA ?? 0):#,#}");

                row.Cells[7].AddParagraph($"{viagem.DomingosAB:#,#}");
                row.Cells[8].AddParagraph($"{viagem.DomingosBA:#,#}");
                row.Cells[9].AddParagraph($"{(viagem.DomingosAB ?? 0) + (viagem.DomingosBA ?? 0):#,#}");
              }

              // Totais
              Expression<Func<ViagemHora, bool>> condition = q => q.LinhaId == item.Id;
              int[] totais = new int[3] {
                  (viagens.GetQuery(condition).Sum(p => p.UteisAB) ?? 0) +
                    (viagens.GetQuery(condition).Sum(p => p.UteisBA) ?? 0),
                  (viagens.GetQuery(condition).Sum(p => p.SabadosAB) ?? 0) +
                    (viagens.GetQuery(condition).Sum(p => p.SabadosBA) ?? 0),
                  (viagens.GetQuery(condition).Sum(p => p.DomingosAB) ?? 0) +
                    (viagens.GetQuery(condition).Sum(p => p.DomingosBA) ?? 0)
              };

              row = table.AddRow();
              row.Height = "0.25 in";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              row.Cells[1].AddParagraph($"{viagens.GetQuery(condition).Sum(p => p.UteisAB):#,#}");
              row.Cells[2].AddParagraph($"{viagens.GetQuery(condition).Sum(p => p.UteisBA):#,#}");
              row.Cells[3].AddParagraph($"{totais[0]:#,#}");
              row.Cells[4].AddParagraph($"{viagens.GetQuery(condition).Sum(p => p.SabadosAB):#,#}");
              row.Cells[5].AddParagraph($"{viagens.GetQuery(condition).Sum(p => p.SabadosBA):#,#}");
              row.Cells[6].AddParagraph($"{totais[1]:#,#}");
              row.Cells[7].AddParagraph($"{viagens.GetQuery(condition).Sum(p => p.DomingosAB):#,#}");
              row.Cells[8].AddParagraph($"{viagens.GetQuery(condition).Sum(p => p.DomingosBA):#,#}");
              row.Cells[9].AddParagraph($"{totais[2]:#,#}");
            }
          }

          /*
           * Horarios da Linha
           */
          paragraph = document.LastSection.AddParagraph();
          paragraph.Format.Alignment = ParagraphAlignment.Left;
          paragraph.Format.SpaceBefore = "0.4 cm";
          paragraph.AddFormattedText(Resources.HorarioViewModel, TextFormat.Bold);

          using Services<Horario> horarios = new Services<Horario>();
          int[] tabelas = horarios.GetQuery(
                              q => q.LinhaId == item.Id, q => q.OrderBy(h => h.DiaId)
                          ).Select(h => h.DiaId).Distinct().ToArray();

          if (tabelas.Length > 0) {
            int aux = 0;
            int size = 6;

            AddTable(this.section);
            for (int i = 0; i < Sentido.Items.Count; i++) {
              for (int j = 0; j < size; j++) {
                column = table.AddColumn("1.45 cm");
              }
              if (i == 0) {
                column = table.AddColumn("0.6 cm");
              }
            }

            foreach (int hr in tabelas) {
              row = table.AddRow();
              row.Height = "1.2 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              concat = new StringBuilder($"{Workday.Items[hr]} ({Sentido.Items["AB"]})");
              row.Cells[0].MergeRight = 5;
              row.Cells[0].AddParagraph(concat.ToString());

              concat = new StringBuilder($"{Workday.Items[hr]} ({Sentido.Items["BA"]})");
              row.Cells[7].MergeRight = 5;
              row.Cells[7].AddParagraph(concat.ToString());

              int[] rows = {
                  horarios.GetCount(q => (q.LinhaId == item.Id) && (q.DiaId == hr) && q.Sentido.Equals("AB")),
                  horarios.GetCount(q => (q.LinhaId == item.Id) && (q.DiaId == hr) && q.Sentido.Equals("BA"))
              };
              aux = (rows[0] > rows[1]) ? rows[0] : rows[1];
              int page = ((aux % size) == 0) ? aux / size : (aux / size) + 1;

              Expression<Func<Horario, bool>> where = null;
              static IOrderedQueryable<Horario> order(IQueryable<Horario> q) => q.OrderBy(e => e.Sentido)
                                                                                 .ThenBy(e => e.Inicio);
              for (int i = 0; i < page; i++) {
                row = table.AddRow();
                row.Height = "0.525 cm";

                for (int j = 0; j < Sentido.Items.Count; j++) {
                  int cell = (j == 0) ? 0 : 7;
                  string ab = (j == 0) ? "AB" : "BA";

                  where = q => (q.LinhaId == item.Id) && (q.DiaId == hr) && q.Sentido.Equals(ab);
                  foreach (Horario horario in horarios.GetQuery(where, order, (i * size), size)) {
                    if (!horario.AtendimentoId.HasValue) {
                      row.Cells[cell++].AddParagraph($@"{horario.Inicio:hh\:mm}");
                    }
                    else {
                      paragraph = row.Cells[cell++].AddParagraph();
                      FormattedText formattedText = paragraph.AddFormattedText($@"{horario.Inicio:hh\:mm}");
                      formattedText.Font.Size = 8;

                      formattedText = paragraph.AddFormattedText($" {$"{listAtt[(int)horario.AtendimentoId]:0}"}");
                      formattedText.Font.Size = 8;
                      formattedText.Bold = true;
                      formattedText.Superscript = true;
                    }
                  }
                }
              }
            }
          }

          /*
           * Periodos Tipicos da Linha
           */
          using (PeriodoTipicoService pTipicos = new PeriodoTipicoService()) {
            if (pTipicos.Exists(q => q.LinhaId == item.Id)) {
              paragraph = document.LastSection.AddParagraph();
              paragraph.Format.Alignment = ParagraphAlignment.Left;
              paragraph.Format.SpaceBefore = "0.4 cm";
              paragraph.Format.SpaceAfter = "0.25 cm";
              paragraph.AddFormattedText(Resources.PrLinhaViewModel, TextFormat.Bold);

              AddTable(this.section);

              colSize = new Unit[6] { "4 cm", "2.5 cm", "2.5 cm", "3 cm", "2.5 cm", "3.5 cm" };
              for (int k = 0; k < colSize.Length; k++) {
                column = table.AddColumn(colSize[k]);
              }

              row = table.AddRow();
              row.Height = "1.2 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              row.Cells[0].AddParagraph(Resources.PeriodoId);
              row.Cells[1].AddParagraph(Resources.InicioPeriodo);
              row.Cells[2].AddParagraph(Resources.TerminoPeriodo);
              row.Cells[3].AddParagraph(Resources.DuracaoPeriodo);
              row.Cells[4].AddParagraph(Resources.Viagens);
              row.Cells[5].AddParagraph(Resources.Ciclo);

              foreach (PeriodoTipico pItem in pTipicos.GetQuery(q => q.LinhaId == item.Id, 
                                                                q => q.OrderBy(e => e.PeriodoId))) {
                row = table.AddRow();
                row.Height = "0.55 cm";
                row.Format.Alignment = ParagraphAlignment.Center;

                row.Cells[0].AddParagraph(pItem.EPeriodo.Denominacao);
                row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

                row.Cells[1].AddParagraph($"{pItem.Inicio:hh\\:mm}");
                row.Cells[2].AddParagraph($"{pItem.Termino:hh\\:mm}");

                concat = new StringBuilder($"{$"{pItem.Duracao:#,##0}"} ({$"{pItem.Duracao / 60:#0}"}:{$"{pItem.Duracao % 60:00}"})");
                row.Cells[3].AddParagraph(concat.ToString());
                row.Cells[3].Format.Alignment = ParagraphAlignment.Right;

                row.Cells[4].AddParagraph($"{pItem.QtdViagens:#,##0}");

                concat = new StringBuilder($"{$"{pItem.Ciclo / 60:#0}"}:{$"{pItem.Ciclo % 60:00}"} ({$"{pItem.CicloAB:#,#}"} + {$"{pItem.CicloBA:#,#}"})");
                row.Cells[5].AddParagraph(concat.ToString());
              }
            }
          }

          /*
           * Plano Operacional da Linha (Horarios)
           */
          paragraph = document.LastSection.AddParagraph();
          paragraph.Format.Alignment = ParagraphAlignment.Left;
          paragraph.Format.SpaceBefore = "0.4 cm";
          paragraph.Format.SpaceAfter = "0.25 cm";
          paragraph.AddFormattedText(Resources.OperacionalViewModel, TextFormat.Bold);

          using (Services<Operacional> operacionais = new Services<Operacional>()) {
            if (operacionais.Exists(q => q.LinhaId == item.Id)) {
              AddTable(this.section);

              colSize = new Unit[4] { "1.5 cm", "1.3 cm", "2.2 cm", "2.2 cm" };
              for (int k = 0; k < colSize.Length; k++) {
                column = table.AddColumn(colSize[k]);
              }
              for (int j = 0; j < 6; j++) {
                Unit _colSize = ((j % 2) == 0) ? "1.5 cm" : "2.1 cm";
                column = table.AddColumn(_colSize);
              }

              row = table.AddRow();
              row.Height = "0.8 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              row.Cells[0].AddParagraph(Resources.Prefixo);
              row.Cells[2].AddParagraph(Resources.Sentido);
              row.Cells[3].AddParagraph(Resources.ExtensaoSentido);

              row.Cells[4].MergeRight = 1;
              row.Cells[4].AddParagraph(Workday.Items[1]);

              row.Cells[6].MergeRight = 1;
              row.Cells[6].AddParagraph(Workday.Items[2]);

              row.Cells[8].MergeRight = 1;
              row.Cells[8].AddParagraph(Workday.Items[3]);

              row = table.AddRow();
              row.Height = "0.8 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              for (int j = 0; j < 6; j += 2) {
                row.Cells[4 + j].AddParagraph(Resources.Viagens);
                row.Cells[5 + j].AddParagraph(Resources.Percurso);
              }

              string prefixo = string.Empty;
              foreach (Operacional opItem in operacionais.GetQuery(q => q.LinhaId == item.Id)) {
                row = table.AddRow();
                row.Height = "0.55 cm";
                row.Format.Alignment = ParagraphAlignment.Right;

                if (!opItem.Prefixo.Equals(prefixo)) {
                  row.Cells[0].AddParagraph(opItem.Prefixo);
                  row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

                  if (opItem.AtendimentoId.HasValue) {
                    row.Cells[1].AddParagraph("Atend.");
                    row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                  }
                }

                row.Cells[2].AddParagraph(Sentido.Items[opItem.Sentido]);
                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[3].AddParagraph($"{opItem.Extensao:#,##0.00}");
                row.Cells[4].AddParagraph($"{opItem.ViagensUtil:#,###}");
                row.Cells[5].AddParagraph($"{opItem.PercursoUtil:#,##0.0}");
                row.Cells[6].AddParagraph($"{opItem.ViagensSab:#,###}");
                row.Cells[7].AddParagraph($"{opItem.PercursoSab:#,##0.0}");
                row.Cells[8].AddParagraph($"{opItem.ViagensDom:#,###}");
                row.Cells[9].AddParagraph($"{opItem.PercursoDom:#,##0.0}");

                prefixo = opItem.Prefixo;
              }

              // Totais
              Expression<Func<Operacional, bool>> condition = q => q.LinhaId == item.Id;
              decimal?[,] totais = new decimal?[3, 2] {
                  { operacionais.GetQuery(condition).Sum(p => p.ViagensUtil),
                    operacionais.GetQuery(condition).Sum(p => p.PercursoUtil) },
                  { operacionais.GetQuery(condition).Sum(p => p.ViagensSab),
                    operacionais.GetQuery(condition).Sum(p => p.PercursoSab) },
                  { operacionais.GetQuery(condition).Sum(p => p.ViagensDom),
                    operacionais.GetQuery(condition).Sum(p => p.PercursoDom) }
              };

              row = table.AddRow();
              row.Height = "0.25 in";
              row.Format.Alignment = ParagraphAlignment.Right;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              row.Cells[2].AddParagraph(Resources.Total);
              decimal[] result = new decimal[2] { (totais[0, 0] ?? 0) * 5,
                                                  (totais[0, 1] ?? 0) * 5 };

              for (int k = 1; k < totais.GetLength(0); k++) {
                result[0] += totais[k, 0] ?? 0;
                result[1] += totais[k, 1] ?? 0;
              }
              row.Cells[3].AddParagraph($"{NumericExtensions.SafeDivision(result[1], result[0]):#,##0.00}");
              row.Cells[4].AddParagraph($"{totais[0, 0]:#,##0}");
              row.Cells[5].AddParagraph($"{totais[0, 1]:#,##0.0}");
              row.Cells[6].AddParagraph($"{totais[1, 0]:#,##0}");
              row.Cells[7].AddParagraph($"{totais[1, 1]:#,##0.0}");
              row.Cells[8].AddParagraph($"{totais[2, 0]:#,##0}");
              row.Cells[9].AddParagraph($"{totais[2, 1]:#,##0.0}");
            }
          }

          /*
           * Plano Operacional da Linha (Info)
           *//*
          paragraph = document.LastSection.AddParagraph();
          paragraph.Format.Alignment = ParagraphAlignment.Left;
          paragraph.Format.SpaceBefore = "0.4 cm";
          paragraph.Format.SpaceAfter = "0.25 cm";
          paragraph.AddFormattedText(Resources.OperacionalViewModel, TextFormat.Bold);

          using (Services<PlanOperacional> operacionais = new Services<PlanOperacional>()) {
            if (operacionais.Exists(q => q.LinhaId == item.Id)) {
              AddTable(this.section);

              colSize = new Unit[4] { "1.5 cm", "1.3 cm", "2.2 cm", "2.2 cm" };
              for (int k = 0; k < colSize.Length; k++) {
                column = table.AddColumn(colSize[k]);
              }
              for (int j = 0; j < 6; j++) {
                Unit _colSize = ((j % 2) == 0) ? "1.5 cm" : "2.1 cm";
                column = table.AddColumn(_colSize);
              }

              row = table.AddRow();
              row.Height = "0.8 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              row.Cells[0].AddParagraph(Resources.Prefixo);
              row.Cells[2].AddParagraph(Resources.Sentido);
              row.Cells[3].AddParagraph(Resources.ExtensaoSentido);

              row.Cells[4].MergeRight = 1;
              row.Cells[4].AddParagraph(Workday.Data[1]);

              row.Cells[6].MergeRight = 1;
              row.Cells[6].AddParagraph(Workday.Data[2]);

              row.Cells[8].MergeRight = 1;
              row.Cells[8].AddParagraph(Workday.Data[3]);

              row = table.AddRow();
              row.Height = "0.8 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              for (int j = 0; j < 6; j += 2) {
                row.Cells[4 + j].AddParagraph(Resources.Viagens);
                row.Cells[5 + j].AddParagraph(Resources.Percurso);
              }

              string prefixo = string.Empty;
              foreach (PlanOperacional opItem in operacionais.GetQuery(q => q.LinhaId == item.Id)) {
                row = table.AddRow();
                row.Height = "0.55 cm";
                row.Format.Alignment = ParagraphAlignment.Right;

                if (!opItem.Prefixo.Equals(prefixo)) {
                  row.Cells[0].AddParagraph(opItem.Prefixo);
                  row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

                  if (opItem.AtendimentoId.HasValue) {
                    row.Cells[1].AddParagraph("Atend.");
                    row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                  }
                }

                row.Cells[2].AddParagraph(Sentido.Items[opItem.Sentido]);
                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[3].AddParagraph($"{opItem.Extensao:#,##0.00}");
                row.Cells[4].AddParagraph($"{opItem.ViagensUtil:#,###}");
                row.Cells[5].AddParagraph($"{opItem.PercursoUtil:#,##0.0}");
                row.Cells[6].AddParagraph($"{opItem.ViagensSab:#,###}");
                row.Cells[7].AddParagraph($"{opItem.PercursoSab:#,##0.0}");
                row.Cells[8].AddParagraph($"{opItem.ViagensDom:#,###}");
                row.Cells[9].AddParagraph($"{opItem.PercursoDom:#,##0.0}");

                prefixo = opItem.Prefixo;
              }

              // Totais
              Expression<Func<PlanOperacional, bool>> condition = q => q.LinhaId == item.Id;
              decimal?[,] totais = new decimal?[3, 2] {
                  { operacionais.GetQuery(condition).Sum(p => p.ViagensUtil),
                    operacionais.GetQuery(condition).Sum(p => p.PercursoUtil) },
                  { operacionais.GetQuery(condition).Sum(p => p.ViagensSab),
                    operacionais.GetQuery(condition).Sum(p => p.PercursoSab) },
                  { operacionais.GetQuery(condition).Sum(p => p.ViagensDom),
                    operacionais.GetQuery(condition).Sum(p => p.PercursoDom) }
              };

              row = table.AddRow();
              row.Height = "0.25 in";
              row.Format.Alignment = ParagraphAlignment.Right;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              row.Cells[2].AddParagraph(Resources.Total);
              decimal[] result = new decimal[2] { (totais[0, 0] ?? 0) * 5,
                                                  (totais[0, 1] ?? 0) * 5 };

              for (int k = 1; k < totais.GetLength(0); k++) {
                result[0] += totais[k, 0] ?? 0;
                result[1] += totais[k, 1] ?? 0;
              }
              row.Cells[3].AddParagraph($"{NumericExtensions.SafeDivision(result[1], result[0]):#,##0.00}");
              row.Cells[4].AddParagraph($"{totais[0, 0]:#,##0}");
              row.Cells[5].AddParagraph($"{totais[0, 1]:#,##0.0}");
              row.Cells[6].AddParagraph($"{totais[1, 0]:#,##0}");
              row.Cells[7].AddParagraph($"{totais[1, 1]:#,##0.0}");
              row.Cells[8].AddParagraph($"{totais[2, 0]:#,##0}");
              row.Cells[9].AddParagraph($"{totais[2, 1]:#,##0.0}");
            }
          } */

          /*
           * Resumo das Operacoes da Linha
           */
          paragraph = document.LastSection.AddParagraph();
          paragraph.Format.Alignment = ParagraphAlignment.Left;
          paragraph.Format.SpaceBefore = "0.4 cm";
          paragraph.Format.SpaceAfter = "0.25 cm";
          paragraph.AddFormattedText(Resources.ViagemLinhaViewModel, TextFormat.Bold);

          using (Services<ViagemLinha> viagens = new Services<ViagemLinha>()) {
            if (viagens.Exists(q => q.LinhaId == item.Id)) {
              AddTable(this.section);

              colSize = new Unit[3] { "1.75 cm", "1.75 cm", "2.5 cm" };
              for (int k = 0; k < colSize.Length; k++) {
                column = table.AddColumn(colSize[k]);
              }
              for (int j = 0; j < 6; j++) {
                Unit _colSize = ((j % 2) == 0) ? "1.75 cm" : "2.25 cm";
                column = table.AddColumn(_colSize);
              }

              row = table.AddRow();
              row.Height = "0.8 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              row.Cells[0].AddParagraph(Resources.Prefixo);
              row.Cells[2].AddParagraph(Resources.DiaId);

              row.Cells[3].MergeRight = 1;
              row.Cells[3].AddParagraph(Resources.Semanal);

              row.Cells[5].MergeRight = 1;
              row.Cells[5].AddParagraph(Resources.Mensal);

              row.Cells[7].MergeRight = 1;
              row.Cells[7].AddParagraph(Resources.Anual);

              row = table.AddRow();
              row.Height = "0.8 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              for (int j = 0; j < 6; j += 2) {
                row.Cells[3 + j].AddParagraph(Resources.Viagens);
                row.Cells[4 + j].AddParagraph(Resources.Percurso);
              }

              string aux = string.Empty;
              foreach (ViagemLinha qItem in viagens.GetQuery(q => q.LinhaId == item.Id)) {
                row = table.AddRow();
                row.Height = "0.55 cm";
                row.Format.Alignment = ParagraphAlignment.Right;

                if (!qItem.Prefixo.Equals(aux)) {
                  row.Cells[0].AddParagraph(qItem.Prefixo);
                  row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

                  if (qItem.AtendimentoId.HasValue) {
                    row.Cells[1].AddParagraph("Atend.");
                    row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                  }
                }

                row.Cells[2].AddParagraph(Workday.Items[qItem.DiaId]);
                row.Cells[2].Format.Alignment = ParagraphAlignment.Left;

                row.Cells[3].AddParagraph($"{qItem.ViagensSemana:#,###}");
                row.Cells[4].AddParagraph($"{qItem.PercursoSemana:#,##0.0}");
                row.Cells[5].AddParagraph($"{qItem.ViagensMes:#,###}");
                row.Cells[6].AddParagraph($"{qItem.PercursoMes:#,##0.0}");
                row.Cells[7].AddParagraph($"{qItem.ViagensAno:#,###}");
                row.Cells[8].AddParagraph($"{qItem.PercursoAno:#,##0.0}");

                aux = qItem.Prefixo;
              }
              int? vgTotal = viagens.GetQuery(q => q.LinhaId == item.Id).Sum(q => q.ViagensAno);
              decimal? kmTotal = viagens.GetQuery(q => q.LinhaId == item.Id).Sum(q => q.PercursoAno);

              row = table.AddRow();
              row.Height = "0.25 in";
              row.Format.Alignment = ParagraphAlignment.Right;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              row.Cells[2].AddParagraph(Resources.Total);
              try {
                row.Cells[3].AddParagraph($"{Math.Round((decimal)vgTotal.Value / CustomCalendar.WeeksPerYear):#,###}");
                row.Cells[4].AddParagraph($"{kmTotal.Value / CustomCalendar.WeeksPerYear:#,##0.0}");
              }
              catch (DivideByZeroException) { }

              try {
                row.Cells[5].AddParagraph($"{Math.Round((decimal)vgTotal.Value / CustomCalendar.MonthsPerYear):#,###}");
                row.Cells[6].AddParagraph($"{kmTotal.Value / CustomCalendar.MonthsPerYear:#,##0.0}");
              }
              catch (DivideByZeroException) { }
              row.Cells[7].AddParagraph($"{vgTotal.Value:#,###}");
              row.Cells[8].AddParagraph($"{kmTotal.Value:#,##0.0}");

              concat = new StringBuilder();
              using (Services<DiaTrabalho> Workdays = new Services<DiaTrabalho>()) {
                foreach (int hr in tabelas) {
                  concat.Append($"{Workday.Items[hr]} = {$"{Workdays.GetById(hr).Dias:#,##0}"}; ");
                }
              }

              row = table.AddRow();
              row.Height = "1 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              row.Cells[0].MergeRight = 8;
              row.Cells[0].AddParagraph(concat.ToString().Trim(charsToTrim));
            }
          }

          /*
           * Mapas e Itinerarios da Linha e dos Atendimentos
           */
          using (Services<ItinerarioDistinct> roteiros = new Services<ItinerarioDistinct>()) {
            foreach (ItinerarioDistinct pItem in roteiros.GetQuery(q => q.LinhaId == item.Id)) {
              section.AddPageBreak();

              Expression<Func<MapaLinha, bool>> where = q => (q.LinhaId == pItem.LinhaId) &&
                                                             (q.AtendimentoId == pItem.AtendimentoId) &&
                                                             q.Sentido.Equals(pItem.Sentido);
              using MapaLinhaService mapas = new MapaLinhaService();
              if (mapas.Exists(where)) {
                foreach (MapaLinha mapa in mapas.GetQuery(where, q => q.OrderBy(m => m.AtendimentoId)
                                                                       .ThenBy(m => m.Sentido))) {
                  string fileName = $@"C:\Temp\Mapas\{mapa.Arquivo}";
                  if (File.Exists(fileName)) {
                    Image image = section.AddImage(fileName);
                    image.Height = "13 cm";
                    image.Width = "17.5 cm";
                    image.Top = ShapePosition.Top;
                    image.Left = ShapePosition.Center;
                    image.WrapFormat.Style = WrapStyle.Through;
                  }
                  paragraph = document.LastSection.AddParagraph();
                  paragraph.Style = "Normal";
                  paragraph.Format.SpaceBefore = "13.75 cm";
                  paragraph.Format.SpaceAfter = "0.2 in";

                  if (!string.IsNullOrWhiteSpace(mapa.Descricao)) {
                    paragraph.AddFormattedText(mapa.Descricao, TextFormat.Bold);
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                  }
                  else {
                    if (!mapa.AtendimentoId.HasValue) {
                      using LinhaService pontos = new LinhaService();
                      concat = new StringBuilder($"{item.Prefixo} ({Sentido.Items[mapa.Sentido]})");
                      paragraph.AddFormattedText(concat.ToString(), TextFormat.Bold);
                      paragraph.Format.Alignment = ParagraphAlignment.Center;
                    }
                    else {
                      using AtendimentoService pontos = new AtendimentoService();
                      concat = new StringBuilder($"{mapa.Atendimento.Prefixo} ({Sentido.Items[mapa.Sentido]})");
                      paragraph.AddFormattedText(concat.ToString(), TextFormat.Bold);
                      paragraph.Format.Alignment = ParagraphAlignment.Center;
                    }
                  }

                  // Itinerarios da Linha e dos Atendimentos
                  if (!pItem.AtendimentoId.HasValue) {
                    Expression<Func<Itinerario, bool>> condition = q => (q.LinhaId == pItem.LinhaId) &&
                                                                        q.Sentido.Equals(pItem.Sentido);
                    using Services<Itinerario> itinerarios = new Services<Itinerario>();
                    if (itinerarios.Exists(condition)) {
                      AddTable(this.section);

                      colSize = new Unit[4] { "1 cm", "8 cm", "1 cm", "8 cm" };
                      for (int k = 0; k < colSize.Length; k++) {
                        column = table.AddColumn(colSize[k]);
                        column.Format.Alignment = (k % 2) == 0 ? ParagraphAlignment.Right : ParagraphAlignment.Left;
                      }
                      string[] percurso = itinerarios.GetQuery(condition, q => q.OrderBy(e => e.Id))
                                              .Select(p => p.Percurso).ToArray();

                      int index = percurso.Length / 2 + percurso.Length % 2;
                      for (int i = 0; i < (percurso.Length / 2 + percurso.Length % 2); i++) {
                        row = table.AddRow();
                        row.Height = "0.55 cm";

                        row.Cells[0].AddParagraph($"{i + 1}.");
                        row.Cells[1].AddParagraph(percurso[i]);
                        if (index < percurso.Length) {
                          row.Cells[2].AddParagraph($"{index + 1}.");
                          row.Cells[3].AddParagraph(percurso[index++]);
                        }
                      }
                    }
                  }
                  else {
                    Expression<Func<ItAtendimento, bool>> condition = q => (q.AtendimentoId == pItem.AtendimentoId) &&
                                                                            q.Sentido.Equals(pItem.Sentido);
                    using Services<ItAtendimento> itAtendimentos = new Services<ItAtendimento>();
                    if (itAtendimentos.Exists(condition)) {
                      AddTable(this.section);

                      colSize = new Unit[4] { "1 cm", "8 cm", "1 cm", "8 cm" };
                      for (int k = 0; k < colSize.Length; k++) {
                        column = table.AddColumn(colSize[k]);
                        column.Format.Alignment = (k % 2) == 0 ? ParagraphAlignment.Right : ParagraphAlignment.Left;
                      }
                      string[] percurso = itAtendimentos.GetQuery(condition, q => q.OrderBy(e => e.Id))
                                              .Select(p => p.Percurso).ToArray();

                      int index = percurso.Length / 2 + percurso.Length % 2;
                      for (int i = 0; i < (percurso.Length / 2 + percurso.Length % 2); i++) {
                        row = table.AddRow();
                        row.Height = "0.55 cm";

                        row.Cells[0].AddParagraph($"{i + 1}.");
                        row.Cells[1].AddParagraph(percurso[i]);
                        if (index < percurso.Length) {
                          row.Cells[2].AddParagraph($"{index + 1}.");
                          row.Cells[3].AddParagraph(percurso[index++]);
                        }
                      }
                    }
                  }
                }
              }
              else {
                paragraph = document.LastSection.AddParagraph();
                paragraph.Style = "Normal";
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.SpaceAfter = "0.25 cm";

                if (!pItem.AtendimentoId.HasValue) {
                  paragraph.AddFormattedText($"{Resources.ItinerarioViewModel} {pItem.Prefixo} - {pItem.Linha.Denominacao}", TextFormat.Bold);

                  using Services<Itinerario> itinerarios = new Services<Itinerario>();
                  if (itinerarios.Exists(q => (q.LinhaId == pItem.LinhaId) &&
                                               q.Sentido.Equals(pItem.Sentido))) {
                    paragraph = document.LastSection.AddParagraph();
                    paragraph.Format.SpaceBefore = "0.4 cm";
                    paragraph.Format.SpaceAfter = "0.2 in";
                    paragraph.AddFormattedText($"{Resources.Sentido}: {Sentido.Items[pItem.Sentido]}", TextFormat.Bold);

                    AddTable(this.section);
                    colSize = new Unit[4] { "1 cm", "8 cm", "1 cm", "8 cm" };
                    for (int k = 0; k < colSize.Length; k++) {
                      column = table.AddColumn(colSize[k]);
                      column.Format.Alignment = (k % 2) == 0 ? ParagraphAlignment.Right : ParagraphAlignment.Left;
                    }
                    string[] percurso = itinerarios.GetQuery(q => (q.LinhaId == pItem.LinhaId) &&
                                                                   q.Sentido.Equals(pItem.Sentido),
                                                             q => q.OrderBy(e => e.Id))
                                            .Select(p => p.Percurso).ToArray();

                    int index = percurso.Length / 2 + percurso.Length % 2;
                    for (int i = 0; i < (percurso.Length / 2 + percurso.Length % 2); i++) {
                      row = table.AddRow();
                      row.Height = "0.55 cm";

                      row.Cells[0].AddParagraph($"{i + 1}.");
                      row.Cells[1].AddParagraph(percurso[i]);
                      if (index < percurso.Length) {
                        row.Cells[2].AddParagraph($"{index + 1}.");
                        row.Cells[3].AddParagraph(percurso[index++]);
                      }
                    }
                  }
                }
                else {
                  paragraph.AddFormattedText($"{Resources.ItAtendimentoViewModel} {pItem.Prefixo} - {pItem.Atendimento.Denominacao}", TextFormat.Bold);

                  using Services<ItAtendimento> itAtendimentos = new Services<ItAtendimento>();
                  if (itAtendimentos.Exists(q => (q.AtendimentoId == pItem.AtendimentoId) &&
                                                  q.Sentido.Equals(pItem.Sentido))) {
                    paragraph = document.LastSection.AddParagraph();
                    paragraph.Format.SpaceBefore = "0.4 cm";
                    paragraph.Format.SpaceAfter = "0.2 in";
                    paragraph.AddFormattedText($"{Resources.Sentido}: {Sentido.Items[pItem.Sentido]}", TextFormat.Bold);

                    AddTable(this.section);
                    colSize = new Unit[4] { "1 cm", "8 cm", "1 cm", "8 cm" };
                    for (int k = 0; k < colSize.Length; k++) {
                      column = table.AddColumn(colSize[k]);
                      column.Format.Alignment = (k % 2) == 0 ? ParagraphAlignment.Right : ParagraphAlignment.Left;
                    }
                    string[] percurso = itAtendimentos.GetQuery(q => (q.AtendimentoId == pItem.AtendimentoId) &&
                                                                      q.Sentido.Equals(pItem.Sentido),
                                                                q => q.OrderBy(e => e.Id))
                                            .Select(p => p.Percurso).ToArray();

                    int index = percurso.Length / 2 + percurso.Length % 2;
                    for (int i = 0; i < (percurso.Length / 2 + percurso.Length % 2); i++) {
                      row = table.AddRow();
                      row.Height = "0.55 cm";

                      row.Cells[0].AddParagraph($"{i + 1}.");
                      row.Cells[1].AddParagraph(percurso[i]);
                      if (index < percurso.Length) {
                        row.Cells[2].AddParagraph($"{index + 1}.");
                        row.Cells[3].AddParagraph(percurso[index++]);
                      }
                    }
                  }
                }
              }
            }
          }

          /*
           * Demanda Mensal e Anual da Linha
           */
          using TCategoriaService categorias = new TCategoriaService();
          using DemandaMesService demanda = new DemandaMesService();
          concat = new StringBuilder($"{Resources.LinhaId}: {item.Prefixo} - {item.Denominacao}");

          // Demanda Mensal
          if (demanda.Exists(d => d.LinhaId == item.Id)) {
            AddSection(Orientation.Landscape);
            Footer(this.section, concat.ToString());
            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.Format.SpaceAfter = "0.25 cm";
            paragraph.AddFormattedText(Resources.DemandaMesViewModel, TextFormat.Bold);
            AddTable(this.section);

            colSize = new Unit[categorias.GetCount(t => t.EmpresaId == item.EmpresaId) + 6];
            for (int k = 0; k < colSize.Length; k++) {
              colSize[k].Centimeter = 26.8 / (categorias.GetCount(t => t.EmpresaId == item.EmpresaId) + 6);
              column = table.AddColumn(colSize[k]);
            }

            row = table.AddRow();
            row.Height = "1.6 cm";
            row.Format.Alignment = ParagraphAlignment.Center;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Format.Font.Bold = true;

            int j = 0;
            row.Cells[j].AddParagraph(Resources.DataReferencia);
            foreach (TCategoria modal in categorias.GetQuery(t => t.EmpresaId == item.EmpresaId)) {
              row.Cells[++j].AddParagraph(modal.Denominacao);
            }
            row.Cells[++j].AddParagraph(Resources.Total);

            row.Cells[++j].MergeRight = 1;
            row.Cells[j++].AddParagraph(Resources.Equivalencia);

            row.Cells[++j].AddParagraph(Resources.IPK);
            row.Cells[++j].AddParagraph(Resources.IPKe);

            var records = demanda.GetQuery(d => d.LinhaId == item.Id)
                              .Select(q => new { q.Ano, q.Mes }).Distinct()
                              .OrderBy(q => q.Ano).ThenBy(q => q.Mes);
            Expression<Func<DemandaMes, bool>> whereAs;

            decimal[] total = new decimal[2] { 0, 0 };
            foreach (var dItem in records) {
              row = table.AddRow();
              row.Height = "0.55 cm";
              row.Format.Alignment = ParagraphAlignment.Right;

              j = 0;
              row.Cells[j].AddParagraph($"{Mes.Short[dItem.Mes]}/{dItem.Ano}");
              row.Cells[j].Format.Alignment = ParagraphAlignment.Left;

              total = new decimal[2] { 0, 0 };
              foreach (TCategoria modal in categorias.GetQuery(t => t.EmpresaId == item.EmpresaId)) {
                whereAs = q => (q.LinhaId == item.Id) && (q.Ano == dItem.Ano) &&
                               (q.Mes == dItem.Mes) && (q.Categoria == modal.Id);
                int?[] values = new int?[2] {
                        demanda.GetFirst(whereAs)?.Passageiros,
                        demanda.GetFirst(whereAs)?.Equivalente
                };
                total[0] += values[0] ?? 0;
                total[1] += values[1] ?? 0;

                row.Cells[++j].AddParagraph($"{values[0]:#,##0}");
              }
              row.Cells[++j].AddParagraph($"{total[0]:#,##0}");
              row.Cells[++j].AddParagraph($"{total[1]:#,##0}");
              row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[1], total[0]):P1}");
              row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[0], (decimal)pmmKm[1]):0.000}");
              row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[1], (decimal)pmmKm[1]):0.000}");
            }

            // Totais Mensais
            if (records.Count() > 1) {
              row = table.AddRow();
              row.Height = "0.8 cm";
              row.Format.Alignment = ParagraphAlignment.Right;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              j = 0;
              total = new decimal[2] { 0, 0 };
              foreach (TCategoria modal in categorias.GetQuery(t => t.EmpresaId == item.EmpresaId)) {
                whereAs = q => (q.LinhaId == item.Id) && (q.Categoria == modal.Id);
                int?[] values = new int?[2] {
                        demanda.GetQuery(whereAs)?.Sum(p => p.Passageiros),
                        demanda.GetQuery(whereAs)?.Sum(p => p.Equivalente)
                };
                total[0] += values[0] ?? 0;
                total[1] += values[1] ?? 0;

                row.Cells[++j].AddParagraph($"{values[0]:#,##0}");
              }
              row.Cells[++j].AddParagraph($"{total[0]:#,##0}");
              row.Cells[++j].AddParagraph($"{total[1]:#,##0}");
              row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[1], total[0]):P1}");

              // Medias Mensais
              row = table.AddRow();
              row.Height = "0.6 cm";
              row.Format.Alignment = ParagraphAlignment.Right;
              row.Format.Font.Bold = true;

              j = 0;
              foreach (TCategoria modal in categorias.GetQuery(t => t.EmpresaId == item.EmpresaId)) {
                whereAs = q => (q.LinhaId == item.Id) && (q.Categoria == modal.Id);

                row.Cells[++j].AddParagraph($"{(decimal?)demanda.GetQuery(whereAs)?.Average(p => p.Passageiros):#,##0}");
              }

              int months = demanda.GetQuery(q => q.LinhaId == item.Id).Select(p => new { p.Ano, p.Mes }).Distinct().Count();
              total = new decimal[2] {
                    Math.Round(NumericExtensions.SafeDivision(
                                   (decimal)(demanda.GetQuery(q => q.LinhaId == item.Id)?.Sum(p => p.Passageiros) ?? 0), months), 0),
                    Math.Round(NumericExtensions.SafeDivision(
                                   (decimal)(demanda.GetQuery(q => q.LinhaId == item.Id)?.Sum(p => p.Equivalente) ?? 0), months), 0)
              };

              row.Cells[++j].AddParagraph($"{total[0]:#,##0}");
              row.Cells[++j].AddParagraph($"{total[1]:#,##0}");
              row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[1], total[0]):P1}");
              row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[0], (decimal)pmmKm[1]):0.000}");
              row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[1], (decimal)pmmKm[1]):0.000}");
            }
          }

          /* // Demanda Anual
          using (DemandaAnoService demanda = new DemandaAnoService()) {
            if (demanda.Exists(d => d.LinhaId == item.Id)) {
              int[] records = demanda.GetQuery(
                                  d => d.LinhaId == item.Id
                              ).Select(q => q.Ano).Distinct().ToArray();
              Expression<Func<DemandaAno, bool>> whereAs;

              section.AddPageBreak();
              paragraph = document.LastSection.AddParagraph();
              paragraph.Format.Alignment = ParagraphAlignment.Left;
              paragraph.Format.SpaceAfter = "0.25 cm";
              paragraph.AddFormattedText(Resources.DemandaAnoViewModel, TextFormat.Bold);
              AddTable(this.section);

              colSize = new Unit[categorias.GetCount(t => t.EmpresaId == item.EmpresaId) + 6];
              for (int k = 0; k < colSize.Length; k++) {
                colSize[k].Centimeter = 26.8 / (categorias.GetCount(t => t.EmpresaId == item.EmpresaId) + 6);
                column = table.AddColumn(colSize[k]);
              }

              row = table.AddRow();
              row.Height = "1.6 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              int j = 0;
              row.Cells[j].AddParagraph(Resources.DataReferencia);
              foreach (TCategoria modal in categorias.GetQuery(t => t.EmpresaId == item.EmpresaId)) {
                row.Cells[++j].AddParagraph(modal.Denominacao);
              }
              row.Cells[++j].AddParagraph(Resources.Total);

              row.Cells[++j].MergeRight = 1;
              row.Cells[j++].AddParagraph(Resources.Equivalencia);

              row.Cells[++j].AddParagraph(Resources.IPK);
              row.Cells[++j].AddParagraph(Resources.IPKe);

              decimal[] total = new decimal[2] { 0, 0 };
              foreach (int ano in records) {
                row = table.AddRow();
                row.Height = "0.55 cm";
                row.Format.Alignment = ParagraphAlignment.Right;

                j = 0;
                row.Cells[j].AddParagraph(ano.ToString());
                row.Cells[j].Format.Alignment = ParagraphAlignment.Center;

                total = new decimal[2] { 0, 0 };
                foreach (TCategoria modal in categorias.GetQuery(t => t.EmpresaId == item.EmpresaId)) {
                  whereAs = q => (q.LinhaId == item.Id) && (q.Ano == ano) && (q.Categoria == modal.Id);
                  int?[] values = new int?[2] {
                        demanda.GetFirst(whereAs)?.Passageiros,
                        demanda.GetFirst(whereAs)?.Equivalente
                  };
                  total[0] += values[0] ?? 0;
                  total[1] += values[1] ?? 0;

                  row.Cells[++j].AddParagraph($"{values[0]:#,##0}");
                }
                row.Cells[++j].AddParagraph($"{total[0]:#,##0}");
                row.Cells[++j].AddParagraph($"{total[1]:#,##0}");
                row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[1], total[0]):P1}");
                row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[0], (decimal)pmmKm[0]):0.0000}");
                row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[1], (decimal)pmmKm[0]):0.0000}");
              }

              // Totais Anuais
              if (records.Length > 1) {
                row = table.AddRow();
                row.Height = "0.8 cm";
                row.Format.Alignment = ParagraphAlignment.Right;
                row.VerticalAlignment = VerticalAlignment.Center;
                row.Format.Font.Bold = true;

                j = 0;
                total = new decimal[2] { 0, 0 };
                foreach (TCategoria modal in categorias.GetQuery(t => t.EmpresaId == item.EmpresaId)) {
                  whereAs = q => (q.LinhaId == item.Id) && (q.Categoria == modal.Id);
                  int?[] values = new int?[2] {
                        demanda.GetQuery(whereAs)?.Sum(p => p.Passageiros),
                        demanda.GetQuery(whereAs)?.Sum(p => p.Equivalente)
                  };
                  total[0] += values[0] ?? 0;
                  total[1] += values[1] ?? 0;

                  row.Cells[++j].AddParagraph($"{values[0]:#,##0}");
                }
                row.Cells[++j].AddParagraph($"{total[0]:#,##0}");
                row.Cells[++j].AddParagraph($"{total[1]:#,##0}");
                row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[1], total[0]):P1}");

                // Medias Anuais
                row = table.AddRow();
                row.Height = "0.6 cm";
                row.Format.Alignment = ParagraphAlignment.Right;
                row.Format.Font.Bold = true;

                j = 0;
                foreach (TCategoria modal in categorias.GetQuery(t => t.EmpresaId == item.EmpresaId)) {
                  decimal? aux = (decimal?)demanda.GetQuery(
                                             q => (q.LinhaId == item.Id) && (q.Categoria == modal.Id)
                                           )?.Average(p => p.Passageiros);

                  row.Cells[++j].AddParagraph($"{aux:#,##0}");
                }

                using Services<DemandaMod> totais = new Services<DemandaMod>();
                total = new decimal[2] {
                    Math.Round(NumericExtensions.SafeDivision(
                                   (decimal)(totais.GetQuery(q => q.LinhaId == item.Id)?.Sum(p => p.Passageiros) ?? 0),
                                   CustomCalendar.MonthsPerYear), 0),
                    Math.Round(NumericExtensions.SafeDivision(
                                   (decimal)(totais.GetQuery(q => q.LinhaId == item.Id)?.Sum(p => p.Equivalente) ?? 0),
                                   CustomCalendar.MonthsPerYear), 0)
                };

                row.Cells[++j].AddParagraph($"{total[0]:#,##0}");
                row.Cells[++j].AddParagraph($"{total[1]:#,##0}");
                row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[1], total[0]):P1}");
                row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[0], (decimal)pmmKm[0]):0.0000}");
                row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[1], (decimal)pmmKm[0]):0.0000}");
              }
            }
          } */
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
        logo.Height = "0.5 in";
        logo.Width = "0.625 in";
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
  }
}
