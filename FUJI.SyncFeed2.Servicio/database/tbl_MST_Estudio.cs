//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FUJI.SyncFeed2.Servicio.database
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_MST_Estudio
    {
        public tbl_MST_Estudio()
        {
            this.tbl_DET_Estudio = new HashSet<tbl_DET_Estudio>();
        }
    
        public int intEstudioID { get; set; }
        public Nullable<int> id_Sitio { get; set; }
        public Nullable<int> intModalidadID { get; set; }
        public string vchAccessionNumber { get; set; }
        public string vchPatientBirthDate { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public Nullable<System.DateTime> datFecha { get; set; }
        public Nullable<int> intNumeroArchivo { get; set; }
        public Nullable<int> intTamanoTotal { get; set; }
        public string vchgenero { get; set; }
        public string vchStudyInstanceUID { get; set; }
        public string vchEdad { get; set; }
    
        public virtual ICollection<tbl_DET_Estudio> tbl_DET_Estudio { get; set; }
        public virtual tbl_ConfigSitio tbl_ConfigSitio { get; set; }
    }
}
