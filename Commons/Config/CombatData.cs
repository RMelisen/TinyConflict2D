using Godot;
using TinyConflict2D.Commons.Enums;
using System.Collections.Generic;

namespace TinyConflict2D.Commons.Config;

public static class CombatData
{
    // Note for myself, the damage table for the original game is here : https://advancewars.fandom.com/wiki/Damage
    
    // Table for base damage : <Attacker, <Defenser, BaseDamage>>
    private static readonly Dictionary<UnitType, Dictionary<UnitType, int>> BaseDamageChart =
    new Dictionary<UnitType, Dictionary<UnitType, int>>()
    {            
        { 
            // Attacker Unit
            UnitType.AA, new Dictionary<UnitType, int>()
            {
                // Defender Units
                { UnitType.AA, 45 },
                { UnitType.APC, 50 },
                { UnitType.Battleship, 0 },
                { UnitType.BCopter, 120 },
                { UnitType.Bomber, 75 },
                { UnitType.Infantry, 105 },
                { UnitType.Lander, 0 },
                { UnitType.Mech, 105 },
                { UnitType.Recon, 60 },
                { UnitType.Submarine, 0 },
                { UnitType.Supply, 50 },   
                { UnitType.Tank, 25 },
                { UnitType.TCopter, 120 },
            }
        },            { 
            // Attacker Unit
            UnitType.APC, new Dictionary<UnitType, int>()
            {
                // Defender Units
                { UnitType.AA, 0 },
                { UnitType.APC, 0 },
                { UnitType.Battleship, 0 },
                { UnitType.BCopter, 0 },
                { UnitType.Bomber, 0 },
                { UnitType.Infantry, 0 },
                { UnitType.Lander, 0 },
                { UnitType.Mech, 0 },
                { UnitType.Recon, 0 },
                { UnitType.Submarine, 0 },
                { UnitType.Supply, 0 },   
                { UnitType.Tank, 0 },
                { UnitType.TCopter, 0 },
            }
        },            { 
            // Attacker Unit
            UnitType.Battleship, new Dictionary<UnitType, int>()
            {
                // Defender Units
                { UnitType.AA, 85 },
                { UnitType.APC, 80 },
                { UnitType.Battleship, 50 },
                { UnitType.BCopter, 0 },
                { UnitType.Bomber, 0 },
                { UnitType.Infantry, 95 },
                { UnitType.Lander, 95 },
                { UnitType.Mech, 90 },
                { UnitType.Recon, 90 },
                { UnitType.Submarine, 95 },
                { UnitType.Supply, 80 },   
                { UnitType.Tank, 85 },
                { UnitType.TCopter, 0 },
            }
        },            { 
            // Attacker Unit
            UnitType.BCopter, new Dictionary<UnitType, int>()
            {
                // Defender Units
                { UnitType.AA, 25 },
                { UnitType.APC, 60 },
                { UnitType.Battleship, 25 },
                { UnitType.BCopter, 65 },
                { UnitType.Bomber, 0 },
                { UnitType.Infantry, 75 },
                { UnitType.Lander, 25 },
                { UnitType.Mech, 75 },
                { UnitType.Recon, 55 },
                { UnitType.Submarine, 25 },
                { UnitType.Supply, 60 },   
                { UnitType.Tank, 55 },
                { UnitType.TCopter, 95 },
            }
        },            { 
            // Attacker Unit
            UnitType.Bomber, new Dictionary<UnitType, int>()
            {
                // Defender Units
                { UnitType.AA, 95 },
                { UnitType.APC, 105 },
                { UnitType.Battleship, 75 },
                { UnitType.BCopter, 0 },
                { UnitType.Bomber, 0 },
                { UnitType.Infantry, 110 },
                { UnitType.Lander, 95 },
                { UnitType.Mech, 110 },
                { UnitType.Recon, 105 },
                { UnitType.Submarine, 95 },
                { UnitType.Supply, 105 },   
                { UnitType.Tank, 105 },
                { UnitType.TCopter, 0 },
            }
        },
        { 
            // Attacker Unit
            UnitType.Infantry, new Dictionary<UnitType, int>()
            {
                // Defender Units
                { UnitType.AA, 5 },
                { UnitType.APC, 14 },
                { UnitType.Battleship, 0 },
                { UnitType.BCopter, 7 },
                { UnitType.Bomber, 0 },
                { UnitType.Infantry, 55 },
                { UnitType.Lander, 0 },
                { UnitType.Mech, 45 },
                { UnitType.Recon, 12 },
                { UnitType.Submarine, 0 },
                { UnitType.Supply, 14 },   
                { UnitType.Tank, 5 },
                { UnitType.TCopter, 30 },
            }
        },
        { 
            // Attacker Unit
            UnitType.Lander, new Dictionary<UnitType, int>()
            {
                // Defender Units
                { UnitType.AA, 0 },
                { UnitType.APC, 0 },
                { UnitType.Battleship, 0 },
                { UnitType.BCopter, 0 },
                { UnitType.Bomber, 0 },
                { UnitType.Infantry, 0 },
                { UnitType.Lander, 0 },
                { UnitType.Mech, 0 },
                { UnitType.Recon, 0 },
                { UnitType.Submarine, 0 },
                { UnitType.Supply, 0 },   
                { UnitType.Tank, 0 },
                { UnitType.TCopter, 0 },
            }
        },
        { 
            // Attacker Unit
            UnitType.Mech, new Dictionary<UnitType, int>()
            {
                // Defender Units
                { UnitType.AA, 65 },
                { UnitType.APC, 75 },
                { UnitType.Battleship, 0 },
                { UnitType.BCopter, 9 },
                { UnitType.Bomber, 0 },
                { UnitType.Infantry, 65 },
                { UnitType.Lander, 0 },
                { UnitType.Mech, 55 },
                { UnitType.Recon, 85 },
                { UnitType.Submarine, 0 },
                { UnitType.Supply, 75 },   
                { UnitType.Tank, 55 },
                { UnitType.TCopter, 30 },
            }
        },
        { 
            // Attacker Unit
            UnitType.Recon, new Dictionary<UnitType, int>()
            {
                // Defender Units
                { UnitType.AA, 4 },
                { UnitType.APC, 45 },
                { UnitType.Battleship, 0 },
                { UnitType.BCopter, 10 },
                { UnitType.Bomber, 0 },
                { UnitType.Infantry, 70 },
                { UnitType.Lander, 0 },
                { UnitType.Mech, 65 },
                { UnitType.Recon, 35 },
                { UnitType.Submarine, 0 },
                { UnitType.Supply, 45 },   
                { UnitType.Tank, 6 },
                { UnitType.TCopter, 35 },
            }
        },
        { 
            // Attacker Unit
            UnitType.Submarine, new Dictionary<UnitType, int>()
            {
                // Defender Units
                { UnitType.AA, 0 },
                { UnitType.APC, 0 },
                { UnitType.Battleship, 55 },
                { UnitType.BCopter, 0 },
                { UnitType.Bomber, 0 },
                { UnitType.Infantry, 0 },
                { UnitType.Lander, 95 },
                { UnitType.Mech, 0 },
                { UnitType.Recon, 0 },
                { UnitType.Submarine, 55 },
                { UnitType.Supply, 0 },   
                { UnitType.Tank, 0 },
                { UnitType.TCopter, 0 },
            }
        },
        { 
            // Attacker Unit
            UnitType.Supply, new Dictionary<UnitType, int>()
            {
                // Defender Units
                { UnitType.AA, 0 },
                { UnitType.APC, 0 },
                { UnitType.Battleship, 0 },
                { UnitType.BCopter, 0 },
                { UnitType.Bomber, 0 },
                { UnitType.Infantry, 0 },
                { UnitType.Lander, 0 },
                { UnitType.Mech, 0 },
                { UnitType.Recon, 0 },
                { UnitType.Submarine, 0 },
                { UnitType.Supply, 0 },   
                { UnitType.Tank, 0 },
                { UnitType.TCopter, 0 },
            }
        },
        { 
            // Attacker Unit
            UnitType.Tank, new Dictionary<UnitType, int>()
            {
                // Defender Units
                { UnitType.AA, 65 },
                { UnitType.APC, 75 },
                { UnitType.Battleship, 1 },
                { UnitType.BCopter, 10 },
                { UnitType.Bomber, 0 },
                { UnitType.Infantry, 75 },
                { UnitType.Lander, 10 },
                { UnitType.Mech, 70 },
                { UnitType.Recon, 85 },
                { UnitType.Submarine, 1 },
                { UnitType.Supply, 75 },   
                { UnitType.Tank, 55 },
                { UnitType.TCopter, 40 },
            }
        },
        { 
            // Attacker Unit
            UnitType.TCopter, new Dictionary<UnitType, int>()
            {
                // Defender Units
                { UnitType.AA, 0 },
                { UnitType.APC, 0 },
                { UnitType.Battleship, 0 },
                { UnitType.BCopter, 0 },
                { UnitType.Bomber, 0 },
                { UnitType.Infantry, 0 },
                { UnitType.Lander, 0 },
                { UnitType.Mech, 0 },
                { UnitType.Recon, 0 },
                { UnitType.Submarine, 0 },
                { UnitType.Supply, 0 },   
                { UnitType.Tank, 0 },
                { UnitType.TCopter, 0 },
            }
        }
    };

    public static int GetBaseDamagePercentage(UnitType attackerType, UnitType defenderType)
    {
        if (BaseDamageChart.TryGetValue(attackerType, out Dictionary<UnitType, int> defenderMatchups))
        {
            if (defenderMatchups.TryGetValue(defenderType, out int baseDamage))
            {
                return baseDamage;
            }
        }

        GD.PrintErr($"Base damage not defined for {attackerType} attacking {defenderType}.");
        return 0;
    }
}
