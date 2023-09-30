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
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;

class Solution {
public:
    string maximumOddBinaryNumber(string s) {
        int cnt = -1;
        for (char& c : s) {
            if (c == '1')
                cnt++;
        }
        int n = s.size();
        string rst(n, '0');
        rst[n - 1] = '1';
        for (int i = 0; i < cnt; i++) {
            rst[i] = '1';
        }
        return rst;

    }
};