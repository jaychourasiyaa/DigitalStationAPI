using System;

namespace DigitalStation.Contract.Models;

public class Base
{
    public DateTime CreatedOn { get; set; } = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "India Standard Time");
    public int? CreatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;
}
