using namespace std;
#include <vector>
#include <unordered_map>
#include <queue>
#include <functional>
//广搜 记忆化搜索 哈希
class Solution {
public:
    int minJumps(vector<int>& arr) {
        int n = arr.size();
        vector<int> f(n,n);
        unordered_map<int, vector<int>> mp;
        for (int i = 0; i < n;i++) {
            mp[arr[i]].push_back(i);
        }
        queue<int> q;
        q.push(0);
        int steps = 0;
        while (!q.empty()) {
            int len = q.size();
            while (len--) {
                int x = q.front();
                q.pop();
                if (f[x] != n)continue;
                f[x] = steps;
                if (x == n - 1)return steps;
                //做操作尝试
                while(!mp[arr[x]].empty()){
                    int e = mp[arr[x]].back();
                    if (e != x) {
                        q.push(e);
                    }
                    mp[arr[x]].pop_back();
                }
                if (x > 0)q.push(x-1);
                if (x < n - 1)q.push(x+1);
            }
            steps++;
        }
        return f[n - 1];
    }
};

//记忆化搜索 广搜 哈希
//超时
class Solution {
public:
    int minJumps(vector<int>& arr) {
        int n = arr.size();
        vector<int> f(n, n - 1);
        vector<bool> v(n, 0);
        unordered_map<int, vector<int>> mp;
        for (int i = 0; i < n; i++) {
            mp[arr[i]].push_back(i);
        }
        function<void(int, int)> dfs = [&](int x, int steps) {
            //排除无用操作
            if (steps > f[n - 1])return;
            //设标记 记结果
            v[x] = 1;
            f[x] = steps;
            //到达目标返回结果
            if (x == n - 1)return;
            //做操作尝试
            for (int& e : mp[arr[x]]) {
                if (e != x&&!v[e])
                    dfs(e, steps + 1);
            }
            if (x > 0)dfs(x - 1, steps + 1);
            if (x < n - 1)dfs(x + 1, steps + 1);
            //取消访问标记
            v[x] = 0;
        };
        dfs(0, 0);
        return f[n - 1];
    }
};

