using namespace std;
#include <vector>
//巧思
//求finalSum拆分成最多数目的偶整数和
//finalSum为奇数，返回空
//累加偶数，直到和刚好大于finalSum,去掉差值偶数
class Solution {
public:
    vector<long long> maximumEvenSplit(long long finalSum) {
        if (finalSum % 2)return {};
        vector<long long> rst;
        long long num = 2;
        long long sum = 0;
        while (sum <= finalSum) {
            rst.push_back(num);
            sum += num;
            num += 2;
        }
        rst.erase(find(rst.begin(),rst.end(),sum - finalSum));
        return rst;
    }
};