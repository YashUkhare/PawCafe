using System;
using System.Collections.Generic;

namespace PawfectCate.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public string AppointmentTime { get; set; } = null!;

    public int CustomerId { get; set; }

    public int PetId { get; set; }

    public int ServiceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual Customer Customer { get; set; } = null!;

    public virtual Pet Pet { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
