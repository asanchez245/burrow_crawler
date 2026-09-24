using UnityEngine;

public class TESTZONE : MonoBehaviour
{
    public ManaOrb manaGenerator;
    public void Emmit10NeutralMana()
    {
        ManaPoolController.Instance.SpawnManaOrbs(ManaPoolController.Instance.transform.position, ManaOrb.OrbType.NeutralMana, 10);
    }
    public void Emmit10RedMana()
    {
        ManaPoolController.Instance.SpawnManaOrbs(ManaPoolController.Instance.transform.position, ManaOrb.OrbType.RedMana, 10);
    }

    public void Emmit10BlueMana()
    {
        ManaPoolController.Instance.SpawnManaOrbs(ManaPoolController.Instance.transform.position, ManaOrb.OrbType.BlueMana, 10);
    }
    public void Emmit10GreenMana()
    {
        ManaPoolController.Instance.SpawnManaOrbs(ManaPoolController.Instance.transform.position, ManaOrb.OrbType.GreenMana, 10);
    }
}
