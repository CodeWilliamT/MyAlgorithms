using namespace std;
#include <iostream>
#include <unordered_set>
//遍历s，记录从每一位开始的最大长度，比较刷新答案
//当遇到重复的说明达到当前位最大长度，则用长度进行比较刷新答案。
class Solution {
public:
    int lengthOfLongestSubstring(string s) {
        int ans=0;
        unordered_set<int> set;
        int count=0;
        int n = s.size();
        for (int i=0;i<n&&i+count<n;i++)//count只会-1更小的一定刷新不了答案
        {
            while (!set.count(s[i + count]))
            { 
                set.insert(s[i + count]); 
                count++;
                if (i + count >= n)
                {
                    break;
                }
            }
            ans = max(ans, count);
            count--;
            set.erase(s[i]);
        }
        return ans;
    }
};

//int main()
//{
//    Solution s;
//    int n;
//    while (cin >> n)
//        cout << s.hammingWeight(n) << endl;
//    return 0;
//}