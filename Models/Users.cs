using System;
using System.Collections.Generic;

namespace to_do_api.Models;

public partial class Users
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public virtual ICollection<Activities> Activities { get; set; } = new List<Activities>();
}
