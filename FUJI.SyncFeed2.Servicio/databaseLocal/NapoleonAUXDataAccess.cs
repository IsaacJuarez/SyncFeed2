using FUJI.SyncFeed2.Servicio.databaseLocal;
using FUJI.SyncFeed2.Servicio.Entidades;
using FUJI.SyncFeed2.Servicio.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUJI.SyncFeed2.Servicio
{
    public class NapoleonAUXDataAccess
    {
        private NAPOLEONAUXEntities NapAUXDA;
        public List<clsEstudio> getEstudiosPendientes()
        {
            List<clsEstudio> _lstEst = new List<clsEstudio>();
            try
            {
                using (NapAUXDA = new NAPOLEONAUXEntities())
                {
                    if (NapAUXDA.tbl_DET_EstudioAUX.Any(x => !(bool)x.bitSync))
                    {
                        var query = (from item in NapAUXDA.tbl_DET_EstudioAUX
                                     join mst in NapAUXDA.tbl_MST_EstudioAUX on item.intEstudioID equals mst.intEstudioID into MD
                                     from MD1 in MD.DefaultIfEmpty()
                                     where !(bool)item.bitSync
                                     select new
                                     {
                                         intDetEstudioID = item.intDetEstudioID,
                                         intEstudioID = item.intEstatusID,
                                         intEstatusID = item.intEstatusID,
                                         vchNameFile = item.vchNameFile,
                                         intSizeFile = item.intSizeFile,
                                         vchPathFile = item.vchPathFile,
                                         vchStudyInstanceUID = item.vchStudyInstanceUID,
                                         datFecha = item.datFecha,
                                         id_Sitio = MD1.id_Sitio,
                                         intModalidad = MD1.intModalidadID,
                                         vchAccessionNumber = MD1.vchAccessionNumber,
                                         vchPatientBirthDate = MD1.vchPatientBirthDate,
                                         PatientID = MD1.PatientID,
                                         PatientName = MD1.PatientName,
                                         vchgenero = MD1.vchgenero,
                                         vchEdad = MD1.vchEdad
                                     }).ToList();
                        if (query != null)
                        {
                            if(query.Count > 0)
                            {
                                foreach(var item in query)
                                {
                                    clsEstudio mdl = new clsEstudio();
                                    mdl.intDetEstudioID = item.intDetEstudioID;
                                    mdl.intEstudioID = (int)item.intEstatusID;
                                    mdl.intEstatusID = (int)item.intEstatusID;
                                    mdl.vchNameFile = item.vchNameFile;
                                    mdl.intSizeFile = (int)item.intSizeFile;
                                    mdl.vchPathFile = item.vchPathFile;
                                    mdl.vchStudyInstanceUID = item.vchStudyInstanceUID;
                                    mdl.datFecha = (DateTime)item.datFecha;
                                    mdl.id_Sitio = (int)item.id_Sitio;
                                    mdl.intModalidadID = (int)item.intModalidad;
                                    mdl.vchAccessionNumber = item.vchAccessionNumber;
                                    mdl.vchPatientBirthDate = item.vchPatientBirthDate;
                                    mdl.PatientID = item.PatientID;
                                    mdl.PatientName = item.PatientName;
                                    mdl.vchgenero = item.vchgenero;
                                    mdl.vchEdad = item.vchEdad;
                                    _lstEst.Add(mdl);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception egEP)
            {
                _lstEst = null;
                Log.EscribeLog("Existe un error en getEstudiosPendientes: " + egEP.Message);
            }
            return _lstEst;
        }

        public bool updateEstudioSync(int intDetEstudioID)
        {
            bool valido = false;
            try
            {
                using(NapAUXDA = new NAPOLEONAUXEntities())
                {
                    if(NapAUXDA.tbl_DET_EstudioAUX.Any(x => x.intDetEstudioID == intDetEstudioID))
                    {
                        tbl_DET_EstudioAUX detalle = new tbl_DET_EstudioAUX();
                        detalle = NapAUXDA.tbl_DET_EstudioAUX.First(x => x.intDetEstudioID == intDetEstudioID);
                        detalle.bitSync = true;
                        NapAUXDA.SaveChanges();
                        valido = true;
                    }
                }
            }
            catch(Exception euE)
            {
                Log.EscribeLog("Existe un error en updateEstudioSync: " + euE.Message);
            }
            return valido;
        }
    }
}
