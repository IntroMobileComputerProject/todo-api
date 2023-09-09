using System;
using System.Collections.Generic;

namespace to_do_api.Models;

public partial class Activities
{
    public int ActivityId { get; set; }

    public string ActivityName { get; set; } = null!;

    public DateTime ActivitiesTime { get; set; }

    public int? UserId { get; set; }

    public virtual Users? User { get; set; }
}
