using System.Threading.Tasks;
using Models;

namespace Services.Interfaces;

public interface IPingService
{
    Task StartPingersTests();
}