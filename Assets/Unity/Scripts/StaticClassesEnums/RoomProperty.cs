using UnityEngine;
using System.Collections;

public class RoomProperty {
    // photon network provides sql match making. Using registers C0 .. C10 you can set your own properties
    public const string GameMode = "C0";
    public const string Map = "C1";
}
