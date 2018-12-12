using KosarkaApp.Interfaces;
using KosarkaApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace KosarkaApp.Repository
{
    public class KosarkasRepository : IDisposable, IKosarkasRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool Add(Kosarkas kosarkas)
        {
            db.Kosarkasi.Add(kosarkas);
            int res = db.SaveChanges();
            return res > 0 ? true : false;
        }

        public bool Delete(Kosarkas kosarkas)
        {
            db.Kosarkasi.Remove(kosarkas);
            int res = db.SaveChanges();
            return res > 0 ? true : false;
        }

        // preuzimanje svih košarkaša, sortiranih po prosečnom broju poena opadajuće
        public IEnumerable<Kosarkas> GetAll()
        {
            return db.Kosarkasi.Include(x => x.klub).OrderByDescending(x => x.ProsecanBrojPoena);
        }

        public Kosarkas GetById(int id)
        {
            return db.Kosarkasi.Include(x=>x.klub).FirstOrDefault(x => x.Id == id);
        }

        //preuzimanje svih košarkaša koji su rođeni nakon prosleđene godine, sortirano po godini rođenja rastuće
        public IEnumerable<Kosarkas> GetByGodina(int godina)
        {
            return db.Kosarkasi
                .Include(x => x.klub)
                .Where(x => x.Godina > godina)
                .OrderBy(x => x.Godina);
        }

        // preuzimanje košarkaša sa brojem utakmica za klub između dve unete vrednosti (najmanje i najvise),
        // sortiranih po prosečnom broju poena opadajuće
        public IEnumerable<Kosarkas> GetPretraga(int najmanje, int najvise)
        {
            return db.Kosarkasi
                .Include(x => x.klub)
                .Where(x => x.BrojUtakmica > najmanje && x.BrojUtakmica < najvise)
                .OrderByDescending(x => x.ProsecanBrojPoena);
        }

        public bool Update(Kosarkas kosarkas)
        {
            db.Entry(kosarkas).State = EntityState.Modified;
            try
            {
                int res = db.SaveChanges();
                return res > 0 ? true : false;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
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