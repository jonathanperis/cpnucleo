USE [DB-SANDBOX]
GO

SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPN_TB_SISTEMA] (
  [SIS_ID] uniqueidentifier NOT NULL PRIMARY KEY default newid(),
  [SIS_NOME] varchar(50) NOT NULL,
  [SIS_DESCRICAO] varchar(450) NOT NULL,
  [SIS_DATA_INCLUSAO] datetime NOT NULL default getdate(),
  [SIS_DATA_ALTERACAO] datetime NULL,
  [SIS_DATA_EXCLUSAO] datetime NULL,
  [SIS_ATIVO] bit NOT NULL default 1
);

CREATE TABLE [dbo].[CPN_TB_PROJETO] (
  [PROJ_ID] uniqueidentifier NOT NULL PRIMARY KEY default newid(),
  [PROJ_NOME] varchar(50) NOT NULL,
  [SIS_ID] uniqueidentifier NOT NULL FOREIGN KEY REFERENCES CPN_TB_SISTEMA (SIS_ID),  
  [PROJ_DATA_INCLUSAO] datetime NOT NULL default getdate(),
  [PROJ_DATA_ALTERACAO] datetime NULL,
  [PROJ_DATA_EXCLUSAO] datetime NULL,  
  [PROJ_ATIVO] bit NOT NULL default 1
);

CREATE TABLE [dbo].[CPN_TB_RECURSO] (
  [REC_ID] uniqueidentifier NOT NULL PRIMARY KEY default newid(),
  [REC_NOME] varchar(50) NOT NULL,
  [REC_LOGIN] varchar(50) NOT NULL,
  [REC_SENHA] varchar(64) NOT NULL,
  [REC_SENHA_SALT] varchar(64) NOT NULL,
  [REC_DATA_INCLUSAO] datetime NOT NULL default getdate(),
  [REC_DATA_ALTERACAO] datetime NULL,
  [REC_DATA_EXCLUSAO] datetime NULL,  
  [REC_ATIVO] bit NOT NULL default 1
);

CREATE TABLE [dbo].[CPN_TB_RECURSO_PROJETO] (
  [RPROJ_ID] uniqueidentifier NOT NULL PRIMARY KEY default newid(),
  [REC_ID] uniqueidentifier NOT NULL FOREIGN KEY REFERENCES CPN_TB_RECURSO (REC_ID),
  [PROJ_ID] uniqueidentifier NOT NULL FOREIGN KEY REFERENCES CPN_TB_PROJETO (PROJ_ID),  
  [RPROJ_DATA_INCLUSAO] datetime NOT NULL default getdate(),
  [RPROJ_DATA_EXCLUSAO] datetime NULL,  
  [RPROJ_ATIVO] bit NOT NULL default 1
);

CREATE TABLE [dbo].[CPN_TB_WORKFLOW] (
  [WOR_ID] uniqueidentifier NOT NULL PRIMARY KEY default newid(),
  [WOR_NOME] varchar(50) NOT NULL,
  [WOR_ORDEM] int NOT NULL,
  [WOR_DATA_INCLUSAO] datetime NOT NULL default getdate(),
  [WOR_DATA_ALTERACAO] datetime NULL,
  [WOR_DATA_EXCLUSAO] datetime NULL,  
  [WOR_ATIVO] bit NOT NULL default 1  
);

CREATE TABLE [dbo].[CPN_TB_IMPEDIMENTO] (
  [IMP_ID] uniqueidentifier NOT NULL PRIMARY KEY default newid(),
  [IMP_NOME] varchar(50) NOT NULL,
  [IMP_DATA_INCLUSAO] datetime NOT NULL default getdate(),
  [IMP_DATA_ALTERACAO] datetime NULL,
  [IMP_DATA_EXCLUSAO] datetime NULL,  
  [IMP_ATIVO] bit NOT NULL default 1
);

CREATE TABLE [dbo].[CPN_TB_TIPO_TAREFA] (
  [TIP_ID] uniqueidentifier NOT NULL PRIMARY KEY default newid(),
  [TIP_NOME] varchar(50) NOT NULL,
  [TIP_IMAGE_CARD] varchar(100) NOT NULL,
  [TIP_DATA_INCLUSAO] datetime NOT NULL default getdate(),
  [TIP_DATA_ALTERACAO] datetime NULL,
  [TIP_DATA_EXCLUSAO] datetime NULL,  
  [TIP_ATIVO] bit NOT NULL default 1
);

