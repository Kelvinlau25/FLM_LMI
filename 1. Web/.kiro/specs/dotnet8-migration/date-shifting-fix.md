# Date Shifting Issue Fix - FILM_LMI_PRODUCTIVITY_TARGET

## Problem Description

When updating productivity target data (e.g., row 2021-01-01), after clicking save:
- The update was successful in the database
- BUT the row shifted - 2021-01-01 was replaced by 2021-01-02
- This is a **timezone conversion bug** common in .NET Framework to .NET 8 migrations

## Root Cause

### Original Code (JavaScript)
```javascript
var date = new Date(year, month-1, day);  // Creates local time date
SDATE.push(date.toISOString());           // Converts to UTC ISO string
```

**What happened:**
1. JavaScript: `new Date(2021, 0, 1)` → `2021-01-01 00:00:00` (local time)
2. `.toISOString()` → `"2021-01-01T00:00:00.000Z"` or `"2020-12-31T16:00:00.000Z"` (depending on timezone)
3. .NET 8 deserializes this with timezone awareness
4. Date shifts by one day due to timezone offset!

### Why This Worked in .NET Framework

.NET Framework's JSON deserializer was more lenient and often ignored timezone information for date-only scenarios. .NET 8's `System.Text.Json` is stricter and respects timezone information.

## Solution Implemented

### 1. JavaScript Fix (Views/Mstmain/FILM_LMI_PRODUCTIVITY_TARGET.cshtml)

**Changed from:**
```javascript
var date = new Date(year, month-1, day);
if (SDATE.indexOf(date.toISOString()) === -1) {
    SDATE.push(date.toISOString());
}
```

**Changed to:**
```javascript
// Create date string in YYYY-MM-DD format (no timezone conversion)
var dateString = year + '-' + month + '-' + day;
if (SDATE.indexOf(dateString) === -1) {
    SDATE.push(dateString);
}
```

**Benefits:**
- No `Date` object creation = no timezone conversion
- Sends pure date string: `"2021-01-01"`
- Matches the format expected by the database

### 2. Custom JSON Converter (Helpers/DateOnlyJsonConverter.cs)

Created a custom converter to handle date-only strings:

```csharp
public class DateOnlyJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();
        
        // Parse as date-only string (yyyy-MM-dd) without timezone
        if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", 
            CultureInfo.InvariantCulture, 
            DateTimeStyles.None, 
            out DateTime result))
        {
            return result;
        }
        
        // Fallback: extract date part only from ISO dates
        if (DateTime.TryParse(dateString, out DateTime isoResult))
        {
            return isoResult.Date;
        }
        
        throw new JsonException($"Unable to parse date: {dateString}");
    }
}
```

### 3. Model Update (Models/MstMainModel.cs)

Applied the converter to the date property:

```csharp
public class LMI_PRODUCTIVITY_TARGET
{
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateTime SDATE { get; set; }
    public decimal TARGET1 { get; set; }
    public decimal TARGET2 { get; set; }
    public decimal TARGET3 { get; set; }
}
```

## Testing Checklist

- [ ] Filter to period 2021-01
- [ ] Modify Target F3 for row 2021-01-01 to 33852
- [ ] Click Save
- [ ] Verify success message
- [ ] Verify row 2021-01-01 still shows (not shifted to 2021-01-02)
- [ ] Verify database has correct date (2021-01-01)
- [ ] Test with different timezones if possible
- [ ] Test with dates at month boundaries (e.g., 2021-01-31)

## Similar Issues to Check

This same pattern should be applied to other date-handling pages:

1. **FILM_PRODUCTION_HOLIDAYS** - Uses DateTime for holiday dates
2. **FILM_MACHINE_STOPPAGE** - Uses DateTime for period
3. **STOCK_CONTROL_LIST_BUDGET** - Uses string for SDATE (should be OK)
4. **DAILY_BUDGET_TIME** - Uses DateTime for SDATE
5. **DAILY_PLAN_TYPE** - Check if dates are involved

## Prevention for Future Development

**Rule:** When sending dates from JavaScript to .NET 8:
- ❌ DON'T use `new Date().toISOString()` for date-only values
- ✅ DO send date strings in `"YYYY-MM-DD"` format
- ✅ DO use custom JSON converters for date-only properties
- ✅ DO test with different timezones (UTC, UTC+8, UTC-5, etc.)

## References

- [System.Text.Json Date Handling](https://learn.microsoft.com/en-us/dotnet/standard/datetime/system-text-json-support)
- [Custom JSON Converters](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to)
