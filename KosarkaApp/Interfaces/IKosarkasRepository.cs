using KosarkaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KosarkaApp.Interfaces
{
    public interface IKosarkasRepository
    {
        IEnumerable<Kosarkas> GetAll();
        Kosarkas GetById(int id);
        IEnumerable<Kosarkas> GetByGodina(int godina);
        IEnumerable<Kosarkas> GetPretraga(int najmanje, int najvise);
        bool Add(Kosarkas kosarkas);
        bool Update(Kosarkas kosarkas);
        bool Delete(Kosarkas kosarkas);
    }
}
