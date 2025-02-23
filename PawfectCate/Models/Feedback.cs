using System;
using System.Collections.Generic;

namespace PawfectCate.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public string Description { get; set; } = null!;

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
