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
//巧思计算
class Solution {
public:
    int totalMoney(int n) {
        int rst = (n / 7) * 4*7 + (n / 7 * 7)/2*n/7+(n%7)*(n/7+(n%7+1+1)/2);
        return rst;
    }
};
//简单模拟
//class Solution {
//public:
//    int totalMoney(int n) {
//        int rst=0,tmp=1;
//        for (int i = 0; i < n; i++) {
//            rst += tmp;
//            if (i % 7 == 6)tmp -= 6;
//            tmp++;
//        }
//        return rst;
//    }
//};