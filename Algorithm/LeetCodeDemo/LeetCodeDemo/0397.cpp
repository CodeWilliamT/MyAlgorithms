using namespace std;
#include <iostream>
#include <queue>
//广搜
class Solution {
typedef pair<long long, int> Point;
public:
    int integerReplacement(int n) {
        
        queue<Point> q;
        q.push({ n, 0 });
        while (!q.empty())
        {
            auto cur = q.front();
            if (cur.first == 1)
            {
                return cur.second;
            }
            q.pop();
            if (cur.first % 2)
            {
                q.push({ cur.first - 1,cur.second + 1 });
                q.push({ cur.first + 1,cur.second + 1 });
            }
            else
                q.push({ cur.first/2,cur.second + 1 });
        }
        return q.front().second;
    }
};