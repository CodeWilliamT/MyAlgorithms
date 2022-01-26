using namespace std;
#include <iostream>
#include <vector>
#include <unordered_map>
//哈希
//哈希表记录顶点，然后查询顶点时，遍历哈希集同x轴顶点计算其他俩点是否存在
class DetectSquares {
    vector<unordered_map<short, short>> v;
public:
    DetectSquares() {
        v= vector<unordered_map<short, short>>(1001);
    }

    void add(vector<int> point) {
        v[point[0]][point[1]]++;
    }

    int count(vector<int> point) {
        int x,y,cnt=0;
        for (auto& e : v[point[0]]) {
            if (e.first == point[1])continue;
            x = point[0] + e.first - point[1];
            y = e.first;
            if (x<1001 && x>-1 && y<1001 && y>-1 && v[x].count(point[1]) && v[x].count(y))
                cnt += v[point[0]][y] * v[x][point[1]] * v[x][y];
            x = point[0] - e.first + point[1];
            y = e.first;
            if (x<1001 && x>-1 && y<1001 && y>-1 && v[x].count(point[1]) && v[x].count(y))
                cnt += v[point[0]][y] * v[x][point[1]] * v[x][y];
        }
        return cnt;
    }
};
//朴素思路
//直接遍历所有宽度所有方向的四个方向的正方形其他三个点是否存在
class DetectSquares {
    short v[1001][1001]{};
    int dir[4][2] = { {1,1},{1,-1},{-1,1},{-1,-1} };
public:
    DetectSquares() {
        memset(v, 0, sizeof(v));
    }

    void add(vector<int> point) {
        v[point[0]][point[1]]++;
    }

    int count(vector<int> point) {
        int x, y, cnt = 0;
        for (int i = 1; i < 1001; i++) {
            for (int j = 0; j < 4; j++) {
                x = point[0] + dir[j][0] * i;
                y = point[1] + dir[j][1] * i;
                if (x<1001 && x>-1 && y<1001 && y>-1)
                    cnt += v[point[0]][y] * v[x][point[1]] * v[x][y];
            }
        }
        return cnt;
    }
};