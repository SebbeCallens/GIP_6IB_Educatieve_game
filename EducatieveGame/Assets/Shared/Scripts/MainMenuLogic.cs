using UnityEngine;

public class MainMenuLogic : MenuLogic
{
    [SerializeField] private GameObject _pokemonPuzzels; //prefab van alle pokemon puzzels

    private GameObject PokemonPuzzels { get => _pokemonPuzzels; set => _pokemonPuzzels = value; }

    private void Awake() //laad de pokemon puzzels bij opstarten, voorkomt lag laden puzzels
    {
        AwakeBase();
        if (GameObject.FindWithTag("Pokemon") == null)
        {
            GameObject pokemonPuzzels = Instantiate(PokemonPuzzels);
            DontDestroyOnLoad(pokemonPuzzels);
        }
    }
}
