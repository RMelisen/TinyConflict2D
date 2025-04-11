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
    
    #region Terrain Costs
    
    // Defaults
    public const int DEFAULT_PLAIN_COST = 1;
    public const int DEFAULT_FOREST_COST = 2;
    public const int DEFAULT_MOUNTAIN_COST = 999;
    public const int DEFAULT_ROAD_COST = 1;
    public const int DEFAULT_WATER_COST = 999;
    
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
    
    #endregion
    
    #region Custom Data

    public const string TERRAINTYPE_CUSTOMDATA = "TerrainType";
    public const string PROPERTYOWNER_CUSTOMDATA = "PropertyOwner";
    public const string MOVEMENTCOST_CUSTOMDATA = "MovementCost";
    
    #endregion

    #endregion
    
    #region PATHS

    public const string UIMANAGER_NODE_PATH = "Node2D/Core/UnitManagerCore";
    public const string UNIT_SPRITES_PATH = "res://resources/TinyConflict/Tiles/Units/";

    #endregion
}