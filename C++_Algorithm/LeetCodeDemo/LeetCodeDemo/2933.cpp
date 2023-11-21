using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <numeric>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
typedef pair<int, bool> pib;
typedef pair<int, int> pii;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<ll, int> pli;
#define MAXN (int)(1e5+1)
#define MAXM (int)(1e5+1)
#define MOD (int)(1e9+7)
//模拟 哈希
class Solution {
    bool datedelta(string x, string y) {
        return abs(stoi(x.substr(0, 2)) - stoi(y.substr(0, 2)) + (stoi(x.substr(2, 2)) - stoi(y.substr(2, 2))) / 60.0) < 1;
    }
public:
    vector<string> findHighAccessEmployees(vector<vector<string>>& a) {
        sort(a.begin(), a.end(), [](vector<string>& x, vector<string>& y) {return x[0]<y[0]||x[0]==y[0]&&x[1]<y[1]; });
        vector<string> rst;
        int n = a.size();
        for (int i = 0; i < n-2;i++) {
            if (!rst.empty()&&rst.back()==a[i][0]||a[i][0] != a[i + 2][0])
                continue;
            if (datedelta(a[i][1], a[i + 2][1]))
                rst.push_back(a[i][0]);
        }
        return rst;
    }
};