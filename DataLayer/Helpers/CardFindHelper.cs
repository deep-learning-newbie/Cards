using Models;
using System.Collections.Generic;
using System.Linq;

namespace Queries.Helpers
{
    public class CardFindHelper
    {
        public static Card Find(List<Card> list, int? id)
        {
            if (!id.HasValue || !list.Any())
                return null;

            var node = list.SingleOrDefault(x => x.Id == id);
            if (node == null)
            {
                foreach (var item in list)
                {
                    var found = Find(item.Childs, id);
                    if (found != null)
                        return found;
                }
            }
            else
                return node;

            return null;
        }
    }
}
