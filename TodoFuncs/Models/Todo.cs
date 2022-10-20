#nullable enable
namespace TodoFuncs.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Todo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? TaskDescription { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
