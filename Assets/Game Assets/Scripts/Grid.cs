using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grid : MonoBehaviour {

    
    public static int w = 10;
    public static int h = 20;
   
    public static Transform[,] grid = new Transform[w, h];

    // converter um vetor real em coordenadas discretas usando Mathf.Round
    public static Vector2 roundVector2(Vector2 v) {
        return new Vector2 (Mathf.Round (v.x), Mathf.Round (v.y));
    }

    // verifica se algum vetor está dentro dos limites do jogo (borders left, right and down)
    public static bool insideBorder(Vector2 pos) {
        return ((int)pos.x >= 0 &&
                (int)pos.x < w &&
                (int)pos.y >= 0);
    }

    // destruir a linha na linha y
    public static void deleteRow(int y) {
        for (int x = 0; x < w; x++) {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }


    // Sempre que uma linha é eliminada, as linhas acima devem cair uma unidade em direção ao fundo. 
    // a decreaseRow serve para isso
    public static void decreaseRow(int y) {
        for (int x = 0; x < w; x++) {
            if (grid[x, y] != null) {
                // move um para baixo
                grid[x, y - 1] = grid[x, y];
                grid[x,y] = null;

                // atualiza a posição do bloco
                grid[x, y-1].position += new Vector3(0, -1, 0);
            }
        }
    }

    // sempre que uma linha é eliminada, todas as linhas acima devem ser diminuídas em 1
    public static void decreaseRowAbove(int y) {
        for (int i = y; i < h; i++) {
            decreaseRow(i);
        }
    }

    // verificar se uma linha está cheia e, portanto, pode ser eliminada (pontuação +1)
    public static bool isRowFull(int y){
        for (int x = 0; x < w; x++) {
            if (grid[x, y] == null) {
                return false;
            }
        }
        return true;

    }

    public static void deleteFullRows() {
        for (int y = 0; y < h; y++) {
            if (isRowFull(y)) {
                deleteRow(y);
                decreaseRowAbove(y + 1);
                // adicionar novos pontos à pontuação quando uma linha é eliminada
                ScoreManager.score += (h - y) * 10;
                --y;
            }
        }
    }

}
