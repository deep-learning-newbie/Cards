DELETE FROM Cards
DELETE FROM CardsRelationship
DELETE FROM CardsResources
DELETE FROM ImageCardsResources
DELETE FROM TableCardsResources
DELETE FROM Blobs


SELECT * FROM Cards
SELECT * FROM CardsRelationship
SELECT * FROM CardsResources
SELECT * FROM ImageCardsResources
SELECT * FROM TableCardsResources
SELECT * FROM Blobs

INSERT INTO Blobs(Data) SELECT Data
FROM OPENROWSET(BULK N'C:\dev\CardsVova2\Cards\MainApp\Resources\Images\340719-200.png', SINGLE_BLOB) AS ImageSource(Data);

INSERT INTO Blobs(Data) SELECT Data
FROM OPENROWSET(BULK N'C:\dev\CardsVova2\Cards\MainApp\Resources\Images\340719-200.png', SINGLE_BLOB) AS ImageSource(Data);

INSERT INTO Blobs(Data) SELECT Data
FROM OPENROWSET(BULK N'C:\dev\CardsVova2\Cards\MainApp\Resources\Images\340719-200.png', SINGLE_BLOB) AS ImageSource(Data);

INSERT INTO Cards (Title) VALUES('Card 1')
INSERT INTO Cards (Title) VALUES('Card 2')
INSERT INTO CardsRelationship (ParentId, ChildId) VALUES (1, 2)
INSERT INTO CardsResources (CardId, ResourceType) VALUES(1, 0)
INSERT INTO CardsResources (CardId, ResourceType) VALUES(2, 0)
INSERT INTO CardsResources (CardId, ResourceType) VALUES(2, 0)
INSERT INTO CardsResources (CardId, ResourceType) VALUES(2, 1)
INSERT INTO ImageCardsResources (BlobId, Body, ResourceId) VALUES(1, 'Test Image 1', 1)
INSERT INTO ImageCardsResources (BlobId, Body, ResourceId) VALUES(2, 'Test Image 2', 2)
INSERT INTO ImageCardsResources (BlobId, Body, ResourceId) VALUES(3, 'Test Image 3', 3)
INSERT INTO TableCardsResources (Column1, Column2, ResourceId) VALUES('Test 1', 'Test 2', 4)