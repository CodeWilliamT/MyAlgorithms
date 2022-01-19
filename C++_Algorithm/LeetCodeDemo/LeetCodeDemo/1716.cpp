//巧思计算
//等差数列
class Solution {
public:
    int totalMoney(int n) {
        int weeks = n / 7;
        int firstWeek = 4 * 7;
        int lastWeek = firstWeek+(weeks-1)*7;
        int weeksAdd = (firstWeek + lastWeek) * weeks / 2;
        int restDays = n % 7;
        int firstDay = weeks + 1;
        int lastDay = weeks + restDays;
        int daysAdd = (firstDay + lastDay) * restDays / 2;
        return weeksAdd+ daysAdd;
    }
};
//简单模拟
//class Solution {
//public:
//    int totalMoney(int n) {
//        int rst=0,tmp=1;
//        for (int i = 0; i < n; i++) {
//            rst += tmp;
//            if (i % 7 == 6)tmp -= 6;
//            tmp++;
//        }
//        return rst;
//    }
//};