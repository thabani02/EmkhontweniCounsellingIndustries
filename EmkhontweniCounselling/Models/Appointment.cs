using System;
using System.Collections.Generic;

namespace EmkhontweniCounselling.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int ClientId { get; set; }

    public string? ServiceType { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }
    public decimal Amount { get; set; }
    // NEW FIELDS
    public string? PaymentReference { get; set; }
    public string? ProofOfPaymentPath { get; set; }
    public bool IsPaymentVerified { get; set; }

    public virtual Client Client { get; set; } = null!;
}
