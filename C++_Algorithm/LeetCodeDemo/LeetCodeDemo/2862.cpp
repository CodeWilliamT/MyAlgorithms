using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
#include "myAlgo\Structs\TreeNode.cpp"
typedef pair<int, bool> pib;
typedef pair<int, int> pii;
//巧思 数学性质分析
//需先观察出：构成完全组的元素，分解质因数后，出现为奇数个数的所有质数的乘积，相同。
class Solution {
    typedef long long ll;
    int core(int x) {
        int rst = 1;
        int cnt;
        for (int i = 2; i <= x; i++) {
            cnt = 0;
            while(x % i == 0) {
                x /= i;
                cnt++;
            }
            if (cnt % 2) {
                rst *= i;
            }
        }
        return rst;
    }
public:
    long long maximumSum(vector<int>& nums) {
        unordered_map<int, vector<int>> mp;
        int n = nums.size();
        for (int i = 1; i <= n; i++) {
            mp[core(i)].push_back(i);
        }
        ll rst = 0,tmp;
        for (auto& [k,v]: mp) {
            tmp = 0;
            for (int& e : v) {
                tmp += nums[e-1];
            }
            rst = max(rst, tmp);
            
        }
        return rst;
    }
};