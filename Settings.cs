using System;
using UnityEngine;
using UnityEngine.UI;

public class Settings {
    public static string SeedString = "";
    public static int Score = 0;
    public static int PumpkinsScored = 0;
    public static int PumpkinsSmashedOnBarriers = 0;
    public static Material SpriteMaterial = Resources.Load<Material>("SpritesDiffuse");
    public static int PumpkinsLeft = 0;
    public static int EnemiesKilled = 0;
    public static bool PlayerDidAnything = false;
    public static float DistanceTravelled = 0;
    public static bool[] RibbonsUnlocked = new bool[10];
}
