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
    
    public partial class Message
    {
        public int ID { get; set; }
        public int FromUserID { get; set; }
        public int ToUserID { get; set; }
        public string Sujet { get; set; }
        public string Texte { get; set; }
        public System.DateTime DateEnvoie { get; set; }
        public bool Statut_lu { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
