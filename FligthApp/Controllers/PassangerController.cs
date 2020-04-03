using FligthApp.Context;
using FligthApp.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FligthApp.Controllers
{
    public class PassangerController : ApiController
    {
        private static List<Passanger> Passangers = new List<Passanger>();
        BaseContext baseContext;

        public PassangerController()
        {
            baseContext = new BaseContext();
            baseContext.Initial();
            baseContext.Create();
        }

        public HashSet<Passanger> Get()
        {
            return baseContext.AllPassangers();
        }

        public HttpResponseMessage Post([FromBody] Passanger passanger)
        {
            if (passanger != null)
            {
                Passangers.Add(passanger);
                baseContext.InsertPassanger(passanger);
                return Request.CreateResponse(HttpStatusCode.OK, "sucess");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "failed");
            }
        }

        [HttpPost]
        [Route("api/Passanger/PassangerToAirplane")]
        public HttpResponseMessage InsertPassangerToAirplane(int idPassanger, int idAirplane)
        {
            if (idPassanger > 0 && idAirplane > 0)
            {
                baseContext.InsertPassangerToAirplane(idPassanger, idAirplane);
                return Request.CreateResponse(HttpStatusCode.OK, "sucess");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "failed");
            }
        }

        [HttpGet]
        [Route("api/Passanger/AllPassangerByAirplane")]
        public List<Passanger> ListAllPassangerByAirplane(int idAirplane)
        {
            if(idAirplane > 0)
            {
                return baseContext.ListAllPassangerByAirplane(idAirplane);
            }
            return null;
        }

        public void Delete(string Nome)
        {
            Passangers.Remove(Passangers.Find(x => x.Nome.Equals(Nome)));
        }
    }
}

