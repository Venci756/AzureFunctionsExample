namespace TodoFuncs.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TodoUpdateModel
    {
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
}
