using UnityEngine;
using UnityEngine.UI;

public class UI_GenderChoice : MonoBehaviour
{
    [SerializeField] private GameObject _maleCharacter;
    [SerializeField] private GameObject _femaleCharacter;

    [SerializeField] private Button _maleButton;
    [SerializeField] private Button _femaleButton;

    private ECharacterType _characterType;

    void Start()
    {
        _maleButton.onClick.AddListener(() => OnClickGenderSelect(ECharacterType.Male));
        _femaleButton.onClick.AddListener(() => OnClickGenderSelect(ECharacterType.Female));
    }

    private void OnClickGenderSelect(ECharacterType type)
    {
        _characterType = type;
        _maleCharacter.SetActive(type == ECharacterType.Male);
        _femaleCharacter.SetActive(type == ECharacterType.Female);
    }
    
    
}
