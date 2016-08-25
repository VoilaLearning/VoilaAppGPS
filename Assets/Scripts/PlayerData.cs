using UnityEngine;

public static class PlayerData {

    static string playerName = "Student";
    static int points;
    // currency?
    // milestones


    public static void SetPlayerName (string newPlayerName) {

        playerName = newPlayerName;
    }

    public static string GetPlayerName () {

        return playerName;
    }
}