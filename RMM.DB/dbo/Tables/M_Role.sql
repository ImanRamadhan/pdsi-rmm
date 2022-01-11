CREATE TABLE [dbo].[M_Role] (
    [id]                 INT           IDENTITY (1, 1) NOT NULL,
    [name]               NVARCHAR (50) NULL,
    [last_modified_by]   NVARCHAR (50) NULL,
    [last_modified_date] DATETIME      NULL,
    CONSTRAINT [PK_M_Role] PRIMARY KEY CLUSTERED ([id] ASC)
);

