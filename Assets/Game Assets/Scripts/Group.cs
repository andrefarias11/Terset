using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Group : MonoBehaviour {

    // tempo da última queda, usado para auto queda após
    // tempo parametrizado por `level`
    private float lastFall;

    // tempo da última tecla pressionada, para lidar com o comportamento de pressão prolongada
    private float lastKeyDown;
    private float timeKeyPressed;

    public void AlignCenter() {
        transform.position += transform.position - Utils.Center(gameObject);
    }


    bool isValidGridPos() {
        foreach (Transform child in transform) {
            Vector2 v = Grid.roundVector2(child.position);

            // não tá dentro da Borda?
            if(!Grid.insideBorder(v)) {
                return false;
            }

            if (Grid.grid[(int)(v.x), (int)(v.y)] != null &&
                Grid.grid[(int)(v.x), (int)(v.y)].parent != transform) {
                return false;
            }
        }

        return true;
    }

    // atualiza a grid com os filhos do grupo
    void updateGrid() {
        // remove os filhos antigos da grid
        for (int y = 0; y < Grid.h; ++y) {
            for (int x = 0; x < Grid.w; ++x) {
                if (Grid.grid[x,y] != null &&
                    Grid.grid[x,y].parent == transform) {
                    Grid.grid[x,y] = null;
                }
            } 
        }

        insertOnGrid();
    }

    void insertOnGrid() {
        // adiciona os filhos atuais na grid
        foreach (Transform child in transform) {
            Vector2 v = Grid.roundVector2(child.position);
            Grid.grid[(int)v.x,(int)v.y] = child;
        }
    }

    void gameOver() {
        Debug.Log("Perdendo");
        while (!isValidGridPos()) {
            
            transform.position  += new Vector3(0, 1, 0);
        } 
        updateGrid(); // para não ultrapassar os grupos inválidos
        enabled = false; // desativar o script quando morre
        UIController.gameOver(); // ativa o menu de game over
        Highscore.Set(ScoreManager.score); // seta o highscore
    }

    void Start () {
        lastFall = Time.time;
        lastKeyDown = Time.time;
        timeKeyPressed = Time.time;
        if (isValidGridPos()) {
            insertOnGrid();
        } else { 
            Debug.Log("Iniciandooo AQUIII");
            gameOver();
        }

    }

    void tryChangePos(Vector3 v) {
        // alterando posição
        transform.position += v;

        if (isValidGridPos()) {
            updateGrid();
        } else {
            transform.position -= v;
        }
    }

    void fallGroup() {
        // modificar
        transform.position += new Vector3(0, -1, 0);

        if (isValidGridPos()){
            // se for valido, atualiza a grid
            updateGrid();
        } else {
            // não é valido, volta a posição anterior
            transform.position += new Vector3(0, 1, 0);

            // Linhas horizontais preenchidas e limpas
            Grid.deleteFullRows();


            FindObjectOfType<Spawner>().spawnNext();


            // desliga 
            enabled = false;
        }

        lastFall = Time.time;

    }

    
    bool getKey(KeyCode key) {
        bool keyDown = Input.GetKeyDown(key);
        bool pressed = Input.GetKey(key) && Time.time - lastKeyDown > 0.5f && Time.time - timeKeyPressed > 0.05f;

        if (keyDown) {
            lastKeyDown = Time.time;
        }
        if (pressed) {
            timeKeyPressed = Time.time;
        }
 
        return keyDown || pressed;
    }


    void Update () {
        if (UIController.isPaused) {
            return; // não faz nada se o jogo estiver pausado
        }
        if (getKey(KeyCode.LeftArrow)) {
            tryChangePos(new Vector3(-1, 0, 0));
        } else if (getKey(KeyCode.RightArrow)) {  // mover para a direita
            tryChangePos(new Vector3(1, 0, 0));
        } else if (getKey(KeyCode.UpArrow) && gameObject.tag != "Cube") { // rotação
            transform.Rotate(0, 0, -90);

            // verifica se é uma posição válida
            if (isValidGridPos()) {
                // se é valido atualiza a grid
                updateGrid();
            } else {
                // se não é valido, volta a posição anterior
                transform.Rotate(0, 0, 90);
            }
        } else if (getKey(KeyCode.DownArrow) || (Time.time - lastFall) >= (float)1 / Mathf.Sqrt(LevelManager.level)) {
            fallGroup();
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            while (enabled) { // queda até ao fundo
                fallGroup();
            }
        }

    }
}
