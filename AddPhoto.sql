UPDATE rooms 
SET photo = (SELECT BulkColumn FROM Openrowset(Bulk 'C:\Users\Admin\Downloads\3.jpg', Single_Blob) as img)
WHERE roomId = 4