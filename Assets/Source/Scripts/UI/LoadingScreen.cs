using Source.Scripts.UI;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private LoadingText _loadingText;

    public LoadingText LoadingText => _loadingText;
}