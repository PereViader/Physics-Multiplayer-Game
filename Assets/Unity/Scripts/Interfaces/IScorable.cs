using UnityEngine;
using System.Collections;

public interface IScorable {
    // increase score by value
    void Score(int team, int value);
    // Set team score value
    void SetScore(int team, int value);
    // Set score value
    void SetScore(int[] score);
}
