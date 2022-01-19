using namespace std;
#include <iostream>
#include <vector>
#include <queue>
//设计题，队列，动态数组，栈
class SnakeGame {
public:
    int width, height;
    int fi;
    int score;
    bool foodVisable;
    vector<vector<int>> snake;
    vector<vector<int>> food;
    
    /** Initialize your data structure here.
        @param width - screen width
        @param height - screen height
        @param food - A list of food positions
        E.g food = [[1,1], [1,0]] means the first food is positioned at [1,1], the second is at [1,0]. */
    SnakeGame(int _width, int _height, vector<vector<int>>& _food) {
        width = _width;
        height = _height;
        food = _food;
        fi = 0;
        snake.push_back({ 0,0 });
        score = 0;
        foodVisable = food[fi] != snake[0];
    }
    bool judge(vector<int> pos)
    {
        if (pos[0] < 0)return 0;
        if (pos[0] >= height)return 0;
        if (pos[1] < 0)return 0;
        if (pos[1] >= width)return 0;
        for (auto item : snake)
        {
            if (item == pos)return 0;
        }
        return 1;
    }
    void foodDisplay()
    {
        foodVisable = judge(food[fi]);
    }
    void eat(vector<int> head)
    {
        if (fi >= food.size())goto end;
        foodDisplay();
        if (!foodVisable)goto end;
        if (head == food[fi])
        {
            score++;
            fi++;
            if (fi >= food.size())return;
            foodDisplay();
            return;
        }
        end:
        snake.erase(snake.begin());
    }
    /** Moves the snake.
        @param direction - 'U' = Up, 'L' = Left, 'R' = Right, 'D' = Down
        @return The game's score after the move. Return -1 if game over.
        Game over when snake crosses the screen boundary or bites its body. */
    int move(string direction) {
        vector<int> head;
        if (direction == "U")
        {
            head = { snake.back()[0] - 1,snake.back()[1] };
        }
        else if (direction == "L")
        {
            head = { snake.back()[0],snake.back()[1] - 1 };
        }
        else if (direction == "R")
        {
            head = { snake.back()[0],snake.back()[1] + 1 };
        }
        else if (direction == "D")
        {
            head = { snake.back()[0] + 1,snake.back()[1] };
        }
        else
        {
            return -1;
        }
        eat(head);
        if (!judge(head))return -1;
        snake.push_back(head);
        return score;
    }
};

//int main()
//{
//    vector<vector<int> >food = { {0, 1}, { 0, 2} , { 1, 2} , { 2, 2} , { 2, 1} , { 2, 0}, { 1, 0} };
//    SnakeGame snakeGame = SnakeGame(3, 3, food);
//    cout << snakeGame.move("D") << endl;
//    cout << snakeGame.move("D") << endl;
//    cout << snakeGame.move("R") << endl;
//    cout << snakeGame.move("U") << endl;
//    cout << snakeGame.move("U") << endl;
//    cout << snakeGame.move("L") << endl;
//    cout << snakeGame.move("D") << endl;
//    cout << snakeGame.move("R") << endl;
//    cout << snakeGame.move("R") << endl;
//    cout << snakeGame.move("U") << endl;
//    cout << snakeGame.move("L") << endl;;
//    cout << snakeGame.move("L") << endl;
//    cout << snakeGame.move("D") << endl;
//    cout << snakeGame.move("R") << endl;
//    cout << snakeGame.move("U") << endl;
//
//    return 0;
//}