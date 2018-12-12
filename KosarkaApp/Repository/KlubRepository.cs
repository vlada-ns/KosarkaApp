using KosarkaApp.Interfaces;
using KosarkaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KosarkaApp.Repository
{
    public class KlubRepository : IDisposable, IKlubRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Klub> GetAll()
        {
            return db.Klubovi;
        }

        public Klub GetById(int id)
        {
            return db.Klubovi.FirstOrDefault(x => x.Id == id);
        }

        //prikaz kluba sa najvećim brojem trofeja i kluba sa najmanjim brojem trofeja, sortirano po broju trofeja rastuće
        public IEnumerable<Klub> GetEkstremi()
        {
            List<Klub> minMax = new List<Klub>();
            var minRes = db.Klubovi.OrderBy(x => x.Trofeji).First();
            var maxRes = db.Klubovi.OrderByDescending(x => x.Trofeji).First();
            minMax.Add(minRes);
            minMax.Add(maxRes);
            IEnumerable<Klub> result = minMax.AsEnumerable<Klub>();
            result.OrderBy(x => x.Trofeji);
            return result;
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}