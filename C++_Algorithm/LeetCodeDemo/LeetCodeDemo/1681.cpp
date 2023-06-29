using namespace std;
#include <iostream>
#include <vector>
#include <unordered_map>
#include <map>
#include <functional>
//深搜 回溯 枚举
//以序列表示情况，每k个一组，枚举所有符合要求的序列，刷出最小的子集最大差的和。
//剪枝：组间第一个增序，组内增序，
class Solution {
public:
    int minimumIncompatibility(vector<int>& nums, int k) {
        int n = nums.size();
        if (n % k)return -1;
        if (n == k)return 0;

        map<int, int> mp; 

        for (int& e : nums) {
            mp[e]++;
            if (mp[e] > k)return -1;
        }
        int digs = mp.size();
        vector<int> keys(digs);
        unordered_map<int, int>ump;
        int index = 0;
        for (auto& e : mp) {
            ump[e.first] = index;
            keys[index++] = e.first;
        }
        int num = n / k;
        int rst = 257;
        int mn, mx;
        vector<int> line;
        function<void(int, int, uint32_t)> dfs = [&](int idx, int sum, uint32_t rep) {
            if (sum >= rst) {
                return;
            }
            if (idx == n) {
                rst = min(sum, rst);
                return;
            }
            int id = idx % num;
            int pos = id?ump[line[idx - 1]]+1: ump[(idx-num<0)?1:line[idx-num]];
            for (int x = pos; x < digs; x++) {
                int i = keys[x];
                if (!mp[i])continue;
                if (id <= num - 1 && (rep >> i) & 1) {
                    continue;
                }
                mp[i]--;
                rep = rep + (1 << i);
                line.push_back(i);

                if (id == num - 1) {
                    mn = line[idx-num+1], mx = line[idx];
                    dfs(idx + 1, sum + mx - mn, 0);
                }
                else {
                    dfs(idx + 1, sum, rep);
                }
                mp[i]++;
                rep = rep - (1 << i);
                line.pop_back();
                if (!id) {
                    break;
                }
            }
            return;
        };
        dfs(0, 0, 0);
        return rst;
    }
};