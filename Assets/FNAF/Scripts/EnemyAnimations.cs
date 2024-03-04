using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct EnemyAnimaitonNames
{
    public string wakeup;
    public string walking;
    public string jumpscare;
}

class EnemyAnimations
{

    static readonly string bonnie_wakeup = "bonnie_wakeup";
    static readonly string bonnie_walking = "bonnie_walking";
    static readonly string bonnie_jumpscare = "bonnie_jumpscare";

    static readonly string freddy_wakeup = "freddy_wakeup";
    static readonly string freddy_walking = "freddy_walking";
    static readonly string freddy_jumpscare = "freddy_jumpscare";

    static readonly string chica_wakeup = "chica_wakeup";
    static readonly string chica_walking = "chica_walking";
    static readonly string chica_jumpscare = "chica_jumpscare";

    static readonly string endoskeleton_wakeup = "endoskeleton_wakeup";
    static readonly string endoskeleton_walking = "endoskeleton_walking";
    static readonly string endoskeleton_jumpscare = "endoskeleton_jumpscare";

    public static EnemyAnimaitonNames getAnimationNamesByCharacter(Characters character)
    {
        if (character == Characters.bonnie)
        {
            return new EnemyAnimaitonNames { wakeup= bonnie_wakeup, walking = bonnie_walking, jumpscare = bonnie_jumpscare };
        }
        else if (character == Characters.freddy)
        {
            return new EnemyAnimaitonNames { wakeup = freddy_wakeup, walking = freddy_walking, jumpscare = freddy_jumpscare };
        }
        else if (character == Characters.chica)
        {
            return new EnemyAnimaitonNames { wakeup = chica_wakeup, walking = chica_walking, jumpscare = chica_jumpscare };
        }
        else if (character == Characters.endoskeleton)
        {
            return new EnemyAnimaitonNames { wakeup = endoskeleton_wakeup, walking = endoskeleton_walking, jumpscare = endoskeleton_jumpscare };
        }

        return new EnemyAnimaitonNames();
    }

}
