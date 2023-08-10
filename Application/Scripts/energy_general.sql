
	--Общая часть показаний для сервера энергии

	insert #measureIds (MeasureID, ParamID, Kind, Range, Tarif)
	SELECT 
		mid.Id as MeasureID,
		mid.ParamID,
		mid.Kind,
		mid.Range,
		mid.Tarif
	FROM MeasureID mid WITH(NOLOCK)
	INNER JOIN #measureParams mp on mp.ParamID = mid.ParamID
	WHERE mid.Tarif = 0 and mid.Kind in (0, 2)

	--SET @t2 = GETDATE();
	--insert @timers (stepName, diff_ml)
	--SELECT 'insert #measureIds', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;
	--SET @t1 = GETDATE();



	--MeasuresCurrent end (на begin нет смысла)
	insert #measures (MeasureID, ParamID, Kind, Range, Tarif, Time, ValOut)
	SELECT 
		mid.MeasureID as MeasureID,
		mid.ParamID,
		mid.Kind,
		mid.Range,
		mid.Tarif,
		[Time]=md.Time,
		md.ValOut
	FROM MeasuresCurrent md  WITH(NOLOCK)
	INNER JOIN #measureIds mid on mid.MeasureID = md.MeasureID
	where (md.Time > DateADD(Day, -2, @endDate) and md.Time < @endDate) and mid.Kind = 0

	--SET @t2 = GETDATE();
	--insert @timers (stepName, diff_ml)
	--SELECT 'insert #measuresCurrent ', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;
	--SET @t1 = GETDATE();


	
	--MeasuresLog end
	insert #measures (MeasureID, ParamID, Kind, Range, Tarif, Time, ValOut)
	SELECT 
		ml.MeasureID,
		mid.ParamID,
		mid.Kind,
		mid.Range,
		mid.Tarif,
		ml.Time,
		ml.ValOut
	FROM #measureIds mid
	CROSS APPLY (select top 1 * from MeasuresLog x WITH(NOLOCK) where mid.MeasureID=x.MeasureID AND x.Time < @endDate and x.Time > DATEADD(WEEK, -2, @endDate) order by Time desc) ml
	LEFT JOIN #measures m on m.ParamID = mid.ParamID and m.Time >= DateADD(day, -1, @endDate) and m.Time < @endDate
	WHERE
		m.ParamID is null
		and mid.Kind = 0
	
	--SET @t2 = GETDATE();
	--insert @timers (stepName, diff_ml)
	--SELECT 'insert #measuresLog end ', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;
	--SET @t1 = GETDATE();



	--MeasuresLog begin
	insert #measures (MeasureID, ParamID, Kind, Range, Tarif, Time, ValOut)
	SELECT 
		ml.MeasureID,
		mid.ParamID,
		mid.Kind,
		mid.Range,
		mid.Tarif,
		ml.Time,
		ml.ValOut
	FROM #measureIds mid
	CROSS APPLY (select top 1 * from MeasuresLog x WITH(NOLOCK) where mid.MeasureID=x.MeasureID AND x.Time < @beginDate and x.Time > DATEADD(WEEK, -2, @beginDate) order by Time desc) ml
	WHERE mid.Kind = 0

	--SET @t2 = GETDATE();
	--insert @timers (stepName, diff_ml)
	--SELECT 'insert #measuresLog begin', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;
	--SET @t1 = GETDATE();



	--MeasuresCut begin
	insert #measures (MeasureID, ParamID, Kind, Range, Tarif, Time, ValOut)
	SELECT 
		mid.MeasureID,
		mid.ParamID,
		mid.Kind,
		mid.Range,
		mid.Tarif,
		[Time]=md.Time,
		md.ValOut
	FROM #measureIds mid
	CROSS APPLY (select top 1 * from MeasuresCut x WITH(NOLOCK) where mid.MeasureID=x.MeasureID and x.Time > DATEADD(MONTH, -1, @beginDate) AND x.Time < @beginDate order by Time desc) md
	LEFT JOIN #measures m on m.ParamID = mid.ParamID and m.Time >= DateADD(day, -1, @beginDate) and m.Time < @beginDate
	where m.ParamID is null
		and mid.Kind = 0
	
	--SET @t2 = GETDATE();
	--insert @timers (stepName, diff_ml)
	--SELECT 'insert #measuresCut begin', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;
	--SET @t1 = GETDATE();



	--MeasuresCut end
	insert #measures (MeasureID, ParamID, Kind, Range, Tarif, Time, ValOut)
	SELECT 
		mid.MeasureID,
		mid.ParamID,
		mid.Kind,
		mid.Range,
		mid.Tarif,
		[Time]=md.Time,
		md.ValOut
	FROM #measureIds mid
	CROSS APPLY (select top 1 * from MeasuresCut x WITH(NOLOCK) where mid.MeasureID=x.MeasureID and x.Time > DATEADD(MONTH, -1, @endDate) AND x.Time < @endDate order by Time desc) md
	LEFT JOIN #measures m on m.ParamID = mid.ParamID and m.Time >= DateADD(day, -1, @endDate) and m.Time < @endDate
	where m.ParamID is null
		and mid.Kind = 0

	--SET @t2 = GETDATE();
	--insert @timers (stepName, diff_ml)
	--SELECT 'insert #measuresCut end', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;
	--SET @t1 = GETDATE();



	--MeasuresDay begin
	insert #measures (MeasureID, ParamID, Kind, Range, Tarif, Time, ValOut)
	SELECT 
		mid.MeasureID,
		mid.ParamID,
		mid.Kind,
		mid.Range,
		mid.Tarif,
		[Time]=md.Timemark,
		md.ValOut
	FROM #measureIds mid
	CROSS APPLY (select top 1 * from MeasuresDay x WITH(NOLOCK) where mid.MeasureID=x.MeasureID and x.Timemark > DateADD(WEEK, -2, @beginDate) and x.Timemark < @beginDate order by Timemark desc) md
	LEFT JOIN #measures m on m.ParamID = mid.ParamID and m.Time >= DateADD(day, -1, @beginDate) and m.Time < @beginDate
	where m.ParamID is null
		and mid.Kind = 2

	--SET @t2 = GETDATE();
	--insert @timers (stepName, diff_ml)
	--SELECT 'insert #measuresDay begin', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;
	--SET @t1 = GETDATE();


	
	--MeasuresDay end
	insert #measures (MeasureID, ParamID, Kind, Range, Tarif, Time, ValOut)
	SELECT 
		mid.MeasureID,
		mid.ParamID,
		mid.Kind,
		mid.Range,
		mid.Tarif,
		[Time]=md.Timemark,
		md.ValOut
	FROM #measureIds mid
	CROSS APPLY (select top 1 * from MeasuresDay x WITH(NOLOCK) where mid.MeasureID=x.MeasureID and x.Timemark > DateADD(WEEK, -2, @endDate) and x.Timemark < @endDate order by Timemark desc) md
	LEFT JOIN #measures m on m.ParamID = mid.ParamID and m.Time >= DateADD(day, -1, @endDate) and m.Time < @endDate
	where m.ParamID is null
		and mid.Kind = 2

	--SET @t2 = GETDATE();
	--insert @timers (stepName, diff_ml)
	--SELECT 'insert #measuresDay end', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;
	--SET @t1 = GETDATE();


	--MeasuresLong end
	insert #measures (MeasureID, ParamID, Kind, Range, Tarif, Time, ValOut)
	SELECT 
		mid.MeasureID,
		mid.ParamID,
		mid.Kind,
		mid.Range,
		mid.Tarif,
		[Time]=md.Timemark,
		md.ValOut
	FROM #measureIds mid
	CROSS APPLY (select top 1 * from MeasuresLong x WITH(NOLOCK) where mid.MeasureID=x.MeasureID and x.Timemark > DateADD(MONTH, -2, @endDate) and x.Timemark < @endDate order by Timemark desc) md
	LEFT JOIN #measures m on m.ParamID = mid.ParamID and m.Time >= DateADD(day, -1, @endDate) and m.Time < @endDate
	where m.ParamID is null
		and mid.Kind = 2

	--SET @t2 = GETDATE();
	--insert @timers (stepName, diff_ml)
	--SELECT 'insert #measuresLong ', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;
	--SET @t1 = GETDATE();


	--MeasuresHour begin
	insert #measures (MeasureID, ParamID, Kind, Range, Tarif, Time, ValOut)
	SELECT
		mid.MeasureID,
		mid.ParamID,
		mid.Kind,
		mid.Range,
		mid.Tarif,
		[Time]=md.Timemark,
		md.ValOut
	FROM #measureIds mid
	CROSS APPLY (select top 1 * from MeasuresHour x WITH(NOLOCK) where mid.MeasureID=x.MeasureID and x.Timemark > DateADD(HOUR, -4, @beginDate) and x.Timemark < @beginDate order by Timemark desc) md
	LEFT JOIN #measures m on m.ParamID = mid.ParamID and m.Time >= DateADD(day, -2, @beginDate) and m.Time < @beginDate
	where m.ParamID is null
		and mid.Kind = 2

	--SET @t2 = GETDATE();
	--insert @timers (stepName, diff_ml)
	--SELECT 'insert #measuresHour begin', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;
	--SET @t1 = GETDATE();


	--MeasuresHour end
	insert #measures (MeasureID, ParamID, Kind, Range, Tarif, Time, ValOut)
	SELECT 
		mid.MeasureID,
		mid.ParamID,
		mid.Kind,
		mid.Range,
		mid.Tarif,
		[Time]=md.Timemark,
		md.ValOut
	FROM #measureIds mid
	CROSS APPLY (select top 1 * from MeasuresHour x WITH(NOLOCK) where mid.MeasureID=x.MeasureID and x.Timemark > DateADD(HOUR, -4, @endDate) and x.Timemark < @endDate order by Timemark desc) md
	LEFT JOIN #measures m on m.ParamID = mid.ParamID and m.Time >= DateADD(day, -2, @endDate) and m.Time < @endDate
	where m.ParamID is null
		and mid.Kind = 2

	--SET @t2 = GETDATE();
	--insert @timers (stepName, diff_ml)
	--SELECT 'insert #measuresHour end', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;
	--SET @t1 = GETDATE();


	insert #measuresBegin (DeviceId, MeasureID, ParamID, Time, ValOut)
	select 
		DeviceId,
		MeasureId,
		ParamID,
		Time,
		ValOut
	from
	(
		select 
			m.DeviceId,
			m.MeasureId,
			z.ParamID,
			z.Time,
			ROUND(case when m.TypeSSN = 'MCAL.COMBI' then z.ValOut * 0.000859845 else z.ValOut end, 3) as ValOut
			,ROW_NUMBER() OVER (Partition by m.DeviceId, m.ParamID order by z.Time desc) as row
		from #measureParams m 
		INNER JOIN #measures z on m.ParamID = z.ParamID
		where z.Time < @beginDate
	) y
	where y.row = 1

	--SET @t2 = GETDATE();
	--insert @timers (stepName, diff_ml)
	--SELECT 'insert #measuresBegin2', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;
	--SET @t1 = GETDATE();


	insert #measuresEnd (DeviceId, MeasureID, ParamID, Time, ValOut)
	select 
		DeviceId,
		MeasureId,
		ParamID,
		Time,
		ValOut
	from
	(
		select 
			m.DeviceId,
			m.MeasureId,
			z.ParamID,
			z.Time,
			ROUND(case when m.TypeSSN = 'MCAL.COMBI' then z.ValOut * 0.000859845 else z.ValOut end, 3) as ValOut
			,ROW_NUMBER() OVER (Partition by m.DeviceId, m.ParamID order by z.Time desc) as row
		from #measureParams m
		INNER JOIN #measures z on m.ParamID = z.ParamID
		where z.Time > DateADD(MONTH, -1, @endDate) and z.Time < @endDate
	) y
	where y.row = 1

	--SET @t2 = GETDATE();
	--insert @timers (stepName, diff_ml)
	--SELECT 'insert #measuresEnd2', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;
	--SET @t1 = GETDATE();

