IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE TABLE [Categorias] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(50) NOT NULL,
        [Descricao] nvarchar(250) NULL,
        [ValorDiariaBase] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE TABLE [Clientes] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(150) NOT NULL,
        [CPF] nvarchar(14) NOT NULL,
        [Email] nvarchar(100) NOT NULL,
        [Telefone] nvarchar(20) NOT NULL,
        [CNH] nvarchar(20) NOT NULL,
        [DataCadastro] datetime2 NOT NULL,
        CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE TABLE [Fabricantes] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(100) NOT NULL,
        [PaisOrigem] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_Fabricantes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE TABLE [Veiculos] (
        [Id] int NOT NULL IDENTITY,
        [Modelo] nvarchar(100) NOT NULL,
        [AnoFabricacao] int NOT NULL,
        [Quilometragem] int NOT NULL,
        [Placa] nvarchar(10) NOT NULL,
        [Cor] nvarchar(30) NOT NULL,
        [Status] int NOT NULL,
        [FabricanteId] int NOT NULL,
        [CategoriaId] int NOT NULL,
        CONSTRAINT [PK_Veiculos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Veiculos_Categorias_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Veiculos_Fabricantes_FabricanteId] FOREIGN KEY ([FabricanteId]) REFERENCES [Fabricantes] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE TABLE [Alugueis] (
        [Id] int NOT NULL IDENTITY,
        [ClienteId] int NOT NULL,
        [VeiculoId] int NOT NULL,
        [DataInicio] datetime2 NOT NULL,
        [DataPrevistaDevolucao] datetime2 NOT NULL,
        [DataDevolucaoEfetiva] datetime2 NULL,
        [KmInicial] int NOT NULL,
        [KmFinal] int NULL,
        [ValorDiaria] decimal(18,2) NOT NULL,
        [ValorTotal] decimal(18,2) NOT NULL,
        [Status] int NOT NULL,
        CONSTRAINT [PK_Alugueis] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Alugueis_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Alugueis_Veiculos_VeiculoId] FOREIGN KEY ([VeiculoId]) REFERENCES [Veiculos] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE TABLE [Pagamentos] (
        [Id] int NOT NULL IDENTITY,
        [AluguelId] int NOT NULL,
        [DataPagamento] datetime2 NOT NULL,
        [ValorPago] decimal(18,2) NOT NULL,
        [MetodoPagamento] int NOT NULL,
        [Status] int NOT NULL,
        CONSTRAINT [PK_Pagamentos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Pagamentos_Alugueis_AluguelId] FOREIGN KEY ([AluguelId]) REFERENCES [Alugueis] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Descricao', N'Nome', N'ValorDiariaBase') AND [object_id] = OBJECT_ID(N'[Categorias]'))
        SET IDENTITY_INSERT [Categorias] ON;
    EXEC(N'INSERT INTO [Categorias] ([Id], [Descricao], [Nome], [ValorDiariaBase])
    VALUES (1, N''Carros compactos e econômicos, ideais para o dia a dia na cidade.'', N''Econômico'', 110.0),
    (2, N''Conforto, amplo porta-malas e excelente dirigibilidade para viagens.'', N''Sedan Médio'', 175.0),
    (3, N''Espaço, robustez e versatilidade para toda a família.'', N''SUV'', 240.0),
    (4, N''Alto padrão, acabamento sofisticado e máxima performance.'', N''Premium'', 380.0)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Descricao', N'Nome', N'ValorDiariaBase') AND [object_id] = OBJECT_ID(N'[Categorias]'))
        SET IDENTITY_INSERT [Categorias] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CNH', N'CPF', N'DataCadastro', N'Email', N'Nome', N'Telefone') AND [object_id] = OBJECT_ID(N'[Clientes]'))
        SET IDENTITY_INSERT [Clientes] ON;
    EXEC(N'INSERT INTO [Clientes] ([Id], [CNH], [CPF], [DataCadastro], [Email], [Nome], [Telefone])
    VALUES (1, N''12345678900'', N''123.456.789-01'', ''2025-01-15T10:00:00.0000000Z'', N''carlos.silva@email.com'', N''Carlos Eduardo Silva'', N''(11) 98765-4321''),
    (2, N''98765432100'', N''987.654.321-09'', ''2025-02-01T14:30:00.0000000Z'', N''mariana.santos@email.com'', N''Mariana Costa Santos'', N''(21) 99876-5432''),
    (3, N''45678912300'', N''456.789.123-45'', ''2025-02-20T09:15:00.0000000Z'', N''roberto.lima@email.com'', N''Roberto Ferreira Lima'', N''(31) 97654-3210'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CNH', N'CPF', N'DataCadastro', N'Email', N'Nome', N'Telefone') AND [object_id] = OBJECT_ID(N'[Clientes]'))
        SET IDENTITY_INSERT [Clientes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome', N'PaisOrigem') AND [object_id] = OBJECT_ID(N'[Fabricantes]'))
        SET IDENTITY_INSERT [Fabricantes] ON;
    EXEC(N'INSERT INTO [Fabricantes] ([Id], [Nome], [PaisOrigem])
    VALUES (1, N''Toyota'', N''Japão''),
    (2, N''Volkswagen'', N''Alemanha''),
    (3, N''Chevrolet'', N''Estados Unidos''),
    (4, N''Fiat'', N''Itália''),
    (5, N''Hyundai'', N''Coreia do Sul'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome', N'PaisOrigem') AND [object_id] = OBJECT_ID(N'[Fabricantes]'))
        SET IDENTITY_INSERT [Fabricantes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AnoFabricacao', N'CategoriaId', N'Cor', N'FabricanteId', N'Modelo', N'Placa', N'Quilometragem', N'Status') AND [object_id] = OBJECT_ID(N'[Veiculos]'))
        SET IDENTITY_INSERT [Veiculos] ON;
    EXEC(N'INSERT INTO [Veiculos] ([Id], [AnoFabricacao], [CategoriaId], [Cor], [FabricanteId], [Modelo], [Placa], [Quilometragem], [Status])
    VALUES (1, 2023, 2, N''Prata'', 1, N''Corolla XEi 2.0'', N''BRA2E19'', 18500, 1),
    (2, 2024, 1, N''Branco'', 2, N''Polo Track 1.0'', N''ABC1D23'', 8200, 1),
    (3, 2023, 3, N''Azul Eclipse'', 3, N''Tracker Premier 1.2 Turbo'', N''XYZ9K88'', 24000, 2),
    (4, 2022, 1, N''Vermelho'', 4, N''Argo Drive 1.0'', N''MNO4T56'', 35000, 1),
    (5, 2024, 3, N''Preto Onix'', 5, N''Creta Ultimate 2.0'', N''HYU7B44'', 5100, 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AnoFabricacao', N'CategoriaId', N'Cor', N'FabricanteId', N'Modelo', N'Placa', N'Quilometragem', N'Status') AND [object_id] = OBJECT_ID(N'[Veiculos]'))
        SET IDENTITY_INSERT [Veiculos] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClienteId', N'DataDevolucaoEfetiva', N'DataInicio', N'DataPrevistaDevolucao', N'KmFinal', N'KmInicial', N'Status', N'ValorDiaria', N'ValorTotal', N'VeiculoId') AND [object_id] = OBJECT_ID(N'[Alugueis]'))
        SET IDENTITY_INSERT [Alugueis] ON;
    EXEC(N'INSERT INTO [Alugueis] ([Id], [ClienteId], [DataDevolucaoEfetiva], [DataInicio], [DataPrevistaDevolucao], [KmFinal], [KmInicial], [Status], [ValorDiaria], [ValorTotal], [VeiculoId])
    VALUES (1, 1, ''2025-03-05T07:30:00.0000000Z'', ''2025-03-01T08:00:00.0000000Z'', ''2025-03-05T08:00:00.0000000Z'', 18500, 17900, 2, 175.0, 700.0, 1),
    (2, 2, NULL, ''2025-03-10T09:00:00.0000000Z'', ''2025-03-15T09:00:00.0000000Z'', NULL, 24000, 1, 240.0, 1200.0, 3)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClienteId', N'DataDevolucaoEfetiva', N'DataInicio', N'DataPrevistaDevolucao', N'KmFinal', N'KmInicial', N'Status', N'ValorDiaria', N'ValorTotal', N'VeiculoId') AND [object_id] = OBJECT_ID(N'[Alugueis]'))
        SET IDENTITY_INSERT [Alugueis] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AluguelId', N'DataPagamento', N'MetodoPagamento', N'Status', N'ValorPago') AND [object_id] = OBJECT_ID(N'[Pagamentos]'))
        SET IDENTITY_INSERT [Pagamentos] ON;
    EXEC(N'INSERT INTO [Pagamentos] ([Id], [AluguelId], [DataPagamento], [MetodoPagamento], [Status], [ValorPago])
    VALUES (1, 1, ''2025-03-05T08:00:00.0000000Z'', 3, 2, 700.0)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AluguelId', N'DataPagamento', N'MetodoPagamento', N'Status', N'ValorPago') AND [object_id] = OBJECT_ID(N'[Pagamentos]'))
        SET IDENTITY_INSERT [Pagamentos] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Alugueis_ClienteId] ON [Alugueis] ([ClienteId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Alugueis_VeiculoId] ON [Alugueis] ([VeiculoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Categorias_Nome] ON [Categorias] ([Nome]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Clientes_CNH] ON [Clientes] ([CNH]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Clientes_CPF] ON [Clientes] ([CPF]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Clientes_Email] ON [Clientes] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Pagamentos_AluguelId] ON [Pagamentos] ([AluguelId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Veiculos_CategoriaId] ON [Veiculos] ([CategoriaId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Veiculos_FabricanteId] ON [Veiculos] ([FabricanteId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Veiculos_Placa] ON [Veiculos] ([Placa]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260920015637_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260920015637_InitialCreate', N'8.0.11');
END;
GO

COMMIT;
GO

