USE [Optimize]
GO
/****** Object:  UserDefinedFunction [opc].[toMinute]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [opc].[toMinute] (@reference TIME(7)) RETURNS INT WITH SCHEMABINDING AS
BEGIN
  RETURN datePart(hour, @reference) * 60 + datePart(minute, @reference);
END;
GO
/****** Object:  View [opc].[PTipicos]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[PTipicos] WITH SCHEMABINDING AS
SELECT fonte.Id,
       PrLinhas.LinhaId,
       EPeriodos.Denominacao,
       PrLinhas.DiaId,
       PrLinhas.Inicio,
       PrLinhas.Termino,
       fonte.pInicio,
       fonte.pTermino,
       fonte.pTermino - fonte.pInicio Duracao,
       convert(INT, floor(CVeiculos.Minimo * Ocupacoes.Densidade)) Lotacao,
       PrLinhas.CicloAB,
       PrLinhas.CicloBA
FROM (
  SELECT _periodo.Id,
         opc.toMinute(_periodo.Inicio) pInicio,
         CASE
           WHEN (_periodo.Inicio < _periodo.Termino) THEN
             opc.toMinute(_periodo.Termino)
           ELSE
             1440 + opc.toMinute(_periodo.Termino)
         END pTermino  
  FROM   opc.PrLinhas _periodo) fonte
INNER JOIN opc.PrLinhas ON PrLinhas.Id = fonte.Id
  INNER JOIN opc.EPeriodos ON EPeriodos.Id = PrLinhas.PeriodoId
  INNER JOIN opc.CVeiculos ON CVeiculos.Id = PrLinhas.CVeiculoId
  INNER JOIN opc.Ocupacoes ON Ocupacoes.Id = PrLinhas.OcupacaoId;
GO
/****** Object:  UserDefinedFunction [opc].[getPeriodo]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [opc].[getPeriodo] (@linhaId INT, @diaId INT, @time TIME(7)) 
    RETURNS INT WITH SCHEMABINDING AS
BEGIN
  DECLARE @result INT;
  
  SET @result = (SELECT min(_time.Id)
                 FROM   opc.PTipicos _time
                 WHERE  _time.LinhaId = @linhaId AND _time.DiaId = @diaId AND
                        opc.toMinute(@time) BETWEEN _time.pInicio AND _time.pTermino);
  IF (@result IS NULL) BEGIN
    SET @result = (SELECT max(_time.Id)
                   FROM   opc.PTipicos _time
                   WHERE  _time.LinhaId = @linhaId AND _time.DiaId = @diaId);
  
  END;
         
  RETURN @result;
END;
GO
/****** Object:  View [opc].[FAjuste]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[FAjuste] WITH SCHEMABINDING AS
SELECT   fa.LinhaId,
         fa.Ano, 
         fa.Mes, 
         eoMonth(dateFromParts(fa.Ano, fa.Mes, 1)) Referencia,
         sum(fa.Passageiros) Passageiros,
         (SELECT max(_fonte.total)
          FROM (SELECT   _fa.LinhaId, _fa.Ano, _fa.Mes, sum(_fa.Passageiros) total
                FROM     opc.Ofertas _fa
                GROUP BY _fa.LinhaId, _fa.Ano, _fa.Mes) _fonte
          WHERE _fonte.LinhaId = fa.LinhaId AND _fonte.Ano = fa.Ano) / 
              nullIf(convert(NUMERIC, sum(fa.Passageiros)), 0) Fator
FROM     opc.Ofertas fa
GROUP BY fa.LinhaId, fa.Ano, fa.Mes;
GO
/****** Object:  UserDefinedFunction [opc].[coeficienteAjuste]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [opc].[coeficienteAjuste] (@referencia DATETIME, @linhaId INT, @diaId INT = NULL) 
    RETURNS NUMERIC(18, 6) WITH SCHEMABINDING AS
BEGIN
  DECLARE @fa NUMERIC(15, 6),
          @ir NUMERIC(15, 6);
          
  SET @fa = (SELECT max(fa.Fator)
             FROM   opc.FAjuste fa
             WHERE  fa.LinhaId = @linhaId AND
                    fa.Referencia = (SELECT max(_fa.Referencia)
                                     FROM   opc.FAjuste _fa
                                     WHERE  _fa.LinhaId = @linhaId AND
                                            _fa.Referencia <= @referencia));
            
  SET @ir = 0;
  IF (NOT @diaId IS NULL) BEGIN
    SET @ir = (SELECT max(ir.Indice)
               FROM   opc.Renovacao ir
               WHERE  ir.LinhaId = @linhaId AND ir.DiaId = @diaId AND
                      ir.Referencia = (SELECT max(_ir.Referencia)
                                       FROM   opc.Renovacao _ir 
                                       WHERE  _ir.LinhaId = @linhaId AND _ir.DiaId = @diaId AND
                                              _ir.Referencia <= @referencia));
  END;
                   
  RETURN coalesce(@fa, 1) / (1 + (coalesce(@ir, 0) * 0.01));
END;
GO
/****** Object:  UserDefinedFunction [dbo].[contarFeriados]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[contarFeriados] (@opTableId INT, @year INT) RETURNS INT WITH SCHEMABINDING AS
BEGIN
  RETURN (
    SELECT count(fonte.Tabela)
    FROM (SELECT CASE
                   WHEN (f.Mes IS NOT NULL) AND (f.Dia IS NOT NULL) THEN
                     CASE
                       WHEN (datePart(weekDay, dateFromParts(@year, f.Mes, f.Dia)) = 1) THEN 3
                       WHEN (datePart(weekDay, dateFromParts(@year, f.Mes, f.Dia)) = 7) THEN 2
                       ELSE 1
                     END
                   ELSE 1
                 END Tabela
          FROM dbo.Feriados f) fonte
    WHERE fonte.Tabela = @opTableId);
END;
GO
/****** Object:  UserDefinedFunction [dbo].[isLeapYear]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[isLeapYear] (@year INT) RETURNS BIT WITH SCHEMABINDING AS
BEGIN
  DECLARE @result BIT;
  
  SET @result = 0;
  IF (((@year % 400) = 0 OR (@year % 100) <> 0) AND (@year % 4) = 0)
    SET @result = 1;
  
  RETURN @result;
END;
GO
/****** Object:  UserDefinedFunction [opc].[getDiasOp]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [opc].[getDiasOp](@year INT)
  RETURNS @resultSet TABLE (Id INT PRIMARY KEY,
                            Dias INT NOT NULL) WITH SCHEMABINDING AS
BEGIN
  DECLARE @i INT,
          @j INT;
          
  SET @i = 1;
  WHILE (@i <= 3) BEGIN
    IF (@i = 1)        -- Dias Uteis
      SET @j = (365 + dbo.isLeapYear(@year) - ((104 - dbo.contarFeriados(2, @year)) + 
                 (SELECT count(Id) FROM dbo.Feriados) - dbo.contarFeriados(3, @year)));
    ELSE BEGIN
      IF (@i = 2)      -- Sabados
        SET @j = 52 - dbo.contarFeriados(2, @year);
      ELSE             -- Domingos
        SET @j = 52 + ((SELECT count(Id) FROM dbo.Feriados) - dbo.contarFeriados(3, @year));
    END;
    
    INSERT INTO @resultSet VALUES (@i, @j);
    SET @i += 1;
  END;
  
  RETURN;
END;
GO
/****** Object:  View [opc].[PlanoSintese]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[PlanoSintese] WITH SCHEMABINDING AS
SELECT base.EmpresaId,
       base.DiaId,
       max(base.Dias) Dias,
       sum(base.Viagens) Viagens,
       sum(base.Percurso) Percurso
       -- round((max(base.Dias) * sum(base.Viagens)) / 12, 1) ViagensMes,
       -- round((max(base.Dias) * sum(base.Percurso)) / 12, 1) PercursoMes,
       -- round(max(base.Dias) * sum(base.Viagens), 1) ViagensAno,
       -- round(max(base.Dias) * sum(base.Percurso), 1) PercursoAno       
FROM (
  SELECT fonte.EmpresaId, 
         wd.Id DiaId, 
         max(wd.Dias) Dias,
         CASE
           WHEN (wd.Id = 1) THEN sum(fonte.ViagensUtil)
           WHEN (wd.Id = 2) THEN sum(fonte.ViagensSab)
           WHEN (wd.Id = 3) THEN sum(fonte.ViagensDom)
         END Viagens,
         CASE
           WHEN (wd.Id = 1) THEN sum(fonte.PercursoUtil)
           WHEN (wd.Id = 2) THEN sum(fonte.PercursoSab)
           WHEN (wd.Id = 3) THEN sum(fonte.PercursoDom)
         END Percurso
  FROM (
    SELECT Linhas.EmpresaId,
           Planos.LinhaId,
           Planos.AtendimentoId,
           Planos.Sentido,                  
           sum(Planos.ViagensUtil) ViagensUtil,
           CASE
             WHEN (Planos.Sentido = 'AB') THEN
               max(Linhas.ExtensaoAB) * sum(Planos.ViagensUtil)
             WHEN (Planos.Sentido = 'BA') THEN
               max(Linhas.ExtensaoBA) * sum(Planos.ViagensUtil)
           END PercursoUtil,             
           sum(Planos.ViagensSab) ViagensSab,
           CASE
             WHEN (Planos.Sentido = 'AB') THEN
               max(Linhas.ExtensaoAB) * sum(Planos.ViagensSab)
             WHEN (Planos.Sentido = 'BA') THEN
               max(Linhas.ExtensaoBA) * sum(Planos.ViagensSab)
           END PercursoSab,
           sum(Planos.ViagensDom) ViagensDom,
           CASE
             WHEN (Planos.Sentido = 'AB') THEN
               max(Linhas.ExtensaoAB) * sum(Planos.ViagensDom)
             WHEN (Planos.Sentido = 'BA') THEN
               max(Linhas.ExtensaoBA) * sum(Planos.ViagensDom)
           END PercursoDom
    FROM opc.Planos
    INNER JOIN opc.Linhas ON Linhas.Id = Planos.LinhaId
    WHERE Planos.AtendimentoId IS NULL
    GROUP BY Linhas.EmpresaId, Planos.LinhaId, 
             Planos.AtendimentoId, Planos.Sentido) fonte, opc.getDiasOp(year(getDate())) wd
  GROUP BY fonte.EmpresaId, wd.Id

  UNION ALL
  SELECT fonte.EmpresaId, 
         wd.Id DiaId, 
         max(wd.Dias) Dias,
         CASE
           WHEN (wd.Id = 1) THEN sum(fonte.ViagensUtil)
           WHEN (wd.Id = 2) THEN sum(fonte.ViagensSab)
           WHEN (wd.Id = 3) THEN sum(fonte.ViagensDom)
         END Viagens,
         CASE
           WHEN (wd.Id = 1) THEN sum(fonte.PercursoUtil)
           WHEN (wd.Id = 2) THEN sum(fonte.PercursoSab)
           WHEN (wd.Id = 3) THEN sum(fonte.PercursoDom)
         END Percurso
  FROM (
    SELECT Linhas.EmpresaId,
           Planos.LinhaId,
           Planos.AtendimentoId,
           Planos.Sentido,
           sum(Planos.ViagensUtil) ViagensUtil,
           CASE
             WHEN (Planos.Sentido = 'AB') THEN
               max(Atendimentos.ExtensaoAB) * sum(Planos.ViagensUtil)
             WHEN (Planos.Sentido = 'BA') THEN
               max(Atendimentos.ExtensaoBA) * sum(Planos.ViagensUtil)
           END PercursoUtil,             
           sum(Planos.ViagensSab) ViagensSab,
           CASE
             WHEN (Planos.Sentido = 'AB') THEN
               max(Atendimentos.ExtensaoAB) * sum(Planos.ViagensSab)
             WHEN (Planos.Sentido = 'BA') THEN
               max(Atendimentos.ExtensaoBA) * sum(Planos.ViagensSab)
           END PercursoSab,
           sum(Planos.ViagensDom) ViagensDom,
           CASE
             WHEN (Planos.Sentido = 'AB') THEN
               max(Atendimentos.ExtensaoAB) * sum(Planos.ViagensDom)
             WHEN (Planos.Sentido = 'BA') THEN
               max(Atendimentos.ExtensaoBA) * sum(Planos.ViagensDom)
           END PercursoDom
    FROM opc.Planos  
    INNER JOIN opc.Atendimentos ON Atendimentos.Id = Planos.AtendimentoId
    INNER JOIN opc.Linhas ON Linhas.Id = Atendimentos.LinhaId
    WHERE Planos.AtendimentoId IS NOT NULL
    GROUP BY Linhas.EmpresaId, Planos.LinhaId,
             Planos.AtendimentoId, Planos.Sentido) fonte, opc.getDiasOp(year(getDate())) wd
  GROUP BY fonte.EmpresaId, wd.Id) base
GROUP BY base.EmpresaId, base.DiaId;
GO
/****** Object:  UserDefinedFunction [opc].[getLinhaDiaOp]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [opc].[getLinhaDiaOp] (@linhaId INT) RETURNS VARCHAR(32) WITH SCHEMABINDING AS
BEGIN
  DECLARE @util   BIT, 
          @sab    BIT, 
          @dom    BIT,
          @result VARCHAR(32);
          
  SET @util = (SELECT ln.Uteis FROM opc.Linhas ln WHERE ln.Id = @linhaId);
  SET @sab = (SELECT ln.Sabados FROM opc.Linhas ln WHERE ln.Id = @linhaId);
  SET @dom = (SELECT ln.Domingos FROM opc.Linhas ln WHERE ln.Id = @linhaId);
  
  SET @result = '';
  IF (@util = 1) BEGIN
    SET @result += 'Dia Útil; ';
  END;
  IF (@sab = 1) BEGIN
    SET @result += 'Sábado; ';
  END;
  IF (@dom = 1) BEGIN
    SET @result += 'Domingo;';
  END;
     
  RETURN left(rtrim(@result), len(rtrim(@result)) - 1);
END;
GO
/****** Object:  UserDefinedFunction [opc].[getAtendDiaOp]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [opc].[getAtendDiaOp] (@atendId INT) RETURNS VARCHAR(32) WITH SCHEMABINDING AS
BEGIN
  DECLARE @util   BIT, 
          @sab    BIT, 
          @dom    BIT,
          @result VARCHAR(32);
          
  SET @util = (SELECT att.Uteis FROM opc.Atendimentos att WHERE att.Id = @atendId);
  SET @sab = (SELECT att.Sabados FROM opc.Atendimentos att WHERE att.Id = @atendId);
  SET @dom = (SELECT att.Domingos FROM opc.Atendimentos att WHERE att.Id = @atendId);
  
  SET @result = '';
  IF (@util = 1) BEGIN
    SET @result += 'Dia Útil; ';
  END;
  IF (@sab = 1) BEGIN
    SET @result += 'Sábado; ';
  END;
  IF (@dom = 1) BEGIN
    SET @result += 'Domingo;';
  END;
     
  RETURN left(rtrim(@result), len(rtrim(@result)) - 1);
END;
GO
/****** Object:  UserDefinedFunction [opc].[getLinhaFuncao]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [opc].[getLinhaFuncao] (@linhaId INT) RETURNS VARCHAR(64) WITH SCHEMABINDING AS
BEGIN
  DECLARE @captacao  BIT, 
          @transport BIT, 
          @distrib   BIT,
          @result    VARCHAR(64);
          
  SET @captacao  = (SELECT ln.Captacao FROM opc.Linhas ln WHERE ln.Id = @linhaId);
  SET @transport = (SELECT ln.Transporte FROM opc.Linhas ln WHERE ln.Id = @linhaId);
  SET @distrib   = (SELECT ln.Distribuicao FROM opc.Linhas ln WHERE ln.Id = @linhaId);
  
  SET @result = '';
  IF (@captacao = 1) BEGIN
    SET @result += 'Captação; ';
  END;
  IF (@transport = 1) BEGIN
    SET @result += 'Transporte; ';
  END;
  IF (@distrib = 1) BEGIN
    SET @result += 'Distribuição;';
  END;
     
  RETURN left(rtrim(@result), len(rtrim(@result)) - 1);
END;
GO
/****** Object:  View [opc].[Operacional]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[Operacional] WITH SCHEMABINDING AS
SELECT 
DISTINCT linha.EmpresaId,
         Horarios.LinhaId,
         NULL AtendimentoId,
         linha.Prefixo,
         linha.Denominacao,
         Horarios.Sentido,
         opc.getLinhaDiaOp(linha.Id) DiaOperacao,
         opc.getLinhaFuncao(linha.Id) Funcao,
         CASE
           WHEN (Horarios.Sentido = 'AB') THEN linha.ExtensaoAB
           WHEN (Horarios.Sentido = 'BA') THEN linha.ExtensaoBA
         END Extensao,
         nullIf((SELECT count(hr.Inicio)
                 FROM opc.Horarios hr
                 WHERE hr.LinhaId = linha.Id AND hr.DiaId = 1 AND 
                       hr.Sentido = Horarios.Sentido AND hr.AtendimentoId IS NULL), 0) ViagensUtil,
         CASE
           WHEN (Horarios.Sentido = 'AB') THEN
             nullIf(round(linha.ExtensaoAB * 
                 (SELECT count(hr.Inicio)
                  FROM opc.Horarios hr
                  WHERE hr.LinhaId = linha.Id AND hr.DiaId = 1 AND 
                        hr.Sentido = Horarios.Sentido AND hr.AtendimentoId IS NULL), 3), 0)
           WHEN (Horarios.Sentido = 'BA') THEN
             nullIf(round(linha.ExtensaoBA * 
                 (SELECT count(hr.Inicio)
                  FROM opc.Horarios hr
                  WHERE hr.LinhaId = linha.Id AND hr.DiaId = 1 AND 
                        hr.Sentido = Horarios.Sentido AND hr.AtendimentoId IS NULL), 3), 0)
         END PercursoUtil,
         (SELECT min(hr.Inicio)
          FROM opc.Horarios hr
          WHERE hr.LinhaId = linha.Id AND hr.DiaId = 1 AND
                hr.Sentido = Horarios.Sentido AND hr.AtendimentoId IS NULL) InicioUtil,         
         nullIf((SELECT count(hr.Inicio)
                 FROM opc.Horarios hr
                 WHERE hr.LinhaId = linha.Id AND hr.DiaId = 2 AND 
                       hr.Sentido = Horarios.Sentido AND hr.AtendimentoId IS NULL), 0) ViagensSab,
         CASE
           WHEN (Horarios.Sentido = 'AB') THEN
             nullIf(round(linha.ExtensaoAB * 
                 (SELECT count(hr.Inicio)
                  FROM opc.Horarios hr
                  WHERE hr.LinhaId = linha.Id AND hr.DiaId = 2 AND 
                        hr.Sentido = Horarios.Sentido AND hr.AtendimentoId IS NULL), 3), 0)
           WHEN (Horarios.Sentido = 'BA') THEN
             nullIf(round(linha.ExtensaoBA * 
                 (SELECT count(hr.Inicio)
                  FROM opc.Horarios hr
                  WHERE hr.LinhaId = linha.Id AND hr.DiaId = 2 AND 
                        hr.Sentido = Horarios.Sentido AND hr.AtendimentoId IS NULL), 3), 0)
         END PercursoSab,
         (SELECT min(hr.Inicio)
          FROM opc.Horarios hr
          WHERE hr.LinhaId = linha.Id AND hr.DiaId = 2 AND
                hr.Sentido = Horarios.Sentido AND hr.AtendimentoId IS NULL) InicioSab,         
         nullIf((SELECT count(hr.Inicio)
                 FROM opc.Horarios hr
                 WHERE hr.LinhaId = linha.Id AND hr.DiaId = 3 AND 
                       hr.Sentido = Horarios.Sentido AND hr.AtendimentoId IS NULL), 0) ViagensDom,
         CASE
           WHEN (Horarios.Sentido = 'AB') THEN
             nullIf(round(linha.ExtensaoAB * 
                 (SELECT count(hr.Inicio)
                  FROM opc.Horarios hr
                  WHERE hr.LinhaId = linha.Id AND hr.DiaId = 3 AND 
                        hr.Sentido = Horarios.Sentido AND hr.AtendimentoId IS NULL), 3), 0)
           WHEN (Horarios.Sentido = 'BA') THEN
             nullIf(round(linha.ExtensaoBA * 
                 (SELECT count(hr.Inicio)
                  FROM opc.Horarios hr
                  WHERE hr.LinhaId = linha.Id AND hr.DiaId = 3 AND 
                        hr.Sentido = Horarios.Sentido AND hr.AtendimentoId IS NULL), 3), 0)
         END PercursoDom,
         (SELECT min(hr.Inicio)
          FROM opc.Horarios hr
          WHERE hr.LinhaId = linha.Id AND hr.DiaId = 3 AND
                hr.Sentido = Horarios.Sentido AND hr.AtendimentoId IS NULL) InicioDom
FROM opc.Horarios
INNER JOIN opc.Linhas linha ON linha.Id = Horarios.LinhaId
WHERE Horarios.AtendimentoId IS NULL

UNION ALL
SELECT 
DISTINCT linha.EmpresaId,
         Horarios.LinhaId,
         Horarios.AtendimentoId,
         atend.Prefixo,
         atend.Denominacao,
         Horarios.Sentido,
         opc.getAtendDiaOp(atend.Id) DiaOperacao,
         opc.getLinhaFuncao(atend.LinhaId) Funcao,
         CASE
           WHEN (Horarios.Sentido = 'AB') THEN atend.ExtensaoAB
           WHEN (Horarios.Sentido = 'BA') THEN atend.ExtensaoBA
         END Extensao,
         nullIf((SELECT count(hr.Inicio)
                 FROM opc.Horarios hr
                 WHERE hr.AtendimentoId = atend.Id AND hr.DiaId = 1 AND 
                       hr.Sentido = Horarios.Sentido), 0) ViagensUtil,
         CASE
           WHEN (Horarios.Sentido = 'AB') THEN
             nullIf(round(atend.ExtensaoAB * 
                 (SELECT count(hr.Inicio)
                  FROM opc.Horarios hr
                  WHERE hr.AtendimentoId = atend.Id AND hr.DiaId = 1 AND 
                        hr.Sentido = Horarios.Sentido), 3), 0)
           WHEN (Horarios.Sentido = 'BA') THEN
             nullIf(round(atend.ExtensaoBA * 
                 (SELECT count(hr.Inicio)
                  FROM opc.Horarios hr
                  WHERE hr.AtendimentoId = atend.Id AND hr.DiaId = 1 AND 
                        hr.Sentido = Horarios.Sentido), 3), 0)
         END PercursoUtil,
         (SELECT min(hr.Inicio)
          FROM opc.Horarios hr
          WHERE hr.AtendimentoId = atend.Id AND hr.DiaId = 1 AND
                hr.Sentido = Horarios.Sentido) InicioUtil,                       
         nullIf((SELECT count(hr.Inicio)
                 FROM opc.Horarios hr
                 WHERE hr.AtendimentoId = atend.Id AND hr.DiaId = 2 AND 
                       hr.Sentido = Horarios.Sentido), 0) ViagensSab,
         CASE
           WHEN (Horarios.Sentido = 'AB') THEN
             nullIf(round(atend.ExtensaoAB * 
                 (SELECT count(hr.Inicio)
                  FROM opc.Horarios hr
                  WHERE hr.AtendimentoId = atend.Id AND hr.DiaId = 2 AND 
                        hr.Sentido = Horarios.Sentido), 3), 0)
           WHEN (Horarios.Sentido = 'BA') THEN
             nullIf(round(atend.ExtensaoBA * 
                 (SELECT count(hr.Inicio)
                  FROM opc.Horarios hr
                  WHERE hr.AtendimentoId = atend.Id AND hr.DiaId = 2 AND 
                        hr.Sentido = Horarios.Sentido), 3), 0)
         END PercursoSab,
         (SELECT min(hr.Inicio)
          FROM opc.Horarios hr
          WHERE hr.AtendimentoId = atend.Id AND hr.DiaId = 2 AND
                hr.Sentido = Horarios.Sentido) InicioSab,         
         nullIf((SELECT count(hr.Inicio)
                 FROM opc.Horarios hr
                 WHERE hr.AtendimentoId = atend.Id AND hr.DiaId = 3 AND 
                       hr.Sentido = Horarios.Sentido), 0) ViagensDom,
         CASE
           WHEN (Horarios.Sentido = 'AB') THEN
             nullIf(round(atend.ExtensaoAB * 
                 (SELECT count(hr.Inicio)
                  FROM opc.Horarios hr
                  WHERE hr.AtendimentoId = atend.Id AND hr.DiaId = 3 AND 
                        hr.Sentido = Horarios.Sentido), 3), 0)
           WHEN (Horarios.Sentido = 'BA') THEN
             nullIf(round(atend.ExtensaoBA * 
                 (SELECT count(hr.Inicio)
                  FROM opc.Horarios hr
                  WHERE hr.AtendimentoId = atend.Id AND hr.DiaId = 3 AND 
                        hr.Sentido = Horarios.Sentido), 3), 0)
         END PercursoDom,
         (SELECT min(hr.Inicio)
          FROM opc.Horarios hr
          WHERE hr.AtendimentoId = atend.Id AND hr.DiaId = 3 AND
                hr.Sentido = Horarios.Sentido) InicioDom
FROM opc.Horarios
INNER JOIN opc.Atendimentos atend ON atend.Id = Horarios.AtendimentoId
  INNER JOIN opc.Linhas linha ON linha.Id = atend.LinhaId
WHERE Horarios.AtendimentoId IS NOT NULL;
GO
/****** Object:  View [opc].[ValidatePlan]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[ValidatePlan] WITH SCHEMABINDING AS
SELECT fonte.EmpresaId,
       fonte.LinhaId,
       fonte.AtendimentoId,
       fonte.Prefixo,
       fonte.Denominacao,
       fonte.DiaOperacao,
       fonte.Sentido,
       fonte.Extensao,
       fonte.ViagensUtil,
       fonte.HorariosUtil,
       fonte.ViagensSab,
       fonte.HorariosSab,
       fonte.ViagensDom,
       fonte.HorariosDom       
FROM (
    SELECT Linhas.EmpresaId,
           plano.LinhaId,
           plano.AtendimentoId,
           Linhas.Prefixo,
           Linhas.Denominacao,
           opc.getLinhaDiaOp(plano.LinhaId) DiaOperacao,
           plano.Sentido,
           CASE
             WHEN (plano.Sentido = 'AB') THEN
               Linhas.ExtensaoAB
             WHEN (plano.Sentido = 'BA') THEN
               Linhas.ExtensaoBA
           END Extensao,
           plano.ViagensUtil,
           nullIf((SELECT count(hr.Inicio)
                   FROM opc.Horarios hr
                   WHERE hr.LinhaId = plano.LinhaId AND hr.DiaId = 1 AND 
                         hr.Sentido = plano.Sentido AND hr.AtendimentoId IS NULL), 0) HorariosUtil,       
           plano.ViagensSab,       
           nullIf((SELECT count(hr.Inicio)
                   FROM opc.Horarios hr
                   WHERE hr.LinhaId = plano.LinhaId AND hr.DiaId = 2 AND 
                         hr.Sentido = plano.Sentido AND hr.AtendimentoId IS NULL), 0) HorariosSab,       
           plano.ViagensDom,
           nullIf((SELECT count(hr.Inicio)
                   FROM opc.Horarios hr
                   WHERE hr.LinhaId = plano.LinhaId AND hr.DiaId = 3 AND 
                         hr.Sentido = plano.Sentido AND hr.AtendimentoId IS NULL), 0) HorariosDom       
    FROM opc.Planos plano
    INNER JOIN opc.Linhas ON Linhas.Id = plano.LinhaId
    WHERE plano.AtendimentoId IS NULL

    UNION ALL
    SELECT Linhas.EmpresaId,
           Atendimentos.LinhaId,
           plano.AtendimentoId,
           Atendimentos.Prefixo,
           Atendimentos.Denominacao,
           opc.getLinhaDiaOp(Atendimentos.LinhaId) DiaOperacao,
           plano.Sentido,
           CASE
             WHEN (plano.Sentido = 'AB') THEN
               Atendimentos.ExtensaoAB
             WHEN (plano.Sentido = 'BA') THEN
               Atendimentos.ExtensaoBA
           END Extensao,
           plano.ViagensUtil,
           nullIf((SELECT count(hr.Inicio)
                   FROM opc.Horarios hr
                   WHERE hr.AtendimentoId = plano.AtendimentoId AND hr.DiaId = 1 AND 
                         hr.Sentido = plano.Sentido), 0) HorariosUtil,              
           plano.ViagensSab,
           nullIf((SELECT count(hr.Inicio)
                   FROM opc.Horarios hr
                   WHERE hr.AtendimentoId = plano.AtendimentoId AND hr.DiaId = 2 AND 
                         hr.Sentido = plano.Sentido), 0) HorariosSab,
           plano.ViagensDom,
           nullIf((SELECT count(hr.Inicio)
                   FROM opc.Horarios hr
                   WHERE hr.AtendimentoId = plano.AtendimentoId AND hr.DiaId = 3 AND 
                         hr.Sentido = plano.Sentido), 0) HorariosDom       
    FROM opc.Planos plano
    INNER JOIN opc.Linhas ON Linhas.Id = plano.LinhaId
    INNER JOIN opc.Atendimentos ON Atendimentos.Id = plano.AtendimentoId
    WHERE plano.AtendimentoId IS NOT NULL) fonte
WHERE (coalesce(fonte.ViagensUtil, 0) <> coalesce(fonte.HorariosUtil, 0)) OR
      (coalesce(fonte.ViagensSab, 0) <> coalesce(fonte.HorariosSab, 0)) OR
      (coalesce(fonte.ViagensDom, 0) <> coalesce(fonte.HorariosDom, 0));
GO
/****** Object:  View [opc].[DiasTrabalho]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[DiasTrabalho] WITH SCHEMABINDING AS
SELECT wd.Id, 
       wd.Dias
FROM opc.getDiasOp(year(getDate())) wd
GO
/****** Object:  UserDefinedFunction [opc].[getWorkdays]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [opc].[getWorkdays](@opTableId INT, @year INT) RETURNS INT WITH SCHEMABINDING AS
BEGIN
  RETURN (SELECT wd.Dias
          FROM opc.getDiasOp(@year) wd
          WHERE wd.Id = @opTableId);
END;
GO
/****** Object:  View [opc].[PercursoMensal]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[PercursoMensal] WITH SCHEMABINDING AS
SELECT Linhas.EmpresaId,
       fonte.LinhaId,
       'Principal' Tipo,
       Linhas.Prefixo,
       Linhas.Denominacao,
       CASE
         WHEN (fonte.DiaId = 1) THEN '1. Útil'
         WHEN (fonte.DiaId = 2) THEN '2. Sáb'
         WHEN (fonte.DiaId = 3) THEN '3. Dom'
       END DiaOperacao,
       opc.getWorkdays(fonte.DiaId, year(getDate())) *
           (sum(fonte.ViagensAB) * Linhas.ExtensaoAB) / 12 PercursoAB,
       opc.getWorkdays(fonte.DiaId, year(getDate())) *
           (sum(fonte.ViagensBA) * Linhas.ExtensaoBA) / 12 PercursoBA
FROM (
  SELECT hr.LinhaId,
         hr.DiaId,
         hr.Sentido,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN count(hr.Inicio) 
           WHEN (hr.Sentido = 'BA') THEN NULL
         END ViagensAB,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN NULL
           WHEN (hr.Sentido = 'BA') THEN count(hr.Inicio) 
         END ViagensBA                     
  FROM opc.Horarios hr
  WHERE hr.AtendimentoId IS NULL
  GROUP BY hr.LinhaId, hr.DiaId, hr.Sentido) fonte
INNER JOIN opc.Linhas ON Linhas.Id = fonte.LinhaId
GROUP BY Linhas.EmpresaId, Fonte.LinhaId, Linhas.Prefixo, 
         Linhas.Denominacao, fonte.DiaId, Linhas.ExtensaoAB, Linhas.ExtensaoBA

UNION ALL
SELECT Linhas.EmpresaId,
       fonte.AtendimentoId,
       'Atendimento' Tipo,
       atende.Prefixo,
       atende.Denominacao,
       CASE
         WHEN (fonte.DiaId = 1) THEN '1. Útil'
         WHEN (fonte.DiaId = 2) THEN '2. Sáb'
         WHEN (fonte.DiaId = 3) THEN '3. Dom'
       END DiaOperacao,
       opc.getWorkdays(fonte.DiaId, year(getDate())) *
           (sum(fonte.ViagensAB) * atende.ExtensaoAB) / 12 PercursoAB,
       opc.getWorkdays(fonte.DiaId, year(getDate())) *
           (sum(fonte.ViagensBA) * atende.ExtensaoBA) / 12 PercursoBA
FROM (
  SELECT hr.AtendimentoId,
         hr.DiaId,
         hr.Sentido,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN count(hr.Inicio) 
           WHEN (hr.Sentido = 'BA') THEN NULL
         END ViagensAB,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN NULL
           WHEN (hr.Sentido = 'BA') THEN count(hr.Inicio) 
         END ViagensBA  
  FROM opc.Horarios hr
  WHERE hr.AtendimentoId IS NOT NULL
  GROUP BY hr.AtendimentoId, hr.DiaId, hr.Sentido) fonte
INNER JOIN opc.Atendimentos atende ON atende.Id = fonte.AtendimentoId
  INNER JOIN opc.Linhas ON Linhas.Id = atende.LinhaId
GROUP BY Linhas.EmpresaId, fonte.AtendimentoId, atende.Prefixo,
         atende.Denominacao, fonte.DiaId, atende.ExtensaoAB, atende.ExtensaoBA;
GO
/****** Object:  View [opc].[PercursoPrograma]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[PercursoPrograma] WITH SCHEMABINDING AS
SELECT fonte.EmpresaId,
       fonte.DiaId,
       CASE
         WHEN (fonte.DiaId = 1) THEN '1. Útil'
         WHEN (fonte.DiaId = 2) THEN '2. Sáb'
         WHEN (fonte.DiaId = 3) THEN '3. Dom'
       END DiaOperacao,
       opc.getWorkdays(fonte.DiaId, year(getDate())) Dias,
       sum(fonte.KmDiarioAB) KmDiarioAB,
       sum(fonte.KmDiarioBA) KmDiarioBA,
       round(sum(fonte.KmAnualAB) / 52, 2) KmSemanalAB,
       round(sum(fonte.KmAnualBA) / 52, 2) KmSemanalBA,       
       round(sum(fonte.KmAnualAB) / 12, 2) KmMensalAB,
       round(sum(fonte.KmAnualBA) / 12, 2) KmMensalBA,
       sum(fonte.KmAnualAB) KmAnualAB,
       sum(fonte.KmAnualBA) KmAnualBA
FROM (
  SELECT
  DISTINCT linha.EmpresaId,
           hr.LinhaId,
           hr.DiaId,
           (SELECT sum(_src.ViagensAB) * (SELECT Linhas.ExtensaoAB 
                                          FROM opc.Linhas WHERE Linhas.Id = hr.LinhaId)
            FROM (SELECT count(_hr.Inicio) ViagensAB
                  FROM opc.Horarios _hr
                  WHERE _hr.LinhaId = hr.LinhaId AND _hr.DiaId = hr.DiaId AND
                        _hr.Sentido = 'AB' AND _hr.AtendimentoId IS NULL) _src) KmDiarioAB,
           (SELECT sum(_src.ViagensBA) * (SELECT Linhas.ExtensaoBA
                                          FROM opc.Linhas WHERE Linhas.Id = hr.LinhaId)
            FROM (SELECT count(_hr.Inicio) ViagensBA
                  FROM opc.Horarios _hr
                  WHERE _hr.LinhaId = hr.LinhaId AND _hr.DiaId = hr.DiaId AND
                        _hr.Sentido = 'BA' AND _hr.AtendimentoId IS NULL) _src) KmDiarioBA,
           opc.getWorkdays(hr.DiaId, year(getDate())) *
               (SELECT sum(_src.ViagensAB) * (SELECT Linhas.ExtensaoAB 
                                              FROM opc.Linhas WHERE Linhas.Id = hr.LinhaId)
                FROM (SELECT count(_hr.Inicio) ViagensAB
                      FROM opc.Horarios _hr
                      WHERE _hr.LinhaId = hr.LinhaId AND _hr.DiaId = hr.DiaId AND
                            _hr.Sentido = 'AB' AND _hr.AtendimentoId IS NULL) _src) KmAnualAB,
           opc.getWorkdays(hr.DiaId, year(getDate())) *
               (SELECT sum(_src.ViagensBA) * (SELECT Linhas.ExtensaoBA 
                                              FROM opc.Linhas WHERE Linhas.Id = hr.LinhaId)
                FROM (SELECT count(_hr.Inicio) ViagensBA
                      FROM opc.Horarios _hr
                      WHERE _hr.LinhaId = hr.LinhaId AND _hr.DiaId = hr.DiaId AND
                            _hr.Sentido = 'BA' AND _hr.AtendimentoId IS NULL) _src) KmAnualBA
  FROM opc.Horarios hr
  INNER JOIN opc.Linhas linha ON linha.Id = hr.LinhaId
  WHERE hr.AtendimentoId IS NULL

  UNION ALL
  SELECT
  DISTINCT linha.EmpresaId,
           hr.AtendimentoId,
           hr.DiaId,
           (SELECT sum(_src.ViagensAB) * (SELECT Atendimentos.ExtensaoAB 
                                          FROM opc.Atendimentos 
                                          WHERE Atendimentos.Id = hr.AtendimentoId)
            FROM (SELECT count(_hr.Inicio) ViagensAB
                  FROM opc.Horarios _hr
                  WHERE _hr.AtendimentoId = hr.AtendimentoId AND 
                        _hr.DiaId = hr.DiaId AND _hr.Sentido = 'AB') _src) KmDiarioAB,
           (SELECT sum(_src.ViagensBA) * (SELECT Atendimentos.ExtensaoBA
                                          FROM opc.Atendimentos 
                                          WHERE Atendimentos.Id = hr.AtendimentoId)
            FROM (SELECT count(_hr.Inicio) ViagensBA
                  FROM opc.Horarios _hr
                  WHERE _hr.AtendimentoId = hr.AtendimentoId AND 
                        _hr.DiaId = hr.DiaId AND _hr.Sentido = 'BA') _src) KmDiarioBA,
           opc.getWorkdays(hr.DiaId, year(getDate())) *
               (SELECT sum(_src.ViagensAB) * (SELECT Atendimentos.ExtensaoAB 
                                              FROM opc.Atendimentos 
                                              WHERE Atendimentos.Id = hr.AtendimentoId)
                FROM (SELECT count(_hr.Inicio) ViagensAB
                      FROM opc.Horarios _hr
                      WHERE _hr.AtendimentoId = hr.AtendimentoId AND 
                            _hr.DiaId = hr.DiaId AND _hr.Sentido = 'AB') _src) KmAnualAB,
           opc.getWorkdays(hr.DiaId, year(getDate())) *
               (SELECT sum(_src.ViagensBA) * (SELECT Atendimentos.ExtensaoBA 
                                              FROM opc.Atendimentos 
                                              WHERE Atendimentos.Id = hr.AtendimentoId)
                FROM (SELECT count(_hr.Inicio) ViagensBA
                      FROM opc.Horarios _hr
                      WHERE _hr.AtendimentoId = hr.AtendimentoId AND 
                            _hr.DiaId = hr.DiaId AND _hr.Sentido = 'BA') _src) KmAnualBA
  FROM opc.Horarios hr
  INNER JOIN opc.Atendimentos atende ON atende.Id = hr.AtendimentoId
    INNER JOIN opc.Linhas linha ON linha.Id = atende.LinhaId
  WHERE hr.AtendimentoId IS NOT NULL) fonte
GROUP BY fonte.EmpresaId, fonte.DiaId;
GO
/****** Object:  View [opc].[ViagensMensais]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[ViagensMensais] WITH SCHEMABINDING AS
SELECT Linhas.EmpresaId,
       fonte.LinhaId,
       'Principal' Tipo,
       Linhas.Prefixo,
       Linhas.Denominacao,
       CASE
         WHEN (fonte.DiaId = 1) THEN '1. Útil'
         WHEN (fonte.DiaId = 2) THEN '2. Sáb'
         WHEN (fonte.DiaId = 3) THEN '3. Dom'
       END DiaOperacao,
       convert(INT, round(
         opc.getWorkdays(fonte.DiaId, year(getDate())) *
             convert(NUMERIC, sum(fonte.ViagensAB)) / 12, 0)) ViagensAB,
       convert(INT, round(
         opc.getWorkdays(fonte.DiaId, year(getDate())) *
             convert(NUMERIC, sum(fonte.ViagensBA)) / 12, 0)) ViagensBA
FROM (
  SELECT hr.LinhaId,
         hr.DiaId,
         hr.Sentido,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN count(hr.Inicio) 
           WHEN (hr.Sentido = 'BA') THEN NULL
         END ViagensAB,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN NULL
           WHEN (hr.Sentido = 'BA') THEN count(hr.Inicio) 
         END ViagensBA                     
  FROM opc.Horarios hr
  WHERE hr.AtendimentoId IS NULL
  GROUP BY hr.LinhaId, hr.DiaId, hr.Sentido) fonte
INNER JOIN opc.Linhas ON Linhas.Id = fonte.LinhaId
GROUP BY Linhas.EmpresaId, Fonte.LinhaId, Linhas.Prefixo, 
         Linhas.Denominacao, fonte.DiaId

UNION ALL
SELECT Linhas.EmpresaId,
       fonte.AtendimentoId,
       'Atendimento' Tipo,
       atende.Prefixo,
       atende.Denominacao,
       CASE
         WHEN (fonte.DiaId = 1) THEN '1. Útil'
         WHEN (fonte.DiaId = 2) THEN '2. Sáb'
         WHEN (fonte.DiaId = 3) THEN '3. Dom'
       END DiaOperacao,
       convert(INT, round(
         opc.getWorkdays(fonte.DiaId, year(getDate())) * 
             convert(NUMERIC, sum(fonte.ViagensAB)) / 12, 0)) ViagensBA,
       convert(INT, round(
         opc.getWorkdays(fonte.DiaId, year(getDate())) *
             convert(NUMERIC, sum(fonte.ViagensBA)) / 12, 0)) ViagensBA
FROM (
  SELECT hr.AtendimentoId,
         hr.DiaId,
         hr.Sentido,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN count(hr.Inicio) 
           WHEN (hr.Sentido = 'BA') THEN NULL
         END ViagensAB,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN NULL
           WHEN (hr.Sentido = 'BA') THEN count(hr.Inicio) 
         END ViagensBA  
  FROM opc.Horarios hr
  WHERE hr.AtendimentoId IS NOT NULL
  GROUP BY hr.AtendimentoId, hr.DiaId, hr.Sentido) fonte
INNER JOIN opc.Atendimentos atende ON atende.Id = fonte.AtendimentoId
  INNER JOIN opc.Linhas ON Linhas.Id = atende.LinhaId
GROUP BY Linhas.EmpresaId, fonte.AtendimentoId, atende.Prefixo,
         atende.Denominacao, fonte.DiaId;
GO
/****** Object:  View [opc].[ViagensPrograma]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[ViagensPrograma] WITH SCHEMABINDING AS
SELECT fonte.EmpresaId,
       fonte.DiaId,
       CASE
         WHEN (fonte.DiaId = 1) THEN '1. Útil'
         WHEN (fonte.DiaId = 2) THEN '2. Sáb'
         WHEN (fonte.DiaId = 3) THEN '3. Dom'
       END DiaOperacao,
       opc.getWorkdays(fonte.DiaId, year(getDate())) Dias,
       sum(fonte.DiarioAB) DiarioAB,
       sum(fonte.DiarioBA) DiarioBA,
       convert(INT, round(sum(fonte.AnualAB) / 12, 0)) MensalAB,
       convert(INT, round(sum(fonte.AnualBA) / 12, 0)) MensalBA,
       sum(fonte.AnualAB) AnualAB,
       sum(fonte.AnualBA) AnualBA
FROM (
  SELECT
  DISTINCT linha.EmpresaId,
           hr.DiaId,
           (SELECT sum(_src.ViagensAB)
            FROM (SELECT count(_hr.Inicio) ViagensAB
                  FROM opc.Horarios _hr
                  INNER JOIN opc.Linhas _line ON _line.Id = _hr.LinhaId
                  WHERE _line.EmpresaId = linha.EmpresaId AND _hr.DiaId = hr.DiaId AND
                        _hr.Sentido = 'AB' AND _hr.AtendimentoId IS NULL) _src) DiarioAB,
           (SELECT sum(_src.ViagensBA)
            FROM (SELECT count(_hr.Inicio) ViagensBA
                  FROM opc.Horarios _hr
                  INNER JOIN opc.Linhas _line ON _line.Id = _hr.LinhaId
                  WHERE _line.EmpresaId = linha.EmpresaId AND _hr.DiaId = hr.DiaId AND
                        _hr.Sentido = 'BA' AND _hr.AtendimentoId IS NULL) _src) DiarioBA,
           opc.getWorkdays(hr.DiaId, year(getDate())) *
               (SELECT sum(_src.ViagensAB)
                FROM (SELECT count(_hr.Inicio) ViagensAB
                      FROM opc.Horarios _hr
                      INNER JOIN opc.Linhas _line ON _line.Id = _hr.LinhaId
                      WHERE _line.EmpresaId = linha.EmpresaId AND _hr.DiaId = hr.DiaId AND
                            _hr.Sentido = 'AB' AND _hr.AtendimentoId IS NULL) _src) AnualAB,
           opc.getWorkdays(hr.DiaId, year(getDate())) *
               (SELECT sum(_src.ViagensBA)
                FROM (SELECT count(_hr.Inicio) ViagensBA
                      FROM opc.Horarios _hr
                      INNER JOIN opc.Linhas _line ON _line.Id = _hr.LinhaId
                      WHERE _line.EmpresaId = linha.EmpresaId AND _hr.DiaId = hr.DiaId AND
                            _hr.Sentido = 'BA' AND _hr.AtendimentoId IS NULL) _src) AnualBA
  FROM opc.Horarios hr
  INNER JOIN opc.Linhas linha ON linha.Id = hr.LinhaId
  WHERE hr.AtendimentoId IS NULL
  
  UNION ALL
  SELECT
  DISTINCT linha.EmpresaId,
           hr.DiaId,
           (SELECT sum(_src.ViagensAB)
            FROM (SELECT count(_hr.Inicio) ViagensAB
                  FROM opc.Horarios _hr
                  INNER JOIN opc.Atendimentos _atende ON _atende.Id = _hr.AtendimentoId
                    INNER JOIN opc.Linhas _line ON _line.Id = _atende.LinhaId
                  WHERE _line.EmpresaId = linha.EmpresaId AND _hr.DiaId = hr.DiaId AND
                        _hr.Sentido = 'AB' AND _hr.AtendimentoId IS NOT NULL) _src) DiarioAB,
           (SELECT sum(_src.ViagensBA)
            FROM (SELECT count(_hr.Inicio) ViagensBA
                  FROM opc.Horarios _hr
                  INNER JOIN opc.Atendimentos _atende ON _atende.Id = _hr.AtendimentoId
                    INNER JOIN opc.Linhas _line ON _line.Id = _atende.LinhaId
                  WHERE _line.EmpresaId = linha.EmpresaId AND _hr.DiaId = hr.DiaId AND
                        _hr.Sentido = 'BA' AND _hr.AtendimentoId IS NOT NULL) _src) DiarioBA,
           opc.getWorkdays(hr.DiaId, year(getDate())) *
               (SELECT sum(_src.ViagensAB)
                FROM (SELECT count(_hr.Inicio) ViagensAB
                      FROM opc.Horarios _hr
                      INNER JOIN opc.Atendimentos _atende ON _atende.Id = _hr.AtendimentoId
                        INNER JOIN opc.Linhas _line ON _line.Id = _atende.LinhaId
                      WHERE _line.EmpresaId = linha.EmpresaId AND _hr.DiaId = hr.DiaId AND
                            _hr.Sentido = 'AB' AND _hr.AtendimentoId IS NOT NULL) _src) AnualAB,
           opc.getWorkdays(hr.DiaId, year(getDate())) *
               (SELECT sum(_src.ViagensBA)
                FROM (SELECT count(_hr.Inicio) ViagensBA
                      FROM opc.Horarios _hr
                      INNER JOIN opc.Atendimentos _atende ON _atende.Id = _hr.AtendimentoId
                        INNER JOIN opc.Linhas _line ON _line.Id = _atende.LinhaId
                      WHERE _line.EmpresaId = linha.EmpresaId AND _hr.DiaId = hr.DiaId AND
                           _hr.Sentido = 'BA' AND _hr.AtendimentoId IS NOT NULL) _src) AnualBA
  FROM opc.Horarios hr
  INNER JOIN opc.Atendimentos atende ON atende.Id = hr.AtendimentoId
    INNER JOIN opc.Linhas linha ON linha.Id = atende.LinhaId
  WHERE hr.AtendimentoId IS NOT NULL) fonte
GROUP BY fonte.EmpresaId, fonte.DiaId;
GO
/****** Object:  View [opc].[ViagensLinha]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[ViagensLinha] WITH SCHEMABINDING AS
SELECT Linhas.EmpresaId,
       fonte.LinhaId,
       fonte.AtendimentoId,
       fonte.DiaId,
       fonte.Prefixo,
       opc.getWorkdays(fonte.DiaId, year(getDate())) * fonte.QtdViagens ViagensAno,
       opc.getWorkdays(fonte.DiaId, year(getDate())) * fonte.Extensao PercursoAno       
  FROM (
  SELECT src.LinhaId, 
         src.AtendimentoId, 
         src.DiaId,
         Linhas.Prefixo,
         coalesce(sum(src.ViagensAB), 0) + 
           coalesce(sum(src.ViagensBA), 0) QtdViagens,
         coalesce(sum(src.ViagensAB) * min(Linhas.ExtensaoAB), 0) + 
           coalesce(sum(src.ViagensBA) * min(Linhas.ExtensaoBA), 0) Extensao
  FROM (
    SELECT hr.LinhaId,
           hr.AtendimentoId,
           hr.DiaId,
           hr.Sentido,
           CASE 
             WHEN (hr.Sentido = 'AB') THEN count(hr.Inicio) 
             WHEN (hr.Sentido = 'BA') THEN NULL
           END ViagensAB,
           CASE 
             WHEN (hr.Sentido = 'AB') THEN NULL
             WHEN (hr.Sentido = 'BA') THEN count(hr.Inicio) 
           END ViagensBA
    FROM opc.Horarios hr  
    WHERE hr.AtendimentoId IS NULL
    GROUP BY hr.LinhaId, hr.AtendimentoId, hr.DiaId, hr.Sentido) src
  INNER JOIN opc.Linhas ON Linhas.Id = src.LinhaId
  GROUP BY src.LinhaId, src.AtendimentoId, src.DiaId, Linhas.Prefixo

  UNION ALL
  SELECT src.LinhaId, 
         src.AtendimentoId, 
         src.DiaId,
         Atendimentos.Prefixo,
         coalesce(sum(src.ViagensAB), 0) + 
           coalesce(sum(src.ViagensBA), 0) QtdViagens,
         coalesce(sum(src.ViagensAB) * min(Atendimentos.ExtensaoAB), 0) + 
           coalesce(sum(src.ViagensBA) * min(Atendimentos.ExtensaoBA), 0) Extensao
  FROM (
    SELECT hr.LinhaId, 
           hr.AtendimentoId,
           hr.DiaId,
           hr.Sentido,
           CASE 
             WHEN (hr.Sentido = 'AB') THEN count(hr.Inicio) 
             WHEN (hr.Sentido = 'BA') THEN NULL
           END ViagensAB,
           CASE 
             WHEN (hr.Sentido = 'AB') THEN NULL
             WHEN (hr.Sentido = 'BA') THEN count(hr.Inicio) 
           END ViagensBA
    FROM opc.Horarios hr
    WHERE hr.AtendimentoId IS NOT NULL
    GROUP BY hr.LinhaId, hr.AtendimentoId, hr.DiaId, hr.Sentido) src
  INNER JOIN opc.Atendimentos ON Atendimentos.Id = src.AtendimentoId
  GROUP BY src.LinhaId, src.AtendimentoId, src.DiaId, Atendimentos.Prefixo) fonte
INNER JOIN opc.Linhas ON Linhas.Id = fonte.LinhaId;
GO
/****** Object:  View [opc].[SinteticoPlano]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[SinteticoPlano] WITH SCHEMABINDING AS
SELECT fonte.EmpresaId,
       fonte.LinhaId,
       fonte.Viagens,
       convert(NUMERIC, fonte.Viagens) / nullIf(
         (SELECT 
            coalesce(opc.getWorkDays(1, year(getDate())) * sum(p.ViagensUtil), 0) +
            coalesce(opc.getWorkDays(2, year(getDate())) * sum(p.ViagensSab), 0) +
            coalesce(opc.getWorkDays(3, year(getDate())) * sum(p.ViagensDom), 0)
          FROM opc.Planos p
          INNER JOIN opc.Linhas ON Linhas.Id = p.LinhaId
          WHERE Linhas.EmpresaId = fonte.EmpresaId), 0) Ratio         
FROM (
    SELECT Linhas.EmpresaId,
           Planos.LinhaId,
           coalesce(opc.getWorkDays(1, year(getDate())) * sum(Planos.ViagensUtil), 0) +
             coalesce(opc.getWorkDays(2, year(getDate())) * sum(Planos.ViagensSab), 0) +
             coalesce(opc.getWorkDays(3, year(getDate())) * sum(Planos.ViagensDom), 0) Viagens
    FROM opc.Planos
    INNER JOIN opc.Linhas ON Linhas.Id = Planos.LinhaId
    GROUP BY Linhas.EmpresaId, Planos.LinhaId) fonte;
GO
/****** Object:  UserDefinedFunction [opc].[workDay]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [opc].[workDay] (@reference DATETIME) RETURNS INT WITH SCHEMABINDING AS
BEGIN
  DECLARE @result INT;
  
  SET @result = 1;
  IF (datePart(weekday, @reference) = 1)       -- Domingos
    SET @result = 3;
  ELSE BEGIN
    IF (datePart(weekday, @reference) = 7)     -- Sabados
      SET @result = 2;
  END;
  
  RETURN @result;
END
GO
/****** Object:  View [opc].[Sinotico]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[Sinotico] WITH SCHEMABINDING AS
SELECT fonte.PesquisaId,
       fonte.LinhaId,
       fonte.DiaId,
       ISinotico.Id SinoticoId
FROM (
  SELECT   
  DISTINCT   LnPesquisas.PesquisaId,
             LnPesquisas.LinhaId,
             opc.workDay(Viagens.Data) DiaId
  FROM       opc.Viagens
  INNER JOIN opc.LnPesquisas ON LnPesquisas.Id = Viagens.LinhaId
  WHERE      opc.workDay(Viagens.Data) = 1 AND
             /* Viagens.LinhaId = 3 */
             LnPesquisas.PesquisaId = 2) fonte, dbo.ISinotico;
