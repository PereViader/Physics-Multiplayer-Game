using UnityEngine;
using System.Collections;

public interface IKillManager {
    void Killed(GameObject killed, PhotonPlayer killer);
}
