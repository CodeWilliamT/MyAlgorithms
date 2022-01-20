using namespace std;
#include <string>
class Solution {
public:
    void include_string() {
        //测试用变量
        string s = "";
        int a = 99;
        s = to_string(a);
        a = stoi(s);//string转int,注意没有前导0
        a = atoi('9');//没啥用,可以直接用下面那个方式算
        a = '9' - '0';
    }
};