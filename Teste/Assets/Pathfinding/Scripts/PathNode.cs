using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  class pour un noeud
/// </summary>
public class PathNode {

    private Tableau<PathNode> tableau;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public PathNode cameFromNode;
    /// <summary>
    /// Constructeur d'un noeud
    /// </summary>
    /// <param name="tableau"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public PathNode(Tableau<PathNode> tableau, int x, int y) {
        this.tableau = tableau;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }
    /// <summary>
    /// Calculeur de coût pour atteindre le but finial
    /// </summary>
    public void CalculateFCost() {
        fCost = gCost + hCost;
    }
    /// <summary>
    /// Permet de mettre un noeud marchable ou pas 
    /// </summary>
    /// <param name="isWalkable"></param>
    public void SetIsWalkable(bool isWalkable) {
        this.isWalkable = isWalkable;
        tableau.TriggerGridObjectChanged(x, y);
    }
    /// <summary>
    /// Permet de debug pour voir les positions des noeuds
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
        return x + "," + y;
    }

}