GO
/****** Object:  View [opc].[Dimensionar]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[Dimensionar] WITH SCHEMABINDING AS
SELECT fonte.PesquisaId,
       fonte.LinhaId,
       opc.workDay(fonte.Data) DiaId,
       fonte.PeriodoId,
       fonte.Sentido,
       count(fonte.Id) / count(DISTINCT fonte.Data) QtdViagens,
       CASE 
         WHEN (min(fonte.Inicio) >= (SELECT _periodo.Inicio
                                     FROM   opc.PTipicos _periodo
                                     WHERE  _periodo.Id = fonte.PeriodoId)) THEN
           min(fonte.Inicio)
         ELSE
           CASE
             WHEN (NOT (SELECT     min(_fonte.Inicio)
                        FROM       opc.Viagens _fonte
                        INNER JOIN opc.LnPesquisas _pesquisa ON _pesquisa.Id = _fonte.LinhaId
                        WHERE      _pesquisa.LinhaId = fonte.LinhaId AND
                                   opc.workDay(_fonte.Data) = opc.workDay(fonte.Data) AND
                                   _fonte.Sentido = fonte.Sentido AND
                                   _fonte.Inicio >= (SELECT _periodo.Inicio
                                                     FROM   opc.PTipicos _periodo
                                                     WHERE  _periodo.Id = fonte.PeriodoId)) IS NULL) THEN
               (SELECT     min(_fonte.Inicio)
                FROM       opc.Viagens _fonte
                INNER JOIN opc.LnPesquisas _pesquisa ON _pesquisa.Id = _fonte.LinhaId
                WHERE      _pesquisa.LinhaId = fonte.LinhaId AND
                           opc.workDay(_fonte.Data) = opc.workDay(fonte.Data) AND
                           _fonte.Sentido = fonte.Sentido AND
                           _fonte.Inicio >= (SELECT _periodo.Inicio
                                             FROM   opc.PTipicos _periodo
                                             WHERE  _periodo.Id = fonte.PeriodoId))
             ELSE
               min(fonte.Inicio)
           END
       END Inicio, 
       CASE 
         WHEN (max(fonte.Inicio) <= (SELECT _periodo.Termino
                                     FROM   opc.PTipicos _periodo
                                     WHERE  _periodo.Id = fonte.PeriodoId)) THEN
           max(fonte.Inicio)
         ELSE
           CASE
             WHEN (NOT (SELECT     max(_fonte.Inicio)
                        FROM       opc.Viagens _fonte
                        INNER JOIN opc.LnPesquisas _pesquisa ON _pesquisa.Id = _fonte.LinhaId
                        WHERE      _pesquisa.LinhaId = fonte.LinhaId AND
                                   opc.workDay(_fonte.Data) = opc.workDay(fonte.Data) AND
                                   _fonte.Sentido = fonte.Sentido AND
                                   _fonte.Inicio <= (SELECT _periodo.Termino
                                                     FROM   opc.PTipicos _periodo
                                                     WHERE  _periodo.Id = fonte.PeriodoId)) IS NULL) THEN
               (SELECT     max(_fonte.Inicio)
                FROM       opc.Viagens _fonte
                INNER JOIN opc.LnPesquisas _pesquisa ON _pesquisa.Id = _fonte.LinhaId
                WHERE      _pesquisa.LinhaId = fonte.LinhaId AND
                           opc.workDay(_fonte.Data) = opc.workDay(fonte.Data) AND
                           _fonte.Sentido = fonte.Sentido AND
                           _fonte.Inicio <= (SELECT _periodo.Termino
                                             FROM   opc.PTipicos _periodo
                                             WHERE  _periodo.Id = fonte.PeriodoId))
             ELSE
               max(fonte.Inicio)
           END
       END Termino,
       sum(fonte.Intervalo) / count(DISTINCT fonte.Data) Ociosidade,
       sum(fonte.Passageiros) / count(DISTINCT fonte.Data) Passageiros,
       convert(INT, ceiling(sum(fonte.PassageiroAjuste))) / count(DISTINCT fonte.Data) Ajustado,
       max(fonte.Passageiros) Critica,
       convert(INT, ceiling(max(fonte.PassageiroAjuste))) CriticaAjuste,
       convert(INT, stdevp(fonte.Passageiros)) Desvio,
       CONVERT(INT, stdevp(fonte.PassageiroAjuste)) DesvioAjuste,       
       (SELECT _periodo.Lotacao
        FROM   opc.PTipicos _periodo
        WHERE  _periodo.Id = fonte.PeriodoId) LotacaoP,
       CASE
         WHEN (fonte.Sentido = 'AB') THEN
           (SELECT _periodo.CicloAB
            FROM   opc.PTipicos _periodo
            WHERE  _periodo.Id = fonte.PeriodoId) 
         ELSE NULL
       END CicloAB,
       CASE
         WHEN (fonte.Sentido = 'BA') THEN
           (SELECT _periodo.CicloBA
            FROM   opc.PTipicos _periodo
            WHERE  _periodo.Id = fonte.PeriodoId) 
         ELSE NULL
       END CicloBA,
       CASE 
         WHEN (fonte.Sentido = 'AB') THEN
           (SELECT Linhas.ExtensaoAB
            FROM   opc.Linhas
            WHERE  Linhas.Id = fonte.LinhaId)
         WHEN (fonte.Sentido = 'BA') THEN
           (SELECT Linhas.ExtensaoBA
            FROM   opc.Linhas
            WHERE  Linhas.Id = fonte.LinhaId)
       END Extensao
FROM (
  SELECT   LnPesquisas.PesquisaId,
           Viagens.Id,
           LnPesquisas.LinhaId,
           Viagens.Data,
           Viagens.PeriodoId,
           Viagens.Sentido,
           Viagens.VeiculoId,
           Viagens.Chegada,           
           Viagens.Inicio,
           CASE
             WHEN (NOT Viagens.Chegada IS NULL) THEN
               CASE
                 WHEN (Viagens.Inicio >= Viagens.Chegada) THEN
                   dateDiff(minute, Viagens.Chegada, Viagens.Inicio)
                 ELSE
                   1440 + dateDiff(minute, Viagens.Chegada, Viagens.Inicio)
               END
             ELSE
               convert(INT, NULL)
           END Intervalo,
           CASE 
             WHEN (coalesce(Viagens.Passageiros, 0) > (coalesce(Viagens.Final, 0) - coalesce(Viagens.Inicial, 0))) THEN
               Viagens.Passageiros
             ELSE
               coalesce(Viagens.Final, 0) - coalesce(Viagens.Inicial, 0)
           END Passageiros,
           CASE 
             WHEN (coalesce(Viagens.Passageiros, 0) > (coalesce(Viagens.Final, 0) - coalesce(Viagens.Inicial, 0))) THEN
               Viagens.Passageiros * opc.coeficienteAjuste(Viagens.Data, LnPesquisas.LinhaId, opc.workDay(Viagens.Data))
             ELSE
               (coalesce(Viagens.Final, 0) - coalesce(Viagens.Inicial, 0)) * 
                   opc.coeficienteAjuste(Viagens.Data, LnPesquisas.LinhaId, opc.workDay(Viagens.Data))
           END PassageiroAjuste
  FROM     opc.Viagens
  INNER JOIN opc.LnPesquisas ON LnPesquisas.Id = Viagens.LinhaId
) fonte
WHERE fonte.PeriodoId IS NOT NULL AND opc.workDay(fonte.Data) = 1
GROUP BY fonte.PesquisaId, fonte.LinhaId, opc.workDay(fonte.Data), fonte.PeriodoId, fonte.Sentido
HAVING (sum(fonte.Passageiros) / count(DISTINCT fonte.Data)) > 0;
GO
/****** Object:  View [opc].[PlanOperacional]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[PlanOperacional] WITH SCHEMABINDING AS
SELECT Linhas.EmpresaId,
       plano.LinhaId,
       plano.AtendimentoId,
       Linhas.Prefixo,
       Linhas.Denominacao,
       opc.getLinhaDiaOp(plano.LinhaId) DiaOperacao,
       opc.getLinhaFuncao(plano.LinhaId) Funcao,
       plano.Sentido,
       CASE
         WHEN (plano.Sentido = 'AB') THEN
           Linhas.ExtensaoAB
         WHEN (plano.Sentido = 'BA') THEN
           Linhas.ExtensaoBA
       END Extensao,
       plano.ViagensUtil,
       CASE
         WHEN (plano.Sentido = 'AB') THEN
           Linhas.ExtensaoAB * plano.ViagensUtil
         WHEN (plano.Sentido = 'BA') THEN
           Linhas.ExtensaoBA * plano.ViagensUtil
       END PercursoUtil,
       plano.ViagensSab,
       CASE
         WHEN (plano.Sentido = 'AB') THEN
           Linhas.ExtensaoAB * plano.ViagensSab
         WHEN (plano.Sentido = 'BA') THEN
           Linhas.ExtensaoBA * plano.ViagensSab
       END PercursoSab,
       plano.ViagensDom,
       CASE
         WHEN (plano.Sentido = 'AB') THEN
           Linhas.ExtensaoAB * plano.ViagensDom
         WHEN (plano.Sentido = 'BA') THEN
           Linhas.ExtensaoBA * plano.ViagensDom
       END PercursoDom   
FROM opc.Planos plano
INNER JOIN opc.Linhas ON Linhas.Id = plano.LinhaId
WHERE plano.AtendimentoId IS NULL

UNION ALL
SELECT Linhas.EmpresaId,
       Atendimentos.LinhaId,
       plano.AtendimentoId,
       Atendimentos.Prefixo,
       Atendimentos.Denominacao,
       opc.getLinhaDiaOp(Atendimentos.LinhaId) DiaOperacao,
       opc.getLinhaFuncao(Atendimentos.LinhaId) Funcao,
       plano.Sentido,
       CASE
         WHEN (plano.Sentido = 'AB') THEN
           Atendimentos.ExtensaoAB
         WHEN (plano.Sentido = 'BA') THEN
           Atendimentos.ExtensaoBA
       END Extensao,
       plano.ViagensUtil,
       CASE
         WHEN (plano.Sentido = 'AB') THEN
           Atendimentos.ExtensaoAB * plano.ViagensUtil
         WHEN (plano.Sentido = 'BA') THEN
           Atendimentos.ExtensaoBA * plano.ViagensUtil
       END PercursoUtil,
       plano.ViagensSab,
       CASE
         WHEN (plano.Sentido = 'AB') THEN
           Atendimentos.ExtensaoAB * plano.ViagensSab
         WHEN (plano.Sentido = 'BA') THEN
           Atendimentos.ExtensaoBA * plano.ViagensSab
       END PercursoSab,
       plano.ViagensDom,
       CASE
         WHEN (plano.Sentido = 'AB') THEN
           Atendimentos.ExtensaoAB * plano.ViagensDom
         WHEN (plano.Sentido = 'BA') THEN
           Atendimentos.ExtensaoBA * plano.ViagensDom
       END PercursoDom   
FROM opc.Planos plano
INNER JOIN opc.Linhas ON Linhas.Id = plano.LinhaId
INNER JOIN opc.Atendimentos ON Atendimentos.Id = plano.AtendimentoId
WHERE plano.AtendimentoId IS NOT NULL;
GO
/****** Object:  UserDefinedFunction [dbo].[removeAcentos]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[removeAcentos] (@text VARCHAR(MAX)) 
  RETURNS VARCHAR(MAX) WITH SCHEMABINDING AS
BEGIN
  DECLARE @count  INT,
          @a      CHAR,
          @s      CHAR,
          @result VARCHAR(MAX);

  SET @count  = 1;
  SET @result = '';    
  
  WHILE (@count <= len(@text)) BEGIN
    SET @s = substring(@text, @count, 1);     
    SET @a = CASE
               WHEN (@s COLLATE Latin1_General_CS_AS IN ('Á', 'À', 'Â', 'Ã', 'Ä')) THEN 'A'
               WHEN (@s COLLATE Latin1_General_CS_AS IN ('á', 'à', 'â', 'ã', 'ä')) THEN 'a'
               WHEN (@s COLLATE Latin1_General_CS_AS IN ('É', 'È', 'Ê', 'Ë')) THEN 'E'
               WHEN (@s COLLATE Latin1_General_CS_AS IN ('é', 'è', 'ê', 'ë')) THEN 'e'
               WHEN (@s COLLATE Latin1_General_CS_AS IN ('Í', 'Ì', 'Î', 'Ï')) THEN 'I'
               WHEN (@s COLLATE Latin1_General_CS_AS IN ('í', 'ì', 'î', 'ï')) THEN 'i'
               WHEN (@s COLLATE Latin1_General_CS_AS IN ('Ó', 'Ò', 'Õ', 'Ô', 'Ö')) THEN 'O'
               WHEN (@s COLLATE Latin1_General_CS_AS IN ('ó', 'ò', 'ô', 'õ', 'ö')) THEN 'o'               
               WHEN (@s COLLATE Latin1_General_CS_AS IN ('Ú', 'Ù', 'Û', 'Ü')) THEN 'U'
               WHEN (@s COLLATE Latin1_General_CS_AS IN ('ú', 'ù', 'û', 'ü')) THEN 'u'
               WHEN (@s COLLATE Latin1_General_CS_AS = 'Ç') THEN 'C'
               WHEN (@s COLLATE Latin1_General_CS_AS = 'ç') THEN 'c'
               ELSE @s
             END;
             
    SET @result  = concat(@result, @a);      
    SET @count  += 1;
  END;
  
  RETURN @result;  
END;
GO
/****** Object:  UserDefinedFunction [dbo].[trim]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[trim] (@string VARCHAR(MAX)) 
  RETURNS VARCHAR(MAX) WITH SCHEMABINDING AS
BEGIN
  RETURN rtrim(ltrim(@string));
END;
GO
/****** Object:  UserDefinedFunction [opc].[acumPassageiros]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [opc].[acumPassageiros] (@viagemId INT, @sequence INT) 
    RETURNS INT WITH SCHEMABINDING AS
BEGIN
  RETURN (SELECT coalesce(sum(_f.Embarques), 0) - coalesce(sum(_f.Desembarques), 0)
          FROM   opc.FViagens _f
          WHERE  _f.ViagemId = @viagemId AND _f.Id < @sequence) +
            (SELECT coalesce(f.Embarques, 0) - coalesce(f.Desembarques, 0)
             FROM   opc.FViagens f
             WHERE  f.ViagemId = @viagemId AND f.Id = @sequence);
END;
GO
/****** Object:  UserDefinedFunction [opc].[somaFxEtarias]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [opc].[somaFxEtarias] (@companyId INT, @classId INT)
    RETURNS INT WITH SCHEMABINDING AS
BEGIN
  DECLARE @util INT,
          @soma INT;
          
  SET @soma = 0;
  SET @util = (SELECT max(ecv.Util)
               FROM opc.ECVeiculos ecv
               WHERE ecv.EmpresaId = @companyId AND ecv.ClasseId = @classId);
  WHILE (@util > 0) BEGIN
    SET @soma += @util;
    SET @util -= 1;
  END;
  
  RETURN @soma;
END;
GO
/****** Object:  UserDefinedFunction [opc].[totalKm]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [opc].[totalKm] (@linhaId INT, @diaId INT, @sentido CHAR(2)) 
  RETURNS NUMERIC(24, 3) WITH SCHEMABINDING AS
BEGIN
  DECLARE @extensao NUMERIC(18, 3);
  
  SET @extensao = 
      (SELECT sum(coalesce(src.viagens, 0) * coalesce(src.extensao, 0))
       FROM (
         SELECT   hr.AtendimentoId,
                  count(hr.Id) viagens,
                  CASE
                    WHEN (hr.AtendimentoId IS NULL) THEN
                      CASE
                        WHEN (hr.Sentido = 'AB') THEN
                          (SELECT Linhas.ExtensaoAB
                           FROM   opc.Linhas
                           WHERE  Linhas.Id = hr.LinhaId)
                        WHEN (hr.Sentido = 'BA') THEN
                          (SELECT Linhas.ExtensaoBA
                           FROM   opc.Linhas
                           WHERE  Linhas.Id = hr.LinhaId)               
                      END
                    ELSE
                      CASE
                        WHEN (hr.Sentido = 'AB') THEN
                          (SELECT Atendimentos.ExtensaoAB
                           FROM   opc.Atendimentos
                           WHERE  Atendimentos.Id = hr.AtendimentoId)
                        WHEN (hr.Sentido = 'BA') THEN
                          (SELECT Atendimentos.ExtensaoAB
                           FROM   opc.Atendimentos
                           WHERE  Atendimentos.Id = hr.AtendimentoId)               
                      END
                  END extensao
         FROM     opc.Horarios hr
         WHERE    hr.LinhaId = @linhaId AND hr.DiaId = @diaId AND hr.Sentido = @sentido
         GROUP BY hr.LinhaId, hr.DiaId, hr.Sentido, hr.AtendimentoId) src);
  
  RETURN @extensao;
END;
GO
/****** Object:  View [dbo].[IbgeUf]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[IbgeUf] WITH SCHEMABINDING AS
SELECT Ufs.Id,
       Ufs.Sigla,
       Ufs.Estado,
       Ufs.Capital,
       Ufs.Regiao,
       (SELECT count(Cidades.Id)
        FROM dbo.Cidades
        WHERE Cidades.UfId = Ufs.Id) Unidades
FROM dbo.Ufs;
GO
/****** Object:  View [dbo].[Municipios]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Municipios] WITH SCHEMABINDING AS
SELECT     Cidades.Id,
           Cidades.UfId,
           Cidades.Municipio Nome,
           Ufs.Sigla Estado
FROM       dbo.Cidades
INNER JOIN dbo.Ufs ON Ufs.Id = Cidades.UfId;
GO
/****** Object:  View [opc].[CustoMod]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[CustoMod] WITH SCHEMABINDING AS
SELECT cst.Id,
       cst.EmpresaId,
       cst.Referencia,
       cst.Fixo,
       cst.Variavel
FROM   opc.Custos cst
WHERE  cst.Id IN (SELECT _cst.Id
                  FROM   opc.Custos _cst
                  WHERE  _cst.EmpresaId = cst.EmpresaId AND
                         _cst.Referencia = (SELECT max(__cst.Referencia)
                                            FROM   opc.Custos __cst
                                            WHERE  __cst.EmpresaId = _cst.EmpresaId));
GO
/****** Object:  View [opc].[DemandaAnual]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[DemandaAnual] WITH SCHEMABINDING AS
SELECT Ofertas.LinhaId,
       Ofertas.Ano,
       Ofertas.Categoria,
       (SELECT min(TCategorias.Rateio)
        FROM opc.TCategorias
        WHERE TCategorias.Id = Ofertas.Categoria) Rateio,
       sum(Ofertas.Passageiros) Passageiros,
       convert(INT, ceiling(sum(Ofertas.Passageiros) * 
                      (SELECT min(TCategorias.Rateio)
                       FROM opc.TCategorias
                       WHERE TCategorias.Id = Ofertas.Categoria) * 0.01)) Equivalente
FROM opc.Ofertas
WHERE Ofertas.Ano >= ((SELECT max(_dem.Ano)
                       FROM opc.Ofertas _dem
                       WHERE _dem.LinhaId = Ofertas.LinhaId) - 5) AND
      Ofertas.Ano < (SELECT max(_dem.Ano)
                     FROM opc.Ofertas _dem
                     WHERE _dem.LinhaId = Ofertas.LinhaId)
GROUP BY Ofertas.LinhaId, Ofertas.Ano, Ofertas.Categoria;
GO
/****** Object:  View [opc].[DemandaCriciuma]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[DemandaCriciuma] WITH SCHEMABINDING AS
SELECT pvt.EmpresaId,
       pvt.Ano,
       pvt.Mes,
       [5] ComCartao,
       [6] SemCartao,
       [7] Estudante,
       [8] Professor,
       [9] ValeTransporte,
       [10] Idoso,
       [11] Especial,
       [12] Funcionario,
       [13] Gratuidade,
       coalesce([5], 0) + coalesce([6], 0) + coalesce([7], 0) +
         coalesce([8], 0) + coalesce([9], 0) + coalesce([10], 0) +
         coalesce([11], 0) + coalesce([12], 0) + coalesce([13], 0) Demanda,
       coalesce([5], 0) + floor(coalesce([6], 0) * 1.09) + 
         floor(coalesce([7], 0) * 0.5) + floor(coalesce([8], 0) * 0.75) + 
         coalesce([9], 0) DemandaEqv       
FROM (SELECT p.EmpresaId,
             p.Ano,
             p.Mes,
             p.TarifariaId,
             p.Passageiros
      FROM opc.Producao p
      INNER JOIN opc.TCategorias t ON t.Id = p.TarifariaId
      WHERE t.EmpresaId = 5) fonte
PIVOT (sum(fonte.Passageiros) FOR fonte.TarifariaId IN ([5], [6], [7], [8], [9], [10], [11], [12], [13])) pvt
GO
/****** Object:  View [opc].[DemandaMensal]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[DemandaMensal] WITH SCHEMABINDING AS
SELECT Ofertas.LinhaId,
       Ofertas.Ano,
       Ofertas.Mes,
       Ofertas.Categoria,
       (SELECT min(TCategorias.Rateio)
        FROM opc.TCategorias
        WHERE TCategorias.Id = Ofertas.Categoria) Rateio,
       sum(Ofertas.Passageiros) Passageiros,
       convert(INT, ceiling(sum(Ofertas.Passageiros) * 
                      (SELECT min(TCategorias.Rateio)
                       FROM opc.TCategorias
                       WHERE TCategorias.Id = Ofertas.Categoria) * 0.01)) Equivalente
FROM opc.Ofertas
WHERE dateFromParts(Ofertas.Ano, Ofertas.Mes, 1) > (
    SELECT dateAdd(month, -12, max(dateFromParts(_dem.Ano, _dem.Mes, 1)))
    FROM opc.Ofertas _dem
    WHERE _dem.LinhaId = Ofertas.LinhaId)
GROUP BY Ofertas.LinhaId, Ofertas.Ano, Ofertas.Mes, Ofertas.Categoria;
GO
/****** Object:  View [opc].[DemandaMod]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[DemandaMod] WITH SCHEMABINDING AS
SELECT fonte.LinhaId,
       fonte.Ano,
       fonte.Mes,
       sum(fonte.Passageiros) Passageiros,
       sum(fonte.Equivalente) Equivalente
FROM (
    SELECT Ofertas.LinhaId,
           Ofertas.Ano,
           Ofertas.Mes,
           Ofertas.Categoria,
           (SELECT min(TCategorias.Rateio)
            FROM opc.TCategorias
            WHERE TCategorias.Id = Ofertas.Categoria) Rateio,
           sum(Ofertas.Passageiros) Passageiros,
           convert(INT, 
               ceiling(sum(Ofertas.Passageiros) * 
                   (SELECT min(TCategorias.Rateio)
                    FROM opc.TCategorias
                    WHERE TCategorias.Id = Ofertas.Categoria) * 0.01)) Equivalente
    FROM opc.Ofertas
    WHERE dateFromParts(Ofertas.Ano, Ofertas.Mes, 1) > (
        SELECT dateAdd(month, -12, max(dateFromParts(_dem.Ano, _dem.Mes, 1)))
        FROM opc.Ofertas _dem
        WHERE _dem.LinhaId = Ofertas.LinhaId)
    GROUP BY Ofertas.LinhaId, Ofertas.Ano, Ofertas.Mes, Ofertas.Categoria) fonte
GROUP BY fonte.LinhaId, fonte.Ano, fonte.Mes;
GO
/****** Object:  View [opc].[DemandaPalhoca]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[DemandaPalhoca] WITH SCHEMABINDING AS
SELECT pvt.EmpresaId,
       pvt.Ano,
       pvt.Mes,
       [14] Antecipada,
       [15] Embarcada,
       [16] ValeTransporte,
       [17] Estudante,
       [18] ProfessorMun,       
       [19] ProfessorSC,
       [20] Gratuidade,
       coalesce([14], 0) + coalesce([15], 0) + coalesce([16], 0) + coalesce([17], 0) + 
         coalesce([18], 0) + coalesce([19], 0) + coalesce([20], 0) Demanda,
       coalesce([14], 0) + coalesce([15], 0) + coalesce([16], 0) + 
         floor(coalesce([17], 0) * 0.5) + floor(coalesce([18], 0) * 0.5) DemandaEqv       
FROM (SELECT p.EmpresaId,
             p.Ano,
             p.Mes,
             p.TarifariaId,
             p.Passageiros
      FROM opc.Producao p
      INNER JOIN opc.TCategorias t ON t.Id = p.TarifariaId
      WHERE t.EmpresaId = 7) fonte
PIVOT (sum(fonte.Passageiros) FOR fonte.TarifariaId IN ([14], [15], [16], [17], [18], [19], [20])) pvt
GO
/****** Object:  View [opc].[Depreciacoes]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[Depreciacoes] WITH SCHEMABINDING AS
SELECT ECVeiculos.Id ClasseId,
       FxEtarias.Id EtariaId,
       ((SELECT max(_ecv.Util)
         FROM opc.ECVeiculos _ecv
         WHERE _ecv.Id = ECVeiculos.Id) - FxEtarias.Minimo) Anos       
FROM opc.ECVeiculos, opc.FxEtarias
WHERE ((SELECT max(_ecv.Util)
        FROM opc.ECVeiculos _ecv
        WHERE _ecv.Id = ECVeiculos.Id) - FxEtarias.Minimo) >= 0;
GO
/****** Object:  View [opc].[Distribuicao]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[Distribuicao] WITH SCHEMABINDING AS
SELECT fonte.EmpresaId,
       fonte.DiaId,
       CASE
         WHEN (fonte.DiaId = 1) THEN 'Dias Úteis'
         WHEN (fonte.DiaId = 2) THEN 'Sábados'
         ELSE 'Domingos'
       END TabelaId,       
       fonte.LinhaId,
       (SELECT TOP(1) Linhas.Prefixo FROM opc.Linhas
        WHERE Linhas.Id = fonte.LinhaId) Prefixo,
       (SELECT TOP(1) Linhas.Denominacao FROM opc.Linhas
        WHERE Linhas.Id = fonte.LinhaId) Denominacao,
       fonte.HoraFixa,
       fonte.Viagens,
       CASE 
         WHEN (fonte.ViagemAB > fonte.ViagemBA) THEN
           CASE
             WHEN ((fonte.ViagemAB / (convert(NUMERIC, 60) / fonte.Viagens)) < fonte.Viagens) THEN
               ceiling(fonte.ViagemAB / (convert(NUMERIC, 60) / fonte.Viagens))
             ELSE 
               fonte.Viagens
           END
         ELSE
           CASE
             WHEN ((fonte.ViagemBA / (convert(NUMERIC, 60) / fonte.Viagens)) < fonte.Viagens) THEN
               ceiling(fonte.ViagemBA / (convert(NUMERIC, 60) / fonte.Viagens))
             ELSE
               fonte.Viagens
           END
       END Veiculos       
FROM (
  SELECT linha.EmpresaId,
         hora.LinhaId,
         hora.DiaId,
         datePart(hour, hora.Inicio) HoraFixa,
         (SELECT coalesce(hFaixa.CicloAB, 0)
          FROM opc.PrLinhas hFaixa
          WHERE hFaixa.Id = (SELECT min(_faixa.Id)
                             FROM opc.PrLinhas _faixa
                             WHERE _faixa.LinhaId = hora.LinhaId AND _faixa.DiaId = hora.DiaId AND
                                   _faixa.Inicio <= timeFromParts(datePart(hour, hora.Inicio), 0, 0, 0, 0) AND
                                   _faixa.Termino >= timeFromParts(datePart(hour, hora.Inicio), 59, 0, 0, 0))) ViagemAB,
         (SELECT coalesce(hFaixa.CicloBA, 0)
          FROM opc.PrLinhas hFaixa
          WHERE hFaixa.Id = (SELECT min(_faixa.Id)
                             FROM opc.PrLinhas _faixa
                             WHERE _faixa.LinhaId = hora.LinhaId AND _faixa.DiaId = hora.DiaId AND
                                   _faixa.Inicio <= timeFromParts(datePart(hour, hora.Inicio), 0, 0, 0, 0) AND
                                   _faixa.Termino >= timeFromParts(datePart(hour, hora.Inicio), 59, 0, 0, 0))) ViagemBA,
         count(hora.Inicio) Viagens
FROM opc.Horarios hora
INNER JOIN opc.Linhas linha ON linha.Id = hora.LinhaId
GROUP BY linha.EmpresaId, hora.LinhaId, hora.DiaId, datePart(hour, hora.Inicio)) fonte;
GO
/****** Object:  View [opc].[FichaTecnica]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[FichaTecnica] WITH SCHEMABINDING AS
SELECT linha.Id,
       linha.EmpresaId,
       linha.Prefixo,
       linha.Denominacao
FROM opc.Linhas linha
GO
/****** Object:  View [opc].[FrotaEtaria]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[FrotaEtaria] WITH SCHEMABINDING AS
SELECT pvt.EmpresaId,
       pvt.EtariaId,       
       [1] Micro,
       [2] Mini,
       [3] Midi,
       [4] Basico,
       [5] Padron,
       [6] Especial,
       [7] Articulado,
       [8] BiArticulado,       
       coalesce([1], 0) + coalesce([2], 0) + coalesce([3], 0) + 
         coalesce([4], 0) + coalesce([5], 0) + coalesce([6], 0) + 
         coalesce([7], 0) + coalesce([8], 0) Frota,
       (coalesce([1], 0) + coalesce([2], 0) + coalesce([3], 0) + 
         coalesce([4], 0) + coalesce([5], 0) + coalesce([6], 0) + 
         coalesce([7], 0) + coalesce([8], 0)) / 
         convert(NUMERIC, (SELECT count(_carro.Id)
                           FROM opc.Veiculos _carro
                           WHERE _carro.EmpresaId = pvt.EmpresaId AND
                                 coalesce(_carro.Inativo, 0) = 0)) Ratio,
       (SELECT fx.Minimo 
        FROM   opc.FxEtarias fx 
        WHERE  fx.Id = pvt.EtariaId) * (coalesce([1], 0) + coalesce([2], 0) + 
                                        coalesce([3], 0) + coalesce([4], 0) + 
                                        coalesce([5], 0) + coalesce([6], 0) + 
                                        coalesce([7], 0) + coalesce([8], 0)) EqvIdade
FROM (
    SELECT _fonte.EmpresaId,
           _fonte.EtariaId,           
           _fonte.Classe,
           count(_fonte.Id) Carros
    FROM (
        SELECT carro.Id,
               carro.EmpresaId,
               carro.Classe,
               (SELECT min(f.Id)
                FROM   opc.FxEtarias f
                WHERE  (year(getDate()) - coalesce(chassi.Ano, year(carro.Inicio))) >= f.Minimo AND 
                       (year(getDate()) - coalesce(chassi.Ano, year(carro.Inicio))) < f.Maximo) EtariaId
        FROM  opc.Veiculos carro
        INNER JOIN opc.Chassis chassi ON chassi.VeiculoId = carro.Id
        WHERE carro.Inativo = 0) _fonte
    GROUP BY _fonte.EmpresaId, _fonte.EtariaId, _fonte.Classe) fonte
PIVOT (sum(fonte.Carros) FOR fonte.Classe IN ([1], [2], [3], [4], [5], [6], [7], [8])) pvt
WHERE (coalesce([1], 0) + coalesce([2], 0) + coalesce([3], 0) + coalesce([4], 0) + 
       coalesce([5], 0) + coalesce([6], 0) + coalesce([7], 0) + coalesce([8], 0)) > 0;
GO
/****** Object:  View [opc].[FrotaEtariaCat]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[FrotaEtariaCat] WITH SCHEMABINDING AS
SELECT pvt.EmpresaId,
       pvt.EtariaId,       
       [1] Leve,
       [2] Pesado,
       [3] Especial,
       coalesce([1], 0) + coalesce([2], 0) + coalesce([3], 0) Frota,
       (SELECT fx.Minimo 
        FROM   opc.FxEtarias fx 
        WHERE  fx.Id = pvt.EtariaId) * 
            (coalesce([1], 0) + coalesce([2], 0) + coalesce([3], 0)) EqvIdade
FROM (
    SELECT _fonte.EmpresaId,
           _fonte.EtariaId,           
           _fonte.Categoria,
           count(_fonte.Id) Carros
    FROM (
        SELECT carro.Id,
               carro.EmpresaId,
               carro.Categoria,
               (SELECT min(f.Id)
                FROM   opc.FxEtarias f
                WHERE  (year(getDate()) - coalesce(chassi.Ano, year(carro.Inicio))) >= f.Minimo AND 
                       (year(getDate()) - coalesce(chassi.Ano, year(carro.Inicio))) < f.Maximo) EtariaId
        FROM  opc.Veiculos carro
        INNER JOIN opc.Chassis chassi ON chassi.VeiculoId = carro.Id
        WHERE carro.Inativo = 0) _fonte
    GROUP BY _fonte.EmpresaId, _fonte.EtariaId, _fonte.Categoria) fonte
PIVOT (sum(fonte.Carros) FOR fonte.Categoria IN ([1], [2], [3])) pvt
WHERE (coalesce([1], 0) + coalesce([2], 0) + coalesce([3], 0)) > 0;
GO
/****** Object:  View [opc].[ItinerariosDistinct]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[ItinerariosDistinct] WITH SCHEMABINDING AS
SELECT
DISTINCT Linhas.EmpresaId,
         it.LinhaId,
         NULL AtendimentoId,
         Linhas.Prefixo,
         it.Sentido
FROM opc.Itinerarios it
INNER JOIN opc.Linhas ON Linhas.Id = it.LinhaId

UNION ALL
SELECT
DISTINCT Linhas.EmpresaId,
         Atendimentos.LinhaId,
         iat.AtendimentoId,
         Atendimentos.Prefixo,
         iat.Sentido
FROM opc.ItAtendimentos iat 
INNER JOIN opc.Atendimentos ON Atendimentos.Id = iat.AtendimentoId
  INNER JOIN opc.Linhas ON Linhas.Id = Atendimentos.LinhaId;
GO
/****** Object:  View [opc].[ListItinerarios]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[ListItinerarios] WITH SCHEMABINDING AS
SELECT 
DISTINCT fonte.EmpresaId,
         fonte.Percurso
FROM (
  SELECT 
  DISTINCT Linhas.EmpresaId,
           Itinerarios.Percurso
  FROM opc.Itinerarios
  INNER JOIN opc.Linhas ON Linhas.Id = Itinerarios.LinhaId
  
  UNION ALL
  SELECT
  DISTINCT Linhas.EmpresaId,
           ItAtendimentos.Percurso
  FROM opc.ItAtendimentos
  INNER JOIN opc.Atendimentos ON Atendimentos.Id = ItAtendimentos.AtendimentoId
    INNER JOIN opc.Linhas ON Linhas.Id = Atendimentos.LinhaId) fonte
GO
/****** Object:  View [opc].[Lotacoes]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[Lotacoes] WITH SCHEMABINDING AS
SELECT cv.Id ClasseId,
       cv.Classe,
       os.Id OcupacaoId,
       os.Denominacao,
       convert(INT, floor(cv.Minimo * os.Densidade)) Lotacao
FROM   opc.CVeiculos cv, opc.Ocupacoes os
GO
/****** Object:  View [opc].[PartidasHorario]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[PartidasHorario] WITH SCHEMABINDING AS
SELECT fonte.EmpresaId,
       fonte.DiaId,
       fonte.HoraFixa,
       timeFromParts(fonte.HoraFixa, 0, 0, 0, 0) Inicio,
       timeFromParts(fonte.HoraFixa, 59, 0, 0, 0) Termino,
       count(DISTINCT fonte.LinhaId) QtdLinhas,
       sum(fonte.Viagens) Viagens,
       sum(fonte.Veiculos) Veiculos
FROM (
  SELECT _fonte.EmpresaId,
         _fonte.DiaId,         
         _fonte.LinhaId,
         _fonte.HoraFixa,
         _fonte.Viagens,
         CASE 
           WHEN (_fonte.ViagemAB > _fonte.ViagemBA) THEN
             CASE
               WHEN ((_fonte.ViagemAB / (convert(NUMERIC, 60) / _fonte.Viagens)) < _fonte.Viagens) THEN
                 ceiling(_fonte.ViagemAB / (convert(NUMERIC, 60) / _fonte.Viagens))
               ELSE 
                 _fonte.Viagens
             END
           ELSE
             CASE
               WHEN ((_fonte.ViagemBA / (convert(NUMERIC, 60) / _fonte.Viagens)) < _fonte.Viagens) THEN
                 ceiling(_fonte.ViagemBA / (convert(NUMERIC, 60) / _fonte.Viagens))
               ELSE
                 _fonte.Viagens
             END
         END Veiculos       
  FROM (
    SELECT linha.EmpresaId,
           hora.LinhaId,
           hora.DiaId,
           datePart(hour, hora.Inicio) HoraFixa,
           (SELECT coalesce(hFaixa.CicloAB, 0)
            FROM opc.PrLinhas hFaixa
            WHERE hFaixa.Id = (SELECT min(_faixa.Id)
                               FROM opc.PrLinhas _faixa
                               WHERE _faixa.LinhaId = hora.LinhaId AND _faixa.DiaId = hora.DiaId AND
                                     _faixa.Inicio <= timeFromParts(datePart(hour, hora.Inicio), 0, 0, 0, 0) AND
                                     _faixa.Termino >= timeFromParts(datePart(hour, hora.Inicio), 59, 0, 0, 0))) ViagemAB,
           (SELECT coalesce(hFaixa.CicloBA, 0)
            FROM opc.PrLinhas hFaixa
            WHERE hFaixa.Id = (SELECT min(_faixa.Id)
                               FROM opc.PrLinhas _faixa
                               WHERE _faixa.LinhaId = hora.LinhaId AND _faixa.DiaId = hora.DiaId AND
                                     _faixa.Inicio <= timeFromParts(datePart(hour, hora.Inicio), 0, 0, 0, 0) AND
                                     _faixa.Termino >= timeFromParts(datePart(hour, hora.Inicio), 59, 0, 0, 0))) ViagemBA,
           count(hora.Inicio) Viagens
  FROM opc.Horarios hora
  INNER JOIN opc.Linhas linha ON linha.Id = hora.LinhaId
  GROUP BY linha.EmpresaId, hora.LinhaId, hora.DiaId, datePart(hour, hora.Inicio)) _fonte) fonte
GROUP BY fonte.EmpresaId, fonte.DiaId, fonte.HoraFixa;
GO
/****** Object:  View [opc].[PassageirosMensais]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[PassageirosMensais] WITH SCHEMABINDING AS
SELECT pvt.EmpresaId,
       pvt.Ano,
       pvt.Mes,
       [5] ComCartao,
       [6] SemCartao,
       [7] Estudante,
       [8] Professor,
       [9] ValeTransporte,
       coalesce([5], 0) + coalesce([6], 0) + coalesce([7], 0) +
         coalesce([8], 0) + coalesce([9], 0) Demanda,
       coalesce([5], 0) + floor(coalesce([6], 0) * 1.09) + 
         floor(coalesce([7], 0) * 0.5) + floor(coalesce([8], 0) * 0.75) + 
         coalesce([9], 0) DemandaEqv       
FROM (SELECT p.EmpresaId,
             p.Ano,
             p.Mes,
             p.TarifariaId,
             p.Passageiros
      FROM opc.Producao p
      INNER JOIN opc.TCategorias t ON t.Id = p.TarifariaId
      WHERE t.EmpresaId = 5 AND t.Gratuidade = 0) fonte
PIVOT (sum(fonte.Passageiros) FOR fonte.TarifariaId IN ([5], [6], [7], [8], [9])) pvt
GO
/****** Object:  View [opc].[PercursoDiario]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[PercursoDiario] WITH SCHEMABINDING AS
SELECT Linhas.EmpresaId,
       fonte.LinhaId,
       'Principal' Tipo,
       Linhas.Prefixo,
       Linhas.Denominacao,
       CASE
         WHEN (fonte.DiaId = 1) THEN '1. Útil'
         WHEN (fonte.DiaId = 2) THEN '2. Sáb'
         WHEN (fonte.DiaId = 3) THEN '3. Dom'
       END DiaOperacao,
       sum(fonte.ViagensAB) * Linhas.ExtensaoAB PercursoAB,
       sum(fonte.ViagensBA) * Linhas.ExtensaoBA PercursoBA
FROM (
  SELECT hr.LinhaId,
         hr.DiaId,
         hr.Sentido,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN count(hr.Inicio) 
           WHEN (hr.Sentido = 'BA') THEN NULL
         END ViagensAB,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN NULL
           WHEN (hr.Sentido = 'BA') THEN count(hr.Inicio) 
         END ViagensBA                     
  FROM opc.Horarios hr
  WHERE hr.AtendimentoId IS NULL
  GROUP BY hr.LinhaId, hr.DiaId, hr.Sentido) fonte
INNER JOIN opc.Linhas ON Linhas.Id = fonte.LinhaId
GROUP BY Linhas.EmpresaId, Fonte.LinhaId, Linhas.Prefixo, 
         Linhas.Denominacao, fonte.DiaId, Linhas.ExtensaoAB, Linhas.ExtensaoBA

UNION ALL
SELECT Linhas.EmpresaId,
       fonte.AtendimentoId,
       'Atendimento' Tipo,
       atende.Prefixo,
       atende.Denominacao,
       CASE
         WHEN (fonte.DiaId = 1) THEN '1. Útil'
         WHEN (fonte.DiaId = 2) THEN '2. Sáb'
         WHEN (fonte.DiaId = 3) THEN '3. Dom'
       END DiaOperacao,
       sum(fonte.ViagensAB) * atende.ExtensaoAB PercursoAB,
       sum(fonte.ViagensBA) * atende.ExtensaoBA PercursoBA
FROM (
  SELECT hr.AtendimentoId,
         hr.DiaId,
         hr.Sentido,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN count(hr.Inicio) 
           WHEN (hr.Sentido = 'BA') THEN NULL
         END ViagensAB,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN NULL
           WHEN (hr.Sentido = 'BA') THEN count(hr.Inicio) 
         END ViagensBA  
  FROM opc.Horarios hr
  WHERE hr.AtendimentoId IS NOT NULL
  GROUP BY hr.AtendimentoId, hr.DiaId, hr.Sentido) fonte
INNER JOIN opc.Atendimentos atende ON atende.Id = fonte.AtendimentoId
  INNER JOIN opc.Linhas ON Linhas.Id = atende.LinhaId
GROUP BY Linhas.EmpresaId, fonte.AtendimentoId, atende.Prefixo,
         atende.Denominacao, fonte.DiaId, atende.ExtensaoAB, atende.ExtensaoBA;
GO
/****** Object:  View [opc].[PeriodosTipicos]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[PeriodosTipicos] WITH SCHEMABINDING AS
SELECT fonte.Id,
       periodo.LinhaId,
       periodo.DiaId,
       periodo.PeriodoId,
       fonte.Inicio,
       CASE
         WHEN (fonte.QtdViagens > 1) THEN
           fonte.Termino
         ELSE
           CASE
             WHEN (coalesce(periodo.CicloAB, 0) > coalesce(periodo.CicloBA, 0)) THEN
               dateAdd(minute, coalesce(periodo.CicloAB, 0), fonte.Termino)
             ELSE
               dateAdd(minute, coalesce(periodo.CicloBA, 0), fonte.Termino)
           END
       END Termino,
       fonte.QtdViagens,
       periodo.CicloAB,
       periodo.CicloBA
FROM (
  SELECT tipico.Id,
         (SELECT min(hr.Inicio)
          FROM opc.Horarios hr
          WHERE hr.LinhaId = tipico.LinhaId AND hr.DiaId = tipico.DiaId AND
                (hr.Inicio >= tipico.Inicio AND hr.Inicio <= tipico.Termino)) Inicio,
         (SELECT max(hr.Inicio)
          FROM opc.Horarios hr
          WHERE hr.LinhaId = tipico.LinhaId AND hr.DiaId = tipico.DiaId AND
                (hr.Inicio >= tipico.Inicio AND hr.Inicio <= tipico.Termino)) Termino,
         (SELECT count(hr.Inicio)
          FROM opc.Horarios hr
          WHERE hr.LinhaId = tipico.LinhaId AND hr.DiaId = tipico.DiaId AND
                (hr.Inicio >= tipico.Inicio AND hr.Inicio <= tipico.Termino)) QtdViagens                
  FROM opc.PrLinhas tipico) fonte
INNER JOIN opc.PrLinhas periodo ON periodo.Id = fonte.Id
WHERE fonte.Inicio IS NOT NULL AND fonte.Termino IS NOT NULL;
GO
/****** Object:  View [opc].[ProducaoMedia]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[ProducaoMedia] WITH SCHEMABINDING AS
SELECT fonte.EmpresaId,
       fonte.Ano,
       fonte.TarifariaId,
       sum(fonte.Passageiros) Passageiros,
       convert(INT, ceiling(avg(fonte.Passageiros))) Mensal,
       sum(fonte.Equivalente) Equivalente,
       convert(INT, ceiling(avg(fonte.Equivalente))) MensalEqv,
       convert(NUMERIC, sum(fonte.Passageiros)) / 
           (SELECT sum(prd.Passageiros)
            FROM opc.Producao prd
            WHERE prd.EmpresaId = fonte.EmpresaId AND 
                  prd.Ano = fonte.Ano) Ratio
FROM (
  SELECT Producao.EmpresaId,
         Producao.Ano,
         Producao.Mes,
         Producao.TarifariaId,
         sum(Producao.Passageiros) Passageiros,
         ceiling(sum(
             Producao.Passageiros * coalesce(TCategorias.Rateio, 0) * 0.01)
         ) Equivalente
  FROM opc.Producao
  INNER JOIN opc.TCategorias ON TCategorias.Id = Producao.TarifariaId
  GROUP BY Producao.EmpresaId, Producao.Ano, 
           Producao.Mes, Producao.TarifariaId) fonte
GROUP BY fonte.EmpresaId, fonte.Ano, fonte.TarifariaId;
GO
/****** Object:  View [opc].[ProducaoMediaEqv]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[ProducaoMediaEqv] WITH SCHEMABINDING AS
SELECT fonte.EmpresaId,
       fonte.Ano,
       sum(fonte.Passageiros) Passageiros,
       avg(fonte.Passageiros) Mensal,
       sum(fonte.Equivalente) Equivalente,
       convert(INT, avg(fonte.Equivalente)) MensalEqv,
       sum(fonte.Equivalente) / sum(fonte.Passageiros) Equivalencia
FROM (SELECT _p.EmpresaId, 
             _p.Ano,
             _p.Passageiros,
             ceiling(_p.Passageiros * (coalesce(_t.Rateio, 0) * 0.01)) Equivalente
      FROM opc.Producao _p
      INNER JOIN opc.TCategorias _t ON _t.Id = _p.TarifariaId) fonte
GROUP BY fonte.EmpresaId, fonte.Ano;
GO
/****** Object:  View [opc].[ProducaoMensalEqv]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[ProducaoMensalEqv] WITH SCHEMABINDING AS
SELECT fonte.EmpresaId,
       fonte.Ano,
       fonte.Mes,
       sum(fonte.Passageiros) Passageiros,
       avg(fonte.Passageiros) Mensal,
       sum(fonte.Equivalente) Equivalente,
       convert(INT, avg(fonte.Equivalente)) MensalEqv,
       sum(fonte.Equivalente) / sum(fonte.Passageiros) Equivalencia
FROM (SELECT _p.EmpresaId, 
             _p.Ano,
             _p.Mes,
             _p.Passageiros,
             ceiling(_p.Passageiros * (coalesce(_t.Rateio, 0) * 0.01)) Equivalente
      FROM opc.Producao _p
      INNER JOIN opc.TCategorias _t ON _t.Id = _p.TarifariaId) fonte
GROUP BY fonte.EmpresaId, fonte.Ano, fonte.Mes;
GO
/****** Object:  View [opc].[ProducaoRatio]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[ProducaoRatio] WITH SCHEMABINDING AS
SELECT prd.EmpresaId,       
       prd.Ano,
       prd.Mes,
       prd.TarifariaId,
       sum(prd.Passageiros) Passageiros,
       convert(NUMERIC, sum(prd.Passageiros)) / 
           (SELECT sum(p.Passageiros)
            FROM opc.Producao p
            WHERE p.EmpresaId = prd.EmpresaId AND 
                  p.Ano = prd.Ano AND p.Mes = prd.Mes) Ratio
FROM opc.Producao prd
GROUP BY prd.EmpresaId, prd.Ano, prd.Mes, prd.TarifariaId;
GO
/****** Object:  View [opc].[PtDestino]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[PtDestino] WITH SCHEMABINDING AS
SELECT     PtLinhas.Id,
           PtLinhas.LinhaId,
           PtLinhas.Sentido,
           PtLinhas.PontoId,
           Pontos.Prefixo,
           Pontos.Identificacao
FROM       opc.PtLinhas 
INNER JOIN opc.Pontos ON Pontos.Id = PtLinhas.PontoId
WHERE NOT EXISTS (SELECT 1
                  FROM   opc.PtLinhas lpt
                  WHERE  lpt.Id = PtLinhas.DestinoId);
GO
/****** Object:  View [opc].[PtOrigem]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[PtOrigem] WITH SCHEMABINDING AS
SELECT PtLinhas.Id,
       PtLinhas.LinhaId,
       PtLinhas.Sentido,
       PtLinhas.PontoId,
       Pontos.Prefixo,
       Pontos.Identificacao
FROM   opc.PtLinhas 
INNER JOIN opc.Pontos ON Pontos.Id = PtLinhas.PontoId
WHERE NOT EXISTS (SELECT 1
                  FROM   opc.PtLinhas lpt
                  WHERE  lpt.Id = PtLinhas.OrigemId);
GO
/****** Object:  View [opc].[SinteticoLinhas]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[SinteticoLinhas] WITH SCHEMABINDING AS
SELECT linha.EmpresaId,
       linha.Prefixo,
       linha.Denominacao,
       ClassLinhas.Denominacao Classificacao,
       'Principal' Tipo,
       linha.ExtensaoAB,
       linha.ExtensaoBA,
       coalesce(linha.ExtensaoAB, 0) + coalesce(linha.ExtensaoBA, 0) Extensao
FROM opc.Linhas linha
INNER JOIN opc.CLinhas cl ON cl.EmpresaId = linha.EmpresaId AND
                             cl.Id = linha.Classificacao
    INNER JOIN dbo.ClassLinhas ON ClassLinhas.Id = cl.ClassLinhaId
    
UNION ALL
SELECT linha.EmpresaId,
       atende.Prefixo,
       atende.Denominacao,
       ClassLinhas.Denominacao Classificacao,
       'Atendimento' Tipo,
       atende.ExtensaoAB,
       atende.ExtensaoBA,
       coalesce(atende.ExtensaoAB, 0) + coalesce(atende.ExtensaoBA, 0) Extensao
FROM opc.Atendimentos atende
INNER JOIN opc.Linhas Linha ON atende.LinhaId = linha.Id 
    INNER JOIN opc.CLinhas cl ON cl.EmpresaId = linha.EmpresaId AND
                                 cl.Id = linha.Classificacao
        INNER JOIN dbo.ClassLinhas ON ClassLinhas.Id = cl.ClassLinhaId;
GO
/****** Object:  View [opc].[SpecVeiculos]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[SpecVeiculos] WITH SCHEMABINDING AS
SELECT carros.Id,
       carros.EmpresaId,
       carros.Numero,
       carros.Cor,
       carros.Classe,
       carros.Categoria,
       carros.Placa,
       carros.Renavam,
       carros.Antt,
       carros.Inicio,
       
       chassi.Fabricante ChassiFabricante,
       chassi.Modelo ChassiModelo,
       chassi.ChassiNo,
       chassi.Ano ChassiAno,
       chassi.Aquisicao ChassiAquisicao,
       chassi.Fornecedor ChassiFornecedor,
       chassi.NotaFiscal ChassiNota,
       chassi.Valor ChassiValor,
       chassi.ChaveNfe ChassiChaveNfe,
       chassi.MotorId,
       chassi.Potencia,
       chassi.PosMotor,
       chassi.EixosFrente,
       chassi.EixosTras,
       chassi.PneusFrente,
       chassi.PneusTras,
       chassi.TransmiteId,
       chassi.DirecaoId,
       
       carroca.Fabricante CarroceriaFabricante,
       carroca.Modelo CarroceriaModelo,
       carroca.Referencia,
       carroca.Ano CarroceriaAno,
       carroca.Aquisicao CarroceriaAquisicao,
       carroca.Fornecedor CarroceriaFornecedor,
       carroca.NotaFiscal CarroceriaNota,
       carroca.Valor CarroceriaValor,
       carroca.ChaveNfe CarroceriaChaveNfe,
       carroca.Encarrocamento,
       carroca.QuemEncarroca,
       carroca.NotaEncarroca,
       carroca.ValorEncarroca,
       carroca.Portas,
       carroca.Assentos,
       carroca.Capacidade,
       carroca.Piso,
       carroca.EscapeV,
       carroca.EscapeH,
       carroca.Catraca,
       carroca.PortaIn,
       carroca.SaidaFrente,
       carroca.SaidaMeio,
       carroca.SaidaTras,
       
       (SELECT min(f.Id)
        FROM   opc.FxEtarias f
        WHERE  (year(getDate()) - coalesce(chassi.Ano, year(carros.Inicio))) >= f.Minimo AND 
               (year(getDate()) - coalesce(chassi.Ano, year(carros.Inicio))) < f.Maximo) EtariaId,
       year(getDate()) - coalesce(chassi.Ano, year(carros.Inicio)) Idade,
       carros.Cadastro 
FROM   opc.Veiculos carros
LEFT JOIN opc.Chassis chassi ON chassi.VeiculoId = carros.Id 
LEFT JOIN opc.Carrocerias carroca ON carroca.VeiculoId = carros.Id
WHERE  carros.Inativo = 0;
GO
/****** Object:  View [opc].[TarifaMod]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[TarifaMod] WITH SCHEMABINDING AS
SELECT cat.Id,
       cat.EmpresaId,
       cat.Denominacao,
       cat.Gratuidade,
       cat.Rateio,
       round((SELECT max(tar.Valor)
              FROM   opc.Tarifas tar
              WHERE  tar.EmpresaId = cat.EmpresaId AND
                     tar.Referencia = (SELECT max(_tar.Referencia)
                                       FROM   opc.Tarifas _tar
                                       WHERE  _tar.EmpresaId = tar.EmpresaId)) * 
                (cat.Rateio *  0.01), 2, 1) Tarifa,
       cat.Cadastro
FROM   opc.TCategorias cat
GO
/****** Object:  View [opc].[TotalViagens]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[TotalViagens] WITH SCHEMABINDING AS
SELECT   hr.LinhaId,
         hr.DiaId,         
         hr.PeriodoId,
         hr.Sentido,
         (SELECT pr.Inicio
          FROM   opc.PrLinhas pr
          WHERE  pr.Id = hr.PeriodoId) Inicio,
         (SELECT pr.Termino
          FROM   opc.PrLinhas pr
          WHERE  pr.Id = hr.PeriodoId) Termino,          
         (SELECT coalesce(pr.CicloAB, 0) + coalesce(pr.CicloBA, 0)
          FROM   opc.PrLinhas pr
          WHERE  pr.Id = hr.PeriodoId) Ciclo,
         count(hr.Inicio) QtdViagens,
         nullIf(count(hr.AtendimentoId), 0) QtdAtendimentos,
         CASE
           WHEN (hr.DiaId = 1) THEN
             count(hr.Inicio) * 
                 (SELECT CASE
                           WHEN (hr.Sentido = 'AB') THEN ln.ExtensaoAB
                           ELSE ln.ExtensaoBA
                         END
                  FROM   opc.Linhas ln
                  WHERE  ln.Id = hr.LinhaId)
           ELSE NULL
         END KmDia,
         CASE
           WHEN (hr.DiaId = 1) THEN
             (count(hr.Inicio) * 5) * 
                 (SELECT CASE
                           WHEN (hr.Sentido = 'AB') THEN ln.ExtensaoAB
                           ELSE ln.ExtensaoBA
                         END
                  FROM   opc.Linhas ln
                  WHERE  ln.Id = hr.LinhaId)
           ELSE 
             count(hr.Inicio) *
                 (SELECT CASE
                           WHEN (hr.Sentido = 'AB') THEN ln.ExtensaoAB
                           ELSE ln.ExtensaoBA
                         END
                  FROM   opc.Linhas ln
                  WHERE  ln.Id = hr.LinhaId)
         END KmSemana,
         CASE
           WHEN (hr.DiaId = 1) THEN
             (convert(NUMERIC, 52) / 12) * ((count(hr.Inicio) * 5) * 
                 (SELECT CASE
                           WHEN (hr.Sentido = 'AB') THEN ln.ExtensaoAB
                           ELSE ln.ExtensaoBA
                         END
                  FROM   opc.Linhas ln
                  WHERE  ln.Id = hr.LinhaId))
           ELSE
             (convert(NUMERIC, 52) / 12) * (count(hr.Inicio) * 
                 (SELECT CASE
                           WHEN (hr.Sentido = 'AB') THEN ln.ExtensaoAB
                           ELSE ln.ExtensaoBA
                         END
                         FROM   opc.Linhas ln
                         WHERE  ln.Id = hr.LinhaId))
         END KmMes         
FROM     opc.Horarios hr
GROUP BY hr.LinhaId, hr.DiaId, hr.PeriodoId, hr.Sentido;
GO
/****** Object:  View [opc].[ViagensDiarias]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[ViagensDiarias] WITH SCHEMABINDING AS
SELECT Linhas.EmpresaId,
       fonte.LinhaId,
       'Principal' Tipo,
       Linhas.Prefixo,
       Linhas.Denominacao,
       CASE
         WHEN (fonte.DiaId = 1) THEN '1. Útil'
         WHEN (fonte.DiaId = 2) THEN '2. Sáb'
         WHEN (fonte.DiaId = 3) THEN '3. Dom'
       END DiaOperacao,
       sum(fonte.ViagensAB) ViagensAB,
       sum(fonte.ViagensBA) ViagensBA
FROM (
  SELECT hr.LinhaId,
         hr.DiaId,
         hr.Sentido,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN count(hr.Inicio) 
           WHEN (hr.Sentido = 'BA') THEN NULL
         END ViagensAB,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN NULL
           WHEN (hr.Sentido = 'BA') THEN count(hr.Inicio) 
         END ViagensBA                     
  FROM opc.Horarios hr
  WHERE hr.AtendimentoId IS NULL
  GROUP BY hr.LinhaId, hr.DiaId, hr.Sentido) fonte
INNER JOIN opc.Linhas ON Linhas.Id = fonte.LinhaId
GROUP BY Linhas.EmpresaId, Fonte.LinhaId, Linhas.Prefixo, 
         Linhas.Denominacao, fonte.DiaId

UNION ALL
SELECT Linhas.EmpresaId,
       fonte.AtendimentoId,
       'Atendimento' Tipo,
       atende.Prefixo,
       atende.Denominacao,
       CASE
         WHEN (fonte.DiaId = 1) THEN '1. Útil'
         WHEN (fonte.DiaId = 2) THEN '2. Sáb'
         WHEN (fonte.DiaId = 3) THEN '3. Dom'
       END DiaOperacao,
       sum(fonte.ViagensAB) ViagensAB,
       sum(fonte.ViagensBA) ViagensBA   
FROM (
  SELECT hr.AtendimentoId,
         hr.DiaId,
         hr.Sentido,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN count(hr.Inicio) 
           WHEN (hr.Sentido = 'BA') THEN NULL
         END ViagensAB,
         CASE 
           WHEN (hr.Sentido = 'AB') THEN NULL
           WHEN (hr.Sentido = 'BA') THEN count(hr.Inicio) 
         END ViagensBA  
  FROM opc.Horarios hr
  WHERE hr.AtendimentoId IS NOT NULL
  GROUP BY hr.AtendimentoId, hr.DiaId, hr.Sentido) fonte
INNER JOIN opc.Atendimentos atende ON atende.Id = fonte.AtendimentoId
  INNER JOIN opc.Linhas ON Linhas.Id = atende.LinhaId
GROUP BY Linhas.EmpresaId, fonte.AtendimentoId, atende.Prefixo,
         atende.Denominacao, fonte.DiaId;
GO
/****** Object:  View [opc].[ViagensHora]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [opc].[ViagensHora] WITH SCHEMABINDING AS
SELECT 
DISTINCT Linhas.EmpresaId,
         hr.LinhaId,
         datePart(hour, hr.Inicio) Hora,
         (SELECT count(_hr.Inicio)
          FROM opc.Horarios _hr
          WHERE _hr.LinhaId = hr.LinhaId AND 
                datePart(hour, _hr.Inicio) = datePart(hour, hr.Inicio) AND
                _hr.DiaId = 1 AND _hr.Sentido = 'AB') UteisAB,
         (SELECT count(_hr.Inicio)
          FROM opc.Horarios _hr
          WHERE _hr.LinhaId = hr.LinhaId AND 
                datePart(hour, _hr.Inicio) = datePart(hour, hr.Inicio) AND
                _hr.DiaId = 1 AND _hr.Sentido = 'BA') UteisBA,
         (SELECT count(_hr.Inicio)
          FROM opc.Horarios _hr
          WHERE _hr.LinhaId = hr.LinhaId AND 
                datePart(hour, _hr.Inicio) = datePart(hour, hr.Inicio) AND
                _hr.DiaId = 2 AND _hr.Sentido = 'AB') SabadosAB,
         (SELECT count(_hr.Inicio)
          FROM opc.Horarios _hr
          WHERE _hr.LinhaId = hr.LinhaId AND 
                datePart(hour, _hr.Inicio) = datePart(hour, hr.Inicio) AND
                _hr.DiaId = 2 AND _hr.Sentido = 'BA') SabadosBA,                      
         (SELECT count(_hr.Inicio)
          FROM opc.Horarios _hr
          WHERE _hr.LinhaId = hr.LinhaId AND 
                datePart(hour, _hr.Inicio) = datePart(hour, hr.Inicio) AND
                _hr.DiaId = 3 AND _hr.Sentido = 'AB') DomingosAB,
         (SELECT count(_hr.Inicio)
          FROM opc.Horarios _hr
          WHERE _hr.LinhaId = hr.LinhaId AND 
                datePart(hour, _hr.Inicio) = datePart(hour, hr.Inicio) AND
                _hr.DiaId = 3 AND _hr.Sentido = 'BA') DomingosBA                                
FROM opc.Horarios hr
INNER JOIN opc.Linhas ON Linhas.Id = hr.LinhaId
GO
/****** Object:  StoredProcedure [opc].[Adiciona_PTipicos_e09]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [opc].[Adiciona_PTipicos_e09](@empresaId INT = 9) AS
BEGIN
  DECLARE
    @linhaId    INT,
    @ePeriodoId INT,
    @periodoId  INT,
    @ocupacaoId INT,
    
    @extensaoAB NUMERIC(18, 3),
    @extensaoBA NUMERIC(18, 3),
    @velocidade NUMERIC(9, 3),
    @cicloAB    NUMERIC(18, 3),
    @cicloBA    NUMERIC(18, 3),
    
    @uteis    BIT,
    @sabados  BIT,
    @domingos BIT,
    
    @hInicio  TIME,
    @hTermino TIME;
    
  DECLARE listLinhas CURSOR FOR
    SELECT Linhas.Id, Linhas.Uteis, Linhas.Sabados, Linhas.Domingos,
           Linhas.ExtensaoAB, Linhas.ExtensaoBA
    FROM opc.Linhas
    WHERE Linhas.EmpresaId = @empresaId
    ORDER BY Linhas.Id;
  
  DECLARE listPeriodos CURSOR FOR
    SELECT EPeriodos.Id, EPeriodos.PeriodoId, EPeriodos.Velocidade
    FROM opc.EPeriodos
    WHERE EPeriodos.EmpresaId = @empresaId
    ORDER BY EPeriodos.Id;
    
  OPEN listLinhas;  
  FETCH NEXT FROM listLinhas 
  INTO @linhaId, @uteis, @sabados, @domingos, @extensaoAB, @extensaoBA;
  
  WHILE (@@FETCH_STATUS = 0) BEGIN      
    OPEN listPeriodos;
    FETCH NEXT FROM listPeriodos INTO @ePeriodoId, @periodoId, @velocidade;
      
    WHILE (@@FETCH_STATUS = 0) BEGIN
      SET @ocupacaoId = 4;        
      IF (@periodoId IN (2, 6))    -- Picos
        SET @ocupacaoId = 5;
      IF (@periodoId IN (1, 7, 8)) -- Noite, Vale Noturno e Madrugada
        SET @ocupacaoId = 3;

      IF (@extensaoAB IS NOT NULL) 
        SET @cicloAB = ceiling((@extensaoAB / @velocidade) * 60)
      ELSE 
        SET @cicloAB = 0;
        
      IF (@extensaoBA IS NOT NULL) 
        SET @cicloBA = ceiling((@extensaoBA / @velocidade) * 60)
      ELSE
        SET @cicloBA = 0;
        
      IF (@periodoId = 1) BEGIN    -- Madrugada
        SET @hInicio = timeFromParts(23, 30, 0, 0, 0);
        SET @hTermino = timeFromParts(0, 59, 0, 0, 0);
      END;

      IF (@periodoId = 2) BEGIN    -- Pico da Manha
        SET @hInicio = timeFromParts(5, 0, 0, 0, 0);
        SET @hTermino = timeFromParts(7, 29, 0, 0, 0);
      END;

      IF (@periodoId = 3) BEGIN    -- Entre Pico da Manha
        SET @hInicio = timeFromParts(7, 30, 0, 0, 0);
        SET @hTermino = timeFromParts(11, 29, 0, 0, 0);
      END;

      IF (@periodoId = 4) BEGIN    -- Semi Pico do Almoco
        SET @hInicio = timeFromParts(11, 30, 0, 0, 0);
        SET @hTermino = timeFromParts(13, 29, 0, 0, 0);
      END;
     
      IF (@periodoId = 5) BEGIN    -- Entre Pico da Tarde
        SET @hInicio = timeFromParts(13, 30, 0, 0, 0);
        SET @hTermino = timeFromParts(16, 29, 0, 0, 0);
      END;

      IF (@periodoId = 6) BEGIN    -- Pico da Tarde
        SET @hInicio = timeFromParts(16, 30, 0, 0, 0);
        SET @hTermino = timeFromParts(18, 59, 0, 0, 0);
      END;

      IF (@periodoId = 7) BEGIN    -- Noite
        SET @hInicio = timeFromParts(19, 0, 0, 0, 0);
        SET @hTermino = timeFromParts(23, 29, 0, 0, 0);
      END;
      
      -- Dias Uteis
      IF (@uteis = 1) BEGIN
        INSERT INTO opc.PrLinhas (
          LinhaId, PeriodoId, DiaId, Inicio, Termino, CicloAB, CicloBA, CVeiculoId, OcupacaoId
        ) 
        VALUES (
          @linhaId, @ePeriodoId, 1, @hInicio, @hTermino, 
          nullIf(@cicloAB, 0), nullIf(@cicloBA, 0), 4, @ocupacaoId
        );
      END;
      
      -- Sabados
      IF (@sabados = 1) BEGIN
        INSERT INTO opc.PrLinhas (
          LinhaId, PeriodoId, DiaId, Inicio, Termino, CicloAB, CicloBA, CVeiculoId, OcupacaoId
        ) 
        VALUES (
          @linhaId, @ePeriodoId, 2, @hInicio, @hTermino, 
          nullIf(@cicloAB, 0), nullIf(@cicloBA, 0), 4, @ocupacaoId
        );
      END;
      
      -- Domingos
      IF (@domingos = 1) BEGIN
        INSERT INTO opc.PrLinhas (
          LinhaId, PeriodoId, DiaId, Inicio, Termino, CicloAB, CicloBA, CVeiculoId, OcupacaoId
        ) 
        VALUES (
          @linhaId, @ePeriodoId, 3, @hInicio, @hTermino, 
          nullIf(@cicloAB, 0), nullIf(@cicloBA, 0), 4, @ocupacaoId
        );
      END;
      
      FETCH NEXT FROM listPeriodos INTO @ePeriodoId, @periodoId, @velocidade;
    END;
    CLOSE listPeriodos;
    
    FETCH NEXT FROM listLinhas 
    INTO @linhaId, @uteis, @sabados, @domingos, @extensaoAB, @extensaoBA;
  END;
  CLOSE listLinhas;
  
  DEALLOCATE listPeriodos;
  DEALLOCATE listLinhas;
END;
GO
/****** Object:  StoredProcedure [opc].[Adiciona_PTipicos_e11]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [opc].[Adiciona_PTipicos_e11](@empresaId INT = 11) AS
BEGIN
  DECLARE
    @linhaId    INT,
    @ePeriodoId INT,
    @periodoId  INT,
    @ocupacaoId INT,
    
    @extensaoAB NUMERIC(18, 3),
    @extensaoBA NUMERIC(18, 3),
    @velocidade NUMERIC(9, 3),
    @cicloAB    NUMERIC(18, 3),
    @cicloBA    NUMERIC(18, 3),
    
    @uteis    BIT,
    @sabados  BIT,
    @domingos BIT,
    
    @hInicio  TIME,
    @hTermino TIME;
    
  DECLARE listLinhas CURSOR FOR
    SELECT Linhas.Id, Linhas.Uteis, Linhas.Sabados, Linhas.Domingos,
           Linhas.ExtensaoAB, Linhas.ExtensaoBA
    FROM opc.Linhas
    WHERE Linhas.EmpresaId = @empresaId
    ORDER BY Linhas.Id;
  
  DECLARE listPeriodos CURSOR FOR
    SELECT EPeriodos.Id, EPeriodos.PeriodoId, EPeriodos.Velocidade
    FROM opc.EPeriodos
    WHERE EPeriodos.EmpresaId = @empresaId
    ORDER BY EPeriodos.Id;
    
  OPEN listLinhas;  
  FETCH NEXT FROM listLinhas 
  INTO @linhaId, @uteis, @sabados, @domingos, @extensaoAB, @extensaoBA;
  
  WHILE (@@FETCH_STATUS = 0) BEGIN      
    OPEN listPeriodos;
    FETCH NEXT FROM listPeriodos INTO @ePeriodoId, @periodoId, @velocidade;
      
    WHILE (@@FETCH_STATUS = 0) BEGIN
      SET @ocupacaoId = 4;        
      IF (@periodoId IN (2, 6))    -- Picos
        SET @ocupacaoId = 5;
      IF (@periodoId IN (1, 7, 8)) -- Noite, Vale Noturno e Madrugada
        SET @ocupacaoId = 3;

      IF (@extensaoAB IS NOT NULL) 
        SET @cicloAB = ceiling((@extensaoAB / @velocidade) * 60)
      ELSE 
        SET @cicloAB = 0;
        
      IF (@extensaoBA IS NOT NULL) 
        SET @cicloBA = ceiling((@extensaoBA / @velocidade) * 60)
      ELSE
        SET @cicloBA = 0;
        
      IF (@periodoId = 1) BEGIN    -- Madrugada
        SET @hInicio = timeFromParts(2, 0, 0, 0, 0);
        SET @hTermino = timeFromParts(5, 29, 0, 0, 0);
      END;

      IF (@periodoId = 2) BEGIN    -- Pico da Manha
        SET @hInicio = timeFromParts(5, 30, 0, 0, 0);
        SET @hTermino = timeFromParts(8, 29, 0, 0, 0);
      END;

      IF (@periodoId = 3) BEGIN    -- Entre Pico da Manha
        SET @hInicio = timeFromParts(8, 30, 0, 0, 0);
        SET @hTermino = timeFromParts(11, 29, 0, 0, 0);
      END;

      IF (@periodoId = 4) BEGIN    -- Semi Pico do Almoco
        SET @hInicio = timeFromParts(11, 30, 0, 0, 0);
        SET @hTermino = timeFromParts(13, 29, 0, 0, 0);
      END;
     
      IF (@periodoId = 5) BEGIN    -- Entre Pico da Tarde
        SET @hInicio = timeFromParts(13, 30, 0, 0, 0);
        SET @hTermino = timeFromParts(16, 29, 0, 0, 0);
      END;

      IF (@periodoId = 6) BEGIN    -- Pico da Tarde
        SET @hInicio = timeFromParts(16, 30, 0, 0, 0);
        SET @hTermino = timeFromParts(19, 29, 0, 0, 0);
      END;

      IF (@periodoId = 7) BEGIN    -- Noite
        SET @hInicio = timeFromParts(19, 30, 0, 0, 0);
        SET @hTermino = timeFromParts(22, 59, 0, 0, 0);
      END;

      IF (@periodoId = 8) BEGIN    -- Vale Noturno
        SET @hInicio = timeFromParts(23, 0, 0, 0, 0);
        SET @hTermino = timeFromParts(1, 59, 0, 0, 0);
      END;
      
      -- Dias Uteis
      IF (@uteis = 1) BEGIN
        INSERT INTO opc.PrLinhas (
          LinhaId, PeriodoId, DiaId, Inicio, Termino, CicloAB, CicloBA, CVeiculoId, OcupacaoId
        ) 
        VALUES (
          @linhaId, @ePeriodoId, 1, @hInicio, @hTermino, 
          nullIf(@cicloAB, 0), nullIf(@cicloBA, 0), 4, @ocupacaoId
        );
      END;
      
      -- Sabados
      IF (@sabados = 1) BEGIN
        INSERT INTO opc.PrLinhas (
          LinhaId, PeriodoId, DiaId, Inicio, Termino, CicloAB, CicloBA, CVeiculoId, OcupacaoId
        ) 
        VALUES (
          @linhaId, @ePeriodoId, 2, @hInicio, @hTermino, 
          nullIf(@cicloAB, 0), nullIf(@cicloBA, 0), 4, @ocupacaoId
        );
      END;
      
      -- Domingos
      IF (@domingos = 1) BEGIN
        INSERT INTO opc.PrLinhas (
          LinhaId, PeriodoId, DiaId, Inicio, Termino, CicloAB, CicloBA, CVeiculoId, OcupacaoId
        ) 
        VALUES (
          @linhaId, @ePeriodoId, 3, @hInicio, @hTermino, 
          nullIf(@cicloAB, 0), nullIf(@cicloBA, 0), 4, @ocupacaoId
        );
      END;
      
      FETCH NEXT FROM listPeriodos INTO @ePeriodoId, @periodoId, @velocidade;
    END;
    CLOSE listPeriodos;
    
    FETCH NEXT FROM listLinhas 
    INTO @linhaId, @uteis, @sabados, @domingos, @extensaoAB, @extensaoBA;
  END;
  CLOSE listLinhas;
  
  DEALLOCATE listPeriodos;
  DEALLOCATE listLinhas;
END;
GO
/****** Object:  StoredProcedure [opc].[Distribuicao_Passageiros]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [opc].[Distribuicao_Passageiros](@year INT) AS
BEGIN
  DECLARE @ano   INT,
          @mes   INT,
          @catId INT,
          @total INT,
          
          @coId  INT,
          @linha INT,
          @vgSum INT,
          @qtd   INT,
          @soma  INT,          
          @ratio NUMERIC(30, 12);
          
  DECLARE producao CURSOR FOR
    SELECT prd.Ano,
           prd.Mes,
           prd.TarifariaId,
           sum(prd.Passageiros) Passageiros
    FROM opc.Producao prd
    WHERE prd.EmpresaId = 7 AND prd.Ano = @year
    GROUP BY prd.Ano, prd.Mes, prd.TarifariaId;
    
  IF (@year < 2018) BEGIN
    DECLARE distribuicao CURSOR FOR
      SELECT fonte.EmpresaId,
             fonte.LinhaId,
             fonte.Viagens,
             convert(NUMERIC, fonte.Viagens) / nullIf(
             (SELECT 
                coalesce(opc.getWorkDays(1, year(getDate())) * sum(p.ViagensUtil), 0) +
                coalesce(opc.getWorkDays(2, year(getDate())) * sum(p.ViagensSab), 0) +
                coalesce(opc.getWorkDays(3, year(getDate())) * sum(p.ViagensDom), 0)
              FROM opc.Planos p
              INNER JOIN opc.Linhas ON Linhas.Id = p.LinhaId
              WHERE Linhas.EmpresaId = fonte.EmpresaId AND
                    NOT p.LinhaId IN (255, 257, 261, 263)), 0) Ratio         
      FROM (
          SELECT Linhas.EmpresaId,
                 Planos.LinhaId,
                 coalesce(opc.getWorkDays(1, year(getDate())) * sum(Planos.ViagensUtil), 0) +
                 coalesce(opc.getWorkDays(2, year(getDate())) * sum(Planos.ViagensSab), 0) +
                 coalesce(opc.getWorkDays(3, year(getDate())) * sum(Planos.ViagensDom), 0) Viagens
          FROM opc.Planos
          INNER JOIN opc.Linhas ON Linhas.Id = Planos.LinhaId
          WHERE Linhas.EmpresaId = 7 AND 
                NOT Planos.LinhaId IN (255, 257, 261, 263)
          GROUP BY Linhas.EmpresaId, Planos.LinhaId) fonte;  
  END
  ELSE BEGIN
    DECLARE distribuicao CURSOR FOR
      SELECT fonte.EmpresaId,
             fonte.LinhaId,
             fonte.Viagens,
             convert(NUMERIC, fonte.Viagens) / nullIf(
             (SELECT 
                coalesce(opc.getWorkDays(1, year(getDate())) * sum(p.ViagensUtil), 0) +
                coalesce(opc.getWorkDays(2, year(getDate())) * sum(p.ViagensSab), 0) +
                coalesce(opc.getWorkDays(3, year(getDate())) * sum(p.ViagensDom), 0)
              FROM opc.Planos p
              INNER JOIN opc.Linhas ON Linhas.Id = p.LinhaId
              WHERE Linhas.EmpresaId = fonte.EmpresaId), 0) Ratio         
      FROM (
          SELECT Linhas.EmpresaId,
                 Planos.LinhaId,
                 coalesce(opc.getWorkDays(1, year(getDate())) * sum(Planos.ViagensUtil), 0) +
                 coalesce(opc.getWorkDays(2, year(getDate())) * sum(Planos.ViagensSab), 0) +
                 coalesce(opc.getWorkDays(3, year(getDate())) * sum(Planos.ViagensDom), 0) Viagens
          FROM opc.Planos
          INNER JOIN opc.Linhas ON Linhas.Id = Planos.LinhaId
          WHERE Linhas.EmpresaId = 7
          GROUP BY Linhas.EmpresaId, Planos.LinhaId) fonte;    
  END;
  
  OPEN producao;
  FETCH NEXT FROM producao INTO @ano, @mes, @catId, @total;
  
  WHILE (@@FETCH_STATUS = 0) BEGIN
    SET @soma = 0;
    
    OPEN distribuicao;
    FETCH NEXT FROM distribuicao INTO @coId, @linha, @vgSum, @ratio;
    
    BEGIN TRANSACTION
    WHILE (@@FETCH_STATUS = 0) BEGIN
      SET @qtd = ceiling(@total * @ratio);
      IF (@qtd > (@total - @soma))
        SET @qtd = @total - @soma;
        
      IF (@qtd > 0) BEGIN               
        INSERT INTO opc.Ofertas (LinhaId, Ano, Mes, Categoria, Passageiros)
        VALUES (@linha, @ano, @mes, @catId, @qtd);        
      END;
      SET @soma += @qtd;
      
      FETCH NEXT FROM distribuicao INTO @coId, @linha, @vgSum, @ratio;
    END;
    COMMIT TRANSACTION;
    
    CLOSE distribuicao;    
    FETCH NEXT FROM producao INTO @ano, @mes, @catId, @total;
  END;
  
  DEALLOCATE distribuicao;  
  CLOSE producao;
  DEALLOCATE producao;
END;
GO
/****** Object:  StoredProcedure [opc].[set_Viagem_Horario]    Script Date: 28.11.2019 13:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [opc].[set_Viagem_Horario] AS
BEGIN
  UPDATE opc.Viagens_temp SET
    HorarioId = (SELECT min(Horarios.Id)
                 FROM   opc.Horarios
                 WHERE  Horarios.LinhaId = (SELECT min(LnPesquisas.LinhaId)
                                            FROM LnPesquisas 
                                            WHERE LnPesquisas.Id = Viagens_temp.LinhaId) AND
                        Horarios.DiaId = opc.workDay(Viagens_temp.Data) AND
                        Horarios.Sentido = Viagens_temp.Sentido AND
                        Horarios.Inicio = Viagens_temp.Inicio)
  WHERE Viagens_temp.HorarioId IS NULL;
END;
GO
