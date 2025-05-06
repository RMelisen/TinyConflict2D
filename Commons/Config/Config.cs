namespace TinyConflict2D.Commons.Config;

public class Config
{
    #region CONSTS
    
    #region Map
    
    public const int TILE_SIZE = 16;
    public const string FACTORY_TERRAINTYPE = "factory";
    public const string AIRPORT_TERRAINTYPE = "airport";
    public const string PORT_TERRAINTYPE = "port";
    public const string ROAD_TERRAINTYPE = "road";
    public const string CITY_TERRAINTYPE = "city";
    public const string HEADQUARTERS_TERRAINTYPE = "headquarters";
    public const string ANTENNA_TERRAINTYPE = "antenna";
    public const string SILO_TERRAINTYPE = "silo";
    public const string BRIDGE_TERRAINTYPE = "bridge";
    
    public const string PLAIN_TERRAINTYPE = "plain";
    public const string FOREST_TERRAINTYPE = "forest";
    public const string MOUNTAIN_TERRAINTYPE = "mountain";
    public const string WATER_TERRAINTYPE = "water";
    
    #endregion
    
    #region Game Menu Buttons
    
    public const string GAMEMENU_INFORMATION = "Information";
    public const string GAMEMENU_SETTINGS = "Settings";
    public const string GAMEMENU_ENDTURN = "EndTurn";
    
    #endregion
    
    #region Unit Action Menu Buttons
    
    public const string UNITACTION_WAIT = "wait";
    public const string UNITACTION_ATTACK = "attack";
    public const string UNITACTION_SUPPLY = "supply";
    public const string UNITACTION_CAPTURE = "capture";
    
    #endregion
    
    #region Terrain Costs
    
    // Defaults
    public const int DEFAULT_PLAIN_COST = 1;
    public const int DEFAULT_FOREST_COST = 2;
    public const int DEFAULT_MOUNTAIN_COST = 999;
    public const int DEFAULT_ROAD_COST = 1;
    public const int DEFAULT_WATER_COST = 999;
    public const int DEFAULT_ENEMY_OCCUPIED_COST = 999;
    
    // Infantry
    public const int INFANTRY_FOREST_COST = 1;
    public const int INFANTRY_MOUNTAIN_COST = 2;
    
    // Mech
    public const int MECH_FOREST_COST = 1;
    public const int MECH_MOUNTAIN_COST = 1;
    
    // TireA
    public const int TIREA_PLAIN_COST = 2;
    public const int TIREA_FOREST_COST = 3;
    
    // TireB
    public const int TIREB_PLAIN_COST = 1;
    public const int TIREB_FOREST_COST = 3;
    
    // Treads
    
    // Airborne
    public const int AIRBORNE_FOREST_COST = 1;
    public const int AIRBORNE_MOUNTAIN_COST = 1;
    public const int AIRBORNE_WATER_COST = 1;
    
    // Naval
    public const int NAVAL_PLAIN_COST = 999;
    public const int NAVAL_FOREST_COST = 999;
    public const int NAVAL_ROAD_COST = 999;
    public const int NAVAL_WATER_COST = 1;
    
    // Lander
    public const int LANDER_PLAIN_COST = 999;
    public const int LANDER_FOREST_COST = 999;
    public const int LANDER_ROAD_COST = 999;
    public const int LANDER_WATER_COST = 1;
        
    #endregion
    
    #region Players
    
    public const int STARTING_MONEY = 5000;
    public const int MONEY_PER_TURN = 1000;
    public const int MONEY_PER_BUILDING = 1000;
    public const int RED_PLAYER_NUMBER = 0;
    public const int BLUE_PLAYER_NUMBER = 1;
    public const int GREEN_PLAYER_NUMBER = 2;
    public const int ORANGE_PLAYER_NUMBER = 3;
    
    #endregion
    
    #region Custom Data

    public const string TERRAINTYPE_CUSTOMDATA = "TerrainType";
    public const string PROPERTYOWNER_CUSTOMDATA = "PropertyOwner";
    public const string MOVEMENTCOST_CUSTOMDATA = "MovementCost";

    #endregion

    #region Inputs

    public const string INPUT_UI_UP = "ui_up";
    public const string INPUT_UI_RIGHT = "ui_right";
    public const string INPUT_UI_DOWN = "ui_down";
    public const string INPUT_UI_LEFT = "ui_left";

    #endregion

    #region Buildings
    
    public const int BUILDING_CAPTURE_TRESHOLD = 200;
    
    #endregion
    
    #region Icon Name
    
    public const string AMMO_ICON_NAME = "AmmoIcon";
    public const string FUEL_ICON_NAME = "FuelIcon";
    public const string HEALTH1_ICON_NAME = "HealthIcon1";
    public const string HEALTH2_ICON_NAME = "HealthIcon2";
    public const string HEALTH3_ICON_NAME = "HealthIcon3";
    public const string HEALTH4_ICON_NAME = "HealthIcon4";
    public const string HEALTH5_ICON_NAME = "HealthIcon5";
    public const string HEALTH6_ICON_NAME = "HealthIcon6";
    public const string HEALTH7_ICON_NAME = "HealthIcon7";
    public const string HEALTH8_ICON_NAME = "HealthIcon8";
    public const string HEALTH9_ICON_NAME = "HealthIcon9";
    public const string CAPTURE_ICON_NAME = "CaptureIcon";
    
    #endregion
    
    #endregion

    #region PATHS

    public const string UIMANAGER_NODE_PATH = "Node2D/Core/UnitManager";
    public const string UNIT_SPRITES_PATH = "res://resources/TinyConflict/Tiles/Units/";

    #region Icons

    public const string ICON_CAPTURE_PATH = "res://resources/TinyConflict/Tiles/UI/IconCapture.png";
    public const string ICON_AMMO_PATH = "res://resources/TinyConflict/Tiles/UI/IconAmmo.png";
    public const string ICON_FUEL_PATH = "res://resources/TinyConflict/Tiles/UI/IconFuel.png";
    public const string ICON_HEALTH_0_PATH = "res://resources/TinyConflict/Tiles/UI/IconHealth0.png";
    public const string ICON_HEALTH_1_PATH = "res://resources/TinyConflict/Tiles/UI/IconHealth1.png";
    public const string ICON_HEALTH_2_PATH = "res://resources/TinyConflict/Tiles/UI/IconHealth2.png";
    public const string ICON_HEALTH_3_PATH = "res://resources/TinyConflict/Tiles/UI/IconHealth3.png";
    public const string ICON_HEALTH_4_PATH = "res://resources/TinyConflict/Tiles/UI/IconHealth4.png";
    public const string ICON_HEALTH_5_PATH = "res://resources/TinyConflict/Tiles/UI/IconHealth5.png";
    public const string ICON_HEALTH_6_PATH = "res://resources/TinyConflict/Tiles/UI/IconHealth6.png";
    public const string ICON_HEALTH_7_PATH= "res://resources/TinyConflict/Tiles/UI/IconHealth7.png";
    public const string ICON_HEALTH_8_PATH = "res://resources/TinyConflict/Tiles/UI/IconHealth8.png";
    public const string ICON_HEALTH_9_PATH= "res://resources/TinyConflict/Tiles/UI/IconHealth9.png";

    #endregion

    #endregion
}