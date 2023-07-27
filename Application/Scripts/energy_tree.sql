;WITH MRaw AS (
	SELECT 
		FullName = case 
			when isnull(D4.Val,D5.Val) is not null  
				then D2.Val + ' (' + isnull(D4.Val,D5.Val) + ') ' + D1.TypeSSN + ' ' + D6.Val
			when D2.Val is not null
				then N'ИТ ' + D6.Val + ' ' + D2.Val
			else D7.Val
				end,
		D1.ID AS Device_id,
		d1.MasterID
	FROM dbo.Devices AS D1 
	LEFT OUTER JOIN dbo.DevicesParams AS D2 ON D1.ID = D2.DeviceID AND D2.Name = 'ObjectName'
	LEFT OUTER JOIN dbo.DevicesParams AS D4 ON D1.ID = D4.DeviceID AND D4.Name = 'ResourceType'
	LEFT OUTER JOIN dbo.DevicesParams AS D5 ON D1.ID = D5.DeviceID AND D5.Name = 'ResType'
	LEFT OUTER JOIN dbo.DevicesParams AS D6 ON D1.ID = D6.DeviceID AND D6.Name = 'Number'
	LEFT OUTER JOIN dbo.DevicesParams AS D7 ON D1.ID = D7.DeviceID AND D7.Name = 'Name'
	--LEFT OUTER JOIN dbo.DevicesState AS ds ON ds.DeviceID = D1.ID and ds.Name = 'ONLINK' --and ds.Val = 1
	
	where D1.ID not in (20529, 17960, 29799)
		--and (ds.DeviceID is null or ds.Val = 1)
)
SELECT m.*
FROM MRaw m
LEFT OUTER JOIN dbo.Devices AS z ON m.MasterID = z.ID
ORDER BY FullName