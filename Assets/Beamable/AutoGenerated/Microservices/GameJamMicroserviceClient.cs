//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Beamable.Server.Clients
{
    using System;
    using Beamable.Platform.SDK;
    using Beamable.Server;
    
    
    /// <summary> A generated client for <see cref="Beamable.Server.GameJamMicroservice"/> </summary
    public sealed class GameJamMicroserviceClient : Beamable.Server.MicroserviceClient
    {
        
        /// <summary>
        /// Call the JoinGame method on the GameJamMicroservice microservice
        /// <see cref="Beamable.Server.GameJamMicroservice.JoinGame"/>
        /// </summary>
        public Beamable.Common.Promise<Beamable.Common.Unit> JoinGame(System.Guid ownerId, string name)
        {
            string serialized_ownerId = this.SerializeArgument<System.Guid>(ownerId);
            string serialized_name = this.SerializeArgument<string>(name);
            string[] serializedFields = new string[] {
                    serialized_ownerId,
                    serialized_name};
            return this.Request<Beamable.Common.Unit>("GameJamMicroservice", "JoinGame", serializedFields);
        }
        
        /// <summary>
        /// Call the RefreshGameState method on the GameJamMicroservice microservice
        /// <see cref="Beamable.Server.GameJamMicroservice.RefreshGameState"/>
        /// </summary>
        public Beamable.Common.Promise<string> RefreshGameState(string clientPlayersJson)
        {
            string serialized_clientPlayersJson = this.SerializeArgument<string>(clientPlayersJson);
            string[] serializedFields = new string[] {
                    serialized_clientPlayersJson};
            return this.Request<string>("GameJamMicroservice", "RefreshGameState", serializedFields);
        }
    }
    
    internal sealed class MicroserviceParametersGameJamMicroserviceClient
    {
        
        [System.SerializableAttribute()]
        internal sealed class ParameterSystem_Guid : Beamable.Server.MicroserviceClientDataWrapper<System.Guid>
        {
        }
        
        [System.SerializableAttribute()]
        internal sealed class ParameterSystem_String : Beamable.Server.MicroserviceClientDataWrapper<string>
        {
        }
    }
}
