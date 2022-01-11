CREATE TABLE [dbo].[M_RoleManagement] (
    [username]           NVARCHAR (50) NOT NULL,
    [email]              NVARCHAR (50) NULL,
    [role_id]            INT           NULL,
    [last_modified_by]   NVARCHAR (50) NULL,
    [last_modified_date] DATETIME      NULL,
    CONSTRAINT [PK_M_RoleManagement] PRIMARY KEY CLUSTERED ([username] ASC)
);

