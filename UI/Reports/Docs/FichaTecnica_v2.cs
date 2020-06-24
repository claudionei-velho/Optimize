using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;

using Bll;
using Bll.Lists;
using Bll.Services;

using Dto.Extensions;
using Dto.Models;

using Reports;
using UI.Properties;

namespace UI.Reports.Docs {
  public class FichaTecnica_v2 : ReportBase {
    private readonly Expression<Func<Linha, bool>> filter;

    public FichaTecnica_v2(Expression<Func<Linha, bool>> _filter) : base(Resources.FichaTecnicaPreview) {
      this.filter = _filter;
    }

    public Document CreateDocument() {
      char[] charsToTrim = new char[] { ' ', ';', '/' };
      Sentido sentido = new Sentido();
      Workday workDay = new Workday();
      Dictionary<int, int> listAtt = new Dictionary<int, int>();
      StringBuilder concat = new StringBuilder();

      DefineStyles();

      /*
       * Resumo Executivo do Sistema
       */
      using (LinhaService linhas = new LinhaService()) {
        decimal?[,] values = new decimal?[3, 4] { { 0, 0, 0, 0 },
                                                  { 0, 0, 0, 0 },
                                                  { 0, 0, 0, 0 } };

        foreach (var groupItem in linhas.GetQuery(this.filter).Select(
                                      p => new { p.EmpresaId, p.Classificacao }).Distinct()) {
          using Services<Empresa> empresa = new Services<Empresa>();
          string companyName = empresa.GetById(groupItem.EmpresaId)?.Razao;

          using CLinhaService cLinha = new CLinhaService();
          string catName = cLinha.GetById(groupItem.Classificacao)?.ClassLinha.Denominacao;

          AddSection(Orientation.Landscape);
          Header(this.section, new List<string>() { companyName, document.Info.Title });
          Footer(this.section);
          AddTable(this.section);

          Unit[] colSize = new Unit[7] { "0.8 cm", "7.5 cm", "2 cm", "9 cm", "2.5 cm", "2.5 cm", "2.5 cm" }; 
          /* Unit[] colSize = new Unit[10];
          colSize[0] = "0.8 cm";
          colSize[1] = "8 cm";
          for (int k = 2; k < colSize.Length; k++) {
            colSize[k] = "2.25 cm";
          } */

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

          row.Cells[2].AddParagraph(Resources.Prefixo);
          row.Cells[3].AddParagraph(Resources.Viagem);
          row.Cells[4].AddParagraph(Resources.Extensao);
          row.Cells[5].AddParagraph($"{Resources.Viagens} {Resources.Mensal}");
          row.Cells[6].AddParagraph($"{Resources.Percurso} {Resources.Mensal}");
/*          row.Cells[5].AddParagraph(Resources.Passageiros);

          row.Cells[6].MergeRight = 1;
          row.Cells[6].AddParagraph(Resources.Equivalencia);

          row.Cells[8].AddParagraph(Resources.IPK);
          row.Cells[9].AddParagraph(Resources.IPKe);
*/      
          row = table.AddRow();
          row.Height = "0.8 cm";

          row.Cells[0].MergeRight = 1;
          row.Cells[0].AddParagraph(catName);
          row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
          row.Cells[0].Format.Font.Bold = true;

          string linx = string.Empty;
          foreach (Linha item in linhas.GetQuery(Predicate.And(
                                                     this.filter, 
                                                     p => p.Classificacao == groupItem.Classificacao))
                                                 .OrderBy(p => p.Prefixo)) { 
            row = table.AddRow();
            row.Height = "0.8 cm";

            if (linx != item.Denominacao) {
              row.Cells[1].AddParagraph(item.Denominacao);
              row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
              row.Cells[1].Format.Font.Bold = true;
            }
            row.Cells[2].AddParagraph(item.Prefixo);
            row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

            if (item.Viagem != null) {
              row.Cells[3].AddParagraph(item.Viagem);
              row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            }

            // Total de Viagens e Percurso Medio Mensal            
            using Services<ViagemLinha> viagens = new Services<ViagemLinha>();
            values[0, 0] = viagens.GetQuery(q => q.LinhaId == item.Id).Sum(p => p.ViagensAno);
            values[0, 1] = viagens.GetQuery(q => q.LinhaId == item.Id).Sum(p => p.PercursoAno);

            try {
              row.Cells[4].AddParagraph($"{values[0, 1] / values[0, 0]:#,##0.00}");
            }
            catch (DivideByZeroException) {
              row.Cells[4].AddParagraph();
            }
            row.Cells[5].AddParagraph($"{NumericExtensions.SafeDivision(values[0, 0] ?? 0, CustomCalendar.MonthsPerYear):#,#}");
            row.Cells[6].AddParagraph($"{NumericExtensions.SafeDivision(values[0, 1] ?? 0, CustomCalendar.MonthsPerYear):#,##0.0}");

            // Demanda Mensal da Linha
/*            using Services<DemandaMod> demanda = new Services<DemandaMod>();
            Expression<Func<DemandaMod, bool>> filter = q => q.LinhaId == item.Id;
            values[0, 2] = (decimal)(demanda.GetQuery(filter)?.Average(p => p.Passageiros) ?? 0);
            values[0, 3] = (decimal)(demanda.GetQuery(filter)?.Average(p => p.Equivalente) ?? 0);

            row.Cells[5].AddParagraph($"{values[0, 2]:#,#}");
            row.Cells[6].AddParagraph($"{values[0, 3]:#,#}");
            try {
              row.Cells[7].AddParagraph($"{(decimal)values[0, 3] / values[0, 2]:P1}");
            }
            catch (DivideByZeroException) {
              row.Cells[7].AddParagraph();
            }

            try {
              row.Cells[8].AddParagraph($"{values[0, 2] / values[0, 1]:0.0000}");
            }
            catch (DivideByZeroException) {
              row.Cells[8].AddParagraph();
            }

            try {
              row.Cells[9].AddParagraph($"{values[0, 3] / values[0, 1]:0.0000}");
            }
            catch (DivideByZeroException) {
              row.Cells[9].AddParagraph();
            } */

            for (int j = 0; j < values.GetLength(1); j++) {
              values[1, j] += values[0, j] ?? 0;
            }
            linx = item.Denominacao;
          }

          // Subtotais do Grupo
          if (linhas.GetQuery(this.filter).Where(p => p.Classificacao == groupItem.Classificacao).Count() > 1) {
            row = table.AddRow();
            row.Height = "0.8 cm";
            row.Format.Font.Bold = true;

            row.Cells[3].AddParagraph($"{Resources.Subtotal}: {catName}");
            try {
              row.Cells[4].AddParagraph($"{values[1, 1] / values[1, 0]:#,##0.00}");
            }
            catch (DivideByZeroException) {
              row.Cells[4].AddParagraph();
            }
            row.Cells[5].AddParagraph($"{NumericExtensions.SafeDivision(values[1, 0] ?? 0, CustomCalendar.MonthsPerYear):#,#}");
            row.Cells[6].AddParagraph($"{NumericExtensions.SafeDivision(values[1, 1] ?? 0, CustomCalendar.MonthsPerYear):#,##0.0}");
            /* row.Cells[5].AddParagraph($"{values[1, 2]:#,#}");
            row.Cells[6].AddParagraph($"{values[1, 3]:#,#}");
            try {
              row.Cells[7].AddParagraph($"{(decimal)values[1, 3] / values[1, 2]:P1}");
            }
            catch (DivideByZeroException) {
              row.Cells[7].AddParagraph();
            }

            try {
              row.Cells[8].AddParagraph($"{values[1, 2] / values[1, 1]:0.0000}");
            }
            catch (DivideByZeroException) {
              row.Cells[8].AddParagraph();
            }

            try {
              row.Cells[9].AddParagraph($"{values[1, 3] / values[1, 1]:0.0000}");
            }
            catch (DivideByZeroException) {
              row.Cells[9].AddParagraph();
            } */          
          }
          for (int j = 0; j < values.GetLength(1); j++) {
            values[2, j] += values[1, j] ?? 0;
            values[1, j] = 0;
          }
        }

        // Totais Gerais
        if (linhas.GetQuery(this.filter).Select(
                p => new { p.EmpresaId, p.Classificacao }).Distinct().Count() > 1) {
          Row row = table.AddRow();
          row.Height = "0.8 cm";
          row.Format.Font.Bold = true;

          row.Cells[3].AddParagraph(Resources.GranTotal);
          try {
            row.Cells[4].AddParagraph($"{values[2, 1] / values[2, 0]:#,##0.00}");
          }
          catch (DivideByZeroException) {
            row.Cells[4].AddParagraph();
          }
          row.Cells[5].AddParagraph($"{NumericExtensions.SafeDivision(values[2, 0] ?? 0, CustomCalendar.MonthsPerYear):#,#}");
          row.Cells[6].AddParagraph($"{NumericExtensions.SafeDivision(values[2, 1] ?? 0, CustomCalendar.MonthsPerYear):#,##0.0}");
          /* row.Cells[5].AddParagraph($"{values[2, 2]:#,#}");
          row.Cells[6].AddParagraph($"{values[2, 3]:#,#}");
          try {
            row.Cells[7].AddParagraph($"{(decimal)values[2, 3] / values[2, 2]:P1}");
          }
          catch (DivideByZeroException) {
            row.Cells[7].AddParagraph();
          }

          try {
            row.Cells[8].AddParagraph($"{values[2, 2] / values[2, 1]:0.0000}");
          }
          catch (DivideByZeroException) {
            row.Cells[8].AddParagraph();
          }

          try {
            row.Cells[9].AddParagraph($"{values[2, 3] / values[2, 1]:0.0000}");
          }
          catch (DivideByZeroException) {
            row.Cells[9].AddParagraph();
          } */
        }
      }

      /*
       * Identificacao e Qualificacao da Linha
       */
      using (LinhaService linhas = new LinhaService()) {
        foreach (Linha item in linhas.GetQuery(this.filter, q => q.OrderBy(p => p.Prefixo))) {
          concat = new StringBuilder($"{Resources.LinhaId}: {item.Prefixo} - {item.Viagem}");

          AddSection(Orientation.Portrait);
          Header(this.section, new List<string>() { item.Empresa.Razao, document.Info.Title });
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

          row = table.AddRow();
          row.Height = "0.6 cm";
          row.Format.Font.Bold = true;

          row.Cells[0].AddParagraph(Resources.Viagem);

          if (item.Viagem != null) {
            row.Cells[1].MergeRight = 2;
            row.Cells[1].AddParagraph(item.Viagem);
            row.Cells[1].Format.Font.Bold = false;
          }

          // Dias de Operacao e Funcoes da Linha
          row = table.AddRow();
          row.Height = "0.8 cm";
          row.Format.Font.Bold = true;

          row.Cells[0].AddParagraph(Resources.DiaId);
          row.Cells[1].AddParagraph(item.DiasOp);
          row.Cells[1].Format.Font.Bold = false;

          row.Cells[2].AddParagraph(Resources.Funcao);
          row.Cells[3].AddParagraph(item.Funcoes);
          row.Cells[3].Format.Font.Bold = false;

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
          /* for (int j = 0; j < sentido.Data.Count; j++) {            
            row = table.AddRow();
            row.Height = "0.8 cm";
            row.Format.Font.Bold = true;
            if (j == 0) {
              row.Cells[0].AddParagraph(Resources.PontoInicialAB);
              row.Cells[2].AddParagraph(Resources.PontoFinalAB);
            }
            else {
              row.Cells[0].AddParagraph(Resources.PontoInicialBA);
              row.Cells[2].AddParagraph(Resources.PontoFinalBA);
            }
            string ab = (j == 0) ? "AB" : "BA";

            row.Cells[1].AddParagraph(linhas.GetPontoInicial(item.Id, ab));
            row.Cells[1].Format.Font.Bold = false;

            row.Cells[3].AddParagraph(linhas.GetPontoFinal(item.Id, ab));
            row.Cells[3].Format.Font.Bold = false;
          } */

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
           *//*
          Paragraph paragraph = document.LastSection.AddParagraph();
          paragraph.Format.Alignment = ParagraphAlignment.Left;
          paragraph.Format.SpaceBefore = "0.4 cm";
          paragraph.AddFormattedText(Resources.AtendimentoViewModel, TextFormat.Bold);

          using (Services<Atendimento> atendimentos = new Services<Atendimento>()) {
            if (atendimentos.Exists(q => q.LinhaId == item.Id)) {
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
          } */

          /*
           * Quadro de Viagens/Hora da Linha
           *//*
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
              row.Cells[1].AddParagraph(workDay.Data[1]);

              row.Cells[4].MergeRight = 2;
              row.Cells[4].AddParagraph(workDay.Data[2]);

              row.Cells[7].MergeRight = 2;
              row.Cells[7].AddParagraph(workDay.Data[3]);

              row = table.AddRow();
              row.Height = "0.8 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              int[,] cols = new int[,] { { 1, 2 }, { 4, 5 }, { 7, 8 } };
              for (int j = 0; j <= sentido.Data.Count; j++) {
                for (int k = 0; k < sentido.Data.Count; k++) {
                  if ((k % 2) == 0) {
                    row.Cells[cols[j, k]].AddParagraph(sentido.Data["AB"]);
                  }
                  else {
                    row.Cells[cols[j, k]].AddParagraph(sentido.Data["BA"]);
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
          } */

          /*
           * Horarios da Linha
           */
          Paragraph paragraph = document.LastSection.AddParagraph();
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
            for (int i = 0; i < sentido.Data.Count; i++) {
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

              concat = new StringBuilder($"{workDay.Data[hr]} ({sentido.Data["AB"]})");
              row.Cells[0].MergeRight = 5;
              row.Cells[0].AddParagraph(concat.ToString());

              concat = new StringBuilder($"{workDay.Data[hr]} ({sentido.Data["BA"]})");
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

                for (int j = 0; j < sentido.Data.Count; j++) {
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
           *//*
          paragraph = document.LastSection.AddParagraph();
          paragraph.Format.Alignment = ParagraphAlignment.Left;
          paragraph.Format.SpaceBefore = "0.4 cm";
          paragraph.Format.SpaceAfter = "0.25 cm";
          paragraph.AddFormattedText(Resources.PrLinhaViewModel, TextFormat.Bold);

          using (PeriodoTipicoService pTipicos = new PeriodoTipicoService()) {
            if (pTipicos.Exists(q => q.LinhaId == item.Id)) {
              AddTable(this.section);

              colSize = new Unit[7] { "3 cm", "3 cm", "2.2 cm", "2.4 cm", "1.8 cm", "3.5 cm", "2.1 cm" };
              for (int k = 0; k < colSize.Length; k++) {
                column = table.AddColumn(colSize[k]);
              }

              row = table.AddRow();
              row.Height = "1.2 cm";
              row.Format.Alignment = ParagraphAlignment.Center;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              row.Cells[0].AddParagraph(Resources.DiaId);
              row.Cells[1].AddParagraph(Resources.PeriodoId);
              row.Cells[2].AddParagraph(Resources.InicioPeriodo);
              row.Cells[3].AddParagraph(Resources.DuracaoPeriodo);
              row.Cells[4].AddParagraph(Resources.Viagens);
              row.Cells[5].AddParagraph(Resources.Ciclo);
              row.Cells[6].AddParagraph(Resources.Veiculos);

              int l = 0;
              foreach (int hr in tabelas) {
                int j = 0;
                int[] total = { 0, 0, 0 };
                foreach (PeriodoTipico pItem in pTipicos.GetQuery(
                                                    q => (q.LinhaId == item.Id) && (q.DiaId == hr),
                                                    q => q.OrderBy(e => e.PeriodoId))) {
                  row = table.AddRow();
                  row.Height = "0.55 cm";
                  row.Format.Alignment = ParagraphAlignment.Center;

                  if (j++ == 0) {
                    row.Cells[0].AddParagraph(workDay.Data[pItem.DiaId]);
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                  }

                  row.Cells[1].AddParagraph(pItem.EPeriodo.Denominacao);
                  row.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                  row.Cells[2].AddParagraph($@"{pItem.Inicio:hh\:mm}");

                  concat = new StringBuilder($"{$"{pItem.Duracao:#,##0}"} ({$"{pItem.Duracao / 60:#0}"}:{$"{pItem.Duracao % 60:00}"})");
                  row.Cells[3].AddParagraph(concat.ToString());
                  row.Cells[3].Format.Alignment = ParagraphAlignment.Right;

                  row.Cells[4].AddParagraph($"{pItem.QtdViagens:#,##0}");

                  concat = new StringBuilder($"{$"{pItem.Ciclo / 60:#0}"}:{$"{pItem.Ciclo % 60:00}"} ({$"{pItem.CicloAB:#,#}"} + {$"{pItem.CicloBA:#,#}"})");
                  row.Cells[5].AddParagraph(concat.ToString());
                  row.Cells[6].AddParagraph($"{pItem.MaxVeiculos:#,##0}");

                  total[0] += pItem.Duracao;
                  total[1] += pItem.QtdViagens;
                  if (pItem.MaxVeiculos.HasValue && (total[2] < pItem.MaxVeiculos.Value)) {
                    total[2] = pItem.MaxVeiculos.Value;
                  }
                }
                row = table.AddRow();
                row.Height = "0.25 in";
                row.Format.Alignment = ParagraphAlignment.Left;
                row.VerticalAlignment = VerticalAlignment.Center;
                row.Format.Font.Bold = true;

                concat = new StringBuilder($"({$"{total[0] / 60:#0}"}:{$"{total[0] % 60:00}"})");
                row.Cells[3].AddParagraph(concat.ToString());
                row.Cells[3].Format.Alignment = ParagraphAlignment.Right;

                row.Cells[4].AddParagraph(total[1].ToString());
                row.Cells[4].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[6].AddParagraph($"{total[2]:#,###}");
                row.Cells[6].Format.Alignment = ParagraphAlignment.Center;

                if (++l < tabelas.Length) {
                  row = table.AddRow();
                }
              }
            }
          } */

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
              row.Cells[4].AddParagraph(workDay.Data[1]);

              row.Cells[6].MergeRight = 1;
              row.Cells[6].AddParagraph(workDay.Data[2]);

              row.Cells[8].MergeRight = 1;
              row.Cells[8].AddParagraph(workDay.Data[3]);

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

                row.Cells[2].AddParagraph(sentido.Data[opItem.Sentido]);
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
              row.Cells[4].AddParagraph(workDay.Data[1]);

              row.Cells[6].MergeRight = 1;
              row.Cells[6].AddParagraph(workDay.Data[2]);

              row.Cells[8].MergeRight = 1;
              row.Cells[8].AddParagraph(workDay.Data[3]);

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

                row.Cells[2].AddParagraph(sentido.Data[opItem.Sentido]);
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

                row.Cells[2].AddParagraph(workDay.Data[qItem.DiaId]);
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
                row.Cells[3].AddParagraph($"{vgTotal.Value / CustomCalendar.WeeksPerYear:#,###}");
                row.Cells[4].AddParagraph($"{kmTotal.Value / CustomCalendar.WeeksPerYear:#,##0.0}");
              }
              catch (DivideByZeroException) {
                row.Cells[3].MergeRight = 1;
                row.Cells[3].AddParagraph();
              }
              try {
                row.Cells[5].AddParagraph($"{vgTotal.Value / CustomCalendar.MonthsPerYear:#,###}");
                row.Cells[6].AddParagraph($"{kmTotal.Value / CustomCalendar.MonthsPerYear:#,##0.0}");
              }
              catch (DivideByZeroException) {
                row.Cells[5].MergeRight = 1;
                row.Cells[5].AddParagraph();
              }
              row.Cells[7].AddParagraph($"{vgTotal.Value:#,###}");
              row.Cells[8].AddParagraph($"{kmTotal.Value:#,##0.0}");

              concat = new StringBuilder();
              using (Services<DiaTrabalho> workDays = new Services<DiaTrabalho>()) {
                foreach (int hr in tabelas) {
                  concat.Append($"{workDay.Data[hr]} = {$"{workDays.GetById(hr).Dias:#,##0}"}; ");
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
                      concat = new StringBuilder($"{item.Prefixo} ({sentido.Data[mapa.Sentido]})");
                      paragraph.AddFormattedText(concat.ToString(), TextFormat.Bold);
                      paragraph.Format.Alignment = ParagraphAlignment.Center;
                    }
                    else {
                      using AtendimentoService pontos = new AtendimentoService();
                      concat = new StringBuilder($"{mapa.Atendimento.Prefixo} ({sentido.Data[mapa.Sentido]})");
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
                    paragraph.AddFormattedText($"{Resources.Sentido}: {sentido.Data[pItem.Sentido]}", TextFormat.Bold);

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
                    paragraph.AddFormattedText($"{Resources.Sentido}: {sentido.Data[pItem.Sentido]}", TextFormat.Bold);

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
          using (DemandaMesService demanda = new DemandaMesService()) {
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
                row.Cells[j].AddParagraph($"{new Mes().Short[dItem.Mes]}/{dItem.Ano}");
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
                row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[0], (decimal)pmmKm[1]):0.0000}");
                row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[1], (decimal)pmmKm[1]):0.0000}");
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
                total = new decimal[2] { 0, 0 };
                foreach (TCategoria modal in categorias.GetQuery(t => t.EmpresaId == item.EmpresaId)) {
                  whereAs = q => (q.LinhaId == item.Id) && (q.Categoria == modal.Id);
                  decimal?[] values = new decimal?[2] {
                        (decimal?)demanda.GetQuery(whereAs)?.Average(p => p.Passageiros),
                        (decimal?)demanda.GetQuery(whereAs)?.Average(p => p.Equivalente)
                  };
                  total[0] += values[0] ?? 0;
                  total[1] += values[1] ?? 0;

                  row.Cells[++j].AddParagraph($"{values[0]:#,##0}");
                }
                row.Cells[++j].AddParagraph($"{total[0]:#,##0}");
                row.Cells[++j].AddParagraph($"{total[1]:#,##0}");
                row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[1], total[0]):P1}");
                row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[0], (decimal)pmmKm[1]):0.0000}");
                row.Cells[++j].AddParagraph($"{NumericExtensions.SafeDivision(total[1], (decimal)pmmKm[1]):0.0000}");
              }
            }
          }

          // Demanda Anual
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
                total = new decimal[2] { 0, 0 };
                foreach (TCategoria modal in categorias.GetQuery(t => t.EmpresaId == item.EmpresaId)) {
                  whereAs = q => (q.LinhaId == item.Id) && (q.Categoria == modal.Id);
                  decimal?[] values = new decimal?[2] {
                        (decimal?)demanda.GetQuery(whereAs)?.Average(p => p.Passageiros),
                        (decimal?)demanda.GetQuery(whereAs)?.Average(p => p.Equivalente)
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
            }
          }

        }
      }
      return this.document;
    }
  }
}
