using namespace std;
#include <iostream>
//测试用变量
class io{
    void main() {
        double x = 2.0;
        int a = 1, b = 2, c = 3;
        string s = "";//声明string
        min(a, b);//求a,b的最小值
        max(a, b);//求a,b的最大值
        swap(a, b);//交换两个元素
        a = ceil(x);//功 能： double的上取整，返回大于或者等于指定表达式的最小整数 头文件：math.h
        b = floor(x);//功 能: 向下取整，返回x的下一个double类型最小值。
        x = log(a);//以e为低的log
        x = log(a) / log(b);//求以m为底，求logm(n)的值
        a = rand();//获取一个随机整数0~2^15-1 取几之内的就取模
        //三角函数
        //arcsin用asin,asin返回值范围[-pi/2,pi/2];
        //arccos用acos,acos返回值范围[0,pi];
        //arctan用atan,atan返回值范围(-pi/2,pi/2);
        double pi = acos(-1);//求pi
        a = a * 180 / acos(-1);//a从弧度转为角度(建议保留起码三位小数)
        isalpha('1');//判定字符是否是大小写字母,不是则返回0
        //格式转化
        //符号转义
        //%% 印出百分比符号，不转换。
        //%c 字符输出到缓冲区，不转换。
        //%d 整数转成十进位。
        //%f 倍精确度数字转成浮点数。
        //%o 整数转成八进位。
        //%s 字符串输出到缓冲区，不转换。
        //%x 整数转成小写十六进位。
        //%X 整数转成大写十六进位。
        char outs[10];
        //sprintf 格式化输出,输出数据为char*,缺点没限制写入会溢出报错，可用snprintf，返回以format为格式argument为内容组成的结果被写入string的字节数，结束字符‘\0’不计入内
        sprintf(outs, "%08.2f", 0.95 * 100);//格式化输出，这里f转char*输出，如果转化出的数长度不足8位保留前导0使长度8个字符;
        sprintf(outs, "%-08.2f", 0.95 * 100);//格式化输出，这里f转char*输出，如果转化出的数长度不足8位保留后缀0使长度8个字符;
        sprintf(outs, "%.2f%%", 0.95 * 100); // 格式化为百分比,.2保留小数位2位；
        //snprintf 格式化输出,输出数据为char*,最长写到限定的长度，可用snprintf，返回以format为格式argument为内容组成的结果被写入string的字节数，结束字符‘\0’不计入内
        snprintf(outs, 10, "%08.2f", 0.95 * 100);//格式化输出，这里f转char*输出，如果转化出的数长度不足8位保留前导0使长度8个字符;
        snprintf(outs, 10, "%-08.2f", 0.95 * 100);//格式化输出，这里f转char*输出，如果转化出的数长度不足8位保留后缀0使长度8个字符;
        snprintf(outs, 10, "%.2f%%", 0.95 * 100); // 格式化为百分比,.2保留小数位2位；

        //打表
	    string name, url;
	    //将标准输入流重定向到 in.txt 文件
	    freopen("in.txt", "r", stdin);
	    cin >> name >> url;
	    //将标准输出重定向到 out.txt文件
	    freopen("out.txt", "w", stdout);
	    cout << name << "\n" << url;
        /*string a = "())()))()(()(((())(()()))))((((()())(())";
        string b = "1011101100010001001011000000110010100101";
        s.canBeValid(a, b
        );*/
    }
};


using namespace std;
#include <string>
//测试用变量
class str {
    void main() {
        string s = "";
        int a = 99;
        s = to_string(a);
        a = stoi(s);//string转int,注意没有前导0
        a = atoi(s.c_str());//char[]指针换算
        a = '9' - '0';
    }
};
using namespace std;
#include <vector>
#include <algorithm>
//测试用变量
class algo {
    void main() {
        int num = 3;
        vector<int> nums = { 3,2,1 };
        vector<int> nums1 = { 1,3,9 };
        vector<int> nums2 = { 2,4,6 };
        int nums3[3] = { 2,3,4 };
        int nums4[3] = { 7,8,9 };
        //void sort(iterator first, iterator last, Compare comp );//快速排序O(nlog2(n))，集合首指针或迭代器，集合尾指针或迭代器，比较函数bool类型传入比较用的2个元素，返回值为true则第一个元素在前。
        sort(nums.begin(), nums.end(), [](int& x, int& y) {return x > y; });//排序成递减数组
        merge(nums1.begin(), nums1.end(), nums2.begin(), nums2.end(), [](int& x, int& y) {return x < y; });//<algorithm>合并有序数组,类似于sort。
        merge(nums3, nums3 + sizeof(nums3), nums4, nums4 + sizeof(nums4), [](int& x, int& y) {return x < y; });//<algorithm>合并有序数组,类似于sort。
        int k = 4;
        nth_element(nums.begin(), nums.begin() + k, nums.end());//将第k小的元素放到第k+1小(k)的位置。
        nth_element(nums.begin(), nums.begin() + k, nums.end(), [](int a, int b) {return a > b; });//将第k大的元素放到第k+1大(k)的位置。
        int index = max_element(nums.begin(), nums.end()) - nums.begin();//找到最大的元素的索引
        //两分查找
        int num = 3;
        vector<int> nums = { 3,2,1 };
        vector<int>::iterator it;
        int* address;
        int idx;
        it = lower_bound(nums.begin(), nums.end(), num);//两分查找，在递增数组中，从数组的begin位置到end-1位置二分查找第一个大于或等于num的数字，找到返回该数字的地址，不存在则返回end。通过返回的地址减去起始地址begin,得到找到数字在数组中的下标。
        idx = it - nums.begin();
        address = upper_bound(nums3, nums3 + sizeof(nums3), num);//两分查找，在递增数组中，从数组的begin位置到end-1位置二分查找第一个大于num的数字，找到返回该数字的地址，不存在则返回end。通过返回的地址减去起始地址begin,得到找到数字在数组中的下标。
        idx = address - nums3;
        //最大公约数，最小公倍数
        int a = 1, b = 2;
        //__gcd(a,b);//求a,b的最大公约数,不过VS不能识别,GNU的私货,在Linux下的编译器可用
        //__lcm(a,b);//求a,b的最小公倍数,不过VS不能识别,GNU的私货,在Linux下的编译器可用

    }
};
using namespace std;
#include <string>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
//数据集合
//哈希的erase(a),a可以是key，也可是迭代器。
//当使用decltype(var)的形式时，decltype会直接返回变量的类型
//自定义set pq的比较 小的在前/队头
class dt {
    void main() {
        typedef pair<int, int> pii;
        auto cmp = [&](pii a, pii b) {return a.first / a.second < b.first / b.second; };
        set <pii, decltype(cmp)> st(cmp);
        priority_queue<pii, vector<pii>, decltype(cmp)> pq(cmp);

        unordered_map<int, int> mp;
        //pair集合的遍历,需要VC16+可用
        for (auto& [x, y]: mp);//x为first,y为second
    }
};
using namespace std;
#include<functional>
//函数内方法声明
function<bool(int, int)> check = [&](int p, int q) {
    return p < q;
};


using namespace std;
#include<numeric>
//函数内方法声明
class numeric {
    int main() {
        int myints[] = { 10,20,30,40,50 };
        int sum = accumulate(myints, myints + 5, 0);
    }
};