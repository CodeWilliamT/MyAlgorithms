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
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//简单模拟
//初始最小精力为所有人精力总和+1；初始最小经验为序模拟的正的差的和+1；
class Solution {
public:
    int minNumberOfHours(int initialEnergy, int initialExperience, vector<int>& energy, vector<int>& experience) {
        int mine=1,minex=0,nowex= 0;
        for (int i = 0; i < energy.size(); i++) {
            mine += energy[i];
            if (nowex <= experience[i])
                minex += experience[i] - nowex + 1, nowex = experience[i]+1;
            nowex += experience[i];
        }
        int rst = 0;;
        if (mine > initialEnergy)rst += mine - initialEnergy;
        if(minex> initialExperience)rst += minex - initialExperience;
        return rst;
    }
};