using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <unordered_set>
//状态压缩,枚举,回溯(深搜)
//状态压缩.9行9列9框，3个数组表示，各个数位状态可用二进制存在到2的9次即512表示，赋True或上1,赋alse与上0。
//由于数据小，不枚举
//直接回溯深搜，跑完所有结果
class Solution {
    int row[9], column[9], squere[3][3];
    vector<pair<int, int>> rest;
    int popCount(int mask)
    {
        int count = 0;
        for (int i = 1; i < 10; i++)
        {
            if ((mask >> i) & 1)count++;
        }
        return count;
    }
    int ctz(int mask)
    {
        for (int i = 1; i < 10; i++)
        {
            if (!((mask >> i) & 1))return i;
        }
        return 10;
    }
    void switchFlags(int x, int y, int digit)
    {
        row[x] ^= (1 << digit);
        column[y] ^= (1 << digit);
        squere[x / 3][y / 3] ^= (1 << digit);
    }
    bool dfs(int t, int count, vector<vector<char>>& b)
    {
        if (t >= count)return true;
        int i = rest[t].first;
        int j = rest[t].second;
        int mask = row[i] | column[j] | squere[i / 3][j / 3];
        if (popCount(mask) == 9)return false;
        for (int k = 1; k <= 9; k++)
        {
            if ((mask >> k) & 1)continue;
            switchFlags(i, j, k);
            b[i][j] = k + '0';
            if (dfs(t + 1, count, b))
            {
                return true;
            }
            b[i][j] = '.';
            switchFlags(i, j, k);
        }
        return false;
    }
public:
    void solveSudoku(vector<vector<char>>& board) {
        memset(row, 0, sizeof(row));
        memset(column, 0, sizeof(column));
        memset(squere, 0, sizeof(squere));
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i][j] != '.')
                {
                    switchFlags(i, j, board[i][j] - '0');
                }
            }
        }
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i][j] == '.')
                {
                    rest.emplace_back(i, j);
                }
            }
        }
        dfs(0, rest.size(), board);
    }
};
//状态压缩,枚举,回溯(深搜)
//状态压缩.9行9列9框，3个数组表示，各个数位状态可用二进制存在到2的9次即512表示，赋True或上1,赋alse与上0。
//先枚举，把可以填的都填上
//然后要猜的做回溯深搜，跑完所有结果
//class Solution {
//    int row[9], column[9], squere[3][3];
//    vector<pair<int, int>> rest;
//    int popCount(int mask)
//    {
//        int count = 0;
//        for (int i = 1; i < 10; i++)
//        {
//            if ((mask >> i) & 1)count++;
//        }
//        return count;
//    }
//    int ctz(int mask)
//    {
//        for (int i = 1; i < 10; i++)
//        {
//            if (!((mask >> i) & 1))return i;
//        }
//        return 10;
//    }
//    void switchFlags(int x, int y, int digit)
//    {
//        row[x] ^= (1 << digit);
//        column[y] ^= (1 << digit);
//        squere[x / 3][y / 3] ^= (1 << digit);
//    }
//    bool dfs(int t, int count, vector<vector<char>>& b)
//    {
//        if (t >= count)return true;
//        int i = rest[t].first;
//        int j = rest[t].second;
//        int mask= row[i] | column[j] | squere[i / 3][j / 3];
//        if (popCount(mask) == 9)return false;
//        for (int k = 1; k <= 9; k++)
//        {
//            if ((mask >> k) & 1)continue;
//            switchFlags(i, j, k);
//            b[i][j] = k+'0';
//            if (dfs(t + 1, count,b))
//            {
//                return true;
//            }
//            b[i][j] = '.';
//            switchFlags(i, j, k);
//        }
//        return false;
//    }
//public:
//    void solveSudoku(vector<vector<char>>& board) {
//        memset(row, 0, sizeof(row));
//        memset(column, 0, sizeof(column));
//        memset(squere, 0, sizeof(squere));
//        for (int i = 0; i < 9; i++)
//        {
//            for (int j = 0; j < 9; j++)
//            {
//                if (board[i][j] != '.')
//                {
//                    switchFlags(i, j, board[i][j] - '0');
//                }
//            }
//        }
//        int count = 1;
//        vector<char> anss;
//        int mask, digit;
//        while (count)
//        {
//            count = 0;
//            for (int i = 0; i < 9; i++)
//            {
//                for (int j = 0; j < 9; j++)
//                {
//                    if (board[i][j] == '.')
//                    {
//                        mask = (row[i] | column[j] | squere[i / 3][j / 3]);
//                        if (popCount(mask)==8)
//                        {
//                            digit = ctz(mask);
//                            board[i][j] = digit+'0';
//                            switchFlags(i, j, digit);
//                            count++;
//                        }
//                        anss.clear();
//                    }
//                }
//            }
//        }
//        for (int i = 0; i < 9; i++)
//        {
//            for (int j = 0; j < 9; j++)
//            {
//                if (board[i][j] == '.')
//                {
//                    rest.emplace_back(i,j);
//                }
//            }
//        }
//        dfs(0, rest.size(), board);
//    }
//};
 
