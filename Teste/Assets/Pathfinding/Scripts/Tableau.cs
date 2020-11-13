using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class pour créé un tableau 
/// </summary>
/// <typeparam name="TableauObjet"></typeparam>
public class Tableau<TableauObjet> {

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }

    private int largeur;
    private int heuteur;
    private float grosseurCellulle;
    private Vector3 positionOriginal;
    private TableauObjet[,] tableau;
    public const int sortingOrderDefault = 5000;
    /// <summary>
    /// Instanciation d'une tableau 
    /// </summary>
    /// <param name="largeur"></param>
    /// <param name="hauteur"></param>
    /// <param name="grosseurCellulle"></param>
    /// <param name="positionOriginal"></param>
    /// <param name="createGridObject"></param>
    public Tableau(int largeur, int hauteur, float grosseurCellulle, Vector3 positionOriginal, Func<Tableau<TableauObjet>, int, int, TableauObjet> createGridObject) {
        this.largeur = largeur;
        this.heuteur = hauteur;
        this.grosseurCellulle = grosseurCellulle;
        this.positionOriginal = positionOriginal;

        tableau = new TableauObjet[largeur, hauteur];

        for (int x = 0; x < tableau.GetLength(0); x++) {
            for (int y = 0; y < tableau.GetLength(1); y++) {
                tableau[x, y] = createGridObject(this, x, y);
            }
        }

        bool showDebug = false;
        if (showDebug) {
            TextMesh[,] debugTextArray = new TextMesh[largeur, hauteur];

            for (int x = 0; x < tableau.GetLength(0); x++) {
                for (int y = 0; y < tableau.GetLength(1); y++) {
                    debugTextArray[x, y] = CreateWorldText(tableau[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(grosseurCellulle, grosseurCellulle) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, hauteur), GetWorldPosition(largeur, hauteur), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(largeur, 0), GetWorldPosition(largeur, hauteur), Color.white, 100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.y].text = tableau[eventArgs.x, eventArgs.y]?.ToString();
            };
        }
    }
    /// <summary>
    /// Retrouver la largeur du tableau
    /// </summary>
    /// <returns></returns>
    public int GetWidth() {
        return largeur;
    }
    /// <summary>
    /// Retrouver la hauteur du tableau
    /// </summary>
    /// <returns></returns>
    public int GetHeight() {
        return heuteur;
    }
    /// <summary>
    /// Retrouver la grosseur de cellule du tableau
    /// </summary>
    /// <returns></returns>
    public float GetCellSize() {
        return grosseurCellulle;
    }
    /// <summary>
    /// Permet de trouvé la position de chaque cellule pour délimité son espace 
    /// </summary>
    /// <param name="x">Position en X</param>
    /// <param name="y">Position en Y</param>
    /// <returns></returns>
    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * grosseurCellulle + positionOriginal;
    }
    /// <summary>
    /// Permet de trouvé les x,y d'une cellule selon sa position dans l'environnement
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - positionOriginal).x / grosseurCellulle);
        y = Mathf.FloorToInt((worldPosition - positionOriginal).y / grosseurCellulle);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="value"></param>
    public void SetGridObject(int x, int y, TableauObjet value) {
        if (x >= 0 && y >= 0 && x < largeur && y < heuteur) {
            tableau[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void TriggerGridObjectChanged(int x, int y) {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="value"></param>
    public void SetGridObject(Vector3 worldPosition, TableauObjet value) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public TableauObjet GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < largeur && y < heuteur) {
            return tableau[x, y];
        } else {
            return default(TableauObjet);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public TableauObjet GetGridObject(Vector3 worldPosition) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
    /// <summary>
    /// Permet de créée du Texte pour le debug pour montré l'affiche du tableau 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="parent"></param>
    /// <param name="localPosition"></param>
    /// <param name="fontSize"></param>
    /// <param name="color"></param>
    /// <param name="textAnchor"></param>
    /// <param name="textAlignment"></param>
    /// <param name="sortingOrder"></param>
    /// <returns></returns>
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

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
