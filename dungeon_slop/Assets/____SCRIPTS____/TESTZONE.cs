using UnityEngine;

public class TESTZONE : MonoBehaviour
{
    public ManaOrbGenerator manaGenerator;
    public void Emmit10NeutralMana()
    {
        manaGenerator.EmitExactParticles(10, ManaOrbGenerator.OrbType.NeutralMana);
    }
    public void Emmit10RedMana()
    {
        manaGenerator.EmitExactParticles(10, ManaOrbGenerator.OrbType.RedMana);
    }

    public void Emmit10BlueMana()
    {
        manaGenerator.EmitExactParticles(10, ManaOrbGenerator.OrbType.BlueMana);
    }
    public void Emmit10YellowMana()
    {
        manaGenerator.EmitExactParticles(10, ManaOrbGenerator.OrbType.YellowMana);
    }
}
