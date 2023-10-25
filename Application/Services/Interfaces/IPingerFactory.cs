using System.Collections.ObjectModel;
using Models;

namespace Services.Interfaces;

public interface IPingerFactory
{
    IEnumerable<IPinger> GetPingers();
}