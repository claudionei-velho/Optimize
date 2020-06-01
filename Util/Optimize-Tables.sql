USE [Optimize]
GO
/****** Object:  Schema [nfe]    Script Date: 28.11.2019 13:51:05 ******/
CREATE SCHEMA [nfe]
GO
/****** Object:  Schema [opc]    Script Date: 28.11.2019 13:51:05 ******/
CREATE SCHEMA [opc]
GO
/****** Object:  Table [dbo].[Centros]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Centros](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Classificacao] [varchar](16) NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[VinculoId] [int] NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Centros] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cidades]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cidades](
	[Id] [int] NOT NULL,
	[UfId] [int] NOT NULL,
	[Municipio] [varchar](64) NOT NULL,
 CONSTRAINT [PK_Cidades] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassLinhas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassLinhas](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[Descricao] [varchar](512) NULL,
 CONSTRAINT [PK_ClassLinhas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contas](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Classificacao] [varchar](16) NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[VinculoId] [int] NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Contas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dominios]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dominios](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[Denominacao] [varchar](32) NOT NULL,
	[Descricao] [varchar](256) NULL,
 CONSTRAINT [PK_Dominios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EEncargos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EEncargos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[EncargoId] [int] NOT NULL,
	[Formula] [varchar](1024) NULL,
	[Coeficiente] [numeric](24, 12) NOT NULL,
	[Vigente] [bit] NOT NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_EEncargos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empresas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Razao] [varchar](64) NOT NULL,
	[Fantasia] [varchar](64) NOT NULL,
	[Cnpj] [varchar](32) NOT NULL,
	[IEstadual] [varchar](16) NULL,
	[IMunicipal] [varchar](16) NULL,
	[Endereco] [varchar](128) NOT NULL,
	[EnderecoNo] [varchar](8) NULL,
	[Complemento] [varchar](64) NULL,
	[Cep] [int] NULL,
	[Bairro] [varchar](32) NULL,
	[Municipio] [varchar](32) NOT NULL,
	[UfId] [char](2) NOT NULL,
	[PaisId] [varchar](8) NOT NULL,
	[Telefone] [varchar](32) NULL,
	[Email] [varchar](256) NULL,
	[Inicio] [time](7) NULL,
	[Termino] [time](7) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Empresas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Encargos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Encargos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Grupo] [char](1) NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[Observacao] [varchar](2048) NULL,
 CONSTRAINT [PK_Encargos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ESistemas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ESistemas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[SistemaId] [int] NOT NULL,
	[Codigo] [varchar](16) NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[ResponsavelId] [int] NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_ESistemas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [ESistemas_key] UNIQUE NONCLUSTERED 
(
	[EmpresaId] ASC,
	[SistemaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDICES]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EUsuarios]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EUsuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[UsuarioId] [int] NOT NULL,
	[Ativo] [bit] NOT NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_EUsuarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [EUsuarios_key] UNIQUE NONCLUSTERED 
(
	[EmpresaId] ASC,
	[UsuarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDICES]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feriados]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feriados](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Mes] [int] NULL,
	[Dia] [int] NULL,
	[Denominacao] [varchar](32) NOT NULL,
 CONSTRAINT [PK_Feriados] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FInstalacoes]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FInstalacoes](
	[Id] [int] NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[Descricao] [varchar](512) NULL,
 CONSTRAINT [FInstalacoes_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ISinotico]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ISinotico](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Classe] [varchar](32) NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[Unidade] [varchar](16) NULL,
 CONSTRAINT [PK_ISinotico] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OperLinhas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperLinhas](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[Descricao] [varchar](512) NULL,
 CONSTRAINT [PK_OperLinhas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Paises]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Paises](
	[Id] [varchar](8) NOT NULL,
	[Nome] [varchar](64) NOT NULL,
	[Capital] [varchar](32) NULL,
	[Continente] [varchar](32) NULL,
 CONSTRAINT [PK_Paises] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Periodos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Periodos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Denominacao] [varchar](32) NOT NULL,
 CONSTRAINT [PK_Periodos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RhIndices]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RhIndices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Indice] [varchar](8) NOT NULL,
	[Denominacao] [varchar](128) NOT NULL,
	[Unidade] [varchar](16) NULL,
	[Referencia] [numeric](18, 6) NULL,
 CONSTRAINT [PK_RhIndices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [RhIndices_indice] UNIQUE NONCLUSTERED 
(
	[Indice] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDICES]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sistemas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sistemas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](16) NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Sistemas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Turnos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Turnos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Denominacao] [varchar](32) NOT NULL,
	[Inicio] [time](7) NULL,
	[Termino] [time](7) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Turnos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ufs]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ufs](
	[Id] [int] NOT NULL,
	[Sigla] [char](2) NOT NULL,
	[Estado] [varchar](32) NOT NULL,
	[Capital] [varchar](32) NOT NULL,
	[Regiao] [varchar](16) NOT NULL,
 CONSTRAINT [PK_Ufs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Ufs_key] UNIQUE NONCLUSTERED 
(
	[Sigla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](64) NOT NULL,
	[Login] [varchar](256) NOT NULL,
	[Senha] [varchar](256) NOT NULL,
	[Perfil] [int] NOT NULL,
	[Ativo] [bit] NOT NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Usuarios_login] UNIQUE NONCLUSTERED 
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDICES]
) ON [PRIMARY]
GO
/****** Object:  Table [nfe].[AnpProdutos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [nfe].[AnpProdutos](
	[Id] [int] NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[Informar] [bit] NOT NULL,
 CONSTRAINT [PK_AnpProdutos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [nfe].[Fornecedores]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [nfe].[Fornecedores](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[Cnpj] [varchar](32) NULL,
	[Cpf] [varchar](16) NULL,
	[Razao] [varchar](64) NOT NULL,
	[Fantasia] [varchar](64) NOT NULL,
	[Endereco] [varchar](128) NOT NULL,
	[EnderecoNo] [varchar](8) NULL,
	[Complemento] [varchar](64) NULL,
	[Bairro] [varchar](32) NULL,
	[MunicipioId] [int] NOT NULL,
	[UfId] [char](2) NOT NULL,
	[Cep] [int] NULL,
	[PaisId] [varchar](8) NOT NULL,
	[Telefone] [varchar](32) NULL,
	[IEstadual] [varchar](16) NULL,
	[IESubstituto] [varchar](16) NULL,
	[IMunicipal] [varchar](16) NULL,
	[Cnae] [varchar](8) NULL,
	[TributarioId] [int] NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Fornecedores] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [nfe].[Ncm]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [nfe].[Ncm](
	[Id] [int] NOT NULL,
	[Classificacao] [varchar](16) NOT NULL,
	[Descricao] [varchar](256) NOT NULL,
	[Ipi] [numeric](9, 6) NULL,
	[Vigente] [bit] NOT NULL,
	[GrupoId] [int] NULL,
 CONSTRAINT [PK_Ncm] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [nfe].[UComercial]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [nfe].[UComercial](
	[Id] [varchar](8) NOT NULL,
	[Denominacao] [varchar](32) NOT NULL,
 CONSTRAINT [PK_UndComercial] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Atendimentos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Atendimentos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LinhaId] [int] NOT NULL,
	[Prefixo] [varchar](16) NOT NULL,
	[Denominacao] [varchar](128) NOT NULL,
	[Uteis] [bit] NOT NULL,
	[Sabados] [bit] NOT NULL,
	[Domingos] [bit] NOT NULL,
	[ExtensaoAB] [numeric](18, 3) NULL,
	[ExtensaoBA] [numeric](18, 3) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Atendimentos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Atendimentos_key] UNIQUE NONCLUSTERED 
(
	[LinhaId] ASC,
	[Prefixo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Carrocerias]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Carrocerias](
	[VeiculoId] [int] NOT NULL,
	[Fabricante] [varchar](64) NULL,
	[Modelo] [varchar](64) NULL,
	[Referencia] [varchar](32) NULL,
	[Ano] [int] NULL,
	[Aquisicao] [date] NULL,
	[Fornecedor] [varchar](64) NULL,
	[NotaFiscal] [varchar](16) NULL,
	[Valor] [money] NULL,
	[ChaveNfe] [varchar](64) NULL,
	[Encarrocamento] [date] NULL,
	[QuemEncarroca] [varchar](64) NULL,
	[NotaEncarroca] [varchar](16) NULL,
	[ValorEncarroca] [money] NULL,
	[Portas] [tinyint] NOT NULL,
	[Assentos] [tinyint] NULL,
	[Capacidade] [tinyint] NULL,
	[Piso] [varchar](32) NULL,
	[EscapeV] [bit] NOT NULL,
	[EscapeH] [bit] NOT NULL,
	[Catraca] [int] NULL,
	[PortaIn] [int] NOT NULL,
	[SaidaFrente] [bit] NOT NULL,
	[SaidaMeio] [bit] NOT NULL,
	[SaidaTras] [bit] NOT NULL,
 CONSTRAINT [PK_Carrocerias] PRIMARY KEY CLUSTERED 
(
	[VeiculoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Chassis]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Chassis](
	[VeiculoId] [int] NOT NULL,
	[Fabricante] [varchar](64) NULL,
	[Modelo] [varchar](64) NULL,
	[ChassiNo] [varchar](32) NULL,
	[Ano] [int] NULL,
	[Aquisicao] [date] NULL,
	[Fornecedor] [varchar](64) NULL,
	[NotaFiscal] [varchar](16) NULL,
	[Valor] [money] NULL,
	[ChaveNfe] [varchar](64) NULL,
	[MotorId] [int] NULL,
	[Potencia] [varchar](32) NULL,
	[PosMotor] [int] NULL,
	[EixosFrente] [tinyint] NOT NULL,
	[EixosTras] [tinyint] NOT NULL,
	[PneusFrente] [varchar](16) NULL,
	[PneusTras] [varchar](16) NULL,
	[TransmiteId] [int] NULL,
	[DirecaoId] [int] NULL,
 CONSTRAINT [PK_Chassis] PRIMARY KEY CLUSTERED 
(
	[VeiculoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[CLinhas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[CLinhas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[ClassLinhaId] [int] NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_CLinhas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Corredores]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Corredores](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Prefixo] [varchar](16) NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[Caracteristicas] [varchar](512) NULL,
	[Municipio] [varchar](32) NOT NULL,
	[UfId] [char](2) NOT NULL,
	[Extensao] [numeric](18, 3) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Corredores] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Custos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Custos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Referencia] [datetime] NOT NULL,
	[Fixo] [money] NOT NULL,
	[Variavel] [money] NOT NULL,
 CONSTRAINT [PK_Custos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[CVeiculos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[CVeiculos](
	[Id] [int] NOT NULL,
	[Categoria] [varchar](16) NOT NULL,
	[Classe] [varchar](32) NOT NULL,
	[Minimo] [int] NULL,
	[Maximo] [int] NULL,
 CONSTRAINT [PK_CVeiculos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[CVeiculosAtt]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[CVeiculosAtt](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Caracteristica] [varchar](128) NOT NULL,
	[Unidade] [varchar](16) NULL,
	[Variavel] [bit] NOT NULL,
 CONSTRAINT [PK_CVeiculosAtt] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[ECVeiculos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[ECVeiculos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[ClasseId] [int] NOT NULL,
	[Minimo] [int] NULL,
	[Maximo] [int] NULL,
	[Passageirom2] [tinyint] NOT NULL,
	[Util] [int] NULL,
	[Residual] [numeric](9, 6) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_ECVeiculos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[EDominios]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[EDominios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[DominioId] [int] NOT NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_EDominios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[EInstalacoes]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[EInstalacoes](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[InstalacaoId] [int] NOT NULL,
	[PropositoId] [int] NOT NULL,
	[AreaCoberta] [numeric](18, 3) NULL,
	[AreaTotal] [numeric](18, 3) NULL,
	[QtdEmpregados] [int] NULL,
	[Inicio] [time](7) NULL,
	[Termino] [time](7) NULL,
	[Efluentes] [bit] NOT NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_EInstalacao] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[EPeriodos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[EPeriodos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[PeriodoId] [int] NOT NULL,
	[Denominacao] [varchar](32) NOT NULL,
	[Velocidade] [numeric](9, 3) NULL,
	[Pico] [bit] NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_EPeriodos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[FrotaHoras]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[FrotaHoras](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Ano] [int] NOT NULL,
	[Mes] [int] NOT NULL,
	[HorarioId] [int] NOT NULL,
	[Frota] [int] NOT NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_FrotaHoras] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Frotas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Frotas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Ano] [int] NOT NULL,
	[Mes] [int] NOT NULL,
	[CategoriaId] [int] NOT NULL,
	[EtariaId] [int] NOT NULL,
	[ArCondicionado] [bit] NOT NULL,
	[Quantidade] [int] NOT NULL,
 CONSTRAINT [PK_Frotas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[FViagens]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[FViagens](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ViagemId] [int] NOT NULL,
	[PontoId] [int] NOT NULL,
	[Embarques] [int] NULL,
	[Desembarques] [int] NULL,
	[Acumulado]  AS ([opc].[acumPassageiros]([ViagemId],[Id])),
 CONSTRAINT [PK_FViagens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[FxEtarias]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[FxEtarias](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[Denominacao] [varchar](32) NOT NULL,
	[Minimo] [int] NOT NULL,
	[Maximo] [int] NOT NULL,
 CONSTRAINT [PK_FxEtarias] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Horarios]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Horarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LinhaId] [int] NOT NULL,
	[DiaId] [int] NOT NULL,
	[Sentido] [char](2) NOT NULL,
	[Inicio] [time](7) NOT NULL,
	[AtendimentoId] [int] NULL,
	[PeriodoId]  AS ([opc].[getPeriodo]([LinhaId],[DiaId],[Inicio])),
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Horarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Instalacoes]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Instalacoes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Prefixo] [varchar](16) NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[Endereco] [varchar](128) NOT NULL,
	[EnderecoNo] [varchar](8) NULL,
	[Complemento] [varchar](64) NULL,
	[Cep] [int] NULL,
	[Bairro] [varchar](32) NULL,
	[Municipio] [varchar](32) NOT NULL,
	[UfId] [char](2) NOT NULL,
	[Telefone] [varchar](32) NULL,
	[Email] [varchar](256) NULL,
	[Latitude] [numeric](24, 12) NULL,
	[Longitude] [numeric](24, 12) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Instalacoes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[ItAtendimentos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[ItAtendimentos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AtendimentoId] [int] NOT NULL,
	[Sentido] [char](2) NOT NULL,
	[Percurso] [varchar](256) NOT NULL,
	[Extensao] [numeric](18, 3) NULL,
	[PavimentoId] [int] NULL,
	[Abrangencia] [numeric](9, 3) NULL,
	[CondicaoId] [int] NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_ItAtendimentos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Itinerarios]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Itinerarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LinhaId] [int] NOT NULL,
	[Sentido] [char](2) NOT NULL,
	[Percurso] [varchar](256) NOT NULL,
	[Extensao] [numeric](18, 3) NULL,
	[PavimentoId] [int] NULL,
	[Abrangencia] [numeric](9, 3) NULL,
	[CondicaoId] [int] NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Itinerarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[ItTroncos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[ItTroncos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TroncoId] [int] NOT NULL,
	[Sentido] [char](2) NOT NULL,
	[Percurso] [varchar](256) NOT NULL,
	[Extensao] [numeric](18, 3) NULL,
	[PavimentoId] [int] NULL,
	[Abrangencia] [numeric](9, 3) NULL,
	[CondicaoId] [int] NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_ItTroncos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Linhas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Linhas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Prefixo] [varchar](16) NOT NULL,
	[Denominacao] [varchar](128) NOT NULL,
	[Uteis] [bit] NOT NULL,
	[Sabados] [bit] NOT NULL,
	[Domingos] [bit] NOT NULL,
	[DominioId] [int] NOT NULL,
	[OperacaoId] [int] NOT NULL,
	[Classificacao] [int] NOT NULL,
	[Captacao] [bit] NOT NULL,
	[Transporte] [bit] NOT NULL,
	[Distribuicao] [bit] NOT NULL,
	[ExtensaoAB] [numeric](18, 3) NULL,
	[ExtensaoBA] [numeric](18, 3) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Linhas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Linhas_key] UNIQUE NONCLUSTERED 
(
	[EmpresaId] ASC,
	[Prefixo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDICES]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[LnCorredores]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[LnCorredores](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CorredorId] [int] NOT NULL,
	[LinhaId] [int] NOT NULL,
	[Sentido] [char](2) NULL,
	[Extensao] [numeric](18, 3) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_LnCorredores] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[LnPesquisas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[LnPesquisas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PesquisaId] [int] NOT NULL,
	[LinhaId] [int] NOT NULL,
	[Uteis] [bit] NOT NULL,
	[Sabados] [bit] NOT NULL,
	[Domingos] [bit] NOT NULL,
	[Responsavel] [varchar](64) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_LnPesquisas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [LnPesquisas_key] UNIQUE NONCLUSTERED 
(
	[PesquisaId] ASC,
	[LinhaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDICES]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[LnTerminais]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[LnTerminais](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TerminalId] [int] NOT NULL,
	[LinhaId] [int] NOT NULL,
	[Uteis] [bit] NOT NULL,
	[UteisFluxo] [int] NULL,
	[Sabados] [bit] NOT NULL,
	[SabadosFluxo] [int] NULL,
	[Domingos] [bit] NOT NULL,
	[DomingosFluxo] [int] NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_LnTerminais] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [LnTerminais_key] UNIQUE NONCLUSTERED 
(
	[TerminalId] ASC,
	[LinhaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDICES]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[LnTroncos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[LnTroncos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TroncoId] [int] NOT NULL,
	[LinhaId] [int] NOT NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_LnTroncos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [LnTroncos_key] UNIQUE NONCLUSTERED 
(
	[TroncoId] ASC,
	[LinhaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDICES]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[MapasLinha]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[MapasLinha](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LinhaId] [int] NOT NULL,
	[Sentido] [char](2) NOT NULL,
	[AtendimentoId] [int] NULL,
	[Descricao] [varchar](64) NULL,
	[Arquivo] [varchar](256) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Motores]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Motores](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[Classificacao] [varchar](64) NOT NULL,
	[Descricao] [varchar](256) NULL,
 CONSTRAINT [PK_Motores] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Ocupacoes]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Ocupacoes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Denominacao] [varchar](32) NOT NULL,
	[Nivel] [varchar](4) NOT NULL,
	[Densidade] [numeric](9, 3) NOT NULL,
 CONSTRAINT [PK_Ocupacoes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Ofertas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Ofertas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LinhaId] [int] NOT NULL,
	[Ano] [int] NOT NULL,
	[Mes] [int] NOT NULL,
	[Categoria] [int] NOT NULL,
	[Passageiros] [int] NOT NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Ofertas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Operacoes]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Operacoes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[OperLinhaId] [int] NOT NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Operacoes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Pesquisas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Pesquisas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Identificacao] [varchar](64) NOT NULL,
	[Inicio] [date] NOT NULL,
	[Termino] [date] NOT NULL,
	[TerminalId] [int] NULL,
	[TroncoId] [int] NULL,
	[CorredorId] [int] NULL,
	[OperacaoId] [int] NULL,
	[Classificacao] [int] NULL,
	[Interna] [bit] NOT NULL,
	[Fornecedor] [varchar](64) NULL,
	[Contrato] [varchar](32) NULL,
	[Uteis] [bit] NOT NULL,
	[Sabados] [bit] NOT NULL,
	[Domingos] [bit] NOT NULL,
	[Responsavel] [varchar](64) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Pesquisas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Planos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Planos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LinhaId] [int] NOT NULL,
	[AtendimentoId] [int] NULL,
	[Sentido] [char](2) NOT NULL,
	[ViagensUtil] [int] NULL,
	[ViagensSab] [int] NULL,
	[ViagensDom] [int] NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Planos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Pontos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Pontos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Prefixo] [varchar](16) NOT NULL,
	[Identificacao] [varchar](32) NOT NULL,
	[Endereco] [varchar](128) NOT NULL,
	[EnderecoRef] [varchar](64) NULL,
	[Cep] [int] NULL,
	[Bairro] [varchar](32) NULL,
	[Municipio] [varchar](32) NOT NULL,
	[UfId] [char](2) NOT NULL,
	[Intercambio] [bit] NOT NULL,
	[Latitude] [numeric](24, 12) NULL,
	[Longitude] [numeric](24, 12) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Pontos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[PrLinhas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[PrLinhas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LinhaId] [int] NOT NULL,
	[PeriodoId] [int] NOT NULL,
	[DiaId] [int] NOT NULL,
	[Inicio] [time](7) NOT NULL,
	[Termino] [time](7) NOT NULL,
	[CicloAB] [int] NULL,
	[CicloBA] [int] NULL,
	[CVeiculoId] [int] NULL,
	[OcupacaoId] [int] NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_PrLinhas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Producao]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Producao](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Ano] [int] NOT NULL,
	[Mes] [int] NOT NULL,
	[TarifariaId] [int] NOT NULL,
	[LinhaId] [int] NULL,
	[Passageiros] [int] NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Producao] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[PtAtendimentos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[PtAtendimentos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AtendimentoId] [int] NOT NULL,
	[Sentido] [char](2) NOT NULL,
	[PontoId] [int] NOT NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_PtAtendimentos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[PtLinhas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[PtLinhas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LinhaId] [int] NOT NULL,
	[Sentido] [char](2) NOT NULL,
	[PontoId] [int] NOT NULL,
	[OrigemId] [int] NULL,
	[DestinoId] [int] NULL,
	[Fluxo] [int] NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_PtLinhas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[PtTroncos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[PtTroncos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TroncoId] [int] NOT NULL,
	[Sentido] [char](2) NOT NULL,
	[PontoId] [int] NOT NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_PtTroncos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Renovacao]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Renovacao](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[LinhaId] [int] NOT NULL,
	[Ano] [int] NOT NULL,
	[Mes] [int] NOT NULL,
	[DiaId] [int] NULL,
	[Indice] [numeric](18, 3) NOT NULL,
	[Referencia]  AS (eomonth(datefromparts([Ano],[Mes],(1)))),
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Renovacao] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Tarifas]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Tarifas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Referencia] [date] NOT NULL,
	[Valor] [money] NOT NULL,
	[Decreto] [varchar](128) NULL,
 CONSTRAINT [PK_Tarifas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[TCategorias]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[TCategorias](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Denominacao] [varchar](32) NOT NULL,
	[Gratuidade] [bit] NOT NULL,
	[Rateio] [numeric](9, 3) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_TCategorias] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Terminais]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Terminais](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Prefixo] [varchar](16) NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[Cnpj] [varchar](32) NULL,
	[IEstadual] [varchar](16) NULL,
	[IMunicipal] [varchar](16) NULL,
	[Endereco] [varchar](128) NOT NULL,
	[EnderecoNo] [varchar](8) NULL,
	[Complemento] [varchar](64) NULL,
	[Cep] [int] NULL,
	[Bairro] [varchar](32) NULL,
	[Municipio] [varchar](32) NOT NULL,
	[UfId] [char](2) NOT NULL,
	[Telefone] [varchar](32) NULL,
	[Email] [varchar](256) NULL,
	[Inicio] [time](7) NULL,
	[Termino] [time](7) NULL,
	[Latitude] [numeric](24, 12) NULL,
	[Longitude] [numeric](24, 12) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Terminais] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Troncos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Troncos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Prefixo] [varchar](16) NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[ExtensaoAB] [numeric](18, 3) NULL,
	[ExtensaoBA] [numeric](18, 3) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Troncos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[TServicos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[TServicos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TerminalId] [int] NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
	[Descricao] [varchar](256) NULL,
	[Inicio] [time](7) NULL,
	[Termino] [time](7) NULL,
 CONSTRAINT [PK_TServicos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Veiculos]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Veiculos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[Numero] [varchar](16) NOT NULL,
	[Cor] [varchar](32) NOT NULL,
	[Classe] [int] NOT NULL,
	[Categoria] [int] NULL,
	[Placa] [varchar](16) NOT NULL,
	[Renavam] [varchar](16) NOT NULL,
	[Antt] [varchar](16) NULL,
	[Inicio] [date] NULL,
	[Inativo] [bit] NOT NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Veiculos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Veiculos_numero] UNIQUE NONCLUSTERED 
(
	[EmpresaId] ASC,
	[Numero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDICES]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[VeiculosAtt]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[VeiculosAtt](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Classe] [int] NOT NULL,
	[Attributo] [int] NOT NULL,
	[Conteudo] [varchar](512) NOT NULL,
 CONSTRAINT [PK_VeiculosAtt] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Viagens]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Viagens](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LinhaId] [int] NOT NULL,
	[Item] [int] NULL,
	[Data] [date] NOT NULL,
	[Sentido] [char](2) NOT NULL,
	[HorarioId] [int] NULL,
	[PontoId] [int] NULL,
	[VeiculoId] [int] NULL,
	[Chegada] [time](7) NULL,
	[Inicio] [time](7) NOT NULL,
	[Termino] [time](7) NULL,
	[PeriodoId] [int] NULL,
	[Passageiros] [int] NULL,
	[Inicial] [int] NULL,
	[Final] [int] NULL,
	[Responsavel] [varchar](32) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Viagens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Viagens_temp]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Viagens_temp](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LinhaId] [int] NOT NULL,
	[Item] [int] NULL,
	[Data] [date] NOT NULL,
	[Sentido] [char](2) NOT NULL,
	[HorarioId] [int] NULL,
	[PontoId] [int] NULL,
	[VeiculoId] [int] NULL,
	[Chegada] [time](7) NULL,
	[Inicio] [time](7) NOT NULL,
	[Termino] [time](7) NULL,
	[Passageiros] [int] NULL,
	[Inicial] [int] NULL,
	[Final] [int] NULL,
	[Responsavel] [varchar](32) NULL,
	[Cadastro] [datetime] NULL,
 CONSTRAINT [PK_Viagens_temp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [opc].[Vias]    Script Date: 28.11.2019 13:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [opc].[Vias](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Denominacao] [varchar](64) NOT NULL,
 CONSTRAINT [PK_Vias] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Centros] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [dbo].[Contas] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [dbo].[EEncargos] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [dbo].[Empresas] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [dbo].[ESistemas] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [dbo].[EUsuarios] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [dbo].[Sistemas] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [dbo].[Turnos] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT ((2)) FOR [Perfil]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [nfe].[Fornecedores] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Atendimentos] ADD  CONSTRAINT [DF__Atendimen__Cadas__2B0A656D]  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Carrocerias] ADD  DEFAULT ((2)) FOR [Portas]
GO
ALTER TABLE [opc].[Carrocerias] ADD  DEFAULT ((0)) FOR [EscapeV]
GO
ALTER TABLE [opc].[Carrocerias] ADD  DEFAULT ((0)) FOR [EscapeH]
GO
ALTER TABLE [opc].[Carrocerias] ADD  DEFAULT ((1)) FOR [PortaIn]
GO
ALTER TABLE [opc].[Carrocerias] ADD  DEFAULT ((0)) FOR [SaidaFrente]
GO
ALTER TABLE [opc].[Carrocerias] ADD  DEFAULT ((1)) FOR [SaidaMeio]
GO
ALTER TABLE [opc].[Carrocerias] ADD  DEFAULT ((1)) FOR [SaidaTras]
GO
ALTER TABLE [opc].[Chassis] ADD  DEFAULT ((1)) FOR [MotorId]
GO
ALTER TABLE [opc].[Chassis] ADD  DEFAULT ((1)) FOR [EixosFrente]
GO
ALTER TABLE [opc].[Chassis] ADD  DEFAULT ((1)) FOR [EixosTras]
GO
ALTER TABLE [opc].[CLinhas] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Corredores] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[ECVeiculos] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[EDominios] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[EInstalacoes] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[EPeriodos] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[FrotaHoras] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Horarios] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Instalacoes] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[ItAtendimentos] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Itinerarios] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[ItTroncos] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Linhas] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[LnCorredores] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[LnPesquisas] ADD  CONSTRAINT [DF__LnPesquis__Cadas__42ACE4D4]  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[LnTerminais] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[LnTroncos] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Ofertas] ADD  CONSTRAINT [DF__Ofertas__Cadastr__0F2D40CE]  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Operacoes] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Pesquisas] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Planos] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Pontos] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[PrLinhas] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Producao] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[PtAtendimentos] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[PtLinhas] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[PtTroncos] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Renovacao] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[TCategorias] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Terminais] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Troncos] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Veiculos] ADD  DEFAULT ((0)) FOR [Inativo]
GO
ALTER TABLE [opc].[Veiculos] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Viagens] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [opc].[Viagens_temp] ADD  DEFAULT (getdate()) FOR [Cadastro]
GO
ALTER TABLE [dbo].[Centros]  WITH CHECK ADD  CONSTRAINT [Centros_centro] FOREIGN KEY([VinculoId])
REFERENCES [dbo].[Centros] ([Id])
GO
ALTER TABLE [dbo].[Centros] CHECK CONSTRAINT [Centros_centro]
GO
ALTER TABLE [dbo].[Cidades]  WITH CHECK ADD  CONSTRAINT [Cidades_uf] FOREIGN KEY([UfId])
REFERENCES [dbo].[Ufs] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Cidades] CHECK CONSTRAINT [Cidades_uf]
GO
ALTER TABLE [dbo].[Contas]  WITH CHECK ADD  CONSTRAINT [Contas_conta] FOREIGN KEY([VinculoId])
REFERENCES [dbo].[Contas] ([Id])
GO
ALTER TABLE [dbo].[Contas] CHECK CONSTRAINT [Contas_conta]
GO
ALTER TABLE [dbo].[EEncargos]  WITH CHECK ADD  CONSTRAINT [EEncargos_encargo] FOREIGN KEY([EncargoId])
REFERENCES [dbo].[Encargos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[EEncargos] CHECK CONSTRAINT [EEncargos_encargo]
GO
ALTER TABLE [dbo].[Empresas]  WITH NOCHECK ADD  CONSTRAINT [Empresas_pais] FOREIGN KEY([PaisId])
REFERENCES [dbo].[Paises] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Empresas] NOCHECK CONSTRAINT [Empresas_pais]
GO
ALTER TABLE [dbo].[ESistemas]  WITH CHECK ADD  CONSTRAINT [ESistemas_sistema] FOREIGN KEY([SistemaId])
REFERENCES [dbo].[Sistemas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ESistemas] CHECK CONSTRAINT [ESistemas_sistema]
GO
ALTER TABLE [dbo].[EUsuarios]  WITH CHECK ADD  CONSTRAINT [EUsuarios_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[EUsuarios] CHECK CONSTRAINT [EUsuarios_empresa]
GO
ALTER TABLE [dbo].[EUsuarios]  WITH CHECK ADD  CONSTRAINT [EUsuarios_usuario] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[EUsuarios] CHECK CONSTRAINT [EUsuarios_usuario]
GO
ALTER TABLE [nfe].[Fornecedores]  WITH CHECK ADD  CONSTRAINT [Fornecedores_municipio] FOREIGN KEY([MunicipioId])
REFERENCES [dbo].[Cidades] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [nfe].[Fornecedores] CHECK CONSTRAINT [Fornecedores_municipio]
GO
ALTER TABLE [nfe].[Fornecedores]  WITH NOCHECK ADD  CONSTRAINT [Fornecedores_pais] FOREIGN KEY([PaisId])
REFERENCES [dbo].[Paises] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [nfe].[Fornecedores] NOCHECK CONSTRAINT [Fornecedores_pais]
GO
ALTER TABLE [nfe].[Ncm]  WITH CHECK ADD  CONSTRAINT [Ncm_grupo] FOREIGN KEY([GrupoId])
REFERENCES [nfe].[Ncm] ([Id])
GO
ALTER TABLE [nfe].[Ncm] CHECK CONSTRAINT [Ncm_grupo]
GO
ALTER TABLE [opc].[Atendimentos]  WITH CHECK ADD  CONSTRAINT [Atendimentos_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[Linhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Atendimentos] CHECK CONSTRAINT [Atendimentos_linha]
GO
ALTER TABLE [opc].[Chassis]  WITH CHECK ADD  CONSTRAINT [Chassis_motor] FOREIGN KEY([MotorId])
REFERENCES [opc].[Motores] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Chassis] CHECK CONSTRAINT [Chassis_motor]
GO
ALTER TABLE [opc].[CLinhas]  WITH CHECK ADD  CONSTRAINT [CLinhas_classlinha] FOREIGN KEY([ClassLinhaId])
REFERENCES [dbo].[ClassLinhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[CLinhas] CHECK CONSTRAINT [CLinhas_classlinha]
GO
ALTER TABLE [opc].[CLinhas]  WITH CHECK ADD  CONSTRAINT [CLinhas_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[CLinhas] CHECK CONSTRAINT [CLinhas_empresa]
GO
ALTER TABLE [opc].[Corredores]  WITH CHECK ADD  CONSTRAINT [Corredores_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Corredores] CHECK CONSTRAINT [Corredores_empresa]
GO
ALTER TABLE [opc].[Custos]  WITH CHECK ADD  CONSTRAINT [Custos_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Custos] CHECK CONSTRAINT [Custos_empresa]
GO
ALTER TABLE [opc].[ECVeiculos]  WITH CHECK ADD  CONSTRAINT [ECVeiculos_classe] FOREIGN KEY([ClasseId])
REFERENCES [opc].[CVeiculos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[ECVeiculos] CHECK CONSTRAINT [ECVeiculos_classe]
GO
ALTER TABLE [opc].[ECVeiculos]  WITH CHECK ADD  CONSTRAINT [ECVeiculos_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[ECVeiculos] CHECK CONSTRAINT [ECVeiculos_empresa]
GO
ALTER TABLE [opc].[EDominios]  WITH CHECK ADD  CONSTRAINT [EDominios_dominio] FOREIGN KEY([DominioId])
REFERENCES [dbo].[Dominios] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[EDominios] CHECK CONSTRAINT [EDominios_dominio]
GO
ALTER TABLE [opc].[EDominios]  WITH CHECK ADD  CONSTRAINT [EDominios_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[EDominios] CHECK CONSTRAINT [EDominios_empresa]
GO
ALTER TABLE [opc].[EInstalacoes]  WITH CHECK ADD  CONSTRAINT [EInstalacoes_instalacao] FOREIGN KEY([InstalacaoId])
REFERENCES [opc].[Instalacoes] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[EInstalacoes] CHECK CONSTRAINT [EInstalacoes_instalacao]
GO
ALTER TABLE [opc].[EInstalacoes]  WITH CHECK ADD  CONSTRAINT [EInstalacoes_proposito] FOREIGN KEY([PropositoId])
REFERENCES [dbo].[FInstalacoes] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[EInstalacoes] CHECK CONSTRAINT [EInstalacoes_proposito]
GO
ALTER TABLE [opc].[EPeriodos]  WITH CHECK ADD  CONSTRAINT [EPeriodos_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[EPeriodos] CHECK CONSTRAINT [EPeriodos_empresa]
GO
ALTER TABLE [opc].[EPeriodos]  WITH CHECK ADD  CONSTRAINT [EPeriodos_periodo] FOREIGN KEY([PeriodoId])
REFERENCES [dbo].[Periodos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[EPeriodos] CHECK CONSTRAINT [EPeriodos_periodo]
GO
ALTER TABLE [opc].[Frotas]  WITH CHECK ADD  CONSTRAINT [Frotas_categoria] FOREIGN KEY([CategoriaId])
REFERENCES [opc].[CVeiculos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Frotas] CHECK CONSTRAINT [Frotas_categoria]
GO
ALTER TABLE [opc].[Frotas]  WITH CHECK ADD  CONSTRAINT [Frotas_etaria] FOREIGN KEY([EtariaId])
REFERENCES [opc].[FxEtarias] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Frotas] CHECK CONSTRAINT [Frotas_etaria]
GO
ALTER TABLE [opc].[FViagens]  WITH CHECK ADD  CONSTRAINT [FViagens_ponto] FOREIGN KEY([PontoId])
REFERENCES [opc].[PtLinhas] ([Id])
GO
ALTER TABLE [opc].[FViagens] CHECK CONSTRAINT [FViagens_ponto]
GO
ALTER TABLE [opc].[FViagens]  WITH CHECK ADD  CONSTRAINT [FViagens_viagem] FOREIGN KEY([ViagemId])
REFERENCES [opc].[Viagens] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[FViagens] CHECK CONSTRAINT [FViagens_viagem]
GO
ALTER TABLE [opc].[Horarios]  WITH CHECK ADD  CONSTRAINT [Horarios_atendimento] FOREIGN KEY([AtendimentoId])
REFERENCES [opc].[Atendimentos] ([Id])
GO
ALTER TABLE [opc].[Horarios] CHECK CONSTRAINT [Horarios_atendimento]
GO
ALTER TABLE [opc].[Horarios]  WITH CHECK ADD  CONSTRAINT [Horarios_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[Linhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Horarios] CHECK CONSTRAINT [Horarios_linha]
GO
ALTER TABLE [opc].[Instalacoes]  WITH CHECK ADD  CONSTRAINT [Instalacoes_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Instalacoes] CHECK CONSTRAINT [Instalacoes_empresa]
GO
ALTER TABLE [opc].[ItAtendimentos]  WITH CHECK ADD  CONSTRAINT [ItAtendimentos_atendimento] FOREIGN KEY([AtendimentoId])
REFERENCES [opc].[Atendimentos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[ItAtendimentos] CHECK CONSTRAINT [ItAtendimentos_atendimento]
GO
ALTER TABLE [opc].[ItAtendimentos]  WITH CHECK ADD  CONSTRAINT [ItAtendimentos_pavimento] FOREIGN KEY([PavimentoId])
REFERENCES [opc].[Vias] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[ItAtendimentos] CHECK CONSTRAINT [ItAtendimentos_pavimento]
GO
ALTER TABLE [opc].[Itinerarios]  WITH CHECK ADD  CONSTRAINT [Itinerarios_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[Linhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Itinerarios] CHECK CONSTRAINT [Itinerarios_linha]
GO
ALTER TABLE [opc].[Itinerarios]  WITH CHECK ADD  CONSTRAINT [Itinerarios_pavimento] FOREIGN KEY([PavimentoId])
REFERENCES [opc].[Vias] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Itinerarios] CHECK CONSTRAINT [Itinerarios_pavimento]
GO
ALTER TABLE [opc].[ItTroncos]  WITH CHECK ADD  CONSTRAINT [ItTroncos_pavimento] FOREIGN KEY([PavimentoId])
REFERENCES [opc].[Vias] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[ItTroncos] CHECK CONSTRAINT [ItTroncos_pavimento]
GO
ALTER TABLE [opc].[ItTroncos]  WITH CHECK ADD  CONSTRAINT [ItTroncos_tronco] FOREIGN KEY([TroncoId])
REFERENCES [opc].[Troncos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[ItTroncos] CHECK CONSTRAINT [ItTroncos_tronco]
GO
ALTER TABLE [opc].[Linhas]  WITH CHECK ADD  CONSTRAINT [Linhas_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
GO
ALTER TABLE [opc].[Linhas] CHECK CONSTRAINT [Linhas_empresa]
GO
ALTER TABLE [opc].[LnCorredores]  WITH CHECK ADD  CONSTRAINT [LnCorredores_corredor] FOREIGN KEY([CorredorId])
REFERENCES [opc].[Corredores] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[LnCorredores] CHECK CONSTRAINT [LnCorredores_corredor]
GO
ALTER TABLE [opc].[LnCorredores]  WITH CHECK ADD  CONSTRAINT [LnCorredores_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[Linhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[LnCorredores] CHECK CONSTRAINT [LnCorredores_linha]
GO
ALTER TABLE [opc].[LnPesquisas]  WITH CHECK ADD  CONSTRAINT [LnPesquisas_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[Linhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[LnPesquisas] CHECK CONSTRAINT [LnPesquisas_linha]
GO
ALTER TABLE [opc].[LnPesquisas]  WITH CHECK ADD  CONSTRAINT [LnPesquisas_pesquisa] FOREIGN KEY([PesquisaId])
REFERENCES [opc].[Pesquisas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[LnPesquisas] CHECK CONSTRAINT [LnPesquisas_pesquisa]
GO
ALTER TABLE [opc].[LnTerminais]  WITH CHECK ADD  CONSTRAINT [LnTerminais_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[Linhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[LnTerminais] CHECK CONSTRAINT [LnTerminais_linha]
GO
ALTER TABLE [opc].[LnTerminais]  WITH CHECK ADD  CONSTRAINT [LnTerminais_terminal] FOREIGN KEY([TerminalId])
REFERENCES [opc].[Terminais] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[LnTerminais] CHECK CONSTRAINT [LnTerminais_terminal]
GO
ALTER TABLE [opc].[LnTroncos]  WITH CHECK ADD  CONSTRAINT [LnTroncos_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[Linhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[LnTroncos] CHECK CONSTRAINT [LnTroncos_linha]
GO
ALTER TABLE [opc].[LnTroncos]  WITH CHECK ADD  CONSTRAINT [LnTroncos_tronco] FOREIGN KEY([TroncoId])
REFERENCES [opc].[Troncos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[LnTroncos] CHECK CONSTRAINT [LnTroncos_tronco]
GO
ALTER TABLE [opc].[MapasLinha]  WITH CHECK ADD  CONSTRAINT [MapasLinha_atendimento] FOREIGN KEY([AtendimentoId])
REFERENCES [opc].[Atendimentos] ([Id])
GO
ALTER TABLE [opc].[MapasLinha] CHECK CONSTRAINT [MapasLinha_atendimento]
GO
ALTER TABLE [opc].[MapasLinha]  WITH CHECK ADD  CONSTRAINT [MapasLinha_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[Linhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[MapasLinha] CHECK CONSTRAINT [MapasLinha_linha]
GO
ALTER TABLE [opc].[Ofertas]  WITH CHECK ADD  CONSTRAINT [Ofertas_categoria] FOREIGN KEY([Categoria])
REFERENCES [opc].[TCategorias] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Ofertas] CHECK CONSTRAINT [Ofertas_categoria]
GO
ALTER TABLE [opc].[Ofertas]  WITH CHECK ADD  CONSTRAINT [Ofertas_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[Linhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Ofertas] CHECK CONSTRAINT [Ofertas_linha]
GO
ALTER TABLE [opc].[Operacoes]  WITH CHECK ADD  CONSTRAINT [Operacoes_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Operacoes] CHECK CONSTRAINT [Operacoes_empresa]
GO
ALTER TABLE [opc].[Operacoes]  WITH CHECK ADD  CONSTRAINT [Operacoes_operlinha] FOREIGN KEY([OperLinhaId])
REFERENCES [dbo].[OperLinhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Operacoes] CHECK CONSTRAINT [Operacoes_operlinha]
GO
ALTER TABLE [opc].[Pesquisas]  WITH CHECK ADD  CONSTRAINT [Pesquisas_classificacao] FOREIGN KEY([Classificacao])
REFERENCES [opc].[CLinhas] ([Id])
GO
ALTER TABLE [opc].[Pesquisas] CHECK CONSTRAINT [Pesquisas_classificacao]
GO
ALTER TABLE [opc].[Pesquisas]  WITH CHECK ADD  CONSTRAINT [Pesquisas_corredor] FOREIGN KEY([CorredorId])
REFERENCES [opc].[Corredores] ([Id])
GO
ALTER TABLE [opc].[Pesquisas] CHECK CONSTRAINT [Pesquisas_corredor]
GO
ALTER TABLE [opc].[Pesquisas]  WITH CHECK ADD  CONSTRAINT [Pesquisas_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Pesquisas] CHECK CONSTRAINT [Pesquisas_empresa]
GO
ALTER TABLE [opc].[Pesquisas]  WITH CHECK ADD  CONSTRAINT [Pesquisas_operacao] FOREIGN KEY([OperacaoId])
REFERENCES [opc].[Operacoes] ([Id])
GO
ALTER TABLE [opc].[Pesquisas] CHECK CONSTRAINT [Pesquisas_operacao]
GO
ALTER TABLE [opc].[Pesquisas]  WITH CHECK ADD  CONSTRAINT [Pesquisas_terminal] FOREIGN KEY([TerminalId])
REFERENCES [opc].[Terminais] ([Id])
GO
ALTER TABLE [opc].[Pesquisas] CHECK CONSTRAINT [Pesquisas_terminal]
GO
ALTER TABLE [opc].[Pesquisas]  WITH CHECK ADD  CONSTRAINT [Pesquisas_tronco] FOREIGN KEY([TroncoId])
REFERENCES [opc].[Troncos] ([Id])
GO
ALTER TABLE [opc].[Pesquisas] CHECK CONSTRAINT [Pesquisas_tronco]
GO
ALTER TABLE [opc].[Planos]  WITH CHECK ADD  CONSTRAINT [Planos_atendimento] FOREIGN KEY([AtendimentoId])
REFERENCES [opc].[Atendimentos] ([Id])
GO
ALTER TABLE [opc].[Planos] CHECK CONSTRAINT [Planos_atendimento]
GO
ALTER TABLE [opc].[Planos]  WITH CHECK ADD  CONSTRAINT [Planos_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[Linhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Planos] CHECK CONSTRAINT [Planos_linha]
GO
ALTER TABLE [opc].[Pontos]  WITH CHECK ADD  CONSTRAINT [Pontos_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Pontos] CHECK CONSTRAINT [Pontos_empresa]
GO
ALTER TABLE [opc].[PrLinhas]  WITH CHECK ADD  CONSTRAINT [PrLinhas_cveiculo] FOREIGN KEY([CVeiculoId])
REFERENCES [opc].[CVeiculos] ([Id])
GO
ALTER TABLE [opc].[PrLinhas] CHECK CONSTRAINT [PrLinhas_cveiculo]
GO
ALTER TABLE [opc].[PrLinhas]  WITH CHECK ADD  CONSTRAINT [PrLinhas_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[Linhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[PrLinhas] CHECK CONSTRAINT [PrLinhas_linha]
GO
ALTER TABLE [opc].[PrLinhas]  WITH CHECK ADD  CONSTRAINT [PrLinhas_ocupacao] FOREIGN KEY([OcupacaoId])
REFERENCES [opc].[Ocupacoes] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[PrLinhas] CHECK CONSTRAINT [PrLinhas_ocupacao]
GO
ALTER TABLE [opc].[PrLinhas]  WITH CHECK ADD  CONSTRAINT [PrLinhas_periodo] FOREIGN KEY([PeriodoId])
REFERENCES [opc].[EPeriodos] ([Id])
GO
ALTER TABLE [opc].[PrLinhas] CHECK CONSTRAINT [PrLinhas_periodo]
GO
ALTER TABLE [opc].[Producao]  WITH CHECK ADD  CONSTRAINT [Producao_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[Linhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Producao] CHECK CONSTRAINT [Producao_linha]
GO
ALTER TABLE [opc].[Producao]  WITH CHECK ADD  CONSTRAINT [Producao_tarifaria] FOREIGN KEY([TarifariaId])
REFERENCES [opc].[TCategorias] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Producao] CHECK CONSTRAINT [Producao_tarifaria]
GO
ALTER TABLE [opc].[PtAtendimentos]  WITH CHECK ADD  CONSTRAINT [PtAtendimentos_atendimento] FOREIGN KEY([AtendimentoId])
REFERENCES [opc].[Atendimentos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[PtAtendimentos] CHECK CONSTRAINT [PtAtendimentos_atendimento]
GO
ALTER TABLE [opc].[PtAtendimentos]  WITH CHECK ADD  CONSTRAINT [PtAtendimentos_ponto] FOREIGN KEY([PontoId])
REFERENCES [opc].[Pontos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[PtAtendimentos] CHECK CONSTRAINT [PtAtendimentos_ponto]
GO
ALTER TABLE [opc].[PtLinhas]  WITH CHECK ADD  CONSTRAINT [PtLinhas_destino] FOREIGN KEY([DestinoId])
REFERENCES [opc].[PtLinhas] ([Id])
GO
ALTER TABLE [opc].[PtLinhas] CHECK CONSTRAINT [PtLinhas_destino]
GO
ALTER TABLE [opc].[PtLinhas]  WITH CHECK ADD  CONSTRAINT [PtLinhas_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[Linhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[PtLinhas] CHECK CONSTRAINT [PtLinhas_linha]
GO
ALTER TABLE [opc].[PtLinhas]  WITH CHECK ADD  CONSTRAINT [PtLinhas_origem] FOREIGN KEY([OrigemId])
REFERENCES [opc].[PtLinhas] ([Id])
GO
ALTER TABLE [opc].[PtLinhas] CHECK CONSTRAINT [PtLinhas_origem]
GO
ALTER TABLE [opc].[PtLinhas]  WITH CHECK ADD  CONSTRAINT [PtLinhas_ponto] FOREIGN KEY([PontoId])
REFERENCES [opc].[Pontos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[PtLinhas] CHECK CONSTRAINT [PtLinhas_ponto]
GO
ALTER TABLE [opc].[PtTroncos]  WITH CHECK ADD  CONSTRAINT [PtTroncos_ponto] FOREIGN KEY([PontoId])
REFERENCES [opc].[Pontos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[PtTroncos] CHECK CONSTRAINT [PtTroncos_ponto]
GO
ALTER TABLE [opc].[PtTroncos]  WITH CHECK ADD  CONSTRAINT [PtTroncos_tronco] FOREIGN KEY([TroncoId])
REFERENCES [opc].[Troncos] ([Id])
GO
ALTER TABLE [opc].[PtTroncos] CHECK CONSTRAINT [PtTroncos_tronco]
GO
ALTER TABLE [opc].[Renovacao]  WITH CHECK ADD  CONSTRAINT [Renovacao_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[Linhas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Renovacao] CHECK CONSTRAINT [Renovacao_linha]
GO
ALTER TABLE [opc].[Tarifas]  WITH CHECK ADD  CONSTRAINT [Tarifas_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Tarifas] CHECK CONSTRAINT [Tarifas_empresa]
GO
ALTER TABLE [opc].[Terminais]  WITH CHECK ADD  CONSTRAINT [Terminais_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Terminais] CHECK CONSTRAINT [Terminais_empresa]
GO
ALTER TABLE [opc].[Troncos]  WITH CHECK ADD  CONSTRAINT [Troncos_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Troncos] CHECK CONSTRAINT [Troncos_empresa]
GO
ALTER TABLE [opc].[TServicos]  WITH CHECK ADD  CONSTRAINT [TServicos_terminal] FOREIGN KEY([TerminalId])
REFERENCES [opc].[Terminais] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[TServicos] CHECK CONSTRAINT [TServicos_terminal]
GO
ALTER TABLE [opc].[Veiculos]  WITH NOCHECK ADD  CONSTRAINT [Veiculos_classe] FOREIGN KEY([Classe])
REFERENCES [opc].[CVeiculos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Veiculos] NOCHECK CONSTRAINT [Veiculos_classe]
GO
ALTER TABLE [opc].[Veiculos]  WITH CHECK ADD  CONSTRAINT [Veiculos_empresa] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Veiculos] CHECK CONSTRAINT [Veiculos_empresa]
GO
ALTER TABLE [opc].[VeiculosAtt]  WITH CHECK ADD  CONSTRAINT [VeiculosAtt_atributo] FOREIGN KEY([Attributo])
REFERENCES [opc].[CVeiculosAtt] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[VeiculosAtt] CHECK CONSTRAINT [VeiculosAtt_atributo]
GO
ALTER TABLE [opc].[VeiculosAtt]  WITH CHECK ADD  CONSTRAINT [VeiculosAtt_classe] FOREIGN KEY([Classe])
REFERENCES [opc].[CVeiculos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[VeiculosAtt] CHECK CONSTRAINT [VeiculosAtt_classe]
GO
ALTER TABLE [opc].[Viagens]  WITH CHECK ADD  CONSTRAINT [Viagens_horario] FOREIGN KEY([HorarioId])
REFERENCES [opc].[Horarios] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Viagens] CHECK CONSTRAINT [Viagens_horario]
GO
ALTER TABLE [opc].[Viagens]  WITH CHECK ADD  CONSTRAINT [Viagens_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[LnPesquisas] ([Id])
GO
ALTER TABLE [opc].[Viagens] CHECK CONSTRAINT [Viagens_linha]
GO
ALTER TABLE [opc].[Viagens]  WITH CHECK ADD  CONSTRAINT [Viagens_periodo] FOREIGN KEY([PeriodoId])
REFERENCES [opc].[PrLinhas] ([Id])
GO
ALTER TABLE [opc].[Viagens] CHECK CONSTRAINT [Viagens_periodo]
GO
ALTER TABLE [opc].[Viagens]  WITH CHECK ADD  CONSTRAINT [Viagens_ponto] FOREIGN KEY([PontoId])
REFERENCES [opc].[PtLinhas] ([Id])
GO
ALTER TABLE [opc].[Viagens] CHECK CONSTRAINT [Viagens_ponto]
GO
ALTER TABLE [opc].[Viagens]  WITH CHECK ADD  CONSTRAINT [Viagens_veiculo] FOREIGN KEY([VeiculoId])
REFERENCES [opc].[Veiculos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Viagens] CHECK CONSTRAINT [Viagens_veiculo]
GO
ALTER TABLE [opc].[Viagens_temp]  WITH CHECK ADD  CONSTRAINT [Viagens_temp_horario] FOREIGN KEY([HorarioId])
REFERENCES [opc].[Horarios] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [opc].[Viagens_temp] CHECK CONSTRAINT [Viagens_temp_horario]
GO
ALTER TABLE [opc].[Viagens_temp]  WITH CHECK ADD  CONSTRAINT [Viagens_temp_linha] FOREIGN KEY([LinhaId])
REFERENCES [opc].[LnPesquisas] ([Id])
GO
ALTER TABLE [opc].[Viagens_temp] CHECK CONSTRAINT [Viagens_temp_linha]
GO
ALTER TABLE [opc].[Viagens_temp]  WITH CHECK ADD  CONSTRAINT [Viagens_temp_ponto] FOREIGN KEY([PontoId])
REFERENCES [opc].[PtLinhas] ([Id])
GO
ALTER TABLE [opc].[Viagens_temp] CHECK CONSTRAINT [Viagens_temp_ponto]
GO
ALTER TABLE [opc].[PtLinhas]  WITH CHECK ADD  CONSTRAINT [PtLinhas_destino_check] CHECK  ((([DestinoId] IS NULL OR [DestinoId]<>[Id]) AND [DestinoId]<>[OrigemId]))
GO
ALTER TABLE [opc].[PtLinhas] CHECK CONSTRAINT [PtLinhas_destino_check]
GO
ALTER TABLE [opc].[PtLinhas]  WITH CHECK ADD  CONSTRAINT [PtLinhas_origem_check] CHECK  ((([OrigemId] IS NULL OR [OrigemId]<>[Id]) AND [OrigemId]<>[DestinoId]))
GO
ALTER TABLE [opc].[PtLinhas] CHECK CONSTRAINT [PtLinhas_origem_check]
GO
