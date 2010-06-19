using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    public class ISCShared
    {
        public enum ServerType
        {
            Login, Cluster, World, PHPConnector
        }
        public enum Commands
        {
            // Authentication stuff
            Authentication,         // Authentication data transfer.
            AuthenticationResult,   // The result of authentication. warning: it can be deadly. o_O
            // Cross-channel stuff
            KickFromServers,    // Kick a player from the servers.
            KickFromGuild,      // Kick a player from their guild.
            SendNotice,         // Sends an announcement to some servers
            
            // Login server related
            UpdateServerList,   // Sends the complete server list to the login server

            // Cluster server related
            UpdateWorldTable,       // Sends the world ID and the IP of the world server that this cluster will route

            // Shared (login & cluster)
            UpdateLocalization  // Sends the complete list of localized IP's.
        }
        /*
         * Packet structures
         *      KickFromServers
         *          (int) dwAccountID
         *          Received from any server.
         *          Sends the same packet to all connected servers excluding the PHPConnector.
         *          
         *      KickFromGuild
         *          (int) dwCharacterID
         *          Received from a world server.
         *          Sends the same packet to all world servers connected to the same cluster server.
         *          
         *      SendNotice
         *          (int) dwNoticeType
         *          (string) strNotice
         *          Received from a world server.
         *          dwNoticeType as 0: sends the notice to all servers.
         *          dwNoticeType as 1: sends the notice to the world server's cluster.
         *          Sends the same packet to... depends on dwNoticeType.
         *      
         *      UpdateServerList (a bit more complicated)
         *          (int) dwClustersCount
         *          loop until dwClustersCount:
         *              (int) dwClusterID
         *              (string) strClusterName
         *              (string) strClusterIP
         *              (int) dwWorldsCount
         *              loop until dwWorldsCount:
         *                  (int) dwWorldID
         *                  (string) strWorldName
         *              end
         *          end
         *          Does not receive this packet.
         *          Sends this packet to the login server after any server (servertype != login,phpconnector) connects to the ISC.
         *          
         *      UpdateWorldTable (also a bit more complicated)
         *          (int) dwWorldsCount
         *          loop until dwWorldsCount:
         *              (int) dwWorldID
         *              (string) strWorldIP
         *          end
         *          Does not receive this packet.
         *          Sends this pacekt to the cluster server after a world server connects to the ISC having the same cluster ID.
         *          
         *      UpdateLocalization (would be complicated implementing, nothing else..)
         *          (int) dwIPCount
         *          loop until dwIPCount:
         *              (string) strLocalizedIP
         *          end
         *          Does not receive this packet.
         *          Sends this packet to every server after authentication.
         *          
         *      Authentication (isc->others)
         *          [no body]
         *          Sends this packet to every server upon connection.
         *          Will accept packets with the same header (see Authentication others->isc). for more info, scroll down.
         *          
         *      Authentication (others->isc)
         *          (string) strPasswordHash
         *          • check if the password is correct. if it isn't, call AuthenticationResult(failed) procedure
         *          (string) strServerName
         *          (int, enum ServerType) nServerType
         *          nServerType == Login:
         *              • check for more login servers. if another one exists, call AuthenticationResult(failed) procedure
         *          nServerType == Cluster:
         *              (int) dwClusterID
         *              (string) strServerIP
         *              • check for a login server. if none, call AuthenticationResult(failed) procedure
         *              • check for a cluster server with the same ID. if exists, call AuthenticationResult(failed) procedure
         *          nServerType == World:
         *              (int) dwClusterID
         *              (int) dwWorldID
         *              (string) strServerIP
         *              • check for a login server. if none, call AuthenticationResult(failed) procedure
         *              • check for a cluster server with ID dwClusterID. if none, call AuthenticationResult(failed) procedure
         *              • check for a world server with ID dwClusterID,dwWorldID. if exists, call AuthenticationResult(failed) procedure
         *          • call AuthenticationResult(succeeded) procedure.
         *         
         *      AuthenticationResult
         *          (int) dwResult
         *          Does not receive this packet.
         *          Sends this packet after authentication - lets the server know if authentication is successful or not but does not provide details like where it failed, if it did.
         */
    }
}
