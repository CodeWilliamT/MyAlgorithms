using namespace std;
#include <iostream>
//麻烦题 细致条件分析
//算出距离1970年1月4日有多少天%7。
//1970年1月4日是周日。
//处理 判定有多少闰年；累加每月日子；判定当前年是闰年；
//闰年判定条件 不能被100整除则必须能被4整除 能被100整除的必须能被400整除
//
class Solution {
public:
    string dayOfTheWeek(int day, int month, int year) {
        string dayname[7] = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        int monthdays[12] = { 31,28,31,30,31,30,31,31,30,31,30,31 };
        int ans = 365 * (year - 1970) + (year - 1969) / 4 + day;
        for (int i = 0; i < month - 1; i++) {
            ans += monthdays[i];
            if (i == 1 && (year % 4 == 0 && year % 100 != 0 || year % 400 == 0 && year % 100 == 0))ans++;
        }
        return dayname[(ans + 3) % 7];
    }
};