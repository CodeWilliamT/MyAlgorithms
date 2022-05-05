using namespace std;
#include <vector>
#include <string>
#include <unordered_set>
//找数组最多k个可被p整除的元素数的不同组成的连续非空子数组的数目。
//细致条件分析 哈希+前缀和+双指针
//最多k个可被p整除的元素数：前缀和统计前i个元素(包括i)的可被p整除的数的数目
//不同：搞成字符串做哈希
//连续非空，数组长度最多200：俩for
class Solution {
public:
    int countDistinct(vector<int>& nums, int k, int p) {
        int n = nums.size();
        vector<int> headcnts(n, 0);
        for (int i = 0; i < n; i++) {
            if(i>0)headcnts[i] = headcnts[i - 1];
            if (nums[i] % p == 0) {
                headcnts[i]++;
            }
        }
        unordered_set<string> st;
        string tmp;
        for (int i = 0; i < n; i++) {
            tmp = "";
            for (int j = i; j < n; j++) {
                if (headcnts[j] - (i > 0 ? headcnts[i - 1] : 0)>k) {
                    break;
                }
                tmp += to_string(nums[j]) + ",";
                if (!st.count(tmp)) {
                    st.insert(tmp);
                }
            }
        }
        return st.size();
    }
};