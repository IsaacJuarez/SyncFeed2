using FUJI.SyncFeed2.Servicio.Entidades;
using FUJI.SyncFeed2.Servicio.Extensions;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace FUJI.SyncFeed2.Servicio.database
{
    public class NapoleonServerDataAccess
    {
        public static int id_Servicio = 0;
        public static string vchClaveSitio = "";
        public static NAPOLEONEntities NapoleonDA;
        public static string AETitle = "";
        public static string vchPathRep = "";
        public static string path = "";
        public static clsConfiguracion _conf;

        public bool setEstudioServer(clsEstudio _estudio)
        {
            bool valido = false;
            try
            {
                using(NapoleonDA = new NAPOLEONEntities())
                {
                    if(NapoleonDA.tbl_MST_Estudio.Any(x => x.id_Sitio == _estudio.id_Sitio && x.vchAccessionNumber == _estudio.vchAccessionNumber))
                    {
                        int intEstudioID = NapoleonDA.tbl_MST_Estudio.First(x => x.id_Sitio == _estudio.id_Sitio && x.vchAccessionNumber == _estudio.vchAccessionNumber).intEstudioID;
                        if (intEstudioID > 0)
                        {
                            tbl_DET_Estudio mdl = new tbl_DET_Estudio();
                            mdl.datFecha = _estudio.datFecha;
                            mdl.intEstatusID = _estudio.intEstatusID;
                            mdl.intEstudioID = intEstudioID;
                            mdl.intSizeFile = _estudio.intSizeFile;
                            mdl.vchNameFile = _estudio.vchNameFile;
                            mdl.vchPathFile = _estudio.vchPathFile;
                            mdl.vchStudyInstanceUID = _estudio.vchStudyInstanceUID;
                            NapoleonDA.tbl_DET_Estudio.Add(mdl);
                            NapoleonDA.SaveChanges();
                            valido = true;
                        }
                    }
                    else
                    {
                        tbl_MST_Estudio MST = new tbl_MST_Estudio();
                        MST.datFecha = _estudio.datFecha;
                        MST.id_Sitio = _estudio.id_Sitio;
                        MST.intModalidadID = _estudio.intModalidadID;
                        MST.PatientID = _estudio.PatientID;
                        MST.PatientName = _estudio.PatientName;
                        MST.vchAccessionNumber = _estudio.vchAccessionNumber;
                        MST.vchPatientBirthDate = _estudio.vchPatientBirthDate;
                        MST.vchgenero = _estudio.vchgenero;
                        MST.vchEdad = _estudio.vchEdad;
                        NapoleonDA.tbl_MST_Estudio.Add(MST);
                        NapoleonDA.SaveChanges();
                        if (MST.intEstudioID > 0)
                        {
                            tbl_DET_Estudio mdl = new tbl_DET_Estudio();
                            mdl.datFecha = _estudio.datFecha;
                            mdl.intEstatusID = _estudio.intEstatusID;
                            mdl.intEstudioID = MST.intEstudioID;
                            mdl.intSizeFile = _estudio.intSizeFile;
                            mdl.vchNameFile = _estudio.vchNameFile;
                            mdl.vchPathFile = _estudio.vchPathFile;
                            mdl.vchStudyInstanceUID = _estudio.vchStudyInstanceUID;
                            NapoleonDA.tbl_DET_Estudio.Add(mdl);
                            NapoleonDA.SaveChanges();
                            valido = true;
                        }
                    }
                }
            }
            catch(Exception esES)
            {
                valido = false;
                Log.EscribeLog("Existe un error en setEstudioServer: " + esES.Message);
            }
            return valido;
        }

        public void setService()
        {
            try
            {
                try
                {
                    path = ConfigurationManager.AppSettings["ConfigDirectory"] != null ? ConfigurationManager.AppSettings["ConfigDirectory"].ToString() : "";
                }
                catch (Exception ePath)
                {
                    path = "";
                    Log.EscribeLog("Error al obtener el path desde appSettings: " + ePath.Message);
                }
                if (File.Exists(path + "info.xml"))
                {
                    _conf = XMLConfigurator.getXMLfile();
                    AETitle = _conf.vchAETitle;
                    vchPathRep = _conf.vchPathLocal;
                    vchClaveSitio = _conf.vchClaveSitio;
                }
                tbl_DET_ServicioSitio mdl = new tbl_DET_ServicioSitio();

                if (id_Servicio > 0)
                {
                    using (NapoleonDA = new NAPOLEONEntities())
                    {
                        if (NapoleonDA.tbl_DET_ServicioSitio.Any(x => x.id_Sitio == id_Servicio))
                        {
                            using (NapoleonDA = new NAPOLEONEntities())
                            {
                                mdl = NapoleonDA.tbl_DET_ServicioSitio.First(x => x.id_Sitio == id_Servicio);
                                mdl.datFechaSync = DateTime.Now;
                                NapoleonDA.SaveChanges();
                            }
                        }
                        else
                        {
                            using (NapoleonDA = new NAPOLEONEntities())
                            {
                                mdl.datFechaSync = DateTime.Now;
                                mdl.id_Sitio = id_Servicio;
                                NapoleonDA.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    if (vchClaveSitio != "")
                    {
                        using (NapoleonDA = new NAPOLEONEntities())
                        {
                            tbl_ConfigSitio mdlSitio = new tbl_ConfigSitio();
                            if (NapoleonDA.tbl_ConfigSitio.Any(x => x.vchClaveSitio == vchClaveSitio))
                            {
                                mdlSitio = NapoleonDA.tbl_ConfigSitio.First(x => x.vchClaveSitio == vchClaveSitio);
                                mdl = NapoleonDA.tbl_DET_ServicioSitio.First(x => x.id_Sitio == mdlSitio.id_Sitio);
                                mdl.datFechaSync = DateTime.Now;
                                NapoleonDA.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception eSS)
            {
                Log.EscribeLog("Existe un error en setService: " + eSS.Message);
                //throw eSS;
            }
        }
    }
}
