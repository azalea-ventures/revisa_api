using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace revisa_api.Data;

public partial class ContentDetail
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int GradeId { get; set; }

    public int SubjectId { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public string OriginalFilename { get; set; } = null!;

    public int OwnerId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<ContentVersion> ContentVersions { get; set; } = new List<ContentVersion>();

    public virtual Grade Grade { get; set; } = null!;

    public virtual User Owner { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
