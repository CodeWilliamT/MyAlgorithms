using namespace std;
#include <iostream>
#include <vector>
//巧思
//空间优化，位运算
//把每个单词每个字母都遍历一遍,hash[x][y],记录第x个字符串，字符y+'a'是否出现过。
//遍历每个字符串与另一个字符串,遍历a~z：如果有都出现的那么说明不可行。
class Solution {
public:
    int maxProduct(vector<string>& words) {
        int hash[1001]{};
        int len;
        int n = words.size();
        for (int i = 0; i < n; i++)
        {
            len = words[i].size();
            for (int j = 0; j < len; j++)
            {
                hash[i]|= 1<<(words[i][j] - 'a');
            }
        }
        int ans = 0;
        int len1, len2;
        for (int i = 0; i < n; i++)
        {
            len1 = words[i].size();
            for (int j = 1; j < n; j++)
            {
                len2 = words[j].size();
                if (!(hash[i] & hash[j]))
                {
                    ans = max(ans, len1 * len2);
                }
            }
        }
        return ans;
    }
};
//巧思
//把每个单词每个字母都遍历一遍,hash[x][y],记录第x个字符串，字符y+'a'是否出现过。
//遍历每个字符串与另一个字符串,遍历a~z：如果有都出现的那么说明不可行。
//class Solution {
//public:
//    int maxProduct(vector<string>& words) {
//        bool hash[1001][26]{};
//        int len;
//        int n = words.size();
//        for (int i=0;i<n;i++)
//        {
//            len = words[i].size();
//            for (int j = 0; j < len; j++)
//            {
//                hash[i][words[i][j]-'a']=1;
//            }
//        }
//        int ans = 0;
//        int len1, len2;
//        bool flag = 0;
//        for (int i = 0; i < n; i++)
//        {
//            len1 = words[i].size();
//            for (int j = 1; j < n; j++)
//            {
//                len2= words[j].size(); 
//                flag = 0;
//                for (int k = 0; k < 26; k++)
//                {
//                    if (hash[i][k]&& hash[j][k])
//                    {
//                        flag = 1;
//                        break;
//                    }
//                }
//                if (!flag)ans = max(ans, len1 * len2);
//            }
//        }
//        return ans;
//    }
//};