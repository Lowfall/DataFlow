CREATE PROCEDURE calculate
AS
begin
DECLARE @sum bigint, @median float;

SELECT @sum = sum(cast(GeneratedInteger as bigint)) from FilesData;

WITH nums AS
(
	SELECT GeneratedFloat, row_number() over (order by GeneratedFloat) as rowNumber, count(*) OVER () as numberAmount from FilesData 
)
SELECT @median = avg(cast(GeneratedFloat as float)) from nums where rowNumber in (numberAmount/2, numberAmount/2+1);

SELECT @sum as IntegerSum, @median as FloatMedian;
END;

DROP PROCEDURE calculate;
EXECUTE calculate;
