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
//找规律 巧思
class Solution {
public:
    int minimumOperations(string num) {
        int n=num.size();
        string s[] = {"00","25","50","75"};
        int match[4]{};
        
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < 4; j++) {
                if (num[n - 1 - i] == s[j][1-match[j]]) {
                    if (match[j])return i-1;
                    match[j]++;
                }
            }
        }
        if (match[0])return n-1;
        return n;
    }
};