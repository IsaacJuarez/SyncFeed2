using FUJI.SyncFeed2.Servicio.database;
using FUJI.SyncFeed2.Servicio.Entidades;
using FUJI.SyncFeed2.Servicio.Extensions;
using FUJI.SyncFeed2.Servicio.Feed2Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace FUJI.SyncFeed2.Servicio
{
    partial class SyncFeed2Service : ServiceBase
    {
        private Timer SyncTimer = new Timer();
        private NapoleonAUXDataAccess NapAuxDA = new NapoleonAUXDataAccess();
        private NapoleonServerDataAccess NapSerDA = new NapoleonServerDataAccess();
        public static int id_Servicio = 0;
        public static string vchClaveSitio = "";
        public static string AETitle = "";
        public static string vchPathRep = "";
        public static string path = "";
        public static Feed2Service.clsConfiguracion _conf;

        public SyncFeed2Service()
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
                    id_Servicio = _conf.id_Sitio;
                    AETitle = _conf.vchAETitle;
                    vchPathRep = _conf.vchPathLocal;
                    vchClaveSitio = _conf.vchClaveSitio;
                }
                cargaServicio();
            }
            catch (Exception eSync)
            {
                Log.EscribeLog("Existe un error en SyncFeed2Service: " + eSync.Message);
            }

        }

        private void cargaServicio()
        {
            try
            {
                Console.WriteLine("Se cargó correctamente el servicio SyncFeed2Service. " + "[" + DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString() + "]");
                Log.EscribeLog("Se cargó correctamente el servicio SyncFeed2Service. ");
                int segundosPoleo;
                try
                {
                    segundosPoleo = ConfigurationManager.AppSettings["segundosPoleo"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["segundosPoleo"].ToString()) : 1;
                }
                catch(Exception eGPOLeo)
                {
                    Log.EscribeLog("Existe un error al obtener el tiempo para el poleo del servicio: " + eGPOLeo.Message);
                    segundosPoleo = 60;
                }
                int minutos = (int)(1000 * segundosPoleo);
                SyncTimer.Elapsed += new System.Timers.ElapsedEventHandler(SyncTimer_Elapsed);
                SyncTimer.Interval = minutos;
                SyncTimer.Enabled = true;
                SyncTimer.Start();
                try
                {
                    NapSerDA.setService(id_Servicio,vchClaveSitio);
                }
                catch(Exception e)
                {
                    Log.EscribeLog("Existe un error en setService: " + e.Message);
                }
            }
            catch (Exception ecS)
            {
                Log.EscribeLog("Existe un error en cargaServicio: " + ecS.Message);
            }
        }

        private void SyncTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Log.EscribeLog("[" + DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString() + "] Leyendo estudios para sincronizar.");
                Console.WriteLine("[" + DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString() + "] Leyendo estudios para sincronizar.");
                try
                {
                    NapSerDA.setService(id_Servicio,vchClaveSitio);
                }
                catch (Exception eSyc)
                {
                    Log.EscribeLog("Existe un error en setService: " + eSyc.Message);
                }
                List<Entidades.clsEstudio> lstEstudio = new List<Entidades.clsEstudio>();
                lstEstudio = NapAuxDA.getEstudiosPendientes();
                if(lstEstudio != null)
                {
                    if (lstEstudio.Count > 0)
                    {
                        foreach (Entidades.clsEstudio item in lstEstudio)
                        {
                            Feed2Service.clsEstudio mdl = new Feed2Service.clsEstudio();
                            mdl.vchClaveSitio = vchClaveSitio;
                            mdl.intDetEstudioID = item.intDetEstudioID;
                            mdl.intEstudioID = item.intEstatusID;
                            mdl.intEstatusID = item.intEstatusID;
                            mdl.vchNameFile = item.vchNameFile;
                            mdl.intSizeFile = item.intSizeFile;
                            mdl.vchPathFile = item.vchPathFile;
                            mdl.vchStudyInstanceUID = item.vchStudyInstanceUID;
                            mdl.datFecha = item.datFecha;
                            mdl.id_Sitio = item.id_Sitio;
                            mdl.intModalidad = item.intModalidadID;
                            mdl.vchAccessionNumber = item.vchAccessionNumber;
                            mdl.vchPatientBirthDate = item.vchPatientBirthDate;
                            mdl.PatientID = item.PatientID;
                            mdl.PatientName = item.PatientName;
                            mdl.vchgenero = item.vchgenero;
                            mdl.vchEdad = item.vchEdad;
                            ClienteF2CResponse mdlResponse = new ClienteF2CResponse();
                            mdlResponse = NapSerDA.setEstudioServer(mdl);
                            if (mdlResponse != null)
                            {
                                if (mdlResponse.valido)
                                {
                                    NapAuxDA.updateEstudioSync(item.intDetEstudioID);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception eSYTi)
            {
                Log.EscribeLog("Existe un error en SyncTimer_Elapsed: " + eSYTi.Message);
            }
        }


        protected override void OnStart(string[] args)
        {
            // TODO: agregar código aquí para iniciar el servicio.
        }

        protected override void OnStop()
        {
            // TODO: agregar código aquí para realizar cualquier anulación necesaria para detener el servicio.
        }
    }
}
