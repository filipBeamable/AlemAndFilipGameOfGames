using Beamable.Server;

namespace Beamable.Server
{
   [Microservice("GameJamService")]
   public class GameJamService : Microservice
   {
      [ClientCallable]
      public int Multiply(int x, int y)
      {
            return x * y;
      }
   }
}