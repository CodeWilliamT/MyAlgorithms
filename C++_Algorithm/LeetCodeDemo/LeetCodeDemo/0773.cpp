using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <queue>
#include <unordered_set>
//广搜，哈希判重
//状态存储结构
class Solution {
    struct State
    {
        string order="";
        vector<int> lct = { 0,0 };
        int step=0;
        State(string x,vector<int> y, int z) :order(x),lct(y),step(z) {}
    };
public:
    int slidingPuzzle(vector<vector<int>>& board) {
        int n = board.size();
        if (n < 2)return n;
        int m = board[0].size();
        if (m < 2)return m;
        State* start, * end, * cur;
        string str;
        queue<State*> q;
        unordered_set<string> hash;
        vector<int> loc;
        for(int i=0;i<n;i++)
            for (int j = 0; j < m; j++)
            {
                if (!board[i][j])loc = { i,j };
                str.push_back('0'+board[i][j]);
            }
        start = new State(str, loc, 0);
        end = new State("123450", {1,2}, 0);
        if (start->order == end->order)return start->step;
        q.push(start);
        while (!q.empty())
        {
            cur = q.front();
            q.pop();
            if (hash.count(cur->order))
                continue;
            hash.insert(cur->order);
            cur->step++;
            str = cur->order;
            if (cur->lct[0] != 0)//Up
            {
                swap(cur->order[cur->lct[0] * m + cur->lct[1]], cur->order[(cur->lct[0]-1) * m + cur->lct[1]]);
                if (cur->order == end->order)return cur->step;
                cur->lct[0]--;
                q.push(new State(cur->order,cur->lct,cur->step));
                cur->lct[0]++;
                cur->order =str;
            }
            if (cur->lct[0] != 1)//D
            {
                swap(cur->order[cur->lct[0] * m + cur->lct[1]], cur->order[(cur->lct[0] +1) * m + cur->lct[1]]);
                if (cur->order == end->order)return cur->step;
                cur->lct[0]++;
                q.push(new State(cur->order, cur->lct, cur->step));
                cur->lct[0]--;
                cur->order = str;
            }
            if (cur->lct[1] != 0)//L
            {
                swap(cur->order[cur->lct[0] * m + cur->lct[1]], cur->order[cur->lct[0] * m + (cur->lct[1]-1)]);
                if (cur->order == end->order)return cur->step;
                cur->lct[1]--;
                q.push(new State(cur->order, cur->lct, cur->step));
                cur->lct[1]++;
                cur->order = str;
            }
            if (cur->lct[1] != 2)//R
            {
                swap(cur->order[cur->lct[0] * m + cur->lct[1]], cur->order[cur->lct[0] * m + (cur->lct[1]+1)]);
                if (cur->order == end->order)return cur->step;
                cur->lct[1]++;
                q.push(new State(cur->order, cur->lct, cur->step));
                cur->lct[1]--;
                cur->order = str;
            }
        }
        return -1;
    }
};
//int main()
//{
//    Solution s;
//    vector<vector<int>> board = { {4, 1, 2 }, { 5,0,3 } };
//    cout<<s.slidingPuzzle(board)<<endl;
//    
//    return 0;
//}