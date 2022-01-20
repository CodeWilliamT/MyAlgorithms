using namespace std;
#include <string>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
class Solution {
public:
    void include_collection() {
        //当使用decltype(var)的形式时，decltype会直接返回变量的类型
        //自定义set pq的比较
        unordered_map<int, int> mp; 
        auto cmp = [&](pair<int, int> a, pair<int, int> b) {return a.first / a.second < b.first / b.second; };
        set < pair<int, int>, decltype(cmp)> st(cmp);
        priority_queue<pair<int, int>, vector<pair<int, int>>, decltype(cmp)> pq(cmp);

        unordered_map<int, int> mp;
        //pair集合的遍历,不过VS不能识别,GNU的私货,在Linux下的编译器可用
        //for (auto& [x, y]: mp);//x为first,y为second
    }
}；