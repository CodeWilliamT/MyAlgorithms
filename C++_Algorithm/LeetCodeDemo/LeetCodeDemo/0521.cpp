using namespace std;
#include <iostream>
//巧思
//分析下所求就知道,不同则为其较长的，相同则没。
class Solution {
public:
    int findLUSlength(string a, string b) {
        return a==b?-1 : max(a.size(), b.size());
    }
};