using UnityEngine;
using System.Collections;

public interface IGame {
    void OnRoundSetup();
    void OnRoundStart();
    void OnRoundEnd();
}
