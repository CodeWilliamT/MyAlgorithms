using namespace std;
#include <iostream>
//朴素实现
//注意闰年
class Solution {
public:
    int dayOfYear(string date) {

		bool flag1 = false, flag2 = false;
		string year,mon, day;
		int y,m, d;
		int monthToDay[13] = { 0,0,31,59,90,120,151,181,212,243,273,304,334};
		for (int i = 0; i < date.size(); i++)
		{
			if (!flag1 && !flag2) {
				year+= date[i];
			}
			if (flag1) {
				mon += date[i];
			}
			if (flag2) {
				day += date[i];
			}
			if (date[i] == '-')
			{
				if (flag1)
				{
					flag2 = 1;
				}
				flag1 = !flag1;
			}
		}
		y = stoi(year);
		m = stoi(mon);
		d= stoi(day);
		d +=((y % 100 == 0 && y % 400 == 0 || y % 100 != 0 && y % 4 == 0)&&m>2) + monthToDay[m];
		return d;
    }
};