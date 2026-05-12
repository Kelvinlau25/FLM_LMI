# Complete Date Handling Issues Analysis

## Summary of Issues Found

| Page/Function | Issue Type | Status | Priority |
|---------------|-----------|--------|----------|
| FILM_LMI_PRODUCTIVITY_TARGET | ✅ FIXED | Date shifting with toISOString() | HIGH |
| DAILY_BUDGET_TIME | ❌ NEEDS FIX | Date shifting with toISOString() | HIGH |
| FILM_PRODUCTION_HOLIDAYS | ⚠️ PARTIAL | Uses Date object in remove() | MEDIUM |
| FILM_MACHINE_STOPPAGE | ✅ OK | Uses date input value directly | LOW |

## Detailed Analysis

### 1. ✅ FILM_LMI_PRODUCTIVITY_TARGET - FIXED
**Status:** Already fixed in previous update
- JavaScript now sends date as string: `"2021-01-01"`
- Model has `[JsonConverter(typeof(DateOnlyJsonConverter))]`
- No timezone conversion issues

### 2. ❌ DAILY_BUDGET_TIME - NEEDS FIX
**Location:** `Views/Mstmain/DAILY_BUDGET_TIME.cshtml` (Lines 195-196)

**Problem Code:**
```javascript
var date = new Date(year, month - 1, day);
if (SDATE.indexOf(date.toISOString()) === -1) {
    SDATE.push(date.toISOString());
}
```

**Impact:** Same date shifting issue as FILM_LMI_PRODUCTIVITY_TARGET
- Dates will shift by one day when saving
- Example: 2021-01-01 becomes 2021-01-02

**Model:** `LMI_DAILY_BUDGET_TIME.SDATE` (DateTime property)

**Fix Required:**
1. Change JavaScript to send date string
2. Add `[JsonConverter(typeof(DateOnlyJsonConverter))]` to model

### 3. ⚠️ FILM_PRODUCTION_HOLIDAYS - PARTIAL ISSUE
**Location:** `Views/Mstmain/FILM_PRODUCTION_HOLIDAYS.cshtml`

**Problem Code (in remove function):**
```javascript
function remove(prod, rectype) {
    var now = new Date(prod);  // ← Creates Date object
    var json = {
        "DATETIME": now,       // ← Sends Date object (will be serialized to ISO)
        "ACTION": rectype
    }
}
```

**Impact:** When deleting a holiday, the date might shift

**Add Function:** Uses HTML date input directly - this is OK:
```javascript
var dateTime = document.getElementById("DATETIME").value; // Gets "2021-01-01" string
```

**Models:**
- `MM_FILM_PRODUCTION_HOLIDAYS.DATETIME` (DateTime)
- `FILM_PRODUCTION_HOLIDAYS.HOLIDAY_DATE` (DateTime)

**Fix Required:**
1. Fix the remove() function to send date string
2. Add JSON converter to both DateTime properties

### 4. ✅ FILM_MACHINE_STOPPAGE - OK
**Location:** `Views/Mstmain/FILM_MACHINE_STOPPAGE.cshtml`

**Good Code:**
```javascript
var date = document.getElementById('PERIOD').value; // Gets "2021-01-01" string
var PERIOD = date;  // Sends as string
```

**Why it works:**
- Uses HTML date input value directly (already a string)
- No Date object creation
- No toISOString() conversion

**Model:** `MM_OPR_RATIO_STOPPAGE.PERIOD` (DateTime)
- Has `[DisplayFormat]` attribute but might still benefit from JSON converter

### 5. Other DateTime Properties to Check

From `Models/MstMainModel.cs`:
- `LMI_OPR_RATIO_STOPPAGE.SDATE` - Used in FILM_MACHINE_STOPPAGE (OK - no JS manipulation)
- All other date properties appear to be display-only or server-side

## Fix Priority

### HIGH Priority (Data Corruption Risk)
1. **DAILY_BUDGET_TIME** - Active date shifting bug

### MEDIUM Priority (Potential Issues)
2. **FILM_PRODUCTION_HOLIDAYS** - remove() function has issue
3. **Add JSON converters** to all DateTime properties for consistency

### LOW Priority (Preventive)
4. **FILM_MACHINE_STOPPAGE** - Add JSON converter for robustness
5. **Code review** of any other date handling

## Recommended Actions

1. ✅ Fix DAILY_BUDGET_TIME JavaScript immediately
2. ✅ Fix FILM_PRODUCTION_HOLIDAYS remove() function
3. ✅ Add JSON converters to all DateTime properties in models
4. ✅ Test all date operations with different timezones
5. ✅ Document date handling standards for future development
