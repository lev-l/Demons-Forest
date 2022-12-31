using System;

struct Forest_levelsMap
{
    public string[,] GetMap()
    {
        string[,] map = new string[1, 3] {
            { "ForestT_0", "ForestT_1", "ForestT_2" }
        };

        return map;
    }
}
