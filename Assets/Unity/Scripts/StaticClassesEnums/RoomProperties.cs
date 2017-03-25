using UnityEngine;
using System.Collections;

public class RoomProperties {
    // photon network provides sql match making. Using registers C0 .. C10 you can set your own properties
    public const string GameMode = "C0";
    public const string Map = "C1";
    public const string Score = "Sc";
}
