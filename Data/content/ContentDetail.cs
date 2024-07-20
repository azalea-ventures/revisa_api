using System;
using System.Collections.Generic;

namespace revisa_api.Data.content;

public partial class ContentDetail
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int GradeId { get; set; }

    public int SubjectId { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public int OwnerId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? FileId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<ContentVersion> ContentVersions { get; set; } = new List<ContentVersion>();

    public virtual ClientFile? File { get; set; }

    public virtual Grade Grade { get; set; } = null!;

    public virtual User Owner { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
