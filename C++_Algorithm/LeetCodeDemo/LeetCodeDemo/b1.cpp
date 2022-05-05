using namespace std;
#include <vector>
#include <unordered_set>
//哈希
class Solution {
public:
    int countPrefixes(vector<string>& words, string s) {
        unordered_set<string> st;
        string tmp="";
        for (char& c : s) {
            tmp.push_back(c);
            st.insert(tmp);
        }
        int rst = 0;
        for (auto& e : words) {
            if (st.count(e)) {
                rst++;
            }
        }
        return rst;
    }
};