//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _420_476.Projet.Session.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rating
    {
        public int ID { get; set; }
        public int MembreID { get; set; }
        public int OffrantID { get; set; }
        public decimal Note { get; set; }
        public string Text { get; set; }
    
        public virtual Membre Membre { get; set; }
        public virtual Offrant Offrant { get; set; }
    }
}
