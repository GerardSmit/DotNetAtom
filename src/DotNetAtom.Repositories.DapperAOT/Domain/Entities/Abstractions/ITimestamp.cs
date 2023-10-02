using System;

namespace DotNetAtom.Entities;

public interface ITimestamp
{
    int? CreatedByUserId { get; set; }

    DateTime? CreatedOnDate { get; set; }

    int? LastModifiedByUserId { get; set; }

    DateTime? LastModifiedOnDate { get; set; }
}
