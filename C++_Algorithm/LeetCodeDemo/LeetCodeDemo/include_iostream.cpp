using namespace std;
#include <iostream>
class Solution {
public:
    void include_iostream() {
        //测试用变量
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
    }
};