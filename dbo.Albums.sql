CREATE TABLE [dbo].[Albums] (
    [AlbumID]     INT           NOT NULL,
    [ArtistID]    INT           NOT NULL,
    [AlbumTitle]  NVARCHAR (50) NOT NULL,
    [Genre]       NVARCHAR (50) NOT NULL,
    [ReleaseDate] DATE          NOT NULL,
    PRIMARY KEY CLUSTERED ([AlbumID] ASC), 
    CONSTRAINT [FK_ArtistID] FOREIGN KEY (ArtistID) REFERENCES Artists(ArtistID), 
    
);

