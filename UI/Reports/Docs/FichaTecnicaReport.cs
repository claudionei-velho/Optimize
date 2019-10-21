﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Shapes;

using Bll;
using Bll.Lists;
using Bll.Services;
using Dto.Models;
using Reports;
using UI.Properties;

namespace UI.Reports.Docs {
  public class FichaTecnicaReport : ReportBase {
    private readonly Expression<Func<Linha, bool>> filter;

    public FichaTecnicaReport(Expression<Func<Linha, bool>> _filter) : base(Resources.FichaTecnicaPreview) {
      this.filter = _filter;
    }

    public Document CreateDocument() {
      char[] charsToTrim = new char[] { ' ', ';', '/' };
      Sentido sentido = new Sentido();
      Workday workDay = new Workday();
      Dictionary<int, int> listAtt = new Dictionary<int, int>();
      int[] tabelas;

      DefineStyles();

      /*
       * Identificacao e Qualificacao da Linha
       */
      StringBuilder concat = new StringBuilder();
      using (LinhaService linhas = new LinhaService()) {
        foreach (Linha item in linhas.GetQuery(this.filter)) {
          concat = new StringBuilder($"{Resources.LinhaId}: {item.Prefixo} - {item.Denominacao}");

          CreatePage(Orientation.Portrait);
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
          for (int j = 0; j < sentido.Data.Count; j++) {
            string ab = (j == 0) ? "AB" : "BA";

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

            row.Cells[1].AddParagraph(linhas.GetPontoInicial(item.Id, ab));
            row.Cells[1].Format.Font.Bold = false;

            row.Cells[3].AddParagraph(linhas.GetPontoFinal(item.Id, ab));
            row.Cells[3].Format.Font.Bold = false;
          }

          // PMM (Percurso Medio Mensal) da Linha
          row = table.AddRow();
          row.Height = "0.6 cm";
          row.Format.Font.Bold = true;

          row.Cells[0].AddParagraph(Resources.PMM);
          using (Services<ViagemLinha> viagens = new Services<ViagemLinha>()) {
            Expression<Func<ViagemLinha, bool>> condition = q => q.LinhaId == item.Id;
            try {
              concat = new StringBuilder($"{viagens.GetQuery(condition).Sum(q => q.PercursoAno) / CustomCalendar.MonthsPerYear:#,###.#}");
            }
            catch (DivideByZeroException ex) {
              concat = new StringBuilder(ex.Message);
            }
            row.Cells[1].AddParagraph(concat.ToString());
          }

          /*
           * Atendimentos da Linha
           */
          Paragraph paragraph = document.LastSection.AddParagraph();
          paragraph.Format.Alignment = ParagraphAlignment.Left;
          paragraph.Format.SpaceBefore = "0.4 cm";
          paragraph.Format.SpaceAfter = "0.25 cm";
          paragraph.AddFormattedText(Resources.AtendimentoViewModel, TextFormat.Bold);

          using (Services<Atendimento> atendimentos = new Services<Atendimento>()) {
            if (atendimentos.GetCount(q => q.LinhaId == item.Id) > 0) {
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
            if (viagens.GetCount(q => q.LinhaId == item.Id) > 0) {
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
              int[] totais = {
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

          using (Services<Horario> horarios = new Services<Horario>()) {
            tabelas = horarios.GetQuery(q => q.LinhaId == item.Id, q => q.OrderBy(h => h.DiaId))
                          .Select(h => h.DiaId).Distinct().ToArray();
          }
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

              using (Services<Horario> horarios = new Services<Horario>()) {
                int[] rows = {
                    horarios.GetCount(q => (q.LinhaId == item.Id) && (q.DiaId == hr) && q.Sentido.Equals("AB")),
                    horarios.GetCount(q => (q.LinhaId == item.Id) && (q.DiaId == hr) && q.Sentido.Equals("BA"))
                };
                aux = (rows[0] > rows[1]) ? rows[0] : rows[1];
                int page = ((aux % size) == 0) ? aux / size : (aux / size) + 1;

                Expression<Func<Horario, bool>> where = null;
                IOrderedQueryable<Horario> order(IQueryable<Horario> q) => q.OrderBy(e => e.Sentido).ThenBy(e => e.Inicio);
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
          }

          /*
           * Periodos Tipicos da Linha
           */
          paragraph = document.LastSection.AddParagraph();
          paragraph.Format.Alignment = ParagraphAlignment.Left;
          paragraph.Format.SpaceBefore = "0.4 cm";
          paragraph.Format.SpaceAfter = "0.25 cm";
          paragraph.AddFormattedText(Resources.PrLinhaViewModel, TextFormat.Bold);

          using (PeriodoTipicoService pTipicos = new PeriodoTipicoService()) {
            if (pTipicos.GetCount(q => q.LinhaId == item.Id) > 0) {
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
                foreach (PeriodoTipico pItem in pTipicos.GetQuery(q => (q.LinhaId == item.Id) && (q.DiaId == hr),
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
          }

          /*
           * Plano Operacional da Linha
           */
          paragraph = document.LastSection.AddParagraph();
          paragraph.Format.Alignment = ParagraphAlignment.Left;
          paragraph.Format.SpaceBefore = "0.4 cm";
          paragraph.Format.SpaceAfter = "0.25 cm";
          paragraph.AddFormattedText(Resources.OperacionalViewModel, TextFormat.Bold);

          using (Services<Operacional> operacionais = new Services<Operacional>()) {
            if (operacionais.GetCount(q => q.LinhaId == item.Id) > 0) {
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
                row.Cells[4].AddParagraph($"{opItem.ViagensDU:#,###}");
                row.Cells[5].AddParagraph($"{opItem.PercursoDU:#,###.##}");
                row.Cells[6].AddParagraph($"{opItem.ViagensSab:#,###}");
                row.Cells[7].AddParagraph($"{opItem.PercursoSab:#,###.##}");
                row.Cells[8].AddParagraph($"{opItem.ViagensDom:#,###}");
                row.Cells[9].AddParagraph($"{opItem.PercursoDom:#,###.##}");

                prefixo = opItem.Prefixo;
              }

              // Totais
              Expression<Func<Operacional, bool>> condition = q => q.LinhaId == item.Id;
              int?[] vgTotal = {
                  operacionais.GetQuery(condition).Sum(p => p.ViagensDU),
                  operacionais.GetQuery(condition).Sum(p => p.ViagensSab),
                  operacionais.GetQuery(condition).Sum(p => p.ViagensDom)
              };
              decimal?[] kmTotal = {
                  operacionais.GetQuery(condition).Sum(p => p.PercursoDU),
                  operacionais.GetQuery(condition).Sum(p => p.PercursoSab),
                  operacionais.GetQuery(condition).Sum(p => p.PercursoDom)
              };

              row = table.AddRow();
              row.Height = "0.25 in";
              row.Format.Alignment = ParagraphAlignment.Right;
              row.VerticalAlignment = VerticalAlignment.Center;
              row.Format.Font.Bold = true;

              row.Cells[2].AddParagraph(Resources.Total);
              try {
                decimal result = ((kmTotal[0] ?? 0) + (kmTotal[1] ?? 0) + (kmTotal[2] ?? 0)) /
                                   ((vgTotal[0] ?? 0) + (vgTotal[1] ?? 0) + (vgTotal[2] ?? 0));
                row.Cells[3].AddParagraph($"{result:#,##0.00}");
              }
              catch (DivideByZeroException ex) {
                row.Cells[3].AddParagraph(ex.Message);
              }
              row.Cells[4].AddParagraph($"{vgTotal[0]:#,##0}");
              row.Cells[5].AddParagraph($"{kmTotal[0]:#,##0.00}");
              row.Cells[6].AddParagraph($"{vgTotal[1]:#,##0}");
              row.Cells[7].AddParagraph($"{kmTotal[1]:#,##0.00}");
              row.Cells[8].AddParagraph($"{vgTotal[2]:#,##0}");
              row.Cells[9].AddParagraph($"{kmTotal[2]:#,##0.00}");
            }
          }

          /*
           * Resumo das Operacoes da Linha
           */
          paragraph = document.LastSection.AddParagraph();
          paragraph.Format.Alignment = ParagraphAlignment.Left;
          paragraph.Format.SpaceBefore = "0.4 cm";
          paragraph.Format.SpaceAfter = "0.25 cm";
          paragraph.AddFormattedText(Resources.ViagemLinhaViewModel, TextFormat.Bold);

          using (Services<ViagemLinha> viagens = new Services<ViagemLinha>()) {
            if (viagens.GetCount(q => q.LinhaId == item.Id) > 0) {
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
                row.Cells[4].AddParagraph($"{qItem.PercursoSemana:#,###.#}");
                row.Cells[5].AddParagraph($"{qItem.ViagensMes:#,###}");
                row.Cells[6].AddParagraph($"{qItem.PercursoMes:#,###.#}");
                row.Cells[7].AddParagraph($"{qItem.ViagensAno:#,###}");
                row.Cells[8].AddParagraph($"{qItem.PercursoAno:#,###.#}");

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
                row.Cells[4].AddParagraph($"{kmTotal.Value / CustomCalendar.WeeksPerYear:#,###.#}");
              }
              catch (DivideByZeroException ex) {
                row.Cells[3].MergeRight = 1;
                row.Cells[3].AddParagraph(ex.Message);
              }
              try {
                row.Cells[5].AddParagraph($"{vgTotal.Value / CustomCalendar.MonthsPerYear:#,###}");
                row.Cells[6].AddParagraph($"{kmTotal.Value / CustomCalendar.MonthsPerYear:#,###.#}");
              }
              catch (DivideByZeroException ex) {
                row.Cells[5].MergeRight = 1;
                row.Cells[5].AddParagraph(ex.Message);
              }
              row.Cells[7].AddParagraph($"{vgTotal.Value:#,###}");
              row.Cells[8].AddParagraph($"{kmTotal.Value:#,###.#}");

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
              using (MapaLinhaService mapas = new MapaLinhaService()) {
                if (mapas.GetCount(where) > 0) {
                  foreach (MapaLinha mapa in mapas.GetQuery(where, q => q.OrderBy(m => m.AtendimentoId)
                                                                         .ThenBy(m => m.Sentido))) {
                    string fileName = $@"C:\Temp\Mapas\{mapa.Arquivo}";
                    if (File.Exists(fileName)) {
                      Image image = section.AddImage(fileName);
                      image.Height = "14 cm";
                      image.Width = "14 cm";
                      image.Top = ShapePosition.Top;
                      image.Left = ShapePosition.Center;
                      image.WrapFormat.Style = WrapStyle.Through;
                    }
                    paragraph = document.LastSection.AddParagraph();
                    paragraph.Style = "Normal";
                    paragraph.Format.SpaceBefore = "14.75 cm";
                    paragraph.Format.SpaceAfter = "0.2 in";

                    if (!string.IsNullOrWhiteSpace(mapa.Descricao)) {
                      paragraph.AddFormattedText(mapa.Descricao, TextFormat.Bold);
                      paragraph.Format.Alignment = ParagraphAlignment.Center;
                    }
                    else {
                      if (!mapa.AtendimentoId.HasValue) {
                        using (LinhaService pontos = new LinhaService()) {
                          concat = new StringBuilder($"{item.Prefixo} ({sentido.Data[mapa.Sentido]})");
                          paragraph.AddFormattedText(concat.ToString(), TextFormat.Bold);
                          paragraph.Format.Alignment = ParagraphAlignment.Center;
                        }
                      }
                      else {
                        using (AtendimentoService pontos = new AtendimentoService()) {
                          concat = new StringBuilder($"{mapa.Atendimento.Prefixo} ({sentido.Data[mapa.Sentido]})");
                          paragraph.AddFormattedText(concat.ToString(), TextFormat.Bold);
                          paragraph.Format.Alignment = ParagraphAlignment.Center;
                        }
                      }
                    }

                    // Itinerarios da Linha e dos Atendimentos
                    if (!pItem.AtendimentoId.HasValue) {
                      Expression<Func<Itinerario, bool>> condition = q => (q.LinhaId == pItem.LinhaId) &&
                                                                          q.Sentido.Equals(pItem.Sentido);
                      using (Services<Itinerario> itinerarios = new Services<Itinerario>()) {
                        if (itinerarios.GetCount(condition) > 0) {
                          AddTable(this.section);
                          column = table.AddColumn("18 cm");

                          StringBuilder percurso = new StringBuilder();
                          foreach (Itinerario iItem in itinerarios.GetQuery(condition, q => q.OrderBy(e => e.Id))) {
                            percurso.Append($"{iItem.Percurso}; ");
                          }

                          row = table.AddRow();
                          row.Height = "0.8 cm";
                          row.Format.Alignment = ParagraphAlignment.Left;
                          row.Cells[0].AddParagraph(percurso.ToString().Trim(charsToTrim));
                        }
                      }
                    }
                    else {
                      Expression<Func<ItAtendimento, bool>> condition = q => (q.AtendimentoId == pItem.AtendimentoId) &&
                                                                             q.Sentido.Equals(pItem.Sentido);
                      using (Services<ItAtendimento> itAtendimentos = new Services<ItAtendimento>()) {
                        if (itAtendimentos.GetCount(condition) > 0) {
                          AddTable(this.section);
                          column = table.AddColumn("18 cm");

                          StringBuilder percurso = new StringBuilder();
                          foreach (ItAtendimento itItem in itAtendimentos.GetQuery(condition, q => q.OrderBy(e => e.Id))) {
                            percurso.Append($"{itItem.Percurso}; ");
                          }

                          row = table.AddRow();
                          row.Height = "0.8 cm";
                          row.Format.Alignment = ParagraphAlignment.Left;
                          row.Cells[0].AddParagraph(percurso.ToString().Trim(charsToTrim));
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
                    paragraph.AddFormattedText(
                      $"{Resources.ItinerarioViewModel} {pItem.Prefixo} - {pItem.Linha.Denominacao}", TextFormat.Bold);

                    using (Services<Itinerario> itinerarios = new Services<Itinerario>()) {
                      if (itinerarios.GetCount(q => (q.LinhaId == pItem.LinhaId) &&
                                                    q.Sentido.Equals(pItem.Sentido)) > 0) {
                        paragraph = document.LastSection.AddParagraph();
                        paragraph.Format.SpaceBefore = "0.4 cm";
                        paragraph.Format.SpaceAfter = "0.2 in";
                        paragraph.AddFormattedText($"{Resources.Sentido}: {sentido.Data[pItem.Sentido]}", TextFormat.Bold);

                        foreach (Itinerario iItem in itinerarios.GetQuery(q => (q.LinhaId == pItem.LinhaId) &&
                                                                               q.Sentido.Equals(pItem.Sentido),
                                                                          q => q.OrderBy(e => e.Id))) {
                          paragraph = document.LastSection.AddParagraph();
                          paragraph.Style = "Table";
                          paragraph.Format.SpaceAfter = "0.25 cm";
                          paragraph.Format.LeftIndent = "0.55 cm";
                          paragraph.AddText(iItem.Percurso);
                        }
                      }
                    }
                  }
                  else {
                    paragraph.AddFormattedText(
                      $"{Resources.ItAtendimentoViewModel} {pItem.Prefixo} - {pItem.Atendimento.Denominacao}", TextFormat.Bold);

                    using (Services<ItAtendimento> itAtendimentos = new Services<ItAtendimento>()) {
                      if (itAtendimentos.GetCount(q => (q.AtendimentoId == pItem.AtendimentoId) &&
                                                       q.Sentido.Equals(pItem.Sentido)) > 0) {
                        paragraph = document.LastSection.AddParagraph();
                        paragraph.Format.SpaceBefore = "0.4 cm";
                        paragraph.Format.SpaceAfter = "0.2 in";
                        paragraph.AddFormattedText($"{Resources.Sentido}: {sentido.Data[pItem.Sentido]}", TextFormat.Bold);

                        foreach (ItAtendimento itItem in itAtendimentos.GetQuery(
                            q => (q.AtendimentoId == pItem.AtendimentoId) && q.Sentido.Equals(pItem.Sentido),
                            q => q.OrderBy(e => e.Id))) {
                          paragraph = document.LastSection.AddParagraph();
                          paragraph.Style = "Table";
                          paragraph.Format.SpaceAfter = "0.25 cm";
                          paragraph.Format.LeftIndent = "0.55 cm";
                          paragraph.AddText(itItem.Percurso);
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      return this.document;
    }
  }
}