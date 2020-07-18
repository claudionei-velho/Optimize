SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE opc.Adiciona_PTipicos_e18(@empresaId INT = 18) AS
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
        SET @hInicio = timeFromParts(4, 30, 0, 0, 0);
        SET @hTermino = timeFromParts(5, 59, 0, 0, 0);
      END;

      IF (@periodoId = 2) BEGIN    -- Pico da Manha
        SET @hInicio = timeFromParts(6, 0, 0, 0, 0);
        SET @hTermino = timeFromParts(7, 59, 0, 0, 0);
      END;

      IF (@periodoId = 3) BEGIN    -- Entre Pico da Manha
        SET @hInicio = timeFromParts(8, 0, 0, 0, 0);
        SET @hTermino = timeFromParts(10, 59, 0, 0, 0);
      END;

      IF (@periodoId = 4) BEGIN    -- Semi Pico do Almoco
        SET @hInicio = timeFromParts(11, 0, 0, 0, 0);
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
        SET @hTermino = timeFromParts(23, 59, 0, 0, 0);
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
