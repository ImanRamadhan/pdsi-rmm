CREATE TABLE [dbo].[T_RigMaterialMovement] (
    [id]                 INT           IDENTITY (1, 1) NOT NULL,
    [area]               NVARCHAR (50) NULL,
    [rig]                NVARCHAR (50) NULL,
    [rute_dari]          NVARCHAR (50) NULL,
    [rute_ke]            NVARCHAR (50) NULL,
    [jarak]              FLOAT (53)    NULL,
    [transporter_id]     INT           NULL,
    [tanggal_mulai]      DATETIME      NULL,
    [target_hari]        INT           NULL,
    [target_trip]        INT           NULL,
    [biaya]              BIGINT        NULL,
    [last_modified_by]   NVARCHAR (50) NULL,
    [last_modified_date] DATETIME      NULL,
    CONSTRAINT [PK_T_RigMaterialMovement] PRIMARY KEY CLUSTERED ([id] ASC)
);