CREATE TABLE [dbo].[CPN_TB_TAREFA] (
  [TAR_ID] uniqueidentifier NOT NULL PRIMARY KEY default newid(),
  [TAR_NOME] varchar(450) NOT NULL,
  [TAR_DATA_INICIO] datetime NOT NULL,
  [TAR_DATA_TERMINO] datetime NOT NULL,
  [TAR_QTD_HORAS] int NOT NULL,
  [TAR_DETALHE] varchar(1000) NULL,
  [PROJ_ID] uniqueidentifier NOT NULL FOREIGN KEY REFERENCES CPN_TB_PROJETO (PROJ_ID),
  [WOR_ID] uniqueidentifier NOT NULL FOREIGN KEY REFERENCES CPN_TB_WORKFLOW (WOR_ID),
  [REC_ID] uniqueidentifier NOT NULL FOREIGN KEY REFERENCES CPN_TB_RECURSO (REC_ID),
  [TIP_ID] uniqueidentifier NOT NULL FOREIGN KEY REFERENCES CPN_TB_TIPO_TAREFA (TIP_ID),  
  [TAR_DATA_INCLUSAO] datetime NOT NULL default getdate(),
  [TAR_DATA_ALTERACAO] datetime NULL,
  [TAR_DATA_EXCLUSAO] datetime NULL,  
  [TAR_ATIVO] bit NOT NULL default 1
);

CREATE TABLE [dbo].[CPN_TB_RECURSO_TAREFA] (
  [RTAR_ID] uniqueidentifier NOT NULL PRIMARY KEY default newid(),
  [RTAR_PERCENTUAL] int NOT NULL,
  [REC_ID] uniqueidentifier NOT NULL FOREIGN KEY REFERENCES CPN_TB_RECURSO (REC_ID),
  [TAR_ID] uniqueidentifier NOT NULL FOREIGN KEY REFERENCES CPN_TB_TAREFA (TAR_ID),  
  [RTAR_DATA_INCLUSAO] datetime NOT NULL default getdate(),
  [RTAR_DATA_ALTERACAO] datetime NULL,
  [RTAR_DATA_EXCLUSAO] datetime NULL,  
  [RTAR_ATIVO] bit NOT NULL default 1
);

CREATE TABLE [dbo].[CPN_TB_TAREFA_IMPEDIMENTO] (
  [ITAR_ID] uniqueidentifier NOT NULL PRIMARY KEY default newid(),
  [ITAR_DESCRICAO] varchar(450) NOT NULL,
  [TAR_ID] uniqueidentifier NOT NULL FOREIGN KEY REFERENCES CPN_TB_TAREFA (TAR_ID),  
  [IMP_ID] uniqueidentifier NOT NULL FOREIGN KEY REFERENCES CPN_TB_IMPEDIMENTO (IMP_ID),  
  [ITAR_DATA_INCLUSAO] datetime NOT NULL default getdate(),
  [ITAR_DATA_ALTERACAO] datetime NULL,
  [ITAR_DATA_EXCLUSAO] datetime NULL,  
  [ITAR_ATIVO] bit NOT NULL default 1
);

CREATE TABLE [dbo].[CPN_TB_LANCAMENTO] (
  [LANC_ID] uniqueidentifier NOT NULL PRIMARY KEY default newid(),
  [LANC_DESCRICAO] varchar(450) NOT NULL,
  [LANC_DATA_LANCAMENTO] datetime NOT NULL,
  [LANC_QTD_HORAS] int NOT NULL,
  [TAR_ID] uniqueidentifier NOT NULL FOREIGN KEY REFERENCES CPN_TB_TAREFA (TAR_ID),  
  [REC_ID] uniqueidentifier NOT NULL FOREIGN KEY REFERENCES CPN_TB_RECURSO (REC_ID),  
  [LANC_DATA_INCLUSAO] datetime NOT NULL default getdate(),
  [LANC_DATA_ALTERACAO] datetime NULL,
  [LANC_DATA_EXCLUSAO] datetime NULL,  
  [LANC_ATIVO] bit NOT NULL default 1
);