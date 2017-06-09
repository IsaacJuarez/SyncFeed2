using FUJI.SyncFeed2.Servicio.database;
using FUJI.SyncFeed2.Servicio.Entidades;
using FUJI.SyncFeed2.Servicio.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;

namespace FUJI.SyncFeed2.Servicio
{
    partial class SyncFeed2Service : ServiceBase
    {
        private Timer SyncTimer = new Timer();
        private NapoleonAUXDataAccess NapAuxDA = new NapoleonAUXDataAccess();
        private NapoleonServerDataAccess NapSerDA = new NapoleonServerDataAccess();
        
        public SyncFeed2Service()
        {
            try
            {
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
                int minutosPoleo = ConfigurationManager.AppSettings["minutosPoleo"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["minutosPoleo"].ToString()) : 1;
                int minutos = 1000 * 60 * minutosPoleo;
                SyncTimer.Elapsed += new System.Timers.ElapsedEventHandler(SyncTimer_Elapsed);
                SyncTimer.Interval = minutos;
                SyncTimer.Enabled = true;
                SyncTimer.Start();
                try
                {
                    NapSerDA.setService();
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
                    NapSerDA.setService();
                }
                catch (Exception eSyc)
                {
                    Log.EscribeLog("Existe un error en setService: " + eSyc.Message);
                }
                List<clsEstudio> lstEstudio = new List<clsEstudio>();
                lstEstudio = NapAuxDA.getEstudiosPendientes();
                if(lstEstudio != null)
                {
                    if(lstEstudio.Count > 0)
                    {
                        foreach(clsEstudio item in lstEstudio)
                        {
                            bool valido = false;
                            valido = NapSerDA.setEstudioServer(item);
                            if (valido)
                            {
                                NapAuxDA.updateEstudioSync(item.intDetEstudioID);
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
