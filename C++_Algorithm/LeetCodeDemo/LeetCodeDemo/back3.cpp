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
//找规律
//求finalSum拆分成最多数目的偶整数和
//finalSum%2为1则为奇数，返回空
//若一个数大于2的4次，小于2的5次则，返回4个数
class Solution {
public:
    vector<long long> maximumEvenSplit(long long finalSum) {
        if (finalSum % 2)return {};
        vector<long long> rst;
        int cnt = log2(finalSum);
        long long tmp=0;
        long long digit;
        for (int i = 0; i <= cnt; i++) {
            digit = 1 << i;
            tmp+=finalSum & digit;
            if (tmp) {
                rst.push_back(tmp);
                finalSum -= tmp;
                tmp = 0;
            }
        }
        return rst;
    }
};