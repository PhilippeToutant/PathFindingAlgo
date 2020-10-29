using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tableau 
{
    private int hauteur;
    private int largeur;
    private float formatCell;
    private int[,] tableau;
    public const int sortingOrderDefault = 5000;
    private TextMesh[,] debugTextArray;

    public Tableau(int largeur, int hauteur,float formatCell)
    {
        this.hauteur = hauteur;
        this.largeur = largeur;
        this.formatCell = formatCell;

        tableau = new int[hauteur, largeur];
        debugTextArray = new TextMesh[largeur, hauteur];
        for (int x = 0; x < tableau.GetLength(0); x++)
        {
            for (int y = 0; y < tableau.GetLength(1); y++)
            {
                debugTextArray[x,y] = CreateWorldText(tableau[x, y].ToString(),null, ChercheurPosition(x,y) + new Vector3(formatCell,formatCell)* .5f,20,Color.white,TextAnchor.MiddleCenter);
                Debug.DrawLine(ChercheurPosition(x, y), ChercheurPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(ChercheurPosition(x, y), ChercheurPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(ChercheurPosition(0, hauteur), ChercheurPosition(largeur, hauteur), Color.white, 100f);
        Debug.DrawLine(ChercheurPosition(largeur, 0), ChercheurPosition(largeur, hauteur), Color.white, 100f);
    }

    private Vector3 ChercheurPosition(int x,int y)
    {
        return new Vector3(x, y) * formatCell;
    }

    private void GetXY (Vector3 position, out int x, out int y)
    {
        x = Mathf.FloorToInt(position.x / formatCell);
        y = Mathf.FloorToInt(position.y / formatCell);
    }

    public void SetValue(int x , int y, int value)
    {
        if (x >= 0 && y >= 0 && x < largeur && y < hauteur) 
        {
            tableau[x, y] = value;
            debugTextArray[x, y].text = tableau[x, y].ToString();
        }
    }

    public void SetValue(Vector3 position, int value)
    {
        int x;
        int y;
        GetXY(position,out x, out y);
        SetValue(x, y, value);
    }

    // Create Text in the World
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
}
