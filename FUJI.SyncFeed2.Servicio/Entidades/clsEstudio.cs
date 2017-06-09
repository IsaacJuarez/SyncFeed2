using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUJI.SyncFeed2.Servicio.Entidades
{
    public class clsEstudio
    {
        public int intEstudioID { get; set; }
        public int id_Sitio { get; set; }
        public int intModalidadID { get; set; }
        public string vchAccessionNumber { get; set; }
        public string vchPatientBirthDate { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public DateTime datFecha { get; set; }
        public int intDetEstudioID { get; set; }
        public int intEstatusID { get; set; }
        public string vchNameFile { get; set; }
        public int intSizeFile { get; set; }
        public string vchPathFile { get; set; }
        public string vchStudyInstanceUID { get; set; }
        public bool bitSync { get; set; }
        public string vchgenero { get; set; }
        public string vchEdad { get; set; }

        public clsEstudio()
        {
            intEstudioID = int.MinValue;
            id_Sitio = int.MinValue;
            intModalidadID = int.MinValue;
            vchAccessionNumber = string.Empty;
            vchPatientBirthDate = string.Empty;
            PatientID = string.Empty;
            PatientName = string.Empty;
            datFecha = DateTime.MinValue;
            intDetEstudioID = int.MinValue;
            intEstatusID = int.MinValue;
            vchNameFile = string.Empty;
            intSizeFile = int.MinValue;
            vchPathFile = string.Empty;
            vchStudyInstanceUID = string.Empty;
            bitSync = false;
            vchgenero = string.Empty;
            vchEdad = string.Empty;
        }

    }
}
