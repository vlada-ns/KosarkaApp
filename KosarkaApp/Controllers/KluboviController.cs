using KosarkaApp.Interfaces;
using KosarkaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KosarkaApp.Controllers
{
    public class KluboviController : ApiController
    {
        IKlubRepository _repository { get; set; }
        public KluboviController(IKlubRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Klub> Get()
        {
            return _repository.GetAll();
        }

        public IHttpActionResult Get(int id)
        {
            var klub = _repository.GetById(id);
            if (klub == null)
            {
                return NotFound();
            }
            return Ok(klub);
        }

        [Route("api/ekstremi")]
        public IEnumerable<Klub> GetEkstremi()
        {
            return _repository.GetEkstremi();
        }
    }
}
