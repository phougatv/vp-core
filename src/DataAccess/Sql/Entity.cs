namespace VP.Core.DataAccess.Sql
{
    using System;

    /// <summary>
    /// Entity class.
    /// 
    /// Use this class as a base class.
    /// </summary>
    public class Entity
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
