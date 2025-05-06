USE ExpenseManager
go

INSERT INTO ExpenseManager.dbo.Expense 
(UserId,CategoryId,Amount,Description,ExpenseStatus,RejectionReason,ReceiptUrl,PaymentMethod,PaymentLocation,CreateUser,CreateDate,ModifyUser,ModifyDate,IsActive) 
VALUES
	 (6,1,750.00,N'Mesai Öğlen Yemeği',1,NULL,N'/receipts/6ad2bc57-5269-494f-aa92-7752200202b0_YemekHarcamaFis24052025.pdf',N'Kredi Kartı',N'İstanbul/Ümraniye',N'gokhan@gmail.com','2025-05-06 22:48:52.1060270',NULL,NULL,1),
	 (6,3,200.00,N'İspark',1,NULL,N'/receipts/857920e1-a44b-4fca-bb1b-e2fa55493fc0_OtoparkHarcamaFis05042025.pdf',N'Kredi Kartı',N'Kadıköy',N'gokhan@gmail.com','2025-05-06 22:51:04.5869540',NULL,NULL,1),
	 (6,4,2000.00,N'PointHotel İş gezisi',1,NULL,N'/receipts/c8f431f5-3dff-4d59-a85f-6b8a77c60065_HarcamaFis05062025.pdf',N'Nakit',N'Beykoz',N'gokhan@gmail.com','2025-05-06 22:52:07.8089810',NULL,NULL,1)
	 