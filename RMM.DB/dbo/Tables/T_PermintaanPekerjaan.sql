CREATE TABLE [dbo].[T_PermintaanPekerjaan] (
    [id]                 INT           IDENTITY (1, 1) NOT NULL,
    [no_wo]              NVARCHAR (50) NULL,
    [no_activity]        NVARCHAR (50) NULL,
    [judul_pekerjaan]    NVARCHAR (50) NULL,
    [tanggal_pekerjaan]  DATE          NULL,
    [lokasi_asal]        NVARCHAR (50) NULL,
    [cp_lokasi_asal]     NVARCHAR (50) NULL,
    [lokasi_tujuan]      NVARCHAR (50) NULL,
    [cp_lokasi_tujuan]   NVARCHAR (50) NULL,
    [detail_barang]      NVARCHAR (50) NULL,
    [keterangan]         TEXT          NULL,
    [last_modified_by]   NVARCHAR (50) NULL,
    [last_modified_date] DATETIME      NULL,
    [status_id]          INT           NULL,
    [approved_by]        NVARCHAR (50) NULL,
    [approved_date]      DATETIME      NULL,
    CONSTRAINT [PK_T_PermintaanPekerjaan] PRIMARY KEY CLUSTERED ([id] ASC)
);

