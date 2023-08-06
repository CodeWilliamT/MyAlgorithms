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
class Solution {
public:
    string finalString(string s) {
        string rst;
        for (char& c : s) {
            if (c == 'i') {
                reverse(rst.begin(), rst.end());
                continue;
            }
            rst += c;
        }
        return rst;
    }
};