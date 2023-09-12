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
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
class Solution {
public:
    bool isReachableAtTime(int sx, int sy, int fx, int fy, int t) {
        return (!(sx==fx&&sy==fy&&t==1))&&max(abs(fx-sx),abs(fy-sy))<=t;
    }
};