//状态压缩,枚举，回溯(深搜)
//存储低效，状态量占用内存过大
//状态压缩。
//依次遍历计算各个格子记录可行解，填入唯一解，
//之后每次重新计算更改后其他受影响的格子，填入唯一解。直到没格子。
//class Solution {
//    vector<unordered_set<char>> answers;
//    string b;
//    vector<int> rest;
//    int m, n;
//    void eraseAns(int i, int j, int x, int y, bool canSet = true)
//    {
//        if (b[i * n + j] != '.')return;
//        if (j == y && i == x)return;
//        if (answers[i * n + j].count(b[x * n + y]))
//        {
//            answers[i * n + j].erase(answers[i * n + j].find(b[x * n + y]));
//        }
//        if (canSet)
//            if (answers[i * n + j].size() == 1)
//            {
//                b[i * n + j] = *(answers[i * n + j].begin());
//                eraseOtherAns(i, j);
//            }
//    }
//    void eraseOtherAns(int x, int y, bool canSet = true)
//    {
//        for (int i = 0; i < n; i++)
//        {
//            eraseAns(i, y, x, y, canSet);
//        }
//        for (int j = 0; j < n; j++)
//        {
//            eraseAns(x, j, x, y, canSet);
//        }
//        int down = x / 3 * 3 + 3, right = y / 3 * 3 + 3;
//        for (int i = down - 3; i < down; i++)
//        {
//            for (int j = right - 3; j < right; j++)
//            {
//                eraseAns(i, j, x, y, canSet);
//            }
//        }
//    }
//    bool dfs(int t, int count)
//    {
//        if (t >= count)return true;
//        int i = rest[t];
//        if (answers[i].size() == 0)
//            return false;
//        int x = i / n, y = i % n;
//        vector<unordered_set<char>> copy_answers = answers;
//        string copy_b = b;
//        int ansn = answers[i].size();
//        for (int j = 0; j < ansn; j++)
//        {
//            b[x * n + y] = *(next(answers[i].begin(), j));
//            eraseOtherAns(x, y, false);
//            if (dfs(t + 1, count))
//            {
//                return true;
//            }
//            b = copy_b;
//            answers = copy_answers;
//        }
//        return false;
//    }
//public:
//    void solveSudoku(vector<vector<char>>& board) {
//        answers = vector<unordered_set<char>>(81, { '1','2','3','4','5','6','7','8','9' });
//        m = 9, n = 9;
//        b.resize(m * n, '.');
//        for (int i = 0; i < m; i++)
//        {
//            for (int j = 0; j < n; j++)
//            {
//                b[i * n + j] = board[i][j];
//                if (b[i * n + j] != '.')
//                {
//                    eraseOtherAns(i, j);
//                }
//            }
//        }
//        for (int i = 0; i < m; i++)
//        {
//            for (int j = 0; j < n; j++)
//            {
//                if (b[i * n + j] == '.')
//                {
//                    rest.push_back(i * n + j);
//                }
//            }
//        }
//        dfs(0, rest.size());
//        for (int i = 0; i < m; i++)
//        {
//            for (int j = 0; j < n; j++)
//            {
//                board[i][j] = b[i * n + j];
//            }
//        }
//    }
//};


//int main()
//{
//    Solution s;
//    vector<vector<char>> board= {{'.', '.', '9', '7', '4', '8', '.', '.', '.'}, {'7', '.', '.', '.', '.', '.', '.', '.', '.'}, {'.', '2', '.', '1', '.', '9', '.', '.', '.'}, {'.', '.', '7', '.', '.', '.', '2', '4', '.'}, {'.', '6', '4', '.', '1', '.', '5', '9', '.'}, {'.', '9', '8', '.', '.', '.', '3', '.', '.'}, {'.', '.', '.', '8', '.', '3', '.', '2', '.'}, {'.', '.', '.', '.', '.', '.', '.', '.', '6'}, {'.', '.', '.', '2', '7', '5', '9', '.', '.'}};
//    s.solveSudoku(board);
//    for (int i = 0; i < 9; i++)
//    {
//        for (int j = 0; j < 9; j++)
//        {
//            cout << board[i][j]<<" ";
//        }
//        cout << endl;
//    }
//    return 0;
//}