// This is a teleport manager that was designed before Udon Networking supported events with variables
// It allows players to teleport others to specified positions and orientations in the VRChat world (and therefore to other players!).
// Just use TeleportManager.TeleportPlayer(playerId, position, rotation);

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class TeleportManager : UdonSharpBehaviour
{
    [UdonSynced] private int playerTeleport = 0;
    private int _playerTeleport = 0;
    [UdonSynced] private Vector3 playerTeleportToPos = Vector3.zero;
    private Vector3 _playerTeleportToPos = Vector3.zero;
    [UdonSynced] private Quaternion playerTeleportToRot = Quaternion.identity;
    private Quaternion _playerTeleportToRot = Quaternion.identity;

    /// <summary>
    /// Teleports a player to a specified position and rotation.
    /// </summary>
    /// <param name="playerId">The ID of player to teleport</param>
    /// <param name="pos">The position to teleport the player to</param>
    /// <param name="rot">The rotation to teleport the player to</param>
    public void TeleportPlayer(int playerId, Vector3 pos, Quaternion rot)
    {
        _playerTeleport = playerId;
        _playerTeleportToPos = pos;
        _playerTeleportToRot = rot;
        if (Networking.IsOwner(gameObject)) ApplyLocalTeleport();
        else Networking.SetOwner(Networking.LocalPlayer, gameObject);
    }

    private void ApplyLocalTeleport()
    {
        playerTeleport = _playerTeleport;
        playerTeleportToPos = _playerTeleportToPos;
        playerTeleportToRot = _playerTeleportToRot;
        RequestSerialization();
        CheckTeleportSelf();
    }

    private void CheckTeleportSelf()
    {
        if (playerTeleport > 0)
        {
            VRCPlayerApi lp = Networking.LocalPlayer;
            if (playerTeleport == lp.playerId)
            {
                lp.TeleportTo(playerTeleportToPos, playerTeleportToRot, VRC_SceneDescriptor.SpawnOrientation.Default, false);
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, nameof(TeleportSuccess));
            }
        }
    }

    public void TeleportSuccess()
    {
        if (!Networking.IsOwner(gameObject)) return;
        _playerTeleport = 0;
        _playerTeleportToPos = Vector3.zero;
        _playerTeleportToRot = Quaternion.identity;
        ApplyLocalTeleport();
    }

    public override void OnOwnershipTransferred(VRCPlayerApi player)
    {
        if (player.isLocal)
            ApplyLocalTeleport();
    }

    public override void OnDeserialization()
    {
        CheckTeleportSelf();
    }
}
