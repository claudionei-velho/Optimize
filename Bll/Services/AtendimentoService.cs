﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class AtendimentoService : Services<Atendimento> {
    private readonly int userId;

    public AtendimentoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Atendimento> Get(Expression<Func<Atendimento, bool>> filter = null, 
        Func<IQueryable<Atendimento>, IOrderedQueryable<Atendimento>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<Atendimento> query = (from a in context.Atendimentos
                                         join l in context.Linhas on a.LinhaId equals l.Id
                                         where companies.Contains(l.EmpresaId)
                                         orderby l.EmpresaId, a.LinhaId, a.Id
                                         select a).Include(a => a.Linha.Empresa).AsNoTracking();
        if (filter != null) {
          query = query.Where(filter);
        }
        if (orderBy != null) {
          query = orderBy(query);
        }
        return query;
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
    }

    public string GetPontoInicial(int id, string ab) {
      string result = string.Empty;
      using (Services<ItAtendimento> itinerarios = new Services<ItAtendimento>()) {
        result = itinerarios.GetFirst(q => (q.AtendimentoId == id) && q.Sentido.Equals(ab)).Percurso;
      }
      return result;
    }

    public string GetPontoFinal(int id, string ab) {
      string result = string.Empty;
      using (Services<ItAtendimento> itinerarios = new Services<ItAtendimento>()) {
        result = itinerarios.GetById(itinerarios.GetQuery(
                                         q => (q.AtendimentoId == id) && q.Sentido.Equals(ab)
                                     ).Max(p => p.Id)).Percurso;
      }
      return result;
    }
  }
}
