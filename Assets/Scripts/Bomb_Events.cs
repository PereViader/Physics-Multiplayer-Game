using UnityEngine;
using System.Collections;

public class Bomb_Events {

    public delegate void EmptyDelegate();

    public static EmptyDelegate OnEndGame;

    public static void CallOnEndGame()
    {
        if (OnEndGame != null)
            OnEndGame();
    }
}
