using FligthApp.Context;
using FligthApp.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FligthApp.Controllers
{
    public class AirplaneController : ApiController
    {
        private static List<Airplane> Airplanes = new List<Airplane>();
        BaseContext baseContext;

        public AirplaneController()
        {
            baseContext = new BaseContext();
            baseContext.Initial();
            baseContext.Create();
        }


        public List<Airplane> Get()
        {
            return baseContext.AllAirplanes();
        }

        public HttpResponseMessage Post([FromBody] Airplane airplane)
        {
            if (airplane != null)
            {
                Airplanes.Add(airplane);
                baseContext.InsertAirplane(airplane);
                return Request.CreateResponse(HttpStatusCode.OK, "sucess");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "failed");
            }
        }

        public void InsertPassangerToAirplane(int idPassanger, int idAirplane)
        {
            baseContext.InsertPassangerToAirplane(idPassanger, idAirplane);
        }

        public void Delete(string Nome)
        {
            Airplanes.Remove(Airplanes.Find(x => x.Nome.Equals(Nome)));
        }
    }
}



