USE [TibiaDB]
GO
ALTER TABLE [dbo].[VinculoUsuarioMensagem] DROP CONSTRAINT [FK_VinculoUsuarioMensagem_Usuario]
GO
ALTER TABLE [dbo].[VinculoUsuarioMensagem] DROP CONSTRAINT [FK_VinculoUsuarioMensagem_Mensagem]
GO
/****** Object:  Table [dbo].[VinculoUsuarioMensagem]    Script Date: 23/05/2018 19:56:53 ******/
DROP TABLE [dbo].[VinculoUsuarioMensagem]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 23/05/2018 19:56:53 ******/
DROP TABLE [dbo].[Usuario]
GO
/****** Object:  Table [dbo].[Mensagem]    Script Date: 23/05/2018 19:56:53 ******/
DROP TABLE [dbo].[Mensagem]
GO
/****** Object:  Table [dbo].[ConfiguracaoEmail]    Script Date: 23/05/2018 19:56:53 ******/
DROP TABLE [dbo].[ConfiguracaoEmail]
GO
/****** Object:  Table [dbo].[ParametroSistema]    Script Date: 23/05/2018 19:56:53 ******/
DROP TABLE [dbo].[ParametroSistema]
GO
/****** Object:  Table [dbo].[ConfiguracaoEmail]    Script Date: 23/05/2018 19:56:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConfiguracaoEmail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[Senha] [nvarchar](max) NULL,
	[Dominio] [nvarchar](max) NULL,
	[Host] [nvarchar](max) NULL,
	[Port] [int] NULL,
	[SSl] [bit] NULL,
	[EmailSuporte] [nvarchar](max) NULL,
	[DataCadastro] [datetime2](7) NULL,
	[DataAlteracao] [datetime2](7) NULL,
	[Excluido] [bit] NOT NULL,
 CONSTRAINT [PK_ConfiguracaoEmail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mensagem]    Script Date: 23/05/2018 19:56:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mensagem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Corpo] [nvarchar](max) NOT NULL,
	[Cabecalho] [nvarchar](max) NOT NULL,
	[Imagem] [image] NOT NULL,
	[Obrigatorio] [bit] NOT NULL,
	[DataCadastro] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Mensagem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 23/05/2018 19:56:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Senha] [nvarchar](max) NOT NULL,
	/*[Valido] [bit] NOT NULL,*/
	[UsouFreeTrial] [bit] NOT NULL,
	[DataExpirar] [datetime2](7) NULL,
	[CodigoMaquina] [nvarchar](max) NULL,
	[DataUltimoAcesso] [datetime2](7) NULL,
	[Logado] [bit] NOT NULL,
	[ChaveRecuperacao] [nvarchar](max) NULL,
	[NomePlayerTibia] [nvarchar](max) NULL,
	[DataCadastro] [datetime2](7) NOT NULL,
	[Excluido] [bit] NOT NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VinculoUsuarioMensagem]    Script Date: 23/05/2018 19:56:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VinculoUsuarioMensagem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdMensagem] [int] NOT NULL,
	[JaRecebeu] [bit] NOT NULL,
	[Excluido] [bit] NOT NULL,
 CONSTRAINT [PK_VinculoUsuarioMensagem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ParametroSistema]    Script Date: 23/05/2018 19:56:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParametroSistema](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](max) NOT NULL,
	[Valor] [nvarchar](max) NOT NULL,
	[Descricao] [nvarchar](max) NOT NULL,
	[DataCadastro] [datetime2](7) NOT NULL,
	[Excluido] [bit] NOT NULL,
 CONSTRAINT [PK_ParametroSistema] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VinculoUsuarioMensagem]  WITH CHECK ADD  CONSTRAINT [FK_VinculoUsuarioMensagem_Mensagem] FOREIGN KEY([IdMensagem])
REFERENCES [dbo].[Mensagem] ([Id])
GO
ALTER TABLE [dbo].[VinculoUsuarioMensagem] CHECK CONSTRAINT [FK_VinculoUsuarioMensagem_Mensagem]
GO
ALTER TABLE [dbo].[VinculoUsuarioMensagem]  WITH CHECK ADD  CONSTRAINT [FK_VinculoUsuarioMensagem_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([Id])
GO
ALTER TABLE [dbo].[VinculoUsuarioMensagem] CHECK CONSTRAINT [FK_VinculoUsuarioMensagem_Usuario]


GO
SET IDENTITY_INSERT [dbo].[ParametroSistema] ON 
GO
INSERT [dbo].[ParametroSistema] ([Id], [Nome], [Valor], [Descricao], [DataCadastro], [Excluido]) VALUES (1, N'tempoTrial', N'5', N'Tempo Free Trial', CAST(N'2018-06-06T00:00:00.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[ParametroSistema] ([Id], [Nome], [Valor], [Descricao], [DataCadastro], [Excluido]) VALUES (2, N'valorProduto', N'1.99', N'Valor do Produto', CAST(N'2018-06-06T00:00:00.0000000' AS DateTime2), 0)
GO
SET IDENTITY_INSERT [dbo].[ParametroSistema] OFF
GO
