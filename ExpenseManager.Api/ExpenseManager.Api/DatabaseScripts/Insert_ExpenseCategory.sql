USE ExpenseManager
GO

INSERT INTO ExpenseManager.dbo.ExpenseCategory 
(Name,CreateUser,CreateDate,ModifyUser,ModifyDate,IsActive) 
VALUES
	 (N'Yemek',N'dilara@gmail.com','2025-05-04 23:44:19.7365970',NULL,NULL,1),
	 (N'Yol',N'dilara@gmail.com','2025-05-04 23:44:27.3791830',NULL,NULL,1),
	 (N'Otopark',N'dilara@gmail.com','2025-05-04 23:44:37.6158320',NULL,NULL,1),
	 (N'Konaklama',N'dilara@gmail.com','2025-05-05 22:51:03.0210130',N'dilara@gmail.com','2025-05-06 19:47:00.3994030',1),
	 (N'Park',N'dilara@gmail.com','2025-05-06 20:55:40.8653320',N'dilara@gmail.com','2025-05-06 20:56:08.6200540',0);
