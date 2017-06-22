//using FUJI.SyncFeed2.Servicio.Entidades;
using FUJI.SyncFeed2.Servicio.Extensions;
using FUJI.SyncFeed2.Servicio.Feed2Service;
using System;
using System.Configuration;
using System.IO;

namespace FUJI.SyncFeed2.Servicio.database
{
    public class NapoleonServerDataAccess
    {
        
        
        public static NapoleonServiceClient NapoleonDA = new NapoleonServiceClient();

        public ClienteF2CResponse setEstudioServer(clsEstudio _estudio)
        {
            ClienteF2CResponse response = new ClienteF2CResponse();
            try
            {
                ClienteF2CRequest request = new ClienteF2CRequest();
                request.estudio = _estudio;
                //request.id_SitioSpecified = true;
                request.id_Sitio = _estudio.id_Sitio;
                request.vchClaveSitio = _estudio.vchClaveSitio;
                request.Token = Security.Encrypt(_estudio.id_Sitio + "|" + _estudio.vchClaveSitio);
                response = NapoleonDA.setEstudioServer(request);
            }
            catch(Exception esES)
            {
                response.valido = false;
                response.message = esES.Message;
                Log.EscribeLog("Existe un error en setEstudioServer: " + esES.Message);
            }
            return response;
        }

        public void setService(int id_Sitio, string vchClaveSitio)
        {
            try
            {
                ClienteF2CRequest request = new ClienteF2CRequest();
                request.id_Sitio = id_Sitio;
                //request.id_SitioSpecified = true;
                request.vchClaveSitio = vchClaveSitio;
                request.tipoServicio = 2;
                request.Token = Security.Encrypt(id_Sitio + "|" + vchClaveSitio);
                NapoleonDA.setService(request);
            }
            catch (Exception eSS)
            {
                Log.EscribeLog("Existe un error en setService: " + eSS.Message);
                //throw eSS;
            }
        }
    }
}
