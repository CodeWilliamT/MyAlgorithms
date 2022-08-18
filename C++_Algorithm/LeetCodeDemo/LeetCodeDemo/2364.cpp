using namespace std;
#include <vector>
#include <unordered_map>
//找规律 哈希
//坏数对=总数对-好数对
//总数对=n*(n-1)/2;好数对条件为为j-i==nums[j]-nums[i],即nums[i]-i == nums[j] - j
//记录每一个元素值与索引差相同的数目x。好数对为各个x*(x-1)/2的总和
class Solution {
    typedef long long ll;
public:
    long long countBadPairs(vector<int>& nums) {
        unordered_map<int, int> mp;
        ll n = nums.size();
        ll rst = n * (n - 1)/2;
        for (int i = 0; i < n;i++) {
            mp[nums[i] - i]++;
        }
        for (auto& e : mp) {
            rst -= ((ll)e.second * (e.second - 1)/2);
        }
        return rst;
    }
};