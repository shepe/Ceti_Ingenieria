
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/12/2015 22:44:50
-- Generated from EDMX file: C:\Users\user\Source\Repos\Ceti_Ingenieria\RulesDef_Dic\RulesDef_Dic\RulesModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [RulesDefDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Rules_RulesDef]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RulesDefSet] DROP CONSTRAINT [FK_Rules_RulesDef];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DictionarySet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DictionarySet];
GO
IF OBJECT_ID(N'[dbo].[RulesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RulesSet];
GO
IF OBJECT_ID(N'[dbo].[RulesDefSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RulesDefSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DictionarySet'
CREATE TABLE [dbo].[DictionarySet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PropAtm] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RulesSet'
CREATE TABLE [dbo].[RulesSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Result] int  NOT NULL
);
GO

-- Creating table 'RulesDefSet'
CREATE TABLE [dbo].[RulesDefSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Prop] int  NOT NULL,
    [RulesId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'DictionarySet'
ALTER TABLE [dbo].[DictionarySet]
ADD CONSTRAINT [PK_DictionarySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RulesSet'
ALTER TABLE [dbo].[RulesSet]
ADD CONSTRAINT [PK_RulesSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RulesDefSet'
ALTER TABLE [dbo].[RulesDefSet]
ADD CONSTRAINT [PK_RulesDefSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RulesId] in table 'RulesDefSet'
ALTER TABLE [dbo].[RulesDefSet]
ADD CONSTRAINT [FK_Rules_RulesDef]
    FOREIGN KEY ([RulesId])
    REFERENCES [dbo].[RulesSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Rules_RulesDef'
CREATE INDEX [IX_FK_Rules_RulesDef]
ON [dbo].[RulesDefSet]
    ([RulesId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------