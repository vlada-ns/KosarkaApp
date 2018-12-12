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
    public class KosarkasiController : ApiController
    {
        IKosarkasRepository _repository { get; set; }
        public KosarkasiController(IKosarkasRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Kosarkas> Get()
        {
            return _repository.GetAll();
        }

        public IHttpActionResult Get(int id)
        {
            var kosarkas = _repository.GetById(id);
            if (kosarkas == null)
            {
                return NotFound();
            }
            return Ok(kosarkas);
        }

        //public IEnumerable<Kosarkas> GetGodina(int godine)
        public IHttpActionResult GetGodina(int godine)
        {
            var kosarkasi = _repository.GetByGodina(godine);
            if (kosarkasi == null || !kosarkasi.Any())
            {
                return NotFound();
            }
            return Ok(kosarkasi);
        }

        public IHttpActionResult Post(Kosarkas kosarkas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Add(kosarkas);
            return CreatedAtRoute("DefaultApi", new { id = kosarkas.Id }, kosarkas);
        }

        [Authorize]
        [Route("api/pretraga")]
        public IHttpActionResult PostPretraga(Pretraga pretraga)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (pretraga.Najmanje > pretraga.Najvise)
            {
                return BadRequest("Najmanje ne sme biti veci od Najvise pretrage");
            }
            var festivali = _repository.GetPretraga(pretraga.Najmanje, pretraga.Najvise);
            if (festivali == null || !festivali.Any())
            {
                return NotFound();
            }
            return Ok(festivali);
        }

        [Authorize]
        public IHttpActionResult Put(int id, Kosarkas kosarkas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kosarkas.Id)
            {
                return BadRequest();
            }

            try
            {
                _repository.Update(kosarkas);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(kosarkas);
        }

        [Authorize]
        public IHttpActionResult Delete(int id)
        {
            var kosarkas = _repository.GetById(id);
            if (kosarkas == null)
            {
                return NotFound();
            }

            _repository.Delete(kosarkas);
            return Ok();
        }
    }
}
