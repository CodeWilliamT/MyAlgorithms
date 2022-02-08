using namespace std;
#include <vector>
#include <unordered_map>
#include <set>
//哈希 巧思
//第一反应复杂条件快速实现，模拟题
//数据10^9^2, 20000,稀疏表->
//状态压缩记录图，看来不行，不记录点亮的位置->
//巧思,哈希集记录灯的位置，哈希表记录各个行列斜亮的灯数。
class Solution {
public:
    vector<int> gridIllumination(int n, vector<vector<int>>& lamps, vector<vector<int>>& queries) {
        vector<int> ans= vector<int>(queries.size());
        set<pair<int, int>> st;
        unordered_map<int,int> rows,cols,lefts,rights;
        int dir[8][2] = { {1,1},{-1,-1},{1,-1},{-1,1}, {1,0},{0,1},{-1,0},{0,-1} };
        for (auto& e : lamps) {
            pair<int, int> tmp = { e[0],e[1] };
            if (!st.count(tmp)) {
                rows[e[0]]++;
                cols[e[1]]++;
                lefts[n - 1 - e[0] + e[1]]++;
                rights[e[0] + e[1]]++;
                st.insert(tmp);
            }
        }
        int i = 0;
        long long r, c;
        for (auto& e : queries) {
            ans[i] = rows[e[0]] || cols[e[1]] || lefts[n - 1 - e[0] + e[1]] ||rights[e[0] + e[1]];
            i++;
            r = e[0], c = e[1];
            if (st.count({ r,c })) {
                st.erase({ r,c });
                rows[r]--;
                cols[c]--;
                lefts[n - 1 - r + c]--;
                rights[r + c]--;
            }
            for (int k = 0; k < 8; k++) {
                r = e[0] + dir[k][0];
                c = e[1] + dir[k][1];
                if (st.count({ r,c })) {
                    st.erase({ r,c });
                    rows[r]--;
                    cols[c]--;
                    lefts[n - 1 - r + c]--;
                    rights[r + c]--;
                }
            }
        }
        return ans;
    }
};