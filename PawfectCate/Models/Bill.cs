using System;
using System.Collections.Generic;

namespace PawfectCate.Models;

public partial class Bill
{
    public int BillId { get; set; }

    public int AppointmentId { get; set; }

    public string Status { get; set; } = null!;

    public decimal? BillAmount { get; set; }

    public string? Cvvno { get; set; }

    public string? Cardno { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
