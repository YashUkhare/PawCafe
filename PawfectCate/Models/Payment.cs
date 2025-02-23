using System;
using System.Collections.Generic;

namespace PawfectCate.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int BillId { get; set; }

    public int Cvvno { get; set; }

    public long Cardno { get; set; }

    public string Status { get; set; } = null!;

    public virtual Bill Bill { get; set; } = null!;
}
