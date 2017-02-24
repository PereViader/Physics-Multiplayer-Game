public interface IGame {
    void OnGameSetup();
    void OnGameStart();
    void OnRoundSetup();
    void OnRoundStart();
    void OnRoundEnd();
    void OnGameEnd();
}
