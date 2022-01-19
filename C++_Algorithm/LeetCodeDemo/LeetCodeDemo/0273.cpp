using namespace std;
#include <iostream>
#include <unordered_map>
//麻烦题 复杂条件实现
//码个单词表0~20，，就当复习英语了，然后分分类处理下完事
class Solution {
public:
    string numberToWords(int num) {
        unordered_map<int, string> mp = { {0,"Zero"},{1,"One"},{2,"Two"},{3,"Three"},{4,"Four"},{5,"Five"},{6,"Six"},{7,"Seven"},{8,"Eight"},{9,"Nine"},
            {10,"Ten" },{11,"Eleven"},{12,"Twelve"},{13,"Thirteen"},{14,"Fourteen"},{15,"Fifteen"},{16,"Sixteen"},{17,"Seventeen"},{18,"Eighteen"},{19,"Nineteen"},
            {20,"Twenty"},{30,"Thirty"},{40,"Forty"},{50,"Fifty"},{60,"Sixty"},{70,"Seventy"},{80,"Eighty"},{90,"Ninety"},
            {100,"Hundred"},{1000,"Thousand"},{1000000,"Million"},{1000000000,"Billion"}};
        if (num < 21)
        {
            return mp[num];
        }
        string ans;
        int thousandtime = 1000000000;
        int tmpthousand,tmpten,tmpdig;
        for (int i = 0; i < 4; i++, thousandtime/=1000)
        {
            tmpthousand = (num / thousandtime)%1000;
            if (tmpthousand < 1)continue;
            if (tmpthousand > 99)
            {
                ans += mp[tmpthousand / 100] + " " + mp[100]+ " ";
            }
            tmpten = (tmpthousand / 10) % 10;
            if (tmpten > 1)
            {
                ans += mp[tmpten*10] + " ";
                tmpdig = tmpthousand % 10;
                if(tmpdig >0)ans += mp[tmpdig] + " ";
            }
            else
            {
                if (tmpthousand % 100 > 0)ans += mp[tmpthousand%100] + " ";
            }
            if(thousandtime!=1)ans +=mp[thousandtime]+ " ";
        }
        ans.pop_back();
        return ans;
    }
};