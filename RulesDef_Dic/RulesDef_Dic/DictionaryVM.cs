using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace RulesDef_Dic
{
    class DictionaryVM
    {
        public ObservableCollection<Dictionary> PropAtm { get; set; }
        public Dictionary PropSelect { get; set; }

        public DictionaryVM()
        {
            ContentLoad();
        }

        public void ContentLoad()
        {
            using(var db = new RulesModelContainer())
            {
                var query = from myvar in db.DictionarySet
                            orderby myvar.Id
                            select myvar;

                //PropAtm = new ObservableCollection<Dictionary>(query.ToList());
            }
        }
    }
}
