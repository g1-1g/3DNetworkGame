using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public static class PlayerScore
{
    public const string ScoreKey = "score";

    public static void EnsureLocalScore()
    {
        if (PhotonNetwork.LocalPlayer == null)
        {
            return;
        }

        if (PhotonNetwork.LocalPlayer.CustomProperties != null &&
            PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(ScoreKey))
        {
            return;
        }

        var props = new Hashtable { { ScoreKey, 0 } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public static int GetScore(Player player)
    {
        if (player == null || player.CustomProperties == null)
        {
            return 0;
        }

        if (player.CustomProperties.TryGetValue(ScoreKey, out var value))
        {
            if (value is int intValue) return intValue;
        }

        return 0;
    }

    public static void AddLocalScore(int delta)
    {
        if (PhotonNetwork.LocalPlayer == null)
        {
            return;
        }

        int current = GetScore(PhotonNetwork.LocalPlayer);
        var props = new Hashtable { { ScoreKey, current + delta } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }
}
