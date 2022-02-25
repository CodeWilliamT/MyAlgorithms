using namespace std;
#include <string>
//模拟 复杂条件实现
//i*i=-1;
//(a+bi)*(c+di)=ac+bci+adi+bd(i*i)=ac+(bc+ad)i+-bd=ac-bd+(bc+ad)i;
class Solution {
public:
    string complexNumberMultiply(string num1, string num2) {
        string sa, sb, sc, sd;
        bool flag = 0;
        for (char& e : num1) {
            if (e == 'i')break;
            if (e == '+') {
                flag = 1;
                continue;
            }
            if (!flag)
                sa += e;
            else
                sb += e;
        }
        flag = 0;
        for (char& e : num2) {
            if (e == 'i')break;
            if (e == '+') {
                flag = 1;
                continue;
            }
            if (!flag)
                sc += e;
            else
                sd += e;
        }
        int a = stoi(sa), b = stoi(sb), c = stoi(sc), d = stoi(sd);
        return to_string(a * c - b * d) + "+" + to_string(b * c + a * d) + "i";
    }
};