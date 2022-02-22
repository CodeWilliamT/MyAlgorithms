using namespace std;
#include <vector>
//动态规划
//对[2,30]里的每个数重复两个操作：1、分解得到质因数 2、添加到“同一个质因数不会重复出现”的情况中去。（从"1<<10"种情况中找）
class Solution {
private:
    int a[10] = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 };
    int MOD = (int)1e9 + 7;
    int num_max = 30;
public:
    int numberOfGoodSubsets(vector<int>& nums) {
        vector<int> dic(31);
        for (int n : nums)   ++dic[n];
        vector<int> f(1 << 10);
        f[0] = 1;
        for (int _ = 0; _ < dic[1]; ++_)
            f[0] = f[0] * 2 % MOD;

        for (int i = 2; i <= num_max; ++i) {
            if (!dic[i]) continue;
            int subset = 0, x = i;
            bool flag = true;
            for (int j = 0; j < 10; ++j) {
                int prime = a[j];
                if (x % (prime * prime) == 0) {
                    flag = false;
                    break;
                }
                if (x % prime == 0)  subset |= (1 << j);
            }
            if (!flag)   continue;
            for (int mask = (1 << 10) - 1; mask > 0; --mask) {
                if ((mask & subset) == subset) {    
                    f[mask] = (f[mask] + static_cast<long long>(f[mask ^ subset]) * dic[i]) % MOD;
                }
            }
        }
        int ans = 0;
        for (int mask = 1; mask < (1 << 10); ++mask)    ans = (ans + f[mask]) % MOD;

        return ans;
    }
};



//找规律 数学 错误答案 没考虑15 6这种质数积的数
//求子集内都是质数的集合数
//先求质数总数cnt，求取[1,cnt]的组合数-1(单个1不是好子集)，即C(1,cnt)+...C(cnt,cnt)-1。
//C(x,y)为=y!/x!=(x+1)*...*y
//class Solution {
//    int Cselect(int x, int y) {
//        int rst = 1;
//        for (int i = y; i > x; i--) {
//            rst *= i;
//        }
//        for (int i = 1; i <= y - x; i++) {
//            rst /= i;
//        }
//        return rst;
//    }
//public:
//    int numberOfGoodSubsets(vector<int>& nums) {
//        int cnt = 0;
//        int rst = 0;
//        unordered_set<int> prime = { 1,2,3,5,7,11,13,17,19,23,29 };
//        int v[31]{};
//        for (int& e : nums) {
//            if (prime.count(e)) {
//                if (!v[e])
//                    cnt++;
//                v[e]++;
//            }
//            if (e == 1)rst--;
//        }
//        for (int i = 1; i <= cnt; i++) {
//            rst += Cselect(i, cnt);
//        }
//        for (int i = 1; i < 31; i++) {
//            if (v[i] > 1)
//                rst *= v[i];
//        }
//        return rst;
//    }
//};