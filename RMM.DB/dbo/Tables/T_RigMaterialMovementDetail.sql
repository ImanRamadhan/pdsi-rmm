CREATE TABLE [dbo].[T_RigMaterialMovementDetail] (
    [id]                       INT IDENTITY (1, 1) NOT NULL,
    [rig_material_movement_id] INT NULL,
    [trip_move_out]            INT NULL,
    [trip_move_in]             INT NULL,
    CONSTRAINT [PK_T_RigMaterialMovementDetail] PRIMARY KEY CLUSTERED ([id] ASC)
);

