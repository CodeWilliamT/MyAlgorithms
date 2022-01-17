using namespace std;
#include <iostream>
#include <vector>
//设计题 找规律
//坑：第一次从起点出发，余数为0的时候，方向得向下
class Robot {
private:
    int w, h;
    int x, y,dir;
    int mov[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };
    string ways[4] = { "East","North","West","South"};
public:
    Robot(int width, int height) {
        w = width;
        h = height;
        x = 0;
        y = 0;
        dir = 0;
    }

    void move(int num) {
        num = num % (2*(w+h-2));
        while (num > 0)
        {
            if (x + num * mov[dir][0] < 0){
                num -= x;
                x = 0;
            }
            else if (x + num * mov[dir][0] >= w) {
                num -= w - 1 - x;
                x = w - 1;
            }
            else if (y + num * mov[dir][1] < 0) {
                num -= y;
                y = 0;
            }
            else if(y + num * mov[dir][1] >= h){
                num -= h - 1 - y;
                y = h - 1;
            }
            else
            {
                x += num * mov[dir][0];
                y += num * mov[dir][1];
                num = 0;
                break;
            }
            dir =(dir + 1) % 4;
        }
        if (x== 0&&y==0)
        {
            dir = 3;
        }
    }

    vector<int> getPos() {
        return { x,y };
    }

    string getDir() {
        return ways[dir];
    }
};
