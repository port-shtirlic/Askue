
--аномальный расход с возможностью выставления верхнего и нижнего уровня диапазона
--set statistics time on
--set statistics io on

--SET ARITHABORT ON;

IF OBJECT_ID('tempdb.dbo.#result', 'U') IS NOT NULL
  DROP TABLE #result; 
IF OBJECT_ID('tempdb.dbo.#measures', 'U') IS NOT NULL
  DROP TABLE #measures; 
IF OBJECT_ID('tempdb.dbo.#address', 'U') IS NOT NULL
  DROP TABLE #address; 
IF OBJECT_ID('tempdb.dbo.#measureIds', 'U') IS NOT NULL
  DROP TABLE #measureIds; 
IF OBJECT_ID('tempdb.dbo.#measureParams', 'U') IS NOT NULL
  DROP TABLE #measureParams; 
IF OBJECT_ID('tempdb.dbo.#measuresBegin', 'U') IS NOT NULL
  DROP TABLE #measuresBegin; 
IF OBJECT_ID('tempdb.dbo.#measuresEnd', 'U') IS NOT NULL
  DROP TABLE #measuresEnd; 


DECLARE @t1 DATETIME;
DECLARE @t2 DATETIME;
DECLARE @timers TABLE (stepName nvarchar(255), diff_ml int)

SET @t1 = GETDATE();

--переменные внешние
declare @beginDate datetime, @endDate datetime, @min decimal(18, 2), @max decimal(18, 2)


--{1}
--set @beginDate = '20230614' set @endDate = '20230721'

set @beginDate = DATEADD(Day, 1, @beginDate)
set @endDate = DATEADD(Day, 1, @endDate)

--связка связка MeasureID и Devices
CREATE TABLE #measureParams (DeviceId int , MasterID int, DeviceNumber nvarchar(50), TypeSSN varchar(10), Comment nvarchar(512), resName nvarchar(255), MeasureId int, mpsName varchar(16), ParamID int,
isHvs bit, isGvs bit, hasGvs bit, hasHvs bit, hasTe bit, address nvarchar(255), location nvarchar(255))

--уникальные адреса из приборов
CREATE TABLE #address(address nvarchar(255), location nvarchar(255), deviceCount int)

--отфильтрованные MeasureID
CREATE TABLE  #measureIds (MeasureID int, ParamID int, Kind tinyint, Range tinyint, Tarif tinyint)
--показания из каждой таблцы по прибору
CREATE TABLE  #measures (MeasureId int, ParamID int, Kind tinyint, Range tinyint, Tarif tinyint, Time datetime INDEX IX_measures_Time NONCLUSTERED, ValOut float)

--последие показания на дату начала и конца
CREATE TABLE  #measuresBegin (DeviceId int, MeasureId int, ParamID int, Time datetime, ValOut float)
CREATE TABLE  #measuresEnd (DeviceId int, MeasureId int, ParamID int, Time datetime, ValOut float)


;WITH
mydevices AS (
	select
		DeviceId = D1.Id,
		D1.MasterID,
		D1.TypeSSN,
		D1.Comment,
		resName = res1.Val,
		hasGvs = case when res1.Val like 'ГВС%' then 1 else 0 end,
		hasHvs = case when res1.Val like 'ХВС%' then 1 else 0 end,
		hasTe = case when res1.Val like 'ТЭ%' then 1 else 0 end
	from dbo.Devices AS D1  WITH(NOLOCK)
	--{0}
	INNER JOIN dbo.DevicesParams AS res1 ON D1.ID = res1.DeviceID AND res1.Name = 'ResourceType'
	WHERE 
		res1.Val like 'ТЭ%' OR
		res1.Val like 'ГВС%' OR
		res1.Val like 'ХВС%'
)
insert #measureParams (DeviceId, MasterID, DeviceNumber, TypeSSN, Comment, resName, MeasureId, mpsName, ParamID, isHvs, isGvs, hasGvs, hasHvs, hasTe, address, location)
select *
from
(
	select 
		d1.DeviceId,
		d1.MasterID,
		D12.Val,
		d1.TypeSSN,
		d1.Comment,
		d1.resName,
		m.Id as MeasureId,
		mpsName = mps.Name,
		m.ParamID,
		case 
			when D1.TypeSSN='PULSART' and D1.hasTe = 1 and (mps.Name='V1' or mps.Name='V3') then 1
			when D1.hasHvs = 1 then 1 else 0 end as isHvs,
		case 
			when D1.TypeSSN='PULSART' and D1.hasTe = 1 and (mps.Name='V2' or mps.Name='V4') then 1
			when D1.hasGvs = 1 then 1 else 0 end as isGvs,
		D1.hasGvs,
		D1.hasHvs,
		D1.hasTe,
		D3.Val as Address,
		D2.Val as location
	from mydevices d1 WITH(NOLOCK)
	INNER JOIN MeasureID m on d1.DeviceId = m.DeviceID
		and m.Kind = 0 and m.Range = 0 and m.Tarif = 0
	INNER JOIN ChannelsParams cp on cp.ID = m.ParamID
	INNER JOIN MetaParamsSet mps on cp.MetaID = mps.ID
	LEFT OUTER JOIN dbo.DevicesParams AS D12 ON D1.DeviceId = D12.DeviceID AND D12.Name = 'Number'
	LEFT OUTER JOIN dbo.DevicesParams AS D2 ON D1.DeviceId = D2.DeviceID AND D2.Name = 'ObjectName'
	LEFT OUTER JOIN dbo.DevicesParams AS D3 ON D1.DeviceId = D3.DeviceID AND D3.Name = 'Location' 
	WHERE 
	(
		(d1.resName like 'ТЭ%' and mps.Name in ('V1', 'V2', 'V3', 'V4'))
		or ((d1.resName like 'ГВС%' or d1.resName like 'ХВС%') and mps.Name in ('V', 'R'))
	)
) z
WHERE 
	z.isHvs = 1
	or z.isGvs = 1


