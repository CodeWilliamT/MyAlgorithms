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
class Solution {
public:
    vector<long long> sumOfThree(long long num) {
        vector<long long> rst;
        if(num % 3==0)
            rst={ num / 3 - 1, num / 3, num / 3 + 1 };
        return rst;
    }
};