insert #address (address, location, deviceCount)
select distinct 
	address, 
	location, 
	case when mpsName in ('V3', 'V4') then 2 else 1 end
from #measureParams



SET @t2 = GETDATE();
insert @timers (stepName, diff_ml)
SELECT 'insert #measureParams', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;
SET @t1 = GETDATE();

--{GENERAL}

select *
into #result
from
(
	select 
		a.location
		, a.address
		, deviceHvs.DeviceId as [ID ПУ ХВС]
		, deviceHvs. DeviceNumber as [№ ПУ ХВС]

		--, a.deviceCount

		, mStartHvs.ValOut as [Показание ХВС на начало]
		, mEndHvs.ValOut as [Показание ХВС на конец]

		, deviceGvs.DeviceId as [ID ПУ ГВС]
		, deviceGvs. DeviceNumber as [№ ПУ ГВС]
		, mStartGvs.ValOut as [Показание ГВС на начало]
		, mEndGvs.ValOut as [Показание ГВС на конец]

		, cast(mEndHvs.ValOut - mStartHvs.ValOut as decimal(18, 2)) as [Потребление ХВС]
		, cast(mEndGvs.ValOut - mStartGvs.ValOut as decimal(18, 2)) as [Потребление ГВС]

		--into #result
	from #address a 
	left join #measureParams deviceHvs on 
		a.address = deviceHvs.address and 
		a.location = deviceHvs.location and 
		((a.deviceCount = 1 and (deviceHvs.mpsName = 'V1' OR deviceHvs.mpsName = 'R' OR deviceHvs.mpsName = 'V')) or (a.deviceCount = 2 and deviceHvs.mpsName = 'V3')) and 
		deviceHvs.isHvs = 1
	left join #measureParams deviceGvs on 
		a.address = deviceGvs.address and 
		a.location = deviceGvs.location and 
		((a.deviceCount = 1 and (deviceGvs.mpsName = 'V2' OR deviceGvs.mpsName = 'R' OR deviceHvs.mpsName = 'V')) or (a.deviceCount = 2 and deviceGvs.mpsName = 'V4')) and 
		deviceGvs.isGvs = 1

	LEFT OUTER JOIN #measuresBegin mStartHvs on deviceHvs.DeviceId = mStartHvs.DeviceId and deviceHvs.ParamID = mStartHvs.ParamID 
	LEFT OUTER JOIN #measuresEnd mEndHvs on deviceHvs.DeviceId = mEndHvs.DeviceId and deviceHvs.ParamID = mEndHvs.ParamID

	LEFT OUTER JOIN #measuresBegin mStartGvs on deviceGvs.DeviceId = mStartGvs.DeviceId and deviceGvs.ParamID = mStartGvs.ParamID 
	LEFT OUTER JOIN #measuresEnd mEndGvs on deviceGvs.DeviceId = mEndGvs.DeviceId and deviceGvs.ParamID = mEndGvs.ParamID

	WHERE cast(mEndGvs.ValOut - mStartGvs.ValOut as decimal(18, 2)) > 0 AND
		isnull(cast(mEndHvs.ValOut - mStartHvs.ValOut as decimal(18, 2)), 0) = 0
)z
where 
	(isnull([Потребление ХВС], 0) BETWEEN @min and @max) or
	(isnull([Потребление ГВС], 0) BETWEEN @min and @max)
	--(isnull([Потребление ЭЭ], 0) BETWEEN @min and @max)


SET @t2 = GETDATE();
insert @timers (stepName, diff_ml)
SELECT 'end select', DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms;


select * from #result r order by r.address, r.location




--select * from @timers



drop table #measures
drop table #measureIds
drop table #measureParams
drop table #measuresBegin
drop table #measuresEnd
drop table #result

--set statistics time off
--set statistics io off
